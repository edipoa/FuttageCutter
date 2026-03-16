using Cuttage.Presenters;
using Cuttage.Services;
using System.Drawing.Drawing2D;

namespace Cuttage.Views;

public partial class MainForm : Form, IMainView
{
    private MainPresenter _presenter = null!;

    // Dark theme colors
    private static readonly Color BgColor      = Color.FromArgb(15, 15, 25);
    private static readonly Color PanelColor   = Color.FromArgb(25, 25, 42);
    private static readonly Color InputColor   = Color.FromArgb(35, 35, 58);
    private static readonly Color TextColor    = Color.FromArgb(220, 220, 235);
    private static readonly Color MutedColor   = Color.FromArgb(130, 130, 160);
    private static readonly Color AccentColor  = Color.FromArgb(99, 130, 210);
    private static readonly Color SuccessColor = Color.FromArgb(46, 160, 80);
    private static readonly Color DangerColor  = Color.FromArgb(200, 60, 60);

    public MainForm()
    {
        InitializeComponent();
        ApplyTheme();
        SetupDragDrop();
        SetupFormIcon();
        InitializePresenter();
    }

    private void InitializePresenter()
    {
        _presenter = new MainPresenter(this, new FFmpegVideoProcessingService());
    }

    // ── IMainView ──────────────────────────────────────────────────────────────

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
            btnProcess.Enabled      = !value;
            btnSelectInput.Enabled  = !value;
            btnSelectOutput.Enabled = !value;
            dtpStartTime.Enabled    = !value;
            dtpEndTime.Enabled      = !value;
            btnCancel.Visible       = value;
            progressBar.Visible     = value;

            if (value)
                progressBar.Style = ProgressBarStyle.Marquee;
            else
            {
                progressBar.Style = ProgressBarStyle.Blocks;
                progressBar.Value = 0;
            }
        }
    }

    public string StatusText
    {
        set
        {
            if (InvokeRequired)
                Invoke(() => lblStatus.Text = value);
            else
                lblStatus.Text = value;
        }
    }

    public string DurationText
    {
        set => lblDuration.Text = value;
    }

    public void ShowMessage(string message, string title, bool isError = false)
    {
        var icon = isError ? MessageBoxIcon.Error : MessageBoxIcon.Information;
        MessageBox.Show(message, title, MessageBoxButtons.OK, icon);
    }

    // ── Event Handlers ─────────────────────────────────────────────────────────

    private void MainForm_Load(object sender, EventArgs e) =>
        _presenter.OnViewLoaded();

    private void btnSelectInput_Click(object sender, EventArgs e)
    {
        using var dialog = new OpenFileDialog
        {
            Filter = "Arquivos de Vídeo|*.mp4;*.avi;*.mkv;*.mov;*.wmv;*.flv;*.webm|Todos os Arquivos|*.*",
            Title  = "Selecionar arquivo de vídeo"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
        {
            InputFilePath = dialog.FileName;
            _presenter.OnInputFileSelected();
        }
    }

    private void btnSelectOutput_Click(object sender, EventArgs e)
    {
        using var dialog = new SaveFileDialog
        {
            Filter     = "MP4 Video|*.mp4|AVI Video|*.avi|MKV Video|*.mkv|Todos os Arquivos|*.*",
            Title      = "Salvar vídeo cortado como",
            DefaultExt = "mp4",
            FileName   = string.IsNullOrEmpty(InputFilePath) ? string.Empty
                : $"{Path.GetFileNameWithoutExtension(InputFilePath)}_cortado"
        };

        if (dialog.ShowDialog() == DialogResult.OK)
            OutputFilePath = dialog.FileName;
    }

    private async void btnProcess_Click(object sender, EventArgs e) =>
        await _presenter.ProcessVideoAsync();

    private void btnCancel_Click(object sender, EventArgs e) =>
        _presenter.CancelProcessing();

    private void dtpStartTime_ValueChanged(object sender, EventArgs e) =>
        _presenter.OnTimeChanged();

    private void dtpEndTime_ValueChanged(object sender, EventArgs e) =>
        _presenter.OnTimeChanged();

    // ── Drag & Drop ────────────────────────────────────────────────────────────

    private void SetupDragDrop()
    {
        AllowDrop = true;
        DragEnter += OnDragEnter;
        DragDrop  += OnDragDrop;
    }

    private static void OnDragEnter(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetDataPresent(DataFormats.FileDrop) == true)
            e.Effect = DragDropEffects.Copy;
    }

    private void OnDragDrop(object? sender, DragEventArgs e)
    {
        if (e.Data?.GetData(DataFormats.FileDrop) is not string[] files || files.Length == 0) return;

        InputFilePath = files[0];
        _presenter.OnInputFileSelected();
    }

    // ── Theme & Icon ───────────────────────────────────────────────────────────

    private void ApplyTheme()
    {
        BackColor = BgColor;
        ForeColor = TextColor;

        // Panels
        foreach (var panel in new[] { panelFiles, panelTiming })
        {
            panel.BackColor = PanelColor;
            foreach (Control c in panel.Controls)
            {
                c.BackColor = c is TextBox or DateTimePicker ? InputColor : PanelColor;
                c.ForeColor = c is Label lbl && lbl.Tag is "muted" ? MutedColor : TextColor;
                if (c is TextBox tb) tb.BorderStyle = BorderStyle.FixedSingle;
            }
        }

        // Section header labels
        foreach (var lbl in new[] { lblSectionFiles, lblSectionTiming })
        {
            lbl.BackColor = BgColor;
            lbl.ForeColor = AccentColor;
        }

        // Status and duration
        lblStatus.BackColor   = BgColor;
        lblStatus.ForeColor   = MutedColor;
        lblDuration.BackColor = PanelColor;
        lblDuration.ForeColor = MutedColor;

        // Buttons
        StyleButton(btnSelectInput,  AccentColor,  Color.White);
        StyleButton(btnSelectOutput, AccentColor,  Color.White);
        StyleButton(btnProcess,      SuccessColor, Color.White);
        StyleButton(btnCancel,       DangerColor,  Color.White);

        // Progress bar
        progressBar.BackColor = PanelColor;
        progressBar.ForeColor = AccentColor;
    }

    private static void StyleButton(Button btn, Color back, Color fore)
    {
        btn.FlatStyle       = FlatStyle.Flat;
        btn.BackColor       = back;
        btn.ForeColor       = fore;
        btn.FlatAppearance.BorderSize         = 0;
        btn.FlatAppearance.MouseOverBackColor = ControlPaint.Light(back, 0.15f);
        btn.FlatAppearance.MouseDownBackColor = ControlPaint.Dark(back, 0.1f);
        btn.Cursor = Cursors.Hand;
    }

    private void SetupFormIcon()
    {
        try
        {
            using var ms = new MemoryStream(Properties.Resources.video_cutter_icon);
            Icon = new Icon(ms);
        }
        catch
        {
            Icon = CreateFallbackIcon();
        }
    }

    private static Icon CreateFallbackIcon()
    {
        using var bitmap = new Bitmap(32, 32);
        using var g = Graphics.FromImage(bitmap);
        using var brush = new LinearGradientBrush(
            new Rectangle(0, 0, 32, 32),
            Color.CornflowerBlue, Color.RoyalBlue,
            LinearGradientMode.Vertical);

        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
        g.FillEllipse(brush, 0, 0, 32, 32);

        using var pen = new Pen(Color.White, 2);
        g.DrawLine(pen, 10, 10, 22, 22);
        g.DrawLine(pen, 10, 22, 22, 10);

        return Icon.FromHandle(bitmap.GetHicon());
    }
}
