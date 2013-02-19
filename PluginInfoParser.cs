using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SM_Plugin_Checker
{
    class PluginInfoParser
    {
        private readonly int count;
        private Dictionary<string, string>[] parsed;

        public PluginInfoParser(string[] plugins)
        {
            this.count = plugins.Count();
            this.parsed = new Dictionary<string, string>[count];

            int i = 0;
            foreach (var p in plugins)
            {
                parsed[i] = parsePlugin(p);
                i++;
            }

        }
        /// <summary>
        /// Holds the info about a Plugin
        /// </summary>
        /// <param name="plugin"></param>
        /// <returns></returns>
        private Dictionary<string, string> parsePlugin(string plugin)
        {
            //Console.WriteLine("PARSE");
            string[] pieces = plugin.Split(new string[] { "\n" }, StringSplitOptions.None);
            Dictionary<string, string> dict = new Dictionary<string, string>();

            foreach (var p in pieces)
            {
                if (p.Contains("Filename:"))
                {
                    dict.Add("Filename", p.Substring(12));
                }

                if (p.Contains("Title:"))
                {
                    dict.Add("Title", p.Substring(9));
                }

                if (p.Contains("Author:"))
                {
                    dict.Add("Author", p.Substring(10));
                }

                if (p.Contains("Version:"))
                {
                    dict.Add("Version", p.Substring(11));
                }

                if (p.Contains("URL:"))
                {
                    dict.Add("URL", p.Substring(7));
                }

                if (p.Contains("Status:"))
                {
                    dict.Add("Status", p.Substring(10));
                }

                if (p.Contains("Reloads:"))
                {
                    dict.Add("Reloads", p.Substring(11));
                }

                if (p.Contains("Timestamp:"))
                {
                    dict.Add("Timestamp", p.Substring(13));
                }

                if (p.Contains("Hash:"))
                {
                    dict.Add("Hash", p.Substring(8));
                }
                if (p.Contains("Load error:"))
                {
                    dict.Add("Load error", p.Substring(14));
                }
                if (p.Contains("File info:"))
                {
                    dict.Add("File info", p.Substring(13));
                }
                if (p.Contains("File URL:"))
                {
                    dict.Add("File URL", p.Substring(12));
                }
            }

            return dict;
        }

        public string GetFilename(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("Filename"))
            {
                return parsed[index]["Filename"];
            }
            return "NOT AVAILABLE";
        }

        public string GetVersion(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("Version"))
            {
                return parsed[index]["Version"];
            }
            return "";
        }
        public string GetAuthor(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("Author"))
            {
                return parsed[index]["Author"];
            }
            return "";
        }
        public string GetTitle(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("Title"))
            {
                return parsed[index]["Title"];
            }
            return "";
        }
        public bool IsSourceMod(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("Author"))
            {
                if (parsed[index]["Author"] == "AlliedModders LLC")
                    return true;
            }
            return false;

        }

        public string GetLoadError(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("Load error"))
            {
                return parsed[index]["Load error"];
            }
            return "";
        }

        public string GetUrl(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("URL"))
            {
                return parsed[index]["URL"];
            }
            return "";
        }
        public bool IsBroken(int index)
        {
            IndexCheck(index);
            if (parsed[index].ContainsKey("Load error"))
            {
                return true;
            }
            return false;
        }
        private void IndexCheck(int index)
        {
            if (index > count - 1)
            {
                throw new System.ArgumentException("Max Index = " + (count - 1).ToString() + ". Your Index = " + index, "index");
            }
        }
    }
}
/*
  Filename: Ignore.smx
  Title: Ignore list (Provides a way to ignore other client's chat.)
  Author: FaTony + Dr. McKay
  Version: 0.95
  URL: http://fatony.com/
  Status: running
  Reloads: Map Change if Updated
  Timestamp: 01/03/2013 17:54:57
  Hash: c896c97fb25e630c688d8ba75eeacf0b
 * 
 * 
 * 
 * Load error: Native "GetUserMessageType" was not found
   File info: (title "Simple Chat Processor (Redux)") (version "1.1.4")
   File URL: http://forums.alliedmods.net

*/