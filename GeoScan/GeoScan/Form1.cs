using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using FastSearchLibrary;
using System.Threading;
using System.Text.RegularExpressions;

namespace GeoScan
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private static object locker = new object(); 
        private void button1_Click(object sender, EventArgs e)
        {
            List<string> fileExts = new List<string>() {"?.SHP", "?.shx", "?.DBF", "?.PRJ", "?.CBN", "?.XML", "?.MIF", "?.Mid", "?.Tab", "?.KML", "?KMZ", "?.GPS", "*.tiff", "?.jpg", "?.jpeg", "?.geotiff"};
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            List<FileInfo> files = new List<FileInfo>(); // список результатов
            foreach (string fileExt in fileExts)
            {
                FileSearcher searcher = new FileSearcher(@"C:\", fileExt, tokenSource);
                searcher.FilesFound += (sndr, arg) =>
                     {
                         lock (locker)
                         {
                             Invoke((Action)(() =>
                             {
                                 arg.Files.ForEach((f) =>
                                 {
                                     textBox2.Text += "Расположение: " + f.FullName + Environment.NewLine + "Дата создания: " + f.CreationTime + Environment.NewLine + "Дата последнего изменения: " +
                                         f.LastWriteTime + Environment.NewLine;
                                     files.Add(f);
                                 });
                             }));

                         }
                     };
                searcher.SearchCompleted += (sndr, arg) =>
                {
                    if (arg.IsCanceled)
                        textBox2.Text += "Поиск остановлен" + Environment.NewLine;
                    else
                        Invoke((Action)(() =>
                            {
                                textBox2.Text += "Поиск завершён" + Environment.NewLine;
                            }));
                    ;
                    Invoke((Action)(() =>
                        {
                            int fCount = files.Count;
                            textBox2.Text += "Найдено файлов: " + fCount + Environment.NewLine;
                            fCount = 0;
                        }));
                };
                var task = searcher.StartSearchAsync();
                DriveInfo[] driveNames = DriveInfo.GetDrives();
                char[] deaf = { ':', '\\' };
                string result = null;
                string compName = Environment.UserName;
                foreach (DriveInfo driveName in driveNames)
                {
                    result = Convert.ToString(driveName);
                    result = result.Replace(":", "");
                    result = result.Replace("\\", "");
                    driveName.ToString();
                    StreamWriter file = new StreamWriter("C:\\Users\\-\\Desktop\\" + Environment.UserName + "_" + result + ".csv");
                    file.WriteLine(textBox2.Text);
                    file.Close();
                }
            }
           /* string compName = Environment.UserName;
            textBox2.Text = compName + Environment.NewLine;
            SearchFiles();
            //foreach (string file in Directory.GetFiles("C:\\", string.Format("*.{0}", fileType), SearchOption.AllDirectories))
            string dirName = "C:\\";
            string[] files = Directory.GetFiles(dirName);
            if (files.Length > 0)
            {
                textBox2.Text += "Files" + Environment.NewLine;
                foreach (string file in files)
                {
                    FileInfo fileInf = new FileInfo(file);
                    textBox2.Text += file;
                    textBox2.Text += Environment.NewLine + "Creation time" + Environment.NewLine + fileInf.CreationTime + Environment.NewLine + "Length" + Environment.NewLine + fileInf.Length +
                    Environment.NewLine + "Last write time" + Environment.NewLine + fileInf.LastWriteTime + Environment.NewLine;
                }
            }
            else textBox2.Text += "Files not found" + Environment.NewLine;
            string[] dirs = Directory.GetDirectories(dirName);
            if (dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    textBox2.Text += dir + Environment.NewLine;
                }
            }
            DirectoryInfo dirInfo = new DirectoryInfo();
            List<FileInfo> files = new List<FileInfo>();
            string[] fileTypes = new string[] { "shp", "shx", "dbf", "prj", "cbn", "xml", "MIF", "MID", "tab", "kml", "kmz", "gps", "map", "tif", "tiff", "jpg", "jpeg", "geotiff" };
            foreach (DriveInfo drive in drives)
            {
                foreach (string dir in Directory.EnumerateFiles(drive,"?.{0}", fileTypes, SearchOption.AllDirectories))
             * {
             *      DirectoryInfo dirInfo = new DirectoryInfo(dir);
             *      if ((dirInfo.Attributes & FileAttributes.Hidden)==0)
             *      {
             *          
             *      }
             * }
            }
            foreach (string subDir in Directory.GetDirectories("C:\\"))
            {
                try
                {
                    GFiles(subDir);
                }
                catch
                {
                }
            }
             if (File.Exists(dir))
             * {     
             * }
             * if (Directory.Exists(dir))
             * {
             * }
             */

        }
        /*private void SearchFiles()
        {
            List<string> dirPath = new List<string>();
            List<string> dirNames = new List<string>();
            List<string> subDirNames = new List<string>();
            string dirName = "C:\\";
            DirectoryInfo dirInfo = new DirectoryInfo(dirName);
            string[] files = Directory.GetFiles(dirName);
            if (files.Length > 0)
            {
                textBox2.Text += "Files" + Environment.NewLine;
                foreach (string file in files)
                {
                    FileInfo fileInf = new FileInfo(file);
                    textBox2.Text += file;
                    textBox2.Text += Environment.NewLine + "Creation time" + Environment.NewLine + fileInf.CreationTime + Environment.NewLine + "Length" + Environment.NewLine + fileInf.Length +
                    Environment.NewLine + "Last write time" + Environment.NewLine + fileInf.LastWriteTime + Environment.NewLine;
                }
            }
            else
            {
                textBox2.Text += "Files not found" + Environment.NewLine;
            }
            List<string> dirs = new List<string>();
            string[] dirsC = Directory.GetDirectories(dirName);
            dirs.Add(dirsC.ToString());
            if (dirs.Count > 0)
            {
                foreach (string dir in dirs)
                {
                    if ((dirInfo.Attributes & FileAttributes.Hidden) == 0)
                    {
                        dirPath.Add(dirInfo.FullName);
                        dirNames.Add(dir);
                        subDirNames.Clear();
                        dirs.Clear();
                        
                    }
                }
            }
            if (dirNames.Count > 0)
            {
                foreach (string dir in dirNames)
                {
                    if ((dirInfo.Attributes & FileAttributes.Hidden) == 0)
                    {
                        dirName =  dir;
                        dirPath.Add(dirInfo.FullName);
                        subDirNames.Add(dir);
                        dirNames.Clear();
                    }
                }
            }
            if (subDirNames.Count > 0)
            {
                foreach (string dir in subDirNames)
                {
                    if ((dirInfo.Attributes & FileAttributes.Hidden) == 0)
                    {
                        dirName = dir;
                        dirPath.Add(dirInfo.FullName);
                        dirNames.Add(dir);
                    }
                }
            }
            SearchFiles();
        }*/
    }
}
