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
            for (int i = 1; i < db.MonStopIndex; i++)
                monsel.Add(new cbItem { Text = db.MonsList[i], Value = i });
            monsel = monsel.OrderBy(x => x.Text).ToList();
            CB_MonIndex.DataSource = monsel;
            CB_MonIndex.DisplayMember = "Text";
            CB_MonIndex.ValueMember = "Value";           
            FormInit();
        }

        public static Database db = new Database();
        List<cbItem> monsel = new List<cbItem>();
        ShuffleItems SI_Items = new ShuffleItems();

        bool loaded, updating, overrideHS;
        
        public static byte[] savedata = null;

        private void FormInit()
        {
            B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = false;
            PB_Mon.Image = GetCaughtImage((int)CB_MonIndex.SelectedValue, CHK_CaughtMon.Checked);
            PB_Main.Image = PB_Event.Image = PB_Expert.Image = GetStageImage(0, 0);
            PB_Team1.Image = PB_Team2.Image = PB_Team3.Image = PB_Team4.Image = ResizeImage(GetMonImage(0), 48, 48);
            PB_Lollipop.Image = new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("lollipop"), 24, 24));
            NUP_MainIndex.Maximum = BitConverter.ToInt32(db.StagesMain, 0) - 1;
            NUP_ExpertIndex.Maximum = BitConverter.ToInt32(db.StagesExpert, 0);
            NUP_EventIndex.Maximum = BitConverter.ToInt32(db.StagesEvent, 0) - 1;
            ItemsGrid.SelectedObject = null;
            TB_FilePath.Text = string.Empty;
            savedata = null;         
        }

        private void B_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { FileName = "savedata.bin", Filter = ".bin files (*.bin)|*.bin|All files (*.*)|*.*" , FilterIndex = 1};
            if (ofd.ShowDialog() == DialogResult.OK)
                    Open(ofd.FileName);
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;
            updating = true;
            SaveFileDialog sfd = new SaveFileDialog { FileName = TB_FilePath.Text };
            if (sfd.ShowDialog() != DialogResult.OK) return;

            File.WriteAllBytes(sfd.FileName, savedata);
            MessageBox.Show("Saved save file to " + sfd.FileName + "." + Environment.NewLine + "Remember to delete secure value before importing.");
            updating = false;
        }

        private void Open(string file)
        {
            if (IsShuffleSave(file))
            {
                TB_FilePath.Text = file;
                savedata = File.ReadAllBytes(file);
                Parse();
                B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = true;
                UpdateForm(null, null);
            }
            else
                MessageBox.Show("Couldn't open your file, it wasn't detected as a proper Pokemon Shuffle savefile.");
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
                //Owned Bow Properties
                int ind = (int)CB_MonIndex.SelectedValue;
                ushort set_level = (ushort)(CHK_CaughtMon.Checked ? (NUP_Level.Value == 1 ? 0 : NUP_Level.Value) : 0);
                ushort set_rml = (ushort)(CHK_CaughtMon.Checked ? NUP_Lollipop.Value : 0);
                if (set_level > 10 + set_rml)
                {
                    if ((sender as Control).Name.Contains("Level"))
                        set_rml = (ushort)(set_level - 10);
                    else if ((sender as Control).Name.Contains("Lollipop"))
                        set_level = (ushort)(10 + set_rml);
                }
                SetCaught(ind, CHK_CaughtMon.Checked);
                SetLevel(ind, set_level, set_rml);
                SetStone(ind, CHK_MegaX.Checked, CHK_MegaY.Checked);
                SetSpeedup(ind, (db.HasMega[db.Mons[ind].Item1][0] && CHK_CaughtMon.Checked && CHK_MegaX.Checked), (int)Math.Min(NUP_SpeedUpX.Value, NUP_SpeedUpX.Maximum), (db.HasMega[db.Mons[ind].Item1][1] && CHK_CaughtMon.Checked && CHK_MegaY.Checked), (int)Math.Min(NUP_SpeedUpY.Value, NUP_SpeedUpY.Maximum));   
                   
                //Stages Box Properties          
                SetScore((int)NUP_MainIndex.Value - 1, 0, (ulong)NUP_MainScore.Value);
                SetScore((int)NUP_ExpertIndex.Value - 1, 1, (ulong)NUP_ExpertScore.Value);
                SetScore((int)NUP_EventIndex.Value, 2, (ulong)NUP_EventScore.Value);

                //Ressources Box Properties
                SetResources((int)NUP_Hearts.Value, (uint)NUP_Coins.Value, (uint)NUP_Jewels.Value, SI_Items.Items, SI_Items.Enchantments);
            }
            Parse();
            updating = false;
        }

        private void UpdateResourceBox()
        {
            NUP_Coins.Value = GetRessources().Coins;
            NUP_Jewels.Value = GetRessources().Jewels;
            NUP_Hearts.Value = GetRessources().Hearts;
            for (int i = 0; i < SI_Items.Items.Length; i++)
                SI_Items.Items[i] = GetRessources().Items[i];
            for (int i = 0; i < SI_Items.Enchantments.Length; i++)
                SI_Items.Enchantments[i] = GetRessources().Enhancements[i];
            ItemsGrid.Refresh();  
        }

        private void UpdateOwnedBox()
        {
            int ind = (int)CB_MonIndex.SelectedValue, lev, exp, rml;    //exp variable ready for an exp viewer
            
            //team preview
            byte[] teamData = savedata.Skip(0xE0).Take(0x7).ToArray();
            PB_Team1.Image = ResizeImage(GetMonImage((BitConverter.ToInt32(teamData, 0) >> 5) & 0xFFF), 48, 48);
            PB_Team2.Image = ResizeImage(GetMonImage((BitConverter.ToInt16(teamData, 0x02) >> 1) & 0xFFF), 48, 48);
            PB_Team3.Image = ResizeImage(GetMonImage((BitConverter.ToInt32(teamData, 0x03) >> 5) & 0xFFF), 48, 48);
            PB_Team4.Image = ResizeImage(GetMonImage((BitConverter.ToInt16(teamData, 0x05) >> 1) & 0xFFF), 48, 48);
            
            //caught CHK
            CHK_CaughtMon.Checked = GetCaught(ind);

            //level view
            GetLevel(ind, out lev, out exp, out rml);
            NUP_Lollipop.Maximum = Math.Min(db.Mons[ind].Item4, 5);
            NUP_Lollipop.Value = rml;
            NUP_Level.Maximum = 10 + NUP_Lollipop.Maximum;   // The max on the box could be higher than 10 now  
            NUP_Level.Value = lev;
             
            //Speedup values
            if (db.MegaList.IndexOf(ind) != -1) //temporary fix while there are still some mega forms missing in megastone.bin
            {        
                NUP_SpeedUpX.Maximum = db.HasMega[db.Mons[ind].Item1][0] ? db.Megas[db.MegaList.IndexOf(ind)].Item2 : 0;
                NUP_SpeedUpY.Maximum = db.HasMega[db.Mons[ind].Item1][1] ? db.Megas[db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1)].Item2 : 0;
                NUP_SpeedUpX.Value = GetSpeedupX(ind);
                NUP_SpeedUpY.Value = GetSpeedupY(ind);
            }
            else
            {
                NUP_SpeedUpX.Maximum = NUP_SpeedUpY.Maximum = 1;
                NUP_SpeedUpX.Value = NUP_SpeedUpY.Value = 0;
            }

            #region Visibility
            L_Level.Visible = NUP_Level.Visible = CHK_CaughtMon.Checked;
            PB_Lollipop.Visible = NUP_Lollipop.Visible = (CHK_CaughtMon.Checked && NUP_Lollipop.Maximum != 0);
            PB_Mon.Image = GetCaughtImage(ind, CHK_CaughtMon.Checked);
            PB_MegaX.Visible = CHK_MegaX.Visible = db.HasMega[db.Mons[ind].Item1][0];
            PB_MegaY.Visible = CHK_MegaY.Visible = db.HasMega[db.Mons[ind].Item1][1];
            PB_MegaX.Image = db.HasMega[db.Mons[ind].Item1][0] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + db.Mons[ind].Item1.ToString("000") + (db.HasMega[db.Mons[ind].Item1][1] ? "_X" : string.Empty))) : new Bitmap(16, 16);
            PB_MegaY.Image = db.HasMega[db.Mons[ind].Item1][1] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + db.Mons[ind].Item1.ToString("000") + "_Y")) : new Bitmap(16, 16);
            int mega_ofs = 0x406 + (ind + 2) / 4;
            CHK_MegaX.Checked = ((BitConverter.ToUInt16(savedata, mega_ofs) >> (5 + (ind << 1)) % 8) & 1) == 1;
            CHK_MegaY.Checked = (((BitConverter.ToUInt16(savedata, mega_ofs) >> (5 + (ind << 1)) % 8) >> 1) & 1) == 1;
            NUP_SpeedUpX.Visible = PB_SpeedUpX.Visible = CHK_CaughtMon.Checked && CHK_MegaX.Visible && CHK_MegaX.Checked;
            NUP_SpeedUpY.Visible = PB_SpeedUpY.Visible = CHK_CaughtMon.Checked && CHK_MegaY.Visible && CHK_MegaY.Checked; //Else NUP_SpeedUpY appears if the next mega in terms of offsets has been obtained
            PB_SpeedUpX.Image = db.HasMega[db.Mons[ind].Item1][0] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);
            PB_SpeedUpY.Image = db.HasMega[db.Mons[ind].Item1][1] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);
            #endregion
        }

        private void UpdateStageBox()
        {
            //Ranks
            int rank;
            bool completed;
            foreach (Label lbl in new[] { L_RankM, L_RankEx, L_RankEv})
            {
                if (lbl == L_RankM)
                {
                    rank = GetRank((int)NUP_MainIndex.Value - 1, 0);
                    completed = GetStage((int)NUP_MainIndex.Value - 1, 0);
                }
                else if (lbl == L_RankEx)
                {
                    rank = GetRank((int)NUP_ExpertIndex.Value - 1, 1);
                    completed = GetStage((int)NUP_ExpertIndex.Value - 1, 1);
                }
                else if (lbl == L_RankEv)
                {
                    rank = GetRank((int)NUP_EventIndex.Value, 2);
                    completed = GetStage((int)NUP_EventIndex.Value, 2);
                }
                else return;
                GetRankImage(lbl, rank, completed);
            }

            //Score
            NUP_MainScore.Value = GetScore((int)NUP_MainIndex.Value - 1, 0);
            NUP_ExpertScore.Value = GetScore((int)NUP_ExpertIndex.Value - 1, 1);
            NUP_EventScore.Value = GetScore((int)NUP_EventIndex.Value, 2);

            //Stage sprite
            int stagelen = BitConverter.ToInt32(db.StagesMain, 0x4);
            int mainspec = BitConverter.ToUInt16(db.StagesMain, 0x50 + stagelen * (int)NUP_MainIndex.Value) & 0x3FF;
            int expertspec = BitConverter.ToUInt16(db.StagesExpert, 0x50 + stagelen * ((int)NUP_ExpertIndex.Value - 1)) & 0x3FF;
            int eventspec = BitConverter.ToUInt16(db.StagesEvent, 0x50 + stagelen * (int)NUP_EventIndex.Value) & 0x3FF;
            PB_Main.Image = GetStageImage(mainspec, 0, (L_RankM.Text != "-" || overrideHS));
            PB_Expert.Image = GetStageImage(expertspec, 1, true);
            PB_Event.Image = GetStageImage((eventspec == 25) ? 0 : eventspec,2, true);

            PB_override.Image = overrideHS ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("warn")) : new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("valid"));
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

        private void TB_Filepath_DoubleClick(object sender, EventArgs e)    //Resets Form when double-clicked
        {
            if ((sender as Control).Enabled == true) 
            {
                loaded = false;
                GroupBox[] list = { GB_Caught, GB_HighScore, GB_Resources };
                foreach (GroupBox gb in list)
                {
                    foreach (Control ctrl in gb.Controls)
                    {
                        if (ctrl is CheckBox)
                        {
                            (ctrl as CheckBox).Checked = false;
                            if (ctrl != CHK_CaughtMon)
                                (ctrl as CheckBox).Visible = false;
                        }
                        if (ctrl is PictureBox)
                            (ctrl as PictureBox).Image = new Bitmap(ctrl.Width, ctrl.Height);
                        if (ctrl is NumericUpDown)
                        {
                            (ctrl as NumericUpDown).Value = (ctrl as NumericUpDown).Minimum;
                            if (gb == GB_Caught)
                                (ctrl as NumericUpDown).Visible = false;
                        }
                        if (ctrl is Label)
                        {
                            if (gb == GB_Caught)
                                (ctrl as Label).Visible = false;
                            if (ctrl.Name.Contains("Rank"))
                                (ctrl as Label).Text = "-";
                        }
                    }
                }                
                FormInit();
            }
        }

        private void B_CheatsForm_Click(object sender, EventArgs e)
        {
            new Cheats().ShowDialog();
            updating = true;
            Parse();
            updating = false;
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

        private void PB_Override_Click(object sender, EventArgs e)
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
                if (GetStage(ind, i - 1)) //is completed
                {
                    if (GetRank(ind, i - 1) > 0 && GetRank(ind, i - 1) < 4) //is rank != C
                        SetRank(ind, i - 1, GetRank(ind, i - 1) - 1);   //minus 1 rank
                    else //is rank = C or unknown    
                    {
                        SetRank(ind, i - 1, 3);  //rank S
                        PatchScore(ind, i - 1);
                    }    
                }
                //Nothing happens if uncompleted
            }
            if (me.Button == System.Windows.Forms.MouseButtons.Right)   //Right Click
            {
                SetStage(ind, i - 1, !GetStage(ind, i - 1));    //invert completed state
                if (GetStage(ind, i - 1)) //is completed (was uncompleted)
                {
                    if (i == 1 && !overrideHS) //Main stages
                    {
                        for (int j = ind; j >= 0; j--)
                        {
                            SetStage(j, i - 1, true); //completed for every previous stage
                            PatchScore(j, i - 1);
                        }
                    }
                    SetRank(ind, i - 1, 3);  //rank S
                    PatchScore(ind, i - 1);
                }
                else //is uncompleted (was completed)
                {
                    if (i == 1 && !overrideHS) //Main stages
                    {
                        for (int j = ind; j < max; j++) //revert every next stage to default
                        {
                            SetStage(j, i - 1); 
                            SetRank(j, i - 1, 0);
                            SetScore(j, i - 1, 0);
                        }
                    }
                }
            }
            updating = true;
            Parse();
            updating = false;
        }        
    }   
}