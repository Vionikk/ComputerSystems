using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Lab1InformationAnalyze
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.Unicode;

            string path = @"C:\Users\vlady\OneDrive\Desktop\Computer systems\Lab1\";
            string textFile;
            int totalCountCharacters;
            long fileSize;
            double entropy, infoQuant;
            Dictionary<char, double> characters = new Dictionary<char, double>();


            try
            {
                Console.Write("Enter the name of text file (example.txt): ");
                textFile = Console.ReadLine();
                Console.WriteLine(); 
                Console.WriteLine("===========================================================");
                path += textFile;

                fileSize = ReadFile(path, characters, out totalCountCharacters);
                CharactProbability(characters, totalCountCharacters);
                entropy = AverageEntropy(characters);
                infoQuant = InfoQuantity(entropy, totalCountCharacters);
                AnalyzeInfo(characters, totalCountCharacters, entropy, infoQuant, fileSize);
                Console.WriteLine("===========================================================");
            }
            catch (FileNotFoundException fnfexc)
            {
                Console.WriteLine(fnfexc.Message);
            }
            catch (IOException ioexc)
            {
                Console.WriteLine(ioexc.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            Console.ReadKey();
        }
        public static void CharactProbability(Dictionary<char, double> chrcts, int totalCount)
        {
            //The number of keys in the dictionary
            int countKeysDict = chrcts.Keys.Count;
            char[] keysDict = new char[countKeysDict];
            chrcts.Keys.CopyTo(keysDict, 0);

            for (int iter = 0; iter < countKeysDict; iter++)
            {
                chrcts[keysDict[iter]] /= totalCount;
            }
        }

        public static double AverageEntropy(Dictionary<char, double> chrcts)
        {
            int countChrct = chrcts.Keys.Count;
            char[] keysDict = new char[countChrct];
            chrcts.Keys.CopyTo(keysDict, 0);
            double probability = 0, entropy = 0;

            for (int iter = 0; iter < countChrct; iter++)
            {
                probability = chrcts[keysDict[iter]];
                entropy -= probability * Math.Log(probability, 2);
            }
            return entropy;
        }

        public static double InfoQuantity(double entrp, int chrctCount)
        {
            return entrp * chrctCount;
        }

        public static void AnalyzeInfo(Dictionary<char, double> chrcts, int totalCount, double entropy, double infoQuant, long fileSize)
        {
            Console.WriteLine("Size of file = {0} bytes", fileSize);
            Console.WriteLine("Info Quantity = {0} bytes", infoQuant / 8);
            Console.WriteLine("Info Quantity = {0} bits", infoQuant);
            Console.WriteLine("Entropy = {0}", entropy);
            Console.WriteLine("TotalCountChrcts = {0}", totalCount);
            Console.WriteLine("===========================================================");
            SortedDictionary<char, double> sortedDict = new SortedDictionary<char, double>(chrcts);
            foreach (KeyValuePair<char, double> kvp in sortedDict)
            {
                Console.WriteLine(kvp.Key + " - " + kvp.Value);
            }
        }

        public static long ReadFile(string pathFile, Dictionary<char, double> chrcts, out int totalCountChrcts)
        {
            FileInfo fileSize = new FileInfo(pathFile);
            int iterator;
            //Repetition of a character in the text
            double chrctRecur;
            totalCountChrcts = 0;

            string allText = File.ReadAllText(pathFile);
            iterator = 0;
            while (iterator < allText.Length)
            {
                chrctRecur = 1;
                if (!chrcts.ContainsKey(allText[iterator]))
                {
                    chrcts.Add(allText[iterator], chrctRecur);
                }
                else
                    if (chrcts.ContainsKey(allText[iterator]))
                {
                    chrcts[allText[iterator]]++;
                }
                iterator++;
                totalCountChrcts++;
            }
            return fileSize.Length;
        }

    }
}