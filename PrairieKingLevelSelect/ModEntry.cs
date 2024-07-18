using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PrairieKingLevelSelect.Framework;
using PrairieKingLevelSelect.Framework.GMCM;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.Events;
using StardewValley.Minigames;

namespace PrairieKingLevelSelect
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config;
        private LevelData LevelData = new LevelData();

        public override void Entry(IModHelper helper)
        {
            // Read or create config file
            Config = Helper.ReadConfig<ModConfig>();

            // Harmony patching
            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.GameLocation), nameof(StardewValley.GameLocation.showPrairieKingMenu)),
               prefix: new HarmonyMethod(typeof(Framework.SelectLevelPatch), nameof(Framework.SelectLevelPatch.ShowPrairieKingMenuPrefix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.GameLocation), nameof(StardewValley.GameLocation.answerDialogueAction)),
               prefix: new HarmonyMethod(typeof(Framework.SelectLevelPatch), nameof(Framework.SelectLevelPatch.AnswerDialogueActionPrefix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Minigames.AbigailGame), nameof(StardewValley.Minigames.AbigailGame.getMap)),
               postfix: new HarmonyMethod(typeof(Framework.SelectLevelPatch), nameof(Framework.SelectLevelPatch.GetMapPostfix))
            );

            // Setup events
            helper.Events.GameLoop.GameLaunched += SetupConfig;

            SelectLevelPatch.Initialize(Monitor, Config);
        }

        // Set up the GenericModConfigMenu
        private void SetupConfig(object? sender, EventArgs e)
        {
            // Get GenericModConfigMenu API
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                return;
            }

            // Register mod
            configMenu.Register(
                mod: ModManifest,
                reset: () => Config.UnlockLevels = false,
                save: () => Helper.WriteConfig(Config)
            );

            // Add options
            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => Config.UnlockLevels,
                setValue: value => Config.UnlockLevels = value,
                name: () => "Unlock All Levels",
                tooltip: () => "Unlock all levels when playing Journey of the Prairie King"
            );
        }
    }
}