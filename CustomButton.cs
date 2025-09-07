/*
    BaudMate
    Author: Mathias Gyldenberg (https://github.com/MathiasGyldenberg)
    License: GNU General Public License v3.0 (GPLv3)

    Disclaimer:
    BaudMate is provided as open source software under the GPLv3 license. It is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    All modifications and derivative works must also remain open source under the same license.
*/

namespace BaudMate
{
    /// <summary>
    /// Creates a custom connect/disconnect button with a corresponding color bitmap
    /// </summary>
    internal class ConnectButton : Button
    {
        internal ConnState ConnState { get; private set; }

        internal void SetConnected()
        {
            this.Image = CustomBitmap.GetIcon(Color.Red);
            this.Text = "Disconnect";
            this.ConnState = ConnState.Connected;
        }

        internal void SetDisconnected()
        {
            this.Image = CustomBitmap.GetIcon(Color.Green);
            this.Text = "Connect";
            this.ConnState = ConnState.Disconnected;
        }

        internal ConnectButton(ConnState connState)
        {
            this.ImageAlign = ContentAlignment.MiddleLeft;
            this.TextImageRelation = TextImageRelation.ImageBeforeText;
            this.ConnState = connState;

            if (connState == ConnState.Disconnected) { SetDisconnected(); }
            else { SetConnected(); }
        }
    }

    internal enum ConnState
    {
        Connected,
        Disconnected,
    }
}
