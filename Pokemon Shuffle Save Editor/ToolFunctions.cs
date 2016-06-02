using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static Pokemon_Shuffle_Save_Editor.Main;

namespace Pokemon_Shuffle_Save_Editor
{
    public static class ToolFunctions
    {
        public static monItem GetMon(int ind)
        {
            bool caught = true;
            foreach (int array in Caught.Ofset(ind))
            {
                if (((savedata[array] >> Caught.Shift(ind)) & 1) != 1)
                    caught = false;
            }
            int lev = (BitConverter.ToUInt16(savedata, Level.Ofset(ind)) >> Level.Shift(ind)) & 0xF;
            lev = (lev == 0) ? 1 : lev;
            int rml = Math.Min((BitConverter.ToUInt16(savedata, Lollipop.Ofset(ind)) >> Lollipop.Shift(ind)) & 0x3F, 5); //hardcoded 5 as a maximum
            int exp = (BitConverter.ToInt32(savedata, Experience.Ofset(ind)) >> Experience.Shift(ind)) & 0xFFFFFF;
            short stone = (short)((savedata[Mega.Ofset(ind)] >> Mega.Shift(ind)) & 3);  //0 = 00, 1 = X0, 2 = 0Y, 3 = XY
            short speedUpX = (short)(db.HasMega[db.Mons[ind].Item1][0] ? (BitConverter.ToInt16(savedata, SpeedUpX.Ofset(ind)) >> SpeedUpX.Shift(ind)) & 0x7F : 0);
            short speedUpY = (short)(db.HasMega[db.Mons[ind].Item1][1] ? (BitConverter.ToInt32(savedata, SpeedUpY.Ofset(ind)) >> SpeedUpY.Shift(ind)) & 0x7F : 0);
            short skillLvl = (short)((BitConverter.ToInt16(savedata, SkillLevel.Ofset(ind)) >> SkillLevel.Shift(ind)) & 0x7);
            skillLvl = (skillLvl < 2) ? (short)1 : skillLvl;
            short skillExp = savedata[SkillExp.Ofset(ind)];

            return new monItem { Caught = caught, Level = lev, Lollipops = rml, Exp = exp, Stone = stone, SpeedUpX = speedUpX, SpeedUpY = speedUpY, SkillLevel = skillLvl, SkillExp = skillExp };
        }

        public static void SetLevel(int ind, int lev = 1, int set_rml = -1, int set_exp = -1)
        {
            if (savedata != null)
            {
                //level patcher
                lev = (lev < 2) ? 0 : ((lev > 15) ? 15 : lev);    //hardcoded 15 as the max level, change this if ever needed
                short level = (short)((BitConverter.ToInt16(savedata, Level.Ofset(ind)) & ~(0xF << Level.Shift(ind))) | (lev << Level.Shift(ind)));
                Array.Copy(BitConverter.GetBytes(level), 0, savedata, Level.Ofset(ind), 2);

                //lollipop patcher
                set_rml = (set_rml < 0) ? ((lev - 10 < 0) ? 0 : (lev - 10)) : set_rml;
                short numRaiseMaxLevel = (short)((BitConverter.ToInt16(savedata, Lollipop.Ofset(ind)) & ~(0x3F << Lollipop.Shift(ind))) | (set_rml << Lollipop.Shift(ind)));
                Array.Copy(BitConverter.GetBytes(numRaiseMaxLevel), 0, savedata, Lollipop.Ofset(ind), 2);

                //experience patcher
                int entrylen = BitConverter.ToInt32(db.MonLevel, 0x4);
                byte[] data = db.MonLevel.Skip(0x50 + ((((lev < 2) ? 1 : lev) - 1) * entrylen)).Take(entrylen).ToArray(); //corrected level value, because if it's 0 then it means 1
                set_exp = (set_exp < 0) ? BitConverter.ToInt32(data, 0x4 * (db.Mons[ind].Item5 - 1)) : set_exp;
                int exp = (BitConverter.ToInt32(savedata, Experience.Ofset(ind)) & ~(0xFFFFFF << Experience.Shift(ind))) | (set_exp << Experience.Shift(ind));
                Array.Copy(BitConverter.GetBytes(exp), 0, savedata, Experience.Ofset(ind), 4);
            }            
        }

