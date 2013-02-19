using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;


namespace SM_Plugin_Checker
{
    class SmPluginCheck
    {
        private readonly string _rconPw;
        public readonly string Ip;
        public readonly int Port;
        private readonly string _command;
        private string[] _plugins;
        private string[] _mods;
        private string[] _exts;
        public PluginInfoParser ParsedPlugins;
        private string _step = "LIST";
        private int _count = 0;
        private int _curCount = 0;
        private string _error = "";

        //private SourceQueries.Source Rcon = new SourceQueries.Source();
        private SourceRcon Rcon = new SourceRcon();

        public SmPluginCheck(string ip, int port, string rconPw)
        {
            this.Ip = ip;
            this.Port = port;
            this._rconPw = rconPw;
            this._command = "sm plugins list";
        }
        /*  public void Condenser()
          {

              CSSRcon r = new CSSRcon();
              r.Connect(new IPEndPoint(IPAddress.Parse(_ip), _port), _rconPw);

    
              r.Errors += new StringOutput(ErrorOutput);
              r.ServerOutput += new StringOutput(ConsoleOutput);


              if (r.Connect(new IPEndPoint(IPAddress.Parse(_ip), _port), _rconPw))
              {
                  while (!r.Connected)
                  {
                      Thread.Sleep(10);
                  }
              }
              else
              {
                  Console.WriteLine("No connection!");
                  Thread.Sleep(1000);
              }
              r.ServerCommand(_command);
              Thread.Sleep(1000);
              return;
            
          } */

        /*    public void S()
            {
            
                Rcon.ServerOutput += new Source.StringEventHandler(ConsoleOutput);
                Rcon.ConnectionStatus += new Source.StringEventHandler(ConectionStatus);
            
           
                //Thread.Sleep(2000);
                //Rcon.SendCommand("sm plugins list");
                //Thread.Sleep(2000);
               // Rcon.Dispose();
               // Rcon.Disconnect();

                if (Rcon.Connect(_ip, _port, _rconPw))
                {
                    while (!Rcon.Connected)
                    {
                        Thread.Sleep(10);
                    }

                    Rcon.SendCommand("sm plugins list");
                    Thread.Sleep(100);
                    while (_step != "INFO")
                    {
                        Thread.Sleep(30);
                    }
                    getAllPlugins();
                    while (_step != "DONE")
                    {
                        Thread.Sleep(30);
                    }
                    Console.WriteLine("####### DONE ########");
                    Rcon.Disconnect();
                    Rcon.Dispose();
                }
                else
                {
                    Console.WriteLine("No connection! wOOt!?");
                }

      

            }   */

        public bool GetSmPluginList()
        {
            Rcon.Errors += new StringOutput(ErrorOutput);
            Rcon.ServerOutput += new StringOutput(ConsoleOutput);

            if (Port > 65536)
            {
                _error = "Check Your port number --- Port is > 65536";
                return false;
            }
                

            if (Rcon.Connect(new IPEndPoint(IPAddress.Parse(Ip), Port), _rconPw))
            {
                while (!Rcon.Connected)
                {
                    Thread.Sleep(10);
                    if (HasError())
                        return false;
                }

                Rcon.ServerCommand("sm plugins list");
                Thread.Sleep(50);
                while (_step != "INFO")
                {
                    Thread.Sleep(10);
                    if (HasError())
                        return false;
                }
                getAllPlugins();
                while (_step != "DONE")
                {
                    Thread.Sleep(20);
                    if (HasError())
                        return false;
                }
                Rcon = null;
                ParsedPlugins = new PluginInfoParser(_plugins);
                _plugins = null;
                Console.WriteLine("IP: "+ Ip + ":" + Port +  "   ####### DONE ########");
                if (HasError())
                    return false;
                return true;
            }
            else
            {
                Console.WriteLine("No connection! wOOt!?");
                _error = "No connection!";
                //throw new Exception("No connection2!");       
                return false;
            }

        }

        private void GetPluginCount(string p)
        {
            _count = Convert.ToInt32(p.Substring(13, 3).Replace(" ", ""));
            _plugins = new string[_count];
            //_count =  p.Split(new string[] { "\n" }, StringSplitOptions.None).Count() - 2;
#if DEBUG
            Console.WriteLine("Count: " + _count);
#endif
        }
        private void getAllPlugins()
        {
            for (int i = 0; i <= _count; i++)
            {
                Rcon.ServerCommand("sm plugins info " + i);
                Thread.Sleep(10);
            }
        }
        private void getPluginInfo(string p, int num)
        {
            string[] pieces = p.Split(new string[] { "\n" }, StringSplitOptions.None);
            Console.WriteLine(pieces.Count() - 2);
        }


        private void ErrorOutput(string data)
        {
          
            Console.WriteLine("ERROR: {0}", data);
            if (data.Contains("Connection Failed!") || data.Contains("Connection closed by remote host"))
            {
                _error = data;
                Console.WriteLine(Ip + ":" + Port + " - " + _error);
                //throw new Exception("No connection!");
            }
            
        }

        private void ConsoleOutput(string data)
        {
            /*if (!string.IsNullOrEmpty(data))
            {
                if (data.Contains("\nL"))
                {
                    data = Regex.Split(data, "\nL")[0];
                }
            }*/
            //Console.WriteLine("data: " + data);
            if (data.Contains("Unknown command"))
            {
                _error = "No SourceMod installed";
                Console.WriteLine(Ip+":"+Port + " - "+ _error);
                //throw new Exception(_error);

            }
            if (data.Contains("[SM] Listing"))
            {
               
                GetPluginCount(data);
                _step = "INFO";
            }
            if (_step == "INFO" && data.Contains("Filename"))
            {
                _plugins[_curCount++] = data;
#if DEBUG
                Console.WriteLine("cur: " + _curCount + " | " + _count);
#endif
                if (_curCount >= _count)
                    _step = "DONE";
            }

            //Console.WriteLine("CONSOLE: {0}", input);
        }
        private void ConectionStatus(string data)
        {
            Console.WriteLine("ConnectionStatus: " + data);

        }

        public int Count()
        {
            return _count;
        }
        public bool HasError()
        {
            if (_error.Length == 0)
                return false;
            else
                return true;
        }
        public string GetErrorMessage()
        {
            if (_error.Length == 0)
                return _error;
            else
                return _error;
        }

    }
}
