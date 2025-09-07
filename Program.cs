/*
    BaudMate
    Author: Mathias Gyldenberg (https://github.com/MathiasGyldenberg)
    License: GNU General Public License v3.0 (GPLv3)

    Disclaimer:
    BaudMate is provided as open source software under the GPLv3 license. It is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    All modifications and derivative works must also remain open source under the same license.
*/

using System.Diagnostics;

namespace BaudMate
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]

        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.ApplicationExit += Application_ApplicationExit;
            Application.Run(new MainWin());
        }

        private static void Application_ApplicationExit(object? sender, EventArgs e)
        {
            ClsPowerManagement.AllowSleep();
            Debug.WriteLine("Application exited gracefully!");
        }
    }
}