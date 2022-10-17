using BetterConsoles.Tables;
using BetterConsoles.Tables.Builders;
using BetterConsoles.Tables.Configuration;
using IniParser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace pings
{
    public class Pinger
    {
        private List<Host>      _hosts;
        private List<Task>      _tasks;
        private IniDataParser   _ini;
        private Table           _table;

        public Pinger()
        {
            _ini = new IniDataParser();
            _hosts = new List<Host>();
            _tasks = new List<Task>();

            _table = new TableBuilder(TableConfig.Unicode())
                .AddColumn("Наименование").RowFormatter<string>((val) => val)
                .AddColumn("IP адрес")
                .AddColumn("Время ответа").RowFormatter<string>((val) => val)
                .AddColumn("Потери")
                .AddColumn("История").RowFormatter<string>((val) => val)
                .Build();

            try
            {
                var cfg = _ini.Parse(File.ReadAllText("config.ini"));

                foreach (var item in cfg["Hosts"])
                {
                    var host = new Host(item.Key.Trim(), item.Value.Trim());
                    _hosts.Add(host);
                    _table.AddRow(host.Name, host.IP, host.Time, host.Loss, host.History);
                }

                Console.Write(_table.ToString());
            }
            catch (FileNotFoundException)
            {
                throw new Exception("config.ini file not found!");
            }
        }

        public async Task Run()
        {
            while (true)
            {
                foreach (var host in _hosts)
                {
                    _tasks.Add(host.Update());
                    _tasks.Add(Task.Delay(1000));
                }

                await Task.WhenAll(_tasks);
                _tasks.Clear();

                for (int i = 0; i < _hosts.Count; i++)
                {
                    _table.Rows[i][0] = _hosts[i].Name;
                    _table.Rows[i][2] = _hosts[i].Time;
                    _table.Rows[i][3] = _hosts[i].Loss;
                    _table.Rows[i][4] = _hosts[i].History;
                }

                Console.SetCursorPosition(0, 0);
                Console.Write(_table.ToString());
            }
        }
    }
}