        public static void SetCaught(int ind, bool caught)
        {
            foreach (int array in Caught.Ofset(ind))
                savedata[array] = (byte)((savedata[array] & (byte)(~(1 << Caught.Shift(ind)))) | (byte)((caught ? 1 : 0) << Caught.Shift(ind)));
        }

        public static void SetStone(int ind, bool X = false, bool Y = false)
        {
            short mega_val = (short)((BitConverter.ToInt16(savedata, Mega.Ofset(ind)) & ~(3 << Mega.Shift(ind))) | (((X ? 1 : 0) | (Y ? 2 : 0)) << Mega.Shift(ind)));
            Array.Copy(BitConverter.GetBytes(mega_val), 0, savedata, Mega.Ofset(ind), 2);
        }

        public static void SetSpeedup(int ind, bool X = false, int suX = 0, bool Y = false, int suY = 0)
        {
            if (db.HasMega[db.Mons[ind].Item1][0] || db.HasMega[db.Mons[ind].Item1][1])
            {
                int speedUp_Val = BitConverter.ToInt32(savedata, SpeedUpX.Ofset(ind));
                if (db.HasMega[db.Mons[ind].Item1][0])
                {
                    speedUp_Val &= ~(0x7F << SpeedUpX.Shift(ind));
                    speedUp_Val |= (X ? suX : 0) << SpeedUpX.Shift(ind);
                }
                if (db.HasMega[db.Mons[ind].Item1][1])
                {   //Y shifts are relative to X ofsets.
                    speedUp_Val &= ~(0x7F << ((SpeedUpY.Ofset(ind) - SpeedUpX.Ofset(ind)) * 8 + SpeedUpY.Shift(ind)));
                    speedUp_Val |= (Y ? suY : 0) << ((SpeedUpY.Ofset(ind) - SpeedUpX.Ofset(ind)) * 8 + SpeedUpY.Shift(ind));
                }
                Array.Copy(BitConverter.GetBytes(speedUp_Val), 0, savedata, SpeedUpX.Ofset(ind), 4);
            }
        }

        public static void SetSkill(int ind, int lvl = 1)
        {
            //level
            lvl = (lvl < 2) ? 0 : ((lvl > 5) ? 5 : lvl);    //hardcoded skill level to be 5 max
            int skilllvl = BitConverter.ToInt16(savedata, SkillLevel.Ofset(ind));
            skilllvl = (skilllvl & ~(0x7 << SkillLevel.Shift(ind))) | (lvl << SkillLevel.Shift(ind));
            Array.Copy(BitConverter.GetBytes(skilllvl), 0, savedata, SkillLevel.Ofset(ind), 2);

            //exp
            int entrylen = BitConverter.ToInt32(db.MonAbility, 0x4);
            savedata[SkillExp.Ofset(ind)] = (lvl < 2) ? (byte)0 : db.MonAbility.Skip(0x50 + db.Mons[ind].Item6 * entrylen).Take(entrylen).ToArray()[0x1A + lvl];
        }

        public static stgItem GetStage(int ind, int type)
        {
            return new stgItem {
                Completed = ((BitConverter.ToInt16(savedata, Completed.Ofset(ind, type)) >> Completed.Shift(ind, type)) & 7) == 5,
                Rank = (BitConverter.ToInt16(savedata, Rank.Ofset(ind, type)) >> Rank.Shift(ind, type)) & 0x3,
                Score = (int)((BitConverter.ToUInt64(savedata, Score.Ofset(ind, type)) >> Score.Shift(ind, type)) & 0xFFFFFF)
            };
        }

        public static void SetStage(int ind, int type, bool completed = false)
        {
            short stage = (short)(BitConverter.ToInt16(savedata, Completed.Ofset(ind, type)) & (~(0x7 << Completed.Shift(ind, type))) | ((completed ? 5 : 0) << Completed.Shift(ind, type)));
            Array.Copy(BitConverter.GetBytes(stage), 0, savedata, Completed.Ofset(ind, type), 2);
        }

