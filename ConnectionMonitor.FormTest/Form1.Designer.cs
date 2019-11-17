namespace ConnectionMonitor.FormTest
{
    partial class Form1
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
            this.btnStartMonitor = new System.Windows.Forms.Button();
            this.btnStopMonitor = new System.Windows.Forms.Button();
            this.lvConnections = new System.Windows.Forms.ListView();
            this.SuspendLayout();
            // 
            // btnStartMonitor
            // 
            this.btnStartMonitor.Location = new System.Drawing.Point(40, 42);
            this.btnStartMonitor.Name = "btnStartMonitor";
            this.btnStartMonitor.Size = new System.Drawing.Size(104, 34);
            this.btnStartMonitor.TabIndex = 0;
            this.btnStartMonitor.Text = "Start Monitor";
            this.btnStartMonitor.UseVisualStyleBackColor = true;
            this.btnStartMonitor.Click += new System.EventHandler(this.BtnStartMonitor_Click);
            // 
            // btnStopMonitor
            // 
            this.btnStopMonitor.Location = new System.Drawing.Point(181, 42);
            this.btnStopMonitor.Name = "btnStopMonitor";
            this.btnStopMonitor.Size = new System.Drawing.Size(105, 34);
            this.btnStopMonitor.TabIndex = 1;
            this.btnStopMonitor.Text = "Stop Monitor";
            this.btnStopMonitor.UseVisualStyleBackColor = true;
            this.btnStopMonitor.Click += new System.EventHandler(this.BtnStopMonitor_Click);
            // 
            // lvConnections
            // 
            this.lvConnections.HideSelection = false;
            this.lvConnections.Location = new System.Drawing.Point(40, 111);
            this.lvConnections.Name = "lvConnections";
            this.lvConnections.Size = new System.Drawing.Size(530, 376);
            this.lvConnections.TabIndex = 2;
            this.lvConnections.UseCompatibleStateImageBehavior = false;
            this.lvConnections.View = System.Windows.Forms.View.List;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 511);
            this.Controls.Add(this.lvConnections);
            this.Controls.Add(this.btnStopMonitor);
            this.Controls.Add(this.btnStartMonitor);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnStartMonitor;
        private System.Windows.Forms.Button btnStopMonitor;
        private System.Windows.Forms.ListView lvConnections;
    }
}

