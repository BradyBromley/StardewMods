using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewModdingAPI.Events;
using StardewValley;
using StardewValley.Menus;
using StardewValley.Minigames;
using StardewValley.Tools;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using xTile.Dimensions;

namespace JunimoKartPrizes.Framework
{
    internal class PrizePatch
    {
        private static IMonitor Monitor;
        private static ModConfig Config;
        private static int coinCount;


        internal static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }


        // Set the coin count based on modData
        internal static void SetCoinCount()
        {
            StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
            if (!modData.ContainsKey("JunimoKartCoinCount"))
            {
                modData.Add("JunimoKartCoinCount", "0");
            }
            coinCount = int.Parse(modData["JunimoKartCoinCount"]);
        }


        // Save the coin count into modData
        internal static void SaveCoinCount()
        {
            StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
            modData["JunimoKartCoinCount"] = coinCount.ToString();
        }


        // Open up a dialogue box to choose whether to play Junimo Kart or redeem prizes
        internal static bool PerformActionPrefix(GameLocation __instance, ref bool __result, string[] action, Farmer who, Location tileLocation, Dictionary<string, Func<GameLocation, string[], Farmer, Point, bool>> ___registeredTileActions)
        {
            try
            {
                // Run original logic
                if (__instance.ShouldIgnoreAction(action, who, tileLocation) || !ArgUtility.TryGet(action, 0, out var value, out var error))
                {
                    return true;
                }

                if (who.IsLocalPlayer)
                {
                    if (___registeredTileActions.TryGetValue(value, out var value2) || value != "Arcade_Minecart")
                    {
                        return true;
                    }

                    if (who.hasSkullKey)
                    {
                        // Dialogue option to either play Junimo Kart or redeem prizes
                        Response[] responses = new Response[3]
                        {
                            new Response("Play", "Play Junimo Kart"),
                            new Response("Prizes", "Redeem Prizes"),
                            new Response("Exit", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Exit"))
                        };
                        GameLocation.afterQuestionBehavior junimoKartDialogue = new GameLocation.afterQuestionBehavior(JunimoKartQuestionResponse);
                        Game1.player.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Menu"), responses, junimoKartDialogue);
                    }
                    else
                    {
                        Game1.drawObjectDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Inactive"));
                    }
                    __result = true;
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(PerformActionPrefix)}:\n{ex}", LogLevel.Error);
                return false;
            }
        }


        // Open up different dialogue boxes depending on if Play or Prizes was chosen previously
        private static void JunimoKartQuestionResponse(Farmer player, string responseKey)
        {
            switch (responseKey)
            {
                case "Play":
                    Response[] answerChoices2 = new Response[3]
                        {
                        new Response("Progress", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_ProgressMode")),
                        new Response("Endless", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_EndlessMode")),
                        new Response("Exit", Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Exit"))
                        };
                    player.currentLocation.createQuestionDialogue(Game1.content.LoadString("Strings\\Locations:Saloon_Arcade_Minecart_Menu"), answerChoices2, "MinecartGame");
                    break;
                case "Prizes":
                    Response[] responses = new Response[3]
                    {
                        new Response("1Prize", "1 Prize (50 coins)"),
                        new Response("10Prize", "10 Prizes (500 coins)"),
                        new Response("Exit", "Exit")
                    };
                    Game1.player.currentLocation.createQuestionDialogue($"{coinCount} Available Coins", responses, "JunimoKartPrizes");
                    break;
                default:
                    Game1.player.currentLocation.answerDialogueAction(responseKey, null);
                    break;
            }

        }
        
        
        // Open up a dialogue box to redeem prizes at the Junimo Kart arcade machine
        internal static void AnswerDialogueActionPostfix(string questionAndAnswer)
        {
            try
            {
                if (questionAndAnswer == "JunimoKartPrizes_1Prize")
                {
                    if (coinCount >= 50)
                    {
                        coinCount -= 50;
                        GetPrizes(1);
                    }
                    else
                    {
                        Game1.addHUDMessage(new HUDMessage("You do not have enough coins.", 3));
                    }
                }
                else if (questionAndAnswer == "JunimoKartPrizes_10Prize")
                {
                    if (coinCount >= 500)
                    {
                        coinCount -= 500;
                        GetPrizes(10);
                    }
                    else
                    {
                        Game1.addHUDMessage(new HUDMessage("You do not have enough coins.", 3));
                    }
                }

            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(AnswerDialogueActionPostfix)}:\n{ex}", LogLevel.Error);
            }
            
        }
        

        // Generate the prizes randomly based on rarity
        private static void GetPrizes(int numPrizes)
        {
            List<Item> list = new List<Item>();
            Prize prize;
            double randomNumber;

            for (int i = 0; i < numPrizes; i++)
            {
                randomNumber = Game1.random.NextDouble();
                if (randomNumber <= Config.LegendaryItem)
                {
                    // Legendary Item
                    prize = PrizeData.legendaryItems[Game1.random.Next(PrizeData.legendaryItems.Length)];
                    if (prize.itemID.StartsWith("(W)"))
                    {
                        Item item = MeleeWeapon.attemptAddRandomInnateEnchantment(ItemRegistry.Create(prize.itemID), Game1.random);
                        item.specialItem = true;
                        list.Add(item);
                    }
                    else
                    {
                        list.Add(ItemRegistry.Create(prize.itemID, prize.itemQuantity));
                    }
                }
                else if (randomNumber <= Config.LegendaryItem + Config.EpicItem)
                {
                    // Epic Item
                    prize = PrizeData.epicItems[Game1.random.Next(PrizeData.epicItems.Length)];
                    list.Add(ItemRegistry.Create(prize.itemID, prize.itemQuantity));
                }
                else if (randomNumber <= Config.LegendaryItem + Config.EpicItem + Config.RareItem)
                {
                    // Rare Item
                    prize = PrizeData.rareItems[Game1.random.Next(PrizeData.rareItems.Length)];
                    list.Add(ItemRegistry.Create(prize.itemID, prize.itemQuantity));
                }
                else
                {
                    // Common Item
                    prize = PrizeData.commonItems[Game1.random.Next(PrizeData.commonItems.Length)];
                    list.Add(ItemRegistry.Create(prize.itemID, prize.itemQuantity));
                }
            }

            ItemGrabMenu itemGrabMenu = new ItemGrabMenu(list).setEssential(essential: true);
            itemGrabMenu.source = 3;
            Game1.activeClickableMenu = itemGrabMenu;
        }

        
        // Each coin collected in Junimo Kart counts towards redeeming prizes
        internal static void CollectCoinPostfix(int amount)
        {
            try
            {
                // only add to the coin count when coins are collected, not when fruit is collected
                if (amount == 1)
                {
                    coinCount++;
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(CollectCoinPostfix)}:\n{ex}", LogLevel.Error);
            }
        }


        // Each fruit collected in Junimo Kart counts towards redeeming prizes
        internal static void CollectFruitPostfix()
        {
            try
            {
                coinCount += 10;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(CollectCoinPostfix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}
