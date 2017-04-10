using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Xml;
namespace Question1_Assignment2_
{
    public partial class Service1 : ServiceBase
    {
        FileSystemWatcher watcher;
        private static Timer _timer;
        int timercount = 1 * 60 * 1000; //check every minute
        public Service1()
        {
            InitializeComponent();
        }
        public void onDebug(){
            OnStart(null);
        }
        protected override void OnStart(string[] args){
         
            _timer = new Timer(timercount);//1 * 60 * 1000 
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.Start();

        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
                _timer.Stop();
                watcher = new FileSystemWatcher();
                watcher.Path = @"C:\Users\k132212\Documents\MonitorThisFolder";
                watcher.NotifyFilter = NotifyFilters.Attributes |
                NotifyFilters.CreationTime |
                NotifyFilters.DirectoryName |
                NotifyFilters.FileName |
                NotifyFilters.LastAccess |
                NotifyFilters.LastWrite |
                NotifyFilters.Security |
                NotifyFilters.Size;
                watcher.Filter = "*.*";
                watcher.Changed += new FileSystemEventHandler(OnChanged);
                watcher.Created += new FileSystemEventHandler(OnChanged);
                watcher.Deleted += new FileSystemEventHandler(OnChanged);
                watcher.Renamed += new RenamedEventHandler(OnRenamed);
                watcher.EnableRaisingEvents = true;
            
            if (_timer.Interval + (2 * 60 * 1000) < 3600000) //360000 is equal to 1 hour
            {
                _timer.Interval = _timer.Interval + (2 * 60 * 1000);
                _timer.Start();

            }
            else
            {
                //if time limit is exceeding an hour then stick to an hour
                _timer.Interval = 3600000;
                _timer.Start();

            }
          
            }
        public static void OnChanged(object source, FileSystemEventArgs e)
    {
        Console.WriteLine("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);
        File.Copy(@"C:\Users\k132212\Documents\MonitorThisFolder\" + e.Name, @"C:\Users\k132212\Documents\CopyToThis\" + e.Name);
        _timer.Interval = 1 * 60 * 1000; //reset the timer because there was change in the folder

        }

    public static void OnRenamed(object source, RenamedEventArgs e)
    {
        Console.WriteLine(" {0} renamed to {1}", e.OldFullPath, e.FullPath);
        File.Copy(@"C:\Users\k132212\Documents\MonitorThisFolder\" + e.Name, @"C:\Users\k132212\Documents\CopyToThis\" + e.Name);
        _timer.Interval = 1 * 60 * 1000; //reset the timer because there was change in the folder
    }

        protected override void OnStop()
        {
            
        }
    }
}
