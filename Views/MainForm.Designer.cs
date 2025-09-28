using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cuttage.Views
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;
        private TextBox txtInputFile;
        private TextBox txtOutputFile;
        private Button btnSelectInput;
        private Button btnSelectOutput;
        private DateTimePicker dtpStartTime;
        private DateTimePicker dtpEndTime;
        private Button btnProcess;
        private Label lblInputFile;
        private Label lblOutputFile;
        private Label lblStartTime;
        private Label lblEndTime;
        private Label lblDuration;
        private Label lblStatus;
        private ProgressBar progressBar;
        private GroupBox groupBoxFiles;
        private GroupBox groupBoxTiming;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            txtInputFile = new TextBox();
            txtOutputFile = new TextBox();
            btnSelectInput = new Button();
            btnSelectOutput = new Button();
            dtpStartTime = new DateTimePicker();
            dtpEndTime = new DateTimePicker();
            btnProcess = new Button();
            lblInputFile = new Label();
            lblOutputFile = new Label();
            lblStartTime = new Label();
            lblEndTime = new Label();
            lblDuration = new Label();
            lblStatus = new Label();
            progressBar = new ProgressBar();
            groupBoxFiles = new GroupBox();
            groupBoxTiming = new GroupBox();
            groupBoxFiles.SuspendLayout();
            groupBoxTiming.SuspendLayout();
            SuspendLayout();
            // 
            // txtInputFile
            // 
            txtInputFile.Location = new Point(15, 50);
            txtInputFile.Name = "txtInputFile";
            txtInputFile.ReadOnly = true;
            txtInputFile.Size = new Size(450, 23);
            txtInputFile.TabIndex = 1;
            // 
            // txtOutputFile
            // 
            txtOutputFile.Location = new Point(15, 100);
            txtOutputFile.Name = "txtOutputFile";
            txtOutputFile.ReadOnly = true;
            txtOutputFile.Size = new Size(450, 23);
            txtOutputFile.TabIndex = 3;
            // 
            // btnSelectInput
            // 
            btnSelectInput.Location = new Point(475, 50);
            btnSelectInput.Name = "btnSelectInput";
            btnSelectInput.Size = new Size(85, 23);
            btnSelectInput.TabIndex = 2;
            btnSelectInput.Text = "Selecionar";
            btnSelectInput.UseVisualStyleBackColor = true;
            btnSelectInput.Click += btnSelectInput_Click;
            // 
            // btnSelectOutput
            // 
            btnSelectOutput.Location = new Point(475, 100);
            btnSelectOutput.Name = "btnSelectOutput";
            btnSelectOutput.Size = new Size(85, 23);
            btnSelectOutput.TabIndex = 4;
            btnSelectOutput.Text = "Selecionar";
            btnSelectOutput.UseVisualStyleBackColor = true;
            btnSelectOutput.Click += btnSelectOutput_Click;
            // 
            // dtpStartTime
            // 
            dtpStartTime.Format = DateTimePickerFormat.Time;
            dtpStartTime.Location = new Point(15, 50);
            dtpStartTime.Name = "dtpStartTime";
            dtpStartTime.ShowUpDown = true;
            dtpStartTime.Size = new Size(120, 23);
            dtpStartTime.TabIndex = 6;
            dtpStartTime.Value = new DateTime(2025, 9, 28, 0, 0, 0, 0);
            dtpStartTime.ValueChanged += dtpStartTime_ValueChanged;
            // 
            // dtpEndTime
            // 
            dtpEndTime.Format = DateTimePickerFormat.Time;
            dtpEndTime.Location = new Point(150, 50);
            dtpEndTime.Name = "dtpEndTime";
            dtpEndTime.ShowUpDown = true;
            dtpEndTime.Size = new Size(120, 23);
            dtpEndTime.TabIndex = 7;
            dtpEndTime.Value = new DateTime(2025, 9, 28, 0, 1, 0, 0);
            dtpEndTime.ValueChanged += dtpEndTime_ValueChanged;
            // 
            // btnProcess
            // 
            btnProcess.BackColor = Color.Green;
            btnProcess.ForeColor = Color.White;
            btnProcess.Location = new Point(250, 274);
            btnProcess.Name = "btnProcess";
            btnProcess.Size = new Size(100, 35);
            btnProcess.TabIndex = 8;
            btnProcess.Text = "Cortar Vídeo";
            btnProcess.UseVisualStyleBackColor = false;
            btnProcess.Click += btnProcess_Click;
            // 
            // lblInputFile
            // 
            lblInputFile.Location = new Point(15, 25);
            lblInputFile.Name = "lblInputFile";
            lblInputFile.Size = new Size(120, 23);
            lblInputFile.TabIndex = 0;
            lblInputFile.Text = "Arquivo de entrada:";
            lblInputFile.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblOutputFile
            // 
            lblOutputFile.Location = new Point(15, 75);
            lblOutputFile.Name = "lblOutputFile";
            lblOutputFile.Size = new Size(120, 23);
            lblOutputFile.TabIndex = 3;
            lblOutputFile.Text = "Arquivo de saída:";
            lblOutputFile.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStartTime
            // 
            lblStartTime.Location = new Point(15, 25);
            lblStartTime.Name = "lblStartTime";
            lblStartTime.Size = new Size(100, 23);
            lblStartTime.TabIndex = 0;
            lblStartTime.Text = "Tempo inicial:";
            lblStartTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblEndTime
            // 
            lblEndTime.Location = new Point(150, 25);
            lblEndTime.Name = "lblEndTime";
            lblEndTime.Size = new Size(100, 23);
            lblEndTime.TabIndex = 7;
            lblEndTime.Text = "Tempo final:";
            lblEndTime.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblDuration
            // 
            lblDuration.Location = new Point(15, 85);
            lblDuration.Name = "lblDuration";
            lblDuration.Size = new Size(300, 23);
            lblDuration.TabIndex = 8;
            lblDuration.Text = "Duração: 00:01:00";
            lblDuration.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // lblStatus
            // 
            lblStatus.Location = new Point(27, 312);
            lblStatus.Name = "lblStatus";
            lblStatus.Size = new Size(546, 23);
            lblStatus.TabIndex = 10;
            lblStatus.Text = "Pronto";
            lblStatus.TextAlign = ContentAlignment.MiddleLeft;
            lblStatus.Visible = false;
            // 
            // progressBar
            // 
            progressBar.Location = new Point(27, 343);
            progressBar.Name = "progressBar";
            progressBar.Size = new Size(546, 15);
            progressBar.TabIndex = 9;
            progressBar.Visible = false;
            // 
            // groupBoxFiles
            // 
            groupBoxFiles.Controls.Add(lblInputFile);
            groupBoxFiles.Controls.Add(txtInputFile);
            groupBoxFiles.Controls.Add(btnSelectInput);
            groupBoxFiles.Controls.Add(lblOutputFile);
            groupBoxFiles.Controls.Add(txtOutputFile);
            groupBoxFiles.Controls.Add(btnSelectOutput);
            groupBoxFiles.Location = new Point(12, 12);
            groupBoxFiles.Name = "groupBoxFiles";
            groupBoxFiles.Size = new Size(576, 130);
            groupBoxFiles.TabIndex = 0;
            groupBoxFiles.TabStop = false;
            groupBoxFiles.Text = "Arquivos";
            // 
            // groupBoxTiming
            // 
            groupBoxTiming.Controls.Add(lblStartTime);
            groupBoxTiming.Controls.Add(dtpStartTime);
            groupBoxTiming.Controls.Add(lblEndTime);
            groupBoxTiming.Controls.Add(dtpEndTime);
            groupBoxTiming.Controls.Add(lblDuration);
            groupBoxTiming.Location = new Point(12, 148);
            groupBoxTiming.Name = "groupBoxTiming";
            groupBoxTiming.Size = new Size(576, 120);
            groupBoxTiming.TabIndex = 5;
            groupBoxTiming.TabStop = false;
            groupBoxTiming.Text = "Tempo de Corte";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(600, 370);
            Controls.Add(groupBoxFiles);
            Controls.Add(groupBoxTiming);
            Controls.Add(btnProcess);
            Controls.Add(progressBar);
            Controls.Add(lblStatus);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Futtage Cutter - Cortar Vídeos";
            groupBoxFiles.ResumeLayout(false);
            groupBoxFiles.PerformLayout();
            groupBoxTiming.ResumeLayout(false);
            ResumeLayout(false);
        }
    }
}
