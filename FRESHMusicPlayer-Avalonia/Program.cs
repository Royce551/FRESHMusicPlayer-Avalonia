using Avalonia;
using System;

namespace FRESHMusicPlayer_Avalonia
{
    class Program
    {
        // Initialization code. Don't use any Avalonia, third-party APIs or any
        // SynchronizationContext-reliant code before AppMain is called: things aren't initialized
        // yet and stuff might break.
        public static Logger logInstance;
        private static string getFilePathFromStringArray(ref string[] arr, int startIndex=0)
        {
            string retValue="";
            if(arr[startIndex].StartsWith('\"'))
            {
                bool foundEnd = false;
                int arrIt = startIndex;
                while(!foundEnd && arrIt<arr.Length){
                    foreach(char c in arr[arrIt])
                    {
                        if(c!='\"')
                        {
                            retValue+=c;
                        }
                        else
                        {
                            foundEnd =true;
                            break;
                        }
                    }
                    retValue+=" ";
                    arrIt++;
                }
                if(!foundEnd)
                {
                    throw new ArgumentException($"Incorrect file path. Did you forget a \"?, Recieved:{retValue}");
                }
            }
            else
                retValue = arr[startIndex];
            return retValue;
        }

        public static void Main(string[] args)
        {
            bool enableConsoleLogging = true;
            bool enableFileLogging = true;
            bool enableErrorLogging = true;
            bool enableWarningLogging = true;
            bool enableInformationLogging = true;
            string lfp = "";
            for (int i=0;i<args.Length;i++)
            {
                string arg = args[i];
                switch(arg)
                {
                    case "--nologall":
                        enableConsoleLogging = false;
                        enableFileLogging = false;
                        break;
                    case "--nologconsole":
                        enableConsoleLogging = false;
                        break;
                    case "--nologfile":
                        enableFileLogging = false;
                        break;
                    case "--logfile":
                        try
                        {
                            lfp = getFilePathFromStringArray(ref args,i+1);
                        }
                        catch(ArgumentException e)
                        {
                            Console.WriteLine(e.Message);
                        }
                        catch(IndexOutOfRangeException)
                        {
                            Console.WriteLine("No file path speicified");
                        }
                    break;
                    case "--nologerror":
                        enableErrorLogging = false;
                        break;
                    case "--nologwarn":
                        enableWarningLogging = false;
                        break;
                    case "--nologinfo":
                        enableInformationLogging = false;
                        break;
                    case "--database":
                        //TODO
                    break;
                }
            }
            if(lfp == "")
            {
                lfp = DateTime.Now.ToString("yyyy-MM-dd-hh-mm-ss")+".txt";
            }
            logInstance = new Logger(enableConsoleLogging,enableFileLogging,lfp,enableErrorLogging,enableWarningLogging,enableInformationLogging);
            BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
        }
        // Avalonia configuration, don't remove; also used by visual designer.
        public static AppBuilder BuildAvaloniaApp()
            => AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace();
    }
}
