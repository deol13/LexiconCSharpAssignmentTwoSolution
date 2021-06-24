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
        static void Main(string[] args)
        {
            bool shouldGuessCount = true;
            bool guessedCorr = false;

            string userGuess = "";
            int guessesLeft = 10;

            Hangman hangman = new Hangman();

            //Intro
            Console.WriteLine("Welcome to hangman, a mystery word will be picked and you will be presented with _ instead of its letter."
                + "\nYou have 10 tries to guess what letter the word contains, guessing the same word twice doesn't use a try."
                + "\nYou can try to guess the entire word at any time, but it cost you one try, you lose the game when all 10 tries are used."
                + "\nGood luck and have fun"
                + "\nPress a key to start the game!");
            Console.ReadKey();
            Console.Clear();

            Console.WriteLine("\n\n\n\n\n\n");

            try
            {
                hangman.InitiateHangman();

                //Loop
                do
                {
                    Console.Clear();

                    shouldGuessCount = true;
                    Console.WriteLine(hangman.SecretWord);
                    
                    ShowVisual(guessesLeft, hangman);
                    
                    userGuess = ReadInputFromUser();

                    guessedCorr = hangman.HandlingGuesses(userGuess, ref shouldGuessCount);

                    if (shouldGuessCount)
                    {
                        guessesLeft--;
                    }

                } while (guessesLeft > 0 && !guessedCorr);


                Console.Clear();
                if (guessedCorr)
                {
                    Console.WriteLine("\nCongrats you guessed correctly!");
                }
                else
                {
                    Console.WriteLine("You lost, better luck next time!\n");
                }

                Console.WriteLine("The secret words was: " + hangman.SecretWord
                    + "\nPress a key to exit the program.");
                Console.ReadKey();
            }
            catch (IOException e)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("File error: " + e.Message);
                Console.ResetColor();
            }
            catch (ArgumentNullException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Error: File path is null!");
                Console.ResetColor();
            }
            catch (OutOfMemoryException) //If the line is too long and causes out of memory failure
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The size of the file is too large!");
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Process the visual and output it
        /// Loops through the correct guessed array and writes any letters there, if there isn't a letter in an index, it writes an underscore _
        /// Then it goes through incorrent guessed letters stringbuilder to show the user what he/she have guessed
        /// </summary>
        /// <param name="guessesLeft">Number of guesses left</param>
        /// <param name="hangman">Hangman object</param>
        static void ShowVisual(int guessesLeft, Hangman hangman)
        {
            char[] correctGuessedLetters = hangman.CorrectGuessedLetters;
            StringBuilder incorrectGuessedLetters = hangman.IncorrectGuessedLetters;

            Console.WriteLine($"Number of guesses left: {guessesLeft}\n\n\n\n");

            //Step 3.1, Show secret word
            //Outputs correctly guessed letters or _ if not correctly guessed
            for (int i = 0; i < correctGuessedLetters.Length; i++)
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

        /// <summary>
        /// Waits for a guess, transform it to lower case if it isn't because c# is case sensitive.
        /// </summary>
        /// <returns>Return the guess</returns>
        static string ReadInputFromUser()
        {
            Console.Write("Input your guess (All the mystery words are in english): ");
            string guess = Console.ReadLine();
            guess = guess.ToLower();

            return guess;
        }
    }
}
