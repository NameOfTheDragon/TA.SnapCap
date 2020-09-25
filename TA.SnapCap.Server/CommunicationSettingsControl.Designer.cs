namespace TA.SnapCap.Server
{
    partial class CommunicationSettingsControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.CommPortName = new System.Windows.Forms.ComboBox();
            this.UseSimulatorCheckbox = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.ConnectionStringDisplay = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Connection:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(167, 11);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(149, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = global::TA.SnapCap.Server.Properties.Settings.Default.SerialParameters;
            // 
            // CommPortName
            // 
            this.CommPortName.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TA.SnapCap.Server.Properties.Settings.Default, "CommPortName", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.CommPortName.FormattingEnabled = true;
            this.CommPortName.Location = new System.Drawing.Point(81, 11);
            this.CommPortName.Name = "CommPortName";
            this.CommPortName.Size = new System.Drawing.Size(80, 21);
            this.CommPortName.TabIndex = 0;
            this.CommPortName.Text = global::TA.SnapCap.Server.Properties.Settings.Default.CommPortName;
            this.CommPortName.SelectionChangeCommitted += new System.EventHandler(this.CommPortName_Changed);
            // 
            // UseSimulatorCheckbox
            // 
            this.UseSimulatorCheckbox.AutoSize = true;
            this.UseSimulatorCheckbox.Location = new System.Drawing.Point(81, 36);
            this.UseSimulatorCheckbox.Name = "UseSimulatorCheckbox";
            this.UseSimulatorCheckbox.Size = new System.Drawing.Size(140, 17);
            this.UseSimulatorCheckbox.TabIndex = 3;
            this.UseSimulatorCheckbox.Text = "Use Hardware Simulator";
            this.UseSimulatorCheckbox.UseVisualStyleBackColor = true;
            this.UseSimulatorCheckbox.CheckedChanged += new System.EventHandler(this.UseSimulatorCheckbox_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(11, 56);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(94, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Connection String:";
            // 
            // ConnectionStringDisplay
            // 
            this.ConnectionStringDisplay.AutoSize = true;
            this.ConnectionStringDisplay.DataBindings.Add(new System.Windows.Forms.Binding("Text", global::TA.SnapCap.Server.Properties.Settings.Default, "ConnectionString", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.ConnectionStringDisplay.Location = new System.Drawing.Point(111, 56);
            this.ConnectionStringDisplay.Name = "ConnectionStringDisplay";
            this.ConnectionStringDisplay.Size = new System.Drawing.Size(45, 13);
            this.ConnectionStringDisplay.TabIndex = 5;
            this.ConnectionStringDisplay.Text = global::TA.SnapCap.Server.Properties.Settings.Default.ConnectionString;
            // 
            // CommunicationSettingsControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Controls.Add(this.ConnectionStringDisplay);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.UseSimulatorCheckbox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CommPortName);
            this.Margin = new System.Windows.Forms.Padding(99);
            this.Name = "CommunicationSettingsControl";
            this.Padding = new System.Windows.Forms.Padding(8);
            this.Size = new System.Drawing.Size(327, 77);
            this.Load += new System.EventHandler(this.CommunicationSettingsControl_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox CommPortName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.CheckBox UseSimulatorCheckbox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label ConnectionStringDisplay;
        }
}
