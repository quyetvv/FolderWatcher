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
        public FileWatcherService()
        {
            InitializeComponent();
            eventLog1 = new System.Diagnostics.EventLog();
            if (!System.Diagnostics.EventLog.SourceExists("FolderWatcher"))
            {
                System.Diagnostics.EventLog.CreateEventSource(
                    "FolderWatcher", "MyLog");
            }
            eventLog1.Source = "FolderWatcher";
            eventLog1.Log = "MyLog";            
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                WriteLog("Service started");
                folderPaths = ConfigurationManager.AppSettings["FolderPaths"].Split(';').ToList();
                servicePaths = ConfigurationManager.AppSettings["ServicePaths"].Split(';').ToList();
                foreach (string path in folderPaths)
                {
                    WriteLog("Registering path:" + path);
                    // Create a new FileSystemWatcher and set its properties.
                    using (FileSystemWatcher watcher = new FileSystemWatcher())
                    {
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
                        watcher.Created += OnChanged;
                        watcher.Deleted += OnChanged;

                        // Begin watching.
                        watcher.EnableRaisingEvents = true;

                        // Wait for the user to quit the program.
                        WriteLog("Monitoring event on " + path);
                    }
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

        private void WriteLog(string msg)
        {
            // eventLog1.WriteEntry($"Event on folder: {e.Name} {e.ChangeType} {servicePathFound}");
            File.AppendAllText("C:\\fw-log.txt", "\r\n" + msg);
        }

        protected override void OnStop()
        {
            eventLog1.WriteEntry($"Service stopped");
        }
    }
}
