namespace TA.SnapCap.Server
{
    partial class ServerStatusDisplay
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.registeredClientCount = new System.Windows.Forms.Label();
            this.OnlineClients = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.ClientStatus = new System.Windows.Forms.ListBox();
            this.annunciatorPanel1 = new ASCOM.Controls.AnnunciatorPanel();
            this.ConnectedAnnunciator = new ASCOM.Controls.Annunciator();
            this.MotorAnnunciator = new ASCOM.Controls.Annunciator();
            this.IlluminationAnnunciator = new ASCOM.Controls.Annunciator();
            this.BrightnessAnnunciator = new ASCOM.Controls.Annunciator();
            this.DispositionAnnunciator = new ASCOM.Controls.Annunciator();
            this.SetupCommand = new System.Windows.Forms.Button();
            this.annunciatorPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 21);
            this.label1.TabIndex = 0;
            this.label1.Text = "Registered clients:";
            // 
            // registeredClientCount
            // 
            this.registeredClientCount.AutoSize = true;
            this.registeredClientCount.Location = new System.Drawing.Point(122, 10);
            this.registeredClientCount.Name = "registeredClientCount";
            this.registeredClientCount.Size = new System.Drawing.Size(13, 13);
            this.registeredClientCount.TabIndex = 1;
            this.registeredClientCount.Text = "0";
            // 
            // OnlineClients
            // 
            this.OnlineClients.AutoSize = true;
            this.OnlineClients.Location = new System.Drawing.Point(226, 10);
            this.OnlineClients.Name = "OnlineClients";
            this.OnlineClients.Size = new System.Drawing.Size(13, 13);
            this.OnlineClients.TabIndex = 3;
            this.OnlineClients.Text = "0";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(166, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 21);
            this.label3.TabIndex = 2;
            this.label3.Text = "Online:";
            // 
            // ClientStatus
            // 
            this.ClientStatus.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ClientStatus.FormattingEnabled = true;
            this.ClientStatus.Location = new System.Drawing.Point(13, 35);
            this.ClientStatus.Name = "ClientStatus";
            this.ClientStatus.Size = new System.Drawing.Size(725, 108);
            this.ClientStatus.TabIndex = 4;
            // 
            // annunciatorPanel1
            // 
            this.annunciatorPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.annunciatorPanel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.annunciatorPanel1.Controls.Add(this.ConnectedAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.MotorAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.IlluminationAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.BrightnessAnnunciator);
            this.annunciatorPanel1.Controls.Add(this.DispositionAnnunciator);
            this.annunciatorPanel1.Location = new System.Drawing.Point(352, 7);
            this.annunciatorPanel1.Name = "annunciatorPanel1";
            this.annunciatorPanel1.Size = new System.Drawing.Size(386, 19);
            this.annunciatorPanel1.TabIndex = 5;
            // 
            // ConnectedAnnunciator
            // 
            this.ConnectedAnnunciator.ActiveColor = System.Drawing.Color.DarkSeaGreen;
            this.ConnectedAnnunciator.AutoSize = true;
            this.ConnectedAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.ConnectedAnnunciator.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectedAnnunciator.ForeColor = System.Drawing.Color.DarkSeaGreen;
            this.ConnectedAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.ConnectedAnnunciator.Location = new System.Drawing.Point(3, 0);
            this.ConnectedAnnunciator.Mute = false;
            this.ConnectedAnnunciator.Name = "ConnectedAnnunciator";
            this.ConnectedAnnunciator.Size = new System.Drawing.Size(90, 19);
            this.ConnectedAnnunciator.TabIndex = 0;
            this.ConnectedAnnunciator.Text = "Connected";
            // 
            // MotorAnnunciator
            // 
            this.MotorAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.MotorAnnunciator.AutoSize = true;
            this.MotorAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.MotorAnnunciator.Cadence = ASCOM.Controls.CadencePattern.BlinkAlarm;
            this.MotorAnnunciator.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MotorAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.MotorAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.MotorAnnunciator.Location = new System.Drawing.Point(99, 0);
            this.MotorAnnunciator.Mute = false;
            this.MotorAnnunciator.Name = "MotorAnnunciator";
            this.MotorAnnunciator.Size = new System.Drawing.Size(54, 19);
            this.MotorAnnunciator.TabIndex = 0;
            this.MotorAnnunciator.Text = "Motor";
            // 
            // IlluminationAnnunciator
            // 
            this.IlluminationAnnunciator.ActiveColor = System.Drawing.Color.PaleGoldenrod;
            this.IlluminationAnnunciator.AutoSize = true;
            this.IlluminationAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.IlluminationAnnunciator.Cadence = ASCOM.Controls.CadencePattern.Wink;
            this.IlluminationAnnunciator.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.IlluminationAnnunciator.ForeColor = System.Drawing.Color.PaleGoldenrod;
            this.IlluminationAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.IlluminationAnnunciator.Location = new System.Drawing.Point(159, 0);
            this.IlluminationAnnunciator.Mute = false;
            this.IlluminationAnnunciator.Name = "IlluminationAnnunciator";
            this.IlluminationAnnunciator.Size = new System.Drawing.Size(54, 19);
            this.IlluminationAnnunciator.TabIndex = 0;
            this.IlluminationAnnunciator.Text = "Light";
            // 
            // BrightnessAnnunciator
            // 
            this.BrightnessAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.BrightnessAnnunciator.AutoSize = true;
            this.BrightnessAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.BrightnessAnnunciator.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BrightnessAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.BrightnessAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.BrightnessAnnunciator.Location = new System.Drawing.Point(219, 0);
            this.BrightnessAnnunciator.Mute = false;
            this.BrightnessAnnunciator.Name = "BrightnessAnnunciator";
            this.BrightnessAnnunciator.Size = new System.Drawing.Size(45, 19);
            this.BrightnessAnnunciator.TabIndex = 0;
            this.BrightnessAnnunciator.Text = "100%";
            // 
            // DispositionAnnunciator
            // 
            this.DispositionAnnunciator.ActiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.DispositionAnnunciator.AutoSize = true;
            this.DispositionAnnunciator.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.DispositionAnnunciator.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DispositionAnnunciator.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.DispositionAnnunciator.InactiveColor = System.Drawing.Color.FromArgb(((int)(((byte)(96)))), ((int)(((byte)(4)))), ((int)(((byte)(4)))));
            this.DispositionAnnunciator.Location = new System.Drawing.Point(270, 0);
            this.DispositionAnnunciator.Mute = false;
            this.DispositionAnnunciator.Name = "DispositionAnnunciator";
            this.DispositionAnnunciator.Size = new System.Drawing.Size(108, 19);
            this.DispositionAnnunciator.TabIndex = 0;
            this.DispositionAnnunciator.Text = "Disposition";
            // 
            // SetupCommand
            // 
            this.SetupCommand.Location = new System.Drawing.Point(265, 5);
            this.SetupCommand.Name = "SetupCommand";
            this.SetupCommand.Size = new System.Drawing.Size(75, 23);
            this.SetupCommand.TabIndex = 8;
            this.SetupCommand.Text = "Setup...";
            this.SetupCommand.UseVisualStyleBackColor = true;
            this.SetupCommand.Click += new System.EventHandler(this.SetupCommand_Click);
            // 
            // ServerStatusDisplay
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(750, 160);
            this.Controls.Add(this.SetupCommand);
            this.Controls.Add(this.annunciatorPanel1);
            this.Controls.Add(this.ClientStatus);
            this.Controls.Add(this.OnlineClients);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.registeredClientCount);
            this.Controls.Add(this.label1);
            this.DataBindings.Add(new System.Windows.Forms.Binding("Location", global::TA.SnapCap.Server.Properties.Settings.Default, "MainFormLocation", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Location = global::TA.SnapCap.Server.Properties.Settings.Default.MainFormLocation;
            this.Name = "ServerStatusDisplay";
            this.Text = "SnapCap Controller";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmMain_FormClosing);
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.LocationChanged += new System.EventHandler(this.frmMain_LocationChanged);
            this.annunciatorPanel1.ResumeLayout(false);
            this.annunciatorPanel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label registeredClientCount;
        private System.Windows.Forms.Label OnlineClients;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ListBox ClientStatus;
        private ASCOM.Controls.AnnunciatorPanel annunciatorPanel1;
        private System.Windows.Forms.Button SetupCommand;
        private ASCOM.Controls.Annunciator ConnectedAnnunciator;
        private ASCOM.Controls.Annunciator MotorAnnunciator;
        private ASCOM.Controls.Annunciator IlluminationAnnunciator;
        private ASCOM.Controls.Annunciator BrightnessAnnunciator;
        private ASCOM.Controls.Annunciator DispositionAnnunciator;
    }
}

