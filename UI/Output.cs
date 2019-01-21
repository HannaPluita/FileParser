using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.UI
{
    public static class Output
    {
        #region    Constants
        public const string ARGS_EMPTY = "The argument line is empty. Application cannot run.";
        public const string ARGS_LESS = "The arguments amount is less then it necesssary for the application is running."; 
        public const string TRY_AGAIN_WITH_RESTART = "Please, restart the application with entry data as parameter.";
        public const string APP_CANNOT_PROCESS_EMPTY = "The application cannot process empty data.";
        public const string APP_COMPLETED = "Application completed.";
        public const string APP_CANNOT_CREATE_FILE = "Application cannot create a file.";

        public const string EXTRA = " Extra details:";
        public const string KEY = "   Key:";
        public const string DATA = "  Data:";
        public const string TRACE = "  StackTrace:";
        public const string SITE = "  TargetSite:";

        #endregion

        public static void OutputMessage(string message)
        {
            Console.WriteLine(message);
        }

        public static void OutputMessageSet(params string[] strings)
        {
            StringBuilder sb = new StringBuilder();

            foreach(string s in strings)
            {
                sb.Append(s);
            }

            Console.WriteLine(sb.ToString());
        }

        public static void OutputInstructions()
        {
            string path = @"..\..\Resources\Info.txt";

            try
            {
                if (File.Exists(path))
                {
                    string[] info = File.ReadAllLines(path);

                    foreach (string str in info)
                    {
                        Console.WriteLine(str);
                    }
                }
            }
            catch(Exception e)
            {
                e.Data.Add("File Path:", path);
                e.Data.Add("File Operation:", "Try to read all lines.");

                Console.WriteLine(e.Message);
                foreach(DictionaryEntry entry in e.Data)
                {
                    Console.WriteLine(string.Format("{0} {1}", entry.Key, entry.Value));
                }
            }
            
        }
      
        public static void OutputExceptionInfo(Exception e)
        {
            Console.WriteLine(e.Message);
            Console.WriteLine("{0} {1}", SITE, e.StackTrace);
            Console.WriteLine("{0} {1}", TRACE, e.TargetSite.ToString());
            
            if (e.Data.Count > 0)
            {
                Console.WriteLine(EXTRA);

                foreach (DictionaryEntry entry in e.Data)
                {
                    Console.WriteLine(" {0} {1}        {2} {3}",KEY, entry.Key.ToString(), DATA, entry.Value);
                }
            }
        }
    }

}
