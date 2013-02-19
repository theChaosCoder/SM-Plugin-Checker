using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace SM_Plugin_Checker
{
    class WebCache
    {
        //Filename, Version string from alliedmodders
        public Dictionary<string,string> FileList = new Dictionary<string, string>();
        public Dictionary<string, string> alliedmodsList = new Dictionary<string, string>();

        public WebCache(string versionfile)
        {
            if (readFile(versionfile))
            {
                GetWebVersion();
                WriteCacheFile();
            }
            
        }

        private bool readFile(string file)
        {
            try
            {
                foreach (var line in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, file), Encoding.UTF8))
                {
                    if (line.Length != 0 && line[0].ToString() != "#")
                    {
                        string[] s = Regex.Split(line, "::");
                        
                        if (s.Length == 2 && s[1].Length > 2)
                        {
                           //Console.WriteLine(s[0] + " - " + s[1]);
                           alliedmodsList.Add(s[0], s[1]);
                        }
                    }
                }
                return true;
            }
            catch (FileNotFoundException ex)
            {
                //MessageBox.Show(file + " not found!");
                return false;
            }
        }
        private void GetWebVersion()
        {
            if (alliedmodsList.Count > 0)
            {
                foreach (var line in alliedmodsList)
                {
                    VersionGrabber vgrab = new VersionGrabber(line.Value);
                    if (vgrab.HasVersionString())
                    {
                        FileList.Add(line.Key, vgrab.GetVersion());
                        Console.WriteLine(vgrab.GetVersion());
                    }
                    Thread.Sleep(200); // let the website take breath
                }
                
            }
        }
        private void WriteCacheFile()
        {
            System.IO.File.WriteAllLines("webcache.txt", FileList.Select(x => x.Key + "|" + x.Value).ToArray());
        }



    }
}
