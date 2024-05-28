using System;
using HarmonyLib;
using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewModdingAPI.Utilities;
using StardewValley;
using StardewValley.GameData;

namespace FarmIconOnLoadScreen
{
    public class ModEntry : Mod
    {
        private ModConfig Config;

        public override void Entry(IModHelper helper)
        {
            // Read or Create config file
            Config = this.Helper.ReadConfig<ModConfig>();

            // Harmony patching
            var harmony = new Harmony(this.ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Menus.LoadGameMenu.SaveFileSlot), nameof(StardewValley.Menus.LoadGameMenu.SaveFileSlot.Draw)),
               postfix: new HarmonyMethod(typeof(FarmIconPatch), nameof(FarmIconPatch.DrawFarmIconPostfix))
            );

            // Setup Events
            helper.Events.GameLoop.SaveCreating += SaveFarmType;
            helper.Events.GameLoop.Saving += SaveFarmType;
            helper.Events.GameLoop.GameLaunched += SetupConfig;

            FarmIconPatch.Initialize(Monitor, Config);
        }

        // Save the farm type in farmer modData after creating a farmer so that it can be easily accessed later
        private void SaveFarmType(object? sender, EventArgs e)
        {
            try
            {
                StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
                if (!modData.ContainsKey("FarmIconOnLoadScreen"))
                {
                    modData.Add("FarmIconOnLoadScreen", (Game1.whichFarm).ToString());
                }
                
                if ((Game1.whichFarm == 7) && (!modData.ContainsKey("ModFarmIconOnLoadScreen")))
                {
                    ModFarmType farmType = Game1.whichModFarm;
                    modData.Add("ModFarmIconOnLoadScreen", farmType.IconTexture);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(SaveFarmType)}:\n{ex}", LogLevel.Error);
            }
        }

        // Set up the GenericModConfigMenu
        private void SetupConfig(object? sender, EventArgs e)
        {
            // Get GenericModConfigMenu API
            var configMenu = this.Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                return;
            }

            // Register Mod
            configMenu.Register(
                mod: ModManifest,
                reset: () =>
                {
                    Config = new ModConfig();
                    Helper.WriteConfig(Config);
                },
                save: () => Helper.WriteConfig(Config)
            );

            configMenu.AddBoolOption(
                mod: ModManifest,
                getValue: () => Config.SmallIcons,
                setValue: value => Config.SmallIcons = value,
                name: () => "Small Icons",
                tooltip: () => "Smaller farm icons on save files"
            );
        }
    }


}
