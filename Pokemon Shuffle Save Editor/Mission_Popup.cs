using System;
using System.Windows.Forms;
using static Pokemon_Shuffle_Save_Editor.Main;

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
            for (int i = 0; i < retStates.Length; i++)
            {
                for (int j = 0; j < retStates[i].Length; j++)
                {
                    retStates[i][j] = (((savedata[MissionCards.Ofset(i, j)] >> MissionCards.Shift(i, j)) & 1) == 1);
                }
            }
            NUP_Active.Value = active;
            NUP_Mission.Value = (active > 0) ? active : 1;
            NUP_Active.Maximum = NUP_Mission.Maximum = db.Missions.Length;
            UpdateForm();         
        }

        private void UpdateForm()
        {
            foreach (CheckBox chk in GB_States.Controls)
            {
                chk.Visible = db.Missions[(int)NUP_Mission.Value - 1][Int32.Parse(chk.Text) - 1];
                chk.Checked = db.Missions[(int)NUP_Mission.Value - 1][Int32.Parse(chk.Text) - 1] && retStates[(int)NUP_Mission.Value - 1][Int32.Parse(chk.Text) - 1];
            }
        }

        private void StatesInit()
        {
            retStates = new bool[db.Missions.Length][];
            for (int i = 0; i < retStates.Length; i++)
                retStates[i] = new bool[db.Missions[i].Length];
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
            retStates[(int)NUP_Mission.Value - 1][Int32.Parse((sender as CheckBox).Text) - 1] = (sender as CheckBox).Checked;
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
