using BoqiangH5.BQProtocol;
using BoqiangH5Entity;
using BoqiangH5Repository;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Windows.Media;
using System.Windows.Controls.Primitives;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.IO.Ports;
using System.Windows.Documents;
using System.Text;

namespace BoqiangH5
{
    /// <summary>
    /// UserCtrlBqBmsInfo.xaml 的交互逻辑
    /// </summary>
    public partial class UserCtrlBqBmsInfo : UserControl
    {
        public static List<H5BmsInfo> m_ListBmsInfo = new List<H5BmsInfo>();
        public static List<H5BmsInfo> m_ListCellVoltage = new List<H5BmsInfo>();

        static List<BitStatInfo> m_ListSysStatus = new List<BitStatInfo>();
        static List<BitStatInfo> m_ListBatStatus = new List<BitStatInfo>();
        static Dictionary<string, string> m_DicManufactureCode = new Dictionary<string, string>();

        //MCU消息
        List<H5BmsInfo> ListSysInfo1 = new List<H5BmsInfo>();
        List<H5BmsInfo> ListSysInfo2 = new List<H5BmsInfo>();
        List<H5BmsInfo> ListChargeInfo = new List<H5BmsInfo>();
        public UserCtrlBqBmsInfo()
        {
            InitializeComponent();
      
            InitBqBmsInfoWnd();

            timer = new System.Windows.Threading.DispatcherTimer();
            timer.Tick += new EventHandler(OnTimer);
        }

        private void InitBqBmsInfoWnd()
        {
            m_ListCellVoltage.Clear();
            m_ListBmsInfo.Clear();

            string strConfigFile = XmlHelper.m_strCfgFilePath; 

            XmlHelper.LoadXmlConfig(strConfigFile, "bms_info/cell_votage_info", m_ListCellVoltage);
            XmlHelper.LoadXmlConfig(strConfigFile, "bms_info/bms_info_node", m_ListBmsInfo);

            XmlHelper.LoadBqBmsStatusConfig(strConfigFile, "system_status_info/byte_status_info/bit_status_info", m_ListSysStatus);
            XmlHelper.LoadBqBmsStatusConfig(strConfigFile, "battery_status_info/byte_status_info/bit_status_info", m_ListBatStatus);

            XmlHelper.LoadManufactureCode(strConfigFile, "manufacture_code/manufacture_code_node", m_DicManufactureCode);

            //读MCU参数
            XmlHelper.LoadXmlConfig(strConfigFile, "mcu_info/system1/mcu_node_info", ListSysInfo1);
            XmlHelper.LoadXmlConfig(strConfigFile, "mcu_info/system2/mcu_node_info", ListSysInfo2);
            XmlHelper.LoadXmlConfig(strConfigFile, "mcu_info/charge_discharge/mcu_node_info", ListChargeInfo);
            //tbEepromFileName.Text = OneClickFactorySetting.m_EepromFilePath;
            //tbMcuFileName.Text = OneClickFactorySetting.m_MCUFilePath;
            //tbSoc.Text = OneClickFactorySetting.m_SOCValue.ToString();
        }

        //private void cbIsUpdate_Click(object sender, RoutedEventArgs e)
        //{
        //    if ((bool)cbIsUpdate.IsChecked)
        //        BqProtocol.BqInstance.m_bIsUpdateBmsInfo = true;
        //    else
        //        BqProtocol.BqInstance.m_bIsUpdateBmsInfo = false;

        //}

        private void ucBqBmsInfo_Loaded(object sender, RoutedEventArgs e)
        {
            dgBqBmsInfo.ItemsSource = m_ListBmsInfo;
            dgBqBmsCellVoltage.ItemsSource = m_ListCellVoltage;

            listBoxSysStatus.ItemsSource = m_ListSysStatus;
            listBoxBatStatus.ItemsSource = m_ListBatStatus;
            
        }

        public void SetOffLineUIStatus()
        {
            SetOffLineStatus(m_ListSysStatus);
            SetOffLineStatus(m_ListBatStatus);
            count = 0;
            RefreshResult("待测试", false);
            //CSVFileHelper.WriteLogs("log", "掉线", "掉线！");
        }
        private void SetOffLineStatus(List<BitStatInfo> listStatInfo)
        {
            for (int n = 0; n < listStatInfo.Count; n++)
            {
                listStatInfo[n].IsSwitchOn = false;
                listStatInfo[n].BackColor = new SolidColorBrush(Colors.DarkGray);
            }
        }

