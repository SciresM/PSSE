using System;
using System.Windows.Forms;
using static Pokemon_Shuffle_Save_Editor.Main;
using static Pokemon_Shuffle_Save_Editor.ToolFunctions;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Pokathlon_Popup : Form
    {
        public bool retEnabled
        {
            get { return CHK_Paused.Checked; }
        }
        public int retStep
        {
            get { return (int)NUP_Step.Value; }
        }
        public int retMoves
        {
            get { return (int)NUP_Moves.Value; }
        }
        public int retOpponent
        {
            get { return (int)NUP_Opponent.Value; }
        }

        public Pokathlon_Popup(int oValue, int mValue, int sValue, int oMin = 1, int mMin = 0, int sMin = 0, int oMax = 400, int mMax = 99, int sMax = 50)
        {
            InitializeComponent();
            int j = 0;
            int[] list = { oValue, mValue, sValue, oMin, mMin, sMin, oMax, mMax, sMax };
            foreach (NumericUpDown nup in new[] { NUP_Opponent, NUP_Moves, NUP_Step })
            {
                nup.Minimum = list[3 + j];
                nup.Maximum = list[6 + j];
                nup.Value = list[j];
                j++;
            }
            UpdateForm();
            CHK_Paused.Checked = (((BitConverter.ToInt16(savedata, 0xB768) >> 7) & 3) == 3);
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

        private void ValueChanged(object sender, EventArgs e)
        {
            UpdateForm();
        }

        private void UpdateForm()
        {
            B_Random.Visible = ((int)NUP_Step.Value > 0);
            PB_Opponent.Image = ResizeImage(GetMonImage(BitConverter.ToInt16(db.StagesMain, 0x50 + BitConverter.ToInt32(db.StagesMain, 0x4) * (int)NUP_Opponent.Value) & 0x3FF), 48, 48);
        }

        private void B_OK_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void B_Random_Click(object sender, EventArgs e)
        {
            int min, max;
            if (ModifierKeys == Keys.Control)
            {
                min = (int)NUP_Opponent.Minimum;
                max = (int)NUP_Opponent.Maximum;
            }
            else
            {
                min = db.PokathlonRand[(int)NUP_Step.Value - 1][0];
                max = db.PokathlonRand[(int)NUP_Step.Value - 1][1] + 1; //Random() never equals its max value, hence max +1
            }
            NUP_Opponent.Value = new Random().Next(min, max);
        }
    }
}