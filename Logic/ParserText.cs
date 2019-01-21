using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FileParser.UI;

namespace FileParser.Logic
{
    public static class ParserText
    {
        public static Object locker = new object();

        public const string EMPTY_ARGS = "The empty file path string passed or no assigned substring to search for replacement.";
        public const string ERR_BASE_FILE_NOT_EXISTS = "Error: base file does not exist.";
        public const string ERR_BASE_TEMP_FILES_NOT_EXISTS = "Error: base file does not exist or tempt file was not be created.";
        public const string ERR_SUBSTR_EMPTY = "The substring to replace with is empty.\n Deletion of the search substring is performing";
        public const string ERR_STRING_LENGTH = "Error of string length.";
        public const string ERR_SUBSTR_LONGER = "Error of string length: search substring is longer than the base string.";
        public const string ERR_EMPTY_BASE_FILE = "Base file is empty.";
        public const string ERR_EMPTY_STR = "Error of file path string: empty base file path or empty temp file name.";
        public const string ERR_TEMP_PATH_NOT_CORRECT = "Error of temp file path: incorrect data for assigning temp file path string.";
        public const string TEMP_FILE_EXISTS = "Temp file alrady exists.";

        public const string ERR_EMPTY = "The empty file path string passed or no assigned substring to search for replacement.";
        public const string AMOUNT = "Amount of subsrting inclusions in lines:";
        public const string TOTAL = "Total amount";

        public static void ReplacePatternInFile(string pathBase, string pathResult, string searchSubstr, string patternToReplace, bool tempFileExists)
        {
            if (string.IsNullOrEmpty(pathBase) || string.IsNullOrEmpty(pathResult) || string.IsNullOrEmpty(searchSubstr) || patternToReplace == null)
            {
                Output.OutputMessage(EMPTY_ARGS);
                return;
            }

            if (!File.Exists(pathBase)) 
            {
                Output.OutputMessage(ERR_BASE_FILE_NOT_EXISTS);
                Console.ReadKey();
                return;
            }

            if (patternToReplace == null)
            {
                Output.OutputMessage(ERR_SUBSTR_EMPTY);
                patternToReplace = string.Empty;
            }

            lock(locker)
            {
                if(!tempFileExists)
                {
                    FileInfo fInfo = new FileInfo(pathResult);
                    using (fInfo.Create())
                    {
                    }
                }

                using (StreamReader reader = new StreamReader(pathBase))
                using (StreamWriter writer = new StreamWriter(pathResult))
                {
                    string baseLine = string.Empty;

                    while ((baseLine = reader.ReadLine()) != null)
                    {
                        baseLine = ReplacePatternInLine(baseLine, searchSubstr, patternToReplace);

                        writer.WriteLine(baseLine);
                    }
                }
               
            }
        }

        public static string ReplacePatternInLine(string baseTxt, string searchSubstr, string pattern)
        {
            if (baseTxt.Length < pattern.Length || baseTxt.Length < pattern.Length)
            {
                Console.WriteLine(ERR_STRING_LENGTH);
                return null;
            }

            string resultTxt = baseTxt;

            int iteration = 0;
            bool contains = false;

            if (baseTxt.Contains(searchSubstr))
            {
                int startIndex = 0;
                string currTxt = baseTxt;
                int currIndex = 0;

                while (contains = baseTxt.Contains(searchSubstr) && (startIndex + pattern.Length < baseTxt.Length - 1))
                {
                    ++iteration;

                    currIndex = resultTxt.IndexOf(searchSubstr, startIndex);       
                    currTxt = currTxt.Remove(currIndex, searchSubstr.Length);    
                    currTxt = currTxt.Insert(currIndex, pattern);             

                    startIndex = currIndex + pattern.Length;
                    resultTxt = currTxt;
                }
            }

            return resultTxt;
        }

        public static uint CountContainedInFile(string pathBase, string pathResult, string searchSubstr, bool resultFileExists)
        {
            if (string.IsNullOrEmpty(pathBase) || string.IsNullOrEmpty(pathResult) || string.IsNullOrEmpty(searchSubstr))
            {
                Output.OutputMessage(ERR_EMPTY);
                return 0;
            }

            if (!resultFileExists)
            {
                FileInfo fInfo = new FileInfo(pathResult);
                using (fInfo.Create())
                {
                }
            }

            uint totalCount = 0;

            using (StreamReader reader = new StreamReader(pathBase))
            using (StreamWriter writer = new StreamWriter(pathResult))
            {
                writer.WriteLine(AMOUNT);

                string lineToCount = string.Empty;
                uint lineIndex = 0;

                while ((lineToCount = reader.ReadLine()) != null)
                {
                    ++lineIndex;
                    uint countInLine = CountContainedInLine(lineToCount, searchSubstr);

                    if (countInLine > 0)
                    {
                        writer.WriteLine(string.Format("{0}: {1}", lineIndex, countInLine));
                    }

                    totalCount += countInLine;

                    Console.WriteLine(string.Format("{0}: {1}", lineIndex, countInLine));
                }

                writer.WriteLine("{0}: {1}", TOTAL, totalCount);
            }

            return totalCount;
        }

        public static uint CountContainedInLine(string baseTxt, string searchSubstr)
        {
            if (baseTxt.Length < searchSubstr.Length)
            {
                Console.WriteLine(ERR_SUBSTR_LONGER);
                return 0;
            }

            uint iteration = 0;

            if (baseTxt.Contains(searchSubstr))
            {
                string currTxt = baseTxt;
                int currIndex = 0;

                while (baseTxt.Contains(searchSubstr) && (currIndex < baseTxt.Length - 1))
                {
                    ++iteration;

                    currIndex = currTxt.IndexOf(searchSubstr, currIndex);
                    currTxt = currTxt.Remove(currIndex, searchSubstr.Length);
                }
            }

            return iteration;
        }
    }
}
