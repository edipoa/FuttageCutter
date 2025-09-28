using Cuttage.Presenters;
using Cuttage.Services;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Views
{
    public partial class MainForm : Form, IMainView
    {
        private MainPresenter _presenter;

        public MainForm()
        {
            InitializeComponent();
            InitializePresenter();            
            SetupFormIcon();
        }

        private void SetupFormIcon()
        {
            try
            {
                byte[] iconBytes = Properties.Resources.video_cutter_icon;
                using (MemoryStream ms = new MemoryStream(iconBytes))
                {
                    this.Icon = new Icon(ms);
                }
            }
            catch (Exception ex)
            {
#if DEBUG
                MessageBox.Show("Não foi possível carregar o ícone do aplicativo. Erro: " + ex.Message);
#endif

                this.Icon = CreateSimpleIcon();
            }
        }

        private Icon CreateSimpleIcon()
        {
            var bitmap = new Bitmap(32, 32);
            using (var g = Graphics.FromImage(bitmap))
            {
                using (var brush = new LinearGradientBrush(
                    new Rectangle(0, 0, 32, 32),
                    Color.DodgerBlue,
                    Color.RoyalBlue,
                    LinearGradientMode.Vertical))
                {
                    g.FillEllipse(brush, 0, 0, 32, 32);
                }

                using (var pen = new Pen(Color.Navy, 1))
                {
                    g.DrawEllipse(pen, 0, 0, 31, 31);
                }

                using (var pen = new Pen(Color.White, 2))
                {
                    g.DrawLine(pen, 10, 10, 22, 22);
                    g.DrawLine(pen, 10, 22, 22, 10);

                    g.DrawEllipse(pen, 6, 6, 4, 4);
                    g.DrawEllipse(pen, 6, 22, 4, 4);
                }
            }

            IntPtr hIcon = bitmap.GetHicon();
            return Icon.FromHandle(hIcon);
        }

        private void InitializePresenter()
        {
            var videoService = new FFmpegVideoProcessingService();
            _presenter = new MainPresenter(this, videoService);
        }

        public string InputFilePath
        {
            get => txtInputFile.Text;
            set => txtInputFile.Text = value;
        }

        public string OutputFilePath
        {
            get => txtOutputFile.Text;
            set => txtOutputFile.Text = value;
        }

        public TimeSpan StartTime
        {
            get => dtpStartTime.Value.TimeOfDay;
            set => dtpStartTime.Value = DateTime.Today.Add(value);
        }

        public TimeSpan EndTime
        {
            get => dtpEndTime.Value.TimeOfDay;
            set => dtpEndTime.Value = DateTime.Today.Add(value);
        }

        public bool IsProcessing
        {
            set
            {
                btnProcess.Enabled = !value;
                btnSelectInput.Enabled = !value;
                btnSelectOutput.Enabled = !value;
                progressBar.Visible = value;

                if (value)
                {
                    progressBar.Style = ProgressBarStyle.Marquee;
                }
                else
                {
                    progressBar.Style = ProgressBarStyle.Blocks;
                    progressBar.Value = 0;
                }
            }
        }

        public void ShowMessage(string message, string title, bool isError = false)
        {
            var icon = isError ? MessageBoxIcon.Error : MessageBoxIcon.Information;
            MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
        }

        public void UpdateProgress(string message)
        {
            lblStatus.Text = message;
            Application.DoEvents();
        }

        private void btnSelectInput_Click(object sender, EventArgs e)
        {
            using (var dialog = new OpenFileDialog())
            {
                dialog.Filter = "Arquivos de Vídeo|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv|Todos os Arquivos|*.*";
                dialog.Title = "Selecionar arquivo de vídeo";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    InputFilePath = dialog.FileName;
                    _presenter.OnInputFileSelected();
                }
            }
        }

        private void btnSelectOutput_Click(object sender, EventArgs e)
        {
            using (var dialog = new SaveFileDialog())
            {
                dialog.Filter = "MP4 Video|*.mp4|AVI Video|*.avi|MKV Video|*.mkv|Todos os Arquivos|*.*";
                dialog.Title = "Salvar vídeo cortado como";
                dialog.DefaultExt = "mp4";

                if (!string.IsNullOrEmpty(InputFilePath))
                {
                    var inputFileName = Path.GetFileNameWithoutExtension(InputFilePath);
                    dialog.FileName = $"{inputFileName}_cortado";
                }

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    OutputFilePath = dialog.FileName;
                }
            }
        }

        private async void btnProcess_Click(object sender, EventArgs e)
        {
            await _presenter.ProcessVideoAsync();
        }

        private void dtpStartTime_ValueChanged(object sender, EventArgs e)
        {
            _presenter.OnTimeChanged();
        }

        private void dtpEndTime_ValueChanged(object sender, EventArgs e)
        {
            _presenter.OnTimeChanged();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            _presenter.OnViewLoaded();
        }
    }
}
