using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PrairieKingLevelSelect.Framework
{
    public class LevelData
    {
        public static Dictionary<string, string> levelDictionary = new Dictionary<string, string>();

        public LevelData()
        {
            // ammoLevel-bulletDamage-coins-died-fireSpeedLevel-lives-score-runSpeedLevel-spreadPistol-whichRound-whichWave-waveTimer-world
            levelDictionary.Add("1-1", "0:1:0:F:0:3:0:0:F:0:0:80000:0");
            levelDictionary.Add("1-2", "0:1:8:F:0:3:0:0:F:0:1:80000:0");
            levelDictionary.Add("1-3", "1:2:0:F:0:3:0:0:F:0:2:80000:0");
            levelDictionary.Add("1-4", "1:2:10:F:0:3:0:0:F:0:3:80000:0");
            levelDictionary.Add("1-B", "1:2:0:F:1:3:0:1:F:0:4:80000:0");
            levelDictionary.Add("2-1", "1:2:0:F:1:3:0:1:F:0:5:80000:2");
            levelDictionary.Add("2-2", "1:2:0:F:2:3:0:1:F:0:6:80000:2");
            levelDictionary.Add("2-3", "1:2:12:F:2:3:0:1:F:0:7:80000:2");
            levelDictionary.Add("2-B", "2:3:0:F:2:3:0:1:F:0:8:80000:2");
            levelDictionary.Add("3-1", "2:3:0:F:2:3:0:1:F:0:9:80000:1");
            levelDictionary.Add("3-2", "2:3:0:F:2:3:0:2:F:0:10:80000:1");
            levelDictionary.Add("3-3", "2:3:15:F:2:3:0:2:F:0:11:80000:1");
            levelDictionary.Add("3-B", "2:3:0:F:3:3:0:2:F:0:12:80000:1");
        }
    }
}