        //Queue<string> msgQueue = null;
        public void HandleRecvBmsInfoDataEvent(object sender, CustomRecvDataEventArgs e)
        {            
            BqUpdateBmsInfo(e.RecvMsg);
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var soc = m_ListBmsInfo.SingleOrDefault(p => p.Description == "SOC");
                if(soc != null)
                {
                    ucBattery.Battery_Glasses(100, 0, double.Parse(soc.StrValue));
                }
                var current = m_ListCellVoltage.SingleOrDefault(p => p.Description == "实时电流");
                if(current != null){
                    tbCurrent.Text = current.StrValue;
                }
                var voltage = m_ListCellVoltage.SingleOrDefault(p => p.Description == "总电压");
                if (voltage != null)
                {
                    tbVoltage.Text = voltage.StrValue;
                }
            }));
        }

        string bmsfilePath = string.Empty;
        string cellfilePath = string.Empty;
        System.Windows.Threading.DispatcherTimer timer = null;
        FileStream _fs = null;
        StreamWriter sw = null;
        //lipeng   2020.3.26,增加BMS信息记录
        private void cbIsSaveBms_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = null;
            bmsfilePath = System.AppDomain.CurrentDomain.BaseDirectory + @"Data\Bms_" + System.DateTime.Now.ToString("yyyyMMddHHmmss") + ".csv";
            if ((bool)cbIsSaveBms.IsChecked)
            {
                FileInfo fi = new FileInfo(bmsfilePath);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }
                if (!File.Exists(bmsfilePath))
                {
                    fs = File.Create(bmsfilePath);//创建该文件
                    fs.Close();
                    CSVFileHelper.SaveBmsORCellCSVTitle(bmsfilePath,true,m_ListBmsInfo,m_ListCellVoltage,new List<H5BmsInfo>());//保存Bms数据文件头
                }

                int _interval = SelectCANWnd.m_RecordInterval;

                //msgQueue = new Queue<string>();
                timer.Interval = new TimeSpan(0, 0, _interval);
                timer.Start();

                _fs = new FileStream(bmsfilePath, System.IO.FileMode.Append, System.IO.FileAccess.Write);

                sw = new StreamWriter(_fs, System.Text.Encoding.Default);
            }
            else
            {
                BqProtocol.BqInstance.m_bIsSaveBmsInfo = false;
                if (fs != null)
                {
                    fs.Close();
                    bmsfilePath = string.Empty;
                }
                if (timer != null)
                    timer.Stop();
                sw.Close();
                _fs.Close();
            }

        }

        private void OnTimer(object sender, EventArgs e)
        {
            string str = System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒");
            string strLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47}",
                    System.DateTime.Now.ToString("yyyy年MM月dd日 HH时mm分ss秒"), m_ListBmsInfo[0].StrValue, m_ListBmsInfo[1].StrValue, m_ListBmsInfo[2].StrValue, m_ListBmsInfo[3].StrValue, m_ListBmsInfo[4].StrValue,
                    m_ListBmsInfo[5].StrValue, m_ListBmsInfo[6].StrValue, m_ListBmsInfo[7].StrValue, m_ListBmsInfo[8].StrValue, m_ListBmsInfo[9].StrValue, m_ListBmsInfo[10].StrValue, m_ListBmsInfo[11].StrValue,
                    m_ListBmsInfo[12].StrValue, m_ListBmsInfo[13].StrValue, m_ListBmsInfo[14].StrValue, m_ListBmsInfo[15].StrValue, m_ListBmsInfo[16].StrValue, m_ListBmsInfo[17].StrValue, m_ListBmsInfo[18].StrValue,
                    m_ListBmsInfo[19].StrValue, m_ListBmsInfo[20].StrValue, m_ListBmsInfo[21].StrValue, m_ListBmsInfo[22].StrValue, m_ListBmsInfo[23].StrValue, m_ListBmsInfo[24].StrValue,
                    m_ListBmsInfo[25].StrValue, m_ListBmsInfo[26].StrValue,  m_ListBmsInfo[27].StrValue, m_ListBmsInfo[28].StrValue, m_ListCellVoltage[16].StrValue, m_ListCellVoltage[17].StrValue,m_ListCellVoltage[0].StrValue,
                    m_ListCellVoltage[1].StrValue, m_ListCellVoltage[2].StrValue,m_ListCellVoltage[3].StrValue, m_ListCellVoltage[4].StrValue, m_ListCellVoltage[5].StrValue, m_ListCellVoltage[6].StrValue, m_ListCellVoltage[7].StrValue,
                    m_ListCellVoltage[8].StrValue, m_ListCellVoltage[9].StrValue,m_ListCellVoltage[10].StrValue, m_ListCellVoltage[11].StrValue, m_ListCellVoltage[12].StrValue, m_ListCellVoltage[13].StrValue, m_ListCellVoltage[14].StrValue, m_ListCellVoltage[15].StrValue);
            if (!string.IsNullOrEmpty(strLine))
            {
                sw.WriteLine(strLine); ;
            }
        }


        bool isDeepSleep = false;//在下发消息命令的时候增加此bool变量，拒绝总线上的其他回复消息
        public event EventHandler DeepSleepEvent;
        public event EventHandler ShallowSleepEvent;
        private void btnDeepSleep_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                var resistance = m_ListBmsInfo.FirstOrDefault(p => p.Description == "进水阻抗");
                if(resistance != null)
                {
                    int resistanceVal = Int32.Parse(resistance.StrValue);
                    if (Math.Abs(resistanceVal * Math.Pow(10, 3)) < Math.Abs(OneClickFactorySetting.m_minWaterResistance * Math.Pow(10, 3))
                        || Math.Abs(resistanceVal * Math.Pow(10, 3)) > Math.Abs(OneClickFactorySetting.m_maxWaterResistance * Math.Pow(10, 3)))
                    {
                        RefreshResult("失败", false);
                        ShowMessage("进水阻抗检验失败，进水阻抗不在范围内 ！", false);
                        return;
                    }
                    else
                    {
                        ShowMessage("进水阻抗检验成功！", true);
                    }
                }
                else
                {
                    RefreshResult("失败", false);
                    ShowMessage("找不到进水阻抗检测项，请确认检测项 ！", false);
                    return;
                }
                var soc = m_ListBmsInfo.FirstOrDefault(p => p.Description == "SOC");
                if(soc != null)
                {
                    int socVal = Int32.Parse(soc.StrValue);
                    if(socVal != 0)
                    {
                        RefreshResult("失败", false);
                        ShowMessage("SOC值检测失败，SOC值不为0 ！", false);
                        return;
                    }
                    else
                    {
                        RefreshResult("成功", true);
                        ShowMessage("SOC值检测成功！", true);
                    }
                }
                else
                {
                    RefreshResult("失败", false);
                    ShowMessage("找不到SOC检测项，请确认检测项 ！", false);
                    return;
                }
                isDeepSleep = true;
                BqProtocol.bReadBqBmsResp = true;
                BqProtocol.BqInstance.m_bIsStopCommunication = true;
                Thread.Sleep(600);
                BqProtocol.BqInstance.BQ_Shutdown();
            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }
        bool isShallowSleep = false;//在下发消息命令的时候增加此bool变量，拒绝总线上的其他回复消息
        private void btnShallowSleep_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                isShallowSleep = true;
                BqProtocol.bReadBqBmsResp = true;
                BqProtocol.BqInstance.m_bIsStopCommunication = true;
                Thread.Sleep(600);
                BqProtocol.BqInstance.BQ_Sleep();

            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }

        public void HandleDebugEvent(object sender, CustomRecvDataEventArgs e)
        {
            if(isDeepSleep || isShallowSleep || isAlterSOC)
            {
                BqProtocol.bReadBqBmsResp = true;
                switch (e.RecvMsg[0])
                {
                    case 0xD4:
                        isDeepSleep = false;
                        BqProtocol.BqInstance.m_bIsStopCommunication = false;
                        DeepSleepEvent?.Invoke(this, EventArgs.Empty);//设置深休眠成功，断开连接
                        ShowMessage("进入深休眠状态设置成功！", true);
                        break;
                    case 0xD5:
                        isShallowSleep = false;
                        BqProtocol.BqInstance.m_bIsStopCommunication = false;
                        ShallowSleepEvent?.Invoke(this, EventArgs.Empty);//设置浅休眠成功，断开连接
                        ShowMessage("进入浅休眠状态设置成功！", true);
                        break;
                    case 0xD2:
                        if (isAlterSOC)
                        {
                            ShowMessage("SOC设置成功！", true);
                            isAlterSOC = false;
                        }
                        break;
                    default:
                        break;
                }
            }
        }
        //bool isRefresh = true;
        private void dgBqBmsInfo_MouseLeftDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            //if (cbIsUpdate.IsChecked == true)
            //{
            //    MessageBox.Show("写入制造信息前请先停止数据刷新！", "写入制造信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
            //    return;
            //}

            //int index = 0;
            //foreach (var it in m_ListBmsInfo)
            //{
            //    if (it.Description == "制造信息")
            //        break;
            //    index++;
            //}
            //DataGridCell cell = DataGridExtension.GetCell(dgBqBmsInfo, index, 2);
            //cell.IsEditing = true;

            //cell.KeyDown += SelectCell_KeyDownEvent;
        }

        bool isWriteManufacture = false;//在下发消息命令的时候增加此bool变量，拒绝总线上的其他回复消息
        private void SelectCell_KeyDownEvent(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                if (MainWindow.m_statusBarInfo.IsOnline)
                {
                    DataGridCell cell = sender as DataGridCell;
                    string str = (cell.Content as TextBox).Text.Trim();
                    if (m_DicManufactureCode.ContainsKey(str))
                    {
                        BqProtocol.BqInstance.BQ_WriteManufacturingInformation(str);
                        isWriteManufacture = true;
                    }
                    else
                    {
                        MessageBox.Show("输入制造信息错误，请查阅相关文档！", "写入制造信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    MessageBoxForm.Show("系统未连接，请连接后再进行操作！", "提示", 1000);
                    //MessageBox.Show("系统未连接，请先连接设备再设置制造信息！", "写入制造信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    tbSn.Text = string.Empty;
                }
            }
        }

        public void HandleWriteManufacturingInfoEvent(object sender, CustomRecvDataEventArgs e)
        {
            if(isWriteManufacture)
            {
                BqProtocol.bReadBqBmsResp = true;
                if (e.RecvMsg[0] == 0xDE)
                {
                    MessageBoxForm.Show("写入 制造信息 参数成功！", "写入制造信息提示", 1000);
                    //MessageBox.Show("写入 制造信息 参数成功！", "写入制造信息提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    isWriteManufacture = false;
                }
            }
        }

        #region  处理条码的写入问题，需先读MCU数据，再写入MCU数据
        bool isScanSn = false;
        bool isWriteSn = false;
        bool isReadMCU = false;
        bool isWriteMCU = false;
        public void HandleRecvMcuDataEvent(object sender, CustomRecvDataEventArgs e)
        {
            if(isReadMCU)
            {
                if (BqUpdateMcuInfo(e.RecvMsg))//读取MCU参数成功，再进行参数下发
                {
                    isScanSn = false;
                    isWriteSn = true;
                    //ListSysInfo2[9].StrValue = tbSn.Text.Trim();
                    ListSysInfo2[10].StrValue = tbSn.Text.Trim();//工厂上位机输入条码为BMS条码
                    byte[] mcuData = new byte[256];
                    int len = 0;
                    if (!GetMcuDataBuf(mcuData, ref len))
                    {
                        //MessageBox.Show("写入 条码 参数失败，请检查输入数据！", "写入 MCU 提示", MessageBoxButton.OK, MessageBoxImage.Error);
                        ShowMessage("写入 条码 参数失败，请检查输入数据！", false);
                        return;
                    }
                    System.Threading.Tasks.Task.Factory.StartNew(() =>
                    {
                        int count = 0;
                        while (isWriteSn)
                        {
                            if (count > 5)
                            {
                                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                {
                                    //MessageBoxForm.Show("写入 条码 参数失败，请重新操作！", "写入 MCU 提示", 3000);
                                    //System.Windows.MessageBox.Show("写入条码数据失败，请重新操作！", "写入条码提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ShowMessage("写入条码数据失败，请重新操作！", false);
                                    tbSn.Text = string.Empty;
                                    tbSn.Focus();

                                }), null);
                                BqProtocol.BqInstance.m_bIsStopCommunication = false;
                                break;
                            }
                            isWriteMCU = false;
                            BqProtocol.BqInstance.SendMultiFrame(mcuData, len, 0xB2);
                            isWriteMCU = true;
                            System.Threading.Thread.Sleep(200);
                            count++;
                        }
                    });
                }
                else
                {
                    //MessageBox.Show("MCU参数读取失败，请查找原因！", "读取MCU参数提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                isReadMCU = false;
            }
        }

        public void HandleWriteMcuDataEvent(object sender, CustomRecvDataEventArgs e)
        {
            if(isWriteMCU)
            {
                BqProtocol.bReadBqBmsResp = true;
                if (e.RecvMsg[0] == 0xB2 || e.RecvMsg.Count == 0x03)
                {
                    isWriteSn = false;
                    isWriteMCU = false;
                    //MessageBoxForm.Show("写入 条码 参数失败，请检查输入数据！", "写入 MCU 提示", 3000);
                    //MessageBox.Show("写入 条码 参数成功！", "写入条码提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowMessage("写入 条码 参数成功！", true);
                    BqProtocol.BqInstance.m_bIsStopCommunication = false;
                    System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        tbSn.Text = string.Empty;
                        tbSn.Focus();
                    }), null);
                }
            }
        }

        private bool GetMcuDataBuf(byte[] mcuBuf, ref int nMcuIndex)
        {
            bool bRet = false;

            try
            {
                GetListBytes(mcuBuf, ListSysInfo1, ref nMcuIndex);

                GetListBytes(mcuBuf, ListSysInfo2, ref nMcuIndex);

                nMcuIndex += 5;

                GetListBytes(mcuBuf, ListChargeInfo, ref nMcuIndex);

                bRet = true;
            }
            catch (Exception ex)
            {

            }
            return bRet;

        }

        private void GetListBytes(byte[] mcuBuf, List<H5BmsInfo> ListInfo, ref int nIndex)
        {
            for (int n = 0; n < ListInfo.Count; n++)
            {
                switch (ListInfo[n].ByteCount)
                {
                    case 1:

                        mcuBuf[nIndex] = Get1BytesValue(ListInfo[n]);

                        break;
                    case 2:
                        byte[] arr2Byte = Get2BytesValue(ListInfo[n]);
                        mcuBuf[nIndex] = arr2Byte[0];
                        mcuBuf[nIndex + 1] = arr2Byte[1];
                        break;
                    case 4:
                        byte[] arr4Byte = null;

                        arr4Byte = Get4BytesValue(ListInfo[n]);
                        for (int m = 0; m < arr4Byte.Length; m++)
                        {
                            mcuBuf[nIndex + m] = arr4Byte[m];
                        }

                        break;
                    case 8:
                        break;
                    case 16:
                        if (ListInfo[n].StrValue != null)
                        {
                            byte[] arr16Byte = System.Text.Encoding.ASCII.GetBytes(ListInfo[n].StrValue);
                            for (int k = 0; k < arr16Byte.Length; k++)
                            {
                                mcuBuf[nIndex + k] = arr16Byte[k];
                            }
                        }
                        break;
                }

                nIndex += ListInfo[n].ByteCount;
            }
        }
        private byte Get1BytesValue(H5BmsInfo nodeInfo)
        {
            byte tempVal = 0;
            if (nodeInfo.Unit == "Hex")
            {
                tempVal = Convert.ToByte(nodeInfo.StrValue, 16);
            }
            else
            {
                if (nodeInfo.MinValue < 0)
                {
                    sbyte sb = 0;
                    sbyte.TryParse(nodeInfo.StrValue, out sb);
                    tempVal = Convert.ToByte(sb & 0xFF);//sbyte.Parse(ListInfo[n].StrValue);
                }
                else
                {
                    tempVal = byte.Parse(nodeInfo.StrValue);
                }
            }

            return tempVal;
        }

        private byte[] Get2BytesValue(H5BmsInfo nodeInfo)
        {
            byte[] arr2Byte = new byte[2];
            switch (nodeInfo.Description)
            {
                case "MCU配置参数":
                case "序列号":
                case "电池化学ID":

                    if (nodeInfo.StrValue.Length == 4)
                    {
                        arr2Byte[0] = Convert.ToByte(nodeInfo.StrValue.Substring(0, 2), 16);

                        arr2Byte[1] = Convert.ToByte(nodeInfo.StrValue.Substring(2, 2), 16);
                    }
                    else
                    {
                        arr2Byte[0] = 0x00;
                        arr2Byte[1] = 0x00;
                    }
                    break;

                case "软件版本":
                case "硬件版本":

                    byte tempVal2 = 0;

                    if (nodeInfo.StrValue.IndexOf('.') >= 0)
                    {
                        string[] strVer = nodeInfo.StrValue.Split('.');

                        arr2Byte[0] = Convert.ToByte(strVer[0], 16);

                        arr2Byte[1] = Convert.ToByte(strVer[1], 16);
                    }
                    else
                    {
                        arr2Byte[0] = 0x00;
                        arr2Byte[1] = 0x00;
                    }
                    break;


                default:
                    if (nodeInfo.MinValue == 0)
                    {
                        UInt16 tempVal4 = 0;
                        UInt16.TryParse(nodeInfo.StrValue, out tempVal4);

                        arr2Byte[0] = (byte)((tempVal4 & 0xFF00) >> 8);
                        arr2Byte[1] = (byte)(tempVal4 & 0x00FF);
                    }
                    else
                    {
                        Int16 tempVal4 = 0;
                        Int16.TryParse(nodeInfo.StrValue, out tempVal4);
                        arr2Byte[0] = (byte)((tempVal4 & 0xFF00) >> 8);
                        arr2Byte[1] = (byte)(tempVal4 & 0x00FF);
                    }
                    break;
            }
            return arr2Byte;

        }

        private byte[] Get4BytesValue(H5BmsInfo nodeInfo)
        {
            byte[] arr4Byte = new byte[4];
            switch (nodeInfo.Description)
            {
                case "生产日期":

                    DateTime dtVal;
                    DateTime.TryParse(nodeInfo.StrValue, out dtVal);
                    if (dtVal == null)
                    {
                        dtVal = DateTime.Now;
                    }

                    arr4Byte[0] = (byte)((dtVal.Year & 0xFF00) >> 8);
                    arr4Byte[1] = (byte)(dtVal.Year & 0x00FF);
                    arr4Byte[2] = (byte)(dtVal.Month);
                    arr4Byte[3] = (byte)(dtVal.Day);

                    break;
                default:
                    int tempVal2 = 0;
                    int.TryParse(nodeInfo.StrValue, out tempVal2);

                    arr4Byte = BitConverter.GetBytes(tempVal2);

                    Array.Reverse(arr4Byte);

                    break;
            }
            return arr4Byte;
        }
        #endregion

        private void tbSn_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                {
                    InitInterfaceShow();
                    if (MainWindow.m_statusBarInfo.IsOnline)
                    {
                        string str = tbSn.Text.Trim();
                        if (str.Length <= 16)
                        {
                            isScanSn = true;
                            BqProtocol.BqInstance.m_bIsStopCommunication = true;
                            System.Threading.Tasks.Task.Factory.StartNew(() =>
                            {
                            int count = 0;
                            while (isScanSn)
                            {
                                    if (count > 5)
                                    {
                                        System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                                       {
                                           ShowMessage("读取MCU数据失败，请重新操作！", false);
                                           tbSn.Text = string.Empty;
                                           tbSn.Focus();

                                       }), null);
                                        BqProtocol.BqInstance.m_bIsStopCommunication = false;
                                        break;
                                    }
                                    BqProtocol.bReadBqBmsResp = true;
                                    BqProtocol.BqInstance.ReadMcuData();
                                    isReadMCU = true;
                                    System.Threading.Thread.Sleep(100);
                                    count++;
                                }
                            });
                        }
                        else
                        {
                            ShowMessage("输入条码长度大于16位，请检查！", false);
                        }
                    }
                    else
                    {
                        tbSn.Text = string.Empty;
                        ShowMessage("系统未连接，请先连接设备再进行操作！", false);
                    }
                }
            }
        }

        bool isAlterSOC = false;
        private void btnAdjustSOC_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline == true)
            {
                isAlterSOC = false;
                string str = @"^[0-9]{1,3}$";
                if (!Regex.IsMatch(XmlHelper.m_strSOCValue, str))
                {
                    ShowMessage("请输入正确的 SOC 值！", false);
                    return;
                }
                byte socVal = byte.Parse(XmlHelper.m_strSOCValue);

                if (socVal < 0 || socVal > 100)
                {
                    ShowMessage("请输入正确的 SOC 值！", false);
                    return;
                }
                
                BoqiangH5.BQProtocol.BqProtocol.BqInstance.BQ_AlterSOC(socVal);
                isAlterSOC = true;
            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }

        bool isAdjustZeroCurrent = false;
        private void btnAdjustZero_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline == true)
            {
                isAdjustZeroCurrent = false;
                BqProtocol.BqInstance.AdjustZeroCurrent(0);
                isAdjustZeroCurrent = true;
            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }

        public void HandleAdjustZeroCurrenEvent(object sender, CustomRecvDataEventArgs e)
        {
            if (isAdjustZeroCurrent)
            {
                BqProtocol.bReadBqBmsResp = true;
                if (e.RecvMsg[0] == 0xC1 || e.RecvMsg.Count == 0x03)
                {
                    ShowMessage("校准零点电流成功！", true);
                }
                else
                {
                    ShowMessage("校准零点电流失败！", false);
                }
                isAdjustZeroCurrent = false;
            }
        }
        bool isAdjust10ACurrent = false;
        private void btnAdjust10A_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline == true)
            {
                if (SelectCANWnd.m_IsUsingKP182)//如果启用KP182
                {
                    if ((MainWindow.serialPort != null) && (MainWindow.serialPort.IsOpen == true))
                    {
                        SetKP182CurrentMode(MainWindow.serialPort,10000);//设置KP182拉载模式为CC模式
                    }
                }
                else
                {
                    if (MessageBoxResult.OK == MessageBox.Show("请开启负载设备进行放电，配置完成后再点击确定！", "-10A校准提示", MessageBoxButton.OKCancel, MessageBoxImage.Information))
                    {
                        isAdjust10ACurrent = false;
                        BqProtocol.BqInstance.AdjustRtCurrent((int)((-10) * Math.Pow(10, 3)));
                        isAdjust10ACurrent = true;
                    }
                };
            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }

        public void HandleAdjust10AEvent(object sender, CustomRecvDataEventArgs e)
        {
            if (isAdjust10ACurrent)
            {
                BqProtocol.bReadBqBmsResp = true;
                if (SelectCANWnd.m_IsUsingKP182)//如果启用KP182
                {
                    if ((MainWindow.serialPort != null) && (MainWindow.serialPort.IsOpen == true))
                    {
                        SetKP182CurrentSwitchOFF(MainWindow.serialPort);
                    }
                }

                if (e.RecvMsg[0] == 0xC2 || e.RecvMsg.Count == 0x03)
                {
                    ShowMessage("10A电流校准成功！", true);
                }
                else
                {
                    ShowMessage("10A电流校准失败！", false);
                }
                isAdjust10ACurrent = false;
            }
        }
        public event EventHandler<EventArgs<string>> WriteMcuEvent;
        private void btnWriteMcu_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (!string.IsNullOrEmpty(OneClickFactorySetting.m_MCUFilePath))
            {
                if (MainWindow.m_statusBarInfo.IsOnline)
                {
                    WriteMcuEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_MCUFilePath.Trim()));
                }
                else
                {
                    ShowMessage("系统未连接，请先连接设备再进行操作！", false);
                }
            }
            else
            {
                ShowMessage("请先选择要写入的MCU文件！", false);
            }
        }

        public event EventHandler<EventArgs<string>> WriteEepromEvent;
        private void btnWriteEeprom_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (!string.IsNullOrEmpty(OneClickFactorySetting.m_EepromFilePath))
            {
                if (MainWindow.m_statusBarInfo.IsOnline)
                {
                    WriteEepromEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_EepromFilePath.Trim()));
                }
                else
                {
                    ShowMessage("系统未连接，请先连接设备再进行操作！", false);
                }
            }
            else
            {
                ShowMessage("请先选择要写入的Eeprom文件！", false);
            }
        }

        public void AutoStartOneClickFactory()
        {
            btnOneClickFactory_Click(null,null);
        }
        #region  一键出厂配置
        public event EventHandler RequireReadRTCEvent;
        public event EventHandler RequireSecondReadRTCEvent;
        public event EventHandler RequireAdjustRTCEvent;
        public event EventHandler RequireReadBootMsgEvent;
        public event EventHandler<EventArgs<string>> RequireAdjustSOCEvent;
        public event EventHandler RequireAdjustZeroCurrentEvent;
        public event EventHandler RequireAdjust10ACurrentEvent;
        public event EventHandler<EventArgs<string>> RequireWriteEepromEvent;
        public event EventHandler RequireWriteMcuEvent;
        //public EventWaitHandle waitHandle = new AutoResetEvent(false);
        public event EventHandler RequireReadRecordEvent;//读一条备份数据，检查Eeprom是否异常
        bool isOneClickFactoryConfig = false;
        public long count = 0;
        private void btnOneClickFactory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InitInterfaceShow();
                if (MainWindow.m_statusBarInfo.IsOnline)
                {
                    ShowMessage("一键出厂配置开始！", true);
                    //CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置开始");
                    RefreshResult("配置中", false);
                    BqProtocol.BqInstance.m_bIsOneClick = true;
                    isOneClickFactoryConfig = true;
                    Thread.Sleep(300);
                    //OneClickEvent?.Invoke(this, new EventArgs<bool>(true));
                    //RequireReadRecordEvent?.Invoke(this, EventArgs.Empty);//暂取消Eeprom检测
                    if (OneClickFactorySetting.m_WriteEeprom == 1)
                    {
                        RequireWriteEepromEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_EepromFilePath.Trim()));
                    }
                    else
                    {
                        ////写入MCU
                        if (OneClickFactorySetting.m_WriteMCU == 1)
                        {
                            RequireWriteMcuEvent?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            ////部件型号校验
                            RequireReadBootMsgEvent?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
                else
                {
                    //MessageBoxForm.Show("系统未连接，请先连接设备再进行操作！", "提示", 1000);
                    ShowMessage("系统未连接，请先连接设备再进行操作！", false);
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void GetRecordsOver(bool isOK)
        {
            if(isOK)
            {
                //AutoClosedMsgBox.Show("读取故障记录成功，检查Eeprom正常！", "检查Eeprom提示", 500, 64);
                ShowMessage("读取故障记录成功，检查Eeprom正常！", true);
                if (isOneClickCheck)
                {
                    if (OneClickFactorySetting.m_VoltageCheck == 1)
                    {
                        ////电压核验
                        if (isVoltageDiffHigh)
                        {
                            RefreshResult("失败", false);
                            //MessageBox.Show("电芯电压核验失败，压差过大！", "电压核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            ShowMessage("电芯电压核验失败，压差过大！", false);
                            BqProtocol.BqInstance.m_bIsOneClick = false;
                            isOneClickCheck = false;
                            return;
                        }
                        else
                        {
                            //MessageBox.Show("电芯电压检验成功！", "电压检验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            //AutoClosedMsgBox.Show("电芯电压检验成功！", "电压检验提示", 500, 64);
                            ShowMessage("电芯电压检验成功！", true);
                        }
                    }
                    Thread.Sleep(100);
                    double temperature = 0, humidity = 0;
                    if (OneClickFactorySetting.m_TemperatureCheck == 1)
                    {
                        if (XmlHelper.m_strIsUsingTH10SB == "1")
                        {
                            if (MainWindow.th10sbSerialPort != null)
                            {
                                if (MainWindow.th10sbSerialPort.IsOpen)
                                {
                                    ReadTH10SBVaule(out temperature, out humidity);
                                    if (temperature != 0)
                                    {
                                        bool isTempDiffNG = false;
                                        foreach (var item in m_ListBmsInfo)
                                        {
                                            if (item.Description == "环境温度" || item.Description == "电芯温度1" || item.Description == "电芯温度2" || item.Description == "电芯温度3"
                                                || item.Description == "电芯温度4" || item.Description == "单体最大温度" || item.Description == "单体最小温度")
                                            {
                                                double val = double.Parse(item.StrValue);
                                                if (Math.Abs(val - temperature) > OneClickFactorySetting.m_TemperatureError)
                                                {
                                                    isTempDiffNG = true;
                                                }
                                            }
                                        }
                                        ////温度核验
                                        if (isTempDiffNG)
                                        {
                                            RefreshResult("失败", false);
                                            //MessageBox.Show("电芯温度核验失败，温差过大！", "温度核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                            ShowMessage("电芯温度核验失败，温差过大！", false);
                                            isOneClickCheck = false;
                                            BqProtocol.BqInstance.m_bIsOneClick = false;
                                            return;
                                        }
                                        else
                                        {
                                            //AutoClosedMsgBox.Show("电芯温度检验成功！", "温度检验提示", 500, 64);
                                            ShowMessage("电芯温度检验成功！", true);
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            ////温度核验
                            if (isTempDiffHigh)
                            {
                                RefreshResult("失败", false);
                                //MessageBox.Show("电芯温度核验失败，温差过大！", "温度核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                ShowMessage("电芯温度核验失败，温差过大！", false);
                                BqProtocol.BqInstance.m_bIsOneClick = false;
                                isOneClickCheck = false;
                                return;
                            }
                            else
                            {
                                //MessageBox.Show("电芯温度检验成功！", "温度检验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                //AutoClosedMsgBox.Show("电芯温度检验成功！", "温度检验提示", 500, 64);
                                ShowMessage("电芯温度检验成功！", true);
                            }
                        }
                    }
                    //循环放电次数检查
                    if (isLoopNumberIsOK)
                    {
                        //AutoClosedMsgBox.Show("循环放电次数检验成功！", "循环放电次数检验提示", 500, 64);
                        ShowMessage("循环放电次数检验成功！", true);
                    }
                    else
                    {
                        RefreshResult("失败", false);
                        //MessageBox.Show("循环放电次数检验失败，循环放电次数不为 0 ！", "循环放电次数检验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowMessage("循环放电次数检验失败，循环放电次数不为 0 ！", false);
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        isOneClickCheck = false;
                        return;
                    }

                    var _item = m_ListBmsInfo.FirstOrDefault(p => p.Description == "SOC");
                    if(_item.StrValue == "0" || _item.StrValue == "1")
                    {
                        ShowMessage("SOC值检验成功！", true);
                    }
                    else
                    {
                        RefreshResult("失败", false);
                        ShowMessage("SOC检验失败，SOC值不为 0或1 ！", false);
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        isOneClickCheck = false;
                        return;
                    }

                    var resistance = m_ListBmsInfo.FirstOrDefault(p => p.Description == "进水阻抗");
                    int resistanceVal = Int32.Parse(resistance.StrValue);
                    if (Math.Abs(resistanceVal * Math.Pow(10,3)) < Math.Abs(OneClickFactorySetting.m_minWaterResistance * Math.Pow(10,3)) 
                        || Math.Abs(resistanceVal * Math.Pow(10, 3)) > Math.Abs(OneClickFactorySetting.m_maxWaterResistance * Math.Pow(10, 3)))
                    {
                        RefreshResult("失败", false);
                        ShowMessage("进水阻抗检验失败，进水阻抗不在范围内 ！", false);
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        isOneClickCheck = false;
                        return;
                    }
                    else
                    {
                        ShowMessage("进水阻抗检验成功！", true);
                    }
                    //湿度核验
                    if (OneClickFactorySetting.m_HumidityCheck == 1)
                    {
                        if (XmlHelper.m_strIsUsingTH10SB == "1")
                        {
                            if (humidity != 0)
                            {
                                string humidityStr = m_ListBmsInfo.FirstOrDefault(p => p.Description == "湿度").StrValue;
                                double val = double.Parse(humidityStr);
                                if (Math.Abs(val - humidity) > OneClickFactorySetting.m_HumidityError)
                                {
                                    RefreshResult("失败", false);
                                    //MessageBox.Show("电芯湿度核验失败，湿差过大！", "温度核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ShowMessage("电芯湿度核验失败，湿差过大！", false);
                                    BqProtocol.BqInstance.m_bIsOneClick = false;
                                    isOneClickCheck = false;
                                    return;
                                }
                                else
                                {
                                    //AutoClosedMsgBox.Show("电芯湿度检验成功！", "湿度检验提示", 500, 64);
                                    ShowMessage("电芯湿度检验成功！", true);
                                }
                            }
                        }
                    }
                    Thread.Sleep(100);
                    //RTC检验
                    RequireReadRTCEvent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ////写入Eeprom
                    if (OneClickFactorySetting.m_WriteEeprom == 1)
                    {
                        RequireWriteEepromEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_EepromFilePath.Trim()));
                    }
                    else
                    {
                        ////写入MCU
                        if (OneClickFactorySetting.m_WriteMCU == 1)
                        {
                            RequireWriteMcuEvent?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            ////部件型号校验
                            RequireReadBootMsgEvent?.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
            }
            else
            {
                RefreshResult("失败", false);
                //MessageBox.Show("读取故障记录失败，Eeprom可能损坏，请检查！", "检查Eeprom提示", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowMessage("读取故障记录失败，Eeprom可能损坏，请检查！", false);
                BqProtocol.BqInstance.m_bIsOneClick = false;
            }
        }
        public void WriteEepromOver(bool isOK)
        {
            if (isOK)
            {
                //AutoClosedMsgBox.Show("写入Eeprom参数成功！", "写入Eeprom提示", 500, 64);
                ShowMessage("写入Eeprom参数成功！", true);
                Thread.Sleep(100);
                ////写入MCU
                if (OneClickFactorySetting.m_WriteMCU == 1)
                {
                    RequireWriteMcuEvent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    ////部件型号校验
                    RequireReadBootMsgEvent?.Invoke(this, EventArgs.Empty);
                }
            }
            else
            {
                RefreshResult("失败", false);
                //MessageBox.Show("写入Eeprom参数失败！", "写入Eeprom提示", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowMessage("写入Eeprom参数失败！", false);
                BqProtocol.BqInstance.m_bIsOneClick = false;
            }
        }

        public void WriteMcuOver(bool isOK)
        {
            try
            {
                if (isOK)
                {
                    //AutoClosedMsgBox.Show("写入MCU参数成功！", "写入MCU提示", 500, 64);
                    ShowMessage("写入MCU参数成功！", true);
                    Thread.Sleep(200);
                    ////部件型号校验
                    RequireReadBootMsgEvent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    RefreshResult("失败", false);
                    //MessageBox.Show("写入MCU参数失败！", "写入MCU提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowMessage("写入MCU参数失败！", false);
                    BqProtocol.BqInstance.m_bIsOneClick = false;
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void ReadBootOver(string hardwareVersion)
        {
            if (hardwareVersion == OneClickFactorySetting.m_ComponentModel)
            {
                //AutoClosedMsgBox.Show("部件核验成功！", "部件核验提示", 500, 64);
                ShowMessage("部件核验成功！", true);
                //CSVFileHelper.WriteLogs("log", "一键出厂配置", "部件核验成功！");
                Thread.Sleep(200);
                if(OneClickFactorySetting.m_VoltageCheck == 1)
                {
                    ////电压核验
                    bool isVoltageDiffNG = false;
                    int index = 0;

                    foreach (var item in m_ListCellVoltage)
                    {
                        index++;
                        if (item.Description != "总电压" && item.Description != "实时电流")
                        {
                            double val = double.Parse(item.StrValue);
                            if (Math.Abs(Math.Abs(val) - Math.Abs(OneClickFactorySetting.m_VoltageBase)) > Math.Abs(OneClickFactorySetting.m_VoltageError))
                            {
                                isVoltageDiffNG = true;
                            }

                        }
                    }

                    if (isVoltageDiffNG)
                    {
                        RefreshResult("失败", false);
                        //MessageBox.Show("电芯电压核验失败，压差过大！", "电压核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowMessage("电芯电压核验失败，压差过大！", false);
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        return;
                    }
                    else
                    {
                        //AutoClosedMsgBox.Show("电芯电压核验成功！", "电压核验提示", 500, 64);
                        ShowMessage("电芯电压核验成功！", true);
                    }
                }
                //CSVFileHelper.WriteLogs("log", "一键出厂配置", "压差检查完毕！");
                Thread.Sleep(100);
                double temperature = 0, humidity = 0;
                if (OneClickFactorySetting.m_TemperatureCheck == 1)
                {
                    if (XmlHelper.m_strIsUsingTH10SB == "1")
                    {
                        if (MainWindow.th10sbSerialPort != null)
                        {
                            if (MainWindow.th10sbSerialPort.IsOpen)
                            {
                                ReadTH10SBVaule(out temperature, out humidity);
                                if (temperature != 0)
                                {
                                    bool isTempDiffNG = false;
                                    foreach (var item in m_ListBmsInfo)
                                    {
                                        if (item.Description == "环境温度" || item.Description == "电芯温度1" || item.Description == "电芯温度2" || item.Description == "电芯温度3"
                                            || item.Description == "电芯温度4" || item.Description == "单体最大温度" || item.Description == "单体最小温度" || item.Description == "功率温度")
                                        {
                                            double val = double.Parse(item.StrValue);
                                            if (Math.Abs(Math.Abs(val) - Math.Abs(temperature)) > Math.Abs(OneClickFactorySetting.m_TemperatureError))
                                            {
                                                isTempDiffNG = true;
                                            }
                                        }
                                    }
                                    ////温度核验
                                    if (isTempDiffNG)
                                    {
                                        RefreshResult("失败", false);
                                        //MessageBox.Show("电芯温度核验失败，温差过大！", "温度核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                        ShowMessage("电芯温度核验失败，温差过大！", false);
                                        BqProtocol.BqInstance.m_bIsOneClick = false;
                                        return;
                                    }
                                    else
                                    {
                                        //AutoClosedMsgBox.Show("电芯温度检验成功！", "温度检验提示", 500, 64);
                                        ShowMessage("电芯温度检验成功！", true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool isTempDiffNG = false;
                        var it = m_ListBmsInfo.FirstOrDefault(p => p.Description == "环境温度");
                        temperature = float.Parse(it.StrValue);
                        //StringBuilder sb = new StringBuilder();
                        foreach (var item in m_ListBmsInfo)
                        {
                             if (item.Description == "电芯温度1" || item.Description == "电芯温度2" || item.Description == "电芯温度3"
                                || item.Description == "电芯温度4" || item.Description == "单体最大温度" || item.Description == "单体最小温度" || item.Description == "功率温度")
                            {
                                double val = double.Parse(item.StrValue);
                                if (val > 127) val = val - 256;
                                if (Math.Abs(Math.Abs(val) - Math.Abs(temperature)) > Math.Abs(OneClickFactorySetting.m_TemperatureError))
                                {
                                    isTempDiffNG = true;
                                }
                                //string str = string.Format("{0} 值：{1}  基础值：{2}  误差设置值：{3}  ", item.Description, val, temperature, OneClickFactorySetting.m_TemperatureError);
                                //sb.Append(str);
                                //sb.Append("\r\n");
                            }
                        }
                        //sb.Append(isTempDiffNG ? "TRUE" : "FALSE");
                        //CSVFileHelper.WriteLogs("voltageDiff", "温差测试", sb.ToString());
                        ////温度核验
                        if (isTempDiffNG)
                        {
                            RefreshResult("失败", false);
                            //MessageBox.Show("电芯温度核验失败，温差过大！", "温度核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            ShowMessage("电芯温度核验失败，温差过大！", false);
                            BqProtocol.BqInstance.m_bIsOneClick = false;
                            return;
                        }
                        else
                        {
                            //MessageBox.Show("电芯温度检验成功！", "温度检验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                            //AutoClosedMsgBox.Show("电芯温度检验成功！", "温度检验提示", 500, 64);
                            ShowMessage("电芯温度检验成功！", true);
                        }
                    }
                }
                //CSVFileHelper.WriteLogs("log", "一键出厂配置", "温差检查完毕！");
                Thread.Sleep(100);
                //var resistance = m_ListBmsInfo.FirstOrDefault(p => p.Description == "进水阻抗");
                //int resistanceVal = Int32.Parse(resistance.StrValue);
                //if (Math.Abs(resistanceVal * Math.Pow(10, 3)) < Math.Abs(OneClickFactorySetting.m_minWaterResistance * Math.Pow(10, 3))
                //    || Math.Abs(resistanceVal * Math.Pow(10, 3)) > Math.Abs(OneClickFactorySetting.m_maxWaterResistance * Math.Pow(10, 3)))
                //{
                //    RefreshResult("失败", false);
                //    ShowMessage("进水阻抗检验失败，进水阻抗不在范围内 ！", false);
                //    BqProtocol.BqInstance.m_bIsOneClick = false;
                //    isOneClickCheck = false;
                //    return;
                //}
                //else
                //{
                //    ShowMessage("进水阻抗检验成功！", true);
                //}
                //CSVFileHelper.WriteLogs("log", "一键出厂配置", "进水阻抗检查完毕！");
                //湿度核验
                if (OneClickFactorySetting.m_HumidityCheck == 1)
                {
                    if (XmlHelper.m_strIsUsingTH10SB == "1")
                    {
                        if (humidity != 0)
                        {
                            string humidityStr = m_ListBmsInfo.FirstOrDefault(p => p.Description == "湿度").StrValue;
                            double val = double.Parse(humidityStr);
                            if (Math.Abs(val - humidity) > OneClickFactorySetting.m_HumidityError)
                            {
                                RefreshResult("失败", false);
                                //MessageBox.Show("电芯湿度核验失败，湿差过大！", "温度核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                ShowMessage("电芯湿度核验失败，湿度差过大！", false);
                                BqProtocol.BqInstance.m_bIsOneClick = false;
                                return;
                            }
                            else
                            {
                                //AutoClosedMsgBox.Show("电芯湿度检验成功！", "湿度检验提示", 500, 64);
                                ShowMessage("电芯湿度检验成功！", true);
                            }
                        }
                    }
                }
                Thread.Sleep(100);
                //if (OneClickFactorySetting.m_RTCAdjust == 1)
                {
                    //CSVFileHelper.WriteLogs("log", "一键出厂配置", "请求RTC校准");
                    RequireAdjustRTCEvent?.Invoke(this, EventArgs.Empty);
                }
                //else
                //{
                //    ////零点校准
                //    if (OneClickFactorySetting.m_ZeroAdjust == 1)
                //        RequireAdjustZeroCurrentEvent?.Invoke(this, EventArgs.Empty);
                //    else
                //    {
                //        ////-10A校准
                //        if (OneClickFactorySetting.m_Current10AAdjust == 1)
                //        {
                //            if (SelectCANWnd.m_IsUsingKP182)//如果启用KP182
                //            {
                //                if ((MainWindow.serialPort != null) && (MainWindow.serialPort.IsOpen == true))
                //                {
                //                    SetKP182CurrentMode(10000);//设置KP182拉载模式为CC模式
                //                }
                //            }
                //            else
                //            {
                //                if (MessageBoxResult.OK == MessageBox.Show("请开启负载设备进行放电，配置完成后再点击确定！", "-10A校准提示", MessageBoxButton.OKCancel, MessageBoxImage.Information))
                //                {
                //                    RequireAdjust10ACurrentEvent?.Invoke(this, EventArgs.Empty);
                //                }
                //                else
                //                {

                //                    Thread.Sleep(200);
                //                    RefreshResult("完成", true);
                //                    //////浅休眠
                //                    if (OneClickFactorySetting.m_ShallowSleep == 1)
                //                    {
                //                        btnShallowSleep_Click(null, null);
                //                    }
                //                    else if(OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                //                    {
                //                        btnDeepSleep_Click(null, null);
                //                    }
                //                    BqProtocol.BqInstance.m_bIsOneClick = false;
                //                    CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置完成");
                //                    ShowMessage("一键出厂配置完成！", true);
                //                }
                //            }
                //        }
                //        else
                //        {
                //            Thread.Sleep(200);
                //            RefreshResult("完成", true);

                //            //////浅休眠
                //            if (OneClickFactorySetting.m_ShallowSleep == 1)
                //            {
                //                btnShallowSleep_Click(null, null);
                //            }
                //            else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                //            {
                //                btnDeepSleep_Click(null, null);
                //            }
                //            BqProtocol.BqInstance.m_bIsOneClick = false;
                //            CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置完成");
                //            ShowMessage("一键出厂配置完成！", true);
                //        }
                //    }

                //}
            }
            else
            {
                RefreshResult("失败", false);
                //MessageBox.Show("部件核验失败，请检查！", "部件核验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowMessage("部件核验失败，请检查！", false);
                BqProtocol.BqInstance.m_bIsOneClick = false;
            }
        }

        public void AdjustRTCOver(bool isOK)
        {
            try
            {
                if (isOK)
                {
                    //CSVFileHelper.WriteLogs("log", "一键出厂配置", "收到RTC校准完成命令");
                    //AutoClosedMsgBox.Show("校准RTC成功！", "RTC校准提示", 500, 64);
                    ShowMessage("校准RTC成功！", true);
                    Thread.Sleep(100);
                    ////零点校准
                    if (OneClickFactorySetting.m_ZeroAdjust == 1)
                        RequireAdjustZeroCurrentEvent?.Invoke(this, EventArgs.Empty);
                    else
                    {
                        ////-10A校准
                        if (OneClickFactorySetting.m_Current10AAdjust == 1)
                        {
                            if (SelectCANWnd.m_IsUsingKP182)//如果启用KP182
                            {
                                if ((MainWindow.serialPort != null) && (MainWindow.serialPort.IsOpen == true))
                                {
                                    SetKP182CurrentMode(MainWindow.serialPort, 10000);//设置KP182拉载模式为CC模式
                                }
                            }
                            else
                            {
                                if (MessageBoxResult.OK == MessageBox.Show("请开启负载设备进行放电，配置完成后再点击确定！", "-10A校准提示", MessageBoxButton.OKCancel, MessageBoxImage.Information))
                                {
                                    RequireAdjust10ACurrentEvent?.Invoke(this, EventArgs.Empty);
                                }
                                else
                                {
                                    Thread.Sleep(200);
                                    RefreshResult("完成", true);

                                    //////浅休眠
                                    if (OneClickFactorySetting.m_ShallowSleep == 1)
                                    {
                                        btnShallowSleep_Click(null, null);
                                    }
                                    else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                                    {
                                        btnDeepSleep_Click(null, null);
                                    }
                                    BqProtocol.BqInstance.m_bIsOneClick = false;
                                    //CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置完成");
                                    ShowMessage("一键出厂配置完成！", true);
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(200);
                            RefreshResult("完成", true);

                            //////浅休眠
                            if (OneClickFactorySetting.m_ShallowSleep == 1)
                            {
                                btnShallowSleep_Click(null, null);
                            }
                            else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                            {
                                btnDeepSleep_Click(null, null);
                            }
                            BqProtocol.BqInstance.m_bIsOneClick = false;
                            //CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置完成");
                            ShowMessage("一键出厂配置完成！", true);

                            Thread.Sleep(1000);
                            AutoChargeOrDischarge();
                        }
                    }
                }
                else
                {
                    RefreshResult("失败", false);
                    //MessageBox.Show("校准RTC失败！", "RTC校准提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowMessage("校准RTC失败！", false);
                    BqProtocol.BqInstance.m_bIsOneClick = false;
                }
            }
            catch(Exception ex)
            {

            }
        }

        public void AdjustZeroCurrentOver(bool isOK)
        {
            try
            {
                if (isOK)
                {
                    //AutoClosedMsgBox.Show("校准零点电流成功！", "零点电流校准提示", 500, 64);
                    //CSVFileHelper.WriteLogs("log", "一键出厂配置", "校准零点电流成功！");
                    ShowMessage("校准零点电流成功！", true);
                    Thread.Sleep(200);
                    ////-10A校准
                    if (OneClickFactorySetting.m_Current10AAdjust == 1)
                    {
                        if (SelectCANWnd.m_IsUsingKP182)//如果启用KP182
                        {
                            if ((MainWindow.serialPort != null) && (MainWindow.serialPort.IsOpen == true))
                            {
                                //CSVFileHelper.WriteLogs("log", "一键出厂配置", "开启电池负载！");
                                SetKP182CurrentMode(MainWindow.serialPort, 10000);//设置KP182拉载模式为CC模式
                            }
                        }
                        else
                        {
                            if (MessageBoxResult.OK == MessageBox.Show("请开启负载设备进行放电，配置完成后再点击确定！", "-10A校准提示", MessageBoxButton.OKCancel, MessageBoxImage.Information))
                            {
                                RequireAdjust10ACurrentEvent?.Invoke(this, EventArgs.Empty);
                            }
                            else
                            {
                                Thread.Sleep(200);
                                RefreshResult("完成", true);

                                //////浅休眠
                                if (OneClickFactorySetting.m_ShallowSleep == 1)
                                {
                                    btnShallowSleep_Click(null, null);
                                }
                                else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                                {
                                    btnDeepSleep_Click(null, null);
                                }
                                BqProtocol.BqInstance.m_bIsOneClick = false;
                                //CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置完成");
                                ShowMessage("一键出厂配置完成！", true);

                                Thread.Sleep(1000);
                                AutoChargeOrDischarge();
                            }
                        }
                    }
                    else
                    {
                        Thread.Sleep(200);
                        RefreshResult("完成", true);

                        //////浅休眠
                        if (OneClickFactorySetting.m_ShallowSleep == 1)
                        {
                            btnShallowSleep_Click(null, null);
                        }
                        else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                        {
                            btnDeepSleep_Click(null, null);
                        }
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        //CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置完成");
                        ShowMessage("一键出厂配置完成！", true);

                        Thread.Sleep(1000);
                        AutoChargeOrDischarge();
                    }
                }
                else
                {
                    RefreshResult("失败", false);
                    //MessageBox.Show("校准零点电流失败！", "零点电流校准提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowMessage("校准零点电流失败！", false);
                    BqProtocol.BqInstance.m_bIsOneClick = false;
                }
            }
            catch(Exception  ex)
            {

            }
        }

        public void Adjust10ACurrentOver(bool isOK)
        {
            try
            {
                if (isOK)
                {
                    //AutoClosedMsgBox.Show("校准-10A电流成功！", "-10A校准提示", 500, 64);
                    //CSVFileHelper.WriteLogs("log", "一键出厂配置", "校准-10A电流成功！");
                    ShowMessage("校准-10A电流成功！", true);
                    if (SelectCANWnd.m_IsUsingKP182 == true)
                    {
                        if ((MainWindow.serialPort != null) && (MainWindow.serialPort.IsOpen == true))
                        {
                            SetKP182CurrentSwitchOFF(MainWindow.serialPort);//关闭KP182拉载开关
                        }
                    }
                    Thread.Sleep(200);
                    RefreshResult("完成", true);

                    //////浅休眠
                    if (OneClickFactorySetting.m_ShallowSleep == 1)
                    {
                        btnShallowSleep_Click(null, null);
                    }
                    else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                    {
                        btnDeepSleep_Click(null, null);
                    }
                    BqProtocol.BqInstance.m_bIsOneClick = false;
                    //CSVFileHelper.WriteLogs("log", "一键出厂配置", "一键出厂配置完成");
                    ShowMessage("一键出厂配置完成！", true);

                    Thread.Sleep(1000);
                    AutoChargeOrDischarge();
                }
                else
                {
                    RefreshResult("失败", false);
                    //MessageBox.Show("校准-10A电流失败！", "-10A校准提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowMessage("校准-10A电流失败！", false);
                    BqProtocol.BqInstance.m_bIsOneClick = false;
                }
            }
            catch(Exception ex)
            {

            }
        }
        #endregion


        public event EventHandler RequirePowerOnEvent;
        private void btnPowerON_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                RequirePowerOnEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }
        public event EventHandler RequirePowerOffEvent;
        private void btnPowerOFF_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                RequirePowerOffEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }

        public void AutoStartOneClickCheck()
        {
            Thread.Sleep(100);
            btnOneClickFactoryCheck_Click(null, null);
        }
        #region 一键出厂校验
        public event EventHandler<EventArgs<string>> RequireCheckEepromEvent;
        //public event EventHandler<EventArgs<bool>> OneClickEvent;
        bool isOneClickCheck = false;
        private void btnOneClickFactoryCheck_Click(object sender, RoutedEventArgs e)
        {
            InitInterfaceShow();
            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                RefreshResult("检验中", false);
                //CSVFileHelper.WriteLogs("log", "一键出厂检验", "一键出厂检验开始");
                ShowMessage("一键出厂检验开始！", true);
                BqProtocol.BqInstance.m_bIsOneClick = true;
                //OneClickEvent?.Invoke(this, new EventArgs<bool>(true));
                Thread.Sleep(300);
                isOneClickCheck = true;
                //RequireReadRecordEvent?.Invoke(this, EventArgs.Empty);//暂取消Eeprom检测
                if (OneClickFactorySetting.m_VoltageCheck == 1)
                {
                    bool isVoltageDiffNG = false;
                    foreach (var item in m_ListCellVoltage)
                    {
                        if (item.Description != "总电压" && item.Description != "实时电流")
                        {
                            double val = double.Parse(item.StrValue);
                            if (Math.Abs(Math.Abs(val) - Math.Abs(OneClickFactorySetting.m_VoltageBase)) > Math.Abs(OneClickFactorySetting.m_VoltageError))
                            {
                                isVoltageDiffNG = true;
                            }
                        }
                    }
                    ////电压核验
                    if (isVoltageDiffNG)
                    {
                        RefreshResult("失败", false);
                        ShowMessage("电芯电压核验失败，压差过大！", false);
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        isOneClickCheck = false;
                        return;
                    }
                    else
                    {
                        ShowMessage("电芯电压检验成功！", true);
                    }
                }
                Thread.Sleep(100);
                double temperature = 0, humidity = 0;
                if (OneClickFactorySetting.m_TemperatureCheck == 1)
                {
                    if (XmlHelper.m_strIsUsingTH10SB == "1")
                    {
                        if (MainWindow.th10sbSerialPort != null)
                        {
                            if (MainWindow.th10sbSerialPort.IsOpen)
                            {
                                ReadTH10SBVaule(out temperature, out humidity);
                                if (temperature != 0)
                                {
                                    bool isTempDiffNG = false;
                                    foreach (var item in m_ListBmsInfo)
                                    {
                                        if (item.Description == "环境温度" || item.Description == "电芯温度1" || item.Description == "电芯温度2" || item.Description == "电芯温度3"
                                            || item.Description == "电芯温度4" || item.Description == "单体最大温度" || item.Description == "单体最小温度" || item.Description == "功率温度")
                                        {
                                            double val = double.Parse(item.StrValue);
                                            if (Math.Abs(Math.Abs(val) - Math.Abs(temperature)) > Math.Abs(OneClickFactorySetting.m_TemperatureError))
                                            {
                                                isTempDiffNG = true;
                                            }
                                        }
                                    }
                                    ////温度核验
                                    if (isTempDiffNG)
                                    {
                                        RefreshResult("失败", false);
                                        ShowMessage("电芯温度核验失败，温差过大！", false);
                                        isOneClickCheck = false;
                                        BqProtocol.BqInstance.m_bIsOneClick = false;
                                        return;
                                    }
                                    else
                                    {
                                        ShowMessage("电芯温度检验成功！", true);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        bool isTempDiffNG = false;
                        var it = m_ListBmsInfo.FirstOrDefault(p => p.Description == "环境温度");
                        temperature = float.Parse(it.StrValue);
                        foreach (var item in m_ListBmsInfo)
                        {
                            if (item.Description == "电芯温度1" || item.Description == "电芯温度2" || item.Description == "电芯温度3"
                                || item.Description == "电芯温度4" || item.Description == "单体最大温度" || item.Description == "单体最小温度" || item.Description == "功率温度")
                            {
                                double val = double.Parse(item.StrValue);
                                if (Math.Abs(Math.Abs(val) - Math.Abs(temperature)) > Math.Abs(OneClickFactorySetting.m_TemperatureError))
                                {
                                    isTempDiffNG = true;
                                }
                            }
                        }
                        ////温度核验
                        if (isTempDiffNG)
                        {
                            RefreshResult("失败", false);
                            ShowMessage("电芯温度核验失败，温差过大！", false);
                            BqProtocol.BqInstance.m_bIsOneClick = false;
                            isOneClickCheck = false;
                            return;
                        }
                        else
                        {
                            ShowMessage("电芯温度检验成功！", true);
                        }
                    }
                }
                ////循环放电次数检查
                //if (isLoopNumberIsOK)
                //{
                //    ShowMessage("循环放电次数检验成功！", true);
                //}
                //else
                //{
                //    RefreshResult("失败", false);
                //    ShowMessage("循环放电次数检验失败，循环放电次数不为 0 ！", false);
                //    BqProtocol.BqInstance.m_bIsOneClick = false;
                //    isOneClickCheck = false;
                //    return;
                //}

                //var _item = m_ListBmsInfo.FirstOrDefault(p => p.Description == "SOC");
                //if (_item.StrValue == "0" || _item.StrValue == "1")
                //{
                //    ShowMessage("SOC值检验成功！", true);
                //}
                //else
                //{
                //    RefreshResult("失败", false);
                //    ShowMessage("SOC检验失败，SOC值不为 0或1 ！", false);
                //    BqProtocol.BqInstance.m_bIsOneClick = false;
                //    isOneClickCheck = false;
                //    return;
                //}

                //var resistance = m_ListBmsInfo.FirstOrDefault(p => p.Description == "进水阻抗");
                //int resistanceVal = Int32.Parse(resistance.StrValue);
                //if (Math.Abs(resistanceVal * Math.Pow(10, 3)) < Math.Abs(OneClickFactorySetting.m_minWaterResistance * Math.Pow(10, 3))
                //    || Math.Abs(resistanceVal * Math.Pow(10, 3)) > Math.Abs(OneClickFactorySetting.m_maxWaterResistance * Math.Pow(10, 3)))
                //{
                //    RefreshResult("失败", false);
                //    ShowMessage("进水阻抗检验失败，进水阻抗不在范围内 ！", false);
                //    BqProtocol.BqInstance.m_bIsOneClick = false;
                //    isOneClickCheck = false;
                //    return;
                //}
                //else
                //{
                //    ShowMessage("进水阻抗检验成功！", true);
                //}
                //湿度核验
                if (OneClickFactorySetting.m_HumidityCheck == 1)
                {
                    if (XmlHelper.m_strIsUsingTH10SB == "1")
                    {
                        if (humidity != 0)
                        {
                            string humidityStr = m_ListBmsInfo.FirstOrDefault(p => p.Description == "湿度").StrValue;
                            double val = double.Parse(humidityStr);
                            if (Math.Abs(val - humidity) > OneClickFactorySetting.m_HumidityError)
                            {
                                RefreshResult("失败", false);
                                ShowMessage("电芯湿度核验失败，湿差过大！", false);
                                BqProtocol.BqInstance.m_bIsOneClick = false;
                                isOneClickCheck = false;
                                return;
                            }
                            else
                            {
                                ShowMessage("电芯湿度检验成功！", true);
                            }
                        }
                    }
                }
                Thread.Sleep(100);
                firstRtcStr = string.Empty;
                //RTC检验
                RequireReadRTCEvent?.Invoke(this, EventArgs.Empty);

            }
            else
            {
                ShowMessage("系统未连接，请先连接设备再进行操作！", false);
            }
        }

        string firstRtcStr = string.Empty;
        public void ReadRTCOver(string dtStr)
        {
            try
            {
                //CSVFileHelper.WriteLogs("log","RTC-1",dtStr);
                firstRtcStr = dtStr;
                DateTime dt = DateTime.Parse(dtStr);
                long nowTicks = DateTime.Now.Ticks;
                long rtcTicks = dt.Ticks;
                long tsTicks = Math.Abs(Math.Abs(nowTicks) - Math.Abs(rtcTicks));
                TimeSpan synTS = new TimeSpan(0, 0, OneClickFactorySetting.m_SynTimeSpan);

                long synTicks = synTS.Ticks;
                if (tsTicks > synTicks)
                {
                    RefreshResult("失败", false);
                    ShowMessage("RTC检验失败，RTC偏差过大！", false);
                    BqProtocol.BqInstance.m_bIsOneClick = false;
                    isOneClickCheck = false;
                    return;
                }
                else
                {
                    ShowMessage("RTC检验成功！", true);
                    Thread.Sleep(200);
                    if (OneClickFactorySetting.m_EepromCheck == 1)
                    {
                        RequireCheckEepromEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_EepromFilePath.Trim()));
                    }
                    else
                    {
                        if (OneClickFactorySetting.m_McuCheck == 1)
                        {
                            RequireCheckMCUEvent?.Invoke(this, EventArgs.Empty);
                        }
                        else
                        {
                            Thread.Sleep(200);
                            //RefreshResult("完成", true);
                            //SOC校准
                            //if (OneClickFactorySetting.m_SOCAdjust == 1)
                            {
                                RequireAdjustSOCEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_SOCValue.ToString()));
                                //isCheckOrSetting = true;
                            }
                            //else
                            //{
                            //    Thread.Sleep(200);
                            //    RefreshResult("完成", true);

                            //    //////浅休眠
                            //    if (OneClickFactorySetting.m_ShallowSleep == 1)
                            //    {
                            //        btnShallowSleep_Click(null, null);
                            //    }
                            //    else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                            //    {
                            //        btnDeepSleep_Click(null, null);
                            //    }
                            //    BqProtocol.BqInstance.m_bIsOneClick = false;
                            //    isOneClickCheck = false;
                            //    //CSVFileHelper.WriteLogs("log", "一键出厂检验", "一键出厂检验完成");
                            //    ShowMessage("一键出厂检验完成！", true);
                            //}
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        public event EventHandler RequireCheckMCUEvent;
        public void CheckEepromParamOK(bool isOK)
        {
            if(isOK)
            {
                ShowMessage("Eeprom参数检验成功！", true);
                Thread.Sleep(200);
                if (OneClickFactorySetting.m_McuCheck == 1)
                {
                    RequireCheckMCUEvent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    Thread.Sleep(200);
                    //RefreshResult("完成", true);
                    //SOC校准
                    //if (OneClickFactorySetting.m_SOCAdjust == 1)
                    {
                        RequireAdjustSOCEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_SOCValue.ToString()));
                        //isCheckOrSetting = true;
                    }
                    //else
                    //{
                    //    Thread.Sleep(200);
                    //    RefreshResult("完成", true);

                    //    //////浅休眠
                    //    if (OneClickFactorySetting.m_ShallowSleep == 1)
                    //    {
                    //        btnShallowSleep_Click(null, null);
                    //    }
                    //    else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                    //    {
                    //        btnDeepSleep_Click(null, null);
                    //    }
                    //    BqProtocol.BqInstance.m_bIsOneClick = false;
                    //    isOneClickCheck = false;
                    //    //CSVFileHelper.WriteLogs("log", "一键出厂检验", "一键出厂检验完成");
                    //    ShowMessage("一键出厂检验完成！", true);
                    //}
                }
            }
            else
            {
                RefreshResult("失败", false);
                //MessageBox.Show("Eeprom参数检验失败，请检查！", "Eeprom参数检验提示", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowMessage("Eeprom参数检验失败，请检查！", false);
                BqProtocol.BqInstance.m_bIsOneClick = false;
                isOneClickCheck = false;
            }
        }

        //bool isCheckOrSetting = false;
        public void CheckMCUParamOK(bool isOK)
        {
            if (isOK)
            {
                ShowMessage("MCU参数检验成功！", true);
                Thread.Sleep(200);
                //RefreshResult("完成", true);
                //SOC校准
                //if (OneClickFactorySetting.m_SOCAdjust == 1)
                {
                    RequireAdjustSOCEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_SOCValue.ToString()));
                    //isCheckOrSetting = true;
                }
                //else
                //{
                //    Thread.Sleep(200);
                //    RefreshResult("完成", true);

                //    //////浅休眠
                //    if (OneClickFactorySetting.m_ShallowSleep == 1)
                //    {
                //        btnShallowSleep_Click(null, null);
                //    }
                //    else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                //    {
                //        btnDeepSleep_Click(null, null);
                //    }
                //    BqProtocol.BqInstance.m_bIsOneClick = false;
                //    isOneClickCheck = false;
                //    //CSVFileHelper.WriteLogs("log", "一键出厂检验", "一键出厂检验完成");
                //    ShowMessage("一键出厂检验完成！", true);
                //}
            }
            else
            {
                RefreshResult("失败", false);
                ShowMessage("MCU参数检验失败，请检查！", false);
                BqProtocol.BqInstance.m_bIsOneClick = false;
                isOneClickCheck = false;
            }
        }

        public void AlterSOCOver(bool isOK)
        {
            //BqProtocol.BqInstance.m_bIsStopCommunication = false;
            if (isOK)
            {
                ShowMessage("校准SOC成功！", true);
                Thread.Sleep(1000);

                RequireSecondReadRTCEvent?.Invoke(this, EventArgs.Empty);
            }
            else
            {
                RefreshResult("不合格", false);
                ShowMessage("校准SOC失败！", false);
                BqProtocol.BqInstance.m_bIsOneClick = false;
                isOneClickCheck = false;
            }
        }

        public void ReadSecondRTCOver(string dtStr)
        {
            try
            {
                DateTime dt = DateTime.Parse(dtStr);
                if (!string.IsNullOrEmpty(firstRtcStr))
                {
                    DateTime dateTime = DateTime.Parse(firstRtcStr);
                    if (dt == dateTime)
                    {
                        RefreshResult("不合格", false); ;
                        ShowMessage("前后两次读取的RTC值一致，RTC可能损坏！", false);
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        isOneClickCheck = false;
                    }
                    else
                    {
                        RefreshResult("完成", true);
                        ShowMessage("RTC检查正常！", true);
                        //////浅休眠
                        if (OneClickFactorySetting.m_ShallowSleep == 1)
                        {
                            btnShallowSleep_Click(null, null);
                        }
                        else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                        {
                            btnDeepSleep_Click(null, null);
                        }
                        BqProtocol.BqInstance.m_bIsOneClick = false;
                        isOneClickCheck = false;
                        //CSVFileHelper.WriteLogs("log", "一键出厂检验", "一键出厂检验完成");
                        ShowMessage("一键出厂检验完成！", true);
                    }
                }
                else
                {
                    RefreshResult("不合格", false); ;
                    ShowMessage("首次读取RTC失败！", false);
                    BqProtocol.BqInstance.m_bIsOneClick = false;
                    isOneClickCheck = false;
                }
            }
            catch(Exception ex)
            {
                CSVFileHelper.WriteLogs("log", "一次RTC", firstRtcStr);
                CSVFileHelper.WriteLogs("log", "二次RTC", dtStr);
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        #endregion

        void RefreshResult(string content, bool flag)
        {
            Dispatcher.Invoke(new Action(()=>
            {
                labResult.Content = content;
                if (flag)
                {
                    labResult.Background = new SolidColorBrush(Colors.LightGreen);
                }
                else
                {
                    labResult.Background = new SolidColorBrush(Colors.LightGray);
                }
            }));
        }
        #region  使用电池负载KP182
        private bool ReadAndWriteData(SerialPort serial, byte[] cmdBytes ,byte[] recvBytes)
        {
            if (serial != null )
            {
                if(!serial.IsOpen)
                {
                    //MessageBox.Show(string.Format("串口{0}打开失败，请检查！", serial.PortName), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    ShowMessage(string.Format("串口{0}打开失败，请检查！", serial.PortName), false);
                    return false;
                }
            }
            else
            {
                //MessageBox.Show(string.Format("串口未打开，请检查！",serial.PortName), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                ShowMessage(string.Format("串口未打开，请检查！", serial.PortName), false);
                return false;
            }
            try
            {
                serial.DiscardInBuffer();

                serial.Write(cmdBytes, 0, cmdBytes.Length);

                Thread.Sleep(300);
                for (int nRead = 0; nRead < 3; nRead++)
                {
                    serial.DiscardOutBuffer();
                    int nRdCount = serial.Read(recvBytes, 0, recvBytes.Length);

                    if (nRdCount == recvBytes.Length)
                    {
                        return true;
                    }
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex) 
            {
                serial.Close();
                if (isOneClickFactoryConfig) 
                    isOneClickFactoryConfig = false;
                RefreshResult("失败", false);
                ShowMessage("电池负载操作超时！", false);
                BqProtocol.BqInstance.m_bIsOneClick = false;
                MessageBox.Show(ex.Message);
                //CSVFileHelper.WriteLogs("log", "充放电2", ex.Message);
            }
            return false;
        }

        //private void btnSetKP182_Click(object sender, RoutedEventArgs e)
        //{
        //    SetKP182CurrentMode();
        //}

        byte deviceAddress = 0x01;
        int currentValue = 0;
        int waitTime = 0;
        int currentError = 0;

        private void SetKP182CurrentMode(SerialPort serial, int currentVal)
        {
            try
            {
                byte[] cmdBytes = new byte[] { deviceAddress, 0x06, 0x01, 0x10, 0x00, 0x01, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 };
                byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
                cmdBytes[cmdBytes.Length - 1] = crc16[1];
                cmdBytes[cmdBytes.Length - 2] = crc16[0];
                byte[] recvBytes = new byte[cmdBytes.Length - 4];
                if (ReadAndWriteData(serial, cmdBytes, recvBytes))
                {
                    if (recvBytes[1] == cmdBytes[1] && recvBytes[2] == cmdBytes[2] && recvBytes[3] == cmdBytes[3])
                    {
                        //CSVFileHelper.WriteLogs("log", "充放电", string.Format("电池负载 {0} 设置CC模式！", serial.PortName));
                        //MessageBox.Show("设置拉载模式为CC模式成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        SetKP182CurrentValue(serial, currentVal);
                    }
                    else
                    {
                        //MessageBox.Show("设置拉载模式为CC模式失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowMessage("设置拉载模式为CC模式失败！", false);
                    }
                }
            }
            catch(Exception ex)
            {
                serial.Close();
                MessageBox.Show(ex.Message);
                //CSVFileHelper.WriteLogs("log", "充放电3", ex.Message);
            }
        }

        private void SetKP182CurrentValue(SerialPort serial, int currentVal)
        {
            try
            {
                byte[] valBytes = BitConverter.GetBytes(currentVal);
                byte[] cmdBytes = new byte[] { deviceAddress, 0x06, 0x01, 0x16, 0x00, 0x01, 0x04 };
                cmdBytes = cmdBytes.Concat(valBytes.Reverse()).ToArray();
                cmdBytes = cmdBytes.Concat(new byte[] { 0x00, 0x00 }).ToArray();
                byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
                cmdBytes[cmdBytes.Length - 1] = crc16[1];
                cmdBytes[cmdBytes.Length - 2] = crc16[0];
                byte[] recvBytes = new byte[cmdBytes.Length - 4];
                //CSVFileHelper.WriteLogs("log", "充放电", "向184发送设置充放电电压值指令");
                if (ReadAndWriteData(serial, cmdBytes, recvBytes))
                {
                    if (recvBytes[1] == cmdBytes[1] && recvBytes[2] == cmdBytes[2] && recvBytes[3] == cmdBytes[3])
                    {
                        //MessageBox.Show("设置拉载电流10000mA成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        //CSVFileHelper.WriteLogs("log", "充放电", string.Format("电池负载 {0} 设置值 {1}！", serial.PortName,currentVal));
                        SetKP182CurrentSwitchON(serial,waitTime);
                    }
                    else
                    {
                        //MessageBox.Show(string.Format("设置拉载电流 {0} mA失败！", currentVal), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowMessage(string.Format("设置拉载电流 {0} mA失败！", currentVal), false);
                    }
                }
            }
            catch (Exception ex)
            {
                serial.Close();
                MessageBox.Show(ex.Message);
                //CSVFileHelper.WriteLogs("log", "充放电4", ex.Message);
            }
        }
        bool flag = false;
        bool isCharge = false;
        private void SetKP182CurrentSwitchON(SerialPort serial,int waittime)
        {
            try
            {
                byte[] cmdBytes = new byte[] { deviceAddress, 0x06, 0x01, 0x0E, 0x00, 0x01, 0x04, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00 };
                byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
                cmdBytes[cmdBytes.Length - 1] = crc16[1];
                cmdBytes[cmdBytes.Length - 2] = crc16[0];
                byte[] recvBytes = new byte[cmdBytes.Length - 4];
                //CSVFileHelper.WriteLogs("log", "充放电5-1", BitConverter.ToString(cmdBytes));
                if (ReadAndWriteData(serial, cmdBytes, recvBytes))
                {
                    //CSVFileHelper.WriteLogs("log", "充放电5-2",BitConverter.ToString(recvBytes));
                    if (recvBytes[1] == cmdBytes[1] && recvBytes[2] == cmdBytes[2] && recvBytes[3] == cmdBytes[3])
                    {
                        //CSVFileHelper.WriteLogs("log", "充放电", string.Format("电池负载 {0} 开关开启！", serial.PortName));
                        if (isChargeOrDischargeTest)
                        {
                            //CSVFileHelper.WriteLogs("log", "充放电", string.Format("电池负载 {0} 进入充放电等待，时间 {1}！", serial.PortName,waittime));
                            isChargeOrDischargeTest = false;
                            LoadingWnd wnd = new LoadingWnd(waittime);
                            wnd.ShowDialog();
                            //CSVFileHelper.WriteLogs("log", "充放电", "电池负载充放电等待结束}！");
                            var item = m_ListCellVoltage.FirstOrDefault(p => p.Description == "实时电流");
                            if (item != null)
                            {
                                int value = 0;
                                //CSVFileHelper.WriteLogs("log", "充放电5-3", item.StrValue.ToString());
                                if (int.TryParse(item.StrValue, out value))
                                {
                                    //CSVFileHelper.WriteLogs("log", "充放电5-4", value.ToString());
                                    if (Math.Abs(Math.Abs(value) - Math.Abs(currentValue)) < Math.Abs(currentError))
                                    {
                                        if(isCharge)
                                        {
                                            ShowMessage("充放电测试完成！", true);
                                            RefreshResult("合格", true);
                                            SetKP182CurrentSwitchOFF(serial);
                                            //BqProtocol.BqInstance.m_bIsStopCommunication = true;
                                            //Thread.Sleep(600);
                                            //RequireAdjustSOCEvent?.Invoke(this, new EventArgs<string>(OneClickFactorySetting.m_SOCValue.ToString()));
                                        }
                                        else
                                        {
                                            ShowMessage("充放电测试完成！", true);
                                            RefreshResult("合格", true);
                                            SetKP182CurrentSwitchOFF(serial);
                                        }
                                    }
                                    else
                                    {
                                        RefreshResult("不合格", false);
                                        ShowMessage("充放电测试完成！", true);
                                        SetKP182CurrentSwitchOFF(serial);
                                        return;
                                    }

                                    //CSVFileHelper.WriteLogs("log", "充放电", string.Format("电池负载 {0} 进入充放电判断完成！", serial.PortName));


                                    if (flag == false)
                                    {
                                        flag = true;
                                        isCharge = true;
                                        Thread.Sleep(300);
                                        if (SelectCANWnd.m_IsUsingKP182_2)
                                        {
                                            if (MainWindow.kp184SerialPort_2 != null && MainWindow.kp184SerialPort_2.IsOpen)
                                            {
                                                //CSVFileHelper.WriteLogs("log", "充放电", string.Format("打开电池负载 {0}！", MainWindow.kp184SerialPort_2.PortName));
                                                isChargeOrDischargeTest = true;
                                                ShowMessage("充放电测试开始！", false);
                                                RefreshResult("测试开始", false);
                                                deviceAddress = Convert.ToByte(XmlHelper.m_strKP182DeviceAddress_2, 16);
                                                currentValue = SelectCANWnd.m_CurrentValue_2;
                                                waitTime = SelectCANWnd.m_WaitTime_2;
                                                currentError = SelectCANWnd.m_CurrentError_2;
                                                //CSVFileHelper.WriteLogs("log", "充放电", string.Format("放电电压 {0}，等待时间 {1}，误差{2}！", currentValue.ToString(), waitTime.ToString(), currentError.ToString()));
                                                SetKP182CurrentMode(MainWindow.kp184SerialPort_2, currentValue);
                                            }
                                            else
                                            {
                                                //MessageBox.Show("电池负载KP184未打开，请启用该设备！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                                                ShowMessage("电池负载KP184-02未打开，请启用该设备！", false);
                                            }
                                        }
                                    }


                                    //////浅休眠
                                    //if (OneClickFactorySetting.m_ShallowSleep == 1)
                                    //{
                                    //    btnShallowSleep_Click(null, null);
                                    //}
                                    //else if (OneClickFactorySetting.m_DeepSleep == 1)//深休眠
                                    //{
                                    //    btnDeepSleep_Click(null, null);
                                    //}
                                }
                            }
                        }
                        else
                        {
                            Thread.Sleep(3000);
                            ReadCurrentMeasureValue(serial);
                        }
                    }
                    else
                    {
                        //MessageBox.Show("设置拉载开关开启失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowMessage("设置拉载开关开启失败！", false);
                    }
                }
            }
            catch (Exception ex)
            {
                serial.Close();
                MessageBox.Show(ex.Message);
                //CSVFileHelper.WriteLogs("log", "充放电5", ex.Message);
            }
        }


        private void SetKP182CurrentSwitchOFF(SerialPort serial)
        {
            try
            {
                byte[] cmdBytes = new byte[] { deviceAddress, 0x06, 0x01, 0x0E, 0x00, 0x01, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
                cmdBytes[cmdBytes.Length - 1] = crc16[1];
                cmdBytes[cmdBytes.Length - 2] = crc16[0];
                byte[] recvBytes = new byte[cmdBytes.Length - 4];
                if (ReadAndWriteData(serial, cmdBytes, recvBytes))
                {
                    if (recvBytes[1] == cmdBytes[1] && recvBytes[2] == cmdBytes[2] && recvBytes[3] == cmdBytes[3])
                    {
                        //CSVFileHelper.WriteLogs("log", "充放电", string.Format("电池负载 {0} 开关关闭！", serial.PortName));
                    }
                    else
                    {
                        //MessageBox.Show("设置拉载开关关闭失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowMessage("设置拉载开关关闭失败！", false);
                    }
                }
            }
            catch (Exception ex)
            {
                serial.Close();
                MessageBox.Show(ex.Message);
                //CSVFileHelper.WriteLogs("log", "充放电6", ex.Message);
            }
        }

        private void ReadCurrentMeasureValue(SerialPort serial)
        {
            try
            {
                byte[] cmdBytes = new byte[] { deviceAddress, 0x03, 0x03, 0x00, 0x00, 0x00, 0x00, 0x00 };
                byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
                cmdBytes[cmdBytes.Length - 1] = crc16[1];
                cmdBytes[cmdBytes.Length - 2] = crc16[0];
                byte[] recvBytes = new byte[23];

                if (ReadAndWriteData(serial, cmdBytes, recvBytes))
                {
                    //CSVFileHelper.WriteLogs("log", "充放电", string.Format("电池负载 {0} 读取数值！", serial.PortName));
                    int currentVal = ((recvBytes[8] << 16) | (recvBytes[9] << 8) | (recvBytes[10]));
                    if (Math.Abs(currentVal - 10000) <= 20)//KP182电压在范围内，请求-10A校准
                    {
                        if (isOneClickFactoryConfig)
                        {
                            RequireAdjust10ACurrentEvent?.Invoke(this, EventArgs.Empty);
                            isOneClickFactoryConfig = false;
                        }
                        else
                        {
                            isAdjust10ACurrent = false;
                            BqProtocol.BqInstance.AdjustRtCurrent((int)((-10) * Math.Pow(10, 3)));
                            isAdjust10ACurrent = true;
                        }
                        //MessageBox.Show(string.Format("测量电压值 {0} A！ 可以开启放电10A校准！", (currentVal / 1000)), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        //SetKP182CurrentSwitchOFF();
                    }
                    else
                    {
                        //MessageBox.Show(string.Format("测量电流值 {0} A！ 不能开启放电10A校准！", (currentVal / 1000)), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        ShowMessage(string.Format("测量电流值 {0} A！ 不能开启放电10A校准！", (currentVal / 1000)), false);
                    }
                }
            }
            catch (Exception ex)
            {
                serial.Close();
                MessageBox.Show(ex.Message);
                //CSVFileHelper.WriteLogs("log","充放电7",ex.Message);
            }
        }
        #endregion

        #region 使用ReHe电压表、电流表
        byte voltmeterAddress = 0x01;
        private void ReadVoltmeterValue(double voltageBase,int error)
        {
            byte[] cmdBytes = new byte[] {voltmeterAddress,0x03, 0x00, 0x23, 0x00, 0x03, 0x00, 0x00};
            byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
            cmdBytes[cmdBytes.Length - 1] = crc16[0];
            cmdBytes[cmdBytes.Length - 2] = crc16[1];//校验位低位在前，高位在后
            byte[] recvBytes = new byte[11];
            if(ReadAndWriteData(MainWindow.voltmeterSerialPort, cmdBytes, recvBytes))
            {
                if (recvBytes[0] == cmdBytes[0] && recvBytes[1] == cmdBytes[1] && recvBytes[2] == 0x06)
                {
                    int point = recvBytes[3];//获取电压值小数点位置
                    int val = (recvBytes[7] << 8) | recvBytes[8];
                    double voltage = (val * Math.Pow(10, 3)) / Math.Pow(10, (4 - point));
                    //if (Math.Abs(voltage - voltageBase * Math.Pow(10, 3)) > error)
                    //{
                    //    resistanceResult.Fill = new SolidColorBrush(Colors.Red);
                    //}
                    //else
                    //{
                    //    resistanceResult.Fill = new SolidColorBrush(Colors.LightGreen);
                    //}
                    AutoClosedMsgBox.Show("内阻测试完成！", "提示", 1000, 64);
                }
                else
                {
                    MessageBox.Show("读取电压表电压数据失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void ReadAmperemeterValue(double currentBase, int error)
        {
            byte[] cmdBytes = new byte[] { voltmeterAddress, 0x03, 0x00, 0x23, 0x00, 0x09, 0x00, 0x00 };
            byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
            cmdBytes[cmdBytes.Length - 1] = crc16[0];
            cmdBytes[cmdBytes.Length - 2] = crc16[1];//校验位低位在前，高位在后
            byte[] recvBytes = new byte[23];
            if(ReadAndWriteData(MainWindow.amperemeterSerialPort, cmdBytes, recvBytes))
            {
                if (recvBytes[0] == cmdBytes[0] && recvBytes[1] == cmdBytes[1] && recvBytes[2] == 0x12)
                {
                    int point = recvBytes[4];//获取电流值小数点位置
                    int val = (recvBytes[19] << 8) | recvBytes[20];
                    double current = (val * Math.Pow(10, 3)) / Math.Pow(10, (4 - point));
                    //if (Math.Abs(current - currentBase * Math.Pow(10, 3)) > error)
                    //{
                    //    powerResult.Fill = new SolidColorBrush(Colors.Red);
                    //}
                    //else
                    //{
                    //    powerResult.Fill = new SolidColorBrush(Colors.LightGreen);
                    //}
                    AutoClosedMsgBox.Show("功耗测试完成！", "提示", 1000, 64);
                }
                else
                {
                    MessageBox.Show("读取电流表电流数据失败！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }



        //private void btnPowerTest_Click(object sender, RoutedEventArgs e)
        //{
        //    double currentBase;
        //    if (double.TryParse(XmlHelper.m_strPowerCurrentBase, out currentBase))
        //    {
        //        int error;
        //        if (int.TryParse(XmlHelper.m_strPowerCurrentError, out error))
        //        {
        //            powerResult.Fill = new SolidColorBrush(Colors.LightGray);
        //            ReadAmperemeterValue(currentBase, error);
        //        }
        //        else
        //        {
        //            MessageBox.Show("功耗测试电流误差解析错误！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("功耗测试电流基数解析错误！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}

        //private void btnResistanceTest_Click(object sender, RoutedEventArgs e)
        //{
        //    double voltageBase;
        //    if(double.TryParse(XmlHelper.m_strResistanceVoltageBase,out voltageBase))
        //    {
        //        int error;
        //        if(int.TryParse(XmlHelper.m_strResistanceVoltageError,out error))
        //        {
        //            resistanceResult.Fill = new SolidColorBrush(Colors.LightGray);
        //            ReadVoltmeterValue(voltageBase, error);
        //        }
        //        else
        //        {
        //            MessageBox.Show("内阻测试电压误差解析错误！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        //        }
        //    }
        //    else
        //    {
        //        MessageBox.Show("内阻测试电压基数解析错误！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        //    }
        //}

        #endregion

        #region  使用TH10S-B温湿度仪
        private void ReadTH10SBVaule(out double temperature,out double humidity)
        {
            byte[] cmdBytes = new byte[] { deviceAddress, 0x03, 0x00, 0x00, 0x00, 0x02, 0x00, 0x00 };
            byte[] crc16 = CRC_Check.CRC16(cmdBytes, 0, cmdBytes.Length - 2);
            cmdBytes[cmdBytes.Length - 1] = crc16[0];
            cmdBytes[cmdBytes.Length - 2] = crc16[1];
            byte[] recvBytes = new byte[9];
            if (ReadAndWriteData(MainWindow.th10sbSerialPort, cmdBytes, recvBytes))
            {
                int tempVal = ((recvBytes[3] << 8) |(recvBytes[4]));
                temperature = tempVal / Math.Pow(10,1);
                
                int humVal = ((recvBytes[5] << 8) | (recvBytes[6]));
                humidity = humVal / Math.Pow(10, 1);
            }
            else
            {
                temperature = 0;
                humidity = 0;
            }
        }
        #endregion

        public void PowerOnOrPowerOffOver(bool isOff)
        {
            if (isOff)
            {
                ShowMessage("下电成功！", true);
                Thread.Sleep(300);
            }
            else
            {
                ShowMessage("上电成功！", true);
                Thread.Sleep(300);
                if (SelectCANWnd.m_IsUsingKP182)
                {
                    if(MainWindow.serialPort != null && MainWindow.serialPort.IsOpen)
                    {
                        deviceAddress = Convert.ToByte(XmlHelper.m_strKP182DeviceAddress, 16);
                        currentValue = SelectCANWnd.m_CurrentValue;
                        waitTime = SelectCANWnd.m_WaitTime;
                        currentError = SelectCANWnd.m_CurrentError;
                        SetKP182CurrentMode(MainWindow.serialPort, currentValue);
                    }
                }
                else
                {
                    ShowMessage("电池负载KP184-01未打开，请启用该设备！", false);
                }
            }
        }
        bool isChargeOrDischargeTest = false;
        private void btnChargeOrDischarge_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                isChargeOrDischargeTest = true;
                flag = false;//使用两个电池负载的时候，用于判断
                isCharge = false;//开始先放电10A，其次充电16A，所以第二次才是充电，标志位置1
                ShowMessage("充放电测试开始！", false);
                RefreshResult("测试开始", false);
                deviceAddress = 0x01;
                currentValue = 0;
                waitTime = 0;
                currentError = 0;
                btnPowerON_Click(null, null);
            }
            else
            {
                ShowMessage("系统未连接，请连接后再进行操作！", false);
            }
        }

        private void AutoChargeOrDischarge()
        {
            Thread.Sleep(100);
            btnChargeOrDischarge_Click(null, null);
        }

        void InitInterfaceShow()
        {
            rtbMsg.Document.Blocks.Clear();
            RefreshResult("待测试", false);
        }

        void ShowMessage(string content, bool flag)
        {
            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                string msgStr = string.Format("{0}    {1}", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"), content);
                Paragraph paragraph = new Paragraph(new Run(msgStr));

                if (flag)
                {
                    paragraph.FontSize = 12;
                    paragraph.Foreground = new SolidColorBrush(Colors.Black);
                }
                else
                {
                    paragraph.FontSize = 14;
                    paragraph.Foreground = new SolidColorBrush(Colors.Red);
                }

                rtbMsg.Document.Blocks.Add(paragraph);
                rtbMsg.ScrollToEnd();
            }));
        }

        public void ShowCheckMCuMsg(bool isOK,string content)
        {
            ShowMessage(content, isOK);
        }
    }



    public static class DataGridExtension
    {
        /// <summary>
        /// 获取DataGrid控件单元格
        /// </summary>
        /// <param name="dataGrid">DataGrid控件</param>
        /// <param name="rowIndex">单元格所在的行号</param>
        /// <param name="columnIndex">单元格所在的列号</param>
        /// <returns>指定的单元格</returns>
        public static DataGridCell GetCell(this DataGrid dataGrid, int rowIndex, int columnIndex)
        {
            DataGridRow rowContainer = GetRow(dataGrid, rowIndex);
            if (rowContainer != null)
            {
                DataGridCellsPresenter presenter = GetVisualChild<DataGridCellsPresenter>(rowContainer);
                DataGridCell cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                if (cell == null)
                {
                    dataGrid.ScrollIntoView(rowContainer, dataGrid.Columns[columnIndex]);
                    cell = (DataGridCell)presenter.ItemContainerGenerator.ContainerFromIndex(columnIndex);
                }
                return cell;
            }
            return null;
        }

        /// <summary>
        /// 获取DataGrid的行
        /// </summary>
        /// <param name="dataGrid">DataGrid控件</param>
        /// <param name="rowIndex">DataGrid行号</param>
        /// <returns>指定的行号</returns>
        public static DataGridRow GetRow(this DataGrid dataGrid, int rowIndex)
        {
            DataGridRow rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            if (rowContainer == null)
            {
                dataGrid.UpdateLayout();
                dataGrid.ScrollIntoView(dataGrid.Items[rowIndex]);
                rowContainer = (DataGridRow)dataGrid.ItemContainerGenerator.ContainerFromIndex(rowIndex);
            }
            return rowContainer;
        }

        /// <summary>
        /// 获取父可视对象中第一个指定类型的子可视对象
        /// </summary>
        /// <typeparam name="T">可视对象类型</typeparam>
        /// <param name="parent">父可视对象</param>
        /// <returns>第一个指定类型的子可视对象</returns>
        public static T GetVisualChild<T>(Visual parent) where T : Visual
        {
            T child = default(T);
            int numVisuals = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < numVisuals; i++)
            {
                Visual v = (Visual)VisualTreeHelper.GetChild(parent, i);
                child = v as T;
                if (child == null)
                {
                    child = GetVisualChild<T>(v);
                }
                if (child != null)
                {
                    break;
                }
            }
            return child;
        }
    }
}
