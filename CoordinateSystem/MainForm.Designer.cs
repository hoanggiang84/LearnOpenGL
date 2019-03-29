namespace CoordinateSystem
{
    partial class MainForm
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
            this.components = new System.ComponentModel.Container();
            this.panelOpenGL = new OpenTK.GLControl();
            this.timerUpdateFrame = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // panelOpenGL
            // 
            this.panelOpenGL.BackColor = System.Drawing.Color.Black;
            this.panelOpenGL.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelOpenGL.Location = new System.Drawing.Point(0, 0);
            this.panelOpenGL.Name = "panelOpenGL";
            this.panelOpenGL.Size = new System.Drawing.Size(584, 362);
            this.panelOpenGL.TabIndex = 0;
            this.panelOpenGL.VSync = true;
            this.panelOpenGL.Load += new System.EventHandler(this.panelOpenGL_Load);
            this.panelOpenGL.Paint += new System.Windows.Forms.PaintEventHandler(this.panelOpenGL_Paint);
            this.panelOpenGL.KeyDown += new System.Windows.Forms.KeyEventHandler(this.panelOpenGL_KeyDown);
            this.panelOpenGL.KeyUp += new System.Windows.Forms.KeyEventHandler(this.panelOpenGL_KeyUp);
            this.panelOpenGL.MouseDown += new System.Windows.Forms.MouseEventHandler(this.panelOpenGL_MouseDown);
            this.panelOpenGL.MouseLeave += new System.EventHandler(this.panelOpenGL_MouseLeave);
            this.panelOpenGL.MouseMove += new System.Windows.Forms.MouseEventHandler(this.panelOpenGL_MouseMove);
            this.panelOpenGL.MouseUp += new System.Windows.Forms.MouseEventHandler(this.panelOpenGL_MouseUp);
            this.panelOpenGL.Resize += new System.EventHandler(this.panelOpenGL_Resize);
            // 
            // timerUpdateFrame
            // 
            this.timerUpdateFrame.Enabled = true;
            this.timerUpdateFrame.Interval = 25;
            this.timerUpdateFrame.Tick += new System.EventHandler(this.timerUpdateFrame_Tick);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 362);
            this.Controls.Add(this.panelOpenGL);
            this.Name = "MainForm";
            this.Text = "OpenGL";
            this.ResumeLayout(false);

        }

        #endregion

        private OpenTK.GLControl panelOpenGL;
        private System.Windows.Forms.Timer timerUpdateFrame;
    }
}

