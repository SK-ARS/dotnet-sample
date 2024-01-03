using System;
using System.Configuration;
using System.IO;
using System.Threading;
using System.Web.Hosting;
using STP.Common.Configuration;

namespace STP.Common.Logger
{

public enum Log_Priority
{
    NO_LOGGING = 0, // No Logging
    FATAL_ERROR = 1, //Fatal Errors only
    NONFATAL_ERROR = 2, //Fatal and non fatal errors
    WARNING = 3, //Warning , Fatal and non fatal errors
    INFORMATIONAL = 4, //Information, Warning , Fatal and non fatal errors
    FUNCTIONAL = 5, // TESTING, Information, Warning , Fatal and non fatal errors
    DEBUG = 6, //DEBUG,TESTING, Information, Warning , Fatal and non fatal errors 
}


/// <summary>

/// </summary>
public class Logger : IDisposable
{
    private FileInfo fileInfo;           // File Info Variable for File Manipulation
    private FileStream fileStream;       // File Stream
    private StreamWriter streamWriter;   // Stream Writer for writing to file
    private String logFileName;       // The file name
    private String loggerFilePath;    // The file path
    private int bakupSize;              //bakup size    
    private Log_Priority _logPriorityLevel;
    private long LogFileSize = 0;

    private bool isDisposed;
        private readonly object objLogs = new object();
        private static readonly Logger instance = new Logger();
    private static bool initialized = false;
    public static readonly string LogInstance = ConfigurationManager.AppSettings["Instance"];

    public string LogPath
    {
        get
        {
            return loggerFilePath;
        }
        set
        {
            loggerFilePath = value;
        }
    }

    public string LogFileName
    {
        get
        {
            return logFileName;
        }
        set
        {
            logFileName = value;
        }
    }

    public Log_Priority LogPriorityLevel
    {
        get
        {
            return _logPriorityLevel;
        }
        set
        {
            _logPriorityLevel = value;
        }
    }


    public int BakupSize
    {
        get { return bakupSize; }
        set { bakupSize = value; }
    }
   private Logger()
    {
        // Set Everything to Default values.
        this.logFileName = "ApplicationLog.Log";
        this.loggerFilePath = "\\Log";
        this.bakupSize = 10240;
        _logPriorityLevel = Log_Priority.INFORMATIONAL;
    }

   public static Logger GetInstance()
   {
       lock (instance)
       {
           if (!initialized)
           {
            string logPath;
            if (ConfigurationManager.AppSettings["Envrironment"] == "Debug")
                logPath = HostingEnvironment.ApplicationPhysicalPath;
            else
                logPath = ConfigurationManager.AppSettings["ServerLogPhysicalPath"];
            instance.LogFileName = Settings.GetConfigValueString("logfilename", "STPServerLog.txt");
            instance.LogPath = logPath + Settings.GetConfigValueString("logpath", "\\Logs\\SystemLogs");
            instance.BakupSize = Settings.GetConfigValueInt("logbackupsize", 10240);
            instance.LogPriorityLevel = (Log_Priority)Settings.GetConfigValueInt("logpriority", (int)Log_Priority.DEBUG);
            instance.OpenLogFile(true);
            instance.LogMessage(Log_Priority.FATAL_ERROR, "Logger Started");
            initialized = true;
            }
       }

       return instance;
   }

   public void Init(string strLogName, string strLogPath, int nLogSize, Log_Priority priority)
   {
       LogFileName = strLogName;
       LogPath = strLogPath;
       BakupSize = nLogSize;
       LogPriorityLevel = priority;
   }

    ~Logger()
    {
        this.Dispose(false);
    }

    public void CloseLogFile()
    {
        if (streamWriter != null)
        {
            streamWriter.Close();
        }

        if (fileStream != null)
        {
            fileStream.Close();
        }
    }


    public void LogMessage(Log_Priority nSeverity, String strBuffer)
    {
        // Lock this Logger object for Thread Synchronization
        if (this._logPriorityLevel >= nSeverity && this._logPriorityLevel > 0)
        {
            lock (objLogs)
            {
                LogInternal(strBuffer);
            }
        }
    }




    public void LogMessage(Log_Priority nSeverity, String strFormat, params Object[] varList)
    {
        // Lock this Logger object for Thread Synchronization

        if (this._logPriorityLevel >= nSeverity && this._logPriorityLevel > 0)
        {
            lock (objLogs)
            {
                String strBuffer = "";
                //try for Format Exceptions
                try
                {
                    strBuffer = String.Format(strFormat, varList);
                    LogInternal(strBuffer);
                }
                catch
                {
                }

            }
        }
    }

    

    //private Boolean  BackupLogFile()
    //{

    //    String strPathTemp;

    //    // Check if Path is provided                
    //    if (loggerFilePath == null)
    //    {
    //        // If no Path, Set Current Directory as Path
    //        loggerFilePath = Directory.GetCurrentDirectory();

