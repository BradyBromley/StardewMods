using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.GameData;
using System.Runtime.CompilerServices;

namespace FarmIconOnLoadScreen
{
    internal class FarmIconPatch
    {

        private static IMonitor Monitor;
        private static ModConfig Config;

        internal static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }

        // Draw the farm type on the load screen
        internal static void DrawFarmIconPostfix(LoadGameMenu.SaveFileSlot __instance, SpriteBatch b, int i)
        {
            try
            {
                // If you are joining a multiplayer game, don't display a farm icon
                if (__instance.Farmer.slotName == null)
                {
                    return;
                }

                // Setup the icon bounds, which are different for small and large icons
                Single scale;
                int width, height;
                float singleplayerX, singleplayerY, hostX, hostY;
                if (Config.SmallIcons)
                {
                    scale = 3f;
                    singleplayerX = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + 20;
                    singleplayerY = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + 90;
                    hostX = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + 1060;
                    hostY = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + 28;
                } else
                {
                    scale = 4f;
                    singleplayerX = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + 8;
                    singleplayerY = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + 78;
                    hostX = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + 1048;
                    hostY = (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + 16;
                }
                width = 22 * (int)scale;
                height = 20 * (int)scale;

                // The location of the farm icon changes depending on if you are loading a singleplayer farm or hosting a farm
                Rectangle bounds;
                if (__instance.GetType().Name == "SaveFileSlot")
                {
                    // Singleplayer
                    bounds = new Rectangle((int)singleplayerX, (int)singleplayerY, width, height);
                } else
                {
                    // Multiplayer Host
                    bounds = new Rectangle((int)hostX, (int)hostY, width, height);
                }

                string hoverText = "";
                StardewValley.Mods.ModDataDictionary modData = __instance.Farmer.modData;
                if (modData.ContainsKey("FarmIconOnLoadScreen"))
                {
                    bool success = int.TryParse(modData["FarmIconOnLoadScreen"], out int whichFarm);
                    if (success)
                    {
                        switch (whichFarm)
                        {
                            case 0:
                                // Standard
                                b.Draw(Game1.mouseCursors, bounds, new Rectangle(0, 324, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                break;
                            case 1:
                                // Riverland
                                b.Draw(Game1.mouseCursors, bounds, new Rectangle(22, 324, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                break;
                            case 2:
                                // Forest
                                b.Draw(Game1.mouseCursors, bounds, new Rectangle(44, 324, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                break;
                            case 3:
                                // Hills
                                b.Draw(Game1.mouseCursors, bounds, new Rectangle(66, 324, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                break;
                            case 4:
                                // Wilderness
                                b.Draw(Game1.mouseCursors, bounds, new Rectangle(88, 324, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                break;
                            case 5:
                                // Four Corners
                                b.Draw(Game1.mouseCursors, bounds, new Rectangle(0, 345, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                break;
                            case 6:
                                // Beach
                                b.Draw(Game1.mouseCursors, bounds, new Rectangle(22, 345, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                break;
                            case 7:
                                if (modData["ModFarmIconOnLoadScreen"] != null)
                                {
                                    try
                                    {
                                        // Meadowlands and Mod Farms
                                        Texture2D modFarmTexture = Game1.content.Load<Texture2D>(modData["ModFarmIconOnLoadScreen"]);
                                        b.Draw(modFarmTexture, bounds, new Rectangle(0, 0, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                    }
                                    catch (Exception ex)
                                    {
                                        // If the mod farm doesn't load, draw the Standard farm with an X over it
                                        b.Draw(Game1.mouseCursors, bounds, new Rectangle(0, 324, 22, 20), Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0.89f);
                                        b.Draw(Game1.mouseCursors, new Vector2(bounds.X, bounds.Y), new Rectangle(265, 468, 20, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                        Monitor.Log($"Something went wrong. Please ensure that custom farm mods are properly installed.", LogLevel.Error);
                                    }
                                }
                                break;
                        }

                        if (modData.ContainsKey("FarmIconNameOnLoadScreen") && (modData["FarmIconNameOnLoadScreen"] != null))
                        {
                            hoverText = modData["FarmIconNameOnLoadScreen"];
                        }
                        else
                        {
                            hoverText = "Farm name not found";
                        }
                    }
                }

                // Draw the hoverText for the farm icons
                if (bounds.Contains(Game1.input.GetMouseState().Position))
                {
                    IClickableMenu.drawHoverText(b, hoverText, Game1.smallFont, 0, -100);
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DrawFarmIconPostfix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}
