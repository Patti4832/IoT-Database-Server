/*
MIT License
Copyright (c) 2018 Patti4832
Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:
The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.
THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace IoT_Database_Server
{
    class Server
    {
        private bool active = false;
        private int p;

        public Server(int port)
        {
            if (port < 65535 && port > 9)
            {
                Log.Info("Using port " + port + ".");
                p = port;
            }
            else
            {
                Log.Error("Can't use port '" + port + "'! Use port between 10 and 65535.");
            }
        }
        public void Start()
        {
            active = true;
            Loop();
        }
        public void Stop()
        {
            active = false;
        }

        static TcpListener _server;
        static TcpClient _client;
        static Stream _stream;

        private void Loop()
        {
            string request = "";
            byte[] message = new byte[4096];
            int bytesRead = 0;
            bool receivedError = false;

            try
            {
                _server = new TcpListener(p);
                _server.Start();
                active = true;
                Log.Info("Server startet.");
                Log.Info("Waiting for clients ...");
            }
            catch
            {
                active = false;
                Log.Error("Can't start server!");
            }

            if (active)
            {
                try
                {
                    _client = _server.AcceptTcpClient();
                    _stream = _client.GetStream();
                    Log.Info("Stream active.");
                }
                catch
                {
                    Log.Error("Can't get stream!");
                }
            }

            while (active)
            {
                Log.Info("Connection from '" + ((IPEndPoint)_client.Client.RemoteEndPoint).Address.ToString() + "'");
                try
                {
                    bytesRead = _stream.Read(message, 0, 4096);
                    receivedError = false;
                    Log.Info("Received request from client.");
                }
                catch
                {
                    receivedError = true;
                    Log.Warning("Can't get request!");
                }

                if (bytesRead == 0 || receivedError)
                {
                    Log.Info("Restarting server ...");
                    _client.Close();
                    _server.Stop();
                    Loop();
                    break;
                }
                else
                {
                    Log.Info("Encoding request ...");
                    try
                    {
                        ASCIIEncoding encoder = new ASCIIEncoding();
                        request = encoder.GetString(message, 0, bytesRead);
                        Log.Info("Encoding successful.");
                    }
                    catch
                    {
                        Log.Warning("Can't encode request!");
                    }
                    Log.Debug("[Client]\n" + request + "\n[Client End]");

                    string[] parameters = HTTP_Parser.GetParams(request);
                    string tmpanswer = Database.Process(parameters);
                    string answer = HTTP_Parser.CreateAnswer(tmpanswer);

                    bool successful = true;
                    byte[] szData;
                    szData = System.Text.Encoding.ASCII.GetBytes(answer.ToCharArray());

                    foreach (byte b in szData)
                    {
                        try
                        {
                            _stream.WriteByte(b);
                        }
                        catch
                        {
                            successful = false;
                        }
                    }

                    if (successful)
                    {
                        Log.Info("Answer sent.");
                    }
                    else
                    {
                        Log.Info("Answer not sent!");
                    }
                }
            }
        }
    }
}
