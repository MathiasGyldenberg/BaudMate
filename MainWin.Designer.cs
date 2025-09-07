namespace BaudMate
{
    partial class MainWin
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
            ObjTool = new ToolStrip();
            ObjStatus = new StatusStrip();
            ObjPanelMain = new Panel();
            ObjSC = new SplitContainer();
            ObjSCTerminal = new SplitContainer();
            ObjGroupReceived = new GroupBox();
            ObjTextResponse = new TextBox();
            ObjGroupSend = new GroupBox();
            ObjTextSend = new TextBox();
            ObjPanelSendBtn = new Panel();
            BtnSend = new Button();
            ObjGroupProperties = new GroupBox();
            ObjProperties = new PropertyGrid();
            ObjPanelMain.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ObjSC).BeginInit();
            ObjSC.Panel1.SuspendLayout();
            ObjSC.Panel2.SuspendLayout();
            ObjSC.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ObjSCTerminal).BeginInit();
            ObjSCTerminal.Panel1.SuspendLayout();
            ObjSCTerminal.Panel2.SuspendLayout();
            ObjSCTerminal.SuspendLayout();
            ObjGroupReceived.SuspendLayout();
            ObjGroupSend.SuspendLayout();
            ObjPanelSendBtn.SuspendLayout();
            ObjGroupProperties.SuspendLayout();
            SuspendLayout();
            // 
            // ObjTool
            // 
            ObjTool.GripStyle = ToolStripGripStyle.Hidden;
            ObjTool.Location = new Point(0, 0);
            ObjTool.Name = "ObjTool";
            ObjTool.Padding = new Padding(6, 0, 6, 0);
            ObjTool.RenderMode = ToolStripRenderMode.System;
            ObjTool.Size = new Size(1164, 25);
            ObjTool.TabIndex = 0;
            // 
            // ObjStatus
            // 
            ObjStatus.Location = new Point(0, 439);
            ObjStatus.Name = "ObjStatus";
            ObjStatus.Size = new Size(897, 22);
            ObjStatus.TabIndex = 1;
            ObjStatus.Visible = false;
            // 
            // ObjPanelMain
            // 
            ObjPanelMain.Controls.Add(ObjSC);
            ObjPanelMain.Dock = DockStyle.Fill;
            ObjPanelMain.Location = new Point(0, 25);
            ObjPanelMain.Name = "ObjPanelMain";
            ObjPanelMain.Padding = new Padding(6, 0, 6, 0);
            ObjPanelMain.Size = new Size(1164, 736);
            ObjPanelMain.TabIndex = 2;
            // 
            // ObjSC
            // 
            ObjSC.Dock = DockStyle.Fill;
            ObjSC.FixedPanel = FixedPanel.Panel2;
            ObjSC.Location = new Point(6, 0);
            ObjSC.Name = "ObjSC";
            // 
            // ObjSC.Panel1
            // 
            ObjSC.Panel1.Controls.Add(ObjSCTerminal);
            // 
            // ObjSC.Panel2
            // 
            ObjSC.Panel2.AutoScroll = true;
            ObjSC.Panel2.Controls.Add(ObjGroupProperties);
            ObjSC.Size = new Size(1152, 736);
            ObjSC.SplitterDistance = 838;
            ObjSC.TabIndex = 0;
            ObjSC.TabStop = false;
            // 
            // ObjSCTerminal
            // 
            ObjSCTerminal.Dock = DockStyle.Fill;
            ObjSCTerminal.FixedPanel = FixedPanel.Panel2;
            ObjSCTerminal.Location = new Point(0, 0);
            ObjSCTerminal.Name = "ObjSCTerminal";
            ObjSCTerminal.Orientation = Orientation.Horizontal;
            // 
            // ObjSCTerminal.Panel1
            // 
            ObjSCTerminal.Panel1.Controls.Add(ObjGroupReceived);
            // 
            // ObjSCTerminal.Panel2
            // 
            ObjSCTerminal.Panel2.Controls.Add(ObjGroupSend);
            ObjSCTerminal.Panel2.Controls.Add(ObjPanelSendBtn);
            ObjSCTerminal.Size = new Size(838, 736);
            ObjSCTerminal.SplitterDistance = 450;
            ObjSCTerminal.TabIndex = 0;
            ObjSCTerminal.TabStop = false;
            // 
            // ObjGroupReceived
            // 
            ObjGroupReceived.Controls.Add(ObjTextResponse);
            ObjGroupReceived.Dock = DockStyle.Fill;
            ObjGroupReceived.Location = new Point(0, 0);
            ObjGroupReceived.Name = "ObjGroupReceived";
            ObjGroupReceived.Padding = new Padding(6, 0, 6, 3);
            ObjGroupReceived.Size = new Size(838, 450);
            ObjGroupReceived.TabIndex = 4;
            ObjGroupReceived.TabStop = false;
            // 
            // ObjTextResponse
            // 
            ObjTextResponse.AcceptsReturn = true;
            ObjTextResponse.AcceptsTab = true;
            ObjTextResponse.BorderStyle = BorderStyle.None;
            ObjTextResponse.Dock = DockStyle.Fill;
            ObjTextResponse.Font = new Font("Consolas", 11.25F);
            ObjTextResponse.Location = new Point(6, 16);
            ObjTextResponse.MaxLength = 0;
            ObjTextResponse.Multiline = true;
            ObjTextResponse.Name = "ObjTextResponse";
            ObjTextResponse.ReadOnly = true;
            ObjTextResponse.ScrollBars = ScrollBars.Both;
            ObjTextResponse.Size = new Size(826, 431);
            ObjTextResponse.TabIndex = 2;
            // 
            // ObjGroupSend
            // 
            ObjGroupSend.Controls.Add(ObjTextSend);
            ObjGroupSend.Dock = DockStyle.Fill;
            ObjGroupSend.Location = new Point(0, 0);
            ObjGroupSend.Name = "ObjGroupSend";
            ObjGroupSend.Padding = new Padding(6, 0, 6, 3);
            ObjGroupSend.Size = new Size(838, 246);
            ObjGroupSend.TabIndex = 6;
            ObjGroupSend.TabStop = false;
            // 
            // ObjTextSend
            // 
            ObjTextSend.AcceptsReturn = true;
            ObjTextSend.BorderStyle = BorderStyle.None;
            ObjTextSend.Dock = DockStyle.Fill;
            ObjTextSend.Font = new Font("Consolas", 11.25F);
            ObjTextSend.Location = new Point(6, 16);
            ObjTextSend.MaxLength = 0;
            ObjTextSend.Multiline = true;
            ObjTextSend.Name = "ObjTextSend";
            ObjTextSend.ScrollBars = ScrollBars.Both;
            ObjTextSend.Size = new Size(826, 227);
            ObjTextSend.TabIndex = 1;
            ObjTextSend.KeyDown += ObjTextSend_KeyDown;
            ObjTextSend.KeyUp += ObjTextSend_KeyUp;
            // 
            // ObjPanelSendBtn
            // 
            ObjPanelSendBtn.Controls.Add(BtnSend);
            ObjPanelSendBtn.Dock = DockStyle.Bottom;
            ObjPanelSendBtn.Location = new Point(0, 246);
            ObjPanelSendBtn.Name = "ObjPanelSendBtn";
            ObjPanelSendBtn.Padding = new Padding(3, 3, 0, 3);
            ObjPanelSendBtn.Size = new Size(838, 36);
            ObjPanelSendBtn.TabIndex = 5;
            // 
            // BtnSend
            // 
            BtnSend.Dock = DockStyle.Fill;
            BtnSend.FlatStyle = FlatStyle.System;
            BtnSend.Location = new Point(3, 3);
            BtnSend.Name = "BtnSend";
            BtnSend.Size = new Size(835, 30);
            BtnSend.TabIndex = 0;
            BtnSend.Text = "Send";
            BtnSend.UseVisualStyleBackColor = true;
            BtnSend.Click += BtnSend_Click;
            // 
            // ObjGroupProperties
            // 
            ObjGroupProperties.Controls.Add(ObjProperties);
            ObjGroupProperties.Dock = DockStyle.Fill;
            ObjGroupProperties.Location = new Point(0, 0);
            ObjGroupProperties.Name = "ObjGroupProperties";
            ObjGroupProperties.Padding = new Padding(6);
            ObjGroupProperties.Size = new Size(310, 736);
            ObjGroupProperties.TabIndex = 4;
            ObjGroupProperties.TabStop = false;
            ObjGroupProperties.Text = "  Properties  ";
            // 
            // ObjProperties
            // 
            ObjProperties.BackColor = SystemColors.Control;
            ObjProperties.Dock = DockStyle.Fill;
            ObjProperties.Location = new Point(6, 22);
            ObjProperties.Name = "ObjProperties";
            ObjProperties.PropertySort = PropertySort.Categorized;
            ObjProperties.Size = new Size(298, 708);
            ObjProperties.TabIndex = 0;
            // 
            // MainWin
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1164, 761);
            Controls.Add(ObjPanelMain);
            Controls.Add(ObjTool);
            Controls.Add(ObjStatus);
            DoubleBuffered = true;
            Name = "MainWin";
            SizeGripStyle = SizeGripStyle.Hide;
            Text = "BaudMate";
            FormClosing += FrmMain_FormClosing;
            ObjPanelMain.ResumeLayout(false);
            ObjSC.Panel1.ResumeLayout(false);
            ObjSC.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ObjSC).EndInit();
            ObjSC.ResumeLayout(false);
            ObjSCTerminal.Panel1.ResumeLayout(false);
            ObjSCTerminal.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)ObjSCTerminal).EndInit();
            ObjSCTerminal.ResumeLayout(false);
            ObjGroupReceived.ResumeLayout(false);
            ObjGroupReceived.PerformLayout();
            ObjGroupSend.ResumeLayout(false);
            ObjGroupSend.PerformLayout();
            ObjPanelSendBtn.ResumeLayout(false);
            ObjGroupProperties.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ToolStrip ObjTool;
        private StatusStrip ObjStatus;
        private Panel ObjPanelMain;
        private SplitContainer ObjSC;
        private SplitContainer ObjSCTerminal;
        private GroupBox ObjGroupProperties;
        private PropertyGrid ObjProperties;
        private GroupBox ObjGroupReceived;
        private Panel ObjPanelSendBtn;
        private Button BtnSend;
        private GroupBox ObjGroupSend;
        internal TextBox ObjTextResponse;
        internal TextBox ObjTextSend;
    }
}