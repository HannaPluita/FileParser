using FileParser.UI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Logic
{
    public class FileOperator
    {
        protected string _fileBasePath;
        protected string _fileResultsPath;

        protected FileInfo _fileBase; 
        protected FileInfo _fileRes;

        public FileOperator(string basePath, string resPath)
        {
            _fileBasePath = basePath;
            _fileResultsPath = resPath;

            _fileBase = new FileInfo(_fileBasePath);
            _fileRes = new FileInfo(_fileResultsPath);
        }

        public FileOperator(FileOperator op)
            :this(op._fileBasePath, op._fileResultsPath)
        {
        }

        public static bool AssignAttributes(string filePath)
        {
            if (!CheckExitsFile(filePath))
            {
                return false;
            }

            return RemoveAttributes(filePath, FileAttributes.System, FileAttributes.ReadOnly, FileAttributes.Hidden)
                && SetAttributes(filePath, FileAttributes.Normal);
        }

        public static bool RemoveAttributes(string filePath, params FileAttributes[] attributes)
        {
            if (!CheckExitsFile(filePath))
            {
                return false;
            }

            try
            {
                FileAttributes fileAttribs = File.GetAttributes(filePath);

                foreach (FileAttributes attr in attributes)
                {
                    fileAttribs = fileAttribs & ~attr;
                }
            }
            catch (Exception e)
            {
                e.Data.Add("File path", filePath);
                e.Data.Add("File operation", "Removing file attributes");

                throw new Exception(e.Message, e);
            }

            return true;
        }

        public static bool SetAttributes(string filePath, params FileAttributes[] attributes)
        {
            if (!CheckExitsFile(filePath))
            {
                return false;
            }

            try
            {
                FileAttributes fileAttribs = File.GetAttributes(filePath);

                foreach (FileAttributes attr in attributes)
                {
                    fileAttribs = fileAttribs | attr;
                }

                return true;
            }
            catch (Exception e)
            {
                e.Data.Add("File path", filePath);
                e.Data.Add("File operation", "Setting file attributes");

                throw new Exception(e.Message, e);
            }

        }

        public static bool CheckExitsFile(string path)
        {
            if (!File.Exists(path))
            {
                Output.OutputMessage(string.Format("{0}: {1}", path, ParserText.ERR_BASE_FILE_NOT_EXISTS));
                Console.ReadKey();

                return false;
            }

            return true;
        }

        public static bool CheckNotZeroLength(string path)
        {
            FileInfo file = new FileInfo(path);

            if(!File.Exists(path))
            {
                return false;
            }

            if (file.Length == 0)
            {
                Output.OutputMessage(string.Format("{0}: {1}", path, ParserText.ERR_EMPTY_BASE_FILE));
                Console.ReadKey();

                return false;
            }

            return true;
        }

        protected static string GetFullPathTempFile(string baseFullPath, string tmpFileName)
        {
            if (string.IsNullOrEmpty(baseFullPath) || string.IsNullOrEmpty(tmpFileName))
            {
                Output.OutputMessage(ParserText.ERR_EMPTY_STR);
                return null;
            }

            return Path.Combine(Path.GetDirectoryName(baseFullPath), tmpFileName);

        }

        public static bool TempFileCreateOrDelete(string baseFullPath, string tmpFileName, out string tempFilePath)
        {
            if (string.IsNullOrEmpty(baseFullPath) || string.IsNullOrEmpty(tmpFileName))
            {
                Output.OutputMessage(ParserText.ERR_EMPTY_STR);
                tempFilePath = null;

                return false;
            }

            string pathResult = GetFullPathTempFile(baseFullPath, tmpFileName);

            if(pathResult == null)
            {
                Output.OutputMessage(ParserText.ERR_TEMP_PATH_NOT_CORRECT);
                tempFilePath = null;

                return false;
            }

            try
            {
                tempFilePath = pathResult;

                if (!File.Exists(pathResult))
                {
                    File.Create(pathResult);
                    FileOperator.SetAttributes(pathResult);
                    return true;
                }

                FileOperator.SetAttributes(pathResult);
                File.Delete(pathResult);

                return false;
            }
            catch(Exception e)
            {
                e.Data.Add("File path", pathResult);
                e.Data.Add("File operating", "Create temp file");

                throw new Exception(e.Message, e);
            }
        }

        public static bool ReplaceFiles(string pathBase, string pathResultToBase)
        {
            if (!File.Exists(pathBase) || !File.Exists(pathResultToBase))
            {
                Output.OutputMessage(ParserText.ERR_BASE_TEMP_FILES_NOT_EXISTS);
                return false;
            }

            try
            {
                FileInfo fileBase = new FileInfo(pathBase);
                FileInfo fileResultToBase = new FileInfo(pathResultToBase);

                fileBase.Delete();
                fileResultToBase.MoveTo(pathBase);

                return true;
            }
            catch (Exception e)
            {
                e.Data.Add("Base file", pathBase);
                e.Data.Add("Result file", pathResultToBase);
                e.Data.Add("File operating", "Replacing a file");

                throw new Exception(e.Message, e);
            }
        }

    }
}