        public static void SetRank(int ind, int type, int newRank = 0)
        {
            short rank = (short)((BitConverter.ToInt16(savedata, Rank.Ofset(ind, type)) & ~(0x3 << Rank.Shift(ind, type))) | (newRank << Rank.Shift(ind, type)));
            Array.Copy(BitConverter.GetBytes(rank), 0, savedata, Rank.Ofset(ind, type), 2);
        }

        public static void SetScore(int ind, int type, ulong newScore = 0)
        {
            ulong score = (ulong)((BitConverter.ToUInt64(savedata, Score.Ofset(ind, type)) & (uint)(~(0xFFFFFF << Score.Shift(ind, type)))) | (newScore << Score.Shift(ind, type)));
            Array.Copy(BitConverter.GetBytes(score), 0, savedata, Score.Ofset(ind, type), 8);
        }

        public static void PatchScore(int ind, int type)
        {
            byte[] stage;
            int index = ind;
            switch (type)
            {
                case 0:
                    stage = db.StagesMain;
                    index = (ind + 1);
                    break;
                case 1:
                    stage = db.StagesExpert;
                    break;
                case 2:
                    stage = db.StagesEvent;
                    break;
                default:
                    return;
            }
            int entrylen = BitConverter.ToInt32(stage, 0x4);
            byte[] data = stage.Skip(0x50 + index * entrylen).Take(entrylen).ToArray();
            SetScore(ind, type, Math.Max((ulong)GetStage(ind, type).Score, (BitConverter.ToUInt64(data, 0x4) & 0xFFFFFFFF) + (ulong)(Math.Min(7000, ((GetStage(ind, type).Rank > 0) ? ((BitConverter.ToInt16(data, 0x30 + GetStage(ind, type).Rank - 1) >> 4) & 0xFF) : 0) * 500)))); //score = Max(current_highscore, hitpoints + minimum_bonus_points (a.k.a min moves left times 500, capped at 7000))
        }

        public static void SetExcalationStep(int step = 1)  //Will only update 1 escalation battle. Update offsets if there ever are more than 1 at once
        {
            if (step < 1)
                step = 1;
            if (step > 999)
                step = 999;
            int data = BitConverter.ToUInt16(savedata, EscalationStep.Ofset());
            data = (data & (~(0x3FF << EscalationStep.Shift()))) | (step-- << EscalationStep.Shift());  //sets previous step as beaten = selected step shown in game 
            Array.Copy(BitConverter.GetBytes(data), 0, savedata, EscalationStep.Ofset(), 2); 
        }

        public static rsItem GetRessources()
        {
            int[] items = new int[7], enhancements = new int[9];
            for (int i = 0; i < items.Length; i++)
                items[i] = (BitConverter.ToUInt16(savedata, Items.Ofset(i)) >> Items.Shift()) & 0x7F;
            for (int i = 0; i < enhancements.Length; i++)
                enhancements[i] = (savedata[Enhancements.Ofset(i)] >> Enhancements.Shift()) & 0x7F;
            return new rsItem
            {
                Hearts = (BitConverter.ToUInt16(savedata, Hearts.Ofset()) >> Hearts.Shift()) & 0x7F,
                Coins = (BitConverter.ToInt32(savedata, Coins.Ofset()) >> Coins.Shift()) & 0x1FFFF,
                Jewels = (BitConverter.ToInt32(savedata, Jewels.Ofset()) >> Jewels.Shift()) & 0xFF,
                Items = items,
                Enhancements = enhancements
            };
        }

