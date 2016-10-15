namespace Pokemon_Shuffle_Save_Editor
{
    partial class NUP_Popup
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
            this.L_Text = new System.Windows.Forms.Label();
            this.NUP_Value = new System.Windows.Forms.NumericUpDown();
            this.L_Range = new System.Windows.Forms.Label();
            this.B_OK = new System.Windows.Forms.Button();
            this.CHK_Value = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Value)).BeginInit();
            this.SuspendLayout();
            // 
            // L_Text
            // 
            this.L_Text.Dock = System.Windows.Forms.DockStyle.Top;
            this.L_Text.Location = new System.Drawing.Point(0, 0);
            this.L_Text.Name = "L_Text";
            this.L_Text.Size = new System.Drawing.Size(249, 27);
            this.L_Text.TabIndex = 0;
            this.L_Text.Text = "Select desired value :";
            this.L_Text.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // NUP_Value
            // 
            this.NUP_Value.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.NUP_Value.Location = new System.Drawing.Point(63, 25);
            this.NUP_Value.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.NUP_Value.Name = "NUP_Value";
            this.NUP_Value.Size = new System.Drawing.Size(96, 20);
            this.NUP_Value.TabIndex = 1;
            // 
            // L_Range
            // 
            this.L_Range.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.L_Range.AutoSize = true;
            this.L_Range.Location = new System.Drawing.Point(165, 27);
            this.L_Range.Name = "L_Range";
            this.L_Range.Size = new System.Drawing.Size(34, 13);
            this.L_Range.TabIndex = 2;
            this.L_Range.Text = "(0 - 0)";
            this.L_Range.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // B_OK
            // 
            this.B_OK.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.B_OK.Location = new System.Drawing.Point(162, 49);
            this.B_OK.Name = "B_OK";
            this.B_OK.Size = new System.Drawing.Size(75, 23);
            this.B_OK.TabIndex = 3;
            this.B_OK.Text = "OK";
            this.B_OK.UseVisualStyleBackColor = true;
            this.B_OK.Click += new System.EventHandler(this.B_OK_Click);
            // 
            // CHK_Value
            // 
            this.CHK_Value.AutoSize = true;
            this.CHK_Value.Location = new System.Drawing.Point(12, 53);
            this.CHK_Value.Name = "CHK_Value";
            this.CHK_Value.Size = new System.Drawing.Size(57, 17);
            this.CHK_Value.TabIndex = 4;
            this.CHK_Value.Text = "Check";
            this.CHK_Value.UseVisualStyleBackColor = true;
            // 
            // NUP_Popup
            // 
            this.AcceptButton = this.B_OK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 82);
            this.Controls.Add(this.CHK_Value);
            this.Controls.Add(this.B_OK);
            this.Controls.Add(this.L_Range);
            this.Controls.Add(this.NUP_Value);
            this.Controls.Add(this.L_Text);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "NUP_Popup";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Custom value requested";
            ((System.ComponentModel.ISupportInitialize)(this.NUP_Value)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label L_Text;
        private System.Windows.Forms.NumericUpDown NUP_Value;
        private System.Windows.Forms.Label L_Range;
        private System.Windows.Forms.Button B_OK;
        private System.Windows.Forms.CheckBox CHK_Value;
    }
}