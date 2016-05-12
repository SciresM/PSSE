using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Pokemon_Shuffle_Save_Editor
{
    class ToolFunctions
    {
        public static void SetLevel(Database db, int ind, int lev = 1)
        {
            if (lev < 1)
                lev = 1;
            if (lev > 15)
                lev = 15;   //hardcoded 5 as the max number of lollipops, change this if needed
            int level_ofs = (((ind - 1) * 4) / 8);
            int level_shift = ((((ind - 1) * 4) + 1) % 8);
            ushort level = BitConverter.ToUInt16(db.SaveData, 0x187 + level_ofs);
            level = (ushort)((level & (ushort)(~(0xF << level_shift))) | (lev << level_shift));
            Array.Copy(BitConverter.GetBytes(level), 0, db.SaveData, 0x187 + level_ofs, 2);

            //experience patcher
            int exp_ofs = ((4 + ((ind - 1) * 24)) / 8);
            int exp_shift = ((4 + ((ind - 1) * 24)) % 8);
            int exp = BitConverter.ToInt32(db.SaveData, 0x3241 + exp_ofs);
            int entrylen = BitConverter.ToInt32(db.MonLevel, 0x4);
            byte[] data = db.MonLevel.Skip(0x50 + ((lev - 1) * entrylen)).Take(entrylen).ToArray();
            int set_exp = BitConverter.ToInt32(data, 0x4 * (db.Mons[ind].Item5 - 1));
            exp = (exp & ~(0xFFFFFF << exp_shift)) | (set_exp << exp_shift);
            Array.Copy(BitConverter.GetBytes(exp), 0, db.SaveData, 0x3241 + exp_ofs, 4);

            //lollipop patcher
            int rml_ofs = ((ind * 6) / 8);
            int rml_shift = ((ind * 6) % 8);
            ushort numRaiseMaxLevel = BitConverter.ToUInt16(db.SaveData, 0xA9DB + rml_ofs);
            int set_rml = Math.Min((lev - 10 < 0) ? 0 : (lev - 10), 5); //hardcoded 5 as the max number of lollipops, change this if needed
            numRaiseMaxLevel = (ushort)((numRaiseMaxLevel & (ushort)(~(0x3F << rml_shift))) | (set_rml << rml_shift));
            Array.Copy(BitConverter.GetBytes(numRaiseMaxLevel), 0, db.SaveData, 0xA9DB + rml_ofs, 2);
        }

        public static void SetPokemon(Database db, int ind, bool caught)
        {
            int caught_ofs = (((ind - 1) + 6) / 8);
            int caught_shift = (((ind - 1) + 6) % 8);
            foreach (int caught_array_start in new[] { 0xE6, 0x546, 0x5E6 })
                db.SaveData[caught_array_start + caught_ofs] = (byte)(db.SaveData[caught_array_start + caught_ofs] & (byte)(~(1 << caught_shift)) | ((caught ? 1 : 0) << caught_shift));
        }

        public static bool GetPokemon(Database db, int ind)
        {
            int caught_ofs = 0x546 + (((ind - 1) + 6) / 8);
            return ((db.SaveData[caught_ofs] >> ((((ind - 1) + 6) % 8))) & 1) == 1;
        }

        public static void SetMegaStone(Database db, int ind, bool X, bool Y)
        {
            int mega_ofs = 0x406 + ((ind + 2) / 4);
            ushort mega_val = BitConverter.ToUInt16(db.SaveData, mega_ofs);
            mega_val &= (ushort)(~(3 << ((5 + (ind << 1)) % 8)));
            ushort new_mega_insert = (ushort)(0 | (X ? 1 : 0) | (Y ? 2 : 0));
            mega_val |= (ushort)(new_mega_insert << ((5 + (ind << 1)) % 8));
            Array.Copy(BitConverter.GetBytes(mega_val), 0, db.SaveData, mega_ofs, 2);
        }

        public static void SetMegaSpeedup(Database db, int ind, bool X, bool Y)
        {
            if (db.HasMega[db.Mons[ind].Item1][0] || db.HasMega[db.Mons[ind].Item1][1])
            {
                int suX_ofs = (((db.MegaList.IndexOf(ind) * 7) + 3) / 8);
                int suX_shift = (((db.MegaList.IndexOf(ind) * 7) + 3) % 8);
                int suY_ofs = (((db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7) + 3) / 8);
                int suY_shift = (((db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7) + 3) % 8) + ((suY_ofs - suX_ofs) * 8); //relative to suX_ofs
                int speedUp_ValX = BitConverter.ToInt32(db.SaveData, 0x2D5B + suX_ofs);
                int speedUp_ValY = BitConverter.ToInt32(db.SaveData, 0x2D5B + suY_ofs);
                int set_suX = X ? db.Megas[db.MegaList.IndexOf(ind)].Item2 : 0;
                int set_suY = Y ? db.Megas[db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1)].Item2 : 0;
                int newSpeedUp = db.HasMega[db.Mons[ind].Item1][1]
                    ? ((((speedUp_ValX & ~(0x7F << suX_shift)) & ~(0x7F << suY_shift)) | (set_suX << suX_shift)) | (set_suY << suY_shift)) //Erases both X & Y bits at the same time before updating them to make sure Y doesn't overwrite X bits
                    : (speedUp_ValX & ~(0x7F << suX_shift)) | (set_suX << suX_shift);
                Array.Copy(BitConverter.GetBytes(newSpeedUp), 0, db.SaveData, 0x2D5B + suX_ofs, 4);
            }
        }

        public static void SetStage(Database db, int ind, int type, bool completed = false)
        {
            int stage_ofs, stage_shift = (ind * 3) % 8;
            int entrylen = BitConverter.ToInt32(db.StagesMain, 0x4);
            switch (type)
            {
                case 0:
                    stage_ofs = 0x688 + ((ind * 3) / 8); //Main
                    break;
                case 1:
                    stage_ofs = 0x84A + ((ind * 3) / 8); //Expert
                    break;
                case 2:
                    stage_ofs = 0x8BA + (4 + ind * 3) / 8; //Event
                    stage_shift = (4 + ind * 3) % 8;
                    break;
                default:
                    return;
            }
            ushort stage = BitConverter.ToUInt16(db.SaveData, stage_ofs);
            stage = (ushort)((stage & (ushort)(~(0x7 << stage_shift))) | ((completed ? 5 : 0) << stage_shift));
            Array.Copy(BitConverter.GetBytes(stage), 0, db.SaveData, stage_ofs, 2);
            if (!completed)
                SetRank(db, ind, type, 0);
            else
                SetRank(db, ind, type, GetRank(db, ind, type));
        }

        public static bool GetStage(Database db, int ind, int type)
        {
            int stage_ofs, stage_shift = ind * 3 % 8;
            switch (type)
            {
                case 0:
                    stage_ofs = 0x688 + ind * 3 / 8; //Main
                    break;
                case 1:
                    stage_ofs = 0x84A + ind * 3 / 8; //Expert
                    break;
                case 2:
                    stage_ofs = 0x8BA + (4 + ind * 3) / 8; //Event
                    stage_shift = (4 + ind * 3) % 8;
                    break;
                default:
                    return false;
            }
            return ((BitConverter.ToInt16(db.SaveData, stage_ofs) >> stage_shift) & 7) == 5;
        }

        public static void SetRank(Database db, int ind, int type, int newRank = 0)
        {
            int rank_ofs;
            int entrylen = BitConverter.ToInt32(db.StagesMain, 0x4);
            switch (type)
            {
                case 0:
                    rank_ofs = 0x987 + (7 + ind * 2) / 8; //Main
                    break;
                case 1:
                    rank_ofs = 0xAB3 + (7 + ind * 2) / 8; //Expert
                    break;
                case 2:
                    rank_ofs = 0xAFE + (7 + ind * 2) / 8; //Event
                    break;
                default:
                    return;
            }
            int rank_shift = (7 + ind * 2) % 8;
            ushort rank = BitConverter.ToUInt16(db.SaveData, rank_ofs);
            rank = (ushort)((rank & (ushort)(~(0x3 << rank_shift))) | (newRank << rank_shift));
            Array.Copy(BitConverter.GetBytes(rank), 0, db.SaveData, rank_ofs, 2);
            byte[] data = db.StagesMain.Skip(0x50 + (ind + 1) * entrylen).Take(entrylen).ToArray();
            SetScore(db, ind, type, Math.Max(GetScore(db, ind, type), (BitConverter.ToUInt64(data, 0x4) & 0xFFFFFFFF) + (ulong)Math.Min(7000, ((newRank > 0) ? (BitConverter.ToInt16(data, 0x30 + (newRank - 1)) >> 4) & 0xFF : 0) * 500))); //score = Max(current_highscore, hitpoints + minimum_bonus_points (a.k.a min moves left times 500, capped at 7000))
        }

        public static int GetRank(Database db, int ind, int type)
        {
            int rank_ofs;
            switch (type)
            {
                case 0:
                    rank_ofs = 0x987 + (7 + ind * 2) / 8; //Main
                    break;
                case 1:
                    rank_ofs = 0xAB3 + (7 + ind * 2) / 8; //Expert
                    break;
                case 2:
                    rank_ofs = 0xAFE + (7 + ind * 2) / 8; //Event
                    break;
                default:
                    return 0;
            }
            return ((BitConverter.ToInt16(db.SaveData, rank_ofs) >> (7 + ind * 2) % 8) & 0x3);
        }

        public static void SetScore(Database db, int ind, int type, ulong newScore = 0)
        {
            int score_ofs, entrylen = BitConverter.ToInt32(db.StagesMain, 0x4);
            switch (type)
            {
                case 0:
                    score_ofs = 0x4141 + 3 * ind; //Main
                    break;
                case 1:
                    score_ofs = 0x4F51 + 3 * ind; //Expert
                    break;
                case 2:
                    score_ofs = 0x52D5 + 3 * ind; //Event
                    break;
                default:
                    return;
            }
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt64(db.SaveData, score_ofs) & 0xFFFFFFFFF000000FL) | (newScore << 4)), 0, db.SaveData, score_ofs, 8);
        }

        public static ulong GetScore(Database db, int ind, int type)
        {
            int score_ofs, entrylen = BitConverter.ToInt32(db.StagesMain, 0x4);
            switch (type)
            {
                case 0:
                    score_ofs = 0x4141 + 3 * ind; //Main
                    break;
                case 1:
                    score_ofs = 0x4F51 + 3 * ind; //Expert
                    break;
                case 2:
                    score_ofs = 0x52D5 + 3 * ind; //Event
                    break;
                default:
                    return 0;
            }
            return (BitConverter.ToUInt64(db.SaveData, score_ofs) >> 4) & 0x00FFFFFF;
        }

        public static void SetResources(Database db, int hearts = 0, uint coins = 0, uint jewels = 0, int[] items = null, int[] enhancements = null)
        {
            if (items == null)
                items = new int[7];
            if (enhancements == null)
                enhancements = new int[9];
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt32(db.SaveData, 0x68) & 0xF0000007) | (coins << 3) | (jewels << 20)), 0, db.SaveData, 0x68, 4);
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt16(db.SaveData, 0x2D4A) & 0xC07F) | (hearts << 7)), 0, db.SaveData, 0x2D4A, 2);
            for (int i = 0; i < 7; i++) //Items (battle)
            {
                ushort val = BitConverter.ToUInt16(db.SaveData, 0xd0 + i);
                val &= 0x7F;
                val |= (ushort)(items[i] << 7);
                Array.Copy(BitConverter.GetBytes(val), 0, db.SaveData, 0xd0 + i, 2);
            }
            for (int i = 0; i < 9; i++) //Enhancements (pokemon)
                db.SaveData[0x2D4C + i] = (byte)((((enhancements[i]) << 1) & 0xFE) | (db.SaveData[0x2D4C + i] & 1));
        }

        public static void SetExcalationStep(Database db, int step = 0)
        {
            if (step < 0)
                step = 0;
            if (step > 999)
                step = 999;
            int data = BitConverter.ToUInt16(db.SaveData, 0x2D59);
            data = (data & (~(0x3FF << 2))) | (step << 2);
            Array.Copy(BitConverter.GetBytes(data), 0, db.SaveData, 0x2D59, 2); //Will only update 1 escalation battle. Update offsets if there ever are more than 1 at once
        }

        public static Bitmap GetCaughtImage(Database db, int ind, bool caught = false)
        {
            Bitmap bmp = GetMonImage(db, ind);
            GetBlackImage(bmp, caught);
            return bmp;
        }

        public static Bitmap GetMonImage(Database db, int ind)
        {
            string imgname = string.Empty;
            int mon_num = db.Mons[ind].Item1, form = db.Mons[ind].Item2;
            bool mega = db.Mons[ind].Item3;
            if (mega)
            {
                form -= db.HasMega[mon_num][1] ? 1 : 2; //Differenciate Rayquaza/Gyarados from Charizard/Mewtwo, otherwise either stage 300 is Shiny M-Ray or stage 150 is M-mewtwo X
                imgname += "mega_";
            }
            imgname += "pokemon_" + mon_num.ToString("000");
            if (form > 0 && mon_num > 0)
                imgname += "_" + form.ToString("00");
            if (mega)
                imgname += "_lo";
            return new Bitmap((Image)Properties.Resources.ResourceManager.GetObject(imgname));
        }

        public static Bitmap GetStageImage(Database db, int ind, bool completed = true, int type = 0, bool overridePB = false)
        {
            int mon_num = db.Mons[ind].Item1, form = db.Mons[ind].Item2;
            bool mega = db.Mons[ind].Item3;
            Bitmap bmp = new Bitmap(64, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                if (mega && !(type == 2))
                    g.DrawImage(Properties.Resources.PlateMega, new Point(0, 16));
                else
                    g.DrawImage(Properties.Resources.Plate, new Point(0, 16));
                g.DrawImage(ResizeImage(GetMonImage(db, ind), 48, 48), new Point(8, 7));
                GetBlackImage(bmp, (type == 0) ? completed : true);
            }

            return bmp;
        }

        public static Bitmap GetBlackImage(Bitmap bmp, bool caught = true)
        {
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

        public static void GetRankImage(Label label, int rank = default(int), bool completed = false) //Plain text until a way is found to extract Rank sprites from game's folders.
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
    }
}
