using System;
using System.Windows.Forms;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class NUP_Popup : Form
    {
        public int retVal
        {
            get { return (int)NUP_Value.Value; }
        }
        public bool retChk
        {
            get { return CHK_Value.Checked; }
        }

        public NUP_Popup(int min, int max, int def, string str = "value")
        {
            InitializeComponent();
            B_OK.Location = new System.Drawing.Point(84, 51);
            CHK_Value.Visible = false;
            L_Text.Text = "Select desired " + str + " :";
            NUP_Value.Minimum = min;
            NUP_Value.Maximum = max;
            NUP_Value.Value = def;
            L_Range.Text = "(" + min + " - " + max + ")";
        }

        public NUP_Popup(int min, int max, int def, bool chkd, string lbl = "value", string chk = "boolean")
        {
            InitializeComponent();
            B_OK.Location = new System.Drawing.Point(162, 51);
            L_Text.Text = "Select desired " + lbl + " :";
            NUP_Value.Minimum = min;
            NUP_Value.Maximum = max;
            NUP_Value.Value = def;
            L_Range.Text = "(" + min + " - " + max + ")";
            CHK_Value.Text = chk;
            CHK_Value.Checked = chkd;
        }

        protected override bool ProcessDialogKey(Keys keyData)  //Allows quit when Esc is pressed
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}