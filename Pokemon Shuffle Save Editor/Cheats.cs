using System;
using System.Linq;
using System.Windows.Forms;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Cheats : Form
    {
        private byte[] mondata;
        private byte[] monlevel;
        private byte[] stagesMain;
        private byte[] stagesEvent;
        private byte[] stagesExpert;
        private byte[] megaStone;
        public Cheats(byte[] md, byte[] ml, byte[] sm, byte[] sev, byte[] sex, byte[] ms, bool[][] hm, Tuple<int, int, bool, int, int, int, int, Tuple<int>>[] m, Tuple<int, int>[] mg, int[] mgl, ref byte[] sd)
        {
            InitializeComponent();
            mondata = md;
            monlevel = ml;
            stagesMain = sm;
            stagesEvent = sev;
            stagesExpert = sex;
            megaStone = ms;
            HasMega = hm;
            savedata = sd;
            mons = m;
            megas = mg;            
            megalist = mgl;            
        }
        bool[][] HasMega; // [X][0] = X, [X][1] = Y

        byte[] savedata;

        Tuple<int, int, bool, int, int, int, int, Tuple<int>>[] mons; //specieIndex, formIndex, isMega, raiseMaxLevel, basePower, talent, type, rest
        private Tuple<int, int>[] megas; //monsIndex, speedups
        int[] megalist; //derivate an int[] from megas.Item1 to use with ToList() functions (in UpdateForms() & UpdateOwnedBox()) because I don't know how of a "correct" way to do it

        private void B_CaughtEverything_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 883; i++) //includes 15 reserved slots
            {
                SetPokemon(i, true);
            }
            MessageBox.Show("All Pokemon are now caught.");
        }

        private void B_CaughtObtainables_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 883; i++) //includes 15 reserved slots
            {
                if ((mons[i].Rest.Item1 != 999) && ((mons[i].Item5 != 1) || (mons[i].Item6 != 1) || (mons[i].Item7 != 0)))
                {
                    SetPokemon(i, true);
                }
                else SetPokemon(i, false);
            };
            int stagelen = BitConverter.ToInt32(stagesMain, 0x4);
            int Num_MainStages = BitConverter.ToInt32(stagesMain, 0);
            int Num_ExpertStages = BitConverter.ToInt32(stagesExpert, 0);
            for (int i = 1; i < Num_MainStages; i++)
            {
                int ind = BitConverter.ToUInt16(stagesMain, 0x50 + stagelen * (i)) & 0x3FF;
                SetPokemon(ind, true);
            }
            for (int i = 1; i < Num_ExpertStages; i++)
            {
                int ind = BitConverter.ToUInt16(stagesExpert, 0x50 + stagelen * (i)) & 0x3FF;
                SetPokemon(ind, true);
            }
            MessageBox.Show("All obtainable Pokemon are now caught.");
        }

        private void B_AllStones_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 780; i++)
            {
                if (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1])
                {
                    SetMegaStone(i, HasMega[mons[i].Item1][0], HasMega[mons[i].Item1][1]);
                }
            }
            MessageBox.Show("All Mega Stones are now owned.");
        }

        private void B_AllCaughtStones_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 780; i++)
            {
                if (GetPokemon(i))
                {
                    if (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1])
                    {
                        SetMegaStone(i, HasMega[mons[i].Item1][0], HasMega[mons[i].Item1][1]);
                    }
                }
            }
            MessageBox.Show("All Mega Stones are now owned for everything you've caught.\n\nThis includes unreleased stones for released pokemons, be sure to uncheck them manually if you want to keep a legit save.");
        }

        private void B_LevelMax_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 883; i++) //Updated range
            {
                if (GetPokemon(i))
                {
                    //Reads the max amount of lollipops for that pokemon & set level to Max.
                    int numRaiseMaxLevel = Math.Min(mons[i].Item4, 5);
                    int max = 10 + numRaiseMaxLevel;
                    SetLevel(i, max);
                }
            }
            MessageBox.Show("Everything you've caught is now level Max.");
        }

        private void B_MaxResources_Click(object sender, EventArgs e)
        {
            int[] items = new int[7];
            for (int i = 0; i < 7; i++)
            {
                items[i] = 99;
            }
            int[] enhancements = new int[9];
            for (int i = 0; i < 9; i++)
            {
                enhancements[i] = 99;
            }
            SetResources(99, 99999, 150, items, enhancements);
            MessageBox.Show("Gave 99 hearts, 99999 coins, 150 jewels, and 99 of every item.");
        }

        private void B_MaxSpeedups_Click(object sender, EventArgs e)
        {            
            for (int i = 0; i < 780; i++)
            {
                if (GetPokemon(i))
                {
                    if (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1])
                    {
                        SetMegaSpeedup(i, HasMega[mons[i].Item1][0], HasMega[mons[i].Item1][1]);
                    }
                }
            }
            MessageBox.Show("All Owned Megas have been fed with as much Mega Speedups as possible.");
        }

        private void B_AllCompleted_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (BitConverter.ToInt32(stagesMain, 0) - 1); i++)
            {
                SetStage(i, 0, true);
            }
            for (int i = 0; i < BitConverter.ToInt32(stagesExpert, 0); i++)
            {
                SetStage(i, 1, true);
            }
            MessageBox.Show("All Normal & Expert stages have been marked as completed.\nRewards like megastones or jewels can still be redeemed by beating the stage.");
        }

        private void B_SRankCompleted_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < (BitConverter.ToInt32(stagesMain, 0) - 1); i++)
            {
                if (GetStage(i, 0))    
                    SetRank(i, 0, 3);
            }
            for (int i = 0; i < BitConverter.ToInt32(stagesExpert, 0); i++)
            {
                if (GetStage(i, 1))
                    SetRank(i, 1, 3);
            }
            MessageBox.Show("All Completed Normal & Expert stages have been S-ranked");
        }

        private void B_StreetPassDelete_Click(object sender, EventArgs e)
        {
            Array.Copy(BitConverter.GetBytes(0x0000), 0, savedata, 0x5967, 2); //Resets streetpass count to 0
            byte[] blank = new byte[0x68];
            for (int i = 0; i < 10; i++)
            {
                Array.Copy(blank, 0, savedata, 0x59A7 + (i * 0x68), 0x68); //Erase StreetPass tags
            }
            MessageBox.Show("StreetPass data have been cleared & StreetPass count reset to 0.");
        }

        private void B_MaxExcalationBattle_Click(object sender, EventArgs e)
        {
            SetExcalationStep(999);
            MessageBox.Show("Curent escalation battle has been taken to step 999. You'll get all rewards at once by beating it.\n\nCarefull : only use it when there's exactly one active escalation battle.\nI don't know how this behaves if there is 0 or more than 1 active at the same time.");
        }
        
        private void B_PokemonReset_Click(object sender, EventArgs e)
        {
            for (int i = 1; i < 883; i++) //Uncatch
            {
                SetPokemon(i, false);
            }
            for (int i = 0; i < 883; i++) //Un-level, Un-experience & Un-lollipop
            {
                SetLevel(i, 1);                
            }
            for (int i = 0; i < 780; i++) //Un-stone
            {
                if (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1])
                {
                    SetMegaStone(i, false, false);
                }
            }
            for (int i = 0; i < 780; i++) //Unfeed speedups
            {
                if (HasMega[mons[i].Item1][0] || HasMega[mons[i].Item1][1])
                    SetMegaSpeedup(i, false, false);
            }
            MessageBox.Show("All pokemons have been uncaught, reset to level 1 & lost their Mega Stones, speedups or lollipops.\n\nEither reset stages too or make sure to catch at least Espurr, Bulbasaur, Squirtle & Charmander manually.");
        }

        private void B_StageReset_Click(object sender, EventArgs e)
        {
            long score = 0;
            for (int i = 0; i < (BitConverter.ToInt32(stagesMain, 0) - 1); i++)
            {
                SetStage(i, 0, false);
                SetRank(i, 0, 0);
                Array.Copy(BitConverter.GetBytes(score), 0, savedata, 0x4141 + (3 * i), 8);
            }
            for (int i = 0; i < BitConverter.ToInt32(stagesExpert, 0); i++)
            {
                SetStage(i, 1, false);
                SetRank(i, 1, 0);
                Array.Copy(BitConverter.GetBytes(score), 0, savedata, 0x4F51 + (3 * i), 8);
            }
            for (int i = 0; i < 200; i++) //max number of event levels should be 549 but 200 should be enough at any time
            {
                SetStage(i, 2, false);
                SetRank(i, 2, 0);
                Array.Copy(BitConverter.GetBytes(score), 0, savedata, 0x52D5 + (3 * i), 8);
            }
            MessageBox.Show("All stages have been reset to C Rank, 0 score & uncompleted state.\n\n.");
        }

        private void B_ResourcesReset_Click(object sender, EventArgs e)
        {            
            SetResources();
            MessageBox.Show("Deleted all stock hearts, coins, jewels and Items.");
        }

        private void SetLevel(int ind, int lev)
        {
            int level_ofs = (((ind - 1) * 4) / 8);
            int level_shift = ((((ind - 1) * 4) + 1) % 8);
            ushort level = BitConverter.ToUInt16(savedata, 0x187 + level_ofs);
            level = (ushort)((level & (ushort)(~(0xF << level_shift))) | (lev << level_shift));
            Array.Copy(BitConverter.GetBytes(level), 0, savedata, 0x187 + level_ofs, 2);

            //experience patcher
            int exp_ofs = ((4 + ((ind - 1) * 24)) / 8);
            int exp_shift = ((4 + ((ind - 1) * 24)) % 8);
            int exp = BitConverter.ToInt32(savedata, 0x3241 + exp_ofs);
            int entrylen = BitConverter.ToInt32(monlevel, 0x4);
            byte[] data = monlevel.Skip(0x50 + ((lev - 1) * entrylen)).Take(entrylen).ToArray();
            int set_exp = BitConverter.ToInt32(data, 0x4 * (mons[ind].Item5 - 1));
            exp = (exp & ~(0xFFFFFF << exp_shift)) | (set_exp << exp_shift);
            Array.Copy(BitConverter.GetBytes(exp), 0, savedata, 0x3241 + exp_ofs, 4);
            
            //lollipop patcher
            int rml_ofs = ((ind * 6) / 8);
            int rml_shift = ((ind * 6) % 8);
            ushort numRaiseMaxLevel = BitConverter.ToUInt16(savedata, 0xA9DB + rml_ofs);            
            int set_rml = Math.Min((lev - 10 < 0) ? 0 : (lev - 10), 5); //hardcoded 5 as the max number of lollipops, change this if needed
            numRaiseMaxLevel = (ushort)((numRaiseMaxLevel & (ushort)(~(0x3F << rml_shift))) | (set_rml << rml_shift));
            Array.Copy(BitConverter.GetBytes(numRaiseMaxLevel), 0, savedata, 0xA9DB + rml_ofs, 2);            
        }

        private void SetPokemon(int ind, bool caught)
        {
            int caught_ofs = (((ind - 1) + 6) / 8);
            int caught_shift = (((ind - 1) + 6) % 8);
            foreach (int caught_array_start in new[] { 0xE6, 0x546, 0x5E6 })
            {
                savedata[caught_array_start + caught_ofs] = (byte)(savedata[caught_array_start + caught_ofs] & (byte)(~(1 << caught_shift)) | ((caught ? 1 : 0) << caught_shift));
            }
        }

        private bool GetPokemon(int ind)
        {
            int caught_ofs = 0x546 + (((ind - 1) + 6) / 8);
            return ((savedata[caught_ofs] >> ((((ind - 1) + 6) % 8))) & 1) == 1;
        }

        private void SetMegaStone(int ind, bool X, bool Y)
        {
            int mega_ofs = 0x406 + ((ind + 2) / 4);
            ushort mega_val = BitConverter.ToUInt16(savedata, mega_ofs);
            mega_val &= (ushort)(~(3 << ((5 + (ind << 1)) % 8)));
            ushort new_mega_insert = (ushort)(0 | (X ? 1 : 0) | (Y ? 2 : 0));
            mega_val |= (ushort)(new_mega_insert << ((5 + (ind << 1)) % 8));
            Array.Copy(BitConverter.GetBytes(mega_val), 0, savedata, mega_ofs, 2);
        }

        private void SetMegaSpeedup(int ind, bool X, bool Y)
        {
            if (HasMega[mons[ind].Item1][0] || HasMega[mons[ind].Item1][1])
            {
                int suX_ofs = (((megalist.ToList().IndexOf(ind) * 7) + 3) / 8);
                int suX_shift = (((megalist.ToList().IndexOf(ind) * 7) + 3) % 8);
                int suY_ofs = (((megalist.ToList().IndexOf(ind, megalist.ToList().IndexOf(ind) + 1) * 7) + 3) / 8);
                int suY_shift = (((megalist.ToList().IndexOf(ind, megalist.ToList().IndexOf(ind) + 1) * 7) + 3) % 8) + ((suY_ofs - suX_ofs) * 8); //relative to suX_ofs
                int speedUp_ValX = BitConverter.ToInt32(savedata, 0x2D5B + suX_ofs);
                int speedUp_ValY = BitConverter.ToInt32(savedata, 0x2D5B + suY_ofs);
                int set_suX = X ? megas[megalist.ToList().IndexOf(ind)].Item2 : 0;
                int set_suY = Y ? megas[megalist.ToList().IndexOf(ind, megalist.ToList().IndexOf(ind) + 1)].Item2 : 0;
                int newSpeedUp = HasMega[mons[ind].Item1][1]
                    ? ((((speedUp_ValX & ~(0x7F << suX_shift)) & ~(0x7F << suY_shift)) | (set_suX << suX_shift)) | (set_suY << suY_shift)) //Erases both X & Y bits at the same time before updating them to make sure Y doesn't overwrite X bits
                    : (speedUp_ValX & ~(0x7F << suX_shift)) | (set_suX << suX_shift);
                Array.Copy(BitConverter.GetBytes(newSpeedUp), 0, savedata, 0x2D5B + suX_ofs, 4);
            }
        }

        private void SetStage(int ind, int type, bool completed)
        {
            int base_ofs = default(int);
            switch (type)
            {
                case 0:
                    base_ofs = 0x688; //Main
                    break;
                case 1:
                    base_ofs = 0x84A; //Expert
                    break;
                case 2:
                    base_ofs = 0x8BA; //Event
                    break;
                default:
                    return;
            }
            int stage_ofs = base_ofs + ((ind * 3) / 8);
            int stage_shift = (ind * 3) % 8; 
            ushort stage = BitConverter.ToUInt16(savedata, stage_ofs);
            int status = completed ? 5 : 0;
            stage = (ushort)((stage & (ushort)(~(0x7 << stage_shift))) | (status << stage_shift));
            Array.Copy(BitConverter.GetBytes(stage), 0, savedata, stage_ofs, 2);
        }

        private bool GetStage(int ind, int type)
        {
            int base_ofs = default(int);
            switch (type)
            {
                case 0:
                    base_ofs = 0x688; //Main
                    break;
                case 1:
                    base_ofs = 0x84A; //Expert
                    break;
                case 2:
                    base_ofs = 0x8BA; //Event
                    break;
                default:
                    return false;
            }
            int stage_ofs = base_ofs + ((ind * 3) / 8);
            int stage_shift = (ind * 3) % 8;
            return ((BitConverter.ToInt16(savedata, stage_ofs) >> stage_shift) & 7) == 5;
        }

        private void SetRank(int ind, int type, int status)
        {
            int base_ofs = default(int);
            switch (type)
            {
                case 0:
                    base_ofs = 0x688; //Main
                    break;
                case 1:
                    base_ofs = 0x84A; //Expert
                    break;
                case 2:
                    base_ofs = 0x8BA; //Event
                    break;
                default:
                    return;
            }
            int rank_ofs = base_ofs + ((7 + (ind * 2)) / 8);
            int rank_shift = (7 + (ind * 2)) % 8;
            ushort rank = BitConverter.ToUInt16(savedata, rank_ofs);
            rank = (ushort)((rank & (ushort)(~(0x3 << rank_shift))) | (status << rank_shift));
            Array.Copy(BitConverter.GetBytes(rank), 0, savedata, rank_ofs, 2);
        }

        private void SetResources(int hearts = 0, uint coins = 0, uint jewels = 0, int[] items = null, int[] enhancements = null)
        {
            if (items == null)
                items = new int[7];
            if (enhancements == null)
                enhancements = new int[9];
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt32(savedata, 0x68) & 0xF0000007) | (coins << 3) | (jewels << 20)), 0, savedata, 0x68, 4);
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt16(savedata, 0x2D4A) & 0xC07F) | (hearts << 7)), 0, savedata, 0x2D4A, 2);
            for (int i = 0; i < 7; i++) //Items (battle)
            {
                ushort val = BitConverter.ToUInt16(savedata, 0xd0 + i);
                val &= 0x7F;
                val |= (ushort)(items[i] << 7);
                Array.Copy(BitConverter.GetBytes(val), 0, savedata, 0xd0 + i, 2);
            }
            for (int i = 0; i < 9; i++) //Enhancements (pokemon)
            {
                savedata[0x2D4C + i] = (byte)((((enhancements[i]) << 1) & 0xFE) | (savedata[0x2D4C + i] & 1));
            }
        }

        private void SetExcalationStep (int step = 0)
        {
            if (step < 0)
                step = 0;
            if (step > 999)
                step = 999;
            int data = BitConverter.ToUInt16(savedata, 0x2D59);
            data = (data & (~(0x3FF << 2))) | (step << 2);
            Array.Copy(BitConverter.GetBytes(data), 0, savedata, 0x2D59, 2);
        }

        private void B_EscalationReset_Click(object sender, EventArgs e)
        {
            SetExcalationStep();
            MessageBox.Show("Curent escalation battle has been reverted to step 1.\n\nCarefull : only use it when there's exactly one active escalation battle.\nI don't know how this behaves if there is 0 or more than 1 active at the same time.");
        }

    }
}
