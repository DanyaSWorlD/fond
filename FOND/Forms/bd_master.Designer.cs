namespace FOND
{
    partial class bd_master
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(bd_master));
            this.panel1 = new System.Windows.Forms.Panel();
            this.nextB = new System.Windows.Forms.Button();
            this.cancB = new System.Windows.Forms.Button();
            this.bacbB = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // panel1
            // 
            resources.ApplyResources(this.panel1, "panel1");
            this.panel1.Name = "panel1";
            // 
            // nextB
            // 
            resources.ApplyResources(this.nextB, "nextB");
            this.nextB.Name = "nextB";
            this.nextB.UseVisualStyleBackColor = true;
            this.nextB.Click += new System.EventHandler(this.button3_Click);
            // 
            // cancB
            // 
            resources.ApplyResources(this.cancB, "cancB");
            this.cancB.Name = "cancB";
            this.cancB.UseVisualStyleBackColor = true;
            this.cancB.Click += new System.EventHandler(this.cancB_Click);
            // 
            // bacbB
            // 
            resources.ApplyResources(this.bacbB, "bacbB");
            this.bacbB.Name = "bacbB";
            this.bacbB.UseVisualStyleBackColor = true;
            this.bacbB.Click += new System.EventHandler(this.bacbB_Click);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // bd_master
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cancB);
            this.Controls.Add(this.nextB);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.bacbB);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "bd_master";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.bd_master_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.bd_master_FormClosed);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button nextB;
        private System.Windows.Forms.Button cancB;
        private System.Windows.Forms.Button bacbB;
        private System.Windows.Forms.Timer timer1;
    }
}