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
            // Read or create config file
            Config = Helper.ReadConfig<ModConfig>();

            // Harmony patching
            var harmony = new Harmony(ModManifest.UniqueID);
            harmony.Patch(
               original: AccessTools.Method(typeof(StardewValley.Menus.LoadGameMenu.SaveFileSlot), nameof(StardewValley.Menus.LoadGameMenu.SaveFileSlot.Draw)),
               postfix: new HarmonyMethod(typeof(FarmIconPatch), nameof(FarmIconPatch.DrawFarmIconPostfix))
            );

            // Setup events
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

                // Add farm ID to farmer modData
                if (!modData.ContainsKey("FarmIconOnLoadScreen"))
                {
                    modData.Add("FarmIconOnLoadScreen", (Game1.whichFarm).ToString());
                }
                
                // Add mod farm texture name to farmer modData
                if ((Game1.whichFarm == 7) && (!modData.ContainsKey("ModFarmIconOnLoadScreen")))
                {
                    ModFarmType farmType = Game1.whichModFarm;
                    modData.Add("ModFarmIconOnLoadScreen", farmType.IconTexture);
                }

                // Add farm name to farmer modData
                if (!modData.ContainsKey("FarmIconNameOnLoadScreen"))
                {
                    string text = "";
                    switch (Game1.whichFarm)
                    {
                        case 0:
                            text = Game1.content.LoadString("Strings\\UI:Character_FarmStandard");
                            break;
                        case 1:
                            text = Game1.content.LoadString("Strings\\UI:Character_FarmFishing");
                            break;
                        case 2:
                            text = Game1.content.LoadString("Strings\\UI:Character_FarmForaging");
                            break;
                        case 3:
                            text = Game1.content.LoadString("Strings\\UI:Character_FarmMining");
                            break;
                        case 4:
                            text = Game1.content.LoadString("Strings\\UI:Character_FarmCombat");
                            break;
                        case 5:
                            text = Game1.content.LoadString("Strings\\UI:Character_FarmFourCorners");
                            break;
                        case 6:
                            text = Game1.content.LoadString("Strings\\UI:Character_FarmBeach");
                            break;
                        case 7:
                            ModFarmType farmType = Game1.whichModFarm;
                            text = Game1.content.LoadString(farmType.TooltipStringPath);
                            break;
                    }
                    string[] array = text.Split('_', 2);
                    modData.Add("FarmIconNameOnLoadScreen", array[0]);
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
            var configMenu = Helper.ModRegistry.GetApi<IGenericModConfigMenuApi>("spacechase0.GenericModConfigMenu");
            if (configMenu is null)
            {
                return;
            }

            // Register mod
            configMenu.Register(
                mod: ModManifest,
                reset: () => Config.SmallIcons = false,
                save: () => Helper.WriteConfig(Config)
            );

            // Add options
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
