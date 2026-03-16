using Cuttage.Models;
using Cuttage.Services;
using Cuttage.Views;

namespace Cuttage.Presenters;

public sealed class MainPresenter
{
    private readonly IMainView _view;
    private readonly IVideoProcessingService _videoService;
    private CancellationTokenSource? _cts;

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
                "1. Colocar o ffmpeg.exe na pasta 'ffmpeg/' da aplicação\n" +
                "2. OU adicionar o FFmpeg ao PATH do sistema",
                "FFmpeg Necessário",
                true);
        }
    }

    public void OnInputFileSelected()
    {
        if (string.IsNullOrEmpty(_view.InputFilePath)) return;

        var inputFile = _view.InputFilePath;
        var directory = Path.GetDirectoryName(inputFile) ?? string.Empty;
        var fileName = Path.GetFileNameWithoutExtension(inputFile);
        var extension = Path.GetExtension(inputFile);

        _view.OutputFilePath = Path.Combine(directory, $"{fileName}_cortado{extension}");
        _view.StatusText = $"Arquivo selecionado: {Path.GetFileName(inputFile)}";
    }

    public void OnTimeChanged() => UpdateDurationDisplay();

    public async Task ProcessVideoAsync()
    {
        var request = new VideoProcessingRequest(
            _view.InputFilePath,
            _view.OutputFilePath,
            _view.StartTime,
            _view.EndTime);

        if (!ValidateRequest(request)) return;

        _cts = new CancellationTokenSource();

        try
        {
            _view.IsProcessing = true;

            var progress = new Progress<string>(msg => _view.StatusText = msg);
            var result = await _videoService.CutVideoAsync(request, progress, _cts.Token);

            if (result.Success)
            {
                _view.ShowMessage(result.Message, "Sucesso");
                _view.StatusText = $"Salvo em: {Path.GetFileName(request.OutputFilePath)}";
            }
            else
            {
                var errorMessage = string.IsNullOrEmpty(result.ErrorDetails)
                    ? result.Message
                    : $"{result.Message}\n\nDetalhes:\n{result.ErrorDetails}";

                _view.ShowMessage(errorMessage, "Erro no Processamento", true);
                _view.StatusText = "Erro no processamento";
            }
        }
        catch (Exception ex)
        {
            _view.ShowMessage($"Erro inesperado: {ex.Message}", "Erro", true);
            _view.StatusText = "Erro inesperado";
        }
        finally
        {
            _cts?.Dispose();
            _cts = null;
            _view.IsProcessing = false;
        }
    }

    public void CancelProcessing()
    {
        _cts?.Cancel();
        _view.StatusText = "Cancelando...";
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

        return true;
    }

    private void UpdateDurationDisplay()
    {
        var duration = _view.EndTime - _view.StartTime;
        _view.DurationText = duration.TotalSeconds > 0
            ? $"Duração: {duration:hh\\:mm\\:ss}"
            : "Duração: --:--:--";
    }
}
