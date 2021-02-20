using System;
using System.IO;
namespace FRESHMusicPlayer_Avalonia
{
    public class Logger
    {
        private string logFilePath="";
        private bool logOnConsole;
        private bool logOnFile;
        private bool logErrors;
        private bool logWarnings;
        private bool logInformation;

        private FileStream fileStream;
        private StreamWriter logFile;
        public Logger(bool lconsole,
                      bool lfile,
                      string filePath,
                      bool lerror,
                      bool lwarn,
                      bool linfo)
        {
            logOnConsole = lconsole;
            logOnFile = lfile;
            logFilePath = filePath;
            logErrors = lerror;
            logWarnings = lwarn;
            logInformation = linfo;


            if(lfile)
            {   
                try
                {
                    fileStream = new FileStream(logFilePath,FileMode.OpenOrCreate,FileAccess.Write);
                    logFile = new StreamWriter(fileStream);
                }
                catch(Exception e)
                {
                    Console.WriteLine($"Something went wrong while creating the log file: {e.Message}\n{e.StackTrace}");
                }
            }

            if(!lfile && filePath!="")
            {
                Info("Logging file path provided, however file logs are disabled. Will ignore file path");
            }
            Info($"Using {logFilePath}");
            
        }
        ~Logger()
        {
            logFile.Close();
            fileStream.Close();
        }
        public void Error(string info)
        {
            string toWrite = $"[ERROR] {info}";
            if(logErrors)
            {
                if(logOnConsole)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(toWrite);
                    Console.ResetColor();
                }
                if(logOnFile)
                {
                    logFile.WriteLine(toWrite);
                    logFile.Flush();
                }
            }
        }
        public void Warning(string info)
        {
            string toWrite = $"[WARN] {info}";
            if(logWarnings)
            {
                if(logOnConsole)
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(toWrite);
                    Console.ResetColor();
                }
                if(logOnFile)
                {
                    logFile.WriteLine(toWrite);
                    logFile.Flush();
                }
            }
        }
        public void Info(string info)
        {
            string toWrite = $"[INFO] {info}";
            if(logInformation)
            {
                if(logOnConsole)
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.WriteLine(toWrite);
                    Console.ResetColor();
                }
                if(logOnFile)
                {
                    logFile.WriteLine(toWrite);
                    logFile.Flush();
                }
            }
        }
    }
}