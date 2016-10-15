namespace Pokemon_Shuffle_Save_Editor
{
    partial class Mission_Popup
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
            this.NUP_Mission = new System.Windows.Forms.NumericUpDown();
            this.L_Top = new System.Windows.Forms.Label();
            this.GB_States = new System.Windows.Forms.GroupBox();
            this.CHK_10 = new System.Windows.Forms.CheckBox();
            this.CHK_09 = new System.Windows.Forms.CheckBox();
            this.CHK_08 = new System.Windows.Forms.CheckBox();
            this.CHK_07 = new System.Windows.Forms.CheckBox();
            this.CHK_06 = new System.Windows.Forms.CheckBox();
            this.CHK_05 = new System.Windows.Forms.CheckBox();
            this.CHK_04 = new System.Windows.Forms.CheckBox();
            this.CHK_03 = new System.Windows.Forms.CheckBox();
            this.CHK_02 = new System.Windows.Forms.CheckBox();
            this.CHK_01 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.L_Active = new System.Windows.Forms.Label();
            this.NUP_Active = new System.Windows.Forms.NumericUpDown();
            this.B_Erase = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Mission)).BeginInit();
            this.GB_States.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Active)).BeginInit();
            this.SuspendLayout();
            // 
            // NUP_Mission
            // 
            this.NUP_Mission.Location = new System.Drawing.Point(122, 12);
            this.NUP_Mission.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NUP_Mission.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Mission.Name = "NUP_Mission";
            this.NUP_Mission.Size = new System.Drawing.Size(43, 20);
            this.NUP_Mission.TabIndex = 0;
            this.NUP_Mission.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Mission.ValueChanged += new System.EventHandler(this.NUP_Mission_ValueChanged);
            // 
            // L_Top
            // 
            this.L_Top.AutoSize = true;
            this.L_Top.Location = new System.Drawing.Point(12, 14);
            this.L_Top.Name = "L_Top";
            this.L_Top.Size = new System.Drawing.Size(104, 13);
            this.L_Top.TabIndex = 1;
            this.L_Top.Text = "Edit Mission Card # :";
            // 
            // GB_States
            // 
            this.GB_States.Controls.Add(this.CHK_10);
            this.GB_States.Controls.Add(this.CHK_09);
            this.GB_States.Controls.Add(this.CHK_08);
            this.GB_States.Controls.Add(this.CHK_07);
            this.GB_States.Controls.Add(this.CHK_06);
            this.GB_States.Controls.Add(this.CHK_05);
            this.GB_States.Controls.Add(this.CHK_04);
            this.GB_States.Controls.Add(this.CHK_03);
            this.GB_States.Controls.Add(this.CHK_02);
            this.GB_States.Controls.Add(this.CHK_01);
            this.GB_States.Location = new System.Drawing.Point(12, 38);
            this.GB_States.Name = "GB_States";
            this.GB_States.Size = new System.Drawing.Size(260, 101);
            this.GB_States.TabIndex = 2;
            this.GB_States.TabStop = false;
            this.GB_States.Text = "Missions state";
            // 
            // CHK_10
            // 
            this.CHK_10.AutoSize = true;
            this.CHK_10.Location = new System.Drawing.Point(216, 75);
            this.CHK_10.Name = "CHK_10";
            this.CHK_10.Size = new System.Drawing.Size(38, 17);
            this.CHK_10.TabIndex = 12;
            this.CHK_10.Text = "10";
            this.CHK_10.UseVisualStyleBackColor = true;
            this.CHK_10.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_09
            // 
            this.CHK_09.AutoSize = true;
            this.CHK_09.Location = new System.Drawing.Point(168, 75);
            this.CHK_09.Name = "CHK_09";
            this.CHK_09.Size = new System.Drawing.Size(38, 17);
            this.CHK_09.TabIndex = 11;
            this.CHK_09.Text = "09";
            this.CHK_09.UseVisualStyleBackColor = true;
            this.CHK_09.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_08
            // 
            this.CHK_08.AutoSize = true;
            this.CHK_08.Location = new System.Drawing.Point(115, 75);
            this.CHK_08.Name = "CHK_08";
            this.CHK_08.Size = new System.Drawing.Size(38, 17);
            this.CHK_08.TabIndex = 10;
            this.CHK_08.Text = "08";
            this.CHK_08.UseVisualStyleBackColor = true;
            this.CHK_08.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_07
            // 
            this.CHK_07.AutoSize = true;
            this.CHK_07.Location = new System.Drawing.Point(66, 75);
            this.CHK_07.Name = "CHK_07";
            this.CHK_07.Size = new System.Drawing.Size(38, 17);
            this.CHK_07.TabIndex = 9;
            this.CHK_07.Text = "07";
            this.CHK_07.UseVisualStyleBackColor = true;
            this.CHK_07.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_06
            // 
            this.CHK_06.AutoSize = true;
            this.CHK_06.Location = new System.Drawing.Point(6, 75);
            this.CHK_06.Name = "CHK_06";
            this.CHK_06.Size = new System.Drawing.Size(38, 17);
            this.CHK_06.TabIndex = 8;
            this.CHK_06.Text = "06";
            this.CHK_06.UseVisualStyleBackColor = true;
            this.CHK_06.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_05
            // 
            this.CHK_05.AutoSize = true;
            this.CHK_05.Location = new System.Drawing.Point(216, 19);
            this.CHK_05.Name = "CHK_05";
            this.CHK_05.Size = new System.Drawing.Size(38, 17);
            this.CHK_05.TabIndex = 7;
            this.CHK_05.Text = "05";
            this.CHK_05.UseVisualStyleBackColor = true;
            this.CHK_05.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_04
            // 
            this.CHK_04.AutoSize = true;
            this.CHK_04.Location = new System.Drawing.Point(168, 19);
            this.CHK_04.Name = "CHK_04";
            this.CHK_04.Size = new System.Drawing.Size(38, 17);
            this.CHK_04.TabIndex = 6;
            this.CHK_04.Text = "04";
            this.CHK_04.UseVisualStyleBackColor = true;
            this.CHK_04.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_03
            // 
            this.CHK_03.AutoSize = true;
            this.CHK_03.Location = new System.Drawing.Point(115, 19);
            this.CHK_03.Name = "CHK_03";
            this.CHK_03.Size = new System.Drawing.Size(38, 17);
            this.CHK_03.TabIndex = 5;
            this.CHK_03.Text = "03";
            this.CHK_03.UseVisualStyleBackColor = true;
            this.CHK_03.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_02
            // 
            this.CHK_02.AutoSize = true;
            this.CHK_02.Location = new System.Drawing.Point(66, 19);
            this.CHK_02.Name = "CHK_02";
            this.CHK_02.Size = new System.Drawing.Size(38, 17);
            this.CHK_02.TabIndex = 4;
            this.CHK_02.Text = "02";
            this.CHK_02.UseVisualStyleBackColor = true;
            this.CHK_02.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CHK_01
            // 
            this.CHK_01.AutoSize = true;
            this.CHK_01.Location = new System.Drawing.Point(6, 19);
            this.CHK_01.Name = "CHK_01";
            this.CHK_01.Size = new System.Drawing.Size(38, 17);
            this.CHK_01.TabIndex = 3;
            this.CHK_01.Text = "01";
            this.CHK_01.UseVisualStyleBackColor = true;
            this.CHK_01.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.Location = new System.Drawing.Point(197, 142);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // L_Active
            // 
            this.L_Active.AutoSize = true;
            this.L_Active.Location = new System.Drawing.Point(12, 147);
            this.L_Active.Name = "L_Active";
            this.L_Active.Size = new System.Drawing.Size(106, 13);
            this.L_Active.TabIndex = 5;
            this.L_Active.Text = "Active Mission Card :";
            // 
            // NUP_Active
            // 
            this.NUP_Active.Location = new System.Drawing.Point(122, 145);
            this.NUP_Active.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NUP_Active.Name = "NUP_Active";
            this.NUP_Active.Size = new System.Drawing.Size(43, 20);
            this.NUP_Active.TabIndex = 4;
            // 
            // B_Erase
            // 
            this.B_Erase.AutoSize = true;
            this.B_Erase.Location = new System.Drawing.Point(197, 9);
            this.B_Erase.Name = "B_Erase";
            this.B_Erase.Size = new System.Drawing.Size(75, 23);
            this.B_Erase.TabIndex = 6;
            this.B_Erase.Text = "Erase All";
            this.B_Erase.UseVisualStyleBackColor = true;
            this.B_Erase.Click += new System.EventHandler(this.B_Erase_Click);
            // 
            // Mission_Popup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 174);
            this.Controls.Add(this.B_Erase);
            this.Controls.Add(this.L_Active);
            this.Controls.Add(this.NUP_Active);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.GB_States);
            this.Controls.Add(this.L_Top);
            this.Controls.Add(this.NUP_Mission);
            this.MaximumSize = new System.Drawing.Size(300, 213);
            this.MinimumSize = new System.Drawing.Size(300, 213);
            this.Name = "Mission_Popup";
            this.Text = "Mission_Popup";
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Mission)).EndInit();
            this.GB_States.ResumeLayout(false);
            this.GB_States.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Active)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NumericUpDown NUP_Mission;
        private System.Windows.Forms.Label L_Top;
        private System.Windows.Forms.GroupBox GB_States;
        private System.Windows.Forms.CheckBox CHK_01;
        private System.Windows.Forms.CheckBox CHK_10;
        private System.Windows.Forms.CheckBox CHK_09;
        private System.Windows.Forms.CheckBox CHK_08;
        private System.Windows.Forms.CheckBox CHK_07;
        private System.Windows.Forms.CheckBox CHK_06;
        private System.Windows.Forms.CheckBox CHK_05;
        private System.Windows.Forms.CheckBox CHK_04;
        private System.Windows.Forms.CheckBox CHK_03;
        private System.Windows.Forms.CheckBox CHK_02;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label L_Active;
        private System.Windows.Forms.NumericUpDown NUP_Active;
        private System.Windows.Forms.Button B_Erase;
    }
}