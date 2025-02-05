using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
//using NModbus;
//using Modbus.Device;
using System.Net.Sockets;
using System.Net;
using System.Collections.ObjectModel;
using Xamarin.Essentials;

namespace Speech_2._0
{
    public interface IAudioService
    {
        void PlayAudioFile(string fileName);
    }

    public partial class MainPage : ContentPage
    {
        //ModbusIpMaster _master;
        //TcpClient _tcp;
        //public byte unitID = 1;-
        private UdpClient udpClient;
        private IPAddress serverIP;
        private int serverPort;
        private ObservableCollection<string> messages = new ObservableCollection<string>();

        public MainPage()
        {
            InitializeComponent();
            if (Preferences.ContainsKey("SavedIP"))
            {
                txtIP.Text = Preferences.Get("SavedIP", ""); // 设置输入框的文本为保存的 IP 地址
            }
            lstMessages.ItemsSource = messages;
        }

        //語音初始化
        readonly ISpeechToText speech = DependencyService.Get<ISpeechToText>();

        #region 
        private void btnConnect_Clicked(object sender, EventArgs e)
        {
            // messages.Add("1111");
            Preferences.Set("SavedIP", txtIP.Text);
            // 測試用
            serverIP = IPAddress.Parse(txtIP.Text);
            serverPort = int.Parse(txtPort.Text);

            udpClient = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(serverIP, serverPort);

            string parameterToSend = "Hello From Android !";
            try
            {
                byte[] data = Encoding.ASCII.GetBytes(parameterToSend);
                udpClient.Send(data, data.Length, endPoint);
                DateTime endTime = DateTime.Now.AddSeconds(3);
                while (DateTime.Now < endTime)
                {
                    try
                    {
                        //IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] receivedBytes = udpClient.Receive(ref endPoint);
                        string receivedMessage = Encoding.UTF8.GetString(receivedBytes);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            messages.Add(receivedMessage);
                            if (receivedMessage == "未連線到PLC")
                            {
                                // receivedMessage = 未連線到PLC 播放語音
                                DependencyService.Get<IAudioService>().PlayAudioFile("notCommandToPLC.mp3");
                            }
                        });
                        break;
                    }
                    catch
                    {

                    }
                }
            }
            catch(Exception ex)
            {
                _ = DisplayAlert("Error", ex.ToString(), "OK");
            }
            
        }
        #endregion

        private void btnDisconnect_Clicked(object sender, EventArgs e)
        {
            udpClient.Close();
            udpClient = null;
        }

        #region 語音辨識讀取
        protected async void btnSpeech_Clicked(object sender, EventArgs e)
        {
            var SpeechResult = await speech.SpeechToTextAsync();

            serverIP = IPAddress.Parse(txtIP.Text);
            serverPort = int.Parse(txtPort.Text);

            udpClient = new UdpClient();
            IPEndPoint endPoint = new IPEndPoint(serverIP, serverPort);

            string parameterToSend = SpeechResult.Text.ToString();
            //_ = DisplayAlert("parameterToSend",  parameterToSend, "OK");

            try
            {
                byte[] data = Encoding.UTF8.GetBytes(parameterToSend);
                udpClient.Send(data, data.Length, endPoint);

                DateTime endTime = DateTime.Now.AddSeconds(3);
                while (DateTime.Now < endTime)
                {
                    try
                    {
                        //IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                        byte[] receivedBytes = udpClient.Receive(ref endPoint);
                        string receivedMessage = Encoding.UTF8.GetString(receivedBytes);

                        Device.BeginInvokeOnMainThread(() =>
                        {
                            messages.Add(receivedMessage);
                            if (receivedMessage == "指令執行成功")
                            {
                                // receivedMessage = 指令執行成功 播放語音
                                DependencyService.Get<IAudioService>().PlayAudioFile("commandSuccessful.mp3");
                            }
                            else if (receivedMessage == "指令執行失敗")
                            {
                                // receivedMessage = 指令執行失敗 播放語音
                                DependencyService.Get<IAudioService>().PlayAudioFile("commandFailed.mp3");
                            }
                            else if (receivedMessage == "未連線到PLC")
                            {
                                // receivedMessage = 未連線到PLC 播放語音
                                DependencyService.Get<IAudioService>().PlayAudioFile("notCommandToPLC.mp3");
                            }
                        });
                        break;
                    }
                    catch
                    {
                        
                    }
                }
            }
            catch (Exception ex)
            {
                _ = DisplayAlert("Error", ex.ToString(), "OK");
            }
            finally
            {
                udpClient.Close();
            }
        }
        #endregion

        private void btnClear_Clicked(object sender, EventArgs e)
        {
            messages.Clear();
        }

        
    }
}
