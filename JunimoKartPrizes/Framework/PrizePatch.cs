using StardewModdingAPI;
using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunimoKartPrizes.Framework
{
    internal class PrizePatch
    {
        private static IMonitor Monitor;
        private static ModConfig Config;

        internal static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }

        internal static void ShowMapPostfix(int ___gameMode, int ___currentTheme)
        {
            try
            {
                Monitor.Log($"Currently in {nameof(ShowMapPostfix)}:", LogLevel.Info);

                // Don't unlock progress mode levels when playing endless mode
                //if (___gameMode == 2)
                //{
                //    return;
                //}

                //StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
                // Levels unlock when you reach them for the first time
                //if (!modData["JunimoKartUnlockedLevels"].Contains(___currentTheme.ToString()))
                //{
                //    modData["JunimoKartUnlockedLevels"] += ___currentTheme.ToString();
                //}
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(ShowMapPostfix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}
