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
            if (ModifierKeys == Keys.Control)
            {
                for (int i = 0; i < db.MegaStartIndex; i++)
                    SetStone(i);
                MessageBox.Show("You don't own any stone anymore.");
            }
            else
            {
                for (int i = 0; i < db.MegaStartIndex; i++)
                {   //if (caught && (hasMegaX || hasMegaY) && (at least 1 of these not equals to "default" : talent, type, max speedups). Doesn't check if Y form has been released, but both Charizard's & Mewtwo's already have.
                    if (GetMon(i).Caught && (db.HasMega[i][0] || db.HasMega[i][1]) && ((db.Mons[db.MegaStartIndex + db.MegaList.IndexOf(i)].Item6 != 7) || (db.Mons[db.MegaStartIndex + db.MegaList.IndexOf(i)].Item7 != 0) || (db.Megas[db.MegaList.IndexOf(i)].Item2 != 1)))
                        SetStone(i, db.HasMega[i][0], db.HasMega[i][1]);
                }
                MessageBox.Show("You now own every released stone for each of your caught pokemons.");
            }
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
        {   //advanced user stuff
            for (int i = 0; i < db.MegaStartIndex; i++)
            {
                if (db.HasMega[i][0] || db.HasMega[i][1])
                    SetStone(i, db.HasMega[i][0], db.HasMega[i][1]);
            }
            MessageBox.Show("All Mega Stones are now owned.");
        }

        private void B_CaughtEverything_Click(object sender, EventArgs e)
        {   //advanced user stuff
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
            MessageBox.Show("You now own all obtainable pokemons.");
        }

        private void B_LevelMax_Click(object sender, EventArgs e)
        {
            int value = 15;    //default value
            bool boool = false;
            if (ModifierKeys == Keys.Control)
            {
                using (var form = new NUP_Popup(0, 15, value, "max level"))
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                    {
                        value = form.retVal;
                        boool = true;
                    }
                    else return;
                }
            }
            for (int i = 0; i < db.MegaStartIndex; i++)
            {
                if (GetMon(i).Caught)
                    SetLevel(i, Math.Min(10 + db.Mons[i].Item4, value));
            }
            if (boool)
                MessageBox.Show("Everything you've caught is now level " + value + ((value > 10) ? " or below." : "."));
            else
                MessageBox.Show("Everything you've caught is now level max.");
        }

        private void B_MaxExcalationBattle_Click(object sender, EventArgs e)
        {
            int value = 999;    //default value
            if (ModifierKeys == Keys.Control)
            {
                using (var form = new NUP_Popup(0, 999, value, "step"))
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                        value = form.retVal;
                    else return;
                }
            }
            SetExcalationStep(value);
            MessageBox.Show("Curent escalation battle has been taken to step " + value + ".\nIf you skipped rewards you'll get all of them at once after beating it.");
        }

        private void B_MaxResources_Click(object sender, EventArgs e)
        {
            if (ModifierKeys == Keys.Control)
            {
                SetResources();
                MessageBox.Show("Deleted all stock hearts, coins, jewels and Items.");
            }
            else
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
        }

        private void B_MaxSpeedups_Click(object sender, EventArgs e)
        {
            int value = 127;    //default value
            bool boool = false;
            if (ModifierKeys == Keys.Control)
            {
                using (var form = new NUP_Popup(0, 127, value, "max speedUps"))
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                    {
                        value = form.retVal;
                        boool = true;
                    }
                    else return;
                }
            }
            for (int i = 0; i < db.MegaStartIndex; i++)
            {   //if (caught && (hasMegaX || hasMegaY) && (at least one stone owned))
                if (GetMon(i).Caught && (db.HasMega[i][0] || db.HasMega[i][1]) && (GetMon(i).Stone > 0 || GetMon(i).Stone < 4))
                {
                    int suX = Math.Min(db.HasMega[i][0] ? db.Megas[db.MegaList.IndexOf(i)].Item2 : 0, value);
                    int suY = Math.Min(db.HasMega[i][1] ? db.Megas[db.MegaList.IndexOf(i, db.MegaList.IndexOf(i) + 1)].Item2 : 0, value);
                    SetSpeedup(i, (db.HasMega[i][0] && ((GetMon(i).Stone & 1) == 1)), suX, (db.HasMega[i][1] && ((GetMon(i).Stone & 2) == 2)), suY);   //(i, (hasMegaX && owned stoneX), max X value from db, (hasMegaY && owned stoneY), max Y value from db)
                }
            }
            if (boool)
                MessageBox.Show("All Owned Megas (for which you own the stone too) have been fed with " + value + " speedups" + ((value > 1) ? " or below." : "."));
            else
                MessageBox.Show("All Owned Megas (for which you own the stone too) have been fed with as much Mega Speedups as possible.");
        }

        private void B_MaxTalent_Click(object sender, EventArgs e)
        {
            int value = 5;    //default value
            if (ModifierKeys == Keys.Control)
            {
                using (var form = new NUP_Popup(0, 5, value, "skill lvl"))
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                        value = form.retVal;
                    else return;
                }
            }
            for (int i = 0; i < db.MegaStartIndex; i++)
            {
                if (GetMon(i).Caught)
                    SetSkill(i, value);
            }
            MessageBox.Show("Every pokemon that you have caught now has its talent powered to level " + value + " !");
        }

        private void B_PokemonReset_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < db.MegaStartIndex; i++)
            {
                SetCaught(i);    //Uncatch
                SetLevel(i); //Un-level, Un-experience & Un-lollipop
                SetSkill(i);
                if (db.HasMega[i][0] || db.HasMega[i][1])
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
            int value = 3;    //default value
            int j = 0;
            string str;
            if (ModifierKeys == Keys.Control)
            {
                using (var form = new NUP_Popup(0, 3, value, "rank"))
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                        value = form.retVal;
                    else return;
                }
            }
            foreach (byte[] stage in new byte[][] { db.StagesMain, db.StagesExpert })
            {
                int entrylen = BitConverter.ToInt32(stage, 0x4);
                for (int i = 0; i < (BitConverter.ToInt32(stage, 0) - ((stage == db.StagesMain) ? 1 : 0)); i++)
                {
                    if (GetStage(i, j).Completed)
                    {
                        SetRank(i, j, value);
                        PatchScore(i, j);
                    }
                }
                j++;
            }
            switch (value)
            {
                case 0:
                    str = "C";
                    break;

                case 1:
                    str = "B";
                    break;

                case 2:
                    str = "A";
                    break;

                case 3:
                    str = "S";
                    break;

                default:
                    MessageBox.Show("An error occured. Attempted to set rank to : " + value);
                    return;
            }
            MessageBox.Show("All Completed Normal & Expert stages have been " + str + "-ranked.");
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

        private void B_Test_Click(object sender, EventArgs e)
        {   //don't bother, testing stuff
            int j = 0;
            foreach (byte[] stage in new byte[][] { db.StagesMain, db.StagesExpert, db.StagesEvent })
            {
                Console.WriteLine(j + "\n==============");
                int entrylen = BitConverter.ToInt32(stage, 4);
                for (int i = 0; i < BitConverter.ToInt32(stage, 0); i++)
                {
                    byte[] data = stage.Skip(0x50 + i * 0x5C).Take(0x5C).ToArray();
                    if (BitConverter.ToInt16(data, 0x43) != 0)
                    {   //returns 9319 (most skill+-droping stages), 9219 (uxie stage), 6106 (Eevee stage) or 0 (all other stages)
                        Console.WriteLine("{0:X}", BitConverter.ToInt16(data, 0x43));
                        Console.WriteLine(db.MonsList[BitConverter.ToInt16(data, 0) & 0x3FF]);
                    }
                }
                j++;
            }
        }

        private void B_PokathlonStep_Click(object sender, EventArgs e)
        {
            int step = 50, moves = 99, opponent = 150;    //default values
            bool enabled = true; //default values
            if (ModifierKeys == Keys.Control)
            {
                using (var form = new Pokathlon_Popup(BitConverter.ToInt16(savedata, 0xB762) >> 6, (savedata[0xB768] & 0x7F), savedata[0xB760]))
                {
                    form.ShowDialog();
                    if (form.DialogResult == DialogResult.OK)
                    {
                        step = form.retStep;
                        moves = form.retMoves;
                        opponent = form.retOpponent;
                        enabled = form.retEnabled;
                    }
                    else return;
                }
            }
            Array.Copy(BitConverter.GetBytes((BitConverter.ToInt16(savedata, 0xB768) & ~(0x3 << 7)) | ((enabled ? 3 : 0) << 7)), 0, savedata, 0xB768, 2);
            savedata[0xB760] = (byte)step;
            Array.Copy(BitConverter.GetBytes((BitConverter.ToInt16(savedata, 0xB768) & ~(0x7F)) | moves), 0, savedata, 0xB768, 2);
            Array.Copy(BitConverter.GetBytes((BitConverter.ToInt16(savedata, 0xB762) & ~(0x3FF << 6)) | (opponent << 6)), 0, savedata, 0xB762, 2);
            string name = db.MonsList[BitConverter.ToInt16(db.StagesMain, 0x50 + BitConverter.ToInt32(db.StagesMain, 0x4) * opponent) & 0x3FF];
            string str = new string[] { "th", "st", "nd", "rd" }[(!(step > 10 && step < 14) && step % 10 < 4) ? step % 4 : 0];
            MessageBox.Show((!enabled ? "Survival Mode is disabled.\nYou should have faced" : "Survival mode enabled.\nYou'll face") + " survival mode's " + step + str + " step against " + name + " with " + (savedata[0xB768] & 0x7F) + " moves left.");
        }
    }
}