namespace Pokemon_Shuffle_Save_Editor
{
    partial class Cheats
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Cheats));
            this.B_CaughtEverything = new System.Windows.Forms.Button();
            this.B_LevelMax = new System.Windows.Forms.Button();
            this.B_CaughtObtainables = new System.Windows.Forms.Button();
            this.B_MaxResources = new System.Windows.Forms.Button();
            this.B_AllStones = new System.Windows.Forms.Button();
            this.B_AllCaughtStones = new System.Windows.Forms.Button();
            this.B_MaxSpeedups = new System.Windows.Forms.Button();
            this.B_AllCompleted = new System.Windows.Forms.Button();
            this.B_SRankCompleted = new System.Windows.Forms.Button();
            this.B_StreetPassDelete = new System.Windows.Forms.Button();
            this.B_MaxExcalationBattle = new System.Windows.Forms.Button();
            this.Line = new System.Windows.Forms.Label();
            this.B_PokemonReset = new System.Windows.Forms.Button();
            this.B_StageReset = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.B_MaxTalent = new System.Windows.Forms.Button();
            this.B_Test = new System.Windows.Forms.Button();
            this.B_PokathlonStep = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // B_CaughtEverything
            // 
            this.B_CaughtEverything.Location = new System.Drawing.Point(196, 157);
            this.B_CaughtEverything.Name = "B_CaughtEverything";
            this.B_CaughtEverything.Size = new System.Drawing.Size(65, 23);
            this.B_CaughtEverything.TabIndex = 101;
            this.B_CaughtEverything.Text = "Caught All Pokemon";
            this.toolTip1.SetToolTip(this.B_CaughtEverything, "Marks all pokemon in the game\'s database as caught. \r\n\r\nResearchers only.");
            this.B_CaughtEverything.UseVisualStyleBackColor = true;
            this.B_CaughtEverything.Visible = false;
            this.B_CaughtEverything.Click += new System.EventHandler(this.B_CaughtEverything_Click);
            // 
            // B_LevelMax
            // 
            this.B_LevelMax.Location = new System.Drawing.Point(156, 12);
            this.B_LevelMax.Name = "B_LevelMax";
            this.B_LevelMax.Size = new System.Drawing.Size(138, 23);
            this.B_LevelMax.TabIndex = 1;
            this.B_LevelMax.Text = "All Owned Lv (Max)";
            this.toolTip1.SetToolTip(this.B_LevelMax, resources.GetString("B_LevelMax.ToolTip"));
            this.B_LevelMax.UseVisualStyleBackColor = true;
            this.B_LevelMax.Click += new System.EventHandler(this.B_LevelMax_Click);
            // 
            // B_CaughtObtainables
            // 
            this.B_CaughtObtainables.Location = new System.Drawing.Point(12, 12);
            this.B_CaughtObtainables.Name = "B_CaughtObtainables";
            this.B_CaughtObtainables.Size = new System.Drawing.Size(138, 23);
            this.B_CaughtObtainables.TabIndex = 0;
            this.B_CaughtObtainables.Text = "Catch All Obtainables";
            this.toolTip1.SetToolTip(this.B_CaughtObtainables, resources.GetString("B_CaughtObtainables.ToolTip"));
            this.B_CaughtObtainables.UseVisualStyleBackColor = true;
            this.B_CaughtObtainables.Click += new System.EventHandler(this.B_CaughtObtainables_Click);
            // 
            // B_MaxResources
            // 
            this.B_MaxResources.Location = new System.Drawing.Point(156, 99);
            this.B_MaxResources.Name = "B_MaxResources";
            this.B_MaxResources.Size = new System.Drawing.Size(138, 23);
            this.B_MaxResources.TabIndex = 7;
            this.B_MaxResources.Text = "(Maximum) Resources";
            this.toolTip1.SetToolTip(this.B_MaxResources, "Will give you 99 stock hearts, 99.999 coins, 150 jewels, 99 of all \"in-battle\" it" +
        "ems\r\n& 99 of all \"enhancements\" items.");
            this.B_MaxResources.UseVisualStyleBackColor = true;
            this.B_MaxResources.Click += new System.EventHandler(this.B_MaxResources_Click);
            // 
            // B_AllStones
            // 
            this.B_AllStones.Location = new System.Drawing.Point(156, 157);
            this.B_AllStones.Name = "B_AllStones";
            this.B_AllStones.Size = new System.Drawing.Size(65, 23);
            this.B_AllStones.TabIndex = 100;
            this.B_AllStones.Text = "All Stones";
            this.toolTip1.SetToolTip(this.B_AllStones, "Gives you all megastones currently in the game\'s database.\r\n\r\nFor research only.");
            this.B_AllStones.UseVisualStyleBackColor = true;
            this.B_AllStones.Visible = false;
            this.B_AllStones.Click += new System.EventHandler(this.B_AllStones_Click);
            // 
            // B_AllCaughtStones
            // 
            this.B_AllCaughtStones.Location = new System.Drawing.Point(156, 41);
            this.B_AllCaughtStones.Name = "B_AllCaughtStones";
            this.B_AllCaughtStones.Size = new System.Drawing.Size(138, 23);
            this.B_AllCaughtStones.TabIndex = 3;
            this.B_AllCaughtStones.Text = "(All Caught) Megastones";
            this.toolTip1.SetToolTip(this.B_AllCaughtStones, resources.GetString("B_AllCaughtStones.ToolTip"));
            this.B_AllCaughtStones.UseVisualStyleBackColor = true;
            this.B_AllCaughtStones.Click += new System.EventHandler(this.B_AllCaughtStones_Click);
            // 
            // B_MaxSpeedups
            // 
            this.B_MaxSpeedups.Location = new System.Drawing.Point(12, 70);
            this.B_MaxSpeedups.Name = "B_MaxSpeedups";
            this.B_MaxSpeedups.Size = new System.Drawing.Size(138, 23);
            this.B_MaxSpeedups.TabIndex = 4;
            this.B_MaxSpeedups.Text = "All Owned (Max) Speedup";
            this.toolTip1.SetToolTip(this.B_MaxSpeedups, resources.GetString("B_MaxSpeedups.ToolTip"));
            this.B_MaxSpeedups.UseVisualStyleBackColor = true;
            this.B_MaxSpeedups.Click += new System.EventHandler(this.B_MaxSpeedups_Click);
            // 
            // B_AllCompleted
            // 
            this.B_AllCompleted.Location = new System.Drawing.Point(156, 70);
            this.B_AllCompleted.Name = "B_AllCompleted";
            this.B_AllCompleted.Size = new System.Drawing.Size(138, 23);
            this.B_AllCompleted.TabIndex = 5;
            this.B_AllCompleted.Text = "Complete All Stages";
            this.toolTip1.SetToolTip(this.B_AllCompleted, resources.GetString("B_AllCompleted.ToolTip"));
            this.B_AllCompleted.UseVisualStyleBackColor = true;
            this.B_AllCompleted.Click += new System.EventHandler(this.B_AllCompleted_Click);
            // 
            // B_SRankCompleted
            // 
            this.B_SRankCompleted.Location = new System.Drawing.Point(12, 99);
            this.B_SRankCompleted.Name = "B_SRankCompleted";
            this.B_SRankCompleted.Size = new System.Drawing.Size(138, 23);
            this.B_SRankCompleted.TabIndex = 6;
            this.B_SRankCompleted.Text = "(S)-Rank All Completed";
            this.toolTip1.SetToolTip(this.B_SRankCompleted, resources.GetString("B_SRankCompleted.ToolTip"));
            this.B_SRankCompleted.UseVisualStyleBackColor = true;
            this.B_SRankCompleted.Click += new System.EventHandler(this.B_SRankCompleted_Click);
            // 
            // B_StreetPassDelete
            // 
            this.B_StreetPassDelete.Location = new System.Drawing.Point(12, 157);
            this.B_StreetPassDelete.Name = "B_StreetPassDelete";
            this.B_StreetPassDelete.Size = new System.Drawing.Size(138, 23);
            this.B_StreetPassDelete.TabIndex = 53;
            this.B_StreetPassDelete.Text = "Edit StreetPass (reset)";
            this.toolTip1.SetToolTip(this.B_StreetPassDelete, resources.GetString("B_StreetPassDelete.ToolTip"));
            this.B_StreetPassDelete.UseVisualStyleBackColor = true;
            this.B_StreetPassDelete.Click += new System.EventHandler(this.B_StreetPassDelete_Click);
            // 
            // B_MaxExcalationBattle
            // 
            this.B_MaxExcalationBattle.Location = new System.Drawing.Point(12, 128);
            this.B_MaxExcalationBattle.Name = "B_MaxExcalationBattle";
            this.B_MaxExcalationBattle.Size = new System.Drawing.Size(138, 23);
            this.B_MaxExcalationBattle.TabIndex = 8;
            this.B_MaxExcalationBattle.Text = "Escalation Battle (999)";
            this.toolTip1.SetToolTip(this.B_MaxExcalationBattle, resources.GetString("B_MaxExcalationBattle.ToolTip"));
            this.B_MaxExcalationBattle.UseVisualStyleBackColor = true;
            this.B_MaxExcalationBattle.Click += new System.EventHandler(this.B_MaxExcalationBattle_Click);
            // 
            // Line
            // 
            this.Line.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.Line.Location = new System.Drawing.Point(12, 190);
            this.Line.MaximumSize = new System.Drawing.Size(288, 2);
            this.Line.MinimumSize = new System.Drawing.Size(20, 2);
            this.Line.Name = "Line";
            this.Line.Size = new System.Drawing.Size(288, 2);
            this.Line.TabIndex = 50;
            // 
            // B_PokemonReset
            // 
            this.B_PokemonReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.B_PokemonReset.Location = new System.Drawing.Point(12, 200);
            this.B_PokemonReset.Name = "B_PokemonReset";
            this.B_PokemonReset.Size = new System.Drawing.Size(138, 23);
            this.B_PokemonReset.TabIndex = 50;
            this.B_PokemonReset.Text = "Reset Pokemon";
            this.toolTip1.SetToolTip(this.B_PokemonReset, resources.GetString("B_PokemonReset.ToolTip"));
            this.B_PokemonReset.UseVisualStyleBackColor = true;
            this.B_PokemonReset.Click += new System.EventHandler(this.B_PokemonReset_Click);
            // 
            // B_StageReset
            // 
            this.B_StageReset.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.B_StageReset.Location = new System.Drawing.Point(156, 200);
            this.B_StageReset.Name = "B_StageReset";
            this.B_StageReset.Size = new System.Drawing.Size(138, 23);
            this.B_StageReset.TabIndex = 51;
            this.B_StageReset.Text = "Reset Stages";
            this.toolTip1.SetToolTip(this.B_StageReset, "-Marks every Normal & Expert stages a uncompleted\r\n-Sets their rank to C & highsc" +
        "ore to 0\r\n\r\n/!\\ Manually un-own any pokemon that should have been caught in one " +
        "of these levels.");
            this.B_StageReset.UseVisualStyleBackColor = true;
            this.B_StageReset.Click += new System.EventHandler(this.B_StageReset_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.AutoPopDelay = 10000;
            this.toolTip1.InitialDelay = 500;
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ReshowDelay = 100;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Button description";
            // 
            // B_MaxTalent
            // 
            this.B_MaxTalent.Location = new System.Drawing.Point(12, 41);
            this.B_MaxTalent.Name = "B_MaxTalent";
            this.B_MaxTalent.Size = new System.Drawing.Size(138, 23);
            this.B_MaxTalent.TabIndex = 2;
            this.B_MaxTalent.Text = "All Owned Skill Lvl (5)";
            this.toolTip1.SetToolTip(this.B_MaxTalent, "For every pokemon that you\'ve caught, sets their talent to lvl 5 with the proper " +
        "amount of \"experience\" needed.\r\n\r\nCtrl + click to select which level you want to" +
        " set your pokemons\'s skills to.");
            this.B_MaxTalent.UseVisualStyleBackColor = true;
            this.B_MaxTalent.Click += new System.EventHandler(this.B_MaxTalent_Click);
            // 
            // B_Test
            // 
            this.B_Test.Location = new System.Drawing.Point(235, 157);
            this.B_Test.Name = "B_Test";
            this.B_Test.Size = new System.Drawing.Size(65, 23);
            this.B_Test.TabIndex = 102;
            this.B_Test.Text = "Test stuff";
            this.toolTip1.SetToolTip(this.B_Test, "Used w/ B_Test_Click event to search for the \"skill+ drop\" flags in stagedata(s)." +
        "bin");
            this.B_Test.UseVisualStyleBackColor = true;
            this.B_Test.Visible = false;
            this.B_Test.Click += new System.EventHandler(this.B_Test_Click);
            // 
            // B_PokathlonStep
            // 
            this.B_PokathlonStep.Location = new System.Drawing.Point(156, 128);
            this.B_PokathlonStep.Name = "B_PokathlonStep";
            this.B_PokathlonStep.Size = new System.Drawing.Size(138, 23);
            this.B_PokathlonStep.TabIndex = 9;
            this.B_PokathlonStep.Text = "Edit Pokathlon (50th) ";
            this.toolTip1.SetToolTip(this.B_PokathlonStep, "Sets your next Survival mode battle to be the 50th one, against\r\nMega Mewtwo Y an" +
        "d with 99 moves left.\r\n\r\nCtrl + Click to select whichever step, opponent and mov" +
        "es you wish.");
            this.B_PokathlonStep.UseVisualStyleBackColor = true;
            this.B_PokathlonStep.Click += new System.EventHandler(this.B_PokathlonStep_Click);
            // 
            // Cheats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 235);
            this.Controls.Add(this.B_PokathlonStep);
            this.Controls.Add(this.B_Test);
            this.Controls.Add(this.B_CaughtEverything);
            this.Controls.Add(this.B_AllStones);
            this.Controls.Add(this.B_MaxTalent);
            this.Controls.Add(this.Line);
            this.Controls.Add(this.B_StageReset);
            this.Controls.Add(this.B_PokemonReset);
            this.Controls.Add(this.B_MaxExcalationBattle);
            this.Controls.Add(this.B_StreetPassDelete);
            this.Controls.Add(this.B_SRankCompleted);
            this.Controls.Add(this.B_AllCompleted);
            this.Controls.Add(this.B_MaxSpeedups);
            this.Controls.Add(this.B_AllCaughtStones);
            this.Controls.Add(this.B_MaxResources);
            this.Controls.Add(this.B_CaughtObtainables);
            this.Controls.Add(this.B_LevelMax);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(328, 999);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(328, 274);
            this.Name = "Cheats";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Bulk Edits";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button B_CaughtEverything;
        private System.Windows.Forms.Button B_LevelMax;
        private System.Windows.Forms.Button B_CaughtObtainables;
        private System.Windows.Forms.Button B_MaxResources;
        private System.Windows.Forms.Button B_AllStones;
        private System.Windows.Forms.Button B_AllCaughtStones;
        private System.Windows.Forms.Button B_MaxSpeedups;
        private System.Windows.Forms.Button B_AllCompleted;
        private System.Windows.Forms.Button B_SRankCompleted;
        private System.Windows.Forms.Button B_StreetPassDelete;
        private System.Windows.Forms.Button B_MaxExcalationBattle;
        private System.Windows.Forms.Label Line;
        private System.Windows.Forms.Button B_PokemonReset;
        private System.Windows.Forms.Button B_StageReset;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button B_MaxTalent;
        private System.Windows.Forms.Button B_Test;
        private System.Windows.Forms.Button B_PokathlonStep;
    }
}