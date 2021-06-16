using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace LexiconCSharpAssignmentTwo
{
    class Program
    {
        //static string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SecretWordsList.txt";
        static string fileName = "SecretWordsList.txt";
        static string[] secretWordsArray;

        static void Main(string[] args)
        {
            bool shouldGuessCount = true;
            bool guessedCorr = false;

            string secretWord = "";
            char[] correctGuessedLetters;
            int guessCount = 10;
            StringBuilder incorrectGuessedLetters = new StringBuilder("", 29);

            //Intro
            Console.WriteLine("Welcome to hangman, a mystery word will be picked and you will be presented with _ instead of its letter."
                + "\nYou have 10 tries to guess what letter the word contains, guessing the same word twice doesn't use a try."
                + "\nYou can try to guess the entire word at any time, but it cost you one try, you lose the game when all 10 tries are used."
                + "\nGood luck and have fun"
                + "\nPress a key to start the game!");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("\n\n\n\n\n\n");

            //Step 1
            if (ReadListOfSecretWordsFromFile())
            {
                //Step2
                ChooseSecretWord(ref secretWord);

                //Allocate the memory for the char array now that we know what it's size should be.
                //Fill it with ' ' so we can check against them when we output.
                correctGuessedLetters = new char[secretWord.Length];
                for (int i = 0; i < secretWord.Length; i++)
                {
                    correctGuessedLetters[i] = ' ';
                }

                //Loop
                do
                {
                    Console.Clear();
                    shouldGuessCount = true;

                    //Step 3
                    ShowVisual(secretWord, correctGuessedLetters, incorrectGuessedLetters, guessCount);
                    //Step 4
                    guessedCorr = ReadInputFromUser(secretWord, correctGuessedLetters, incorrectGuessedLetters, ref shouldGuessCount);

                    if (shouldGuessCount)
                    {
                        guessCount--;
                    }

                } while (guessCount > 0 && !guessedCorr);

                Console.Clear();
                if(guessedCorr)
                    Console.WriteLine("\nCongrats you guessed correctly!");
                else
                    Console.WriteLine("You lost, better luck next time!\n");

                Console.WriteLine("The secret words was: " + secretWord
                    + "\nPress a key to exit the program.");
                Console.ReadKey();

            }
        }

        //Step 1, read in the secret words
        static bool ReadListOfSecretWordsFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;

                    if ((line = sr.ReadToEnd()) != null)
                    {
                        secretWordsArray = line.Split(",");
                    }
                }
            }   //If for whatever reason the file couldn't be read this will show the message
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File error: " + e.Message);
                Console.ResetColor();

                return false;
            }
            catch (ArgumentNullException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: Path is null!");
                Console.ResetColor();

                return false;
            }
            catch (OutOfMemoryException) //If you are using sr.ReadToEnd
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The size of the file is too large!");
                Console.ResetColor();

                return false;
            }

            return true;
        }

        //Step 2, choose a secret word
        static void ChooseSecretWord(ref string secretWord)
        {
            Random rnd = new Random();

            int index = rnd.Next(0, secretWordsArray.Length);

            secretWord = secretWordsArray[index];

            secretWord = secretWord.ToLower();
        }

        //Step 3, process the visual and output it          Should it be divided into 2?
        static void ShowVisual(string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters, int guesses)
        {
            Console.WriteLine($"Number of guesses left: {guesses}\n\n\n\n");

            //Step 3.1, Show secret word
            //Outputs correctly guessed letters or _ if not correctly guessed
            for (int i = 0; i < secretWord.Length; i++)
            {
                if (correctGuessedLetters[i] != ' ')
                {
                    Console.Write(correctGuessedLetters[i] + " ");
                }
                else
                    Console.Write("_ ");
            }

            Console.WriteLine();
            Console.WriteLine("Incorrectly guessed letters:");

            //Step 3.2, Show incorrect guessed letters
            for (int i = 0; i < incorrectGuessedLetters.Length; i++)
            {
                Console.Write(incorrectGuessedLetters[i] + " ");
            }
            Console.WriteLine();
        }

        //Step 4, wait for input and send it to the right method
        static bool ReadInputFromUser(string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters, ref bool shouldGuessCount)
        {
            bool correctlyGuessedTheWord = false;

            Console.Write("Input your guess (All the mystery words are in english): ");
            string guess = Console.ReadLine();
            guess = guess.ToLower();

            if (guess.Length == 1)
            {
                shouldGuessCount = ProcessSingleLetterGuess(secretWord, correctGuessedLetters, incorrectGuessedLetters, guess);
            }
            else if (guess.Length > 1)
            {
                correctlyGuessedTheWord = ProcessAWordGuess(secretWord, correctGuessedLetters, guess);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You need to input something to make a guess!");
                Console.ResetColor();
            }

            correctlyGuessedTheWord = CheckIfAllLettersCorrectlyGuessed(secretWord, correctGuessedLetters);
            
            return correctlyGuessedTheWord;
        }

        //Step 4.1, process guessed letter, then either go back to step 3 or end the program
        static bool ProcessSingleLetterGuess(string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters, string guess)
        {
            bool shouldGuessCount = true;

            char letter = Convert.ToChar(guess);
            if (char.IsLetter(letter))
            {
                if (!correctGuessedLetters.Contains(letter) && !incorrectGuessedLetters.ToString().Contains(letter))
                {
                    if (secretWord.Contains(guess))
                    {
                        for (int i = 0; i < secretWord.Length; i++)
                        {
                            if(secretWord[i] == letter)
                            {
                                correctGuessedLetters[i] = letter;
                            }
                        }
                    }
                    else
                    {
                        incorrectGuessedLetters.Append(letter);
                    }
                }
                else
                {
                    shouldGuessCount = false;
                }

            }

            return shouldGuessCount;
        }

        //Step 4.1, process a guessed word, then either go back to step 3 or end the program
        static bool ProcessAWordGuess(string secretWord, char[] correctGuessedLetters, string guess)
        {
            bool correct = false;

            if (secretWord.Equals(guess))
            {
                correct = true;
            }

            return correct;
        }

        //Step 4.2, Check if all letter are correctly guessed after ProcessSingleLetterGuess
        static bool CheckIfAllLettersCorrectlyGuessed(string secretWord, char[] correctGuessedLetters)
        {
            bool correctlyGuessedTheWord = false;
            string charStr = new string(correctGuessedLetters);
            if (charStr == secretWord)
            {
                correctlyGuessedTheWord = true;
            }

            return correctlyGuessedTheWord;
        }

    }
}
