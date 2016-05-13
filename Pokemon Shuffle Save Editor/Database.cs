using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pokemon_Shuffle_Save_Editor
{
    public class Database
    {
        #region Properties
        public byte[] MonData { get; private set; }
        public byte[] StagesMain { get; private set; }
        public byte[] StagesEvent { get; private set; }
        public byte[] StagesExpert { get; private set; }
        public byte[] MegaStone { get; private set; }
        public byte[] MonLevel { get; private set; }

        public string[] SpeciesList { get; private set; }
        public string[] MonsList { get; private set; }
        public int MegaStartIndex { get; private set; } // Indexes of first mega & second "---", respectively,...
        public int MonStopIndex { get; private set; }   //...should allow PSSE to work longer without needing an update. 

        public Tuple<int, int>[] Megas { get; private set; }    //monsIndex, speedups
        public List<int> MegaList { get; private set; } //derivate a List from Megas.Item1 to use with IndexOf() functions (in UpdateForms() & UpdateOwnedBox())
        public int[] Forms { get; private set; }     
        public bool[][] HasMega { get; private set; }   // [X][0] = X, [X][1] = Y
        public Tuple<int, int, bool, int, int, int, int, Tuple<int>>[] Mons { get; private set; }   //specieIndex, formIndex, isMega, raiseMaxLevel, basePower, talent, type, Rest
        public Tuple<int>[] Rest { get; private set; }  //stageNum
        #endregion

        public Database()
        {
            MonData = Properties.Resources.pokemonData;
            StagesMain = Properties.Resources.stageData;
            StagesEvent = Properties.Resources.stageDataEvent;
            StagesExpert = Properties.Resources.stageDataExtra;
            MegaStone = Properties.Resources.megaStone;
            MonLevel = Properties.Resources.pokemonLevel;
            string resourcedir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "resources" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(resourcedir))
                Directory.CreateDirectory(resourcedir);
            byte[][] files = { MegaStone, MonData, StagesMain, StagesEvent, StagesExpert, MonLevel };
            string[] filenames = { "MegaStone.bin", "pokemonData.bin", "stageData.bin", "stageDataEvent.bin", "stageDataExtra.bin", "pokemonLevel.bin" };
            for (int i = 0; i < files.Length; i++)
            {
                if (!File.Exists(resourcedir + filenames[i]))
                    File.WriteAllBytes(resourcedir + filenames[i], files[i]);
                else
                {
                    switch (i)
                    {
                        case 0:
                            MegaStone = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 1:
                            MonData = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 2:
                            StagesMain = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 3:
                            StagesEvent = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 4:
                            StagesExpert = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 5:
                            MonLevel = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                    }
                }
            }
            SpeciesList = Properties.Resources.species.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            MonsList = Properties.Resources.mons.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

            MegaStartIndex = MonsList.ToList().IndexOf("Mega Venusaur");
            MonStopIndex = MonsList.ToList().IndexOf("---", 1);
            int entrylen = BitConverter.ToInt32(MonData, 0x4);
            Megas = new Tuple<int, int>[BitConverter.ToUInt32(MegaStone, 0) - 1];
            for (int i = 0; i < Megas.Length; i++)
            {
                int monIndex = BitConverter.ToUInt16(MegaStone, 0x54 + i * 4) & 0x3FF;
                byte[] data = MonData.Skip(0x50 + entrylen * (i + MegaStartIndex)).Take(entrylen).ToArray();
                int maxSpeedup = (BitConverter.ToInt32(data, 0xA) >> 7) & 0x7F;
                Megas[i] = new Tuple<int, int>(monIndex, maxSpeedup);
            }
            MegaList = new List<int>();
            for (int i = 0; i < Megas.Length; i++)
                MegaList.Add(Megas[i].Item1);   
            Forms = new int[SpeciesList.Length];
            HasMega = new bool[SpeciesList.Length][];
            for (int i = 0; i < SpeciesList.Length; i++)
                HasMega[i] = new bool[2];
            Mons = new Tuple<int, int, bool, int, int, int, int, Tuple<int>>[BitConverter.ToUInt32(MonData, 0)];
            Rest = new Tuple<int>[BitConverter.ToUInt32(MonData, 0)];
            for (int i = 0; i < Mons.Length; i++)
            {
                byte[] data = MonData.Skip(0x50 + entrylen * i).Take(entrylen).ToArray();
                bool isMega = i >= MegaStartIndex && i <= MegaStartIndex + Megas.Length - 1;
                int spec = isMega
                    ? SpeciesList.ToList().IndexOf(MonsList[Megas[i - MegaStartIndex].Item1].Replace("Shiny", "").Replace("Winking", "").Replace("Smiling", "").Replace(" ", "")) //crappy but needed for IndexOf() to find the pokemon's name in specieslist (only adjectives on Megas names matter)
                    : (BitConverter.ToInt32(data, 0xE) >> 6) & 0x7FF;
                int raiseMaxLevel = (BitConverter.ToInt16(data, 0x4)) & 0x3F;
                int basePower = (BitConverter.ToInt16(data, 0x3)) & 0x7; //ranges 1-7 for now (30-90 BP), may need an update later on
                int talent = (BitConverter.ToInt16(data, 0x02)) & 0x7F; //ranges 1-~100 for now ("Opportunist" to "Transform"), ordered list in MESSAGE_XX/09 (0x44C-C76 for US)
                int type = (BitConverter.ToInt16(data, 0x01) >> 3) & 0x1F; //ranges 0-17 (normal - fairy) (https://gbatemp.net/threads/psse-pokemon-shuffle-save-editor.396499/page-33#post-6278446)
                int index = (BitConverter.ToInt16(data, 0)) & 0x3FF; //ranges 1-999, it's the number you can see on the team selection menu
                Rest[i] = new Tuple<int>(index); //Mons has more than 7 arguments so 8th one and beyond must be included in another Tuple
                Mons[i] = new Tuple<int, int, bool, int, int, int, int, Tuple<int>>(spec, Forms[spec], isMega, raiseMaxLevel, basePower, talent, type, Rest[i]);
                Forms[spec]++;
            }
            for (int i = 0; i < Megas.Length; i++)
                HasMega[Mons[BitConverter.ToUInt16(MegaStone, 0x54 + i * 4) & 0x3FF].Item1][(MegaStone[0x54 + (i * 4) + 1] >> 3) & 1] = true;
        }        
    }    
}
