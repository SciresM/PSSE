using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using static Pokemon_Shuffle_Save_Editor.ToolFunctions;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Cheats : Form
    {
        public Cheats(Database db, ref byte[] savedata)
        {
            InitializeComponent();
            mondata = db.MonData;
            monlevel = db.MonLevel;
            stagesMain = db.StagesMain;
            stagesEvent = db.StagesEvent;
            stagesExpert = db.StagesExpert;
            megaStone = db.MegaStone;
            HasMega = db.HasMega;
            mons = db.Mons;
            megas = db.Megas;            
            megalist = db.MegaList;
            megaArray_start = db.MegaStartIndex;
            dtb = db;
            this.savedata = savedata;
        }

        private byte[] savedata, mondata, monlevel, stagesMain, stagesExpert, stagesEvent, megaStone;

        private Tuple<int, int, bool, int, int, int, int, Tuple<int>>[] mons; //specieIndex, formIndex, isMega, raiseMaxLevel, basePower, talent, type, stageNum
        private Tuple<int, int>[] megas; //monsIndex, speedups
        private List<int> megalist;
        private int megaArray_start;
        private bool[][] HasMega; // [X][0] = X, [X][1] = Y

        private Database dtb;

        private void B_CaughtEverything_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < megaArray_start; i++) //includes 15 reserved slots
                SetPokemon(ref savedata, dtb, i, true);
            MessageBox.Show("All Pokemon are now caught.");
        }

        private void B_CaughtObtainables_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < megaArray_start; i++) 
                SetPokemon(ref savedata, dtb, i, (mons[i].Rest.Item1 != 999) && ((mons[i].Item5 != 1) || (mons[i].Item6 != 1) || (mons[i].Item7 != 0))); //((displayed number isn't 999) && (at least 1 of these isn't "default" : base power, talent, type))
            int stagelen = BitConverter.ToInt32(stagesMain, 0x4);
            foreach (byte[] stage in new byte[][] { stagesMain, stagesExpert })
            {
                for (int i = 1; i < BitConverter.ToInt32(stage, 0); i++)
                {
                    int ind = BitConverter.ToUInt16(stage, 0x50 + stagelen * (i)) & 0x3FF;
                    SetPokemon(ref savedata, dtb, ind, true);
                }
            }                
            MessageBox.Show("All obtainable Pokemon have now been caught.");
        }

        private void B_AllStones_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < megaArray_start; i++)
            {
                if (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1])
                    SetMegaStone(ref savedata, dtb, i, HasMega[mons[i].Item1][0], HasMega[mons[i].Item1][1]);
            }
            MessageBox.Show("All Mega Stones are now owned.");
        }

        private void B_AllCaughtStones_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < megaArray_start; i++)
            {   //if (caught && (hasMegaX || hasMegaY) && (at least 1 of these not equals to "default" : talent, type, max speedups). Doesn't check if Y form has been released, but both Charizard's & Mewtwo's already have.
                if (GetPokemon(ref savedata, dtb, i) && (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1]) && ((mons[megaArray_start + megalist.IndexOf(i)].Item6 != 7) || (mons[megaArray_start + megalist.IndexOf(i)].Item7 != 0) || (megas[megalist.IndexOf(i)].Item2 != 1))) 
                    SetMegaStone(ref savedata, dtb, i, HasMega[mons[i].Item1][0], HasMega[mons[i].Item1][1]);                        
            }
            MessageBox.Show("All available megastones have been owned for everything you've caught.");
        }

        private void B_LevelMax_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < megaArray_start; i++)
            {
                if (GetPokemon(ref savedata, dtb, i))
                    SetLevel(ref savedata, dtb, i, 10 + Math.Min(mons[i].Item4, 5));
            }
            MessageBox.Show("Everything you've caught is now level Max.");
        }

        private void B_MaxResources_Click(object sender, EventArgs e)
        {
            int[] items = new int[7];
            for (int i = 0; i < 7; i++)
                items[i] = 99;
            int[] enhancements = new int[9];
            for (int i = 0; i < 9; i++)
                enhancements[i] = 99;
            SetResources(ref savedata, dtb, 99, 99999, 150, items, enhancements);
            MessageBox.Show("Gave 99 hearts, 99999 coins, 150 jewels, and 99 of every item.");
        }

        private void B_MaxSpeedups_Click(object sender, EventArgs e)
        {            
            for (int i = 0; i < megaArray_start; i++)
            {   //if (caught && (hasMegaX || hasMegaY) && (at least one stone owned))
                if (GetPokemon(ref savedata, dtb, i) && (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1]) && (GetMegaStone(ref savedata, dtb, i) > 0 || GetMegaStone(ref savedata, dtb, i) < 4))
                    SetMegaSpeedup(ref savedata, dtb, i, (HasMega[mons[i].Item1][0] && ((GetMegaStone(ref savedata, dtb, i) & 1) == 1)), (HasMega[mons[i].Item1][1] && ((GetMegaStone(ref savedata, dtb, i) & 2) == 2)));   //(hasMega(X/Y) && stone(X/Y) is owned)
            }
            MessageBox.Show("All Owned Megas (for which you own the stone too) have been fed with as much Mega Speedups as possible.");
        }

        private void B_AllCompleted_Click(object sender, EventArgs e)
        {
            int j = 0;
            foreach (byte[] stage in new byte[][] { stagesMain, stagesExpert })
            {
                for (int i = 0; i < (BitConverter.ToInt32(stage, 0) - 1); i++)
                {
                    SetStage(ref savedata, dtb, i, j, true);
                    PatchScore(ref savedata, dtb, i, j);
                }                    
                j++;
            }
            MessageBox.Show("All Normal & Expert stages have been marked as completed.\n\nRewards like megastones or jewels can still be redeemed by beating the stage.");
        }

        private void B_SRankCompleted_Click(object sender, EventArgs e) 
        {  
            int j = 0;
            foreach (byte[] stage in new byte[][] { stagesMain, stagesExpert })
            {
                int entrylen = BitConverter.ToInt32(stage, 0x4);
                for (int i = 0; i < (BitConverter.ToInt32(stage, 0) - ((stage == stagesMain) ? 1 : 0)); i++)
                {
                    if (GetStage(ref savedata, dtb, i, j))
                    {
                        SetRank(ref savedata, dtb, i, j, 3);
                        PatchScore(ref savedata, dtb, i, j);
                    }
                }
                j++;
            }
            MessageBox.Show("All Completed Normal & Expert stages have been S-ranked.");
        }

        private void B_StreetPassDelete_Click(object sender, EventArgs e)
        {
            Array.Copy(BitConverter.GetBytes(0x00), 0, savedata, 0x5967, 2); //Resets streetpass count to 0
            byte[] blank = new byte[0x68];
            for (int i = 0; i < 10; i++)
                Array.Copy(blank, 0, savedata, 0x59A7 + (i * 0x68), 0x68); //Erase StreetPass tags
            MessageBox.Show("StreetPass data have been cleared & StreetPass count reset to 0.");
        }

        private void B_MaxExcalationBattle_Click(object sender, EventArgs e)
        {
            SetExcalationStep(ref savedata, dtb, 999);
            MessageBox.Show("Curent escalation battle has been taken to step 999. You'll get all rewards at once by beating it.\n\nCarefull : only use it when there's exactly one active escalation battle.\nI don't know how this behaves if there is 0 or more than 1 active at the same time.");
        }
        
        private void B_PokemonReset_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < megaArray_start; i++) 
            {
                SetPokemon(ref savedata, dtb, i, false);    //Uncatch
                SetLevel(ref savedata, dtb, i); //Un-level, Un-experience & Un-lollipop
                if (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1])
                {
                    SetMegaStone(ref savedata, dtb, i); //Un-stone
                    SetMegaSpeedup(ref savedata, dtb, i);   //Unfeed speedups
                }
            }
            MessageBox.Show("All pokemons have been uncaught, reset to level 1 & lost their Mega Stones, speedups or lollipops.\n\nEither reset stages too or make sure to catch at least Espurr, Bulbasaur, Squirtle & Charmander manually.");
        }

        private void B_StageReset_Click(object sender, EventArgs e)
        {
            int j = 0;
            foreach (int length in new int[] { (BitConverter.ToInt32(stagesMain, 0) - 1), BitConverter.ToInt32(stagesExpert, 0), 100 })//{number of Main stages, number of Expert stages, 100}. Max number of event levels could be up to 549 but it's unlikely there ever are more than 100 at any given time (and that space could be used for something else later).
            {                
                for (int i = 0; i < length; i++)
                {
                    SetStage(ref savedata, dtb, i, j);
                    SetRank(ref savedata, dtb, i, j);
                    SetScore(ref savedata, dtb, i, j);
                }
                j++;
            }
            MessageBox.Show("All stages have been reset to C Rank, 0 score & uncompleted state.");
        }

        private void B_ResourcesReset_Click(object sender, EventArgs e)
        {            
            SetResources(ref savedata, dtb);
            MessageBox.Show("Deleted all stock hearts, coins, jewels and Items.");
        }

        private void B_EscalationReset_Click(object sender, EventArgs e)
        {
            SetExcalationStep(ref savedata, dtb);
            MessageBox.Show("Curent escalation battle has been reverted to step 1.\n\nCarefull : only use it when there's exactly one active escalation battle.\nI don't know how this behaves if there is 0 or more than 1 active at the same time.");
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

    }
}
