using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace ConnectForPhone2._0
{
    public partial class Form1 : Form
    {
        private UdpClient udpClient;
        //private Thread recieveThread;
        IPAddress localIP;
        int localPort;
        private bool isRunning;

        // MX
        private ActUtlTypeLib.ActUtlType AUTLO;
        private int OPLite; // PLC 連線狀態
        private bool PLC_Connect_ING = false;
        private bool PLC_Connect_Check = false; // MX 控制連線

        public Form1()
        {
            AUTLO = new ActUtlTypeLib.ActUtlType();
            InitializeComponent();
            // 執行程式自動獲取電腦 IP
            localIP = IPAddress.Any;
            // 將本機 Port 改為 502
            localPort = 502;
            udpClient = new UdpClient(new IPEndPoint(localIP, localPort));
            isRunning = true;
            Task.Run(async () =>
            {
                while (isRunning)
                {
                    ReceiveMessages();
                    await Task.Delay(1000); // 每秒接收一次
                }
            });
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                AUTLO.ActLogicalStationNumber = 1;
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region 不間斷對PLC發送連線訊號
        private void 總計時_Tick(object sender, EventArgs e)
        {
            if (PLC_Connect_Check)
                MXComponent();
        }
        #endregion

        #region 接收手機語音訊息
        private void ReceiveMessages()
        {
            int caseSelect = -1;
            
            try
            {
                IPEndPoint remoteIPEndPoint = new IPEndPoint(IPAddress.Any, 0);
                byte[] data = udpClient.Receive(ref remoteIPEndPoint);
                IPEndPoint phoneIPEndPoint = new IPEndPoint(remoteIPEndPoint.Address, remoteIPEndPoint.Port);
                
                // 語音訊息 存放變數 message
                string message = Encoding.UTF8.GetString(data);
                string replyMessage;
                byte[] replyMessageByte;

                // 更新 UI，確保在 UI 執行緒上執行
                Invoke(new Action(async () =>
                {
                    ListBox.Items.Add($"收到來自 {remoteIPEndPoint.Address} 的訊息：{message}\n");
                    
                    if (PLC_Connect_Check)
                    {
                        if (ActionPLCParameter(message))
                        {
                            Invoke(new Action(() =>
                            {
                                ListBox.Items.Add($"{message}:指令已成功傳送\n");
                            }));
                            caseSelect = 1;
                        }
                        else
                        {
                            Invoke(new Action(() =>
                            {
                                ListBox.Items.Add($"{message}:指令執行失敗\n");
                            }));
                            caseSelect = 2;
                        }
                    }
                    else
                    {
                        Invoke(new Action(() =>
                        {
                            ListBox.Items.Add("未連線到PLC\n");
                        }));
                        caseSelect = 3;
                    }
                }));
                switch (caseSelect)
                {
                    case 1:
                        replyMessage = "指令執行成功";
                        replyMessageByte = Encoding.UTF8.GetBytes(replyMessage);
                        udpClient.Send(replyMessageByte, replyMessageByte.Length, phoneIPEndPoint);
                        break;
                    case 2:
                        replyMessage = "指令執行失敗";
                        replyMessageByte = Encoding.UTF8.GetBytes(replyMessage);
                        udpClient.Send(replyMessageByte, replyMessageByte.Length, phoneIPEndPoint);
                        break;
                    case 3:
                        replyMessage = "未連線到PLC";
                        replyMessageByte = Encoding.UTF8.GetBytes(replyMessage);
                        udpClient.Send(replyMessageByte, replyMessageByte.Length, phoneIPEndPoint);
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                Invoke(new Action(() =>
                {
                    ListBox.Items.Add($"接收訊息失敗：{ex.Message}\n");
                }));
            }
        }
        #endregion

        #region 關閉程式
        private void StopConnect_Click(object sender, EventArgs e)
        {
            ListBox.Items.Add("停止連線\n");
            isRunning = false;
            // recieveThread.Join();
            udpClient.Close();
            總計時.Stop();
            AUTLO.Close();
            // 緩衝2秒
            System.Threading.Thread.Sleep(2000);
            Application.Exit();
        }
        #endregion

        #region PLC 重新連線
        private void PLC_Connect_Click(object sender, EventArgs e)
        {
            總計時.Stop();
            PLC_Connect_Check = true;
            總計時.Start();
            MXComponent();
        }
        #endregion

        #region 停止連線
        private void Stop_Connect_Click(object sender, EventArgs e)
        {
            總計時.Stop();
            PLC_Connect_Check = false;
            AUTLO.Close();
            PLC_Connect.BackColor = Color.Red;
            ListBox.Items.Add("PLC已中斷連線\n");
            PLC_Connect_ING = false;
        }
        #endregion

        #region PLC MX 連線
        public void MXComponent()
        {
            try
            {
                AUTLO.Close();
                OPLite = AUTLO.Open();

                if(PLC_Connect_ING == false)
                {
                    if (OPLite == 0)
                    {
                        PLC_Connect.BackColor = Color.Green;
                        ListBox.Items.Add("PLC 已連線\n");
                        PLC_Connect_ING = true;
                    }
                    else if (OPLite == 104)
                    {
                        PLC_Connect.BackColor = Color.Yellow;
                        ListBox.Items.Add("接收超時\n");
                    }
                    else if (OPLite == 105)
                    {
                        PLC_Connect.BackColor = Color.Yellow;
                        ListBox.Items.Add("未檢測到 DSR 信號\n");
                    }
                    else if (OPLite == 106)
                    {
                        PLC_Connect.BackColor = Color.Red;
                        ListBox.Items.Add("連接被切斷\n");
                    }
                    else if (OPLite == 107)
                    {
                        PLC_Connect.BackColor = Color.Yellow;
                        ListBox.Items.Add("發送超時\n");
                    }
                    else if (OPLite == 108)
                    {
                        PLC_Connect.BackColor = Color.Yellow;
                        ListBox.Items.Add("順控編號錯誤\n");
                    }
                    else if (OPLite == 200)
                    {
                        PLC_Connect.BackColor = Color.Yellow;
                        ListBox.Items.Add("未發現附屬 DLL\n");
                    }
                    else if (OPLite != 0)
                    {
                        PLC_Connect.BackColor = Color.Red;
                        ListBox.Items.Add($"PLC 已中斷連線 出錯代碼: {OPLite}\n");
                    }
                }
            }
            catch (Exception)
            {
                總計時.Stop();
                PLC_Connect_Check = false;
                AUTLO.Close();
                PLC_Connect.BackColor = Color.Red;
                ListBox.Items.Add("PLC 已中斷連線\n");
                PLC_Connect_ING = false;
            }
        }
        #endregion

        #region 指令
        private bool ActionPLCParameter(string action)
        {
            if (Onlock.Visible == false)
            {
                if(action == "印刷機")
                {
                    ListBox.Items.Add("解除鎖定\n");
                    Onlock.Visible = true;
                    return true;
                }
                else
                {
                    ListBox.Items.Add("尚未解除鎖定\n");
                    return false;
                }
            }

            if (action.Contains("改"))
            {
                // 分割字串 RemoveEmptyEntries 移除分割後的空白字串
                string[] parts = action.Split(new[] {"改"}, StringSplitOptions.RemoveEmptyEntries);
                string ra_action;
                ra_action = ChineseToRoman(parts[0]).Replace(" ", "").Replace("\n", "");
                ListBox.Items.Add(ra_action); // 檢測用(羅馬拼音)

                // 以指令判定修改點位

                switch (ra_action)
                {
                    case "penmocishu": // 噴墨次數
                        ParameterModification("ZR200", parts[1]);
                        return true;
                    case "yijianpenmocishu": // 一鍵噴墨次數
                        ParameterModification("ZR201", parts[1]);
                        return true;
                    case "yinshuasudu": // 印刷速度
                        ParameterModification("ZR500", parts[1]);
                        return true;
                    case "huimosudu": // 回墨速度
                        ParameterModification("ZR501", parts[1]);
                        return true;
                    case "yinwanyanchi": // 印完延遲
                        ParameterModification("ZR504", parts[1]);
                        return true;
                    case "guadaojiaodu": // 刮刀角度
                        ParameterModification("ZR505", parts[1]);
                        return true;
                    case "guayincishu": // 刮印次數
                        ParameterModification("ZR506", parts[1]);
                        return true;
                    case "jibankuandu": // 基板寬度
                        ParameterModification("ZR507", parts[1]);
                        return true;
                    case "jibanchangdu": // 基板長度
                        ParameterModification("ZR508", parts[1]);
                        return true;
                    case "jibanzhijuhoudu": // 基板治具厚度
                        ParameterModification("ZR509", parts[1]);
                        return true;
                    case "jiakong": // 駕空
                        ParameterModification("ZR510", parts[1]);
                        return true;
                    case "yinlibanjuli": // 印離板距離
                        ParameterModification("ZR511", parts[1]);
                        return true;
                    case "zuoguadaoshendu": // 左刮刀深度
                        ParameterModification("ZR513", parts[1]);
                        return true;
                    case "youguadaoshendu": // 右刮刀深度
                        ParameterModification("ZR514", parts[1]);
                        return true;
                    case "zuomodaoshendu": // 左墨刀深度
                        ParameterModification("ZR515", parts[1]);
                        return true;
                    case "youmodaoshendu": // 右墨刀深度
                        ParameterModification("ZR516", parts[1]);
                        return true;
                    case "zuoguayali": // 左刮壓力
                        ParameterModification("ZR517", parts[1]);
                        return true;
                    case "youguayali": // 右刮壓力
                        ParameterModification("ZR518", parts[1]);
                        return true;
                    case "zuomoyali": // 左墨壓力
                        ParameterModification("ZR520", parts[1]);
                        return true;
                    case "youmoyali": // 右墨壓力
                        ParameterModification("ZR521", parts[1]);
                        return true;
                    case "xifengdiaozheng": // 吸風調整
                        ParameterModification("ZR522", parts[1]);
                        return true;
                    case "guadaochangdu": // 刮刀長度
                        ParameterModification("ZR530", parts[1]);
                        return true;
                    case "guadaohoudu": // 刮刀厚度
                        ParameterModification("ZR531", parts[1]);
                        return true;
                    case "guajiaoyingdu": // 刮膠硬度
                        ParameterModification("ZR532", parts[1]);
                        return true;
                    case "wangbanjiance": // 網版檢測
                        ParameterModification("ZR533", parts[1]);
                        return true;
                    case "baojingpanduan": // 報警判斷
                        ParameterModification("ZR534", parts[1]);
                        return true;
                    case "qianhouweiyi": // 前後位移
                        ParameterModification("ZR550", parts[1]);
                        return true;
                    case "zuoyouweiyi": // 左右位移
                        ParameterModification("ZR551", parts[1]);
                        return true;
                    case "huilijuli": // 回離距離
                        ParameterModification("ZR552", parts[1]);
                        return true;
                    case "pinzhongbianhao": // 品種編號
                        ParameterModification("ZR553", parts[1]);
                        return true;
                    case "youmotianjiacishu": // 油墨添加次數
                        ParameterModification("ZR711", parts[1]);
                        return true;
                    case "penmoshijian": // 噴墨時間
                        ParameterModification("ZR1060", parts[1]);
                        return true;
                    default:
                        return false;
                }
            }

            string rn_action;
            rn_action = ChineseToRoman(action).Replace(" ", "").Replace("\n", "");
            ListBox.Items.Add(rn_action); //檢測用(羅馬拼音)

            switch (rn_action)
            {
                case "ceshi": //測試
                    Test();
                    return true;
                case "fuyuan": // 復原
                    Reset();
                    return true;
                case "banjin": // 板進
                    Banjin();
                    return true;
                case "bantui": // 板退
                    Bantui();
                    return true;
                default:
                    break;
            }
            return false;
        }
        #endregion

        #region 寫入範例
        private void Test()
        {
            // 線圈更改方式 1為True 0為False
            AUTLO.SetDevice("X0", 1);
            AUTLO.SetDevice("X1", 0);
            AUTLO.SetDevice("X2", 1);
            AUTLO.SetDevice("X3", 0);

            // D 寫入方式
            AUTLO.WriteDeviceRandom("D0", 1, 1234);
            AUTLO.WriteDeviceRandom("D1", 1, 234);
            AUTLO.WriteDeviceRandom("D2", 1, 34);

            // ZR 寫入方式
            AUTLO.WriteDeviceRandom("ZR100", 1, 10);
            AUTLO.WriteDeviceRandom("ZR101", 1, 30);
            AUTLO.WriteDeviceRandom("ZR102", 1, 100);
            
            // Y 寫入方式
            AUTLO.SetDevice("Y0", 1);
            AUTLO.SetDevice("Y1", 1);

            ListBox.Items.Add("指令已成功執行\n");
        }

        private void Reset()
        {
            // WriteDeviceBlock 寫入連續數據為主
            // WriteDeviceRandom 寫入特定位置數據 一個數據為主

            // 線圈更改方式 1為True 0為False
            AUTLO.SetDevice("X0", 0);
            AUTLO.SetDevice("X1", 0);
            AUTLO.SetDevice("X2", 0);
            AUTLO.SetDevice("X3", 0);

            // D 寫入方式
            AUTLO.WriteDeviceRandom("D0", 1, 0);
            AUTLO.WriteDeviceRandom("D1", 1, 0);
            AUTLO.WriteDeviceRandom("D2", 1, 0);

            // ZR 寫入方式
            AUTLO.WriteDeviceRandom("ZR100", 1, 0);
            AUTLO.WriteDeviceRandom("ZR101", 1, 0);
            AUTLO.WriteDeviceRandom("ZR102", 1, 0);

            // Y 寫入方式
            AUTLO.SetDevice("Y0", 0);
            AUTLO.SetDevice("Y1", 0);

            ListBox.Items.Add("指令已成功執行\n");
        }
        #endregion

        private void CleanListBox_Click(object sender, EventArgs e)
        {
            ListBox.Items.Clear();
        }

        #region 板進
        private void Banjin()
        {
            AUTLO.SetDevice("X50", 1);
        }
        #endregion

        #region 板退
        private void Bantui()
        {
            AUTLO.SetDevice("X50", 0);
        }
        #endregion

        #region 參數修改
        private void ParameterModification(string location, string value)
        {
            int iValue = int.Parse(value);
            AUTLO.WriteDeviceRandom(location, 1, iValue);
        }
        #endregion

        #region 繁體 -> 簡體 -> 羅馬拼音 
        private string ChineseToRoman(string value)
        {
            var simplifiedStr = Microsoft.International.Converters.
                      TraditionalChineseToSimplifiedConverter.
                      ChineseConverter.Convert(value
                      , Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter.ChineseConversionDirection.TraditionalToSimplified);
            return NPinyin.Pinyin.GetPinyin(simplifiedStr);
        }
        #endregion

        private void Onlock_Click(object sender, EventArgs e)
        {
            ListBox.Items.Add("功能已鎖定\n");
            Onlock.Visible = false;
        }
    }
}
