using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Hosting;

namespace DAL.Logger
{
    public class Logger : ILogger
    {
        private readonly string _filePath;
        public Logger()
        {
            string rootDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _filePath = Path.Combine(rootDirectory, "Log/log_MVC.txt");
        }

        public void Log(Exception exception)
        {
            if (File.Exists(_filePath)) { Write(exception); }
            else { File.Create(_filePath); Write(exception); }
        }
        private void Write(Exception exception)
        {
            using (StreamWriter streamWriter = new StreamWriter(_filePath, true))
            {
                streamWriter.WriteLine($"\n__________{DateTime.Now}__________");
                streamWriter.WriteLine(exception.Message);
                streamWriter.WriteLine(exception.InnerException);
                streamWriter.WriteLine(exception.StackTrace);
                streamWriter.WriteLine(exception.Source);
                streamWriter.Flush();
            }
        }
    }
}
