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
using System.IO.Ports;
using System.Text;
using System.Xml.Serialization;

namespace BaudMate
{
    internal partial class MainWin : Form
    {
        internal const string _MainTitle = "BaudMate";
        private const string _PortEmpty = "(empty)";

        // Flags for COM port
        private bool _AwaitingDiscardInBuffer = false;
        private bool _IsConnected = false;
        internal int _msToWait = 1000;
        internal bool _ContinousSendActive = false;
        internal string _ContinousSendCommand = "A";
        internal bool _ShowSendMessage = false;
        internal char _BufferSeparationChar = '\n';

        // Timestamps for message handling
        DateTime? _DTSend = null;
        DateTime? _DTReceive = null;

        // Settings for send window behavior
        internal bool _SendOnEnter = true;
        internal bool _EndWithSepChar = true;
        
        private string @_saveFileName = string.Empty;
        private readonly Stopwatch _SWSendTimer = new();

        // Wrapper for properties window
        private readonly SPPropertyWrapper _SPPropertyWrapper;

        // Serial port configuration
        private readonly SerialPort _SP = new()
        {
            Encoding = Encoding.ASCII,
            BaudRate = 115200
        };

        private CancellationTokenSource _CTS = new();
        private readonly StringBuilder _incomingBuffer = new(256);

        private readonly ToolStripMenuItem _TerminalMenuItem = new("Terminal");

        private readonly ToolStripMenuItem _AddLineEndingMenuItem = new("Add Line-ending") { Checked = true, CheckOnClick = true };
        private readonly ToolStripMenuItem _ActivateReceiveBufferMenuItem = new("Activate Receive Buffer") { Checked = true, CheckOnClick = true };
        private readonly ToolStripMenuItem _ShowTimestampMenuItem = new("Show Timestamp") { Checked = true, CheckOnClick = true };
        private readonly ToolStripMenuItem _DisplayRawInputMenuItem = new("Show Raw Input") { CheckOnClick = true };
        private readonly ToolStripMenuItem _DisplaySendMessageMenuItem = new("Show Send Message") { Checked = true, CheckOnClick = true };
        private readonly ToolStripMenuItem _DisplayResponseTimeMenuItem = new("Show Response Time") { Checked = false, CheckOnClick = true };
        private readonly ToolStripMenuItem _SnapshotMenuItem = new("Take snapshot") { ShortcutKeys = Keys.Control | Keys.N };
        private readonly ToolStripMenuItem _SaveMenuItem = new("Save terminal window") { ShortcutKeys = Keys.Control | Keys.S };
        private readonly ToolStripMenuItem _ClearMenuItem = new("Clear") { ShortcutKeys = Keys.Shift | Keys.Delete };

        private readonly ToolStripMenuItem _PortMenuItem = new("Port");
        private readonly ToolStripMenuItem _DiscardReceiveBufferMenuItem = new("Discard Receive Buffer");
        private readonly ToolStripMenuItem _DiscardTransmitBufferMenuItem = new("Discard Transmit Buffer");

        private readonly ToolStripMenuItem _PresetMenuItem = new("Preset");
        private readonly ToolStripMenuItem _PresetLoadMenuItem = new("Load preset");
        private readonly ToolStripMenuItem _PresetSaveMenuItem = new("Save preset");

