namespace circle_sharp_gui
{
    partial class ServerWindow
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager (typeof (ServerWindow));
			this.outputTextBox = new System.Windows.Forms.RichTextBox ();
			this.SuspendLayout ();
			// 
			// outputTextBox
			// 
			this.outputTextBox.BackColor = System.Drawing.Color.Black;
			this.outputTextBox.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.outputTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
			this.outputTextBox.Font = new System.Drawing.Font ("Lucida Console", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.outputTextBox.ForeColor = System.Drawing.Color.White;
			this.outputTextBox.Location = new System.Drawing.Point (0, 0);
			this.outputTextBox.Margin = new System.Windows.Forms.Padding (1);
			this.outputTextBox.Name = "outputTextBox";
			this.outputTextBox.ReadOnly = true;
			this.outputTextBox.Size = new System.Drawing.Size (611, 408);
			this.outputTextBox.TabIndex = 2;
			this.outputTextBox.Text = "";
			// 
			// ServerWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF (6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size (611, 408);
			this.Controls.Add (this.outputTextBox);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject ("$this.Icon")));
			this.Name = "ServerWindow";
			this.Text = "CircleSharp Server";
			this.Load += new System.EventHandler (this.ServerWindow_Load);
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler (this.ServerWindow_FormClosing);
			this.ResumeLayout (false);

        }

        #endregion

		private System.Windows.Forms.RichTextBox outputTextBox;

	}
}

