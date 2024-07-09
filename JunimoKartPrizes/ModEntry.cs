using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GenericModConfigMenu;
using HarmonyLib;
using JunimoKartPrizes.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using xTile.Dimensions;
using static StardewValley.Minigames.MineCart;

namespace JunimoKartPrizes
{
    internal sealed class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            // Read or create config file
            Config = Helper.ReadConfig<ModConfig>();

            // Harmony patching
            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.GameLocation), nameof(StardewValley.GameLocation.performAction), new Type[] { typeof(string[]), typeof(Farmer), typeof(Location) }),
               prefix: new HarmonyMethod(typeof(Framework.PrizePatch), nameof(Framework.PrizePatch.PerformActionPrefix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.GameLocation), nameof(StardewValley.GameLocation.answerDialogueAction)),
               postfix: new HarmonyMethod(typeof(Framework.PrizePatch), nameof(Framework.PrizePatch.AnswerDialogueActionPostfix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Minigames.MineCart), nameof(StardewValley.Minigames.MineCart.CollectCoin)),
               postfix: new HarmonyMethod(typeof(Framework.PrizePatch), nameof(Framework.PrizePatch.CollectCoinPostfix))
            );

            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Minigames.MineCart), nameof(StardewValley.Minigames.MineCart.CollectFruit)),
               postfix: new HarmonyMethod(typeof(Framework.PrizePatch), nameof(Framework.PrizePatch.CollectFruitPostfix))
            );

            // Setup events
            helper.Events.GameLoop.GameLaunched += SetupConfig;
            helper.Events.GameLoop.DayStarted += SetCoinCount;
            helper.Events.GameLoop.Saving += SaveCoinCount;


            PrizePatch.Initialize(Monitor, Config);
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
                reset: () =>
                {
                    Config.RareItem = 0.25f;
                    Config.EpicItem = 0.04f;
                    Config.LegendaryItem = 0.01f;
                },
                save: () => Helper.WriteConfig(Config)
            );

            // Add options
            configMenu.AddNumberOption(
                mod: ModManifest,
                getValue: () => Config.RareItem,
                setValue: value => Config.RareItem = value,
                name: () => "Rare Rarity",
                tooltip: () => "Set how often you will see rare prizes",
                min: 0.0f,
                max: 1.0f
            );

            configMenu.AddNumberOption(
                mod: ModManifest,
                getValue: () => Config.EpicItem,
                setValue: value => Config.EpicItem = value,
                name: () => "Epic Rarity",
                tooltip: () => "Set how often you will see epic prizes",
                min: 0.0f,
                max: 1.0f
            );

            configMenu.AddNumberOption(
                mod: ModManifest,
                getValue: () => Config.LegendaryItem,
                setValue: value => Config.LegendaryItem = value,
                name: () => "Legendary Rarity",
                tooltip: () => "Set how often you will see legendary prizes",
                min: 0.0f,
                max: 1.0f
            );
        }

        private void SetCoinCount(object? sender, EventArgs e)
        {
            PrizePatch.SetCoinCount();
        }

        private void SaveCoinCount(object? sender, EventArgs e)
        {
            PrizePatch.SaveCoinCount();
        }
    }
}
