using System;

class MiniDNDGame{
    static void Pause()
    {
        Console.Write("\nPress any key to continue...");
        Console.ReadKey(true);
        Console.Write(new string(' ', Console.WindowWidth)); 
        Console.SetCursorPosition(0, Console.CursorTop - 1);
    }

    static void Main(string[] args)
    {
        Random diceRoll = new Random();
        int roll = diceRoll.Next(1, 21); // Simulate a d20 roll just like in actual DnD
        int playerHealth = 100;
        int criticalFailureBonus = 15; // Extra penalty on critical failures / NAT 1 (common house rule in the game dnd)
        int luckyCharm = 1; // Can be used to absorb or nullify the curse of fate ; added/modified in this branch
        string[] path = new string[3]; // Store player's path choices, this will determine whether player succeeds or fails

        Console.WriteLine("=================================");
        Console.WriteLine("| Welcome to this MINI DnD Game!|"); //shorter welcome message
        Console.WriteLine("=================================");
        Console.Write("\nEnter your character's name: ");
        string playerName = Console.ReadLine();

        Console.Clear();
        Console.WriteLine("Greetings, " + playerName + "! Congratulations — you’re officially an adventurer now.");
        Console.WriteLine("The dice are ready, fate is impatient, and there’s a very inconvenient curse waiting inside.");
        Console.WriteLine("Will you conquer the curse that lurks within these halls... or walk away and let it follow you forever?");
        Console.WriteLine("Your journey begins now...");
        Console.Write("\nPress ENTER to continue.");
        Console.ReadLine();


        // Roll required per scene and choice
        int[,] diceThresholds = {
            {8, 10, 6},
            {12, 11, 14},
            {15, 17, 13}
        };

        // HP penalties on failure
        int[,] penalties = {
            {15, 20, 25},      
            {30, 25, 35},     
            {40, 60, 50}       
        };

        string[] sceneTitles = {
            "The Abandoned Chapel","Whispers in the Darkness","The Curse Revelation"
        };

        string[,] sceneNarration = {
            //Scene 1 Narration
            { "\nJust before the sun sets, you arrive at the edge of a village wrapped under mists." +
            "\nThe villagers refuse to speak - except one old woman who whispers: " +
            "\n\t\"The chapel bell rings every midnight... and after that, someone mysteriously disappears.\"\n" +
            "\nThe chapel stands alone on a hill.\nIts bell rope sways gently, even though there is no wind." +
            "\n\nAs you push open the heavy wooden doors, the air was filled with dust. Inside, you saw: " +
                    "\n\t\tAn altar made of scratched stone with faded runes" +
                    "\n\t\tA narrow staircase leading down into darkness" +
                    "\n\t\tThe chapel bell rope hanging above you\n" +
            "\nAnd now the door creaks shut behind you." +
            "\nWhat would you do?"
           },
            //Scene 2 Narration
           { "\nThe chapel echoes with the decision you made, and the air grows colder." +
           "\nAs the candles flicker wildly, the shadows are stretched along the walls. They move when you don’t." +
           "\nThe weeping black mist begins to seep from the corners, whispering threats." +
           "\n\t\"Leave... or be claimed.\"" +
           "\n\nIn the corner of the chapel, you notice: " +
                    "\n\t\tShadows hanging unnaturally to the walls" +
                    "\n\t\tA glowing sigil carved into the stone floor" +
                    "\n\t\tA narrow path between the darkness, barely visible" +
           "\n\nYour heart is racing. Whatever haunts this place is now aware of you. And now, you have to: "
           },
           //Scene 3 Narration
           {"\nAs midnight approaches, the chapel begins to crack." +
           "\nThe shadows converge, twisting together into a towering figure of pure darkness." +
           "\nIts aged, furious voice echoes throughout the chapel." +
           "\n\t\"You should not have come.\"" +
           "\nThe floor cracks beneath your feet." +
           "\nIt feels as though the world is holding its breath." +
           "\nYou sense this is the final moment. There will be no escape after this.\n Your final decision has come, what would it be? "

           }
        };

        string[,] options = {
            {"Inspect the strange altar","Pull the old bell rope","Carefully descend the hidden stairs"}, //Scene 1 options
            {"Attempt to dispel the darkness","Sneak past the lurking shadows","Destroy the glowing cursed sigil"}, //Scene 2 options
            {"Confront the curse head-on","Endure the curse’s final assault","Invoke the ancient light ritual"}, //Scene 3 options
        };

        string[,] successOutcomes = {
            {
                "You uncover faint runes beneath the altar. Knowledge steadies your nerves.",
                "The bell echoes through the halls, pushing back the creeping shadows.",
                "You descend safely, avoiding loose steps and hidden traps."
            },
            {
                "You dispel part of the darkness with a steady hand.",
                "You slip past the lurking presence, unseen and unharmed.",
                "You strike the cursed sigil, weakening its grip."
            },
            {
                "The curse recoils, shattered by your resolve.",
                "You withstand the onslaught and remain standing.",
                "Light erupts as the curse collapses into nothingness."
            },
        };

        string[,] failureOutcomes = {
            {
                "The altar flares violently, searing your hands.",
                "The rope snaps — the bell crashes down toward you.",
                "The stairs crumble, sending you tumbling."
            },
            {
                "Dark whispers claw at your mind.",
                "You misstep, drawing the attention of the shadows.",
                "The sigil erupts with corrupt energy."
            },
            {
                "The curse lashes out with lethal force.",
                "You are crushed by overwhelming darkness.",
                "The ritual backfires violently."
            }
        };

        for (int scene = 0; scene < 3; scene++)
        {
            Console.Clear();
            Console.WriteLine($"=== Scene {scene + 1}: {sceneTitles[scene]} ===");
            Console.WriteLine(sceneNarration[scene, 0]);

            Pause();

            for (int i = 0; i < 3; i++)
            {
                Console.WriteLine($"{i + 1}. {options[scene, i]}");
            }

            int choice = 0;
            bool validChoice = false;
            int hintAttempts = 0;

            while (!validChoice)
            {
                Console.Write("Choose your action (1-3) or type '0' for a hint: ");
                string action = Console.ReadLine();

                if (action == "0")
                {
                    hintAttempts++;
                    if (hintAttempts == 1){
                        Console.WriteLine("\nWhy don't you just guess? :D");
                    }else if (hintAttempts == 2){
                        Console.WriteLine("\nAgain with the hint? Really?!");
                    }else if (hintAttempts == 3){
                        Console.WriteLine("\nSpirits were right in haunting you.");
                    }else if (hintAttempts == 4){
                        Console.WriteLine("\nYou are testing my patience, adventurer.");
                    }else if (hintAttempts >= 5){
                        Console.WriteLine("\n>:(");
                    }
                    continue;
                }

                if (int.TryParse(action, out choice) && choice >= 1 && choice <= 3){
                    validChoice = true;
                }else{
                    Console.WriteLine("The ancient spirits do not understand your gesture and the shadows grow restless... choose a path wisely, adventurer (1, 2, or 3). ");
                }
            }

            Console.WriteLine("\nYou chose to " + options[scene, choice - 1] + ".");
            int threshold = diceThresholds[scene, choice - 1];
            Console.WriteLine($"You need a roll of {threshold} or higher to succeed.");

            // Added feature: Lucky Charm Usage
            bool luckyCharmActivated = false;
            if (luckyCharm > 0)
            {
                string charmInput;
                do
                {
                    Console.Write("You have 1 lucky charm just in case you feel like fate isn't on your side.\nDo you wanna use it to defy fate? (Y/N): ");
                    charmInput = Console.ReadLine().Trim().ToLower();

                    if (charmInput != "y" && charmInput != "n"){
                        Console.WriteLine("\nThe spirits stare at you in confusion... Answer with Y or N only.\n");
                    }

                } while (charmInput != "y" && charmInput != "n");

                if (charmInput == "y")
                {
                    luckyCharmActivated = true;
                    luckyCharm--;
                    Console.WriteLine("\nNow, grip your lucky charm tightly...");
                }
                else
                    Console.WriteLine("You chose not to use your lucky charm. Fate awaits...");
            }             
       
            Console.WriteLine("\nPress ENTER to roll the dice...");
            Console.ReadLine();

            int fateChance = diceRoll.Next(1, 101); //Random chance to determine if curse affects even high rolls ; 55 above will apply curse, 0-54 will not
            roll = diceRoll.Next(1, 21); 
            Console.WriteLine("\nYou rolled the dice and got " + roll + " as a result.");

            int penalty = penalties[scene, choice - 1];
            int scaledPenalty = penalty + (playerHealth / 5); // Penalty increase

            if (roll == 1)
            {
                int totalPenalty = penalty + criticalFailureBonus;
                playerHealth -= totalPenalty;

                Console.WriteLine("\n!!! CRITICAL FAILURE !!!");
                Console.WriteLine("The universe conspires against you. Everything goes wrong!");
                Console.WriteLine(failureOutcomes[scene, choice - 1]);
                Console.WriteLine($"You lose {penalty} HP plus a critical penalty of {criticalFailureBonus} HP! :P\nRemaining HP: {playerHealth}");
                path[scene] = $"Scene {scene + 1}: Critical Failure";
            }
            else if (roll >= threshold)
            {
                // Modified logic for lucky charm and fate curse
                if (fateChance <= 55 || luckyCharmActivated){
                    if (luckyCharmActivated)
                        if (fateChance > 55)
                            Console.WriteLine("\nNice move, the lucky charm absorbs and nullifies the curse’s influence!");
                        else{
                            Console.WriteLine("Your lucky charm was wasted...fate was already on your side.");
                            Console.WriteLine("Lucky Charm used up!\n");
                        }
                    Console.WriteLine("\nSuccess! " + successOutcomes[scene, choice - 1]);
                    path[scene] = $"Scene {scene + 1}: Success";
                }
                else{
                    Console.WriteLine("\nThe roll was high enough, but the CURSE twists your fate!");
                    playerHealth -= scaledPenalty;
                    Console.WriteLine(failureOutcomes[scene, choice - 1]);
                    Console.WriteLine($"You lose {penalty} HP. Remaining HP: {playerHealth}");
                    path[scene] = $"Scene {scene + 1}: Failure due to bad luck";
                }
            }
            else { 
                playerHealth -= scaledPenalty;

                if (luckyCharmActivated) 
                    Console.WriteLine("\nYou've used your lucky charm desperately...");
                   
                Console.WriteLine("\nAlas! " + failureOutcomes[scene, choice - 1]);
                Console.WriteLine($"You lose {penalty} HP due to bad luck. \nRemaining HP: {playerHealth}");
                path[scene] = luckyCharmActivated ? $"Scene {scene + 1}: Failure (Lucky Charm Ineffective)": $"Scene {scene + 1}: Failure";
            }
                Pause();

            if (playerHealth <= 0){
                Console.WriteLine("A final shiver runs through your body as the darkness claims you completely… your strength fades, and your journey ends here.");
                break;
            }
        }
        //Final outcome
        Console.Clear();
        Console.WriteLine("=== ADVENTURE SUMMARY ===");
        Console.WriteLine("Adventurer: " + playerName);
        Console.WriteLine("Remaining HP: " + playerHealth);

        for (int i = 0; i < 3; i++){
            if (path[i] != null)
                Console.WriteLine(path[i]);
        }

        if (playerHealth > 0){
            Console.WriteLine("\nThe shadows fade and the chapel grows still. You have survived the trials, brave adventurer! The curse has been vanquished... for now.");
        }else{
            Console.WriteLine("\nSilence falls over the chapel. The curse has claimed you, but your story will be remembered.");
        }

        Console.WriteLine("\nThe dungeon master nods solemnly: *The dungeon takes pride in your bravery.*");
    }
}