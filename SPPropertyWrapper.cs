/*
    BaudMate
    Author: Mathias Gyldenberg (https://github.com/MathiasGyldenberg)
    License: GNU General Public License v3.0 (GPLv3)

    Disclaimer:
    BaudMate is provided as open source software under the GPLv3 license. It is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    All modifications and derivative works must also remain open source under the same license.
*/

using System.ComponentModel;
using System.IO.Ports;
using System.Text;

namespace BaudMate
{
    /// <summary>
    /// Wrapper class for displaying only editable properties in the propertygrid.
    /// </summary>
    internal class SPPropertyWrapper(SerialPort serialPort, MainWin senderWin)
    {
        private readonly SerialPort _serialPort = serialPort;
        private readonly MainWin _senderWin = senderWin;

        [Category("COM settings")]
        [Browsable(true)]
        public string PortName
        {
            get => _serialPort.PortName;
            set
            {
                try
                {
                    _serialPort.PortName = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for PortName.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM settings")]
        [Browsable(true)]
        public int BaudRate
        {
            get => _serialPort.BaudRate;
            set
            {
                try
                {
                    _serialPort.BaudRate = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for BaudRate.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM settings")]
        [Browsable(true)]
        public Parity Parity
        {
            get => _serialPort.Parity;
            set
            {
                try
                {
                    _serialPort.Parity = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for Parity.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM settings")]
        [Browsable(true)]
        public int DataBits
        {
            get => _serialPort.DataBits;
            set
            {
                try
                {
                    _serialPort.DataBits = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for DataBits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM settings")]
        [Browsable(true)]
        public StopBits StopBits
        {
            get => _serialPort.StopBits;
            set
            {
                try
                {
                    _serialPort.StopBits = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for StopBits.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM settings")]
        [Browsable(true)]
        public Handshake Handshake
        {
            get => _serialPort.Handshake;
            set
            {
                try
                {
                    _serialPort.Handshake = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for Handshake.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM settings")]
        [Browsable(true)]
        public bool DtrEnable
        {
            get => _serialPort.DtrEnable;
            set
            {
                try
                {
                    _serialPort.DtrEnable = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for DtrEnable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM settings")]
        [Browsable(true)]
        public bool RtsEnable
        {
            get => _serialPort.RtsEnable;
            set
            {
                try
                {
                    _serialPort.RtsEnable = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for RtsEnable.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM Timeouts")]
        [Browsable(true)]
        public int ReadTimeout
        {
            get => _serialPort.ReadTimeout;
            set
            {
                try
                {
                    _serialPort.ReadTimeout = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for ReadTimeout.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM Timeouts")]
        [Browsable(true)]
        public int WriteTimeout
        {
            get => _serialPort.WriteTimeout;
            set
            {
                try
                {
                    _serialPort.WriteTimeout = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for WriteTimeout.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("Miscellaneous")]
        [Browsable(false)]
        public string NewLine
        {
            get => _serialPort.NewLine;
            set
            {
                try
                {
                    _serialPort.NewLine = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for NewLine.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("Miscellaneous")]
        [Browsable(false)]
        public Encoding Encoding
        {
            get => _serialPort.Encoding;
            set => _serialPort.Encoding = value;
        }

        [Category("Miscellaneous")]
        [Browsable(false)]
        public bool IsOpen => _serialPort.IsOpen;

        [Category("Miscellaneous")]
        [Browsable(false)]
        public Stream BaseStream => _serialPort.BaseStream;

        [Category("Miscellaneous")]
        [Browsable(false)]
        public bool DiscardNull => _serialPort.DiscardNull;

        [Category("Miscellaneous")]
        [Browsable(false)]
        public int ReceivedBytesThreshold
        {
            get => _serialPort.ReceivedBytesThreshold;
            set => _serialPort.ReceivedBytesThreshold = value;
        }

        [Category("Miscellaneous")]
        [Browsable(false)]
        public int BytesToRead => _serialPort.BytesToRead;

        [Category("Miscellaneous")]
        [Browsable(false)]
        public int BytesToWrite => _serialPort.BytesToWrite;

        [Category("COM Buffers")]
        [Browsable(true)]
        public int ReadBufferSize
        {
            get => _serialPort.ReadBufferSize;
            set
            {
                try
                {
                    _serialPort.ReadBufferSize = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for ReadBufferSize.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("COM Buffers")]
        [Browsable(true)]
        public int WriteBufferSize
        {
            get => _serialPort.WriteBufferSize;
            set
            {
                try
                {
                    _serialPort.WriteBufferSize = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for WriteBufferSize.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("Terminal Window")]
        [Browsable(true)]
        public bool TerminalWordWrap
        {
            get => _senderWin.ObjTextResponse.WordWrap;
            set => _senderWin.ObjTextResponse.WordWrap = value;
        }

        [Category("Send Window")]
        [Browsable(true)]
        public bool SendWordWrap
        {
            get => _senderWin.ObjTextSend.WordWrap;
            set => _senderWin.ObjTextSend.WordWrap = value;
        }

        [Category("Send Window")]
        [Browsable(true)]
        public bool AutoSend
        {
            get => _senderWin._ContinousSendActive;
            set => _senderWin._ContinousSendActive = value;
        }

        [Category("Send Window")]
        [Browsable(true)]
        public int AutoIntervalMs
        {
            get => _senderWin._msToWait;
            set
            {
                if (!double.TryParse(value.ToString(), out _))
                {
                    MessageBox.Show("AutoIntervalMs must be a valid integer.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _senderWin._msToWait = 1000;
                    return;
                }
                if (value < 1)
                {
                    MessageBox.Show("AutoIntervalMs must be at least 1 ms.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    _senderWin._msToWait = 1000;
                }
                else
                {
                    _senderWin._msToWait = value;
                }
            }
        }

        [Category("Send Window")]
        [Browsable(true)]

        public string AutoSendString
        {
            get => _senderWin._ContinousSendCommand;
            set
            {
                try
                {
                    _senderWin._ContinousSendCommand = value;
                }
                catch
                {
                    MessageBox.Show("Invalid value for AutoCommandString.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [Category("Send Window")]
        [Browsable(true)]

        public bool ShowSendMessage
        {
            get => _senderWin._ShowSendMessage;
            set => _senderWin._ShowSendMessage = value;
        }

        [Category("Send Window")]
        [Browsable(true)]
        public bool SendMessageWithEnterKey
        {
            get => _senderWin._SendOnEnter;
            set => _senderWin._SendOnEnter = value;
        }

        [Category("Send Window")]
        [Browsable(true)]
        public bool EndMessageWithSeparationChar
        {
            get => _senderWin._EndWithSepChar;
            set => _senderWin._EndWithSepChar = value;
        }

        [Category("COM Input buffer")]
        [Browsable(true)]
        public string BufferSeparationChar
        {
            get
            {
                // Show as escaped string (e.g., "\n" for newline)
                return MainWin.EscapeSpecialCharacters($"{_senderWin._BufferSeparationChar}");
            }
            set
            {
                // Convert from escaped string to char (e.g., "\n" => '\n')
                string converted = MainWin.ConvertToEscapeChars(value);
                if (!string.IsNullOrEmpty(converted))
                {
                    _senderWin._BufferSeparationChar = converted[0];
                }
                else
                {
                    _senderWin._BufferSeparationChar = '\n'; // fallback
                }
            }
        }
    }
}