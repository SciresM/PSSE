using System;
using System.Linq;
using System.Windows.Forms;
using static Pokemon_Shuffle_Save_Editor.Main;
using static Pokemon_Shuffle_Save_Editor.ToolFunctions;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Cheats : Form
    {
        public Cheats()
        {
            InitializeComponent();
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

        private void B_AllCaughtStones_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < db.MegaStartIndex; i++)
            {   //if (caught && (hasMegaX || hasMegaY) && (at least 1 of these not equals to "default" : talent, type, max speedups). Doesn't check if Y form has been released, but both Charizard's & Mewtwo's already have.
                if (GetMon(i).Caught && (db.HasMega[db.Mons[i].Item1][0] || db.HasMega[db.Mons[i].Item1][1]) && ((db.Mons[db.MegaStartIndex + db.MegaList.IndexOf(i)].Item6 != 7) || (db.Mons[db.MegaStartIndex + db.MegaList.IndexOf(i)].Item7 != 0) || (db.Megas[db.MegaList.IndexOf(i)].Item2 != 1)))
                    SetStone(i, db.HasMega[db.Mons[i].Item1][0], db.HasMega[db.Mons[i].Item1][1]);
            }
            MessageBox.Show("All available megastones have been owned for everything you've caught.");
        }

        private void B_AllCompleted_Click(object sender, EventArgs e)
        {
            int j = 0;
            foreach (byte[] stage in new byte[][] { db.StagesMain, db.StagesExpert })
            {
                int entrylen = BitConverter.ToInt32(stage, 4);
                for (int i = 0; i < (BitConverter.ToInt32(stage, 0) - 1); i++)
                {
                    byte[] data = stage.Skip(0x50 + i * entrylen).Take(entrylen).ToArray();
                    if ((BitConverter.ToInt16(data, 0x4C) & 0x3FF) != 999)  //checks number of S ranks needed to unlock in order to skip "unreleased" expert stages. Not-expert stages should always return 0.
                    {
                        SetStage(i, j, true);
                        PatchScore(i, j);
                    }
                }
                j++;
            }
            MessageBox.Show("All Normal & Expert stages have been marked as completed.\n\nRewards like megastones or jewels can still be redeemed by beating the stage.");
        }

        private void B_AllStones_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < db.MegaStartIndex; i++)
            {
                if (db.HasMega[db.Mons[i].Item1][0] || db.HasMega[db.Mons[i].Item1][1])
                    SetStone(i, db.HasMega[db.Mons[i].Item1][0], db.HasMega[db.Mons[i].Item1][1]);
            }
            MessageBox.Show("All Mega Stones are now owned.");
        }

        private void B_CaughtEverything_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < db.MegaStartIndex; i++) //includes 15 reserved slots
                SetCaught(i, true);
            MessageBox.Show("All Pokemon are now caught.");
        }

        private void B_CaughtObtainables_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < db.MegaStartIndex; i++)
                SetCaught(i, (db.Mons[i].Rest.Item1 != 999) && ((db.Mons[i].Item5 != 1) || (db.Mons[i].Item6 != 1) || (db.Mons[i].Item7 != 0))); //((displayed number isn't 999) && (at least 1 of these isn't "default" : base power, talent, type))
            int stagelen = BitConverter.ToInt32(db.StagesMain, 0x4);
            foreach (byte[] stage in new byte[][] { db.StagesMain, db.StagesExpert })
            {
                for (int i = 1; i < BitConverter.ToInt32(stage, 0); i++)
                {
                    int ind = BitConverter.ToUInt16(stage, 0x50 + stagelen * (i)) & 0x3FF;
                    SetCaught(ind, true);
                }
            }
            MessageBox.Show("All obtainable Pokemon have now been caught.");
        }

        private void B_EscalationReset_Click(object sender, EventArgs e)
        {
            SetExcalationStep();
            MessageBox.Show("Curent escalation battle has been reverted to step 1.\n\nCarefull : only use it when there's exactly one active escalation battle.\nI don't know how this behaves if there is 0 or more than 1 active at the same time.");
        }

        private void B_LevelMax_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < db.MegaStartIndex; i++)
            {
                if (GetMon(i).Caught)
                    SetLevel(i, 10 + Math.Min(db.Mons[i].Item4, 5));
            }
            MessageBox.Show("Everything you've caught is now level Max.");
        }

        private void B_MaxExcalationBattle_Click(object sender, EventArgs e)
        {
            SetExcalationStep(999);
            MessageBox.Show("Curent escalation battle has been taken to step 999. You'll get all rewards at once by beating it.\n\nCarefull : only use it when there's exactly one active escalation battle.\nI don't know how this behaves if there is 0 or more than 1 active at the same time.");
        }

        private void B_MaxResources_Click(object sender, EventArgs e)
        {
            int[] items = new int[7];
            for (int i = 0; i < 7; i++)
                items[i] = 99;
            int[] enhancements = new int[9];
            for (int i = 0; i < 9; i++)
                enhancements[i] = 99;
            SetResources(99, 99999, 150, items, enhancements);
            MessageBox.Show("Gave 99 hearts, 99999 coins, 150 jewels, and 99 of every item.");
        }

        private void B_MaxSpeedups_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < db.MegaStartIndex; i++)
            {   //if (caught && (hasMegaX || hasMegaY) && (at least one stone owned))
                if (GetMon(i).Caught && (db.HasMega[db.Mons[i].Item1][0] || db.HasMega[db.Mons[i].Item1][1]) && (GetMon(i).Stone > 0 || GetMon(i).Stone < 4))
                {
                    int suX = db.HasMega[db.Mons[i].Item1][0] ? db.Megas[db.MegaList.IndexOf(i)].Item2 : 0;
                    int suY = db.HasMega[db.Mons[i].Item1][1] ? db.Megas[db.MegaList.IndexOf(i, db.MegaList.IndexOf(i) + 1)].Item2 : 0;
                    SetSpeedup(i, (db.HasMega[db.Mons[i].Item1][0] && ((GetMon(i).Stone & 1) == 1)), suX, (db.HasMega[db.Mons[i].Item1][1] && ((GetMon(i).Stone & 2) == 2)), suY);   //(i, (hasMegaX && owned stoneX), max X value from db, (hasMegaY && owned stoneY), max Y value from db)
                }
            }
            MessageBox.Show("All Owned Megas (for which you own the stone too) have been fed with as much Mega Speedups as possible.");
        }

        private void B_MaxTalent_Click(object sender, EventArgs e)
        {
            int entrylen = BitConverter.ToInt32(db.MonData, 0x4);
            for (int i = 0; i < db.MegaStartIndex; i++)
            {
                if (GetMon(i).Caught)
                    SetSkill(i, 5);
            }
            MessageBox.Show("Every pokemon that you have caught now has its talent fully powered !");
        }

        private void B_PokemonReset_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < db.MegaStartIndex; i++)
            {
                SetCaught(i, false);    //Uncatch
                SetLevel(i); //Un-level, Un-experience & Un-lollipop
                if (db.HasMega[db.Mons[i].Item1][0] || db.HasMega[db.Mons[i].Item1][1])
                {
                    SetStone(i); //Un-stone
                    SetSpeedup(i);   //Unfeed speedups
                }
            }
            MessageBox.Show("All pokemons have been uncaught, reset to level 1 & lost their Mega Stones, speedups or lollipops.\n\nEither reset stages too or make sure to catch at least Espurr, Bulbasaur, Squirtle & Charmander manually.");
        }

        private void B_ResourcesReset_Click(object sender, EventArgs e)
        {
            SetResources();
            MessageBox.Show("Deleted all stock hearts, coins, jewels and Items.");
        }

        private void B_SRankCompleted_Click(object sender, EventArgs e)
        {
            int j = 0;
            foreach (byte[] stage in new byte[][] { db.StagesMain, db.StagesExpert })
            {
                int entrylen = BitConverter.ToInt32(stage, 0x4);
                for (int i = 0; i < (BitConverter.ToInt32(stage, 0) - ((stage == db.StagesMain) ? 1 : 0)); i++)
                {
                    if (GetStage(i, j).Completed)
                    {
                        SetRank(i, j, 3);
                        PatchScore(i, j);
                    }
                }
                j++;
            }
            MessageBox.Show("All Completed Normal & Expert stages have been S-ranked.");
        }

        private void B_StageReset_Click(object sender, EventArgs e)
        {
            int j = 0;
            foreach (int length in new int[] { (BitConverter.ToInt32(db.StagesMain, 0) - 1), BitConverter.ToInt32(db.StagesExpert, 0), 100 })//{number of Main stages, number of Expert stages, 100}. Max number of event levels could be up to 549 but it's unlikely there ever are more than 100 at any given time (and that space could be used for something else later).
            {
                for (int i = 0; i < length; i++)
                {
                    SetStage(i, j);
                    SetRank(i, j);
                    SetScore(i, j);
                }
                j++;
            }
            MessageBox.Show("All stages have been reset to C Rank, 0 score & uncompleted state.");
        }

        private void B_StreetPassDelete_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 10; i++)
                Array.Copy(new byte[0x68], 0, savedata, 0x59A7 + (i * 0x68), 0x68); //Erase StreetPass tags
            Array.Copy(BitConverter.GetBytes(0x00), 0, savedata, 0x5967, 2); //Resets streetpass count to 0
            MessageBox.Show("StreetPass data have been cleared & StreetPass count reset to 0.");
        }
    }
}