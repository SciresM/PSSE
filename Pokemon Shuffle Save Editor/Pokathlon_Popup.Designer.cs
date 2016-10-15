namespace Pokemon_Shuffle_Save_Editor
{
    partial class Pokathlon_Popup
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
            this.L_Step = new System.Windows.Forms.Label();
            this.NUP_Step = new System.Windows.Forms.NumericUpDown();
            this.NUP_Moves = new System.Windows.Forms.NumericUpDown();
            this.L_Moves = new System.Windows.Forms.Label();
            this.L_Opponent = new System.Windows.Forms.Label();
            this.PB_Opponent = new System.Windows.Forms.PictureBox();
            this.NUP_Opponent = new System.Windows.Forms.NumericUpDown();
            this.B_OK = new System.Windows.Forms.Button();
            this.B_Random = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.CHK_Paused = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Step)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Moves)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Opponent)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Opponent)).BeginInit();
            this.SuspendLayout();
            // 
            // L_Step
            // 
            this.L_Step.AutoSize = true;
            this.L_Step.Location = new System.Drawing.Point(12, 9);
            this.L_Step.Name = "L_Step";
            this.L_Step.Size = new System.Drawing.Size(35, 13);
            this.L_Step.TabIndex = 0;
            this.L_Step.Text = "Step :";
            // 
            // NUP_Step
            // 
            this.NUP_Step.Location = new System.Drawing.Point(15, 24);
            this.NUP_Step.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NUP_Step.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Step.Name = "NUP_Step";
            this.NUP_Step.Size = new System.Drawing.Size(59, 20);
            this.NUP_Step.TabIndex = 1;
            this.NUP_Step.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.NUP_Step.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // NUP_Moves
            // 
            this.NUP_Moves.Location = new System.Drawing.Point(110, 24);
            this.NUP_Moves.Maximum = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.NUP_Moves.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Moves.Name = "NUP_Moves";
            this.NUP_Moves.Size = new System.Drawing.Size(59, 20);
            this.NUP_Moves.TabIndex = 3;
            this.NUP_Moves.Value = new decimal(new int[] {
            99,
            0,
            0,
            0});
            this.NUP_Moves.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // L_Moves
            // 
            this.L_Moves.AutoSize = true;
            this.L_Moves.Location = new System.Drawing.Point(107, 8);
            this.L_Moves.Name = "L_Moves";
            this.L_Moves.Size = new System.Drawing.Size(62, 13);
            this.L_Moves.TabIndex = 2;
            this.L_Moves.Text = "Moves left :";
            // 
            // L_Opponent
            // 
            this.L_Opponent.AutoSize = true;
            this.L_Opponent.Location = new System.Drawing.Point(12, 58);
            this.L_Opponent.Name = "L_Opponent";
            this.L_Opponent.Size = new System.Drawing.Size(85, 13);
            this.L_Opponent.TabIndex = 4;
            this.L_Opponent.Text = "Next Opponent :";
            // 
            // PB_Opponent
            // 
            this.PB_Opponent.Location = new System.Drawing.Point(115, 66);
            this.PB_Opponent.Name = "PB_Opponent";
            this.PB_Opponent.Size = new System.Drawing.Size(48, 48);
            this.PB_Opponent.TabIndex = 29;
            this.PB_Opponent.TabStop = false;
            // 
            // NUP_Opponent
            // 
            this.NUP_Opponent.Location = new System.Drawing.Point(15, 74);
            this.NUP_Opponent.Maximum = new decimal(new int[] {
            150,
            0,
            0,
            0});
            this.NUP_Opponent.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Opponent.Name = "NUP_Opponent";
            this.NUP_Opponent.Size = new System.Drawing.Size(59, 20);
            this.NUP_Opponent.TabIndex = 61;
            this.toolTip1.SetToolTip(this.NUP_Opponent, "The numbers are the stage numbers of all Main stages.");
            this.NUP_Opponent.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.NUP_Opponent.ValueChanged += new System.EventHandler(this.ValueChanged);
            // 
            // B_OK
            // 
            this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.B_OK.Location = new System.Drawing.Point(88, 132);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(75, 23);
            this.B_OK.TabIndex = 62;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // B_Random
            // 
            this.B_Random.Location = new System.Drawing.Point(15, 99);
            this.B_Random.Name = "B_Random";
            this.B_Random.Size = new System.Drawing.Size(59, 23);
            this.B_Random.TabIndex = 63;
            this.B_Random.Text = "Random";
            this.toolTip1.SetToolTip(this.B_Random, "Click to get a random opponent among the legit ones for that step.\r\n\r\nCtrl + Clic" +
        "k to get a random opponent among the first 150 main stages.");
            this.B_Random.UseVisualStyleBackColor = true;
            this.B_Random.Click += new System.EventHandler(this.B_Random_Click);
            // 
            // toolTip1
            // 
            this.toolTip1.IsBalloon = true;
            this.toolTip1.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.toolTip1.ToolTipTitle = "Button description";
            // 
            // CHK_Paused
            // 
            this.CHK_Paused.AutoSize = true;
            this.CHK_Paused.Location = new System.Drawing.Point(15, 136);
            this.CHK_Paused.Name = "CHK_Paused";
            this.CHK_Paused.Size = new System.Drawing.Size(65, 17);
            this.CHK_Paused.TabIndex = 64;
            this.CHK_Paused.Text = "Enabled";
            this.toolTip1.SetToolTip(this.CHK_Paused, "If this is checked, the game will allow you to resume\r\na¨Pokathlon session on nex" +
        "t launch.");
            this.CHK_Paused.UseVisualStyleBackColor = true;
            this.CHK_Paused.CheckedChanged += new System.EventHandler(this.ValueChanged);
            // 
            // Pokathlon_Popup
            // 
            this.AcceptButton = this.B_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(181, 167);
            this.Controls.Add(this.CHK_Paused);
            this.Controls.Add(this.B_Random);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.NUP_Opponent);
            this.Controls.Add(this.PB_Opponent);
            this.Controls.Add(this.L_Opponent);
            this.Controls.Add(this.NUP_Moves);
            this.Controls.Add(this.L_Moves);
            this.Controls.Add(this.NUP_Step);
            this.Controls.Add(this.L_Step);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(197, 206);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(197, 206);
            this.Name = "Pokathlon_Popup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Pokathlon Editor";
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Step)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Moves)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.PB_Opponent)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Opponent)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label L_Step;
        private System.Windows.Forms.NumericUpDown NUP_Step;
        private System.Windows.Forms.NumericUpDown NUP_Moves;
        private System.Windows.Forms.Label L_Moves;
        private System.Windows.Forms.Label L_Opponent;
        private System.Windows.Forms.PictureBox PB_Opponent;
        private System.Windows.Forms.NumericUpDown NUP_Opponent;
        private System.Windows.Forms.Button B_OK;
        private System.Windows.Forms.Button B_Random;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox CHK_Paused;
    }
}