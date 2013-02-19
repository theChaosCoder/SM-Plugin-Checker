using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using HtmlAgilityPack;

namespace SM_Plugin_Checker
{
    class VersionGrabber
    {
        private readonly string threadid;
        private bool hasVersionString;
        private string version;

        public VersionGrabber(string threadid)
        {
            this.threadid = threadid;
            Grab();
        }

        /// <summary>
        /// Grabs the Version String from Alliedmodders if available
        /// </summary>
        private void Grab()
        {
            HtmlWeb htmlPage = new HtmlWeb(); 
            HtmlDocument htmlDoc = htmlPage.Load("http://forums.alliedmods.net/showthread.php?t=" + threadid);

            var tr = htmlDoc.DocumentNode.SelectSingleNode("//tr[@class='alt2']");

            if (tr != null && tr.HasChildNodes)
            {
                if (tr.ChildNodes[7].HasChildNodes)
                {
                    version = tr.ChildNodes[7].ChildNodes[1].InnerText;
                    hasVersionString = true;
                }                
            }
            else
            {
                hasVersionString = false;
            }
            //Console.WriteLine("NEW " + tr.ChildNodes[5].ChildNodes[1].InnerText);
            //Console.WriteLine("NEW " + tr.ChildNodes[7].ChildNodes[1].InnerText);


           /* int j = 0;
            foreach (var c in tr.ChildNodes)
            {
                //Console.WriteLine("j " + j);
                //Console.WriteLine("cc " + c.InnerHtml);
                if (c.HasChildNodes)
                {
                    int i = 0;
                    foreach (var c2 in c.ChildNodes)
                    {
                        //if(containsPlugins)
                        if (c2.ToString().Length > 0)
                        {
                            //Console.WriteLine(i + " c2 |" + c2.InnerText);    
                        }

                        i++;
                    }

                }
                j++;
            }*/
            htmlPage = null;

        }
        public bool HasVersionString()
        {
            return this.hasVersionString;
        }
        public string GetVersion()
        {
            return version;
        }
    }
}
