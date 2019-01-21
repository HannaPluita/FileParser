using FileParser.Logic;
using FileParser.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser
{
    class Program
    {
        static void Main(string[] args)
        {
       
            string argsLine = "C:\\TextDir\\Abra.txt <XX> DAFF";
            string _searchSubstr = "daf";
            string _patternToReplace = "<XX>";
            string _pathBase = @"C:\TextDir\Abra.txt";

            if(args.Length == 0)
            {
                Output.OutputMessage(Output.ARGS_EMPTY);
                Console.ReadKey();
                return;
            }

            if (args.Length == 1)
            {
                Output.OutputMessage(Output.ARGS_LESS);
                Console.ReadKey();
                return;
            }

            try
            {
                if(args.Length == Analyzer.ARGS_TO_REPLACE)
                {
                    Analyzer fileAnalyze = new Analyzer(args[0], args[1], args[2]);
                    fileAnalyze.Analyze(true);   //Replace
                }

                if(args.Length == Analyzer.ARGS_TO_COUNT)
                {
                    Analyzer fileAnalyze = new Analyzer(args[0], args[1]);
                    fileAnalyze.Analyze(false);   //Count 
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }
            finally
            {
                Output.OutputMessage(Output.APP_COMPLETED);
                Console.ReadKey();
            }

            Console.ReadKey();
        }

       

        

    }
}
