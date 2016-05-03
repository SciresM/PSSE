using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
            string resourcedir  = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location)+Path.DirectorySeparatorChar+"resources"+Path.DirectorySeparatorChar;
            if (!Directory.Exists(resourcedir))
                Directory.CreateDirectory(resourcedir);
            byte[][] files = { megaStone, mondata, stagesMain, stagesEvent, stagesExpert, monlevel };
            string[] filenames = { "megaStone.bin","pokemonData.bin","stageData.bin","stageDataEvent.bin","stageDataExtra.bin", "pokemonLevel.bin" };
            for (int i=0;i<files.Length;i++)
            {
                if (!File.Exists(resourcedir + filenames[i]))
                    File.WriteAllBytes(resourcedir + filenames[i], files[i]);
                else
                {
                    switch (i)
                    {
                        case 0:
                            megaStone = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 1:
                            mondata = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 2:
                            stagesMain = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 3:
                            stagesEvent = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 4:
                            stagesExpert = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 5:
                            monlevel = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                    }
                }
            }
            PB_Main.Image = PB_Event.Image = PB_Expert.Image = GetStageImage(0);
            PB_Team1.Image = PB_Team2.Image = PB_Team3.Image = PB_Team4.Image = ResizeImage(GetMonImage(0), 48, 48);
            specieslist = Properties.Resources.species.Split(new[] {Environment.NewLine, "\n"}, StringSplitOptions.RemoveEmptyEntries);
            monslist = Properties.Resources.mons.Split(new[] { Environment.NewLine, "\n"}, StringSplitOptions.RemoveEmptyEntries);
            megaArray_start = monslist.ToList().IndexOf("Mega Venusaur");
            int monArray_stop = monslist.ToList().IndexOf("---", monslist.ToList().IndexOf("---") + 1); //Indexes of first mega & second "---", respectively, should allow PSSE to work longer without needing an update.          
            HasMega = new bool[specieslist.Length][];
            for (int i = 0; i < specieslist.Length; i++)
                HasMega[i] = new bool[2];
            int entrylen = BitConverter.ToInt32(mondata, 0x4);
            megas = new Tuple<int, int>[BitConverter.ToUInt32(megaStone, 0)-1];
            for (int i = 0; i < megas.Length; i++)
            {
                int monIndex = BitConverter.ToUInt16(megaStone, 0x54 + i * 4) & 0x3FF;
                byte[] data = mondata.Skip(0x50 + entrylen * (i+monArray_stop)).Take(entrylen).ToArray();
                int maxSpeedup = (BitConverter.ToInt32(data, 0xA) >> 7) & 0x7F;
                megas[i] = new Tuple<int, int>(monIndex, maxSpeedup);
            }
            megalist = new List<int>(); 
            for (int i = 0; i < megas.Length; i++)
                megalist.Add(megas[i].Item1);   //derivate a List from megas.Item1 to use with IndexOf() functions (in UpdateForms() & UpdateOwnedBox())
            int[] forms = new int[specieslist.Length];
            mons = new Tuple<int, int, bool, int, int, int, int, Tuple<int>>[BitConverter.ToUInt32(mondata, 0)];
            rest = new Tuple<int>[BitConverter.ToUInt32(mondata, 0)];
            for (int i = 0; i < mons.Length; i++)
            {
                byte[] data = mondata.Skip(0x50 + entrylen * i).Take(entrylen).ToArray();
                bool isMega = i >= megaArray_start && i <= megaArray_start + megas.Length - 1; 
                int spec = isMega
                    ? specieslist.ToList().IndexOf(monslist[megas[i - megaArray_start].Item1].Replace("Shiny","").Replace("Winking","").Replace("Smiling","").Replace(" ","")) //crappy but needed for IndexOf() to find the pokemon's name in specieslist (only adjectives on megas names matter)
                    : (BitConverter.ToInt32(data, 0xE) >> 6) & 0x7FF; 
                int raiseMaxLevel = (BitConverter.ToInt16(data, 0x4)) & 0x3F;
                int basePower = (BitConverter.ToInt16(data, 0x3)) & 0x7; //ranges 1-7 for now (30-90 BP), may need an update later on
                int talent = (BitConverter.ToInt16(data, 0x02)) & 0x7F; //ranges 1-~100 for now ("Opportunist" to "Transform"), ordered list in MESSAGE_XX/09 (0x44C-C76 for US)
                int type = (BitConverter.ToInt16(data, 0x01) >> 3) & 0x1F; //ranges 0-17 (normal - fairy) (https://gbatemp.net/threads/psse-pokemon-shuffle-save-editor.396499/page-33#post-6278446)
                int index = (BitConverter.ToInt16(data, 0)) & 0x3FF; //ranges 1-999, it's the number you can see on the team selection menu
                rest[i] = new Tuple<int>(index); //mons has more than 7 arguments so 8th one and beyond must be included in another Tuple
                mons[i] = new Tuple<int, int, bool, int, int, int, int, Tuple<int>>(spec, forms[spec], isMega, raiseMaxLevel, basePower, talent, type, rest[i]);
                forms[spec]++;
            }
            for (int i = 0; i < megas.Length; i++)
                HasMega[mons[BitConverter.ToUInt16(megaStone, 0x54 + i * 4) & 0x3FF].Item1][(megaStone[0x54 + (i * 4) + 1] >> 3) & 1] = true;
            monsel = new List<cbItem>();
            for (int i = 1; i < monArray_stop; i++)
                monsel.Add(new cbItem { Text = monslist[i], Value = i });
            monsel = monsel.OrderBy(ncbi => ncbi.Text).ToList();
            CB_MonIndex.DataSource = monsel;
            CB_MonIndex.DisplayMember = "Text";
            CB_MonIndex.ValueMember = "Value";
            NUP_MainIndex.Minimum = NUP_ExpertIndex.Minimum = 1;
            NUP_EventIndex.Minimum = 0;
            NUP_MainIndex.Maximum = BitConverter.ToInt32(stagesMain, 0) - 1;
            NUP_ExpertIndex.Maximum = BitConverter.ToInt32(stagesExpert, 0);
            NUP_EventIndex.Maximum = BitConverter.ToInt32(stagesEvent, 0) - 1;
            NUP_MainScore.Minimum = NUP_ExpertScore.Minimum = NUP_EventScore.Minimum = 0;
            NUP_MainScore.Maximum = NUP_ExpertScore.Maximum = NUP_EventScore.Maximum = 0xFFFFFF;
            CHK_MegaY.Visible = CHK_MegaX.Visible = NUP_SpeedUpX.Visible = NUP_SpeedUpY.Visible = false;
            NUP_SpeedUpX.Minimum = NUP_SpeedUpY.Minimum = 0;
            NUP_SpeedUpX.Maximum = NUP_SpeedUpY.Maximum = 127;
            PB_Mon.Image = GetCaughtImage((int)CB_MonIndex.SelectedValue, CHK_CaughtMon.Checked);
            ItemsGrid.SelectedObject = null;
        }

        Tuple<int, int, bool, int, int, int, int, Tuple<int>>[] mons; //specieIndex, formIndex, isMega, raiseMaxLevel, basePower, talent, type, rest
        Tuple<int>[] rest; //stageNum

        byte[] mondata = Properties.Resources.pokemonData;
        byte[] stagesMain = Properties.Resources.stageData;
        byte[] stagesEvent = Properties.Resources.stageDataEvent;
        byte[] stagesExpert = Properties.Resources.stageDataExtra;
        byte[] megaStone = Properties.Resources.megaStone;
        byte[] monlevel = Properties.Resources.pokemonLevel;

        string[] specieslist, monslist;
        int megaArray_start;
        List<cbItem> monsel;

        bool[][] HasMega; // [X][0] = X, [X][1] = Y
        Tuple<int, int>[] megas; //monsIndex, speedups
        List<int> megalist; 

        ShuffleItems SI_Items = new ShuffleItems();

        byte[] savedata;
        bool loaded;
        private bool updating;

        private void B_Open_Click(object sender, EventArgs e)
        {
            TB_FilePath.Text = string.Empty;
            B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = false;

            OpenFileDialog ofd = new OpenFileDialog {FileName = "savedata.bin"};
            if (ofd.ShowDialog() != DialogResult.OK) return;
            if (IsShuffleSave(ofd.FileName)) { Open(ofd.FileName); }
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;
            updating = true;
            SaveFileDialog sfd = new SaveFileDialog {FileName = TB_FilePath.Text};
            if (sfd.ShowDialog() != DialogResult.OK) return;

            File.WriteAllBytes(sfd.FileName, savedata);
            MessageBox.Show("Saved save file to " + sfd.FileName + "."+Environment.NewLine+"Remember to delete secure value before importing.");
            updating = false;
        }

        private void Open(string file)
        {
            TB_FilePath.Text = file;
            savedata = File.ReadAllBytes(file);
            Parse();
            B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = true;
            UpdateProperty(null, null);
        }
              
        private bool IsShuffleSave(string file) // Try to do a better job at filtering files rather than just saying "oh, it's not savedata.bin quit"
        {
            FileInfo info = new FileInfo(file);
            if (info.Length != 74807) return false; // Probably not

            var contents = new byte[8];
            File.OpenRead(file).Read(contents, 0, contents.Length);
            return BitConverter.ToInt64(contents, 0) == 0x4000000009L;
        }

        private void Parse()
        {
            UpdateResourceBox();
            UpdateStageBox();
            UpdateOwnedBox();
        }

        private void UpdateForm(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;
            updating = true;
            if (sender != null && !((sender as Control).Name.ToLower().Contains("index")))
            {
                int ind = (int)CB_MonIndex.SelectedValue;

                //level patcher
                int level_ofs = 0x187 + (((ind - 1) * 4) / 8);
                int level_shift = ((((ind - 1) * 4) + 1) % 8);
                ushort level = BitConverter.ToUInt16(savedata, level_ofs);                
                int set_level = CHK_CaughtMon.Checked ? ((int)NUP_Level.Value == 1 ? 0 : (int)NUP_Level.Value) : 0;
                level = (ushort)((level & (ushort)(~(0xF << level_shift))) | (set_level << level_shift));
                Array.Copy(BitConverter.GetBytes(level), 0, savedata, level_ofs, 2);

                //experience patcher
                int exp_ofs = 0x3241 + (4 + (ind - 1) * 24) / 8;
                int exp_shift = (4 + (ind - 1) * 24) % 8;
                int exp = BitConverter.ToInt32(savedata, exp_ofs);
                int entrylen = BitConverter.ToInt32(monlevel, 0x4);
                byte[] data = monlevel.Skip(0x50 + ((int)NUP_Level.Value - 1) * entrylen).Take(entrylen).ToArray();
                int set_exp = BitConverter.ToInt32(data, 0x4 * (mons[ind].Item5 - 1));
                exp = (exp & ~(0xFFFFFF << exp_shift)) | (set_exp << exp_shift);
                Array.Copy(BitConverter.GetBytes(exp), 0, savedata, exp_ofs, 4);

                //lollipop patcher
                int rml_ofs = 0xA9DB + (ind * 6) / 8;
                int rml_shift = (ind * 6) % 8;
                ushort numRaiseMaxLevel = BitConverter.ToUInt16(savedata, rml_ofs);
                int set_rml = Math.Min(((int)NUP_Level.Value - 10 < 0) ? 0 : (int)NUP_Level.Value - 10, 5); //Hardcoded 5 as the max number of lollipops, change this if needed later
                numRaiseMaxLevel = (ushort)((numRaiseMaxLevel & (ushort)(~(0x3F << rml_shift))) | (set_rml << rml_shift));
                Array.Copy(BitConverter.GetBytes(numRaiseMaxLevel), 0, savedata, rml_ofs, 2);

                //caught patcher
                int caught_ofs = (ind - 1 + 6) / 8;
                int caught_shift = (ind - 1 + 6) % 8;
                foreach (int caught_array_start in new[] { 0xE6, 0x546, 0x5E6 })
                    savedata[caught_array_start + caught_ofs] = (byte)(savedata[caught_array_start + caught_ofs] & (byte)(~(1 << caught_shift)) | ((CHK_CaughtMon.Checked ? 1 : 0) << caught_shift));

                //megastone patcher
                int mega_ofs = 0x406 + (ind + 2) / 4;
                ushort mega_val = BitConverter.ToUInt16(savedata, mega_ofs);
                mega_val &= (ushort)(~(3 << ((5 + (ind << 1)) % 8)));
                ushort new_mega_insert = (ushort)(0 | (CHK_MegaX.Checked ? 1 : 0) | (CHK_MegaY.Checked ? 2 : 0));
                mega_val |= (ushort)(new_mega_insert << ((5 + (ind << 1)) % 8));
                Array.Copy(BitConverter.GetBytes(mega_val), 0, savedata, mega_ofs, 2);

                //speedups patcher
                if (HasMega[mons[ind].Item1][0] || HasMega[mons[ind].Item1][1])
                {
                    int suX_ofs = 0x2D5B + (megalist.IndexOf(ind) * 7 + 3) / 8;
                    int suX_shift = (megalist.IndexOf(ind) * 7 + 3) % 8;
                    int suY_ofs = 0x2D5B + ((megalist.IndexOf(ind, megalist.IndexOf(ind) + 1) * 7) + 3) / 8;
                    int suY_shift = ((megalist.IndexOf(ind, megalist.IndexOf(ind) + 1) * 7) + 3) % 8 + (suY_ofs - suX_ofs) * 8; //relative to suX_ofs
                    int speedUp_ValX = BitConverter.ToInt32(savedata, suX_ofs);
                    int speedUp_ValY = BitConverter.ToInt32(savedata, suY_ofs);
                    int set_suX = (HasMega[mons[ind].Item1][0] && CHK_CaughtMon.Checked && CHK_MegaX.Checked) ? (int)Math.Min(NUP_SpeedUpX.Value, NUP_SpeedUpX.Maximum) : 0;
                    int set_suY = (HasMega[mons[ind].Item1][1] && CHK_CaughtMon.Checked && CHK_MegaY.Checked) ? (int)Math.Min(NUP_SpeedUpY.Value, NUP_SpeedUpY.Maximum) : 0;
                    int newSpeedUp = HasMega[mons[ind].Item1][1]
                        ? ((speedUp_ValX & ~(0x7F << suX_shift)) & ~(0x7F << suY_shift)) | (set_suX << suX_shift) | (set_suY << suY_shift) //Erases both X & Y bits at the same time before updating them to make sure Y doesn't overwrite X bits
                        : (speedUp_ValX & ~(0x7F << suX_shift)) | (set_suX << suX_shift);
                    Array.Copy(BitConverter.GetBytes(newSpeedUp), 0, savedata, suX_ofs, 4);
                }
                
                //score patcher
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(savedata, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_MainScore.Value << 4))), 0, savedata, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1), 8);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(savedata, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_ExpertScore.Value << 4))), 0, savedata, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value), 8);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(savedata, 0x52D5 + 3 * ((int)NUP_EventIndex.Value)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_EventScore.Value << 4))), 0, savedata, 0x52D5 + 3 * ((int)NUP_EventIndex.Value), 8);

                //items patcher
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt32(savedata, 0x68) & 0xF0000007) | ((uint)NUP_Coins.Value << 3) | ((uint)NUP_Jewels.Value << 20)), 0, savedata, 0x68, 4);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt16(savedata, 0x2D4A) & 0xC07F) | ((ushort)NUP_Hearts.Value << 7)), 0, savedata, 0x2D4A, 2);
                for (int i = 0; i < SI_Items.Items.Length; i++)
                {
                    ushort val = BitConverter.ToUInt16(savedata, 0xd0 + i);
                    val &= 0x7F;
                    val |= (ushort)(SI_Items.Items[i] << 7);
                    Array.Copy(BitConverter.GetBytes(val), 0, savedata, 0xd0 + i, 2);
                }
                for (int i = 0; i < SI_Items.Enchantments.Length; i++)
                    savedata[0x2D4C + i] = (byte)(((SI_Items.Enchantments[i] << 1) & 0xFE) | (savedata[0x2D4C + i] & 1));
            }
            Parse();
            updating = false;
        }

        private void UpdateResourceBox()
        {
            NUP_Coins.Value = (BitConverter.ToUInt32(savedata, 0x68) >> 3) & 0x1FFFF;
            NUP_Jewels.Value = (BitConverter.ToUInt16(savedata, 0x6A) >> 4) & 0xFF;
            NUP_Hearts.Value = (BitConverter.ToUInt16(savedata, 0x2D4A) >> 7) & 0x7F;
            for (int i = 0; i < SI_Items.Items.Length; i++)
                SI_Items.Items[i] = (BitConverter.ToUInt16(savedata, 0xD0 + i) >> 7) & 0x7F;
            for (int i = 0; i < SI_Items.Enchantments.Length; i++)
                SI_Items.Enchantments[i] = (savedata[0x2D4C + i] >> 1) & 0x7F;  
        }

        private void UpdateOwnedBox()
        {
            int ind = (int)CB_MonIndex.SelectedValue;
            
            //team preview
            byte[] teamData = savedata.Skip(0xE0).Take(0x7).ToArray();
            int teamSlot1 = (BitConverter.ToInt32(teamData, 0) >> 5) & 0xFFF;
            int teamSlot2 = (BitConverter.ToInt16(teamData, 0x02) >> 1) & 0xFFF;
            int teamSlot3 = (BitConverter.ToInt32(teamData, 0x03) >> 5) & 0xFFF;
            int teamSlot4 = (BitConverter.ToInt16(teamData, 0x05) >> 1) & 0xFFF;
            PB_Team1.Image = ResizeImage(GetMonImage(mons[teamSlot1].Item1, mons[teamSlot1].Item2), 48, 48);
            PB_Team2.Image = ResizeImage(GetMonImage(mons[teamSlot2].Item1, mons[teamSlot2].Item2), 48, 48);
            PB_Team3.Image = ResizeImage(GetMonImage(mons[teamSlot3].Item1, mons[teamSlot3].Item2), 48, 48);
            PB_Team4.Image = ResizeImage(GetMonImage(mons[teamSlot4].Item1, mons[teamSlot4].Item2), 48, 48);
            
            //level view
            int level_ofs = 0x187 + (ind - 1) * 4 / 8;
            int level = (BitConverter.ToUInt16(savedata, level_ofs) >> (((ind - 1) * 4) + 1) % 8) & 0xF;
            int num_raise_max_level = Math.Min(mons[ind].Item4, 5); //int num_raise_max_level = Math.Min(((BitConverter.ToUInt16(savedata, 0xA9DB + ((ind * 6) / 8)) >> ((ind * 6) % 8)) & 0x3F), 5); -> old one, uses current number of lollipops given
            NUP_Level.Maximum = 10 + num_raise_max_level;   // The max on the box could be higher than 10 now  
            NUP_Level.Value = level > 0 ? level : 1;    // Stop showing 0 for the level...

            //caught CHK
            int caught_ofs = 0x546 + (ind-1+6)/8;
            CHK_CaughtMon.Checked = ((savedata[caught_ofs] >> ((ind-1+6) % 8)) & 1) == 1;   
                     
            NUP_Level.Visible = CHK_CaughtMon.Checked; 
            PB_Mon.Image = GetCaughtImage(ind, CHK_CaughtMon.Checked);

            #region Mega Visibility
            PB_MegaX.Visible = CHK_MegaX.Visible = HasMega[mons[ind].Item1][0];
            PB_MegaY.Visible = CHK_MegaY.Visible = HasMega[mons[ind].Item1][1];
            PB_MegaX.Image = HasMega[mons[ind].Item1][0] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + mons[ind].Item1.ToString("000") + (HasMega[mons[ind].Item1][1] ? "_X" : string.Empty))) : new Bitmap(16, 16);
            PB_MegaY.Image = HasMega[mons[ind].Item1][1] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + mons[ind].Item1.ToString("000") + "_Y")) : new Bitmap(16, 16);
            PB_SpeedUpX.Image = HasMega[mons[ind].Item1][0] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);
            PB_SpeedUpY.Image = HasMega[mons[ind].Item1][1] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);
            int mega_ofs = 0x406 + (ind + 2) / 4;
            CHK_MegaX.Checked = ((BitConverter.ToUInt16(savedata, mega_ofs) >> (5 + (ind << 1)) % 8) & 1) == 1;
            CHK_MegaY.Checked = (((BitConverter.ToUInt16(savedata, mega_ofs) >> (5 + (ind << 1)) % 8) >> 1) & 1) == 1; 
            NUP_SpeedUpX.Visible = PB_SpeedUpX.Visible = CHK_MegaX.Visible && CHK_MegaX.Checked && CHK_CaughtMon.Checked;
            NUP_SpeedUpY.Visible = PB_SpeedUpY.Visible = CHK_MegaY.Visible && CHK_MegaY.Checked && CHK_CaughtMon.Checked; //Else NUP_SpeedUpY appears if the next mega in terms of offsets has been obtained
            #endregion  
            if (megalist.IndexOf(ind) != -1) //temporary fix while there are still some mega forms missing in megastone.bin
            {
                int suX_ofs = ((megalist.IndexOf(ind) * 7) + 3) / 8;
                int suX_shift = ((megalist.IndexOf(ind) * 7) + 3) % 8;
                int suY_ofs = ((megalist.IndexOf(ind, megalist.IndexOf(ind) + 1) * 7) + 3) / 8; //looped IndexOf() to get the second occurence of ind
                int suY_shift = ((megalist.IndexOf(ind, megalist.IndexOf(ind) + 1) * 7) + 3) % 8;
                NUP_SpeedUpX.Maximum = HasMega[mons[ind].Item1][0] ? megas[megalist.IndexOf(ind)].Item2 : 0;
                NUP_SpeedUpY.Maximum = HasMega[mons[ind].Item1][1] ? megas[megalist.IndexOf(ind, megalist.IndexOf(ind) + 1)].Item2 : 0;
                NUP_SpeedUpX.Value = HasMega[mons[ind].Item1][0] ? Math.Min((BitConverter.ToInt32(savedata, 0x2D5B + suX_ofs) >> suX_shift) & 0x7F, NUP_SpeedUpX.Maximum) : 0;
                NUP_SpeedUpY.Value = HasMega[mons[ind].Item1][1] ? Math.Min((BitConverter.ToInt32(savedata, 0x2D5B + suY_ofs) >> suY_shift) & 0x7F, NUP_SpeedUpY.Maximum) : 0;
            }
            else
            {
                NUP_SpeedUpX.Maximum = NUP_SpeedUpY.Maximum = 1;
                NUP_SpeedUpX.Value = NUP_SpeedUpY.Value = 0;
            }
        }

        private void UpdateStageBox()
        {
            int stagelen = BitConverter.ToInt32(stagesMain, 0x4);
            int mainspec = BitConverter.ToUInt16(stagesMain, 0x50 + stagelen * (int)NUP_MainIndex.Value) & 0x3FF;
            int expertspec = BitConverter.ToUInt16(stagesExpert, 0x50 + stagelen * ((int)NUP_ExpertIndex.Value - 1)) & 0x3FF;
            int eventspec = BitConverter.ToUInt16(stagesEvent, 0x50 + stagelen * (int)NUP_EventIndex.Value) & 0x3FF;

            //Stage sprite
            PB_Main.Image = GetStageImage(mons[mainspec].Item1, mons[mainspec].Item2, mons[mainspec].Item3, false);
            PB_Expert.Image = GetStageImage(mons[expertspec].Item1, mons[expertspec].Item2, mons[expertspec].Item3, false);
            PB_Event.Image = GetStageImage((mons[eventspec].Item1 == 25) ? 0 : mons[eventspec].Item1, mons[eventspec].Item2, mons[eventspec].Item3, true);

            //Score
            NUP_MainScore.Value = (BitConverter.ToUInt64(savedata, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1)) >> 4) & 0x00FFFFFF;
            NUP_ExpertScore.Value = (BitConverter.ToUInt64(savedata, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value - 1)) >> 4) & 0x00FFFFFF;
            NUP_EventScore.Value = (BitConverter.ToUInt64(savedata, 0x52D5 + 3 * ((int)NUP_EventIndex.Value)) >> 4) & 0x00FFFFFF;

            //Rank
            int rankM_ofs = (7 + (((int)NUP_MainIndex.Value - 1) * 2)) / 8;
            int rankM_shift = (7 + (((int)NUP_MainIndex.Value - 1) * 2)) % 8;
            int rankM = (BitConverter.ToInt16(savedata, 0x987 + rankM_ofs) >> rankM_shift) & 0x3;
            bool stateM = ((BitConverter.ToInt16(savedata, 0x688 + ((((int)NUP_MainIndex.Value - 1) * 3) / 8)) >> ((((int)NUP_MainIndex.Value - 1) * 3) % 8)) & 0x7) == 5;
            GetRankImage(RankM, rankM, stateM);
            int rankEx_ofs = (7 + (((int)NUP_ExpertIndex.Value - 1) * 2)) / 8;
            int rankEx_shift = (7 + (((int)NUP_ExpertIndex.Value - 1) * 2)) % 8;
            int rankEx = (BitConverter.ToInt16(savedata, 0xAB3 + rankEx_ofs) >> rankEx_shift) & 0x3;
            bool stateEx = ((BitConverter.ToInt16(savedata, 0x84A + ((((int)NUP_ExpertIndex.Value - 1) * 3) / 8)) >> ((((int)NUP_ExpertIndex.Value - 1) * 3) % 8)) & 0x7) == 5;
            GetRankImage(RankEx, rankEx, stateEx);
            int rankEv_ofs = (7 + (((int)NUP_EventIndex.Value) * 2)) / 8;
            int rankEv_shift = (7 + (((int)NUP_EventIndex.Value) * 2)) % 8;
            int rankEv = (BitConverter.ToInt16(savedata, 0xAFE + rankEv_ofs) >> rankEv_shift) & 0x3;
            bool stateEv = ((BitConverter.ToInt16(savedata, 0x8BA + ((4 + ((int)NUP_EventIndex.Value * 3)) / 8)) >> ((4 + ((int)NUP_EventIndex.Value * 3)) % 8)) & 0x7) == 5;
            GetRankImage(RankEv, rankEv, stateEv);
        }

        private Bitmap GetCaughtImage(int ind, bool caught = false)
        {
            Bitmap bmp = GetMonImage(mons[ind].Item1, mons[ind].Item2, mons[ind].Item3);
            if (!caught)
            {
                for (int x = 0; x < bmp.Width; x++)
                {
                    for (int y = 0; y < bmp.Height; y++)
                    {
                        Color c = bmp.GetPixel(x, y);
                        bmp.SetPixel(x, y, Color.FromArgb(c.A, 0, 0, 0));
                    }
                }
            }
            return bmp;
        }

        private Bitmap GetMonImage(int mon_num, int form = 0, bool mega = false)
        {
            string imgname = string.Empty;
            if (mega)
            {
                form -= HasMega[mon_num][1] ? 1 : 2; //Differenciate Rayquaza/Gyarados from Charizard/Mewtwo, otherwise either stage 300 is Shiny M-Ray or stage 150 is M-mewtwo X
                imgname += "mega_";
            }
            imgname += "pokemon_" + mon_num.ToString("000");
            if (form > 0 && mon_num > 0)
                imgname += "_" + form.ToString("00");
            if (mega)
                imgname += "_lo";
            return new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(imgname));
        }

        private Bitmap GetStageImage(int mon_num, int form = 0, bool mega = false, bool special = false)
        {
            Bitmap bmp = new Bitmap(64, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                if (mega && !special)
                    g.DrawImage(Properties.Resources.PlateMega, new Point(0, 16));                    
                else
                    g.DrawImage(Properties.Resources.Plate, new Point(0, 16));
                g.DrawImage(ResizeImage(GetMonImage(mon_num, form, mega), 48, 48), new Point(8, 7));
            }

            return bmp;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            if (image.HorizontalResolution > 0 && image.VerticalResolution > 0)
                destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        private void GetRankImage(Label label, int rank = default(int), bool completed = false) //Plain text until a way is found to extract Rank sprites from game's folders.
        {                                                                                       //These are in several files in "Layout Archives", #127 for example,
            if (completed)                                                                      //but I can't get a proper png without it being cropped or its colours distorted.
            {
                switch (rank)
                {
                    case 0:
                        label.Text = "C";
                        //label.ForeColor = Color.Orchid;
                        break;
                    case 1:
                        label.Text = "B";
                        //label.ForeColor = Color.ForestGreen;
                        break;
                    case 2:
                        label.Text = "A";
                        //label.ForeColor = Color.RoyalBlue;
                        break;
                    case 3:
                        label.Text = "S";
                        //label.ForeColor = Color.Goldenrod;
                        break;
                    default:
                        label.Text = "-";
                        //label.ForeColor = Color.Black;
                        break;
                }
            }
            else
            {
                label.Text = "-";
                //label.ForeColor = Color.Black;
            }
        }

        private void B_CheatsForm_Click(object sender, EventArgs e)
        {
            new Cheats(mondata, monlevel, stagesMain, stagesEvent, stagesExpert, megaStone, HasMega, mons, megas, megalist, megaArray_start, ref savedata).ShowDialog();
            updating = true;
            Parse();
            updating = false;
        }

        private void ItemsGrid_EnabledChanged(object sender, EventArgs e)
        {
            ItemsGrid.SelectedObject = (ItemsGrid.Enabled) ? SI_Items : null;
        }

        private void UpdateProperty(object s, PropertyValueChangedEventArgs e)
        {
            UpdateForm(s, e);
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            string filename = GetDragFilename(e);
            e.Effect = (filename != null && IsShuffleSave(filename)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            Open(GetDragFilename(e));
        }

        protected string GetDragFilename(DragEventArgs e)
        {
            if ((e.AllowedEffect & DragDropEffects.Copy) == DragDropEffects.Copy)
            {
                Array data = ((IDataObject)e.Data).GetData("FileName") as Array;
                if (data != null)
                {
                    if ((data.Length == 1) && (data.GetValue(0) is String))
                        return ((string[])data)[0];
                }
            }
            return null;
        }

        private void PB_Team_Click(object sender, EventArgs e)
        {            
            byte[] data = savedata.Skip(0xE0).Take(0x7).ToArray();
            int slot;
            List<int> list = new List<int>();
            for (int i = 0; i < monsel.Count; i++)
                list.Add(monsel[i].Value); //get a list of index numbers from monsel.Values to use with IndexOf()
            int s = 0;
            if ((sender as Control).Name.Contains("Team1"))
                s = 1;
            if ((sender as Control).Name.Contains("Team2"))
                s = 2;
            if ((sender as Control).Name.Contains("Team3"))
                s = 3;
            if ((sender as Control).Name.Contains("Team4"))
                s = 4;
            switch (s)
            {
                case 1:
                    slot = (BitConverter.ToInt32(data, 0) >> 5) & 0xFFF;
                    break;
                case 2:
                    slot = (BitConverter.ToInt16(data, 0x02) >> 1) & 0xFFF;
                    break;
                case 3:
                    slot = (BitConverter.ToInt32(data, 0x03) >> 5) & 0xFFF;
                    break;
                case 4:
                    slot = (BitConverter.ToInt16(data, 0x05) >> 1) & 0xFFF;
                    break;
                default:
                    return;
            }
            CB_MonIndex.SelectedIndex = list.IndexOf(slot);
        }

        private void Rank_Click(object sender, EventArgs e)
        {            
            int i = default(int);
            if ((sender as Control).Name.Contains("Main"))
                i = 1;
            if ((sender as Control).Name.Contains("Expert"))
                i = 2;
            if ((sender as Control).Name.Contains("Event"))
                i = 3;
            if (!(i > 0 && i < 4))
                return;
            int stage_ofs = 0, stage_shift = 0, rank_ofs = 0, rank_shift = 0;
            ushort stage = 0, rank = 0;
            switch (i)
            {
                case 1:
                    stage_ofs = 0x688 + ((int)NUP_MainIndex.Value - 1) * 3 / 8;
                    stage_shift = ((int)NUP_MainIndex.Value - 1) * 3 % 8;
                    rank_ofs = 0x987 + (7 + ((int)NUP_MainIndex.Value - 1) * 2) / 8;
                    rank_shift = (7 + ((int)NUP_MainIndex.Value - 1) * 2) % 8;                   
                    break;
                case 2:
                    stage_ofs = 0x84A + ((int)NUP_ExpertIndex.Value - 1) * 3 / 8;
                    stage_shift = ((int)NUP_ExpertIndex.Value - 1) * 3 % 8;
                    rank_ofs = 0xAB3 + (7 + ((int)NUP_ExpertIndex.Value - 1) * 2) / 8;
                    rank_shift = (7 + ((int)NUP_ExpertIndex.Value - 1) * 2) % 8;
                    break;
                case 3:
                    stage_ofs = 0x8BA + (4 + (int)NUP_EventIndex.Value * 3) / 8;
                    stage_shift = (4 + (int)NUP_EventIndex.Value * 3) % 8;
                    rank_ofs = 0xAFE + (7 + (int)NUP_EventIndex.Value * 2) / 8;
                    rank_shift = (7 + (int)NUP_EventIndex.Value * 2) % 8;
                    break;
                default:
                    return;
            }
            stage = BitConverter.ToUInt16(savedata, stage_ofs);
            rank = BitConverter.ToUInt16(savedata, rank_ofs);
            if (((BitConverter.ToInt16(savedata, stage_ofs) >> stage_shift) & 7) == 5) //is completed
            {
                if (((BitConverter.ToUInt16(savedata, rank_ofs) >> rank_shift) & 0x3) > 0) //is rank != C
                    rank = (ushort)((rank & (ushort)(~(0x3 << rank_shift))) | ((((BitConverter.ToUInt16(savedata, rank_ofs) >> rank_shift) & 0x3) - 1) << rank_shift)); //minus 1 rank
                else //is rank = C or unknown
                {
                    rank = (ushort)((rank & (ushort)(~(0x3 << rank_shift))) | (0 << rank_shift)); //rank C
                    stage = (ushort)((stage & (ushort)(~(0x7 << stage_shift))) | (0 << stage_shift)); //uncompleted
                }
            }
            else //is uncompleted
            {
                rank = (ushort)((rank & (ushort)(~(0x3 << rank_shift))) | (3 << rank_shift)); //rank S
                stage = (ushort)((stage & (ushort)(~(0x7 << stage_shift))) | (5 << stage_shift)); //completed
            }
            Array.Copy(BitConverter.GetBytes(rank), 0, savedata, rank_ofs, 2);
            Array.Copy(BitConverter.GetBytes(stage), 0, savedata, stage_ofs, 2);
            Parse(); //Update view            
        }
        
    }

    public class cbItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }
}
