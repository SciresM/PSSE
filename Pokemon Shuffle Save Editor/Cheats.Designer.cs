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
            this.B_CaughtEverything = new System.Windows.Forms.Button();
            this.B_LevelMax = new System.Windows.Forms.Button();
            this.B_CaughtObtainables = new System.Windows.Forms.Button();
            this.B_MaxResources = new System.Windows.Forms.Button();
            this.B_AllStones = new System.Windows.Forms.Button();
            this.B_AllCaughtStones = new System.Windows.Forms.Button();
            this.B_MaxSpeedups = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // B_CaughtEverything
            // 
            this.B_CaughtEverything.Location = new System.Drawing.Point(12, 22);
            this.B_CaughtEverything.Name = "B_CaughtEverything";
            this.B_CaughtEverything.Size = new System.Drawing.Size(138, 23);
            this.B_CaughtEverything.TabIndex = 0;
            this.B_CaughtEverything.Text = "Caught All Pokemon";
            this.B_CaughtEverything.UseVisualStyleBackColor = true;
            this.B_CaughtEverything.Click += new System.EventHandler(this.B_CaughtEverything_Click);
            // 
            // B_LevelMax
            // 
            this.B_LevelMax.Location = new System.Drawing.Point(12, 78);
            this.B_LevelMax.Name = "B_LevelMax";
            this.B_LevelMax.Size = new System.Drawing.Size(138, 23);
            this.B_LevelMax.TabIndex = 2;
            this.B_LevelMax.Text = "All Owned LvMax";
            this.B_LevelMax.UseVisualStyleBackColor = true;
            this.B_LevelMax.Click += new System.EventHandler(this.B_LevelMax_Click);
            // 
            // B_CaughtObtainables
            // 
            this.B_CaughtObtainables.Location = new System.Drawing.Point(161, 22);
            this.B_CaughtObtainables.Name = "B_CaughtObtainables";
            this.B_CaughtObtainables.Size = new System.Drawing.Size(138, 23);
            this.B_CaughtObtainables.TabIndex = 3;
            this.B_CaughtObtainables.Text = "Caught All Obtainables";
            this.B_CaughtObtainables.UseVisualStyleBackColor = true;
            this.B_CaughtObtainables.Click += new System.EventHandler(this.B_CaughtObtainables_Click);
            // 
            // B_MaxResources
            // 
            this.B_MaxResources.Location = new System.Drawing.Point(161, 78);
            this.B_MaxResources.Name = "B_MaxResources";
            this.B_MaxResources.Size = new System.Drawing.Size(138, 23);
            this.B_MaxResources.TabIndex = 4;
            this.B_MaxResources.Text = "Maximum Resources";
            this.B_MaxResources.UseVisualStyleBackColor = true;
            this.B_MaxResources.Click += new System.EventHandler(this.B_MaxResources_Click);
            // 
            // B_AllStones
            // 
            this.B_AllStones.Location = new System.Drawing.Point(12, 50);
            this.B_AllStones.Name = "B_AllStones";
            this.B_AllStones.Size = new System.Drawing.Size(138, 23);
            this.B_AllStones.TabIndex = 5;
            this.B_AllStones.Text = "Have All Mega Stones";
            this.B_AllStones.UseVisualStyleBackColor = true;
            this.B_AllStones.Click += new System.EventHandler(this.B_AllStones_Click);
            // 
            // B_AllCaughtStones
            // 
            this.B_AllCaughtStones.Location = new System.Drawing.Point(161, 51);
            this.B_AllCaughtStones.Name = "B_AllCaughtStones";
            this.B_AllCaughtStones.Size = new System.Drawing.Size(138, 23);
            this.B_AllCaughtStones.TabIndex = 6;
            this.B_AllCaughtStones.Text = "All Caught Mega Stones";
            this.B_AllCaughtStones.UseVisualStyleBackColor = true;
            this.B_AllCaughtStones.Click += new System.EventHandler(this.B_AllCaughtStones_Click);
            // 
            // B_MaxSpeedups
            // 
            this.B_MaxSpeedups.Location = new System.Drawing.Point(88, 107);
            this.B_MaxSpeedups.Name = "B_MaxSpeedups";
            this.B_MaxSpeedups.Size = new System.Drawing.Size(138, 23);
            this.B_MaxSpeedups.TabIndex = 7;
            this.B_MaxSpeedups.Text = "All Owned Max Speedups";
            this.B_MaxSpeedups.UseVisualStyleBackColor = true;
            this.B_MaxSpeedups.Click += new System.EventHandler(this.B_MaxSpeedups_Click);
            // 
            // Cheats
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(312, 137);
            this.Controls.Add(this.B_MaxSpeedups);
            this.Controls.Add(this.B_AllCaughtStones);
            this.Controls.Add(this.B_AllStones);
            this.Controls.Add(this.B_MaxResources);
            this.Controls.Add(this.B_CaughtObtainables);
            this.Controls.Add(this.B_LevelMax);
            this.Controls.Add(this.B_CaughtEverything);
            this.Name = "Cheats";
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
    }
}