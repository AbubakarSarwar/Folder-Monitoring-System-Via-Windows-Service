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
        static Timer _timer;
        FileSystemWatcher watcher;
        public Service1()
        {
            InitializeComponent();
        }
        public void onDebug(){
            OnStart(null);
        }
        protected override void OnStart(string[] args){

            _timer = new Timer(0.1 * 60 * 1000); //15 minutes timer 
            _timer.Elapsed += new System.Timers.ElapsedEventHandler(timer_Elapsed);
            _timer.Start();

        }
        private void timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
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
            
          
            }
        public static void OnChanged(object source, FileSystemEventArgs e)
    {
        Console.WriteLine("{0}, with path {1} has been {2}", e.Name, e.FullPath, e.ChangeType);

        try
        {
            MailMessage mail = new MailMessage();
            mail.To.Add(System.Configuration.ConfigurationManager.AppSettings["Username"]); //SENDING MAIL TO LOCAL USER ITSELF
            mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Username"]);
            mail.Subject = "Changes has been made in the folder! Alert! Alert! Alert!";
            long length = new System.IO.FileInfo(e.FullPath).Length;
            FileInfo filename= new FileInfo(@"C:\Users\k132212\Documents\MonitorThisFolder\"+e.Name);
            mail.Body = "Filename = " + e.Name + "Size = " + filename.Length;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
            smtp.Credentials = new System.Net.NetworkCredential
                 (System.Configuration.ConfigurationManager.AppSettings["Username"], System.Configuration.ConfigurationManager.AppSettings["Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);
            Console.WriteLine("Successfully sent");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
        }

    public static void OnRenamed(object source, RenamedEventArgs e)
    {
        Console.WriteLine(" {0} renamed to {1}", e.OldFullPath, e.FullPath);

        try
        {

            MailMessage mail = new MailMessage();
            mail.To.Add(System.Configuration.ConfigurationManager.AppSettings["Username"]); //SENDING MAIL TO LOCAL USER ITSELF
            mail.From = new MailAddress(System.Configuration.ConfigurationManager.AppSettings["Username"]);
            mail.Subject = "Changes has been made in the folder! Alert! Alert! Alert!";
            long length = new System.IO.FileInfo(e.FullPath).Length;
            FileInfo filename= new FileInfo(@"C:\Users\k132212\Documents\MonitorThisFolder\"+e.Name);
            mail.Body = "Filename = " + e.Name + "Size = " + filename.Length;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = System.Configuration.ConfigurationManager.AppSettings["Host"];
            smtp.Credentials = new System.Net.NetworkCredential
                 (System.Configuration.ConfigurationManager.AppSettings["Username"], System.Configuration.ConfigurationManager.AppSettings["Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(mail);
            Console.WriteLine("Successfully sent");
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
        }    
    }

        protected override void OnStop()
        {
            
        }
    }
}