    //        if (loggerFilePath == null)
    //        {
    //            return false;
    //        }
    //    }

    //    strPathTemp = loggerFilePath;

    //    // Create File in Path Specified
    //    try
    //    {
    //        CreatePath(strPathTemp);

            
    //        if (File.Exists(loggerFilePath + "\\" + logFileName))
    //        {
                
    //            try
    //            {
    //                FileInfo fileInf = new FileInfo(loggerFilePath + "\\" + logFileName);
    //                string sBackupFile = Path.GetFileNameWithoutExtension(fileInf.FullName) + ".bak";

    //                if (File.Exists(loggerFilePath + "\\" + sBackupFile))
    //                { File.Delete(loggerFilePath + "\\" + sBackupFile); }

    //                File.Move(loggerFilePath + "\\" + logFileName, loggerFilePath + "\\" + sBackupFile);
    //            }
    //            catch (Exception ex)
    //            {

    //            }
               
    //        }
            
    //        fileInfo = new FileInfo(loggerFilePath + "\\" + logFileName);
    //        fileStream = new FileStream(loggerFilePath + "\\" + logFileName,
    //                                     FileMode.OpenOrCreate | FileMode.Append,
    //                                     FileAccess.Write, FileShare.ReadWrite);

    //        streamWriter = new StreamWriter(fileStream);
    //    }
    //    catch
    //    {
    //        return false;
    //    }
    //    return true;
    //}



    public bool OpenLogFile(bool IsBackup)
    {
        String strPathTemp;

        // Check if Path is provided                
        if (loggerFilePath == null)
        {
            // If no Path, Set Current Directory as Path
            loggerFilePath = Directory.GetCurrentDirectory();

            if (loggerFilePath == null)
            {
                return false;
            }
        }

        strPathTemp = loggerFilePath;

        // Create File in Path Specified
        try
        {
            CreatePath(strPathTemp);

            if (IsBackup)
            {
                if (File.Exists(loggerFilePath + "\\" + logFileName))
                {
                    FileInfo fileInf = new FileInfo(loggerFilePath + "\\" + logFileName);

                    LogFileSize = fileInf.Length;
                    if (LogFileSize >= this.bakupSize * 1024)
                    {
                        try
                        {
                            string sBackupFile = Path.GetFileNameWithoutExtension(fileInf.FullName) + ".bak";

                            if (File.Exists(loggerFilePath + "\\" + sBackupFile))
                            { File.Delete(loggerFilePath + "\\" + sBackupFile); }

                            File.Move(loggerFilePath + "\\" + logFileName, loggerFilePath + "\\" + sBackupFile);
                        }
                        catch (Exception )
                        { 

                        }
                    }
                }
            }
            fileInfo = new FileInfo(loggerFilePath + "\\" + logFileName);
            fileStream = new FileStream(loggerFilePath + "\\" + logFileName,
                                         FileMode.OpenOrCreate | FileMode.Append,
                                         FileAccess.Write, FileShare.ReadWrite);

            streamWriter = new StreamWriter(fileStream);
        }
        catch
        {
            return false;
        }
        return true;
    }

 
    private static void CreatePath(String strPath)
    {
        if (Directory.Exists(strPath))
        {
            return;
        }
        else
        {
            if (!string.IsNullOrEmpty(strPath))
            {
                Directory.CreateDirectory(strPath);
            }


        }
    }


    private bool LogInternal(String strBuffer)
    {
        // Get the Current time, Current Thread ID and Write to Log File
        try
        {
            String strTime = String.Format("{0:0000} {1:dd}-{1:MM}-{1:yy} {1:HH}:{1:mm}:{1:ss}",
                                           Thread.CurrentThread.GetHashCode(), DateTime.Now);
            WriteDateToFile(String.Format("{0} {1}\r\n", strTime, strBuffer));
        }
        catch
        {
            return false;
        }

        return true;
    }

    private bool WriteDateToFile(String strData)
    {
        LogFileSize += System.Text.ASCIIEncoding.ASCII.GetByteCount(strData);
        //if (LogFileSize >= this.bakupSize * 1024)
        //{
        //    BackupLogFile(); 
        //}
        streamWriter.Write(strData);
        streamWriter.Flush();
        fileStream.Flush();
        return true;
    }

    #region IDisposable Members

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion

    protected virtual void Dispose(bool bIsDisposeByUser)
    {
        if (!this.isDisposed)
        {
            if (bIsDisposeByUser)
            {
                if (fileInfo != null)
                {
                    fileInfo = null;
                }

                if (fileStream != null)
                {
                    fileStream = null;
                }
                logFileName = null;
                loggerFilePath = null;
            }

            isDisposed = true;
        }
    }

        public void LogMessage(Log_Priority fATAL_ERROR, object p)
        {
            throw new NotImplementedException();
        }
    }
}