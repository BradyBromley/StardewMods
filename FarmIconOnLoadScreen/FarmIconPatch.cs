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

                Single scale;
                float singleplayerPadX, singleplayerPadY, hostPadX, hostPadY;
                if (Config.SmallIcons)
                {
                    scale = 3f;
                    singleplayerPadX = 20;
                    singleplayerPadY = 90;
                    hostPadX = 1060;
                    hostPadY = 28;
                } else
                {
                    scale = 4f;
                    singleplayerPadX = 8;
                    singleplayerPadY = 78;
                    hostPadX = 1048;
                    hostPadY = 16;
                }

                // The location of the farm icon changes depending on if you are loading a singleplayer farm or hosting a farm
                Vector2 vector;
                if (__instance.GetType().Name == "SaveFileSlot")
                {
                    // Singleplayer
                    vector = new Vector2((TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + singleplayerPadX, (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + singleplayerPadY);
                } else
                {
                    // Multiplayer Host
                    vector = new Vector2((TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.X + hostPadX, (TitleMenu.subMenu as LoadGameMenu).slotButtons[i].bounds.Y + hostPadY);
                }

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
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(0, 324, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                break;
                            case 1:
                                // Riverland
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(22, 324, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                break;
                            case 2:
                                // Forest
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(44, 324, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                break;
                            case 3:
                                // Hills
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(66, 324, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                break;
                            case 4:
                                // Wilderness
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(88, 324, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                break;
                            case 5:
                                // Four Corners
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(0, 345, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                break;
                            case 6:
                                // Beach
                                b.Draw(Game1.mouseCursors, vector, new Rectangle(22, 345, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                break;
                            case 7:
                                if (modData["ModFarmIconOnLoadScreen"] != null)
                                {
                                    try
                                    {
                                        // Mod Farm
                                        Texture2D modFarmTexture = Game1.content.Load<Texture2D>(modData["ModFarmIconOnLoadScreen"]);
                                        b.Draw(modFarmTexture, vector, new Rectangle(0, 0, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                    }
                                    catch (Exception ex)
                                    {
                                        // If the mod farm doesn't load, draw the Standard farm with an X over it
                                        b.Draw(Game1.mouseCursors, vector, new Rectangle(0, 324, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                        b.Draw(Game1.mouseCursors, vector, new Rectangle(265, 468, 20, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                        Monitor.Log($"Something went wrong. Please ensure that custom farm mods are properly installed.", LogLevel.Error);
                                    }

                                } else
                                {
                                    // Meadowlands
                                    b.Draw(Game1.mouseCursors, vector, new Rectangle(1, 324, 22, 20), Color.White, 0f, Vector2.Zero, scale, SpriteEffects.None, 0.89f);
                                }
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(DrawFarmIconPostfix)}:\n{ex}", LogLevel.Error);
            }
            
        }
    }
}
