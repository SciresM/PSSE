﻿using System;
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
        public static Database db = new Database();
        public static byte[] savedata = null;
        public static Keys lastkeys;    //to detect if ItemsGrid was entered from Tab or Shift+Tab

        private List<cbItem> monsel = new List<cbItem>();
        private List<cbItem> skillsel = new List<cbItem>();
        private ShuffleItems SI_Items = new ShuffleItems();

        private bool loaded, updating, overrideHS;
        public static int ltir; //Last TeamIndex Right-clicked

        public Main()
        {
            InitializeComponent();
            for (int i = 1; i < db.MonStopIndex; i++)
                monsel.Add(new cbItem { Text = db.MonsList[i], Value = i });
            monsel = monsel.OrderBy(x => x.Text).ToList();
            CB_MonIndex.DataSource = monsel;
            CB_Skill.DataSource = skillsel;
            CB_MonIndex.DisplayMember = CB_Skill.DisplayMember = "Text";
            CB_MonIndex.ValueMember = CB_Skill.ValueMember = "Value";
            CB_Skill_Init();
            FormInit();
        }

        private void FormInit()
        {
            CB_MonIndex.SelectedIndex = 0;
            CB_Skill.Visible = CHK_CurrentSkill.Visible = false;
            B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = false;
            PB_Mon.Image = GetCaughtImage((int)CB_MonIndex.SelectedValue, CHK_CaughtMon.Checked);
            PB_Main.Image = PB_Event.Image = PB_Expert.Image = GetStageImage(0, 0);
            PB_Team1.Image = PB_Team2.Image = PB_Team3.Image = PB_Team4.Image = ResizeImage(GetMonImage(0), 48, 48);
            PB_Lollipop.Image = new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("lollipop"), 24, 24));
            PB_Skill.Image = new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("skill"), 24, 24));
            PB_Lollipop.Visible = PB_Skill.Visible = false;
            NUP_MainIndex.Maximum = BitConverter.ToInt32(db.StagesMain, 0) - 1;
            NUP_ExpertIndex.Maximum = BitConverter.ToInt32(db.StagesExpert, 0);
            NUP_EventIndex.Maximum = BitConverter.ToInt32(db.StagesEvent, 0) - 1;
            ItemsGrid.SelectedObject = null;
            TB_FilePath.Text = string.Empty;
            savedata = null;
            ltir = 0;
        }

        private void Open(string file)
        {
            if (IsShuffleSave(file))
            {
                B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = true;
                TB_FilePath.Text = file;
                savedata = File.ReadAllBytes(file);
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

        private void Parse(int slot = 0)
        {
            ltir = slot;
            UpdateResourceBox();
            UpdateStageBox();
            UpdateOwnedBox();
        }

        private void CB_Skill_Init()
        {
            skillsel.Clear();
            for (int i = 0; i < db.Mons[(int)CB_MonIndex.SelectedValue].Item6.Length; i++)
            {
                if (db.Mons[(int)CB_MonIndex.SelectedValue].Item6[i] == 0)
                    break;
                skillsel.Add(new cbItem { Text = db.SkillsList[db.Mons[(int)CB_MonIndex.SelectedValue].Item6[i] - 1], Value = i });
            }
            CB_Skill.DataSource = skillsel.OrderBy(x => x.Value).ToList(); //for some reason the display doesn't update if I don't do this pointless reorderng stuff
            CHK_CurrentSkill.Visible = (CHK_CaughtMon.Checked && skillsel.Count > 1);
        }

        private void UpdateForm(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;
            updating = true;
            if (sender != null)
            {
                if ((sender as Control).Name.ToLower().Contains("index"))
                    CB_Skill_Init();
                else
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
                    SetSpeedup(ind, (db.HasMega[ind][0] && CHK_CaughtMon.Checked && CHK_MegaX.Checked), (int)NUP_SpeedUpX.Value, (db.HasMega[ind][1] && CHK_CaughtMon.Checked && CHK_MegaY.Checked), (int)NUP_SpeedUpY.Value);
                    SetSkill(ind, (int)(CHK_CaughtMon.Checked ? NUP_SkillLvl.Value : 1), (int)(CHK_CaughtMon.Checked ? CB_Skill.SelectedValue : 0));
                    if (!CHK_CaughtMon.Checked || ((sender as Control).Name.ToLower().Contains("currentskill") && CHK_CurrentSkill.Checked))
                        SetCurrentSkill(ind, CHK_CaughtMon.Checked ? (int)CB_Skill.SelectedValue : 0);

                    //Stages Box Properties
                    SetScore((int)NUP_MainIndex.Value - 1, 0, (ulong)NUP_MainScore.Value);
                    SetScore((int)NUP_ExpertIndex.Value - 1, 1, (ulong)NUP_ExpertScore.Value);
                    SetScore((int)NUP_EventIndex.Value, 2, (ulong)NUP_EventScore.Value);

                    //Ressources Box Properties
                    SetResources((int)NUP_Hearts.Value, (uint)NUP_Coins.Value, (uint)NUP_Jewels.Value, SI_Items.Items, SI_Items.Enchantments);
                }
            }
            Parse();
            updating = false;
        }

        private void UpdateOwnedBox()
        {
            int ind = (int)CB_MonIndex.SelectedValue;

            //team preview
            int j = 1;
            foreach (PictureBox pb in new[] { PB_Team1, PB_Team2, PB_Team3, PB_Team4 })
            {
                pb.Image = GetTeamImage(GetTeam(j), (ltir == j));
                j++;
            }

            //caught CHK
            CHK_CaughtMon.Checked = GetMon(ind).Caught;

            //level view
            NUP_Lollipop.Maximum = Math.Min(db.Mons[ind].Item4, 5);
            NUP_Lollipop.Value = GetMon(ind).Lollipops;
            NUP_Level.Maximum = 10 + NUP_Lollipop.Maximum;
            NUP_Level.Value = GetMon(ind).Level;
            
            //Skill level
            CHK_CurrentSkill.Checked = (GetMon(ind).CurrentSkill == (int)CB_Skill.SelectedValue);
            NUP_SkillLvl.Value = GetMon(ind).SkillLevel[(int)CB_Skill.SelectedValue];
            toolTip1.SetToolTip(CB_Skill, db.SkillsTextList[db.Mons[ind].Item6[(int)CB_Skill.SelectedValue] - 1]);

            //Speedup values
            if (db.MegaList.IndexOf(ind) != -1) //temporary fix while there are still some mega forms missing in megastone.bin
            {
                NUP_SpeedUpX.Maximum = db.HasMega[ind][0] ? db.Megas[db.MegaList.IndexOf(ind)].Item2 : 0;
                NUP_SpeedUpY.Maximum = db.HasMega[ind][1] ? db.Megas[db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1)].Item2 : 0;
                NUP_SpeedUpX.Value = GetMon(ind).SpeedUpX;
                NUP_SpeedUpY.Value = GetMon(ind).SpeedUpY;
            }
            else
            {
                NUP_SpeedUpX.Maximum = NUP_SpeedUpY.Maximum = 1;
                NUP_SpeedUpX.Value = NUP_SpeedUpY.Value = 0;
            }

            #region Visibility

            L_Level.Visible = L_Skill.Visible = NUP_Level.Visible = PB_Skill.Visible = NUP_SkillLvl.Visible = CB_Skill.Visible = CHK_CaughtMon.Checked;
            PB_Lollipop.Visible = NUP_Lollipop.Visible = (CHK_CaughtMon.Checked && NUP_Lollipop.Maximum != 0);
            CHK_CurrentSkill.Visible = (CHK_CaughtMon.Checked && db.Mons[ind].Rest.Item2 > 1);
            PB_Mon.Image = GetCaughtImage(ind, CHK_CaughtMon.Checked);
            PB_MegaX.Visible = CHK_MegaX.Visible = db.HasMega[ind][0];
            PB_MegaY.Visible = CHK_MegaY.Visible = db.HasMega[ind][1];
            PB_MegaX.Image = db.HasMega[ind][0] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + db.Mons[ind].Item1.ToString("000") + (db.HasMega[ind][1] ? "_X" : string.Empty))) : new Bitmap(16, 16);
            PB_MegaY.Image = db.HasMega[ind][1] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + db.Mons[ind].Item1.ToString("000") + "_Y")) : new Bitmap(16, 16);
            CHK_MegaX.Checked = (GetMon(ind).Stone & 1) != 0;
            CHK_MegaY.Checked = (GetMon(ind).Stone & 2) != 0;
            NUP_SpeedUpX.Visible = PB_SpeedUpX.Visible = CHK_CaughtMon.Checked && CHK_MegaX.Visible && CHK_MegaX.Checked;
            NUP_SpeedUpY.Visible = PB_SpeedUpY.Visible = CHK_CaughtMon.Checked && CHK_MegaY.Visible && CHK_MegaY.Checked; //Else NUP_SpeedUpY appears if the next mega in terms of offsets has been obtained
            PB_SpeedUpX.Image = db.HasMega[ind][0] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);
            PB_SpeedUpY.Image = db.HasMega[ind][1] ? new Bitmap(ResizeImage((Image)Properties.Resources.ResourceManager.GetObject("mega_speedup"), 24, 24)) : new Bitmap(16, 16);

            #endregion Visibility            
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

        private void UpdateStageBox()
        {
            int ind, type;
            byte[] stage;
            Object nup, pb;
            foreach (Label lbl in new[] { L_RankM, L_RankEx, L_RankEv })
            {
                if (lbl == L_RankM)
                {
                    ind = (int)NUP_MainIndex.Value - 1;
                    type = 0;
                    nup = NUP_MainScore;
                    pb = PB_Main;
                    stage = db.StagesMain;
                }
                else if (lbl == L_RankEx)
                {
                    ind = (int)NUP_ExpertIndex.Value - 1;
                    type = 1;
                    nup = NUP_ExpertScore;
                    pb = PB_Expert;
                    stage = db.StagesExpert;
                }
                else if (lbl == L_RankEv)
                {
                    ind = (int)NUP_EventIndex.Value;
                    type = 2;
                    nup = NUP_EventScore;
                    pb = PB_Event;
                    stage = db.StagesEvent;
                }
                else break;
                GetRankImage(lbl, GetStage(ind, type).Rank, GetStage(ind, type).Completed);
                (nup as NumericUpDown).Value = GetStage(ind, type).Score;
                (pb as PictureBox).Image = GetStageImage(BitConverter.ToInt16(stage, 0x50 + BitConverter.ToInt32(stage, 0x4) * ((type == 0) ? ind + 1 : ind)) & 0x3FF, type);
                //(pb as PictureBox).Image = GetCompletedImage(BitConverter.ToInt16(stage, 0x50 + BitConverter.ToInt32(stage, 0x4) * ((type == 0) ? ind + 1 : ind)) & 0x3FF, type, (type == 0) ? (GetStage(ind, type).Completed || overrideHS) : true);
            }
            PB_override.Image = overrideHS ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("warn")) : new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("valid"));
        }

        private void B_CheatsForm_Click(object sender, EventArgs e)
        {
            new Cheats().ShowDialog();
            updating = true;
            Parse();
            updating = false;
        }

        private void B_Open_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { FileName = "savedata.bin", Filter = ".bin files (*.bin)|*.bin|All files (*.*)|*.*", FilterIndex = 1 };
            if (ofd.ShowDialog() == DialogResult.OK)
                Open(ofd.FileName);
        }

        private void B_Save_Click(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;
            updating = true;
            SaveFileDialog sfd = new SaveFileDialog { FileName = Path.GetFileName(TB_FilePath.Text), Filter = ".bin files (*.bin)|*.bin|All files (*.*)|*.*", FilterIndex = 1 };
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                File.WriteAllBytes(sfd.FileName, savedata);
            MessageBox.Show("Saved save file to " + sfd.FileName + "." + Environment.NewLine + "Remember to delete secure value before importing.");
            }            
            updating = false;
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
            if ((sender as Control).Name.Contains("Skill"))
                s = 5;
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
                        CHK_MegaX.Checked = db.HasMega[(int)CB_MonIndex.SelectedValue][0];
                        CHK_MegaY.Checked = db.HasMega[(int)CB_MonIndex.SelectedValue][1];
                        NUP_SpeedUpX.Value = (db.HasMega[(int)CB_MonIndex.SelectedValue][0]) ? NUP_SpeedUpX.Maximum : 0;
                        NUP_SpeedUpY.Value = (db.HasMega[(int)CB_MonIndex.SelectedValue][1]) ? NUP_SpeedUpY.Maximum : 0;
                        NUP_SkillLvl.Value = NUP_SkillLvl.Maximum;
                    }
                    else CHK_CaughtMon.Checked = CHK_MegaX.Checked = CHK_MegaY.Checked = false;
                    break;

                case 5:
                    bool boool = false;
                    foreach (int sLv in GetMon((int)CB_MonIndex.SelectedValue).SkillLevel)
                    {
                        if (sLv != 5 && sLv != 0)
                            boool = true;
                    }
                    for (int i = 0; i < db.Rest[(int)CB_MonIndex.SelectedValue].Item2; i++)
                            SetSkill((int)CB_MonIndex.SelectedValue, (boool ? 5 : 1), i);
                    break;

                default:
                    return;
            }
            updating = true;
            Parse();
            updating = false;
        }

        private void PB_Stage_Click(object sender, EventArgs e)
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
            if ((e as MouseEventArgs).Button == MouseButtons.Left)    //Left Click
            {
                if (GetStage(ind, i - 1).Completed) //is completed
                {
                    if (GetStage(ind, i - 1).Rank > 0 && GetStage(ind, i - 1).Rank < 4) //is rank != C
                        SetRank(ind, i - 1, GetStage(ind, i - 1).Rank - 1);   //minus 1 rank
                    else //is rank = C or unknown
                    {
                        SetRank(ind, i - 1, 3);  //rank S
                        PatchScore(ind, i - 1);
                    }
                }
                //Nothing happens if uncompleted
            }
            if ((e as MouseEventArgs).Button == System.Windows.Forms.MouseButtons.Right)   //Right Click
            {
                SetStage(ind, i - 1, !GetStage(ind, i - 1).Completed);    //invert completed state
                if (GetStage(ind, i - 1).Completed) //is completed (was uncompleted)
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

        private void PB_Team_Click(object sender, EventArgs e)
        {
            int s = 0;
            if ((sender as Control).Name.Contains("Team1"))
                s = 1;
            if ((sender as Control).Name.Contains("Team2"))
                s = 2;
            if ((sender as Control).Name.Contains("Team3"))
                s = 3;
            if ((sender as Control).Name.Contains("Team4"))
                s = 4;
            if (s > 0 && s < 5)
            {
                if ((e as MouseEventArgs).Button == MouseButtons.Left)
                {
                    if (ModifierKeys == Keys.Control)
                    {
                        int ind = (int)CB_MonIndex.SelectedValue;
                        for (int i = 1; i < 5; i++)
                        {
                            if (i != s && GetTeam(i) == ind)
                                SetTeam(i, GetTeam(s));
                        }
                        if (!GetMon(ind).Caught)
                            SetCaught(ind, true);
                        SetTeam(s, ind);
                    }
                    else                    
                        CB_MonIndex.SelectedValue = GetTeam(s);
                    updating = true;
                    Parse();
                    updating = false;
                }
                else if ((e as MouseEventArgs).Button == MouseButtons.Right)
                {
                    if (ltir == 0)
                    {
                        updating = true;
                        Parse(s);
                        updating = false;
                    }
                    else
                    {
                        int temp = GetTeam(ltir);
                        SetTeam(ltir, GetTeam(s));
                        SetTeam(s, temp);
                        updating = true;
                        Parse();
                        updating = false;
                    }
                }
            }
            else return;
        }

        private void TB_Filepath_DoubleClick(object sender, EventArgs e)
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

        private void Main_DragDrop(object sender, DragEventArgs e)
        {
            Open(GetDragFilename(e));
        }

        private void Main_DragEnter(object sender, DragEventArgs e)
        {
            string filename = GetDragFilename(e);
            e.Effect = (filename != null && IsShuffleSave(filename)) ? DragDropEffects.Copy : DragDropEffects.None;
        }

        private void ItemsGrid_EnabledChanged(object sender, EventArgs e)
        {
            ItemsGrid.SelectedObject = (ItemsGrid.Enabled) ? SI_Items : null;
        }

        private void ItemsGrid_Enter(object sender, EventArgs e)
        {
            if (lastkeys == Keys.Tab) { ItemsGrid.SelectedGridItem = ItemsGrid.EnumerateAllItems().First((item) => item.PropertyDescriptor != null); }
            else if (lastkeys == (Keys.Tab | Keys.Shift)) { ItemsGrid.SelectedGridItem = ItemsGrid.EnumerateAllItems().Last(); }
        }

        private void UpdateSkill(object s, EventArgs e)
        {
            NUP_SkillLvl.Value = GetMon((int)CB_MonIndex.SelectedValue).SkillLevel[(int)CB_Skill.SelectedValue];
            UpdateForm(s, e);
        }

        private void CB_MonIndex_EnabledChanged(object sender, EventArgs e)
        {
            if (CB_MonIndex.Enabled)
                CB_Skill_Init();
        }

        private void UpdateProperty(object s, PropertyValueChangedEventArgs e)
        {
            UpdateForm(s, e);
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            lastkeys = keyData;
            return base.ProcessCmdKey(ref msg, keyData);
        }
    }
}