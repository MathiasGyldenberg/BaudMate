/*
    BaudMate
    Author: Mathias Gyldenberg (https://github.com/MathiasGyldenberg)
    License: GNU General Public License v3.0 (GPLv3)

    Disclaimer:
    BaudMate is provided as open source software under the GPLv3 license. It is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    All modifications and derivative works must also remain open source under the same license.
*/

using System.IO.Ports;

namespace BaudMate
{
    [Serializable]
    public class ClsPreset
    {
        public string? PortName { get; set; }
        public int BaudRate { get; set; }
        public int DataBits { get; set; }
        public Parity Parity { get; set; }
        public StopBits StopBits { get; set; }
        public bool DtrEnable { get; set; }
        public bool RtsEnable { get; set; }
        public Handshake Handshake { get; set; }
        public int ReadTimeout { get; set; }
        public int WriteTimeout { get; set; }
        public int ReadBufferSize { get; set; }
        public int WriteBufferSize { get; set; }
        public bool DiscardNull { get; set; }
        public byte ParityReplace { get; set; }
        public bool CtsHolding { get; set; }
        public bool DsrHolding { get; set; }
        public bool CDHolding { get; set; }

        public bool ContinousSendActive { get; set; }
        public int ContinousAutoInterval { get; set; }
        public string? ContinousCommand { get; set; }
        public bool ContinousShowSendMessage { get; set; }
        public char BufferSeparationChar { get; set; }

        public bool SendMessageOnEnterKey { get; set; }
        public bool EndMessageWithSeparationChar { get; set; }

        public bool ShowTimestamp { get; set; }
        public bool AddLineEnding { get; set; }
        public bool ActivateReceiveBuffer { get; set; }
        public bool DisplayRawInput { get; set; }
        public bool DisplaySendMessage { get; set; }
        public bool DisplayResponseTime { get; set; }

        public bool TerminalWordWrap { get; set; }
        public bool SendWordWrap { get; set; }
    }
}
