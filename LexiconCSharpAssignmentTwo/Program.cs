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
        static string[] secretWords;

        static void Main(string[] args)
        {
            bool isAlive = true;
            string secretWord = "";
            char[] correctGuessedLetters;
            StringBuilder incorrectGuessedLetters = new StringBuilder("", 29);

            //Step 1
            if (ReadListOfSecretWordsFromFile())
            {
                foreach (string word in secretWords)
                {
                    Console.WriteLine(word);
                }

                //Step2
                ChooseSecretWord(ref secretWord);

                correctGuessedLetters = new char[secretWord.Length];

                /*
                //Loop
                while (isAlive)
                {
                    //Step 3
                    ShowVisual(secretWord, correctGuessedLetters, incorrectGuessedLetters);
                    //Step 4
                    isAlive = ReadInputFromUser();
                }
                */
            }
        }

        //Step 1, read in the secret words
        static bool ReadListOfSecretWordsFromFile()
        {
            try
            {
                using (StreamReader sr = new StreamReader(fileName))
                {
                    Console.WriteLine("---Beginning of file---\n");

                    int size = int.Parse(sr.ReadLine());

                    secretWords = new string[size];

                    string line;
                    int index = 0;

                    while ((line = sr.ReadLine()) != null)
                    {
                        secretWords[index] = line;
                        index++;
                    }

                    Console.WriteLine(sr.ReadToEnd());

                    Console.WriteLine("---End of file---");
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
            }   //If the first line in the file isn't an integer that says the number of secret words
            catch (FormatException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("First line in file is suppose to be the number of words in file!");
                Console.ResetColor();

                return false;
            }   //If the integer isn't the right size
            catch (IndexOutOfRangeException)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The integer in file is not the exact number of words in the file!");
                Console.ResetColor();

                return false;
            }
            Console.ReadKey();
            return true;
        }

        //Step 2, choose a secret word
        static void ChooseSecretWord(ref string secretWord) 
        {
            Random rnd = new Random();

            int index = rnd.Next(0, secretWords.Length);

            secretWord = secretWords[index];
        }

        //Step 3, process the visual and output it          Should it be divided into 2?
        static void ShowVisual(string secretWord, char[] correctGuessedLetters, StringBuilder incorrectGuessedLetters) { }

        //Step 4, wait for input and send it to the right method
        static bool ReadInputFromUser() { return true; }

        //Step 5, process guess, then either go back to step 3 or end the program
        static bool ProcessSingleLetterGuess() 
        { 

            return true; 
        }

        static bool ProcessAWordGuess() { return true; }

    }
}
