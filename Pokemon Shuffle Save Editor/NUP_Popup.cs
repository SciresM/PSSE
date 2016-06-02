using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class NUP_Popup : Form
    {
        public int retVal
        {
            get
            {
                return (int)NUP_Value.Value;
            }
        }

        public NUP_Popup(int min, int max, int def, string str = "value")
        {
            InitializeComponent();
            L_Text.Text = "Select desired " + str + " :";
            NUP_Value.Minimum = min;
            NUP_Value.Maximum = max;
            NUP_Value.Value = def;
            L_Range.Text = "(" + min + " - " + max + ")";
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
