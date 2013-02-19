using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Input;
using Application = System.Windows.Application;
using MessageBox = System.Windows.MessageBox;

namespace SM_Plugin_Checker
{


    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private List<SmPluginCheck> myPlugins;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private List<string> errors = new List<string>();
        private Dictionary<string, string> cache = new Dictionary<string, string>(); //filename, version
        private Dictionary<string, string> updateList = new Dictionary<string, string>(); //filename, threadid
        private bool checking = false;
        public enum LinkType
        {
            SourceMod,
            MetaMod,
            AlliedModdersForum
        }

        public MainWindow()
        {
            InitializeComponent();
            Webbrowser.NavigateToString(htmlpage.Header + htmlpage.Footer);
            LoadCache();
            LoadUpdateList();
        }

        private void LoadUpdateList()
        {
            try
            {
                foreach (var line in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "alliedmods.txt"), Encoding.UTF8))
                {
                    if (line.Length != 0 && line[0].ToString() != "#")
                    {
                        string[] s = Regex.Split(line, "::");

                        if (s.Length == 2 && s[1].Length > 2)
                        {
                            updateList.Add(s[0], s[1]);
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                Console.WriteLine("alliedmods.txt not found... " + ex.ToString());
            }
        }

        private void LoadCache()
        {
            try
            {
                foreach (var line in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "webcache.txt"), Encoding.UTF8))
                {
                    if (line.Length > 4)
                    {
                        string[] s = line.Split('|');

                        if (s.Length == 2 && s[1].Length > 2)
                        {
                            cache.Add(s[0], s[1]);
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                //MessageBox.Show("webcache.txt not found!");
                Console.WriteLine("webcache.txt not found...");
            }
        }


        private void Update_Click(object sender, RoutedEventArgs e)
        {

            /* FlowViewer.MaxZoom = 300;
             FlowViewer.MinZoom = 50;
            
            
             int cols = 40;
             int rows = 40;

             for (int c = 0; c < cols; c++)
             {
                 var x = new TableColumn();
                 x.Width = new GridLength(100);
                  myTable.Columns.Add(x);
             }
               
             for (int r = 0; r < rows; r++)
             {
                TableRow tr = new TableRow();

                for (int c = 0; c < cols; c++)
                {
                    tr.Cells.Add(new TableCell(new Paragraph(new Run("Some Text") { Foreground = Brushes.LimeGreen })) { Background = Brushes.MidnightBlue });
                }
                   

                TableRowGroup trg = new TableRowGroup();
                trg.Rows.Add(tr);
                myTable.RowGroups.Add(trg);
             }
           
             */


            //meta list
            //meta info 1
            //sm plugins list
            //sm plugins info 28
            //sm exts list
            //sm exts info 1
            //
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {

            Webbrowser.NavigateToString(htmlpage.progressbar(e.ProgressPercentage));
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {

            Stopwatch sw = new Stopwatch();
            sw.Start();

            Webbrowser.NavigateToString(BuildHtmlContents(myPlugins, errors));
            checking = false;

            sw.Stop();
            Console.WriteLine("gen HTML={0}", sw.Elapsed);
            this.backgroundWorker1.Dispose();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            List<string[]> serverInfo = GetServerInfo();
            errors.Clear();

            if (!(serverInfo.Count > 0))
                errors.Add("No server to check. Add one to server.txt (IP:PORT,RCON_PW)");

            myPlugins = new List<SmPluginCheck>();
            this.backgroundWorker1.ReportProgress(0);
            foreach (var serv in serverInfo)
            {
                Stopwatch sd = new Stopwatch();
                sd.Start();
                SmPluginCheck sm = new SmPluginCheck(serv[0], Int32.Parse(serv[1]), serv[2]);


                if (sm.GetSmPluginList())
                {

                    myPlugins.Add(sm);
                    //myPlugins[i] = sm;
                }
                else
                {
                    errors.Add(serv[0] + ":" + serv[1] + " ERROR: " + sm.GetErrorMessage());
                }


                sd.Stop();
                Console.WriteLine("Elapsed={0}", sd.Elapsed);
                Console.WriteLine((myPlugins.Count * 100 / serverInfo.Count));
                this.backgroundWorker1.ReportProgress(myPlugins.Count * 100 / serverInfo.Count);
            }
        }

        private List<string[]> GetServerInfo()
        {
            //"ip:port,rcon_password"
            List<string[]> info = new List<string[]>();

            try
            {
                foreach (var line in File.ReadAllLines(Path.Combine(Environment.CurrentDirectory, "server.txt"), Encoding.UTF8))
                {
                    if (line.Length != 0 && line[0].ToString() != "#")
                    {
                        string[] s = line.Split(':');
                        string[] s2 = s[1].Split(',');

                        string[] splitted = { s[0], s2[0], s2[1] };
                        if (splitted.Length == 3)
                        {
                            info.Add(splitted);
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                MessageBox.Show("Server.txt not found!");
            }

            return info;
        }


        private string BuildHtmlContents(List<SmPluginCheck> serverplugins, List<string> e)
        {
            string er = "";


            if (e.Count > 0)
            {
                er += "<div class='orange'>";
                foreach (var error in e)
                {
                    er += "<p>" + error + "</p>";
                }
                if (serverplugins.Count == 0)
                {
                    er += "<h1>Nothing to display :(</h1></div>";
                    return htmlpage.Header + er + htmlpage.Footer;
                }
                er += "</div>";

            }

            //ToDo look at HtmlDocument

            string table = "<table border='0'><thead id='head'><tr>";
            List<string> list = new List<string>();



            //Build List of all known filenames and sort it
            foreach (var plugins in serverplugins)
            {
                for (int i = 0; i <= plugins.Count() - 1; i++)
                {
                    if (!list.Contains(plugins.ParsedPlugins.GetFilename(i)))
                    {
                        list.Add(plugins.ParsedPlugins.GetFilename(i));
                        //Console.WriteLine(plugins.ParsedPlugins.GetFilename(i) + " | " + plugins.ParsedPlugins.GetVersion(i) + " | " + plugins.ParsedPlugins.IsSourceMod(i));
                    }
                }
            }
            foreach (var l in list)
            {
                //Console.WriteLine(l);
            }
            list.Sort();


            //Table header (Server 1, Server 2, Server 3 ...)
            table += "<th>smx Files</th>";
            foreach (var entry in serverplugins)
            {
                table += "<th style='font-size: 7pt; font-weight:normal;'>" + entry.Ip + ":" + entry.Port + "</th>";
            }
            table += "</tr></thead><tbody id='data'>";


            //Find the version number for the "Filename" per line
            //sample.smx | 1.3 | 1.2 | --- | 
            //mark highest version number green, else red
            int listcounter = 0;

            foreach (var filename in list)
            {
                //get newest version
                string ver = cache.ContainsKey(filename) ? cache[filename] : GetHighestVersionNumber(filename, serverplugins);


                //Filename
                table += "<tr>";
                if (updateList.ContainsKey(filename) && cache.ContainsKey(filename))
                {
                    //MessageBox.Show(cache[filename]);
                    table += "<td class='rightalign'><a href='" + filename + "'>" + filename + " [" + cache[filename] + "]</a></td>";
                }
                else
                {
                    table += "<td class='rightalign'>" + filename + "</td>";
                }

                listcounter++;



                //Table Cell: Version numbers of Filename in each Server
                foreach (var plugins in serverplugins)
                {

                    bool added = false;
                    for (int i = 0; i <= plugins.Count() - 1; i++)
                    {
                        if (plugins.ParsedPlugins.GetFilename(i) == filename)
                        {
                            if (plugins.ParsedPlugins.IsBroken(i))
                            {
                                table += "<td class='orange hasTooltip'>" + plugins.ParsedPlugins.GetLoadError(i) + "</td>";
                            }
                            else
                            {
                                if (ver == plugins.ParsedPlugins.GetVersion(i))
                                {
                                    table += "<td class='green hasTooltip'>" + plugins.ParsedPlugins.GetVersion(i) + "<span><div style='font-weight:bold'>Filename: " + plugins.ParsedPlugins.GetFilename(i) + "</div><br>Title: " + plugins.ParsedPlugins.GetTitle(i) + "<br>Author: " + plugins.ParsedPlugins.GetAuthor(i) + "<br>URL: " + plugins.ParsedPlugins.GetUrl(i) + "</span></td>";
                                }
                                else
                                {
                                    table += "<td class='red hasTooltip'>" + plugins.ParsedPlugins.GetVersion(i) + "<span><div style='font-weight:bold'>Filename: " + plugins.ParsedPlugins.GetFilename(i) + "</div><br>Title: " + plugins.ParsedPlugins.GetTitle(i) + "<br>Author: " + plugins.ParsedPlugins.GetAuthor(i) + "<br>URL: " + plugins.ParsedPlugins.GetUrl(i) + "</span></td>";
                                }
                            }
                            added = true;
                        }
                    }
                    //Empty Cell
                    if (!added)
                        table += "<td></td>";

                }
                table += "</tr>";
            }



            table += "</tbody></table>";
            return htmlpage.Header + er + table + htmlpage.Footer;
        }

        private string GetHighestVersionNumber(string filename, List<SmPluginCheck> serverplugins)
        {
            string maxversion = "";
            foreach (var plugins in serverplugins)
            {
                for (int i = 0; i <= plugins.Count() - 1; i++)
                {
                    if (plugins.ParsedPlugins.GetFilename(i) == filename)
                    {
                        if (!YepiUtils.ProgramVersionGreater(maxversion, plugins.ParsedPlugins.GetVersion(i)) && !plugins.ParsedPlugins.IsBroken(i))
                        {
                            maxversion = plugins.ParsedPlugins.GetVersion(i);
                        }

                    }
                }
            }
            return maxversion;
        }

        private void Rectangle_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void rect2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //this.MaxHeight = SystemParameters.MaximizedPrimaryScreenHeight;
            //this.MaxWidth = SystemParameters.MaximizedPrimaryScreenWidth - 16;
            this.WindowState = WindowState.Maximized;

        }

        private void UpdateButton_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (!checking)
            {
                checking = true;
                UpdateButton.Foreground = Brushes.White;
                UpdateButton.Style = (Style)FindResource("TextBlock_clicked");

                this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
                this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
                this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
                this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);

                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.RunWorkerAsync();
            }
            else
            {
                MessageBox.Show("I'm busy mkay?");
            }
        }

        private void Webbrowser_Navigating(object sender, System.Windows.Navigation.NavigatingCancelEventArgs e)
        {
            if (e.Uri != null)
            {
                if (updateList.ContainsKey(e.Uri.AbsolutePath))
                {
                    Process.Start("http://forums.alliedmods.net/showthread.php?t=" + updateList[e.Uri.AbsolutePath]);
                }
                e.Cancel = true;
            }

        }

        private void Webbrowser_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {
            //MessageBox.Show(e.Uri.AbsoluteUri);
        }

        private void UpdateCache_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //TODO Progressbar + backgroundworker
            MessageBoxResult result = MessageBox.Show("Update? It will take some time, no Progress will be shown :(", "Update WebCache?", MessageBoxButton.YesNo);
            if (result == MessageBoxResult.Yes)
            {
                WebCache w = new WebCache("alliedmods.txt");
                cache = w.FileList;
                MessageBox.Show("Finished :)");
            }
        }




    }
}