        public static void SetResources(int hearts = 0, uint coins = 0, uint jewels = 0, int[] items = null, int[] enhancements = null)
        {
            if (items == null)
                items = new int[7];
            if (enhancements == null)
                enhancements = new int[9];
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt32(savedata, Coins.Ofset()) & 0xF0000007) | (coins << Coins.Shift()) | (jewels << Jewels.Shift())), 0, savedata, Coins.Ofset(), 4);
            Array.Copy(BitConverter.GetBytes((BitConverter.ToUInt16(savedata, Hearts.Ofset()) & 0xC07F) | (hearts << Hearts.Shift())), 0, savedata, Hearts.Ofset(), 2);
            for (int i = 0; i < 7; i++) //Items (battle)
            {
                ushort val = BitConverter.ToUInt16(savedata, Items.Ofset(i));
                val &= 0x7F;
                val |= (ushort)(items[i] << Items.Shift());
                Array.Copy(BitConverter.GetBytes(val), 0, savedata, Items.Ofset(i), 2);
            }
            for (int i = 0; i < 9; i++) //Enhancements (pokemon)
                savedata[Enhancements.Ofset(i)] = (byte)((((enhancements[i]) << Enhancements.Shift()) & 0xFE) | (savedata[Enhancements.Ofset(i)] & Enhancements.Shift()));
        }

        public static Bitmap GetMonImage(int ind)
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

        public static Bitmap GetStageImage(int ind, int type)
        {
            Bitmap bmp = new Bitmap(64, 80);
            using (Graphics g = Graphics.FromImage(bmp))
            {
                if (db.Mons[ind].Item3 && (type == 0))
                    g.DrawImage(Properties.Resources.PlateMega, new Point(0, 16));
                else
                    g.DrawImage(Properties.Resources.Plate, new Point(0, 16));
                g.DrawImage(ResizeImage(GetMonImage(ind), 48, 48), new Point(8, 7));
            }

            return bmp;
        }

        public static Bitmap GetBlackImage(Bitmap bmp, bool visible = true)
        {
            if (!visible)
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

        public static Bitmap GetCaughtImage(int ind, bool caught = false)
        {
            Bitmap bmp = GetMonImage(ind);
            GetBlackImage(bmp, caught);
            return bmp;
        }

        public static Bitmap GetCompletedImage(int ind, int type, bool completed = true)
        {
            Bitmap bmp = GetStageImage(ind, type);
            GetBlackImage(bmp, completed);
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

    #region Shifts&Ofsets
    public static class Caught
    {
        public static int[] Ofset(int ind)
        {
            int j = 0;
            int[] ofsets = new int[3];
            foreach (int caught_array_start in new[] { 0xE6, 0x546, 0x5E6 })
            {
                ofsets[j] = caught_array_start + ((ind - 1) + 6) / 8;
                j++;
            }
            return ofsets;
        }
        public static int Shift(int ind)
        {
            return ((ind - 1) + 6) % 8;
        }
    }
    public static class Level
    {
        public static int Ofset(int ind)
        {
            return 0x187 + ((ind - 1) * 4 + 1) / 8;
        }
        public static int Shift(int ind)
        {
            return ((ind - 1) * 4 + 1) % 8;
        }
    }
    public static class Lollipop
    {
        public static int Ofset(int ind)
        {
            return 0xA9DB + (ind * 6) / 8;
        }
        public static int Shift(int ind)
        {
            return (ind * 6) % 8;
        }
    }
    public static class Experience
    {
        public static int Ofset(int ind)
        {
            return 0x3241 + (4 + (ind - 1) * 24) / 8;
        }
        public static int Shift(int ind)
        {
            return (4 + (ind - 1) * 24) % 8;
        }
    }
    public static class Mega
    {
        public static int Ofset(int ind)
        {
            return 0x406 + (ind + 2) / 4;
        }
        public static int Shift(int ind)
        {
            return (5 + (ind << 1)) % 8;
        }
    }
    public static class SpeedUpX
    {
        public static int Ofset(int ind)
        {
            return 0x2D5B + (db.MegaList.IndexOf(ind) * 7 + 3) / 8;
        }
        public static int Shift(int ind)
        {
            return (db.MegaList.IndexOf(ind) * 7 + 3) % 8;
        }
    }
    public static class SpeedUpY
    {
        public static int Ofset(int ind)
        {
            return 0x2D5B + (db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7 + 3) / 8;
        }
        public static int Shift(int ind)
        {
            return (db.MegaList.IndexOf(ind, db.MegaList.IndexOf(ind) + 1) * 7 + 3) % 8;
        }
    }
    public static class SkillLevel
    {
        public static int Ofset(int ind)
        {
            return 0xAD9B + (ind * 3) / 8;
        }
        public static int Shift(int ind)
        {
            return (ind * 3) % 8;
        }
    }
    public static class SkillExp
    {
        public static int Ofset(int ind)
        {
            return 0xC9BB + (ind * 8) / 8;
        }
        public static int Shift(int ind)
        {
            return (ind * 8) % 8;
        }
    }    

    public static class Completed
    {
        public static int Ofset(int ind, int type)
        {
            switch (type)
            {
                case 0: //Main
                    return 0x688 + ind * 3 / 8;
                case 1: //Expert
                    return 0x84A + ind * 3 / 8;
                case 2: //Event
                    return 0x8BA + (4 + ind * 3) / 8;
                default:
                    throw new System.ArgumentException("Invalid type parameter", "type");
            }
        }
        public static int Shift(int ind, int type)
        {
            switch (type)
            {
                case 0: //Main
                    return (ind * 3) % 8;
                case 1: //Expert
                    return (ind * 3) % 8;
                case 2: //Event
                    return (4 + ind * 3) % 8;
                default:
                    throw new System.ArgumentException("Invalid type parameter", "type");
            }
        }
    }
    public static class Rank
    {
        public static int Ofset(int ind, int type)
        {
            switch (type)
            {
                case 0: //Main
                    return 0x987 + (7 + ind * 2) / 8;
                case 1: //Expert
                    return 0xAB3 + (7 + ind * 2) / 8;
                case 2: //Event
                    return 0xAFE + (7 + ind * 2) / 8;
                default:
                    throw new System.ArgumentException("Invalid type parameter", "type");
            }
        }
        public static int Shift(int ind, int type)
        {
            switch (type)
            {
                case 0: //Main
                    return (7 + ind * 2) % 8;
                case 1: //Expert
                    return (7 + ind * 2) % 8;
                case 2: //Event
                    return (7 + ind * 2) % 8;
                default:
                    throw new System.ArgumentException("Invalid type parameter", "type");
            }
        }
    }
    public static class Score
    {
        public static int Ofset(int ind, int type)
        {
            switch (type)
            {
                case 0: //Main
                    return 0x4141 + 3 * ind;
                case 1: //Expert
                    return 0x4F51 + 3 * ind;
                case 2: //Event
                    return 0x52D5 + 3 * ind;
                default:
                    throw new System.ArgumentException("Invalid type parameter", "type");
            }
        }
        public static int Shift(int ind, int type)
        {
            switch (type)
            {
                case 0: //Main
                    return 4;
                case 1: //Expert
                    return 4;
                case 2: //Event
                    return 4;
                default:
                    throw new System.ArgumentException("Invalid type parameter", "type");
            }
        }
    }
    public static class EscalationStep
    {
        public static int Ofset()
        {
            return 0x2D59;
        }
        public static int Shift()
        {
            return 2;
        }
    }

    public static class Items
    {
        public static int Ofset(int i)
        {
            return 0xD0 + i;
        }
        public static int Shift()
        {
            return 7;
        }
    }
    public static class Enhancements
    {
        public static int Ofset(int i)
        {
            return 0x2D4C + i;
        }
        public static int Shift()
        {
            return 1;
        }
    }
    public static class Hearts
    {   //these are stock hearts only
        public static int Ofset()
        {
            return 0x2D4A;
        }
        public static int Shift()
        {
            return 7;
        }
    }
    public static class Coins
    {
        public static int Ofset()
        {
            return 0x68;
        }
        public static int Shift()
        {
            return 3;
        }
    }
    public static class Jewels
    {
        public static int Ofset()
        {
            return 0x68;
        }
        public static int Shift()
        {
            return 20;
        }
    }
    #endregion

    #region Custom Objects
    public class cbItem
    {
        public string Text { get; set; }
        public int Value { get; set; }
    }

    public class monItem
    {
        public bool Caught { get; set; }
        public int Level { get; set; }
        public int Lollipops { get; set; }
        public int Exp { get; set; }
        public int Stone { get; set; }
        public int SpeedUpX { get; set; }
        public int SpeedUpY { get; set; }
        public int SkillLevel { get; set; }
        public int SkillExp { get; set; }
    }

    public class stgItem
    {
        public bool Completed { get; set; }
        public int Rank { get; set; }
        public int Score { get; set; }
    }

    public class rsItem
    {
        public int Hearts { get; set; }
        public int Coins { get; set; }
        public int Jewels { get; set; }
        public int[] Items { get; set; }
        public int[] Enhancements { get; set; }
    }
    #endregion
}
