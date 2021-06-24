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

            string userGuess = "";
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
                    userGuess = ReadInputFromUser();

                    guessedCorr = HandlingGuesses(userGuess, secretWord, correctGuessedLetters, incorrectGuessedLetters, ref shouldGuessCount);

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

        /// <summary>
        /// Step 1, read in the secret words
        /// Reads the entire line from file and splits it into an array. It splits by commas
        /// Catches IOexception and ArgumentNullException if there is any problem with reading/accessing the file
        /// Also catches outOfMemoryException incase the line in file is too long
        /// </summary>
        /// <returns></returns>
        static bool ReadListOfSecretWordsFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    string line;

                    //Everyhing is on one line in the file
                    //And we try to read the entire line in one go then split it by commas into an array
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
            catch (OutOfMemoryException) //If the line is too long and causes out of memory failure
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The size of the file is too large!");
                Console.ResetColor();

                return false;
            }

            return true;
        }


        /// <summary>
        /// Step 2, choose a secret word
        /// Randomize an int as index and the word on that index in the secreyWordsArray will be the secret word
        /// </summary>
        /// <param name="secretWord"></param>
        static void ChooseSecretWord(ref string secretWord)
        {
            Random rnd = new Random();

            //Random an index betwen 0 and the arrays length - 1
            int index = rnd.Next(0, secretWordsArray.Length);

            //Take the word from the randomized index
            secretWord = secretWordsArray[index];

            secretWord = secretWord.ToLower();
        }

        /// <summary>
        /// Step 3, process the visual and output it
        /// Loops through the correct guessed array and writes any letters there, if there isn't a letter in an index, it writes an underscore _
        /// Then it goes through incorrent guessed letters stringbuilder to show the user what he/she have guessed
        /// </summary>
        /// <param name="secretWord"></param>
        /// <param name="correctGuessedLetters"></param>
        /// <param name="incorrectGuessedLetters"></param>
        /// <param name="guesses"></param>
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

        /// <summary>
        /// Step 4, wait for input and send it to the right method
        /// Waits for a guess, transform it to lower case if it isn't because c# is case sensitive.
        /// If the input string length is one, the program will call ProcessSingleLetterGuess
        /// If length is larger then one, the program will call ProcessAWordGuess
        /// If its less then writes out a failure message.
        /// After that if/ifelse the program calls CheckIfAllLettersCorrectlyGuessed to check if the user has won by guessing all letters
        /// </summary>
        /// <param name="secretWord"></param>
        /// <param name="correctGuessedLetters"></param>
        /// <param name="incorrectGuessedLetters"></param>
        /// <param name="shouldGuessCount"></param>
        /// <returns></returns>
        static string ReadInputFromUser()
        {
            Console.Write("Input your guess (All the mystery words are in english): ");
            string guess = Console.ReadLine();
            guess = guess.ToLower();

            return guess;
        }

        static bool HandlingGuesses(string guess, string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters, ref bool shouldGuessCount)
        {
            bool correctlyGuessedTheWord = false;

            if (guess.Length == 1)
            {
                shouldGuessCount = ProcessSingleLetterGuess(secretWord, correctGuessedLetters, incorrectGuessedLetters, guess);

                correctlyGuessedTheWord = CheckIfAllLettersCorrectlyGuessed(secretWord, correctGuessedLetters);
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

            return correctlyGuessedTheWord;
        }

        /// <summary>
        /// Step 4.1, process guessed letter, then either go back to step 3 or end the program
        /// Here the program process a single letter guess by converting it to char because most functions used here takes char not string
        /// Such as IsLetter or when the program checks if the letter already
        /// been guessed by comparing it to whats in correctGuessedLetter and incorrectGuessedLetters.
        /// If any of those did not contain that letter, the secretwrd string is looped through to check if multiple of that letter exists in the secret word.
        /// During the loop, if it finds the letter it adds it to correctGuessedLetters on the same index because they are the same length.
        /// Incorrect guessed letters are added to incorrectGuessedLetters
        /// </summary>
        /// <param name="secretWord"></param>
        /// <param name="correctGuessedLetters"></param>
        /// <param name="incorrectGuessedLetters"></param>
        /// <param name="guess"></param>
        /// <returns></returns>
        static bool ProcessSingleLetterGuess(string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters, string guess)
        {
            bool shouldGuessCount = true;

            char letter = Convert.ToChar(guess);
            if (char.IsLetter(letter))
            {
                //Checks if the letter already been guessed before by using array/string.contain
                //StringBuilder needs to be made into a string for the program to use contains unlike a char array
                if (!correctGuessedLetters.Contains(letter) && !incorrectGuessedLetters.ToString().Contains(letter))
                {
                    if (secretWord.Contains(guess))
                    {
                        //Incase there is multiple instance of the letter in the secret word
                        //The secret words is looped through and each index aka letter is checked against the guessed letter
                        //"Contains" only checks if it contains, not how many it contains
                        for (int i = 0; i < secretWord.Length; i++)
                        {
                            if(secretWord[i] == letter)
                            {
                                correctGuessedLetters[i] = letter;
                            }
                        }

                        shouldGuessCount = false;
                    }
                    else
                    {
                        //If it doesn't exist in the secret word, add to incorrect guessed StringBuilder
                        incorrectGuessedLetters.Append(letter);
                    }
                }
                else
                {
                    shouldGuessCount = false;
                }

            }
            else
            {
                shouldGuessCount = false;
            }

            return shouldGuessCount;
        }

        /// <summary>
        /// Step 4.1, process a guessed word, then either go back to step 3 or end the program
        /// Just checks if the guessed word is the right word
        /// </summary>
        /// <param name="secretWord"></param>
        /// <param name="correctGuessedLetters"></param>
        /// <param name="guess"></param>
        /// <returns></returns>
        static bool ProcessAWordGuess(string secretWord, char[] correctGuessedLetters, string guess)
        {
            bool correct = false;

            if (secretWord.Equals(guess))
            {
                correct = true;
            }

            return correct;
        }

        /// <summary>
        /// Step 4.2, Check if all letter are correctly guessed after ProcessSingleLetterGuess
        /// Here the program makes a string by sending in the char array correctGuessedLetters.
        /// So it can be checked against the secret words string. 
        /// Both needs to be strings to compare them with the regular == or equal
        /// </summary>
        /// <param name="secretWord"></param>
        /// <param name="correctGuessedLetters"></param>
        /// <returns></returns>
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
