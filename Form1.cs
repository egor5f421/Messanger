using CefSharp;
using CefSharp.WinForms;
using System;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Messanger
{
    public partial class Messanger : Form
    {
        //flags
        private readonly bool[] flags =
        {
            false, // UserName entered
            true, // Network enabled
        };
        private string username = "Новый пользователь";


        public Messanger()
        {
            InitializeComponent();
        }

        private void Display(string message, Color color)
        {
            RichTextBoxExtensions.AppendText(Messages, message.Trim('\n', ' ') + "\n", color);
        }

        private void Send(object sender, EventArgs e)
        {
            if (Input.Text.Equals(InputPlaceholder) || Input.Text.Equals(string.Empty))
            {
                if (choiseFile.ShowDialog() == DialogResult.OK)
                {
                    SendFile(choiseFile.FileName);
                }
                return;
            }

            string temp = username + ": " + Input.Text.TrimEnd('\n');
            Display(temp, Color.Blue);

            Input.Text = string.Empty;
            Input.Focus();

            if (!flags[1]) return;
            SendNet(temp);
        }

        private void Start(object sender, EventArgs e)
        {
            Input.Text = InputPlaceholder;

            if (!flags[1]) return;
            StartNet();
        }

        private void ResizeWindow(object sender, EventArgs e)
        {
            Point location = new Point(0, 0)
            {
                X = 12,
                Y = Size.Height - 86
            };
            Input.Location = location;

            location.X = Input.Width + 18;
            SendButton.Location = location;

            Size size = new Size(0, 0)
            {
                Height = Input.Height,
                Width = Size.Width - 182
            };
            Input.Size = size;

            size.Width = Size.Width - 38;
            size.Height = Size.Height - 104;
            Messages.Size = size;
        }

        #region Input
        private const string InputPlaceholder = "Ведите текст...";

        private void InputOnLostFocus(object sender, EventArgs e)
        {
            if (Input.Text.Equals(string.Empty))
            {
                Input.Text = InputPlaceholder;
            }
        }

        private void InputOnGotFocus(object sender, EventArgs e)
        {
            if (Input.Text.Equals(InputPlaceholder))
            {
                Input.Text = string.Empty;
            }
        }

        private void OnMessage(object sender, KeyEventArgs e)
        {
            if (!flags[0] && e.KeyCode == Keys.Return)
            {
                flags[0] = true;
                TextInfo info = new CultureInfo("ru-ru").TextInfo;
                username = info.ToTitleCase(Input.Text.Trim('\n'));
                Text = username;
                Send(SendButton, EventArgs.Empty);

                if (flags[1])
                {
                    byte[] buffer = Encoding.UTF8.GetBytes("_newUser: " + username);
                    stream.Write(buffer, 0, buffer.Length);
                }

                return;
            }
            if (e.KeyCode == Keys.Return && e.Modifiers != Keys.Control)
            {
                Send(SendButton, EventArgs.Empty);
            }
        }

        private void OnClickLink(object sender, LinkClickedEventArgs e)
        {
            string url = e.LinkText;
            Form form = new Form()
            {
                MinimizeBox = false,
                MaximizeBox = false,
                Size = new Size(900, 900),
            };
            ChromiumWebBrowser web = new ChromiumWebBrowser
            {
                Dock = DockStyle.Fill
            };
            web.LoadUrl(url);

            web.AddressChanged += (object sender2, AddressChangedEventArgs e2) =>
            {
                form.BeginInvoke((MethodInvoker)(() => form.Text = e2.Address));
            };

            form.FormClosing += (object sender2, FormClosingEventArgs e2) =>
            {
                web.BeginInvoke((MethodInvoker)(() => web.Dispose()));
            };

            form.Controls.Add(web);
            form.ShowDialog();
        }
        #endregion

        #region Internet
        private TcpClient tcpClient = new TcpClient();
        private NetworkStream stream;

        private void SendNet(string data)
        {
            if (!flags[0]) return;
            byte[] buffer = Encoding.UTF8.GetBytes(data);
            stream.Write(buffer, 0, buffer.Length);
        }

        private void Recive(object sender, EventArgs e)
        {
            if (!flags[1]) return;
            if (stream == null) return;
            if (!stream.DataAvailable) return;
            byte[] bytes = new byte[1024];
            int lenght = stream.Read(bytes, 0, bytes.Length);
            string data = Encoding.UTF8.GetString(bytes, 0, lenght).Trim('\n');
            if (data.Split(';')[0].Equals("list"))
            {
                string[] users = data.Replace("list;", "Список пользователей:").Split(';');
                foreach (var user in users)
                {
                    Display(user.Trim('\n', ' ') + "\n", Color.BlueViolet);
                }
            }
            else
            {
                string temp = data.Substring(0, data.IndexOf(": ")).Trim('\n');
                if (username.Equals(temp)) return;
                Display(data, Color.Red);
            }
        }

        private void StopConnections(object sender, FormClosingEventArgs e)
        {
            if (stream != null)
            {
                stream.Close();
                stream.Dispose();
                stream = null;
            }
            if (tcpClient != null)
            {
                tcpClient.Close();
                tcpClient.Dispose();
                tcpClient = null;
            }
        }

        private void StartNet()
        {
            try
            {
                string ip = string.Empty;
                Form form = new Form();
                TextBox tb = new TextBox
                {
                    Dock = DockStyle.Fill
                };
                tb.TextChanged += (object sender2, EventArgs e2) =>
                {
                    ip = tb.Text;
                };
                form.Controls.Add(tb);
                Button btn = new Button
                {
                    Dock = DockStyle.Bottom,
                    Text = "ok"
                };
                btn.Click += (object sender2, EventArgs e2) =>
                {
                    if (tb.Text == "") ip = "127.0.0.1";
                    form.Close();
                };
                form.Controls.Add(btn);
                form.ShowDialog();
                IPAddress ip2 = new IPAddress(new byte[] { 127, 0, 0, 1 });
                if (!IPAddress.TryParse(ip, out ip2))
                {
                    Console.WriteLine(ip);
                }
                Console.WriteLine(ip);
                tcpClient.Connect(ip2, 7305);
                stream = tcpClient.GetStream();
            }
            catch (SocketException ex)
            {
                Console.WriteLine($"Error: {ex}");
                Dispose(true);
            }
        }
        #endregion

        #region File transfer
        private void SendFile(string filename)
        {
            byte[] data = new byte[1024 * 7];
            int lenght = 0;
            using (FileStream fs = new FileStream(filename, FileMode.Open))
            {
                lenght = fs.Read(data, 0, data.Length);
            }

            Form form = new Form();

            Button ok = new Button
            {
                Text = "Отправить",
                Dock = DockStyle.Left,
                DialogResult = DialogResult.OK
            };
            Button cancel = new Button
            {
                Text = "Отмена",
                Dock = DockStyle.Right,
                DialogResult = DialogResult.Cancel
            };
            string stringData = Encoding.UTF8.GetString(data, 0, lenght);
            Label label = new Label
            {
                Text = stringData,
                Dock = DockStyle.Fill
            };

            form.Controls.Add(label);
            form.Controls.Add(ok);
            form.Controls.Add(cancel);

            if (form.ShowDialog() != DialogResult.OK) return;

            string dataForSend = "file;" + stringData;
            byte[] bytesDataForSend = Encoding.UTF8.GetBytes(dataForSend);
            stream.Write(bytesDataForSend, 0, bytesDataForSend.Length);

            Display("Отправлено...", Color.Bisque);
        }
        #endregion
    }
}
