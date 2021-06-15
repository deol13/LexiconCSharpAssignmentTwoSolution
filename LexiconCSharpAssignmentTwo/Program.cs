using System;
using System.IO;
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
            //Step 1
            if (ReadListOfSecretWordsFromFile())
            {
                foreach (string word in secretWords)
                {
                    Console.WriteLine(word);
                }
                
                //Step2
                chooseSecretWord();

                //Loop
                while(isAlive)
                {
                    //Step 3
                    showVisual();
                    //Step 4
                    isAlive = processTheGuess();
                }
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
        static void chooseSecretWord() { }

        //Step 3, process the visual and output it          Should it be divided into 2?
        static void showVisual() { }

        //Step 4, wait for input and process it, go back to step 3
        static bool processTheGuess() { return true; }

    }
}
