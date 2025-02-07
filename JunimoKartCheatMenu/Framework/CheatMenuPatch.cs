using StardewModdingAPI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JunimoKartCheatMenu.Framework
{
    internal class CheatMenuPatch
    {
        private static IMonitor Monitor;
        private static ModConfig Config;

        internal static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }
    }
}
