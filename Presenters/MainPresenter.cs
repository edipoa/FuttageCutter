using Cuttage.Models;
using Cuttage.Services;
using Cuttage.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Presenters
{
    public class MainPresenter
    {
        private readonly IMainView _view;
        private readonly IVideoProcessingService _videoService;

        public MainPresenter(IMainView view, IVideoProcessingService videoService)
        {
            _view = view ?? throw new ArgumentNullException(nameof(view));
            _videoService = videoService ?? throw new ArgumentNullException(nameof(videoService));
        }

        public void OnViewLoaded()
        {
            UpdateDurationDisplay();

            if (!_videoService.IsFFmpegAvailable())
            {
                _view.ShowMessage(
                    "FFmpeg não foi encontrado!\n\n" +
                    "Para usar este aplicativo, você precisa:\n" +
                    "1. Baixar o FFmpeg de https://ffmpeg.org/download.html\n" +
                    "2. Colocar o ffmpeg.exe na pasta da aplicação\n" +
                    "3. OU adicionar o FFmpeg ao PATH do sistema",
                    "FFmpeg Necessário",
                    true);
            }
        }

        public void OnInputFileSelected()
        {
            if (!string.IsNullOrEmpty(_view.InputFilePath))
            {
                var inputFile = _view.InputFilePath;
                var directory = Path.GetDirectoryName(inputFile);
                var fileName = Path.GetFileNameWithoutExtension(inputFile);
                var extension = Path.GetExtension(inputFile);

                var suggestedOutput = Path.Combine(directory, $"{fileName}_cortado{extension}");
                _view.OutputFilePath = suggestedOutput;

                _view.UpdateProgress($"Arquivo selecionado: {Path.GetFileName(inputFile)}");
            }
        }

        public void OnTimeChanged()
        {
            UpdateDurationDisplay();
            ValidateTimeRange();
        }

        public async Task ProcessVideoAsync()
        {
            try
            {
                var request = CreateVideoProcessingRequest();

                if (!ValidateRequest(request))
                    return;

                _view.IsProcessing = true;

                var progress = new Progress<string>(_view.UpdateProgress);
                var result = await _videoService.CutVideoAsync(request, progress);

                if (result.Success)
                {
                    _view.ShowMessage(result.Message, "Sucesso");
                    _view.UpdateProgress($"Vídeo salvo em: {Path.GetFileName(request.OutputFilePath)}");
                }
                else
                {
                    var errorMessage = result.Message;
                    if (!string.IsNullOrEmpty(result.ErrorDetails))
                    {
                        errorMessage += $"\n\nDetalhes técnicos:\n{result.ErrorDetails}";
                    }

                    _view.ShowMessage(errorMessage, "Erro no Processamento", true);
                    _view.UpdateProgress("Erro no processamento");
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage(
                    $"Erro inesperado: {ex.Message}",
                    "Erro",
                    true);
                _view.UpdateProgress("Erro inesperado");
            }
            finally
            {
                _view.IsProcessing = false;
            }
        }

        private VideoProcessingRequest CreateVideoProcessingRequest()
        {
            return new VideoProcessingRequest
            {
                InputFilePath = _view.InputFilePath,
                OutputFilePath = _view.OutputFilePath,
                StartTime = _view.StartTime,
                EndTime = _view.EndTime
            };
        }

        private bool ValidateRequest(VideoProcessingRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.InputFilePath))
            {
                _view.ShowMessage("Por favor, selecione um arquivo de vídeo de entrada.", "Validação", true);
                return false;
            }

            if (!File.Exists(request.InputFilePath))
            {
                _view.ShowMessage("O arquivo de entrada não existe.", "Validação", true);
                return false;
            }

            if (string.IsNullOrWhiteSpace(request.OutputFilePath))
            {
                _view.ShowMessage("Por favor, especifique um arquivo de saída.", "Validação", true);
                return false;
            }

            if (request.StartTime >= request.EndTime)
            {
                _view.ShowMessage("O tempo inicial deve ser menor que o tempo final.", "Validação", true);
                return false;
            }

            if (request.Duration.TotalSeconds < 1)
            {
                _view.ShowMessage("A duração do corte deve ser de pelo menos 1 segundo.", "Validação", true);
                return false;
            }

            try
            {
                var outputDir = Path.GetDirectoryName(request.OutputFilePath);
                if (!string.IsNullOrEmpty(outputDir) && !Directory.Exists(outputDir))
                {
                    var result = System.Windows.Forms.MessageBox.Show(
                        $"O diretório '{outputDir}' não existe.\nDeseja criar o diretório?",
                        "Diretório não encontrado",
                        System.Windows.Forms.MessageBoxButtons.YesNo,
                        System.Windows.Forms.MessageBoxIcon.Question);

                    if (result == System.Windows.Forms.DialogResult.No)
                        return false;
                }
            }
            catch (Exception ex)
            {
                _view.ShowMessage($"Caminho de saída inválido: {ex.Message}", "Validação", true);
                return false;
            }

            return true;
        }

        private void ValidateTimeRange()
        {
            if (_view.StartTime >= _view.EndTime)
            {
                _view.EndTime = _view.StartTime.Add(TimeSpan.FromMinutes(1));
            }
        }

        private void UpdateDurationDisplay()
        {
            var duration = _view.EndTime - _view.StartTime;
            if (duration.TotalSeconds > 0)
            {
                var durationText = duration.ToString(@"hh\:mm\:ss");
                _view.UpdateProgress($"Duração do corte: {durationText}");

                if (_view is MainForm mainForm)
                {
                    var durationLabel = mainForm.Controls.Find("lblDuration", true)[0] as System.Windows.Forms.Label;
                    if (durationLabel != null)
                    {
                        durationLabel.Text = $"Duração: {durationText}";
                    }
                }
            }
        }
    }
}
