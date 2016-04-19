namespace Pokemon_Shuffle_Save_Editor
{
    partial class Main
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
            Pokemon_Shuffle_Save_Editor.ShuffleItems shuffleItems1 = new Pokemon_Shuffle_Save_Editor.ShuffleItems();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Main));
            this.B_Open = new System.Windows.Forms.Button();
            this.TB_FilePath = new System.Windows.Forms.TextBox();
            this.B_Save = new System.Windows.Forms.Button();
            this.GB_HighScore = new System.Windows.Forms.GroupBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.NUP_EventScore = new System.Windows.Forms.NumericUpDown();
            this.NUP_EventIndex = new System.Windows.Forms.NumericUpDown();
            this.NUP_ExpertScore = new System.Windows.Forms.NumericUpDown();
            this.NUP_ExpertIndex = new System.Windows.Forms.NumericUpDown();
            this.NUP_MainScore = new System.Windows.Forms.NumericUpDown();
            this.NUP_MainIndex = new System.Windows.Forms.NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.PB_Event = new System.Windows.Forms.PictureBox();
            this.PB_Expert = new System.Windows.Forms.PictureBox();
            this.PB_Main = new System.Windows.Forms.PictureBox();
            this.GB_Caught = new System.Windows.Forms.GroupBox();
            this.PB_SpeedUpY = new System.Windows.Forms.PictureBox();
            this.PB_SpeedUpX = new System.Windows.Forms.PictureBox();
            this.NUP_SpeedUpY = new System.Windows.Forms.NumericUpDown();
            this.NUP_SpeedUpX = new System.Windows.Forms.NumericUpDown();
            this.PB_MegaX = new System.Windows.Forms.PictureBox();
            this.PB_MegaY = new System.Windows.Forms.PictureBox();
            this.CHK_MegaX = new System.Windows.Forms.CheckBox();
            this.CHK_MegaY = new System.Windows.Forms.CheckBox();
            this.NUP_Level = new System.Windows.Forms.NumericUpDown();
            this.CHK_CaughtMon = new System.Windows.Forms.CheckBox();
            this.CB_MonIndex = new System.Windows.Forms.ComboBox();
            this.PB_Mon = new System.Windows.Forms.PictureBox();
            this.GB_Resources = new System.Windows.Forms.GroupBox();
            this.ItemsGrid = new System.Windows.Forms.PropertyGrid();
            this.NUP_Jewels = new System.Windows.Forms.NumericUpDown();
            this.NUP_Coins = new System.Windows.Forms.NumericUpDown();
            this.NUP_Hearts = new System.Windows.Forms.NumericUpDown();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.B_CheatsForm = new System.Windows.Forms.Button();
            this.GB_HighScore.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_EventScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_EventIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_ExpertScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_ExpertIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_MainScore)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_MainIndex)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Event)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Expert)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Main)).BeginInit();
            this.GB_Caught.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_SpeedUpY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_SpeedUpX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_SpeedUpY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_SpeedUpX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MegaX)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MegaY)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Level)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Mon)).BeginInit();
            this.GB_Resources.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Jewels)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Coins)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Hearts)).BeginInit();
            this.SuspendLayout();
            // 
            // B_Open
            // 
            this.B_Open.Location = new System.Drawing.Point(12, 38);
            this.B_Open.Name = "B_Open";
            this.B_Open.Size = new System.Drawing.Size(228, 32);
            this.B_Open.TabIndex = 19;
            this.B_Open.Text = "Open savedata.bin";
            this.B_Open.UseVisualStyleBackColor = true;
            this.B_Open.Click += new System.EventHandler(this.B_Open_Click);
            // 
            // TB_FilePath
            // 
            this.TB_FilePath.Location = new System.Drawing.Point(12, 12);
            this.TB_FilePath.Name = "TB_FilePath";
            this.TB_FilePath.ReadOnly = true;
            this.TB_FilePath.Size = new System.Drawing.Size(228, 20);
            this.TB_FilePath.TabIndex = 20;
            // 
            // B_Save
            // 
            this.B_Save.Enabled = false;
            this.B_Save.Location = new System.Drawing.Point(12, 76);
            this.B_Save.Name = "B_Save";
            this.B_Save.Size = new System.Drawing.Size(228, 32);
            this.B_Save.TabIndex = 22;
            this.B_Save.Text = "Save savedata.bin";
            this.B_Save.UseVisualStyleBackColor = true;
            this.B_Save.Click += new System.EventHandler(this.B_Save_Click);
            // 
            // GB_HighScore
            // 
            this.GB_HighScore.Controls.Add(this.label10);
            this.GB_HighScore.Controls.Add(this.label9);
            this.GB_HighScore.Controls.Add(this.label8);
            this.GB_HighScore.Controls.Add(this.label6);
            this.GB_HighScore.Controls.Add(this.label5);
            this.GB_HighScore.Controls.Add(this.label4);
            this.GB_HighScore.Controls.Add(this.label3);
            this.GB_HighScore.Controls.Add(this.label2);
            this.GB_HighScore.Controls.Add(this.NUP_EventScore);
            this.GB_HighScore.Controls.Add(this.NUP_EventIndex);
            this.GB_HighScore.Controls.Add(this.NUP_ExpertScore);
            this.GB_HighScore.Controls.Add(this.NUP_ExpertIndex);
            this.GB_HighScore.Controls.Add(this.NUP_MainScore);
            this.GB_HighScore.Controls.Add(this.NUP_MainIndex);
            this.GB_HighScore.Controls.Add(this.label1);
            this.GB_HighScore.Controls.Add(this.PB_Event);
            this.GB_HighScore.Controls.Add(this.PB_Expert);
            this.GB_HighScore.Controls.Add(this.PB_Main);
            this.GB_HighScore.Enabled = false;
            this.GB_HighScore.Location = new System.Drawing.Point(12, 330);
            this.GB_HighScore.Name = "GB_HighScore";
            this.GB_HighScore.Size = new System.Drawing.Size(493, 118);
            this.GB_HighScore.TabIndex = 23;
            this.GB_HighScore.TabStop = false;
            this.GB_HighScore.Text = "High-Scores";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(394, 76);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Score:";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(234, 76);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 16;
            this.label9.Text = "Score:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(76, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(38, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Score:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(401, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Event Stages";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(237, 19);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 13;
            this.label5.Text = "Expert Stages";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(85, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "Main Stages";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(393, 45);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 11;
            this.label3.Text = "Index:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(234, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Index:";
            // 
            // NUP_EventScore
            // 
            this.NUP_EventScore.Location = new System.Drawing.Point(397, 92);
            this.NUP_EventScore.Name = "NUP_EventScore";
            this.NUP_EventScore.Size = new System.Drawing.Size(77, 20);
            this.NUP_EventScore.TabIndex = 9;
            this.NUP_EventScore.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_EventIndex
            // 
            this.NUP_EventIndex.Location = new System.Drawing.Point(435, 43);
            this.NUP_EventIndex.Name = "NUP_EventIndex";
            this.NUP_EventIndex.Size = new System.Drawing.Size(39, 20);
            this.NUP_EventIndex.TabIndex = 8;
            this.NUP_EventIndex.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_ExpertScore
            // 
            this.NUP_ExpertScore.Location = new System.Drawing.Point(240, 92);
            this.NUP_ExpertScore.Name = "NUP_ExpertScore";
            this.NUP_ExpertScore.Size = new System.Drawing.Size(77, 20);
            this.NUP_ExpertScore.TabIndex = 7;
            this.NUP_ExpertScore.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_ExpertIndex
            // 
            this.NUP_ExpertIndex.Location = new System.Drawing.Point(278, 43);
            this.NUP_ExpertIndex.Name = "NUP_ExpertIndex";
            this.NUP_ExpertIndex.Size = new System.Drawing.Size(39, 20);
            this.NUP_ExpertIndex.TabIndex = 6;
            this.NUP_ExpertIndex.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_MainScore
            // 
            this.NUP_MainScore.Location = new System.Drawing.Point(81, 92);
            this.NUP_MainScore.Name = "NUP_MainScore";
            this.NUP_MainScore.Size = new System.Drawing.Size(77, 20);
            this.NUP_MainScore.TabIndex = 5;
            this.NUP_MainScore.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_MainIndex
            // 
            this.NUP_MainIndex.Location = new System.Drawing.Point(117, 43);
            this.NUP_MainIndex.Name = "NUP_MainIndex";
            this.NUP_MainIndex.Size = new System.Drawing.Size(39, 20);
            this.NUP_MainIndex.TabIndex = 4;
            this.NUP_MainIndex.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(76, 45);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Index:";
            // 
            // PB_Event
            // 
            this.PB_Event.Location = new System.Drawing.Point(323, 32);
            this.PB_Event.Name = "PB_Event";
            this.PB_Event.Size = new System.Drawing.Size(64, 80);
            this.PB_Event.TabIndex = 2;
            this.PB_Event.TabStop = false;
            // 
            // PB_Expert
            // 
            this.PB_Expert.Location = new System.Drawing.Point(164, 32);
            this.PB_Expert.Name = "PB_Expert";
            this.PB_Expert.Size = new System.Drawing.Size(64, 80);
            this.PB_Expert.TabIndex = 1;
            this.PB_Expert.TabStop = false;
            // 
            // PB_Main
            // 
            this.PB_Main.Location = new System.Drawing.Point(6, 32);
            this.PB_Main.Name = "PB_Main";
            this.PB_Main.Size = new System.Drawing.Size(64, 80);
            this.PB_Main.TabIndex = 0;
            this.PB_Main.TabStop = false;
            // 
            // GB_Caught
            // 
            this.GB_Caught.Controls.Add(this.PB_SpeedUpY);
            this.GB_Caught.Controls.Add(this.PB_SpeedUpX);
            this.GB_Caught.Controls.Add(this.NUP_SpeedUpY);
            this.GB_Caught.Controls.Add(this.NUP_SpeedUpX);
            this.GB_Caught.Controls.Add(this.PB_MegaX);
            this.GB_Caught.Controls.Add(this.PB_MegaY);
            this.GB_Caught.Controls.Add(this.CHK_MegaX);
            this.GB_Caught.Controls.Add(this.CHK_MegaY);
            this.GB_Caught.Controls.Add(this.NUP_Level);
            this.GB_Caught.Controls.Add(this.CHK_CaughtMon);
            this.GB_Caught.Controls.Add(this.CB_MonIndex);
            this.GB_Caught.Controls.Add(this.PB_Mon);
            this.GB_Caught.Enabled = false;
            this.GB_Caught.Location = new System.Drawing.Point(12, 114);
            this.GB_Caught.Name = "GB_Caught";
            this.GB_Caught.Size = new System.Drawing.Size(228, 125);
            this.GB_Caught.TabIndex = 24;
            this.GB_Caught.TabStop = false;
            this.GB_Caught.Text = "Owned Pokemon";
            // 
            // PB_SpeedUpY
            // 
            this.PB_SpeedUpY.Location = new System.Drawing.Point(198, 97);
            this.PB_SpeedUpY.Name = "PB_SpeedUpY";
            this.PB_SpeedUpY.Size = new System.Drawing.Size(24, 24);
            this.PB_SpeedUpY.TabIndex = 27;
            this.PB_SpeedUpY.TabStop = false;
            // 
            // PB_SpeedUpX
            // 
            this.PB_SpeedUpX.Location = new System.Drawing.Point(124, 97);
            this.PB_SpeedUpX.Name = "PB_SpeedUpX";
            this.PB_SpeedUpX.Size = new System.Drawing.Size(24, 24);
            this.PB_SpeedUpX.TabIndex = 26;
            this.PB_SpeedUpX.TabStop = false;
            // 
            // NUP_SpeedUpY
            // 
            this.NUP_SpeedUpY.Location = new System.Drawing.Point(160, 99);
            this.NUP_SpeedUpY.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NUP_SpeedUpY.Name = "NUP_SpeedUpY";
            this.NUP_SpeedUpY.Size = new System.Drawing.Size(37, 20);
            this.NUP_SpeedUpY.TabIndex = 25;
            this.NUP_SpeedUpY.Visible = false;
            this.NUP_SpeedUpY.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_SpeedUpX
            // 
            this.NUP_SpeedUpX.Location = new System.Drawing.Point(86, 99);
            this.NUP_SpeedUpX.Maximum = new decimal(new int[] {
            127,
            0,
            0,
            0});
            this.NUP_SpeedUpX.Name = "NUP_SpeedUpX";
            this.NUP_SpeedUpX.Size = new System.Drawing.Size(37, 20);
            this.NUP_SpeedUpX.TabIndex = 24;
            this.NUP_SpeedUpX.Visible = false;
            this.NUP_SpeedUpX.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // PB_MegaX
            // 
            this.PB_MegaX.Location = new System.Drawing.Point(86, 77);
            this.PB_MegaX.Name = "PB_MegaX";
            this.PB_MegaX.Size = new System.Drawing.Size(16, 16);
            this.PB_MegaX.TabIndex = 23;
            this.PB_MegaX.TabStop = false;
            // 
            // PB_MegaY
            // 
            this.PB_MegaY.Location = new System.Drawing.Point(160, 77);
            this.PB_MegaY.Name = "PB_MegaY";
            this.PB_MegaY.Size = new System.Drawing.Size(16, 16);
            this.PB_MegaY.TabIndex = 22;
            this.PB_MegaY.TabStop = false;
            // 
            // CHK_MegaX
            // 
            this.CHK_MegaX.AutoSize = true;
            this.CHK_MegaX.Location = new System.Drawing.Point(108, 79);
            this.CHK_MegaX.Name = "CHK_MegaX";
            this.CHK_MegaX.Size = new System.Drawing.Size(15, 14);
            this.CHK_MegaX.TabIndex = 21;
            this.CHK_MegaX.UseVisualStyleBackColor = true;
            this.CHK_MegaX.CheckedChanged += new System.EventHandler(this.UpdateForm);
            // 
            // CHK_MegaY
            // 
            this.CHK_MegaY.AutoSize = true;
            this.CHK_MegaY.Cursor = System.Windows.Forms.Cursors.Default;
            this.CHK_MegaY.Location = new System.Drawing.Point(182, 79);
            this.CHK_MegaY.Name = "CHK_MegaY";
            this.CHK_MegaY.Size = new System.Drawing.Size(15, 14);
            this.CHK_MegaY.TabIndex = 20;
            this.CHK_MegaY.UseVisualStyleBackColor = true;
            this.CHK_MegaY.CheckedChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_Level
            // 
            this.NUP_Level.Location = new System.Drawing.Point(6, 99);
            this.NUP_Level.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.NUP_Level.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Level.Name = "NUP_Level";
            this.NUP_Level.Size = new System.Drawing.Size(64, 20);
            this.NUP_Level.TabIndex = 19;
            this.NUP_Level.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Level.Visible = false;
            this.NUP_Level.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // CHK_CaughtMon
            // 
            this.CHK_CaughtMon.AutoSize = true;
            this.CHK_CaughtMon.Location = new System.Drawing.Point(88, 56);
            this.CHK_CaughtMon.Name = "CHK_CaughtMon";
            this.CHK_CaughtMon.Size = new System.Drawing.Size(60, 17);
            this.CHK_CaughtMon.TabIndex = 17;
            this.CHK_CaughtMon.Text = "Caught";
            this.CHK_CaughtMon.UseVisualStyleBackColor = true;
            this.CHK_CaughtMon.CheckedChanged += new System.EventHandler(this.UpdateForm);
            // 
            // CB_MonIndex
            // 
            this.CB_MonIndex.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CB_MonIndex.FormattingEnabled = true;
            this.CB_MonIndex.Location = new System.Drawing.Point(88, 29);
            this.CB_MonIndex.Name = "CB_MonIndex";
            this.CB_MonIndex.Size = new System.Drawing.Size(121, 21);
            this.CB_MonIndex.TabIndex = 16;
            this.CB_MonIndex.SelectedValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // PB_Mon
            // 
            this.PB_Mon.Location = new System.Drawing.Point(6, 29);
            this.PB_Mon.Name = "PB_Mon";
            this.PB_Mon.Size = new System.Drawing.Size(64, 64);
            this.PB_Mon.TabIndex = 15;
            this.PB_Mon.TabStop = false;
            // 
            // GB_Resources
            // 
            this.GB_Resources.Controls.Add(this.ItemsGrid);
            this.GB_Resources.Controls.Add(this.NUP_Jewels);
            this.GB_Resources.Controls.Add(this.NUP_Coins);
            this.GB_Resources.Controls.Add(this.NUP_Hearts);
            this.GB_Resources.Controls.Add(this.label13);
            this.GB_Resources.Controls.Add(this.label12);
            this.GB_Resources.Controls.Add(this.label11);
            this.GB_Resources.Enabled = false;
            this.GB_Resources.Location = new System.Drawing.Point(252, 6);
            this.GB_Resources.Name = "GB_Resources";
            this.GB_Resources.Size = new System.Drawing.Size(253, 318);
            this.GB_Resources.TabIndex = 25;
            this.GB_Resources.TabStop = false;
            this.GB_Resources.Text = "Resources";
            // 
            // ItemsGrid
            // 
            this.ItemsGrid.Location = new System.Drawing.Point(6, 77);
            this.ItemsGrid.Name = "ItemsGrid";
            shuffleItems1.AttackUp = 0;
            shuffleItems1.Complexity = 0;
            shuffleItems1.Disruption = 0;
            shuffleItems1.Enchantments = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            shuffleItems1.Experience = 0;
            shuffleItems1.ExperienceBoostL = 0;
            shuffleItems1.ExperienceBoostM = 0;
            shuffleItems1.ExperienceBoostS = 0;
            shuffleItems1.Items = new int[] {
        0,
        0,
        0,
        0,
        0,
        0,
        0};
            shuffleItems1.LevelUp = 0;
            shuffleItems1.MegaSpeedup = 0;
            shuffleItems1.MegaStart = 0;
            shuffleItems1.Moves = 0;
            shuffleItems1.RaiseMaxLevel = 0;
            shuffleItems1.SkillBoosterL = 0;
            shuffleItems1.SkillBoosterM = 0;
            shuffleItems1.SkillBoosterS = 0;
            shuffleItems1.Time = 0;
            this.ItemsGrid.SelectedObject = shuffleItems1;
            this.ItemsGrid.Size = new System.Drawing.Size(241, 235);
            this.ItemsGrid.TabIndex = 27;
            this.ItemsGrid.ToolbarVisible = false;
            this.ItemsGrid.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.UpdateProperty);
            this.ItemsGrid.EnabledChanged += new System.EventHandler(this.ItemsGrid_EnabledChanged);
            // 
            // NUP_Jewels
            // 
            this.NUP_Jewels.Location = new System.Drawing.Point(190, 51);
            this.NUP_Jewels.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.NUP_Jewels.Name = "NUP_Jewels";
            this.NUP_Jewels.Size = new System.Drawing.Size(52, 20);
            this.NUP_Jewels.TabIndex = 6;
            this.NUP_Jewels.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_Coins
            // 
            this.NUP_Coins.Location = new System.Drawing.Point(89, 51);
            this.NUP_Coins.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.NUP_Coins.Name = "NUP_Coins";
            this.NUP_Coins.Size = new System.Drawing.Size(67, 20);
            this.NUP_Coins.TabIndex = 5;
            this.NUP_Coins.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // NUP_Hearts
            // 
            this.NUP_Hearts.Location = new System.Drawing.Point(20, 51);
            this.NUP_Hearts.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.NUP_Hearts.Name = "NUP_Hearts";
            this.NUP_Hearts.Size = new System.Drawing.Size(43, 20);
            this.NUP_Hearts.TabIndex = 3;
            this.NUP_Hearts.ValueChanged += new System.EventHandler(this.UpdateForm);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(196, 33);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(39, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "Jewels";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(106, 32);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(33, 13);
            this.label12.TabIndex = 1;
            this.label12.Text = "Coins";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(23, 31);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(38, 13);
            this.label11.TabIndex = 0;
            this.label11.Text = "Hearts";
            // 
            // B_CheatsForm
            // 
            this.B_CheatsForm.Enabled = false;
            this.B_CheatsForm.Location = new System.Drawing.Point(12, 245);
            this.B_CheatsForm.Name = "B_CheatsForm";
            this.B_CheatsForm.Size = new System.Drawing.Size(109, 47);
            this.B_CheatsForm.TabIndex = 26;
            this.B_CheatsForm.Text = "Bulk Edits";
            this.B_CheatsForm.UseVisualStyleBackColor = true;
            this.B_CheatsForm.Click += new System.EventHandler(this.B_CheatsForm_Click);
            // 
            // Main
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(519, 454);
            this.Controls.Add(this.B_CheatsForm);
            this.Controls.Add(this.GB_Resources);
            this.Controls.Add(this.GB_Caught);
            this.Controls.Add(this.GB_HighScore);
            this.Controls.Add(this.B_Save);
            this.Controls.Add(this.TB_FilePath);
            this.Controls.Add(this.B_Open);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(535, 493);
            this.MinimumSize = new System.Drawing.Size(535, 415);
            this.Name = "Main";
            this.Text = "Pokemon Shuffle Save Editor";
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.Form1_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.Form1_DragEnter);
            this.GB_HighScore.ResumeLayout(false);
            this.GB_HighScore.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_EventScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_EventIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_ExpertScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_ExpertIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_MainScore)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_MainIndex)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Event)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Expert)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Main)).EndInit();
            this.GB_Caught.ResumeLayout(false);
            this.GB_Caught.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PB_SpeedUpY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_SpeedUpX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_SpeedUpY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_SpeedUpX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MegaX)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_MegaY)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Level)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Mon)).EndInit();
            this.GB_Resources.ResumeLayout(false);
            this.GB_Resources.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Jewels)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Coins)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Hearts)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button B_Open;
        private System.Windows.Forms.TextBox TB_FilePath;
        private System.Windows.Forms.Button B_Save;
        private System.Windows.Forms.GroupBox GB_HighScore;
        private System.Windows.Forms.PictureBox PB_Main;
        private System.Windows.Forms.PictureBox PB_Event;
        private System.Windows.Forms.PictureBox PB_Expert;
        private System.Windows.Forms.GroupBox GB_Caught;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown NUP_MainIndex;
        private System.Windows.Forms.NumericUpDown NUP_MainScore;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown NUP_EventScore;
        private System.Windows.Forms.NumericUpDown NUP_EventIndex;
        private System.Windows.Forms.NumericUpDown NUP_ExpertScore;
        private System.Windows.Forms.NumericUpDown NUP_ExpertIndex;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.PictureBox PB_Mon;
        private System.Windows.Forms.ComboBox CB_MonIndex;
        private System.Windows.Forms.CheckBox CHK_CaughtMon;
        private System.Windows.Forms.NumericUpDown NUP_Level;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox GB_Resources;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown NUP_Hearts;
        private System.Windows.Forms.NumericUpDown NUP_Jewels;
        private System.Windows.Forms.NumericUpDown NUP_Coins;
        private System.Windows.Forms.PictureBox PB_MegaX;
        private System.Windows.Forms.PictureBox PB_MegaY;
        private System.Windows.Forms.CheckBox CHK_MegaX;
        private System.Windows.Forms.CheckBox CHK_MegaY;
        private System.Windows.Forms.Button B_CheatsForm;
        private System.Windows.Forms.PropertyGrid ItemsGrid;
        private System.Windows.Forms.NumericUpDown NUP_SpeedUpX;
        private System.Windows.Forms.NumericUpDown NUP_SpeedUpY;
        private System.Windows.Forms.PictureBox PB_SpeedUpX;
        private System.Windows.Forms.PictureBox PB_SpeedUpY;
    }
}

