using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Pokemon_Shuffle_Save_Editor
{
    public class Database
    {
        #region Properties

        public byte[] MegaStone { get; private set; }
        public byte[] MonAbility { get; private set; }
        public byte[] MonData { get; private set; }
        public byte[] MonLevel { get; private set; }
        public byte[] StagesEvent { get; private set; }
        public byte[] StagesExpert { get; private set; }
        public byte[] StagesMain { get; private set; }
        public byte[] MissionCard { get; private set; }

        public bool[][] HasMega { get; private set; }   // [X][0] = X, [X][1] = Y
        public int[] Forms { get; private set; }
        public int[][] PokathlonRand { get; private set; }  // [step][0] = min, [step][1] = max
        public bool[][] Missions { get; private set; }
        public string[] MonsList { get; private set; }
        public string[] PokathlonList { get; private set; }
        public string[] SpeciesList { get; private set; }
        public string[] SkillsList { get; private set; }
        public string[] SkillsTextList { get; private set; }
        public Tuple<int, int>[] Megas { get; private set; }    //monsIndex, speedups
        public Tuple<int, int, bool, int, int, int[], int, Tuple<int, int>>[] Mons { get; private set; }   //specieIndex, formIndex, isMega, raiseMaxLevel, basePower, skills, type, Rest
        public Tuple<int, int>[] Rest { get; private set; }  //stageNum, skillsCount
        public List<int> MegaList { get; private set; } //derivate a List from Megas.Item1 to use with IndexOf() functions (in UpdateForms() & UpdateOwnedBox())

        public int MegaStartIndex { get; private set; } // Indexes of first mega & second "---", respectively,...
        public int MonStopIndex { get; private set; }   //...should allow PSSE to work longer without needing an update.

        #endregion Properties

        public Database()
        {
            MonData = Properties.Resources.pokemonData;
            StagesMain = Properties.Resources.stageData;
            StagesEvent = Properties.Resources.stageDataEvent;
            StagesExpert = Properties.Resources.stageDataExtra;
            MegaStone = Properties.Resources.megaStone;
            MonLevel = Properties.Resources.pokemonLevel;
            MonAbility = Properties.Resources.pokemonAbility;
            MissionCard = Properties.Resources.missionCard;
            string resourcedir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + Path.DirectorySeparatorChar + "resources" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(resourcedir))
                Directory.CreateDirectory(resourcedir);
            byte[][] files = { MegaStone, MonData, StagesMain, StagesEvent, StagesExpert, MonLevel, MonAbility, MissionCard };
            string[] filenames = { "MegaStone.bin", "pokemonData.bin", "stageData.bin", "stageDataEvent.bin", "stageDataExtra.bin", "pokemonLevel.bin", "pokemonAbility.bin", "MissionCard.bin" };
            for (int i = 0; i < files.Length; i++)
            {
                if (!File.Exists(resourcedir + filenames[i]))
                    File.WriteAllBytes(resourcedir + filenames[i], files[i]);
                else
                {
                    files[i] = File.ReadAllBytes(resourcedir + filenames[i]);
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

                        case 6:
                            MonAbility = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                        case 7:
                            MissionCard = File.ReadAllBytes(resourcedir + filenames[i]);
                            break;
                    }
                }
            }
            SpeciesList = Properties.Resources.species.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            MonsList = Properties.Resources.mons.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);
            PokathlonList = Properties.Resources.pokathlon.Split(new[] { Environment.NewLine, "\n" }, StringSplitOptions.RemoveEmptyEntries);

            MegaStartIndex = MonsList.ToList().IndexOf("Mega Venusaur");
            MonStopIndex = MonsList.ToList().IndexOf("---", 1);
            int entrylen = BitConverter.ToInt32(MonData, 0x4);
            Megas = new Tuple<int, int>[BitConverter.ToUInt32(MegaStone, 0) - 1];
            for (int i = 0; i < Megas.Length; i++)
            {
                int monIndex = BitConverter.ToUInt16(MegaStone, 0x54 + i * 4) & 0x3FF;
                string str = "Mega " + MonsList[monIndex];
                if (monIndex == 6 || monIndex == 150)
                    str += (monIndex != (BitConverter.ToUInt16(MegaStone, 0x54 + (i - 1) * 4) & 0x3FF))
                        ? " X"
                        : " Y";
                byte[] data = MonData.Skip(0x50 + entrylen * MonsList.ToList().IndexOf(str)).Take(entrylen).ToArray();
                int maxSpeedup = (BitConverter.ToInt32(data, 0xA) >> 7) & 0x7F;
                Megas[i] = new Tuple<int, int>(monIndex, maxSpeedup);
            }
            MegaList = new List<int>();
            for (int i = 0; i < Megas.Length; i++)
                MegaList.Add(Megas[i].Item1);
            Forms = new int[SpeciesList.Length];
            HasMega = new bool[MonsList.Length][];
            for (int i = 0; i < MonsList.Length; i++)
                HasMega[i] = new bool[2];
            Mons = new Tuple<int, int, bool, int, int, int[], int, Tuple<int, int>>[BitConverter.ToUInt32(MonData, 0)];
            Rest = new Tuple<int, int>[Mons.Length];
            for (int i = 0; i < Mons.Length; i++)
            {
                byte[] data = MonData.Skip(0x50 + entrylen * i).Take(entrylen).ToArray();
                bool isMega = i >= MegaStartIndex && i <= MonsList.Count() - 1;
                int spec = (isMega && i <= MegaStartIndex + Megas.Length - 1)
                    ? SpeciesList.ToList().IndexOf(MonsList[Megas[i - MegaStartIndex].Item1].Replace("Shiny", "").Replace("Winking", "").Replace("Smiling", "").Replace(" ", "")) //crappy but needed for IndexOf() to find the pokemon's name in specieslist (only adjectives on Megas names matter)
                    : (BitConverter.ToInt32(data, 0xE) >> 6) & 0x7FF;
                int raiseMaxLevel = (BitConverter.ToInt16(data, 0x4)) & 0x3F;
                int basePower = (BitConverter.ToInt16(data, 0x3)) & 0x7; //ranges 1-7 for now (30-90 BP), may need an update later on
                int[] skill = new int[5];
                int j = 0;
                int skillCount = 0;
                foreach (int adr in new int[] { 0x02, 0x20, 0x21, 0x22, 0x23})
                {
                    skill[j] = data[adr] & 0x7F; //ranges 1-~100 for now ("Opportunist" to "Transform"), ordered list in MESSAGE_XX/message_PokedexXX.bin
                    skillCount += (skill[j] != 0 ? 1 : 0);
                    j++;
                }
                skillCount = Math.Max(skillCount, 1);
                int type = (BitConverter.ToInt16(data, 0x01) >> 3) & 0x1F; //ranges 0-17 (normal - fairy) (https://gbatemp.net/threads/psse-pokemon-shuffle-save-editor.396499/page-33#post-6278446)
                int index = (BitConverter.ToInt16(data, 0)) & 0x3FF; //ranges 1-999, it's the number you can see on the team selection menu
                Rest[i] = new Tuple<int, int>(index, skillCount); //Mons has more than 7 arguments so 8th one and beyond must be included in another Tuple
                Mons[i] = new Tuple<int, int, bool, int, int, int[], int, Tuple<int, int>>(spec, Forms[spec], isMega, raiseMaxLevel, basePower, skill, type, Rest[i]);
                Forms[spec]++;
            }
            for (int i = 0; i < Megas.Length; i++)
                HasMega[BitConverter.ToUInt16(MegaStone, 0x54 + i * 4) & 0x3FF][(MegaStone[0x54 + (i * 4) + 1] >> 3) & 1] = true;
            PokathlonRand = new int[PokathlonList.Length / 2][];
            for (int i = 0; i < PokathlonRand.Length; i++)
            {
                PokathlonRand[i] = new int[2];
                Int32.TryParse(PokathlonList[2 * i], out PokathlonRand[i][0]);
                Int32.TryParse(PokathlonList[1 + 2 * i], out PokathlonRand[i][1]);
            }
            Missions = new bool[BitConverter.ToInt32(MissionCard, 0)][];
            for(int i = 0 ; i < Missions.Length; i++)
            {
                Missions[i] = new bool[10];
                int ientrylen = BitConverter.ToInt32(MissionCard, 0x4);
                byte[] data = MissionCard.Skip(BitConverter.ToInt32(MissionCard, 0x10) + i * ientrylen).Take(ientrylen).ToArray();
                for (int j = 0; j < Missions[i].Length; j++)
                    Missions[i][j] = BitConverter.ToInt16(data, 0x8 + 2 * j) != 0;
            }
            byte[] HexValue = Properties.Resources.messagePokedex_US;
            string StrValue = "";
            List<string> List = new List<string>();
            for (int i = 0; i < HexValue.Length; i += 2)
            {
                if (BitConverter.ToChar(HexValue, i) == '\0' && StrValue != "" && !(StrValue.EndsWith("\u0001ă\u0001\u0003\u0003慮敭") || StrValue.EndsWith("\u0001ă\u0001\u0003\u0005敭慧慎敭")))
                {
                    List.Add(StrValue.Replace("\u0001ă\u0001\u0003\u0003慮敭\0", "[name]").Replace("\u0001ă\u0001\u0003\u0005敭慧慎敭\0", "[name]"));
                    StrValue = "";
                }
                else StrValue += BitConverter.ToChar(HexValue, i);
            }
            SkillsList = List.Skip(List.IndexOf("Opportunist")).Take(List.IndexOf("Attacks can occasionally deal\ngreater damage than usual.") - List.IndexOf("Opportunist")).ToArray();
            SkillsTextList = List.Skip(List.IndexOf("Attacks can occasionally deal\ngreater damage than usual.")).Take(List.IndexOf("Attacks can occasionally deal\ngreater damage than usual.") - List.IndexOf("Opportunist")).ToArray();
        }
    }
}