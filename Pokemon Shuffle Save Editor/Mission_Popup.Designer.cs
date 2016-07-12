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
            this.CB_10 = new System.Windows.Forms.CheckBox();
            this.CB_09 = new System.Windows.Forms.CheckBox();
            this.CB_08 = new System.Windows.Forms.CheckBox();
            this.CB_07 = new System.Windows.Forms.CheckBox();
            this.CB_06 = new System.Windows.Forms.CheckBox();
            this.CB_05 = new System.Windows.Forms.CheckBox();
            this.CB_04 = new System.Windows.Forms.CheckBox();
            this.CB_03 = new System.Windows.Forms.CheckBox();
            this.CB_02 = new System.Windows.Forms.CheckBox();
            this.CB_01 = new System.Windows.Forms.CheckBox();
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
            this.GB_States.Controls.Add(this.CB_10);
            this.GB_States.Controls.Add(this.CB_09);
            this.GB_States.Controls.Add(this.CB_08);
            this.GB_States.Controls.Add(this.CB_07);
            this.GB_States.Controls.Add(this.CB_06);
            this.GB_States.Controls.Add(this.CB_05);
            this.GB_States.Controls.Add(this.CB_04);
            this.GB_States.Controls.Add(this.CB_03);
            this.GB_States.Controls.Add(this.CB_02);
            this.GB_States.Controls.Add(this.CB_01);
            this.GB_States.Location = new System.Drawing.Point(12, 38);
            this.GB_States.Name = "GB_States";
            this.GB_States.Size = new System.Drawing.Size(260, 101);
            this.GB_States.TabIndex = 2;
            this.GB_States.TabStop = false;
            this.GB_States.Text = "Missions state";
            // 
            // CB_10
            // 
            this.CB_10.AutoSize = true;
            this.CB_10.Location = new System.Drawing.Point(216, 75);
            this.CB_10.Name = "CB_10";
            this.CB_10.Size = new System.Drawing.Size(38, 17);
            this.CB_10.TabIndex = 12;
            this.CB_10.Text = "10";
            this.CB_10.UseVisualStyleBackColor = true;
            this.CB_10.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_09
            // 
            this.CB_09.AutoSize = true;
            this.CB_09.Location = new System.Drawing.Point(168, 75);
            this.CB_09.Name = "CB_09";
            this.CB_09.Size = new System.Drawing.Size(38, 17);
            this.CB_09.TabIndex = 11;
            this.CB_09.Text = "09";
            this.CB_09.UseVisualStyleBackColor = true;
            this.CB_09.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_08
            // 
            this.CB_08.AutoSize = true;
            this.CB_08.Location = new System.Drawing.Point(115, 75);
            this.CB_08.Name = "CB_08";
            this.CB_08.Size = new System.Drawing.Size(38, 17);
            this.CB_08.TabIndex = 10;
            this.CB_08.Text = "08";
            this.CB_08.UseVisualStyleBackColor = true;
            this.CB_08.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_07
            // 
            this.CB_07.AutoSize = true;
            this.CB_07.Location = new System.Drawing.Point(66, 75);
            this.CB_07.Name = "CB_07";
            this.CB_07.Size = new System.Drawing.Size(38, 17);
            this.CB_07.TabIndex = 9;
            this.CB_07.Text = "07";
            this.CB_07.UseVisualStyleBackColor = true;
            this.CB_07.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_06
            // 
            this.CB_06.AutoSize = true;
            this.CB_06.Location = new System.Drawing.Point(6, 75);
            this.CB_06.Name = "CB_06";
            this.CB_06.Size = new System.Drawing.Size(38, 17);
            this.CB_06.TabIndex = 8;
            this.CB_06.Text = "06";
            this.CB_06.UseVisualStyleBackColor = true;
            this.CB_06.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_05
            // 
            this.CB_05.AutoSize = true;
            this.CB_05.Location = new System.Drawing.Point(216, 19);
            this.CB_05.Name = "CB_05";
            this.CB_05.Size = new System.Drawing.Size(38, 17);
            this.CB_05.TabIndex = 7;
            this.CB_05.Text = "05";
            this.CB_05.UseVisualStyleBackColor = true;
            this.CB_05.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_04
            // 
            this.CB_04.AutoSize = true;
            this.CB_04.Location = new System.Drawing.Point(168, 19);
            this.CB_04.Name = "CB_04";
            this.CB_04.Size = new System.Drawing.Size(38, 17);
            this.CB_04.TabIndex = 6;
            this.CB_04.Text = "04";
            this.CB_04.UseVisualStyleBackColor = true;
            this.CB_04.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_03
            // 
            this.CB_03.AutoSize = true;
            this.CB_03.Location = new System.Drawing.Point(115, 19);
            this.CB_03.Name = "CB_03";
            this.CB_03.Size = new System.Drawing.Size(38, 17);
            this.CB_03.TabIndex = 5;
            this.CB_03.Text = "03";
            this.CB_03.UseVisualStyleBackColor = true;
            this.CB_03.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_02
            // 
            this.CB_02.AutoSize = true;
            this.CB_02.Location = new System.Drawing.Point(66, 19);
            this.CB_02.Name = "CB_02";
            this.CB_02.Size = new System.Drawing.Size(38, 17);
            this.CB_02.TabIndex = 4;
            this.CB_02.Text = "02";
            this.CB_02.UseVisualStyleBackColor = true;
            this.CB_02.CheckedChanged += new System.EventHandler(this.CheckedChanged);
            // 
            // CB_01
            // 
            this.CB_01.AutoSize = true;
            this.CB_01.Location = new System.Drawing.Point(6, 19);
            this.CB_01.Name = "CB_01";
            this.CB_01.Size = new System.Drawing.Size(38, 17);
            this.CB_01.TabIndex = 3;
            this.CB_01.Text = "01";
            this.CB_01.UseVisualStyleBackColor = true;
            this.CB_01.CheckedChanged += new System.EventHandler(this.CheckedChanged);
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
        private System.Windows.Forms.CheckBox CB_01;
        private System.Windows.Forms.CheckBox CB_10;
        private System.Windows.Forms.CheckBox CB_09;
        private System.Windows.Forms.CheckBox CB_08;
        private System.Windows.Forms.CheckBox CB_07;
        private System.Windows.Forms.CheckBox CB_06;
        private System.Windows.Forms.CheckBox CB_05;
        private System.Windows.Forms.CheckBox CB_04;
        private System.Windows.Forms.CheckBox CB_03;
        private System.Windows.Forms.CheckBox CB_02;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label L_Active;
        private System.Windows.Forms.NumericUpDown NUP_Active;
        private System.Windows.Forms.Button B_Erase;
    }
}