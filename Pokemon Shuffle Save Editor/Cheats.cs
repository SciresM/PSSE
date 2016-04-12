using System;
using System.Windows.Forms;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Cheats : Form
    {
        private byte[] mondata;
        private byte[] stagesMain;
        private byte[] stagesEvent;
        private byte[] stagesExpert;
        private byte[] megaStone;
        public Cheats(byte[] md, byte[] sm, byte[] sev, byte[] sex, byte[] ms, bool[][] hm, Tuple<int, int, bool>[] m, ref byte[] sd)
        {
            InitializeComponent();
            mondata = md;
            stagesMain = sm;
            stagesEvent = sev;
            stagesExpert = sex;
            megaStone = ms;
            HasMega = hm;
            savedata = sd;
            mons = m;
        }
        bool[][] HasMega; // [X][0] = X, [X][1] = Y

        byte[] savedata;

        Tuple<int, int, bool>[] mons;

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
            MessageBox.Show("All obtainable non-event Pokemon are now caught.");
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
            MessageBox.Show("All Mega Stones are now owned for everything you've caught.");
        }

        private void B_Level10_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < 883; i++) //Updated range
            {
                if (GetPokemon(i))
                {
                    //Reads the amount of lollipops used on that pokemon & set level to current Max.
                    int numRaiseMaxLevel = Math.Min(((BitConverter.ToUInt16(savedata, 0xA9DB + ((i * 6) / 8)) >> ((i * 6) % 8)) & 0x3F), 5);
                    int max = 10 + numRaiseMaxLevel;
                    SetLevel(i, max);
                }
            }
            MessageBox.Show("Everything you've caught is now level Max.");
        }

        private void B_MaxResources_Click(object sender, EventArgs e)
        {
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt32(savedata, 0x68) & 0xF0000007) | ((uint)99999 << 3) | ((uint)150 << 20)), 0, savedata, 0x68, 4);
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt16(savedata, 0x2D4A) & 0xC07F) | (99 << 7)), 0, savedata, 0x2D4A, 2);
            for (int i = 0; i < 7; i++)
            {
                ushort val = BitConverter.ToUInt16(savedata, 0xd0 + i);
                val &= 0x7F;
                val |= (99 << 7);
                Array.Copy(BitConverter.GetBytes(val), 0, savedata, 0xd0 + i, 2);
            }
            for (int i = 0; i < 9; i++)
            {
                savedata[0x2D4C + i] = (byte)((((99) << 1) & 0xFE) | (savedata[0x2D4C + i] & 1)); // Mega Speedups & other items
            }
            MessageBox.Show("Gave 99 hearts, 99999 coins, 150 jewels, and 99 of every item.");
        }

        private void SetLevel(int ind, int lev)
        {
            int level_ofs = (((ind - 1) * 4) / 8);
            int level_shift = ((((ind - 1) * 4) + 1) % 8);
            ushort level = BitConverter.ToUInt16(savedata, 0x187 + level_ofs);
            level = (ushort)((level & (ushort)(~(0xF << level_shift))) | (lev << level_shift));
            Array.Copy(BitConverter.GetBytes(level), 0, savedata, 0x187 + level_ofs, 2);
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

        private void SetMegaStone(int ind, bool Y, bool X)
        {
            int mega_ofs = 0x406 + ((ind + 2) / 4);
            ushort mega_val = BitConverter.ToUInt16(savedata, mega_ofs);
            mega_val &= (ushort)(~(3 << ((5 + (ind << 1)) % 8)));
            ushort new_mega_insert = (ushort)(0 | (Y ? 1 : 0) | (X ? 2 : 0));
            mega_val |= (ushort)(new_mega_insert << ((5 + (ind << 1)) % 8));
            Array.Copy(BitConverter.GetBytes(mega_val), 0, savedata, mega_ofs, 2);
        }

    }
}
