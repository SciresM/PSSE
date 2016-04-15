using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
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
            byte[][] files = { megaStone, mondata, stagesMain, stagesEvent, stagesExpert };
            string[] filenames = { "megaStone.bin","pokemonData.bin","stageData.bin","stageDataEvent.bin","stageDataExtra.bin"};
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
                    }
                }
            }
            PB_Main.Image = PB_Event.Image = PB_Expert.Image = GetStageImage(0);
            string[] specieslist = Properties.Resources.species.Split(new[] {Environment.NewLine, "\n"}, StringSplitOptions.RemoveEmptyEntries);
            string[] monslist = Properties.Resources.mons.Split(new[] { Environment.NewLine, "\n"}, StringSplitOptions.RemoveEmptyEntries);            mons = new Tuple<int, int, bool, int>[BitConverter.ToUInt32(mondata, 0)];
            int[] forms = new int[specieslist.Length];
            HasMega = new bool[specieslist.Length][];
            for (int i = 0; i < specieslist.Length; i++)
                HasMega[i] = new bool[2];
            int[] megas = new int[BitConverter.ToUInt32(megaStone, 0)-1];
            for (int i = 0; i < megas.Length; i++)
            {
                megas[i] = BitConverter.ToUInt16(megaStone, 0x54 + i * 4) & 0x3FF;
            }
            for (int i = 0; i < mons.Length; i++)
            {
                int entrylen = BitConverter.ToInt32(mondata,0x4);
                byte[] data = mondata.Skip(0x50 + entrylen * i).Take(entrylen).ToArray();
                bool isMega = i > 882 && i < 934; //changed this because the index was changed
                int spec = isMega
                    ? specieslist.ToList().IndexOf(monslist[megas[i - 883]].Replace("Shiny","").Replace("Winking","").Replace("Smiling","").Replace(" ","")) //crappy but needed for IndexOf() to find the pokemon's name in specieslist (only adjectives on megas names matter)
                    : (BitConverter.ToInt32(data, 0xE) >> 6) & 0x7FF; //this changed too, also updated the resources.resx file with new pics
                int raiseMaxLevel = (BitConverter.ToInt16(data, 0x4)) & 0x3F;
                mons[i] = new Tuple<int, int, bool, int>(spec, forms[spec], isMega, raiseMaxLevel);
                forms[spec]++;
            }
            for (int i = 0; i < megas.Length; i++)
            {
                HasMega[mons[BitConverter.ToUInt16(megaStone, 0x54 + i * 4) & 0x3FF].Item1][(megaStone[0x54 + (i * 4) + 1] >> 3) & 1] = true;
            }
            List<cbItem> monsel = new List<cbItem>();
            for (int i = 1; i < 868; i++)
            {
                monsel.Add(new cbItem { Text = monslist[i], Value = i });
            }
            monsel = monsel.OrderBy(ncbi => ncbi.Text).ToList();
            CB_MonIndex.DataSource = monsel;
            CB_MonIndex.DisplayMember = "Text";
            CB_MonIndex.ValueMember = "Value";
            NUP_ExpertIndex.Minimum = NUP_EventIndex.Minimum = 0;
            NUP_MainIndex.Minimum = 1;
            NUP_MainIndex.Maximum = BitConverter.ToInt32(stagesMain, 0) - 1;
            NUP_ExpertIndex.Maximum = BitConverter.ToInt32(stagesExpert, 0) - 1;
            NUP_EventIndex.Maximum = BitConverter.ToInt32(stagesEvent, 0) - 1;
            NUP_MainScore.Minimum = NUP_ExpertScore.Minimum = NUP_EventScore.Minimum = 0;
            NUP_MainScore.Maximum = NUP_ExpertScore.Maximum = NUP_EventScore.Maximum = 0xFFFFFF;
            CHK_MegaY.Visible = CHK_MegaX.Visible = false;
            PB_Mon.Image = GetCaughtImage((int)CB_MonIndex.SelectedValue, CHK_CaughtMon.Checked);
            ItemsGrid.SelectedObject = null;
        }

        Tuple<int, int, bool, int>[] mons; //specieIndex, formIndex, isMega, raiseMaxLevel

        byte[] mondata = Properties.Resources.pokemonData;
        byte[] stagesMain = Properties.Resources.stageData;
        byte[] stagesEvent = Properties.Resources.stageDataEvent;
        byte[] stagesExpert = Properties.Resources.stageDataExtra;
        byte[] megaStone = Properties.Resources.megaStone;

        bool[][] HasMega; // [X][0] = X, [X][1] = Y

        ShuffleItems SI_Items = new ShuffleItems();

        byte[] savedata;
        bool loaded;

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
            SaveFileDialog sfd = new SaveFileDialog {FileName = TB_FilePath.Text};
            if (sfd.ShowDialog() != DialogResult.OK) return;

            File.WriteAllBytes(sfd.FileName, savedata);
            MessageBox.Show("Saved save file to " + sfd.FileName + "."+Environment.NewLine+"Remember to delete secure value before importing.");
        }

        private void Open(string file)
        {
            TB_FilePath.Text = file;
            savedata = File.ReadAllBytes(file);
            Parse();
            B_Save.Enabled = GB_Caught.Enabled = GB_HighScore.Enabled = GB_Resources.Enabled = B_CheatsForm.Enabled = ItemsGrid.Enabled = loaded = true;
            UpdateProperty(null, null);
        }

        // Try to do a better job at filtering files rather than just saying "oh, it's not savedata.bin quit"
        private bool IsShuffleSave(string file)
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

        private bool updating;

        private void UpdateForm(object sender, EventArgs e)
        {
            if (!loaded || updating)
                return;
            updating = true;
            if (sender != null && !((sender as Control).Name.ToLower().Contains("index")))
            {
                int ind = (int)CB_MonIndex.SelectedValue;
                int level_ofs = (((ind - 1) * 4) / 8);
                int level_shift = ((((ind - 1) * 4) + 1) % 8);
                ushort level = BitConverter.ToUInt16(savedata, 0x187+level_ofs);                
                int set_level = ((int)NUP_Level.Value) == 1 ? 0 : ((int)NUP_Level.Value);
                level = (ushort)((level & (ushort)(~(0xF << level_shift))) | (set_level << level_shift));
                Array.Copy(BitConverter.GetBytes(level), 0, savedata, 0x187 + level_ofs, 2);

                //lollipop patcher
                int rml_ofs = ((ind * 6) / 8);
                int rml_shift = ((ind * 6) % 8);
                ushort numRaiseMaxLevel = BitConverter.ToUInt16(savedata, 0xA9DB + rml_ofs);
                int set_rml = Math.Min(Math.Max((((int)NUP_Level.Value) - 10), ((numRaiseMaxLevel >> rml_shift) & 0x3F)), 5);
                numRaiseMaxLevel = (ushort)((numRaiseMaxLevel & (ushort)(~(0x3F << rml_shift))) | (set_rml << rml_shift));
                Array.Copy(BitConverter.GetBytes(numRaiseMaxLevel), 0, savedata, 0xA9DB + rml_ofs, 2);

                int caught_ofs = (((ind - 1) + 6) / 8);
                int caught_shift = (((ind - 1) + 6) % 8);
                foreach (int caught_array_start in new[] { 0xE6, 0x546, 0x5E6 })
                {
                    savedata[caught_array_start + caught_ofs] = (byte)(savedata[caught_array_start + caught_ofs] & (byte)(~(1 << caught_shift)) | ((CHK_CaughtMon.Checked ? 1 : 0) << caught_shift));
                }
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(savedata, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_MainScore.Value << 4))), 0, savedata, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1), 8);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(savedata, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_ExpertScore.Value << 4))), 0, savedata, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value), 8);
                Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(savedata, 0x52D5 + 3 * ((int)NUP_EventIndex.Value)) & 0xFFFFFFFFF000000FL) | (((ulong)NUP_EventScore.Value << 4))), 0, savedata, 0x52D5 + 3 * ((int)NUP_EventIndex.Value), 8);
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
                {
                    savedata[0x2D4C + i] = (byte)(((SI_Items.Enchantments[i] << 1) & 0xFE) | (savedata[0x2D4C + i] & 1));
                }

                int mega_ofs = 0x406 + ((ind + 2) / 4);
                ushort mega_val = BitConverter.ToUInt16(savedata, mega_ofs);
                mega_val &= (ushort)(~(3 << ((5 + (ind << 1)) % 8)));
                ushort new_mega_insert = (ushort)(0 | (CHK_MegaY.Checked ? 1 : 0) | (CHK_MegaX.Checked ? 2 : 0));
                mega_val |= (ushort)(new_mega_insert << ((5 + (ind << 1)) % 8));
                Array.Copy(BitConverter.GetBytes(mega_val), 0, savedata, mega_ofs, 2);
            }
            UpdateResourceBox();
            UpdateStageBox();
            UpdateOwnedBox();
            updating = false;
        }

        private void UpdateResourceBox()
        {
            NUP_Coins.Value = (BitConverter.ToUInt32(savedata, 0x68) >> 3) & 0x1FFFF;
            NUP_Jewels.Value = (BitConverter.ToUInt16(savedata, 0x6A) >> 4) & 0xFF;
            NUP_Hearts.Value = (BitConverter.ToUInt16(savedata, 0x2D4A) >> 7) & 0x7F;
            for (int i = 0; i < SI_Items.Items.Length; i++)
            {
                SI_Items.Items[i] = (BitConverter.ToUInt16(savedata, 0xD0 + i) >> 7) & 0x7F;
            }

            for (int i = 0; i < SI_Items.Enchantments.Length; i++)
            {
                SI_Items.Enchantments[i] = (savedata[0x2D4C + i] >> 1) & 0x7F;
            }
        }

        private void UpdateOwnedBox()
        {
            int ind = (int)CB_MonIndex.SelectedValue;
            int level_ofs = 0x187 + (((ind - 1) * 4) / 8);
            int level = BitConverter.ToUInt16(savedata, level_ofs);
            level >>= ((((ind-1)*4)+1) % 8);
            level &= 0xF;

            // The max on the box could be higher than 10 now            
            int num_raise_max_level = Math.Min(mons[ind].Item4, 5); //int num_raise_max_level = Math.Min(((BitConverter.ToUInt16(savedata, 0xA9DB + ((ind * 6) / 8)) >> ((ind * 6) % 8)) & 0x3F), 5); -> old one, uses current number of lollipops given
            NUP_Level.Maximum = 10 + num_raise_max_level;
            
            // Stop showing 0 for the level...
            NUP_Level.Value = level > 0 ? level : 1;
            int caught_ofs = 0x546+(((ind-1)+6)/8);
            CHK_CaughtMon.Checked = ((savedata[caught_ofs] >> ((((ind-1)+6) % 8))) & 1) == 1;

            // There's no level if the Pokémon hasn't been caught
            NUP_Level.Visible = CHK_CaughtMon.Checked;

            PB_Mon.Image = GetCaughtImage(ind, CHK_CaughtMon.Checked);
            #region Mega Visibility
            CHK_MegaY.Visible = HasMega[mons[ind].Item1][0];
            CHK_MegaX.Visible = HasMega[mons[ind].Item1][1];
            PB_MegaX.Image = HasMega[mons[ind].Item1][0] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + mons[ind].Item1.ToString("000") + ((HasMega[mons[ind].Item1][0] && HasMega[mons[ind].Item1][1]) ? "_X" : string.Empty))) : new Bitmap(16, 16);
            PB_MegaY.Image = HasMega[mons[ind].Item1][1] ? new Bitmap((Image)Properties.Resources.ResourceManager.GetObject("MegaStone" + mons[ind].Item1.ToString("000") + "_Y")) : new Bitmap(16, 16); 
            #endregion
            int mega_ofs = 0x406 + ((ind + 2) / 4);
            CHK_MegaY.Checked = ((BitConverter.ToUInt16(savedata, mega_ofs) >> ((5 + (ind << 1)) % 8)) & 1) == 1;
            CHK_MegaX.Checked = (((BitConverter.ToUInt16(savedata, mega_ofs) >> ((5 + (ind << 1)) % 8)) >> 1) & 1) == 1;
        }

        private void UpdateStageBox()
        {
            int stagelen = BitConverter.ToInt32(stagesMain, 0x4);
            int mainspec = BitConverter.ToUInt16(stagesMain, 0x50 + stagelen * ((int)NUP_MainIndex.Value)) & 0x3FF;
            int expertspec = BitConverter.ToUInt16(stagesExpert, 0x50 + stagelen * ((int)NUP_ExpertIndex.Value)) & 0x3FF;
            int eventspec = BitConverter.ToUInt16(stagesEvent, 0x50 + stagelen * ((int)NUP_EventIndex.Value)) & 0x3FF;
            PB_Main.Image = GetStageImage(mons[mainspec].Item1, mons[mainspec].Item2, mons[mainspec].Item3);
            PB_Expert.Image = GetStageImage(mons[expertspec].Item1, mons[expertspec].Item2, mons[expertspec].Item3);
            PB_Event.Image = GetStageImage((mons[eventspec].Item1 == 25) ? 0 : mons[eventspec].Item1, mons[eventspec].Item2, mons[eventspec].Item3);
            NUP_MainScore.Value = (BitConverter.ToUInt64(savedata, 0x4141 + 3 * ((int)NUP_MainIndex.Value - 1)) >> 4) & 0x00FFFFFF;
            NUP_ExpertScore.Value = (BitConverter.ToUInt64(savedata, 0x4F51 + 3 * ((int)NUP_ExpertIndex.Value)) >> 4) & 0x00FFFFFF;
            NUP_EventScore.Value = (BitConverter.ToUInt64(savedata, 0x52D5 + 3 * ((int)NUP_EventIndex.Value)) >> 4) & 0x00FFFFFF;
        }

        private Bitmap GetCaughtImage(int ind, bool caught)
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
            if (mega && !HasMega[mon_num][1]) //pretty hacky but necessary to differenciate Rayquaza/Gyarados from Charizard/Mewtwo
                form -= 2;                    
            if (mega && HasMega[mon_num][1])  //Otherwise, either stage 300 is Shiny M-Ray or stage 150 is M-mewtwo X
                form--;
            if (mega)
                imgname += "mega_";
            imgname += "pokemon_" + mon_num.ToString("000");
            if (form > 0 && mon_num > 0)
                imgname += "_" + form.ToString("00");
            if (mega)
                imgname += "_lo";
            return new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(imgname));
        }

        private Bitmap GetStageImage(int mon_num, int form = 0, bool mega = false)
        {
            Bitmap bmp = new Bitmap(64, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.DrawImage(Properties.Resources.Plate, new Point(0, 16));
                g.DrawImage(ResizeImage(GetMonImage(mon_num, form, mega), 48, 48), new Point(8, 8));
            }

            return bmp;
        }

        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

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

        private void B_CheatsForm_Click(object sender, EventArgs e)
        {
            (new Cheats(mondata, stagesMain, stagesEvent, stagesExpert, megaStone, HasMega, mons, ref savedata)).ShowDialog();
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
                    {
                        return ((string[])data)[0];
                    }
                }
            }
            return null;
        }
    }

    public class cbItem
    {
        public string Text { get; set; }
        public object Value { get; set; }
    }
}
