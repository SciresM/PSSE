using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Pokemon_Shuffle_Save_Editor.Main;
using static Pokemon_Shuffle_Save_Editor.ToolFunctions;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Mission_Popup : Form
    {
        public bool[][] retStates { get; private set; }

        public int retActive
        {
            get { return (int)NUP_Active.Value; }
        }

        public Mission_Popup(int active)
        {
            InitializeComponent();
            StatesInit();
            for (int i = 0; i < 50; i++)
            {
                for (int j = 0; j < 10; j++)
                {
                    retStates[i][j] = (((savedata[MissionCards.Ofset(i, j)] >> MissionCards.Shift(i, j)) & 1) == 1);
                }
            }
            NUP_Active.Value = active;
            NUP_Mission.Value = (active > 0) ? active : 1;
            UpdateForm();         
        }

        private void UpdateForm()
        {
            foreach (CheckBox chk in GB_States.Controls)
                chk.Checked = retStates[(int)NUP_Mission.Value - 1][Int32.Parse(chk.Name.Replace("CB_", "")) - 1];
        }

        private void StatesInit()
        {
            retStates = new bool[50][];
            for (int i = 0; i < retStates.Length; i++)
                retStates[i] = new bool[10];
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

        private void NUP_Mission_ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void CheckedChanged(object sender, EventArgs e)
        {
            retStates[(int)NUP_Mission.Value - 1][Int32.Parse((sender as CheckBox).Name.Replace("CB_", "")) - 1] = (sender as CheckBox).Checked;
        }

        private void B_Erase_Click(object sender, EventArgs e)
        {
            StatesInit();
            B_OK_Click(sender, e);
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
