using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

namespace LexiconCSharpAssignmentTwo
{
    public class Hangman
    {
        //static string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) + "\\SecretWordsList.txt";
        string fileName = "SecretWordsList.txt";

        string secretWord = "";
        char[] correctGuessedLetters;
        StringBuilder incorrectGuessedLetters = new StringBuilder("", 29);

        public Hangman()
        {

        }

        public bool InitiateHangman()
        {
            bool complete = false;

            string[] secretWordsArray = ReadListOfSecretWordsFromFile();

            if (complete)
            {
                ChooseSecretWord(secretWordsArray);

                correctGuessedLetters = new char[secretWord.Length];
                for (int i = 0; i < secretWord.Length; i++)
                {
                    correctGuessedLetters[i] = ' ';
                }
            }

            return complete;
        }

        /// <summary>
        /// Step 1, read in the secret words
        /// Reads the entire line from file and splits it into an array. It splits by commas
        /// Catches IOexception and ArgumentNullException if there is any problem with reading/accessing the file
        /// Also catches outOfMemoryException incase the line in file is too long
        /// </summary>
        /// <returns></returns>
        private string[] ReadListOfSecretWordsFromFile()
        {
            string[] secretWordsArray;

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

            return secretWordsArray;
        }


        /// <summary>
        /// Step 2, choose a secret word
        /// Randomize an int as index and the word on that index in the secreyWordsArray will be the secret word
        /// </summary>
        /// <param name="secretWord"></param>
        private void ChooseSecretWord(string[] secretWordsArray)
        {
            Random rnd = new Random();

            //Random an index betwen 0 and the arrays length - 1
            int index = rnd.Next(0, secretWordsArray.Length);

            //Take the word from the randomized index
            secretWord = secretWordsArray[index];

            secretWord = secretWord.ToLower();
        }

        public bool HandlingGuesses(string guess, string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters, ref bool shouldGuessCount)
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
        public bool ProcessSingleLetterGuess(string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters, string guess)
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
                            if (secretWord[i] == letter)
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
        public bool ProcessAWordGuess(string secretWord, char[] correctGuessedLetters, string guess)
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
        public bool CheckIfAllLettersCorrectlyGuessed(string secretWord, char[] correctGuessedLetters)
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
