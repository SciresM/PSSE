using System.ComponentModel;

namespace Pokemon_Shuffle_Save_Editor
{
    internal class ShuffleItems
    {
        private int[] items = new int[7];
        private int[] enchantments = new int[9];

        // Items -- things you use for a stage
        [Browsable(false)]
        public int[] Items
        {
            get { return items; }
            set { items = value; }
        }

        // Enchantments -- permanent Pokémon Enhancements for Pokémon
        [Browsable(false)]
        public int[] Enchantments
        {
            get { return enchantments; }
            set { enchantments = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Items")]
        [DisplayName("Moves +5")]
        [Description("Increases the moves left by 5 moves, but does not affect ability to catch Pokémon.")]
        public int Moves
        {
            get { return items[0]; }
            set { items[0] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Items")]
        [DisplayName("Time +10")]
        [Description("Increases the time left by 10 seconds, but does not affect ability to catch Pokémon.")]
        public int Time
        {
            get { return items[1]; }
            set { items[1] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Items")]
        [DisplayName("Exp. Points ×1.5")]
        [Description("Increases the Exp. Points earned at the end of a stage by 50%.")]
        public int Experience
        {
            get { return items[2]; }
            set { items[2] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Items")]
        [DisplayName("Mega Start")]
        [Description("Your Pokémon in the first slot Mega Evolves as a stage begins.")]
        public int MegaStart
        {
            get { return items[3]; }
            set { items[3] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Items")]
        [DisplayName("Complexity -1")]
        [Description("One less kind of Pokémon, rock, or block will appear.")]
        public int Complexity
        {
            get { return items[4]; }
            set { items[4] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Items")]
        [DisplayName("Disruption Delay")]
        [Description("Delays your opponent's Disruptions.")]
        public int Disruption
        {
            get { return items[5]; }
            set { items[5] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Items")]
        [DisplayName("Attack Power ↑")]
        [Description("Attack power gets doubled.")]
        public int AttackUp
        {
            get { return items[6]; }
            set { items[6] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Mega Speedup")]
        [Description("Use it on a Mega-Evolving Pokémon, and it'll Mega Evolve a little sooner!")]
        public int MegaSpeedup
        {
            get { return enchantments[0]; }
            set { enchantments[0] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Raise Max Level")]
        [Description("This item raises your Pokémon's maximum level by one.")]
        public int RaiseMaxLevel
        {
            get { return enchantments[1]; }
            set { enchantments[1] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Level Up")]
        [Description("Raises a Pokémon's current level by one.")]
        public int LevelUp
        {
            get { return enchantments[2]; }
            set { enchantments[2] = value; }
        }

        /*
         * I know you're going to ask why there are '\t' characters in the names -- this is so the items will be sorted
         * the way I want (S, M, L) rather than alphabetically (L, M, S) They're included when sorting but aren't rendered.
         * Yay for ugly hacks!
         */

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Exp\t Booster S")]
        [Description("Increases your Pokémon's Exp. Points by a small amount. (+50)")]
        public int ExperienceBoostS
        {
            get { return enchantments[3]; }
            set { enchantments[3] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Exp\t\t Booster M")]
        [Description("Increases your Pokémon's Exp. Points by a moderate amount. (+200)")]
        public int ExperienceBoostM
        {
            get { return enchantments[4]; }
            set { enchantments[4] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Exp\t\t\t Booster L")]
        [Description("Increases your Pokémon's Exp. Points by a large amount. (+1.000)")]
        public int ExperienceBoostL
        {
            get { return enchantments[5]; }
            set { enchantments[5] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Skill\t Booster S")]
        [Description("This item will slightly fill your Pokémon's Skill Gauge. (+3)")]
        public int SkillBoosterS
        {
            get { return enchantments[6]; }
            set { enchantments[6] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Skill\t\t Booster M")]
        [Description("This item will moderately fill your Pokémon's Skill Gauge. (+10)")]
        public int SkillBoosterM
        {
            get { return enchantments[7]; }
            set { enchantments[7] = value; }
        }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Pokémon Enhancements")]
        [DisplayName("Skill\t\t\t Booster L")]
        [Description("This item will significantly fill your Pokémon's Skill Gauge. (+30)")]
        public int SkillBoosterL
        {
            get { return enchantments[8]; }
            set { enchantments[8] = value; }
        }
    }
}