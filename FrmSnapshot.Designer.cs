namespace BaudMate
{
    partial class FrmSnapshot
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
            ObjText = new TextBox();
            SuspendLayout();
            // 
            // ObjText
            // 
            ObjText.AcceptsReturn = true;
            ObjText.AcceptsTab = true;
            ObjText.BorderStyle = BorderStyle.None;
            ObjText.Dock = DockStyle.Fill;
            ObjText.Location = new Point(0, 0);
            ObjText.Multiline = true;
            ObjText.Name = "ObjText";
            ObjText.ReadOnly = true;
            ObjText.ScrollBars = ScrollBars.Both;
            ObjText.Size = new Size(800, 450);
            ObjText.TabIndex = 0;
            // 
            // FrmSnapshot
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(ObjText);
            Name = "FrmSnapshot";
            Text = "Snapshot";
            Load += FrmSnapshot_Load;
            Shown += FrmSnapshot_Shown;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        internal TextBox ObjText;
    }
}