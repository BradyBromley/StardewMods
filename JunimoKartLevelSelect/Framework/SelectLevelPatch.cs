using StardewValley;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StardewValley.Minigames;
using static StardewValley.Minigames.MineCart;
using StardewModdingAPI;

namespace JunimoKartLevelSelect.Framework
{
    internal class SelectLevelPatch
    {
        private static IMonitor Monitor;
        private static ModConfig Config;

        internal static void Initialize(IMonitor monitor, ModConfig config)
        {
            Monitor = monitor;
            Config = config;
        }

        // Open up a dialogue box to choose a level when clicking on the Junimo Kart arcade machine
        internal static bool AnswerDialogueActionPrefix(string questionAndAnswer)
        {
            try
            {
                // Run original logic
                if (questionAndAnswer != "MinecartGame_Progress")
                {
                    return true;
                }

                Response[] responses;

                // Setup responses
                if (Config.UnlockLevels)
                {
                    responses = new Response[LevelData.levelDictionary.Count + 1];
                    int count = 0;
                    foreach (var (levelName, levelTransition) in LevelData.levelDictionary)
                    {
                        responses[count] = new Response(levelName, levelName);
                        count++;
                    }
                    responses[count] = new Response("Exit", "Exit");
                } else
                {
                    // Setup the unlocked levels modData
                    StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
                    if (!modData.ContainsKey("JunimoKartUnlockedLevels"))
                    {
                        modData.Add("JunimoKartUnlockedLevels", "0");
                    }
                    string unlockedLevels = modData["JunimoKartUnlockedLevels"];

                    responses = new Response[unlockedLevels.Length + 1];
                    int count = 0;
                    foreach (var (levelName, levelTransition) in LevelData.levelDictionary)
                    {
                        if (unlockedLevels.Contains(levelTransition.destinationLevel.ToString()))
                        {
                            responses[count] = new Response(levelName, levelName);
                            count++;
                        }
                    }
                    responses[count] = new Response("Exit", "Exit");
                }
                GameLocation.afterQuestionBehavior levelSelected = new GameLocation.afterQuestionBehavior(QuestionResponse);
                Game1.player.currentLocation.createQuestionDialogue("Level Select", responses, levelSelected);

                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(AnswerDialogueActionPrefix)}:\n{ex}", LogLevel.Error);
                return false;
            }
        }

        private static void QuestionResponse(Farmer player, string responseKey)
        {
            switch (responseKey)
            {
                case "Crumble Cavern":
                case "Slippery Slopes":
                case "???":
                case "The Gem Sea Giant":
                case "Slomp's Stomp":
                case "Ghastly Galleon":
                case "Glowshroom Grotto":
                case "Red Hot Rollercoaster":
                case "Sunset Speedway":
                    MineCart JunimoKart = new MineCart(0, 3);
                    LevelTransition[] levelTransitions = new LevelTransition[16];
                    levelTransitions[0] = LevelData.levelDictionary[responseKey];

                    for (int i = 0; i < JunimoKart.LEVEL_TRANSITIONS.Length; i++)
                    {
                        levelTransitions[i + 1] = JunimoKart.LEVEL_TRANSITIONS[i];
                    }
                    JunimoKart.LEVEL_TRANSITIONS = levelTransitions;
                    Game1.currentMinigame = JunimoKart;
                    break;
                default:
                    Game1.player.currentLocation.answerDialogueAction(responseKey, null);
                    break;
            }
            
        }

        internal static void ShowMapPostfix(int ___gameMode, int ___currentTheme)
        {
            try
            {
                // Don't unlock progress mode levels when playing endless mode
                if (___gameMode == 2)
                {
                    return;
                }

                StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
                // Levels unlock when you reach them for the first time
                if (!modData["JunimoKartUnlockedLevels"].Contains(___currentTheme.ToString()))
                {
                    modData["JunimoKartUnlockedLevels"] += ___currentTheme.ToString();
                }
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(ShowMapPostfix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}
