using System;
using HarmonyLib;
using JunimoKartCheatMenu.Framework;
using JunimoKartCheatMenu.Framework.GMCM;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;

namespace JunimoKartCheatMenu
{
    public class ModEntry : Mod
    {
        private ModConfig Config;
        public override void Entry(IModHelper helper)
        {
            // Read or create config file
            Config = Helper.ReadConfig<ModConfig>();

            // Harmony patching
            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Minigames.MineCart), nameof(StardewValley.Minigames.MineCart.Die)),
               prefix: new HarmonyMethod(typeof(Framework.CheatMenuPatch), nameof(Framework.CheatMenuPatch.DiePrefix))
            );
            
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Minigames.MineCart.MineCartCharacter), nameof(StardewValley.Minigames.MineCart.MineCartCharacter.GetMaxFallSpeed)),
               postfix: new HarmonyMethod(typeof(Framework.CheatMenuPatch), nameof(Framework.CheatMenuPatch.GetMaxFallSpeedPostfix))
            );

            // Setup events
            helper.Events.GameLoop.GameLaunched += SetupConfig;

            CheatMenuPatch.Initialize(Monitor, Config);
        }

        // Set up the GenericModConfigMenu
        private void SetupConfig (object? sender, EventArgs e)
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
                    Config.InfiniteLives = false;
                    Config.Invincibility = false;
                    Config.JumpGravity = "Default";
                    Config.FallGravity = "Default";
                },
                save: () => Helper.WriteConfig(Config)
            );

            // Add options
            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => Config.InfiniteLives,
                setValue: value => Config.InfiniteLives = value,
                name: () => "Infinite Lives",
                tooltip: () => "Set lives to infinite in Junimo Kart Progress Mode."
            );

            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => Config.Invincibility,
                setValue: value => Config.Invincibility = value,
                name: () => "Invincibility",
                tooltip: () => "Makes junimo invincible when playing Junimo Kart."
            );

            configMenu.AddTextOption(
                mod: ModManifest,
                getValue: () => Config.JumpGravity,
                setValue: value => Config.JumpGravity = value,
                name: () => "Jump Height",
                tooltip: () => "Set jump height in Junimo Kart.",
                allowedValues: new string[] { "Very Small", "Small", "Default", "Large", "Very Large" }
            );

            configMenu.AddTextOption(
                mod: ModManifest,
                getValue: () => Config.FallGravity,
                setValue: value => Config.FallGravity = value,
                name: () => "Gravity",
                tooltip: () => "Set the gravity in Junimo Kart.",
                allowedValues: new string[] { "Very Low", "Low", "Default", "High", "Very High" }
            );
        }
    }


}
