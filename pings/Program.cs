using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace pings
{
    class Program
    {
        #region Librares

        [DllImport("kernel32.dll")]
        private static extern bool SetConsoleMode(IntPtr hConsoleHandle, int dwMode);

        [DllImport("kernel32.dll")]
        private static extern bool GetConsoleMode(IntPtr hConsoleHandle, out int lpMode);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        #endregion

        #region Constants

        private const int ENABLE_QUICK_EDIT = 0x0040;
        private const int STD_INPUT_HANDLE = -10;

        #endregion

        #region Variables

        private static Pinger _pinger;

        #endregion

        static async Task Main(string[] args)
        {
            SetupConsole();

            _pinger = new Pinger();
            await _pinger.Run();

            Console.ReadKey();
        }

        private static void SetupConsole()
        {
            Console.Title = "Pinger";

            IntPtr handle = GetStdHandle(STD_INPUT_HANDLE);

            int mode;

            if (!GetConsoleMode(handle, out mode))
            {
                throw new Exception("");
            }

            mode &= ~ENABLE_QUICK_EDIT;

            if (!SetConsoleMode(handle, mode))
            {
                throw new Exception("");
            }

            Console.CursorVisible = false;
            Console.CancelKeyPress += Console_CancelKeyPress;
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            if (e.Cancel)
            {
                Environment.Exit(0);
            }
        }
    }
}