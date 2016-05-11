using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Pokemon_Shuffle_Save_Editor
{
    public class Database
    {
        #region Variables
        byte[] monData;
        byte[] stagesMain;
        byte[] stagesEvent;
        byte[] stagesExpert;
        byte[] megaStone;
        byte[] monLevel;
        string[] speciesList;
        string[] monsList;

        int megaArray_start; // Indexes of first mega & second "---", respectively,...
        int monArray_stop; //...should allow PSSE to work longer without needing an update. 
        Tuple<int, int>[] megas; //monsIndex, speedups
        List<int> megaList;  //derivate a List from megas.Item1 to use with IndexOf() functions (in UpdateForms() & UpdateOwnedBox())
        Tuple<int, int, bool, int, int, int, int, Tuple<int>>[] mons;    //specieIndex, formIndex, isMega, raiseMaxLevel, basePower, talent, type, rest 
        Tuple<int>[] rest; //stageNum
        bool[][] hasMega;   // [X][0] = X, [X][1] = Y
        int[] forms;

        byte[] savedata;
        #endregion

        #region Properties
        public  byte[] MonData
        {
            get { return monData; }
            set { monData = value; }
        }
        public  byte[] StagesMain
        {
            get { return stagesMain; }
            set { stagesMain = value; }
        }
        public  byte[] StagesEvent
        {
            get { return stagesEvent; }
            set { stagesEvent = value; }
        }
        public  byte[] StagesExpert
        {
            get { return stagesExpert; }
            set { stagesExpert = value; }
        }
        public  byte[] MegaStone
        {
            get { return megaStone; }
            set { megaStone = value; }
        }
        public  byte[] MonLevel
        {
            get { return monLevel; }
            set { monLevel = value; }
        }
        public  string[] SpeciesList
        {
            get { return speciesList; }
        }
        public  string[] MonsList
        {
            get { return monsList; }
        }

        public  int MegaStartIndex
        {
            get { return megaArray_start; }
        }
        public  int MonStopIndex
        {
            get { return monArray_stop; }
        }
        public Tuple<int, int>[] Megas
        {
            get { return megas; }
        }
        public List<int> MegaList
        {
            get { return megaList; }
        }
        public Tuple<int, int, bool, int, int, int, int, Tuple<int>>[] Mons
        {
            get { return mons; }
        }
        public Tuple<int>[] Rest
        {
            get { return rest; }
        }
        public  bool[][] HasMega
        {
            get { return hasMega; }
        }
        public int[] Forms
        {
            get { return forms; }
        }

        public byte[] SaveData
        {
            get { return savedata; }
            set { savedata = value; }
        }
        #endregion
        
        public Database()
        {
            monData = Properties.Resources.pokemonData;
            stagesMain = Properties.Resources.stageData;
            stagesEvent = Properties.Resources.stageDataEvent;
            stagesExpert = Properties.Resources.stageDataExtra;
            megaStone = Properties.Resources.megaStone;
            monLevel = Properties.Resources.pokemonLevel;
            string resourcedir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "resources" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(resourcedir))
                Directory.CreateDirectory(resourcedir);
            byte[][] files = { MegaStone, MonData, StagesMain, StagesEvent, StagesExpert, MonLevel };
            string[] filenames = { "megaStone.bin", "pokemonData.bin", "stageData.bin", "stageDataEvent.bin", "stageDataExtra.bin", "pokemonLevel.bin" };
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
            speciesList = Properties.Resources.species.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            monsList = Properties.Resources.mons.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

            megaArray_start = monsList.ToList().IndexOf("Mega Venusaur");
            monArray_stop = monsList.ToList().IndexOf("---", 1);
            int entrylen = BitConverter.ToInt32(monData, 0x4);
            megas = new Tuple<int, int>[BitConverter.ToUInt32(megaStone, 0) - 1];
            for (int i = 0; i < megas.Length; i++)
            {
                int monIndex = BitConverter.ToUInt16(megaStone, 0x54 + i * 4) & 0x3FF;
                byte[] data = monData.Skip(0x50 + entrylen * (i + megaArray_start)).Take(entrylen).ToArray();
                int maxSpeedup = (BitConverter.ToInt32(data, 0xA) >> 7) & 0x7F;
                megas[i] = new Tuple<int, int>(monIndex, maxSpeedup);
            }
            megaList = new List<int>();
            for (int i = 0; i < megas.Length; i++)
                megaList.Add(megas[i].Item1);   
            forms = new int[speciesList.Length];
            hasMega = new bool[speciesList.Length][];
            for (int i = 0; i < speciesList.Length; i++)
                hasMega[i] = new bool[2];
            mons = new Tuple<int, int, bool, int, int, int, int, Tuple<int>>[BitConverter.ToUInt32(monData, 0)];
            rest = new Tuple<int>[BitConverter.ToUInt32(monData, 0)];
            for (int i = 0; i < mons.Length; i++)
            {
                byte[] data = monData.Skip(0x50 + entrylen * i).Take(entrylen).ToArray();
                bool isMega = i >= megaArray_start && i <= megaArray_start + megas.Length - 1;
                int spec = isMega
                    ? speciesList.ToList().IndexOf(monsList[megas[i - megaArray_start].Item1].Replace("Shiny", "").Replace("Winking", "").Replace("Smiling", "").Replace(" ", "")) //crappy but needed for IndexOf() to find the pokemon's name in specieslist (only adjectives on megas names matter)
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
                hasMega[mons[BitConverter.ToUInt16(megaStone, 0x54 + i * 4) & 0x3FF].Item1][(megaStone[0x54 + (i * 4) + 1] >> 3) & 1] = true;
            savedata = null;
        }
    }    
}
