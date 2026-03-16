namespace Cuttage.Views;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    // Section headers
    private Label lblSectionFiles;
    private Label lblSectionTiming;

    // Panels
    private Panel panelFiles;
    private Panel panelTiming;

    // File controls
    private Label lblInputFile;
    private TextBox txtInputFile;
    private Button btnSelectInput;
    private Label lblOutputFile;
    private TextBox txtOutputFile;
    private Button btnSelectOutput;

    // Timing controls
    private Label lblStartTime;
    private DateTimePicker dtpStartTime;
    private Label lblEndTime;
    private DateTimePicker dtpEndTime;
    private Label lblDuration;

    // Actions
    private Button btnProcess;
    private Button btnCancel;

    // Status
    private ProgressBar progressBar;
    private Label lblStatus;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null)
            components.Dispose();
        base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
        components = new System.ComponentModel.Container();

        // ── Instantiate ──
        lblSectionFiles  = new Label();
        lblSectionTiming = new Label();
        panelFiles       = new Panel();
        panelTiming      = new Panel();
        lblInputFile     = new Label();
        txtInputFile     = new TextBox();
        btnSelectInput   = new Button();
        lblOutputFile    = new Label();
        txtOutputFile    = new TextBox();
        btnSelectOutput  = new Button();
        lblStartTime     = new Label();
        dtpStartTime     = new DateTimePicker();
        lblEndTime       = new Label();
        dtpEndTime       = new DateTimePicker();
        lblDuration      = new Label();
        btnProcess       = new Button();
        btnCancel        = new Button();
        progressBar      = new ProgressBar();
        lblStatus        = new Label();

        panelFiles.SuspendLayout();
        panelTiming.SuspendLayout();
        SuspendLayout();

        // ── lblSectionFiles ──
        lblSectionFiles.AutoSize  = false;
        lblSectionFiles.Location  = new Point(12, 14);
        lblSectionFiles.Size      = new Size(200, 18);
        lblSectionFiles.Text      = "ARQUIVOS";
        lblSectionFiles.Font      = new Font("Segoe UI", 8f, FontStyle.Bold);

        // ── panelFiles ──
        panelFiles.Location = new Point(12, 36);
        panelFiles.Size     = new Size(596, 118);
        panelFiles.Controls.AddRange(new Control[] {
            lblInputFile, txtInputFile, btnSelectInput,
            lblOutputFile, txtOutputFile, btnSelectOutput
        });

        // lblInputFile
        lblInputFile.AutoSize = false;
        lblInputFile.Location = new Point(12, 10);
        lblInputFile.Size     = new Size(200, 18);
        lblInputFile.Text     = "Arquivo de entrada:";
        lblInputFile.Font     = new Font("Segoe UI", 9f);
        lblInputFile.Tag      = "muted";

        // txtInputFile
        txtInputFile.Location  = new Point(12, 32);
        txtInputFile.Size      = new Size(472, 26);
        txtInputFile.ReadOnly  = true;
        txtInputFile.Font      = new Font("Segoe UI", 9f);
        txtInputFile.TabIndex  = 0;

        // btnSelectInput
        btnSelectInput.Location = new Point(494, 31);
        btnSelectInput.Size     = new Size(90, 28);
        btnSelectInput.Text     = "Selecionar";
        btnSelectInput.Font     = new Font("Segoe UI", 9f);
        btnSelectInput.TabIndex = 1;
        btnSelectInput.Click   += btnSelectInput_Click;

        // lblOutputFile
        lblOutputFile.AutoSize = false;
        lblOutputFile.Location = new Point(12, 68);
        lblOutputFile.Size     = new Size(200, 18);
        lblOutputFile.Text     = "Arquivo de saída:";
        lblOutputFile.Font     = new Font("Segoe UI", 9f);
        lblOutputFile.Tag      = "muted";

        // txtOutputFile
        txtOutputFile.Location  = new Point(12, 86);
        txtOutputFile.Size      = new Size(472, 26);
        txtOutputFile.ReadOnly  = true;
        txtOutputFile.Font      = new Font("Segoe UI", 9f);
        txtOutputFile.TabIndex  = 2;

        // btnSelectOutput
        btnSelectOutput.Location = new Point(494, 85);
        btnSelectOutput.Size     = new Size(90, 28);
        btnSelectOutput.Text     = "Salvar como";
        btnSelectOutput.Font     = new Font("Segoe UI", 9f);
        btnSelectOutput.TabIndex = 3;
        btnSelectOutput.Click   += btnSelectOutput_Click;

        // ── lblSectionTiming ──
        lblSectionTiming.AutoSize  = false;
        lblSectionTiming.Location  = new Point(12, 168);
        lblSectionTiming.Size      = new Size(200, 18);
        lblSectionTiming.Text      = "TEMPO DE CORTE";
        lblSectionTiming.Font      = new Font("Segoe UI", 8f, FontStyle.Bold);

        // ── panelTiming ──
        panelTiming.Location = new Point(12, 190);
        panelTiming.Size     = new Size(596, 95);
        panelTiming.Controls.AddRange(new Control[] {
            lblStartTime, dtpStartTime,
            lblEndTime, dtpEndTime,
            lblDuration
        });

        // lblStartTime
        lblStartTime.AutoSize = false;
        lblStartTime.Location = new Point(12, 10);
        lblStartTime.Size     = new Size(80, 18);
        lblStartTime.Text     = "Início:";
        lblStartTime.Font     = new Font("Segoe UI", 9f);
        lblStartTime.Tag      = "muted";

        // dtpStartTime
        dtpStartTime.Format        = DateTimePickerFormat.Time;
        dtpStartTime.ShowUpDown    = true;
        dtpStartTime.Location      = new Point(12, 32);
        dtpStartTime.Size          = new Size(130, 26);
        dtpStartTime.Font          = new Font("Segoe UI", 10f);
        dtpStartTime.Value         = new DateTime(2025, 1, 1, 0, 0, 0);
        dtpStartTime.TabIndex      = 4;
        dtpStartTime.ValueChanged += dtpStartTime_ValueChanged;

        // lblEndTime
        lblEndTime.AutoSize = false;
        lblEndTime.Location = new Point(160, 10);
        lblEndTime.Size     = new Size(80, 18);
        lblEndTime.Text     = "Fim:";
        lblEndTime.Font     = new Font("Segoe UI", 9f);
        lblEndTime.Tag      = "muted";

        // dtpEndTime
        dtpEndTime.Format        = DateTimePickerFormat.Time;
        dtpEndTime.ShowUpDown    = true;
        dtpEndTime.Location      = new Point(160, 32);
        dtpEndTime.Size          = new Size(130, 26);
        dtpEndTime.Font          = new Font("Segoe UI", 10f);
        dtpEndTime.Value         = new DateTime(2025, 1, 1, 0, 1, 0);
        dtpEndTime.TabIndex      = 5;
        dtpEndTime.ValueChanged += dtpEndTime_ValueChanged;

        // lblDuration
        lblDuration.AutoSize = false;
        lblDuration.Location = new Point(12, 68);
        lblDuration.Size     = new Size(400, 20);
        lblDuration.Text     = "Duração: --:--:--";
        lblDuration.Font     = new Font("Segoe UI", 9f, FontStyle.Italic);
        lblDuration.Tag      = "muted";

        // ── btnProcess ──
        btnProcess.Location = new Point(12, 302);
        btnProcess.Size     = new Size(185, 40);
        btnProcess.Text     = "✂  Cortar Vídeo";
        btnProcess.Font     = new Font("Segoe UI", 10f, FontStyle.Bold);
        btnProcess.TabIndex = 6;
        btnProcess.Click   += btnProcess_Click;

        // ── btnCancel ──
        btnCancel.Location = new Point(207, 302);
        btnCancel.Size     = new Size(120, 40);
        btnCancel.Text     = "✕  Cancelar";
        btnCancel.Font     = new Font("Segoe UI", 10f);
        btnCancel.TabIndex = 7;
        btnCancel.Visible  = false;
        btnCancel.Click   += btnCancel_Click;

        // ── progressBar ──
        progressBar.Location = new Point(12, 358);
        progressBar.Size     = new Size(596, 6);
        progressBar.Visible  = false;
        progressBar.TabStop  = false;

        // ── lblStatus ──
        lblStatus.AutoSize  = false;
        lblStatus.Location  = new Point(12, 372);
        lblStatus.Size      = new Size(596, 20);
        lblStatus.Text      = "Pronto  •  Arraste um vídeo para iniciar";
        lblStatus.Font      = new Font("Segoe UI", 8.5f);
        lblStatus.TextAlign = ContentAlignment.MiddleLeft;

        // ── MainForm ──
        AutoScaleDimensions = new SizeF(96f, 96f);
        AutoScaleMode       = AutoScaleMode.Dpi;
        ClientSize          = new Size(620, 405);
        Font                = new Font("Segoe UI", 9f);
        FormBorderStyle     = FormBorderStyle.FixedSingle;
        MaximizeBox         = false;
        StartPosition       = FormStartPosition.CenterScreen;
        Text                = "Cuttage — Cortar Vídeos";

        Controls.AddRange(new Control[] {
            lblSectionFiles,
            panelFiles,
            lblSectionTiming,
            panelTiming,
            btnProcess,
            btnCancel,
            progressBar,
            lblStatus
        });

        Load += MainForm_Load;

        panelFiles.ResumeLayout(false);
        panelTiming.ResumeLayout(false);
        ResumeLayout(false);
    }
}
