using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace FolderWatcher
{
    public partial class FileWatcherService : ServiceBase
    {
        List<string> folderPaths = new List<string>();
        List<string> servicePaths = new List<string>();
        string logPath = ConfigurationManager.AppSettings["LogPath"] + "\\" + DateTime.Now.ToString("dd-MM-yy") + ".txt";
        public FileWatcherService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("FolderWatcher"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "FolderWatcher", "MyLog");
            }
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                WriteLog("Service started");
                folderPaths = ConfigurationManager.AppSettings["FolderPaths"].Split(';').ToList();
                servicePaths = ConfigurationManager.AppSettings["ServicePaths"].Split(';').ToList();
                List<FileSystemWatcher> listWatcher = new List<FileSystemWatcher>();
                foreach (string path in folderPaths)
                {
                    WriteLog("Registering path:" + path);
                    // Create a new FileSystemWatcher and set its properties.
                    FileSystemWatcher watcher = new FileSystemWatcher();
                    watcher.Path = path;

                    //// Watch for changes in LastAccess and LastWrite times, and
                    //// the renaming of files or directories.
                    //watcher.NotifyFilter = NotifyFilters.LastAccess
                    //                     | NotifyFilters.LastWrite
                    //                     | NotifyFilters.FileName
                    //                     | NotifyFilters.DirectoryName;

                    //watcher.Filter = "*.*";

                    // Add event handlers.
                    watcher.Changed += OnChanged;
                    
                    // Begin watching.
                    watcher.EnableRaisingEvents = true;

                    listWatcher.Add(watcher);
                    // Wait for the user to quit the program.
                    WriteLog("Monitoring event on " + path);
                }
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        private void OnChanged(object sender, FileSystemEventArgs e)
        {
            try
            {
                WriteLog(e.FullPath);
                // calling service in here            
                var index = folderPaths.FindIndex(path => e.FullPath.Contains(path));
                var servicePathFound = servicePaths[index];
                WriteLog($"Event on folder: {e.Name} {e.ChangeType} {servicePathFound}");
            }
            catch (Exception ex)
            {
                WriteLog(ex.ToString());
            }
        }

        bool dateChanged = false;
        private bool EnableLog()
        {
            if (string.IsNullOrEmpty(logPath))
            {
                return false;
            }
            if (!dateChanged && DateTime.Now.Hour == 0)
            {
                dateChanged = true;
                logPath = $"${logPath}\\${DateTime.Now.ToString("dd-MM-yy")}.txt";
            }
            if (dateChanged && DateTime.Now.Hour != 0)
            {
                dateChanged = false;
            }
            return true;
        }
        private void WriteLog(string msg)
        {
            if (EnableLog())
            {
                // eventLog1.WriteEntry($"Event on folder: {e.Name} {e.ChangeType} {servicePathFound}");
                File.AppendAllText(logPath, "\r\n" + msg);                
            }
        }

        protected override void OnStop()
        {
            WriteLog($"Service stopped");
        }
    }
}