using FileParser.Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    public class Analyzer
    {
        protected string _baseFile;
        protected string _searchString;
        protected string _pattern;

        public const string TEMP_FILE_NAME = "Temp.txt";
        public const string RESULT_FILE_NAME = "Result.txt";

        public const byte ARGS_TO_REPLACE = 3;
        public const byte ARGS_TO_COUNT = 2;

        public Analyzer()
        {
        }

        public Analyzer(string baseFile, string search, string pattern = "")
        {
            _baseFile = baseFile;
            _searchString = search;
            _pattern = pattern;
        }

        public Analyzer(Analyzer a)
            : this(a._baseFile, a._searchString, a._pattern)
        {
        }

        public void Analyze(bool replace)
        {
            if (!FileOperator.CheckExitsFile(_baseFile))
            {
                return;
            }

            if (!FileOperator.CheckNotZeroLength(_baseFile))
            {
                return;
            }

            string tempFullPath = null;

            if (replace)   //If search substring and replace it with the pattern
            {
                bool tempFileCreated = FileOperator.TempFileCreateOrDelete(_baseFile, TEMP_FILE_NAME, out tempFullPath);

                if (!string.IsNullOrEmpty(tempFullPath))
                {
                    ParserText.ReplacePatternInFile(_baseFile, tempFullPath, _searchString, _pattern, tempFileCreated);
                    FileOperator.ReplaceFiles(_baseFile, tempFullPath);
                }
            }
            else         //Count sunbstrings inclusions amount
            {
                bool resultFileCreated = FileOperator.TempFileCreateOrDelete(_baseFile, RESULT_FILE_NAME, out tempFullPath);

                if (!string.IsNullOrEmpty(tempFullPath))
                {
                    ParserText.CountContainedInFile(_baseFile, RESULT_FILE_NAME, _searchString, resultFileCreated);
                }
            }
        }
    }
}