        private readonly ComboBox _ToolPortName = new() { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly ConnectButton _ToolConnect = new(ConnState.Disconnected);

        private readonly ToolStripControlHost _ToolHostPortName;
        private readonly ToolStripControlHost _ToolHostConnect;

        internal static void SendErrorMsg(string errorMessage)
        {
            MessageBox.Show(errorMessage, _MainTitle, MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        internal MainWin()
        {
            InitializeComponent();

            this.Icon = PrgResource.MainIcon;

            _SPPropertyWrapper = new(_SP, this);

            _ToolHostPortName = new(_ToolPortName) { Padding = new(10, 0, 0, 0) };
            _ToolHostConnect = new(_ToolConnect);
            _ToolPortName.DropDown += ToolPortName_DropDown;
            _ToolPortName.SelectionChangeCommitted += ToolPortName_SelectionChangeCommitted;

            _SaveMenuItem.Click += SaveMenuItem_Click;

            _TerminalMenuItem.DropDownItems.Add(_AddLineEndingMenuItem);
            _TerminalMenuItem.DropDownItems.Add(_ActivateReceiveBufferMenuItem);
            _TerminalMenuItem.DropDownItems.Add(new ToolStripSeparator());
            _TerminalMenuItem.DropDownItems.Add(_ShowTimestampMenuItem);
            _TerminalMenuItem.DropDownItems.Add(_DisplayRawInputMenuItem);
            _TerminalMenuItem.DropDownItems.Add(_DisplaySendMessageMenuItem);
            _TerminalMenuItem.DropDownItems.Add(_DisplayResponseTimeMenuItem);
            _TerminalMenuItem.DropDownItems.Add(new ToolStripSeparator());
            _TerminalMenuItem.DropDownItems.Add(_SnapshotMenuItem);
            _TerminalMenuItem.DropDownItems.Add(_SaveMenuItem);
            _TerminalMenuItem.DropDownItems.Add(_ClearMenuItem);

            _ClearMenuItem.Click += ClearMenuItem_Click;

            _PortMenuItem.DropDownItems.Add(_DiscardReceiveBufferMenuItem);
            _PortMenuItem.DropDownItems.Add(_DiscardTransmitBufferMenuItem);

            _DiscardReceiveBufferMenuItem.Click += DiscardReceiveBufferMenuItem_Click;
            _DiscardTransmitBufferMenuItem.Click += DiscardTransmitBufferMenuItem_Click;

            _PresetMenuItem.DropDownItems.Add(_PresetLoadMenuItem);
            _PresetMenuItem.DropDownItems.Add(_PresetSaveMenuItem);

            _PresetLoadMenuItem.Click += PresetLoadMenuItem_Click;
            _PresetSaveMenuItem.Click += PresetSaveMenuItem_Click;

            _SnapshotMenuItem.Click += (s, e) =>
            {
                FrmSnapshot frmSnapshot = new()
                {
                    Icon = this.Icon,
                    Text = $"{MainWin._MainTitle} ({_SP.PortName}): {DateTime.Now}"
                };
                frmSnapshot.ObjText.Font = ObjTextResponse.Font;
                frmSnapshot.ObjText.BackColor = ObjTextResponse.BackColor;
                frmSnapshot.ObjText.ForeColor = ObjTextResponse.ForeColor;
                frmSnapshot.ObjText.Lines = ObjTextResponse.Lines;
                frmSnapshot.Show();
            };

            _ToolConnect.Click += ToolConnect_Click;

            ObjTool.Items.Add(_TerminalMenuItem);
            ObjTool.Items.Add(_PortMenuItem);
            ObjTool.Items.Add(_PresetMenuItem);
            ObjTool.Items.Add(new ToolStripSeparator());
            ObjTool.Items.Add(_ToolHostPortName);
            ObjTool.Items.Add(_ToolHostConnect);

            UpdatePortNamesInComboBox();

            // Set serial port's PortName to first available port in combobox (if any)
            if (_ToolPortName.Items.Count >= 1)
            {
                _ToolPortName.SelectedIndex = 0;
                if (!string.IsNullOrEmpty(_ToolPortName.Text) && _ToolPortName.Text != _PortEmpty)
                {
                    _SP.PortName = _ToolPortName.Text;
                }
            }

            IsConnected(false);
        }

        private void PresetLoadMenuItem_Click(object? sender, EventArgs e)
        {
            try
            {
                using OpenFileDialog ofd = new()
                {
                    Filter = $"Preset file (.bmft)|*.bmft",
                    FilterIndex = 0
                };
                DialogResult dOk = ofd.ShowDialog();

                if (dOk != DialogResult.OK) { return; }

                string fileName = @ofd.FileName;
                if (!File.Exists(fileName))
                {
                    SendErrorMsg($"File '{fileName}' not found!");
                    return;
                }

                // Load preset
                ClsPreset? preset = LoadFromXml<ClsPreset>(ofd.FileName);

                if (preset == null)
                {
                    SendErrorMsg($"Selected file '{fileName}' is not in correct format!");
                    return;
                }

                _SP.PortName = preset.PortName ?? "COM1";
                _SP.BaudRate = preset.BaudRate;
                _SP.DataBits = preset.DataBits;
                _SP.Parity = preset.Parity;
                _SP.StopBits = preset.StopBits;
                _SP.DtrEnable = preset.DtrEnable;
                _SP.RtsEnable = preset.RtsEnable;
                _SP.Handshake = preset.Handshake;
                _SP.ReadTimeout = preset.ReadTimeout;
                _SP.WriteTimeout = preset.WriteTimeout;
                _SP.ReadBufferSize = preset.ReadBufferSize;
                _SP.WriteBufferSize = preset.WriteBufferSize;
                _SP.DiscardNull = preset.DiscardNull;
                _SP.ParityReplace = preset.ParityReplace;

                _msToWait = preset.ContinousAutoInterval;

                _ShowSendMessage = preset.ContinousShowSendMessage;
                _ContinousSendActive = preset.ContinousSendActive;
                _ContinousSendCommand = preset.ContinousCommand ?? string.Empty;

                // Read char with fallback to '\n' if invalid
                try
                {
                    _BufferSeparationChar = preset.BufferSeparationChar;
                }
                catch
                {
                    _BufferSeparationChar = '\n';
                }

                _SendOnEnter = preset.SendMessageOnEnterKey;
                _EndWithSepChar = preset.EndMessageWithSeparationChar;

                _ShowTimestampMenuItem.Checked = preset.ShowTimestamp;
                _AddLineEndingMenuItem.Checked = preset.AddLineEnding;
                _ActivateReceiveBufferMenuItem.Checked = preset.ActivateReceiveBuffer;
                _DisplayRawInputMenuItem.Checked = preset.DisplayRawInput;
                _DisplaySendMessageMenuItem.Checked = preset.DisplaySendMessage;
                _DisplayResponseTimeMenuItem.Checked = preset.DisplayResponseTime;

                ObjTextResponse.WordWrap = preset.TerminalWordWrap;
                ObjTextSend.WordWrap = preset.SendWordWrap;

                ObjProperties.SelectedObject = _SPPropertyWrapper;
            }
            catch (Exception ex)
            {
                SendErrorMsg($"Error loading preset:\n{ex.Message}");
            }
        }

        private void PresetSaveMenuItem_Click(object? sender, EventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new()
                {
                    Filter = $"Preset file (.bmft)|*.bmft",
                    FilterIndex = 0
                };

                DialogResult dOk = sfd.ShowDialog();

                if (dOk != DialogResult.OK) { return; }

                ClsPreset preset = new()
                {
                    PortName = _SP.PortName,
                    BaudRate = _SP.BaudRate,
                    DataBits = _SP.DataBits,
                    Parity = _SP.Parity,
                    StopBits = _SP.StopBits,
                    DtrEnable = _SP.DtrEnable,
                    RtsEnable = _SP.RtsEnable,
                    Handshake = _SP.Handshake,
                    ReadTimeout = _SP.ReadTimeout,
                    WriteTimeout = _SP.WriteTimeout,
                    ReadBufferSize = _SP.ReadBufferSize,
                    WriteBufferSize = _SP.WriteBufferSize,
                    DiscardNull = _SP.DiscardNull,
                    ParityReplace = _SP.ParityReplace,
                    ContinousAutoInterval = _msToWait,

                    ContinousShowSendMessage = _ShowSendMessage,
                    ContinousSendActive = _ContinousSendActive,
                    ContinousCommand = @_ContinousSendCommand,

                    BufferSeparationChar = _BufferSeparationChar,

                    SendMessageOnEnterKey = _SendOnEnter,
                    EndMessageWithSeparationChar = _EndWithSepChar,

                    ShowTimestamp = _ShowTimestampMenuItem.Checked,
                    AddLineEnding = _AddLineEndingMenuItem.Checked,
                    ActivateReceiveBuffer = _ActivateReceiveBufferMenuItem.Checked,
                    DisplayRawInput = _DisplayRawInputMenuItem.Checked,
                    DisplaySendMessage = _DisplaySendMessageMenuItem.Checked,
                    DisplayResponseTime = _DisplayResponseTimeMenuItem.Checked,

                    TerminalWordWrap = ObjTextResponse.WordWrap,
                    SendWordWrap = ObjTextSend.WordWrap
                };

                SaveToXml(preset, sfd.FileName);
            }
            catch (Exception ex)
            {
                SendErrorMsg($"Error saving preset:\n{ex.Message}");
            }
        }

        private void SetTitle()
        {
            if (string.IsNullOrEmpty(_saveFileName))
            {
                this.Text = $@"{_MainTitle}";
                return;
            }

            try
            {
                string? newTitle = Path.GetFileNameWithoutExtension(_saveFileName);
                this.Text = $@"{(File.Exists(_saveFileName) ? newTitle + " - " : string.Empty)}{_MainTitle}";
            }
            catch
            {
                this.Text = $@"{_MainTitle}";
            }
        }

        private void UpdatePortNamesInComboBox()
        {
            _ToolPortName.Items.Clear();
            try
            {
                foreach (string comport in SerialPort.GetPortNames())
                {
                    _ToolPortName.Items.Add(comport);
                }

                if (SerialPort.GetPortNames().Length <= 0) { _ToolPortName.Items.Add(_PortEmpty); }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"UpdatePortNamesInComboBox():\t {ex.Message}");
            }
        }

        internal void IsConnected(bool isConnected)
        {
            _IsConnected = isConnected;
            ObjSCTerminal.Panel2.Enabled = isConnected;
            _ToolPortName.Enabled = !isConnected;
            _DiscardReceiveBufferMenuItem.Enabled = isConnected;
            _DiscardTransmitBufferMenuItem.Enabled = isConnected;
            _PresetLoadMenuItem.Enabled = !isConnected;

            // Keep computer awake when connected
            if (isConnected)
            {
                _ToolConnect.SetConnected();
                ClsPowerManagement.PreventSleep();
            }
            else
            {
                _ToolConnect.SetDisconnected();
                ClsPowerManagement.AllowSleep();
            }

            ObjProperties.SelectedObject = _SPPropertyWrapper;
        }

        // Converts escape sequences in the input string to their actual character representations
        internal static string ConvertToEscapeChars(string? input)
        {
            if (string.IsNullOrEmpty(input)) { return string.Empty; }

            return input
                //.Replace("\\\\", "\\")    // Convert backslashes
                .Replace("\\n", "\n")   // Convert newline
                .Replace("\\t", "\t")   // Convert tab
                .Replace("\\r", "\r")   // Convert carriage return
                .Replace("\\b", "\b")   // Convert backspace
                .Replace("\\f", "\f")   // Convert form feed
                .Replace("\\v", "\v")   // Convert vertical tab
                .Replace("\\0", "\0");  // Convert zero
        }

        // Escapes special characters in the input string for display purposes
        internal static string EscapeSpecialCharacters(string? input)
        {
            if (string.IsNullOrEmpty(input)) { return string.Empty; }

            return input
                .Replace("\\", "\\\\")  // Escape backslashes
                .Replace("\n", "\\n")   // Escape newline
                .Replace("\t", "\\t")   // Escape tab
                .Replace("\r", "\\r")   // Escape carriage return
                .Replace("\b", "\\b")   // Escape backspace
                .Replace("\f", "\\f")   // Escape form feed
                .Replace("\v", "\\v")   // Escape vertical tab
                .Replace("\0", "\\0");  // Escape zero
        }

        // Sends a message to the serial port and optionally displays it in the terminal
        private async Task SendMessage(bool manualSend, string message, CancellationToken token)
        {
            if (!string.IsNullOrEmpty(message) && !_SP.BreakState)
            {
                byte[] command = Encoding.ASCII.GetBytes($"{ConvertToEscapeChars(message)}{(_EndWithSepChar ? ConvertToEscapeChars($"{_BufferSeparationChar}") : string.Empty)}");
                DateTime dtNow = DateTime.Now;
                if (_DTReceive == null) { _DTSend = dtNow; }

                await _SP.BaseStream.WriteAsync(command, token);

                if (manualSend && _DisplaySendMessageMenuItem.Checked)
                {
                    Invoke(new Action(() => ObjTextResponse.AppendText($"{(_ShowTimestampMenuItem.Checked ? $"{dtNow:HH:mm:ss:fff}:\t" : string.Empty)}## CMD: {@ObjTextSend.Text}{Environment.NewLine}")));
                }
            }

        }

        // Starts asynchronous tasks for sending and receiving data over the serial port
        private async Task StartSerialCommunicationAsync(CancellationToken token)
        {
            byte[] buffer = new byte[_SP.ReadBufferSize]; // Buffer for incoming data

            // Task for continuously sending the command
            var sendTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    // Autosend logic
                    if (_IsConnected && _ContinousSendActive)
                    {
                        if (!_SWSendTimer.IsRunning) { _SWSendTimer.Start(); }

                        try
                        {
                            await SendMessage(_ShowSendMessage, $"{_ContinousSendCommand}", token);

                            while ((_SWSendTimer.ElapsedTicks * 1000) / Stopwatch.Frequency < _msToWait) { }
                            _SWSendTimer.Restart();
                        }
                        catch (OperationCanceledException)
                        {
                            // Task was cancelled
                            Debug.WriteLine("sendTask: OperationCanceledException.");
                            _CTS.Cancel();
                            break;
                        }
                        catch (InvalidOperationException ex)
                        {
                            if (_SP.IsOpen)
                            {
                                // Handle unexpected exceptions
                                Debug.WriteLine($"sendTask: InvalidOperationException: {ex.Message}");
                                Invoke(new Action(() => SendErrorMsg($"Error during sending command: {ex.Message}")));
                            }
                            _CTS.Cancel();
                            break;
                        }
                        catch (Exception ex)
                        {
                            // Handle unexpected exceptions
                            Debug.WriteLine($"sendTask: Exception: {ex.Message}");
                            Invoke(new Action(() => SendErrorMsg($"Error during sending command: {ex.Message}")));
                            _CTS.Cancel();
                            break;
                        }
                    }
                    else
                    {
                        if (_SWSendTimer.IsRunning) { _SWSendTimer.Restart(); _SWSendTimer.Stop(); }
                        await Task.Delay(10, token); // Briefly wait before checking `_sendTrigger` again
                    }
                }
                _SWSendTimer.Stop();
            }, token);

