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
using System.Runtime.InteropServices;

namespace BaudMate
{
    public static class ClsPowerManagement
    {
        // Use DllImport to import the SetThreadExecutionState function
        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern uint SetThreadExecutionState(uint esFlags);

        private const uint ES_CONTINUOUS = 0x80000000;
        private const uint ES_SYSTEM_REQUIRED = 0x00000001;

        // Method to prevent sleep
        public static void PreventSleep()
        {
            try
            {
                _ = SetThreadExecutionState(ES_CONTINUOUS | ES_SYSTEM_REQUIRED);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"PreventSleep(): {ex}");
            }
        }

        // Method to restore the default sleep behavior
        public static void AllowSleep()
        {
            try
            {
                _ = SetThreadExecutionState(ES_CONTINUOUS);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"AllowSleep(): {ex}");
            }
        }
    }
}
