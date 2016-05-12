using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using static Pokemon_Shuffle_Save_Editor.ToolFunctions;

namespace Pokemon_Shuffle_Save_Editor
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();                    
            monsel = new List<cbItem>();
            for (int i = 1; i < db.MonStopIndex; i++)
                monsel.Add(new cbItem { Text = db.MonsList[i], Value = i });
            monsel = monsel.OrderBy(ncbi => ncbi.Text).ToList();
            CB_MonIndex.DataSource = monsel;
            CB_MonIndex.DisplayMember = "Text";
            CB_MonIndex.ValueMember = "Value";
            PB_Mon.Image = GetCaughtImage(db, (int)CB_MonIndex.SelectedValue, CHK_CaughtMon.Checked);
            PB_Main.Image = PB_Event.Image = PB_Expert.Image = GetStageImage(db, 0);
            PB_Team1.Image = PB_Team2.Image = PB_Team3.Image = PB_Team4.Image = ResizeImage(GetMonImage(db, 0), 48, 48);
            PB_Lollipop.Image = new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("lollipop"), 24, 24));
            NUP_MainIndex.Minimum = NUP_ExpertIndex.Minimum = 1;
            NUP_EventIndex.Minimum = NUP_MainScore.Minimum = NUP_ExpertScore.Minimum = NUP_EventScore.Minimum = NUP_SpeedUpX.Minimum = NUP_SpeedUpY.Minimum = 0;
            NUP_MainIndex.Maximum = BitConverter.ToInt32(db.StagesMain, 0) - 1;
            NUP_ExpertIndex.Maximum = BitConverter.ToInt32(db.StagesExpert, 0);
            NUP_EventIndex.Maximum = BitConverter.ToInt32(db.StagesEvent, 0) - 1;            ;
            NUP_MainScore.Maximum = NUP_ExpertScore.Maximum = NUP_EventScore.Maximum = 0xFFFFFF;
            NUP_SpeedUpX.Maximum = NUP_SpeedUpY.Maximum = 127;
            CHK_MegaY.Visible = CHK_MegaX.Visible = NUP_SpeedUpX.Visible = NUP_SpeedUpY.Visible = false;
            ItemsGrid.SelectedObject = null;
        }

        Database db = new Database();
        List<cbItem> monsel;
        ShuffleItems SI_Items = new ShuffleItems();

        bool loaded, updating, overrideHS;

        private void B_Open_Click(object sender, EventArgs e)
        {
            TB_FilePath.Text = string.Empty;
            B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = false;

            OpenFileDialog ofd = new OpenFileDialog { FileName = "savedata.bin" };
            if (ofd.ShowDialog() != DialogResult.OK) return;
            Open(ofd.FileName);
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;
            updating = true;
            SaveFileDialog sfd = new SaveFileDialog { FileName = TB_FilePath.Text };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            File.WriteAllBytes(sfd.FileName, db.SaveData);
            MessageBox.Show("Saved save file to " + sfd.FileName + "." + Environment.NewLine + "Remember to delete secure value before importing.");
            updating = false;
        }

        private void Open(string file)
        {
            if (IsShuffleSave(file))
            {
                TB_FilePath.Text = file;
                db.SaveData = File.ReadAllBytes(file);
                Parse();
                B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = true;
                UpdateForm(null, null);
            }            
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

                //caught patcher
                int caught_ofs = (ind - 1 + 6) / 8;
                int caught_shift = (ind - 1 + 6) % 8;
                foreach (int caught_array_start in new[] { 0xE6, 0x546, 0x5E6 })
                    db.SaveData[caught_array_start + caught_ofs] = (byte)(db.SaveData[caught_array_start + caught_ofs] & (byte)(~(1 << caught_shift)) | ((CHK_CaughtMon.Checked ? 1 : 0) << caught_shift));

                //level & lollipop patcher
                int level_ofs = 0x187 + (((ind - 1) * 4) / 8);
                int level_shift = ((((ind - 1) * 4) + 1) % 8);
                ushort level = BitConverter.ToUInt16(db.SaveData, level_ofs);                
                ushort set_level = (ushort)(CHK_CaughtMon.Checked ? (NUP_Level.Value == 1 ? 0 : NUP_Level.Value) : 0);
                int rml_ofs = 0xA9DB + (ind * 6) / 8;
                int rml_shift = (ind * 6) % 8;
                ushort numRaiseMaxLevel = BitConverter.ToUInt16(db.SaveData, rml_ofs);
                ushort set_rml = (ushort)(CHK_CaughtMon.Checked ? NUP_Lollipop.Value : 0);
                if (set_level > 10 + set_rml)
                {
                    if ((sender as Control).Name.Contains("Level"))
                        set_rml = (ushort)(set_level - 10);
                    if ((sender as Control).Name.Contains("Lollipop"))
                        set_level = (ushort)(10 + set_rml);
                }
                level = (ushort)((level & (ushort)(~(0xF << level_shift))) | (set_level << level_shift));
                Array.Copy(BitConverter.GetBytes(level), 0, db.SaveData, level_ofs, 2);
                numRaiseMaxLevel = (ushort)((numRaiseMaxLevel & (ushort)(~(0x3F << rml_shift))) | (set_rml << rml_shift));    //int set_rml = Math.Min(((int)NUP_Level.Value - 10 < 0) ? 0 : (int)NUP_Level.Value - 10, 5); //Hardcoded 5 as the max number of lollipops, change this if needed later                
                Array.Copy(BitConverter.GetBytes(numRaiseMaxLevel), 0, db.SaveData, rml_ofs, 2);

                //experience patcher
                int exp_ofs = 0x3241 + (4 + (ind - 1) * 24) / 8;
                int exp_shift = (4 + (ind - 1) * 24) % 8;
                int exp = BitConverter.ToInt32(db.SaveData, exp_ofs);
                int entrylen = BitConverter.ToInt32(db.MonLevel, 0x4);
                byte[] data = db.MonLevel.Skip(0x50 + (set_level - 1) * entrylen).Take(entrylen).ToArray();
                int set_exp = BitConverter.ToInt32(data, 0x4 * (db.Mons[ind].Item5 - 1));
                exp = (exp & ~(0xFFFFFF << exp_shift)) | (set_exp << exp_shift);
                Array.Copy(BitConverter.GetBytes(exp), 0, db.SaveData, exp_ofs, 4);

                //megastone patcher
                int mega_ofs = 0x406 + (ind + 2) / 4;
                ushort mega_val = BitConverter.ToUInt16(db.SaveData, mega_ofs);
                mega_val &= (ushort)(~(3 << ((5 + (ind << 1)) % 8)));
                ushort new_mega_insert = (ushort)(0 | (CHK_MegaX.Checked ? 1 : 0) | (CHK_MegaY.Checked ? 2 : 0));
                mega_val |= (ushort)(new_mega_insert << ((5 + (ind << 1)) % 8));
                Array.Copy(BitConverter.GetBytes(mega_val), 0, db.SaveData, mega_ofs, 2);

                //speedups patcher
                if (db.HasMega[db.Mons[ind].Item1][0] || db.HasMega[db.Mons[ind].Item1][1])
                {
                    int suX_ofs = 0x2D5B + (db.MegaList.IndexOf(ind) * 7 + 3) / 8;
                    int suX_shift = (db.MegaList.IndexOf(ind) * 7 + 3) % 8;
                    int suY_ofs = 0x2D5B + ((db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7) + 3) / 8;
                    int suY_shift = ((db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7) + 3) % 8 + (suY_ofs - suX_ofs) * 8; //relative to suX_ofs
                    int speedUp_ValX = BitConverter.ToInt32(db.SaveData, suX_ofs);
                    int speedUp_ValY = BitConverter.ToInt32(db.SaveData, suY_ofs);
                    int set_suX = (db.HasMega[db.Mons[ind].Item1][0] && CHK_CaughtMon.Checked && CHK_MegaX.Checked) ? (int)Math.Min(NUP_SpeedUpX.Value, NUP_SpeedUpX.Maximum) : 0;
                    int set_suY = (db.HasMega[db.Mons[ind].Item1][1] && CHK_CaughtMon.Checked && CHK_MegaY.Checked) ? (int)Math.Min(NUP_SpeedUpY.Value, NUP_SpeedUpY.Maximum) : 0;
                    int newSpeedUp = db.HasMega[db.Mons[ind].Item1][1]
                        ? ((speedUp_ValX & ~(0x7F << suX_shift)) & ~(0x7F << suY_shift)) | (set_suX << suX_shift) | (set_suY << suY_shift) //Erases both X & Y bits at the same time before updating them to make sure Y doesn't overwrite X bits
                        : (speedUp_ValX & ~(0x7F << suX_shift)) | (set_suX << suX_shift);
                    Array.Copy(BitConverter.GetBytes(newSpeedUp), 0, db.SaveData, suX_ofs, 4);
                }
                
                //score patcher
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(db.SaveData, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_MainScore.Value << 4))), 0, db.SaveData, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1), 8);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(db.SaveData, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_ExpertScore.Value << 4))), 0, db.SaveData, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value), 8);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(db.SaveData, 0x52D5 + 3 * ((int)NUP_EventIndex.Value)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_EventScore.Value << 4))), 0, db.SaveData, 0x52D5 + 3 * ((int)NUP_EventIndex.Value), 8);

                //items patcher
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt32(db.SaveData, 0x68) & 0xF0000007) | ((uint)NUP_Coins.Value << 3) | ((uint)NUP_Jewels.Value << 20)), 0, db.SaveData, 0x68, 4);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt16(db.SaveData, 0x2D4A) & 0xC07F) | ((ushort)NUP_Hearts.Value << 7)), 0, db.SaveData, 0x2D4A, 2);
                for (int i = 0; i < SI_Items.Items.Length; i++)
                {
                    ushort val = BitConverter.ToUInt16(db.SaveData, 0xd0 + i);
                    val &= 0x7F;
                    val |= (ushort)(SI_Items.Items[i] << 7);
                    Array.Copy(BitConverter.GetBytes(val), 0, db.SaveData, 0xd0 + i, 2);
                }
                for (int i = 0; i < SI_Items.Enchantments.Length; i++)
                    db.SaveData[0x2D4C + i] = (byte)(((SI_Items.Enchantments[i] << 1) & 0xFE) | (db.SaveData[0x2D4C + i] & 1));
            }
            Parse();
            updating = false;
        }

        private void UpdateResourceBox()
        {
            NUP_Coins.Value = (BitConverter.ToUInt32(db.SaveData, 0x68) >> 3) & 0x1FFFF;
            NUP_Jewels.Value = (BitConverter.ToUInt16(db.SaveData, 0x6A) >> 4) & 0xFF;
            NUP_Hearts.Value = (BitConverter.ToUInt16(db.SaveData, 0x2D4A) >> 7) & 0x7F;
            for (int i = 0; i < SI_Items.Items.Length; i++)
                SI_Items.Items[i] = (BitConverter.ToUInt16(db.SaveData, 0xD0 + i) >> 7) & 0x7F;
            for (int i = 0; i < SI_Items.Enchantments.Length; i++)
                SI_Items.Enchantments[i] = (db.SaveData[0x2D4C + i] >> 1) & 0x7F;
            ItemsGrid.Refresh();  
        }

        private void UpdateOwnedBox()
        {
            int ind = (int)CB_MonIndex.SelectedValue;
            
            //team preview
            byte[] teamData = db.SaveData.Skip(0xE0).Take(0x7).ToArray();
            int teamSlot1 = (BitConverter.ToInt32(teamData, 0) >> 5) & 0xFFF;
            int teamSlot2 = (BitConverter.ToInt16(teamData, 0x02) >> 1) & 0xFFF;
            int teamSlot3 = (BitConverter.ToInt32(teamData, 0x03) >> 5) & 0xFFF;
            int teamSlot4 = (BitConverter.ToInt16(teamData, 0x05) >> 1) & 0xFFF;
            PB_Team1.Image = ResizeImage(GetMonImage(db, teamSlot1), 48, 48);
            PB_Team2.Image = ResizeImage(GetMonImage(db, teamSlot2), 48, 48);
            PB_Team3.Image = ResizeImage(GetMonImage(db, teamSlot3), 48, 48);
            PB_Team4.Image = ResizeImage(GetMonImage(db, teamSlot4), 48, 48);
            
            //level view
            int level_ofs = 0x187 + (ind - 1) * 4 / 8;
            int level = (BitConverter.ToUInt16(db.SaveData, level_ofs) >> (((ind - 1) * 4) + 1) % 8) & 0xF;
            NUP_Lollipop.Maximum = Math.Min(db.Mons[ind].Item4, 5);    //int num_raise_max_level = Math.Min(((BitConverter.ToUInt16(db.SaveData, 0xA9DB + ((ind * 6) / 8)) >> ((ind * 6) % 8)) & 0x3F), 5); -> old way of getting number of given lollipops
            NUP_Lollipop.Value = Math.Min(((BitConverter.ToUInt16(db.SaveData, 0xA9DB + ((ind * 6) / 8)) >> ((ind * 6) % 8)) & 0x3F), 5);  //hardcoded 5 as a maximum
            NUP_Level.Maximum = 10 + NUP_Lollipop.Maximum;   // The max on the box could be higher than 10 now  
            NUP_Level.Value = level > 0 ? level : 1;    // Stop showing 0 for the level...

            //caught CHK
            int caught_ofs = 0x546 + (ind-1+6)/8;
            CHK_CaughtMon.Checked = ((db.SaveData[caught_ofs] >> ((ind-1+6) % 8)) & 1) == 1;   
                     
            L_Level.Visible = NUP_Level.Visible = CHK_CaughtMon.Checked;
            PB_Lollipop.Visible = NUP_Lollipop.Visible = (CHK_CaughtMon.Checked && NUP_Lollipop.Maximum != 0);
            PB_Mon.Image = GetCaughtImage(db, ind, CHK_CaughtMon.Checked);

            #region Mega Visibility
            PB_MegaX.Visible = CHK_MegaX.Visible = db.HasMega[db.Mons[ind].Item1][0];
            PB_MegaY.Visible = CHK_MegaY.Visible = db.HasMega[db.Mons[ind].Item1][1];
            PB_MegaX.Image = db.HasMega[db.Mons[ind].Item1][0] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + db.Mons[ind].Item1.ToString("000") + (db.HasMega[db.Mons[ind].Item1][1] ? "_X" : string.Empty))) : new Bitmap(16, 16);
            PB_MegaY.Image = db.HasMega[db.Mons[ind].Item1][1] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + db.Mons[ind].Item1.ToString("000") + "_Y")) : new Bitmap(16, 16);
            PB_SpeedUpX.Image = db.HasMega[db.Mons[ind].Item1][0] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);
            PB_SpeedUpY.Image = db.HasMega[db.Mons[ind].Item1][1] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);
            int mega_ofs = 0x406 + (ind + 2) / 4;
            CHK_MegaX.Checked = ((BitConverter.ToUInt16(db.SaveData, mega_ofs) >> (5 + (ind << 1)) % 8) & 1) == 1;
            CHK_MegaY.Checked = (((BitConverter.ToUInt16(db.SaveData, mega_ofs) >> (5 + (ind << 1)) % 8) >> 1) & 1) == 1; 
            NUP_SpeedUpX.Visible = PB_SpeedUpX.Visible = CHK_MegaX.Visible && CHK_MegaX.Checked && CHK_CaughtMon.Checked;
            NUP_SpeedUpY.Visible = PB_SpeedUpY.Visible = CHK_MegaY.Visible && CHK_MegaY.Checked && CHK_CaughtMon.Checked; //Else NUP_SpeedUpY appears if the next mega in terms of offsets has been obtained
            #endregion  
            if (db.MegaList.IndexOf(ind) != -1) //temporary fix while there are still some mega forms missing in megastone.bin
            {
                int suX_ofs = ((db.MegaList.IndexOf(ind) * 7) + 3) / 8;
                int suX_shift = ((db.MegaList.IndexOf(ind) * 7) + 3) % 8;
                int suY_ofs = ((db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7) + 3) / 8; //looped IndexOf() to get the second occurence of ind
                int suY_shift = ((db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7) + 3) % 8;
                NUP_SpeedUpX.Maximum = db.HasMega[db.Mons[ind].Item1][0] ? db.Megas[db.MegaList.IndexOf(ind)].Item2 : 0;
                NUP_SpeedUpY.Maximum = db.HasMega[db.Mons[ind].Item1][1] ? db.Megas[db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1)].Item2 : 0;
                NUP_SpeedUpX.Value = db.HasMega[db.Mons[ind].Item1][0] ? Math.Min((BitConverter.ToInt32(db.SaveData, 0x2D5B + suX_ofs) >> suX_shift) & 0x7F, NUP_SpeedUpX.Maximum) : 0;
                NUP_SpeedUpY.Value = db.HasMega[db.Mons[ind].Item1][1] ? Math.Min((BitConverter.ToInt32(db.SaveData, 0x2D5B + suY_ofs) >> suY_shift) & 0x7F, NUP_SpeedUpY.Maximum) : 0;
            }
            else
            {
                NUP_SpeedUpX.Maximum = NUP_SpeedUpY.Maximum = 1;
                NUP_SpeedUpX.Value = NUP_SpeedUpY.Value = 0;
            }
        }

        private void UpdateStageBox()
        {
            int stagelen = BitConverter.ToInt32(db.StagesMain, 0x4);
            int mainspec = BitConverter.ToUInt16(db.StagesMain, 0x50 + stagelen * (int)NUP_MainIndex.Value) & 0x3FF;
            int expertspec = BitConverter.ToUInt16(db.StagesExpert, 0x50 + stagelen * ((int)NUP_ExpertIndex.Value - 1)) & 0x3FF;
            int eventspec = BitConverter.ToUInt16(db.StagesEvent, 0x50 + stagelen * (int)NUP_EventIndex.Value) & 0x3FF;

            //Rank
            int rankM_ofs = (7 + (((int)NUP_MainIndex.Value - 1) * 2)) / 8;
            int rankM_shift = (7 + (((int)NUP_MainIndex.Value - 1) * 2)) % 8;
            int rankM = (BitConverter.ToInt16(db.SaveData, 0x987 + rankM_ofs) >> rankM_shift) & 0x3;
            bool stateM = ((BitConverter.ToInt16(db.SaveData, 0x688 + ((((int)NUP_MainIndex.Value - 1) * 3) / 8)) >> ((((int)NUP_MainIndex.Value - 1) * 3) % 8)) & 0x7) == 5;
            GetRankImage(L_RankM, rankM, stateM);
            int rankEx_ofs = (7 + (((int)NUP_ExpertIndex.Value - 1) * 2)) / 8;
            int rankEx_shift = (7 + (((int)NUP_ExpertIndex.Value - 1) * 2)) % 8;
            int rankEx = (BitConverter.ToInt16(db.SaveData, 0xAB3 + rankEx_ofs) >> rankEx_shift) & 0x3;
            bool stateEx = ((BitConverter.ToInt16(db.SaveData, 0x84A + ((((int)NUP_ExpertIndex.Value - 1) * 3) / 8)) >> ((((int)NUP_ExpertIndex.Value - 1) * 3) % 8)) & 0x7) == 5;
            GetRankImage(L_RankEx, rankEx, stateEx);
            int rankEv_ofs = (7 + (((int)NUP_EventIndex.Value) * 2)) / 8;
            int rankEv_shift = (7 + (((int)NUP_EventIndex.Value) * 2)) % 8;
            int rankEv = (BitConverter.ToInt16(db.SaveData, 0xAFE + rankEv_ofs) >> rankEv_shift) & 0x3;
            bool stateEv = ((BitConverter.ToInt16(db.SaveData, 0x8BA + ((4 + ((int)NUP_EventIndex.Value * 3)) / 8)) >> ((4 + ((int)NUP_EventIndex.Value * 3)) % 8)) & 0x7) == 5;
            GetRankImage(L_RankEv, rankEv, stateEv);

            //Score
            NUP_MainScore.Value = (BitConverter.ToUInt64(db.SaveData, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1)) >> 4) & 0x00FFFFFF;
            NUP_ExpertScore.Value = (BitConverter.ToUInt64(db.SaveData, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value - 1)) >> 4) & 0x00FFFFFF;
            NUP_EventScore.Value = (BitConverter.ToUInt64(db.SaveData, 0x52D5 + 3 * ((int)NUP_EventIndex.Value)) >> 4) & 0x00FFFFFF;

            //Stage sprite
            PB_Main.Image = GetStageImage(db, mainspec, (L_RankM.Text != "-" || overrideHS));
            PB_Expert.Image = GetStageImage(db, expertspec, true, 1);
            PB_Event.Image = GetStageImage(db, (eventspec == 25) ? 0 : eventspec, true, 2);
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            string filename = GetDragFilename(e);
            e.Effect = (filename != null && IsShuffleSave(filename)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void Main_DragDrop(object sender, DragEventArgs e)
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

        private void ItemsGrid_EnabledChanged(object sender, EventArgs e)
        {
            ItemsGrid.SelectedObject = (ItemsGrid.Enabled) ? SI_Items : null;
        }

        private void UpdateProperty(object s, PropertyValueChangedEventArgs e)
        {
            UpdateForm(s, e);
        }

        private void B_CheatsForm_Click(object sender, EventArgs e)
        {
            new Cheats(db).ShowDialog();
            updating = true;
            Parse();
            updating = false;
        }

        private void PB_Team_Click(object sender, EventArgs e)
        {            
            byte[] data = db.SaveData.Skip(0xE0).Take(0x7).ToArray();
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

        private void L_Main_Click(object sender, EventArgs e)
        {
            overrideHS = !overrideHS;
            updating = true;
            Parse();
            updating = false;
        }

        private void PB_Owned_Click(object sender, EventArgs e)
        {
            int s = 0;
            if ((sender as Control).Name.Contains("SpeedUpX"))
                s = 1;
            if ((sender as Control).Name.Contains("SpeedUpY"))
                s = 2;
            if ((sender as Control).Name.Contains("Lollipop"))
                s = 3;
            if ((sender as Control).Name.Contains("Mon"))
                s = 4;
            switch (s)
            {
                case 1:
                    NUP_SpeedUpX.Value = (NUP_SpeedUpX.Value == 0) ? NUP_SpeedUpX.Maximum : 0;
                    break;
                case 2:
                    NUP_SpeedUpY.Value = (NUP_SpeedUpY.Value == 0) ? NUP_SpeedUpY.Maximum : 0;
                    break;
                case 3:
                    NUP_Lollipop.Value = (NUP_Lollipop.Value == 0) ? NUP_Lollipop.Maximum : 0;
                    NUP_Level.Value = 10 + NUP_Lollipop.Value;
                    break;
                case 4:
                    if (!CHK_CaughtMon.Checked)
                    {
                        CHK_CaughtMon.Checked = true;
                        NUP_Lollipop.Value = NUP_Lollipop.Maximum;
                        NUP_Level.Value = NUP_Level.Maximum;
                        CHK_MegaX.Checked = db.HasMega[db.Mons[(int)CB_MonIndex.SelectedValue].Item1][0];
                        CHK_MegaY.Checked = db.HasMega[db.Mons[(int)CB_MonIndex.SelectedValue].Item1][1];
                        NUP_SpeedUpX.Value = (db.HasMega[db.Mons[(int)CB_MonIndex.SelectedValue].Item1][0]) ? NUP_SpeedUpX.Maximum : 0;
                        NUP_SpeedUpY.Value = (db.HasMega[db.Mons[(int)CB_MonIndex.SelectedValue].Item1][1]) ? NUP_SpeedUpY.Maximum : 0;
                    }
                    else CHK_CaughtMon.Checked = CHK_MegaX.Checked = CHK_MegaY.Checked = false;
                    break;
                default:
                    return;
            }
            updating = true;
            Parse();
            updating = false;
        }

        private void Rank_Click(object sender, EventArgs e)
        {
            int i, ind, max;
            if ((sender as Control).Name.Contains("Main"))
            {
                i = 1;
                ind = (int)NUP_MainIndex.Value - 1;
                max = (int)NUP_MainIndex.Maximum;
            }
            else if ((sender as Control).Name.Contains("Expert"))
            {
                i = 2;
                ind = (int)NUP_ExpertIndex.Value - 1;
                max = (int)NUP_ExpertIndex.Maximum;
            }
            else if ((sender as Control).Name.Contains("Event"))
            {
                i = 3;
                ind = (int)NUP_EventIndex.Value;
                max = (int)NUP_EventIndex.Maximum + 1;
            }
            else return;
            MouseEventArgs me = (MouseEventArgs)e;
            if (me.Button == System.Windows.Forms.MouseButtons.Left)    //Left Click
            {
                if (GetStage(db, ind, i - 1)) //is completed
                {
                    if (GetRank(db, ind, i - 1) > 0 && GetRank(db, ind, i - 1) < 4) //is rank != C
                        SetRank(db, ind, i - 1, GetRank(db, ind, i - 1) - 1);   //minus 1 rank
                    else //is rank = C or unknown                    
                        SetRank(db, ind, i - 1, 3);  //rank S
                }
                //Nothing happens if uncompleted
            }
            if (me.Button == System.Windows.Forms.MouseButtons.Right)   //Right Click
            {
                SetStage(db, ind, i - 1, !GetStage(db, ind, i - 1));
                if (GetStage(db, ind, i - 1)) //is completed (was uncompleted)
                {
                    if (i == 1) //Main stages
                    {
                        for (int j = ind; j >= 0; j--)
                            SetStage(db, j, i - 1, true); //completed for every previous stage (C Rank as default)
                    }
                    SetRank(db, ind, i - 1, 3);  //rank S
                }
                else //is uncompleted (was completed)
                {
                    if (i == 1) //Main stages
                    {
                        for (int j = ind; j < max; j++)
                            SetStage(db, j, i - 1); //uncompleted & Rank C for every next stage
                    }
                }
            }
            updating = true;
            Parse();
            updating = false;
        }        
    }

    public class cbItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

}