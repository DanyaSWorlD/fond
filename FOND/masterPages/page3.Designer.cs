namespace FOND.masterPages
{
    partial class page3
    {
        /// <summary> 
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.TvReg = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.TvCentr = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.GazReg = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.GazCentr = new System.Windows.Forms.TextBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label5 = new System.Windows.Forms.Label();
            this.RadReg = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.RadCentr = new System.Windows.Forms.TextBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.internet = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(482, 403);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.TvReg);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.TvCentr);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(474, 374);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Телевидение";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(299, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(102, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "региональные";
            // 
            // TvReg
            // 
            this.TvReg.Location = new System.Drawing.Point(252, 23);
            this.TvReg.Multiline = true;
            this.TvReg.Name = "TvReg";
            this.TvReg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TvReg.Size = new System.Drawing.Size(204, 345);
            this.TvReg.TabIndex = 3;
            this.TvReg.TextChanged += new System.EventHandler(this.TvCentr_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(68, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(96, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "центральные";
            // 
            // TvCentr
            // 
            this.TvCentr.Location = new System.Drawing.Point(21, 23);
            this.TvCentr.Multiline = true;
            this.TvCentr.Name = "TvCentr";
            this.TvCentr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.TvCentr.Size = new System.Drawing.Size(204, 345);
            this.TvCentr.TabIndex = 0;
            this.TvCentr.TextChanged += new System.EventHandler(this.TvCentr_TextChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.GazReg);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.GazCentr);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(474, 374);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Газеты";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(298, 5);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(102, 17);
            this.label3.TabIndex = 8;
            this.label3.Text = "региональные";
            // 
            // GazReg
            // 
            this.GazReg.Location = new System.Drawing.Point(251, 25);
            this.GazReg.Multiline = true;
            this.GazReg.Name = "GazReg";
            this.GazReg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GazReg.Size = new System.Drawing.Size(204, 345);
            this.GazReg.TabIndex = 7;
            this.GazReg.TextChanged += new System.EventHandler(this.TvCentr_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(67, 5);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(96, 17);
            this.label4.TabIndex = 6;
            this.label4.Text = "центральные";
            // 
            // GazCentr
            // 
            this.GazCentr.Location = new System.Drawing.Point(20, 25);
            this.GazCentr.Multiline = true;
            this.GazCentr.Name = "GazCentr";
            this.GazCentr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.GazCentr.Size = new System.Drawing.Size(204, 345);
            this.GazCentr.TabIndex = 5;
            this.GazCentr.TextChanged += new System.EventHandler(this.TvCentr_TextChanged);
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label5);
            this.tabPage3.Controls.Add(this.RadReg);
            this.tabPage3.Controls.Add(this.label6);
            this.tabPage3.Controls.Add(this.RadCentr);
            this.tabPage3.Location = new System.Drawing.Point(4, 25);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(474, 374);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Радио";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(298, 5);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(102, 17);
            this.label5.TabIndex = 8;
            this.label5.Text = "региональные";
            // 
            // RadReg
            // 
            this.RadReg.Location = new System.Drawing.Point(251, 25);
            this.RadReg.Multiline = true;
            this.RadReg.Name = "RadReg";
            this.RadReg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RadReg.Size = new System.Drawing.Size(204, 345);
            this.RadReg.TabIndex = 7;
            this.RadReg.TextChanged += new System.EventHandler(this.TvCentr_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(67, 5);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 17);
            this.label6.TabIndex = 6;
            this.label6.Text = "центральные";
            // 
            // RadCentr
            // 
            this.RadCentr.Location = new System.Drawing.Point(20, 25);
            this.RadCentr.Multiline = true;
            this.RadCentr.Name = "RadCentr";
            this.RadCentr.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.RadCentr.Size = new System.Drawing.Size(204, 345);
            this.RadCentr.TabIndex = 5;
            this.RadCentr.TextChanged += new System.EventHandler(this.TvCentr_TextChanged);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.internet);
            this.tabPage4.Location = new System.Drawing.Point(4, 25);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Size = new System.Drawing.Size(474, 374);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Интернет издания";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // internet
            // 
            this.internet.Location = new System.Drawing.Point(20, 21);
            this.internet.Multiline = true;
            this.internet.Name = "internet";
            this.internet.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.internet.Size = new System.Drawing.Size(434, 335);
            this.internet.TabIndex = 5;
            this.internet.TextChanged += new System.EventHandler(this.TvCentr_TextChanged);
            // 
            // page3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.tabControl1);
            this.Name = "page3";
            this.Size = new System.Drawing.Size(482, 403);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox TvCentr;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox TvReg;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox GazReg;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox GazCentr;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox RadReg;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox RadCentr;
        private System.Windows.Forms.TextBox internet;
    }
}
