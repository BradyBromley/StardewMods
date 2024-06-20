using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using StardewModdingAPI;

namespace JunimoKartPrizes
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            // Read or create config file
            //Config = Helper.ReadConfig<ModConfig>();

            // Harmony patching
            var harmony = new Harmony(ModManifest.UniqueID);
            //harmony.Patch(
            //   original: AccessTools.Method(typeof(StardewValley.GameLocation), nameof(StardewValley.GameLocation.answerDialogueAction)),
            //   prefix: new HarmonyMethod(typeof(Framework.SelectLevelPatch), nameof(Framework.SelectLevelPatch.AnswerDialogueActionPrefix))
            //);

            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Minigames.MineCart), nameof(StardewValley.Minigames.MineCart.ShowMap)),
               postfix: new HarmonyMethod(typeof(Framework.PrizePatch), nameof(Framework.PrizePatch.ShowMapPostfix))
            );

            // Setup events
            //helper.Events.GameLoop.GameLaunched += SetupConfig;

            //SelectLevelPatch.Initialize(Monitor, Config);
        }
    }
}
