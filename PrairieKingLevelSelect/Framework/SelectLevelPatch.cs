using Microsoft.Xna.Framework;
using StardewModdingAPI;
using StardewValley;
using StardewValley.Minigames;
using StardewValley.Network;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static StardewValley.Minigames.AbigailGame;
using static StardewValley.Minigames.MineCart;

namespace PrairieKingLevelSelect.Framework
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

        internal static bool ShowPrairieKingMenuPrefix()
        {
            try
            {
                // Run original logic
                if (Game1.player.jotpkProgress.Value != null)
                {
                    return true;
                }
                PlayPrairieKing();
                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(ShowPrairieKingMenuPrefix)}:\n{ex}", LogLevel.Error);
                return false;
            }
        }

        internal static bool AnswerDialogueActionPrefix(string questionAndAnswer)
        {
            try
            {
                // Run original logic
                if (questionAndAnswer != "CowboyGame_NewGame")
                {
                    return true;
                }
                PlayPrairieKing();
                return false;
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(ShowPrairieKingMenuPrefix)}:\n{ex}", LogLevel.Error);
                return false;
            }
        }


        // Open up a dialogue box to choose a level when clicking on the Prairie King arcade machine
        private static void PlayPrairieKing()
        {
            try
            {
                // Setup the unlocked levels modData
                StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
                if (!modData.ContainsKey("PrairieKingUnlockedLevels"))
                {
                    modData.Add("PrairieKingUnlockedLevels", "1-1");
                }

                // Setup responses
                Response[] responses;
                if (Config.UnlockLevels)
                {
                    responses = new Response[LevelData.levelDictionary.Count + 1];
                    int count = 0;
                    foreach (var (levelName, data) in LevelData.levelDictionary)
                    {
                        responses[count] = new Response(levelName, levelName);
                        count++;
                    }
                    responses[count] = new Response("Exit", "Exit");
                }
                else
                {
                    // Setup responses
                    string[] unlockedLevels = modData["PrairieKingUnlockedLevels"].Split(":");
                    responses = new Response[unlockedLevels.Length + 1];
                    int count = 0;
                    foreach (var (levelName, data) in LevelData.levelDictionary)
                    {
                        if (unlockedLevels.Contains<string>(levelName))
                        {
                            responses[count] = new Response(levelName, levelName);
                            count++;
                        }
                    }
                    responses[count] = new Response("Exit", "Exit");
                }
                GameLocation.afterQuestionBehavior levelSelected = new GameLocation.afterQuestionBehavior(QuestionResponse);
                Game1.player.currentLocation.createQuestionDialogue("Level Select", responses, levelSelected);
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(PlayPrairieKing)}:\n{ex}", LogLevel.Error);
            }
        }

        private static void QuestionResponse(Farmer player, string responseKey)
        {
            try
            {
                if (responseKey == "Exit")
                {
                    Game1.player.currentLocation.answerDialogueAction(responseKey, null);
                    return;
                }

                // Get level data from dictionary
                string data = LevelData.levelDictionary[responseKey];
                string[] dataList = data.Split(":");

                AbigailGame.JOTPKProgress value = new AbigailGame.JOTPKProgress();
                value.ammoLevel.Value = int.Parse(dataList[0]);
                value.bulletDamage.Value = int.Parse(dataList[1]);
                value.coins.Value = int.Parse(dataList[2]);
                value.died.Value = false; // This is always false
                value.fireSpeedLevel.Value = int.Parse(dataList[4]);
                value.lives.Value = int.Parse(dataList[5]);
                value.score.Value = int.Parse(dataList[6]);
                value.runSpeedLevel.Value = int.Parse(dataList[7]);
                value.spreadPistol.Value = false; // This is always false
                value.whichRound.Value = int.Parse(dataList[9]); // This is the NG+ Level
                value.whichWave.Value = int.Parse(dataList[10]);
                value.waveTimer.Value = int.Parse(dataList[11]);
                value.world.Value = int.Parse(dataList[12]);
                value.monsterChances.AddRange(getMonsterChances(responseKey));

                Game1.player.jotpkProgress.Value = value;
                Game1.currentMinigame = new AbigailGame();
            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(QuestionResponse)}:\n{ex}", LogLevel.Error);
                return;
            }
        }


        // Helper function to fill the monsterChances List for Prairie King
        private static List<Vector2> getMonsterChances(string responseKey)
        {
            List<Vector2> monsterChances = new List<Vector2>
                {
                    new Vector2(0.014f, 0.4f),
                    Vector2.Zero,
                    Vector2.Zero,
                    Vector2.Zero,
                    Vector2.Zero,
                    Vector2.Zero,
                    Vector2.Zero
                };

            switch (responseKey)
            {
                case "1-1":
                case "1-2":
                case "1-3":
                case "1-4":
                    monsterChances[0] = new Vector2(monsterChances[0].X + 0.001f, monsterChances[0].Y + 0.02f);
                    if (responseKey != "1-1")
                    {
                        monsterChances[2] = new Vector2(monsterChances[2].X + 0.001f, monsterChances[2].Y + 0.01f);
                    }

                    monsterChances[6] = new Vector2(monsterChances[6].X + 0.001f, monsterChances[6].Y + 0.01f);
                    break;
                case "1-B":
                case "2-1":
                case "2-2":
                case "2-3":
                    if (monsterChances[5].Equals(Vector2.Zero))
                    {
                        monsterChances[5] = new Vector2(0.01f, 0.15f);
                    }

                    monsterChances[0] = Vector2.Zero;
                    monsterChances[6] = Vector2.Zero;
                    monsterChances[2] = new Vector2(monsterChances[2].X + 0.002f, monsterChances[2].Y + 0.02f);
                    monsterChances[5] = new Vector2(monsterChances[5].X + 0.001f, monsterChances[5].Y + 0.02f);
                    monsterChances[1] = new Vector2(monsterChances[1].X + 0.0018f, monsterChances[1].Y + 0.08f);
                    break;
                case "2-B":
                case "3-1":
                case "3-2":
                case "3-3":
                    monsterChances[5] = Vector2.Zero;
                    monsterChances[1] = Vector2.Zero;
                    monsterChances[2] = Vector2.Zero;
                    if (monsterChances[3].Equals(Vector2.Zero))
                    {
                        monsterChances[3] = new Vector2(0.012f, 0.4f);
                    }

                    if (monsterChances[4].Equals(Vector2.Zero))
                    {
                        monsterChances[4] = new Vector2(0.003f, 0.1f);
                    }

                    monsterChances[3] = new Vector2(monsterChances[3].X + 0.002f, monsterChances[3].Y + 0.05f);
                    monsterChances[4] = new Vector2(monsterChances[4].X + 0.0015f, monsterChances[4].Y + 0.04f);
                    if (responseKey == "3-3")
                    {
                        monsterChances[4] = new Vector2(monsterChances[4].X + 0.01f, monsterChances[4].Y + 0.04f);
                        monsterChances[3] = new Vector2(monsterChances[3].X - 0.01f, monsterChances[3].Y + 0.04f);
                    }
                    break;
                case "3-B":
                default:
                    break;
            }

            return monsterChances;
        }


        // Unlock levels when you have reached them for the first time
        internal static void GetMapPostfix(int wave)
        {
            try
            {
                string levelName = "";
                switch (wave)
                {
                    case 0:
                        levelName = "1-1";
                        break;
                    case 1:
                        levelName = "1-2";
                        break;
                    case 2:
                        levelName = "1-3";
                        break;
                    case 3:
                        levelName = "1-4";
                        break;
                    case 4:
                        levelName = "1-B";
                        break;
                    case 5:
                        levelName = "2-1";
                        break;
                    case 6:
                        levelName = "2-2";
                        break;
                    case 7:
                        levelName = "2-3";
                        break;
                    case 8:
                        levelName = "2-B";
                        break;
                    case 9:
                        levelName = "3-1";
                        break;
                    case 10:
                        levelName = "3-2";
                        break;
                    case 11:
                        levelName = "3-3";
                        break;
                    case 12:
                        levelName = "3-B";
                        break;
                    default:
                        break;
                }


                if (levelName != "")
                {
                    StardewValley.Mods.ModDataDictionary modData = Game1.player.modData;
                    // Levels unlock when you reach them for the first time
                    if (!modData["PrairieKingUnlockedLevels"].Contains(levelName))
                    {
                        modData["PrairieKingUnlockedLevels"] += ":" + levelName;
                    }
                }

            }
            catch (Exception ex)
            {
                Monitor.Log($"Failed in {nameof(GetMapPostfix)}:\n{ex}", LogLevel.Error);
            }
        }
    }
}
