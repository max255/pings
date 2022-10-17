using BetterConsoles.Colors.Extensions;
using System;
using System.Drawing;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace pings
{
    public class Host
    {
        #region Constants

        private const long HISTORY_LENGTH = 64;
        private const long LOSS_BUFFER_LENGTH = 3600;

        #endregion

        #region Variables

        private string      _name;
        private IPAddress   _ip;
        private bool        _aviable;
        private long        _time;
        private long        _gain_count;
        private long        _loss_count;
        private long        _ping_count;
        private string[]    _history;
        private string[]    _history_swap;
        private string      _bar;
        private Ping        _ping;

        #endregion

        #region Properties

        public string Name
        {
            get {
                string name = "";

                if (_aviable)
                {
                    name = _name;
                }
                else
                {
                    name = _name.ForegroundColor(Color.Red);
                }

                return name;
            }
        }

        public string IP
        {
            get { 
                return _ip.ToString(); 
            }
        }

        public bool Aviable
        {
            get {
                return _aviable;
            }
        }

        public string Time
        {
            get {
                string time = "";

                if (_time < 1)
                {
                    time = "<".ForegroundColor(Color.Green) + "   1 " + Multilanguage.Ms;
                }
                else if (_time > 1000)
                {
                    time = "    -".ForegroundColor(Color.Red);
                }
                else
                {
                    time = string.Format("{0,5:#} " + Multilanguage.Ms, _time);
                }

                return time;
            }
        }

        public string Loss
        {
            get
            {
                string loss = "";

                if (_ping_count == 0)
                {
                    loss = "  -";
                }
                else
                {
                    loss = string.Format("{0,4:#0} %", (_loss_count / (double)_ping_count) * 100);
                }
                
                return loss;
            }
        }

        public string History
        {
            get {
                string colored = "";

                foreach (var c in _history)
                {
                    colored = colored + c;
                }

                return colored;
            }        
        }

        #endregion

        public Host(string name, string ip)
        {
            _name = name;

            try
            {
                _ip = IPAddress.Parse(ip);
            }
            catch (Exception)
            {
                throw new Exception("Unable to recognize IP address! Check the syntax.");
            }

            _ping = new Ping();
            _time = 1001;
            _aviable = false;

            _history = new string[HISTORY_LENGTH];
            _history_swap = new string[HISTORY_LENGTH];

            for (int i = 0; i < _history.Length; i++)
            {
                _history[i] = " ";
            }

            _history.CopyTo(_history_swap, 0);

            _gain_count = 0;
            _loss_count = 0;
            _ping_count = 0;
        }

        public async Task<Host> Update()
        {
            var result = await _ping.SendPingAsync(_ip, 1000);

            if (result.Status == IPStatus.Success)
            {
                if (_time < 10)
                {
                    _bar = "█".ForegroundColor(Color.Green);
                } 
                else if (_time < 40)
                {
                    _bar = "▓".ForegroundColor(Color.DarkGreen);
                }
                else if (_time < 100)
                {
                    _bar = "▒".ForegroundColor(Color.YellowGreen);
                }
                else
                {
                    _bar = "░".ForegroundColor(Color.Yellow);
                }

                _time = result.RoundtripTime;
                _aviable = true;

                ProcessGain();
            }
            else
            {
                _bar = "X".ForegroundColor(Color.Red);

                _time = 1001;
                _aviable = false;

                ProcessLoss();
            }

            Array.Copy(_history, 0, _history_swap, 1, _history.Length - 1);
            _history_swap[0] = _bar;
            _history = _history_swap;

            if (_ping_count < LOSS_BUFFER_LENGTH)
            {
                _ping_count++;
            }

            return this;
        }

        private void ProcessGain()
        {
            if (_gain_count + _loss_count < LOSS_BUFFER_LENGTH)
            {
                _gain_count++;
            }
            else
            {
                if (_gain_count < LOSS_BUFFER_LENGTH)
                {
                    _gain_count++;
                }

                if (_loss_count > 0)
                {
                    _loss_count--;
                }
            }
        }

        private void ProcessLoss()
        {
            if (_gain_count + _loss_count < LOSS_BUFFER_LENGTH)
            {
                _loss_count++;
            }
            else
            {
                if (_loss_count < LOSS_BUFFER_LENGTH)
                {
                    _loss_count++;
                }

                if (_gain_count > 0)
                {
                    _gain_count--;
                }
            }
        }
    }
}
