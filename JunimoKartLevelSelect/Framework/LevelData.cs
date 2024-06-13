using StardewValley;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.MineCart;

namespace JunimoKartLevelSelect.Framework
{
    public class LevelData
    {
        public static Dictionary<string, LevelTransition> levelDictionary = new Dictionary<string, LevelTransition>();

        public LevelData()
        {
            levelDictionary.Add("Crumble Cavern", new LevelTransition(-1, 0, 2, 5, "rrr"));
            levelDictionary.Add("Slippery Slopes", new LevelTransition(-1, 1, 5, 5, "rddlddrdd"));
            levelDictionary.Add("???", new LevelTransition(-1, 8, 5, 5, "rddrrd"));
            levelDictionary.Add("The Gem Sea Giant", new LevelTransition(-1, 2, 6, 11, "rrurrrrddr"));
            levelDictionary.Add("Slomp's Stomp", new LevelTransition(-1, 5, 6, 11, "rrurruuu"));
            levelDictionary.Add("Ghastly Galleon", new LevelTransition(-1, 3, 13, 12, "rurruuu"));
            levelDictionary.Add("Glowshroom Grotto", new LevelTransition(-1, 9, 16, 8, "rruuluu"));
            levelDictionary.Add("Red Hot Rollercoaster", new LevelTransition(-1, 4, 16, 8, "rrddrddr"));
            levelDictionary.Add("Sunset Speedway", new LevelTransition(-1, 6, 17, 4, "rrdrrru"));
        }
    }
}