            // Task for continuously reading incoming data
            var readTask = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    try
                    {
                        // Read data asynchronously
                        int bytesRead = await _SP.BaseStream.ReadAsync(buffer, token);
                        if (bytesRead > 0)
                        {
                            // Convert the received bytes to a string and handle them
                            if (_DisplayRawInputMenuItem.Checked)
                            {
                                HandleBuffer(EscapeSpecialCharacters(Encoding.ASCII.GetString(buffer, 0, bytesRead)), false);
                            }
                            else
                            {
                                HandleBuffer(Encoding.ASCII.GetString(buffer, 0, bytesRead));
                            }
                        }

                        if (_AwaitingDiscardInBuffer)
                        {
                            _SP.DiscardInBuffer();
                            _AwaitingDiscardInBuffer = false;
                        }
                    }
                    catch (OperationCanceledException)
                    {
                        // Task was cancelled
                        Debug.WriteLine("readTask: OperationCanceledException.");
                        _CTS.Cancel();
                        break;
                    }
                    catch (IOException ex)
                    {
                        // Handle IO exceptions
                        Debug.WriteLine($"readTask: IOException: {ex.Message}");
                        Invoke(new Action(() => SendErrorMsg($"Serial Port IO Exception: {ex.Message}")));
                        _CTS.Cancel();
                        break;
                    }
                    catch (Exception ex)
                    {
                        // Handle unexpected exceptions
                        Debug.WriteLine($"readTask: Exception: {ex.Message}");
                        Invoke(new Action(() => SendErrorMsg($"Error during reading data: {ex.Message}")));
                        _CTS.Cancel();
                        break;
                    }
                }
            }, token);

            // Await both tasks to complete when cancellation is requested
            try
            {
                await Task.WhenAll(sendTask, readTask);
            }
            catch (OperationCanceledException)
            {
                // Handle task cancellation
                Debug.WriteLine("sendTask, readTask: Tasks cancelled.");
            }
            catch (Exception ex)
            {
                // Handles unprepared exceptions
                Debug.WriteLine($"sendTask, readTask: {ex.Message}");
            }
            finally
            {
                // Ensure tasks are cleaned up on completion
                sendTask.Dispose();
                readTask.Dispose();
                if (_SP.IsOpen) { _SP.Close(); }
                IsConnected(false);
                Debug.WriteLine("sendTask, readTask: Disposed correctly.");
            }
        }

        // Handles incoming text, managing the buffer and displaying content as needed
        private void HandleBuffer(string? incomingText, bool useBuffer = true)
        {
            if (string.IsNullOrEmpty(incomingText)) return;

            DateTime passDT = DateTime.Now;
            _DTReceive = passDT;

            if (_ActivateReceiveBufferMenuItem.Checked && useBuffer)
            {
                int lastIndex = 0;
                int nextIndex = incomingText.IndexOf(_BufferSeparationChar);

                // Append incoming text to buffer until a separation character is found
                while (nextIndex >= 0)
                {
                    _incomingBuffer.Append(incomingText, lastIndex, nextIndex - lastIndex);
                    DisplayBufferContent(passDT);
                    _incomingBuffer.Clear();

                    lastIndex = nextIndex + 1;
                    nextIndex = incomingText.IndexOf(_BufferSeparationChar, lastIndex);
                }
                _incomingBuffer.Append(incomingText, lastIndex, incomingText.Length - lastIndex);
            }
            else
            {
                DisplayBufferContent(passDT, incomingText);
            }
        }

        // Displays the content of the buffer in the terminal window
        private void DisplayBufferContent(DateTime received, string? content = null)
        {
            content ??= _incomingBuffer.ToString();

            if (!string.IsNullOrEmpty(content))
            {
                Invoke(new Action(() =>
                {
                    bool timestampActive = _ShowTimestampMenuItem.Checked;

                    // Calculate response time if enabled
                    TimeSpan? ts = _DisplayResponseTimeMenuItem.Checked && _DTSend.HasValue ? received - _DTSend.Value : null;
                    _DTReceive = null;
                    ObjTextResponse.AppendText($"{(timestampActive ? $"{received:HH:mm:ss:fff}:\t" : string.Empty)}{(ts == null ? string.Empty : $"({ts:fff} ms):\t")}{content}{(_AddLineEndingMenuItem.Checked ? Environment.NewLine : "")}");
                }));
            }
        }

        // Function to save the terminal window content to a specified file
        private async Task Save(string filename)
        {
            try
            {
                _SaveMenuItem.Enabled = false;
                using StreamWriter sw = new(filename);

                var saveTask = Task.Run(async () =>
                {
                    foreach (string line in ObjTextResponse.Lines)
                    {
                        if (!string.IsNullOrWhiteSpace(line))
                        {
                            await sw.WriteLineAsync(line);
                        }
                    }
                });

                _saveFileName = filename;
                SetTitle();

                await Task.WhenAny(saveTask);
            }
            catch (Exception ex)
            {
                SendErrorMsg($"{ex}");
            }
            finally
            {
                _SaveMenuItem.Enabled = true;
            }
        }

        // Method to serialize presets to XML and save it to a file
        public static void SaveToXml<T>(T obj, string filePath)
        {
            XmlSerializer serializer = new(typeof(T));
            using StreamWriter writer = new(filePath);
            serializer.Serialize(writer, obj);
        }

        // Method to deserialize presets from an XML file
        public static T? LoadFromXml<T>(string filePath) where T : class
        {
            XmlSerializer serializer = new(typeof(T));
            using StreamReader reader = new(filePath);
            return serializer.Deserialize(reader) as T;
        }

        private void FrmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_CTS != null)
            {
                try { _CTS.Cancel(); Debug.WriteLine("FormClosing(): _CTS Cancelled"); }
                catch (ObjectDisposedException) { Debug.WriteLine("FormClosing(): _CTS already disposed on cancellation try!"); }
                finally
                {
                    try { _CTS.Dispose(); }
                    catch (ObjectDisposedException) { Debug.WriteLine("FormClosing(): _CTS already disposed!"); }
                }
            }

            if (_SP.IsOpen) { _SP.Close(); Debug.WriteLine("FormClosing(): _SP Closed"); }
            try { _SP.Dispose(); Debug.WriteLine("FormClosing(): _SP Disposed"); }
            catch (ObjectDisposedException) { Debug.WriteLine("FormClosing(): _SP already disposed!"); }
        }

        private void DiscardReceiveBufferMenuItem_Click(object? sender, EventArgs e)
        {
            _AwaitingDiscardInBuffer = true;
        }

        private void DiscardTransmitBufferMenuItem_Click(object? sender, EventArgs e)
        {
            try
            {
                if (_SP.IsOpen) { _SP.DiscardOutBuffer(); }
            }
            catch (Exception ex)
            {
                SendErrorMsg($@"{ex.Message}");
            }
        }

        private void SaveMenuItem_Click(object? sender, EventArgs e)
        {
            using SaveFileDialog sfd = new();
            sfd.Filter = ".txt file|*.txt";

            DialogResult result = sfd.ShowDialog();
            if (result == DialogResult.OK)
            {
                _ = Save(@sfd.FileName);
            }
        }

        private void ToolConnect_Click(object? sender, EventArgs e)
        {
            if (_IsConnected)
            {
                try
                {
                    _CTS?.Cancel();
                    _SP.Close();
                }
                catch (IOException)
                {
                    SendErrorMsg($"Error closing serial port");
                }
                IsConnected(false);
            }
            else
            {
                bool errorFree = false;
                try
                {
                    string[] availablePorts = SerialPort.GetPortNames();

                    if (!availablePorts.Contains(_SP.PortName))
                    {
                        SendErrorMsg($"The port {_SP.PortName} is not available.");
                        return;
                    }

                    _SP.Open();
                    if (_CTS != null)
                    {
                        _CTS.Cancel();
                        _CTS.Dispose();
                    }
                    _CTS = new();
                    _ = StartSerialCommunicationAsync(_CTS.Token);
                    errorFree = true;
                }
                catch (IOException ex)
                {
                    SendErrorMsg($"Error opening serial port: {ex.Message}");
                }
                catch (UnauthorizedAccessException ex)
                {
                    SendErrorMsg($"Access to the serial port is denied: {ex.Message}");
                }
                catch (Exception ex)
                {
                    SendErrorMsg($"Unexpected error: {ex.Message}");
                }

                IsConnected(errorFree);
            }
        }

        private void ClearMenuItem_Click(object? sender, EventArgs e)
        {
            DialogResult dOk = MessageBox.Show("Clear terminal window?", _MainTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dOk == DialogResult.Yes)
            {
                ObjTextResponse.Clear();
            }
        }

        private void BtnSend_Click(object sender, EventArgs e)
        {
            _ = SendMessage(true, $"{ObjTextSend.Text}", _CTS.Token);
        }

        private void ToolPortName_DropDown(object? sender, EventArgs? e)
        {
            UpdatePortNamesInComboBox();
        }

        // Updates the serial port's PortName when a new selection is made in the ComboBox
        private void ToolPortName_SelectionChangeCommitted(object? sender, EventArgs? e)
        {
            if (!string.IsNullOrEmpty(_ToolPortName.Text) && _ToolPortName.Text != _PortEmpty)
            {
                _SP.PortName = _ToolPortName.Text;
                ObjProperties.Refresh(); // Refresh the PropertyGrid to show the updated PortName
            }
        }

        // Makes sure that only one message is sent per Enter key press
        bool isEnterPressed = false;

        private void ObjTextSend_KeyDown(object sender, KeyEventArgs e)
        {
            if (_SendOnEnter && !isEnterPressed)
            {
                if (!(e.Control || e.Alt || e.Shift) && e.KeyCode == Keys.Enter)
                {
                    e.Handled = true;
                    e.SuppressKeyPress = true;
                    _ = SendMessage(true, $"{ObjTextSend.Text}", _CTS.Token);
                    isEnterPressed = true;
                }
            }
        }

        private void ObjTextSend_KeyUp(object sender, KeyEventArgs e)
        {
            isEnterPressed = false;
        }
    }
}