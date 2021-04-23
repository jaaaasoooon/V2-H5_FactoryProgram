using BoqiangH5.BQProtocol;
using BoqiangH5Entity;
using BoqiangH5Repository;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;

namespace BoqiangH5
{
    /// <summary>
    /// UserCtrlMCU.xaml 的交互逻辑
    /// </summary>
    public partial class UserCtrlMCU : UserControl
    {
        List<H5BmsInfo> ListSysInfo1 = new List<H5BmsInfo>();
        List<H5BmsInfo> ListSysInfo2 = new List<H5BmsInfo>();
        List<H5BmsInfo> ListChargeInfo = new List<H5BmsInfo>();


        public UserCtrlMCU()
        {
            InitializeComponent();

            InitMcuWnd();
        }

        private void InitMcuWnd()
        {
            string strConfigFile = XmlHelper.m_strCfgFilePath; 

            XmlHelper.LoadXmlConfig(strConfigFile, "mcu_info/system1/mcu_node_info", ListSysInfo1);
            XmlHelper.LoadXmlConfig(strConfigFile, "mcu_info/system2/mcu_node_info", ListSysInfo2);
            XmlHelper.LoadXmlConfig(strConfigFile, "mcu_info/charge_discharge/mcu_node_info", ListChargeInfo);

            dgMcuSys1.ItemsSource = ListSysInfo1;
            dgMcuSys2.ItemsSource = ListSysInfo2;
            dgMcuCharge.ItemsSource = ListChargeInfo;

            UpdateMcuWnd();
        }

        //public bool isWrite = false;
        public void UpdateMcuWnd()
        {
            if(MainWindow.m_statusBarInfo.IsOnline)
            {
                btnReadMcuData.IsEnabled = true;      
                btnSaveMcuData.IsEnabled = true;
                btnLoadMcuPara.IsEnabled = true;
                btnWriteMcuData.IsEnabled = true;
                //if (isWrite)
                //{
                //    if (!string.IsNullOrEmpty(FilePath))
                //    {
                //        LoadPara(FilePath);
                //        //System.Threading.Thread.Sleep(100);
                //        btnWriteMcuData_Click(null, null);//加载完直接下发参数
                //    }
                //}
            }
            else
            {
                btnReadMcuData.IsEnabled = false;
                btnWriteMcuData.IsEnabled = false;
                btnLoadMcuPara.IsEnabled = false;
                btnSaveMcuData.IsEnabled = false;
            }
        }
        //string FilePath = string.Empty;
        bool isCheckMcu = false;
        public void RequireCheckMCU()
        {
            //LoadPara(filePath);
            //FilePath = filePath;
            isCheckMcu = true;
            Thread.Sleep(100);
            btnReadMcuData_Click(null,null);
        }
        public void RequireWriteMcu(string filePath,bool _isRequireWriteMcu)
        {
            isRequireWriteMcu = _isRequireWriteMcu;
            LoadPara(filePath);
            btnWriteMcuData_Click(null, null);//加载完直接下发参数
        }

        bool isUpdateMCUData = false;
        public void UpdateMcuConfigurationData()
        {
            if(MainWindow.m_statusBarInfo.IsOnline)
            {
                isUpdateMCUData = true;
                btnReadMcuData_Click(null, null);
            }
            else
            {
                MessageBoxForm.Show("系统未连接，请连接后再进行操作！", "提示", 1000);
            }
        }

        public void RequireReadMcuMsg()
        {
            isRequireReadMcu = true;
            btnReadMcuData_Click(null, null);
        }
        bool isRequireReadMcu = false;
        bool isReadMCU = false;
        bool isWriteMCU = false;
        bool isRequireWriteMcu = false;
        public event EventHandler<EventArgs<bool>> WriteMcuOverEvent;
        private void btnReadMcuData_Click(object sender, RoutedEventArgs e)
        {
            BqProtocol.bReadBqBmsResp = true;
            isReadMCU = false;
            BqProtocol.BqInstance.m_bIsStopCommunication = true;
            BqProtocol.BqInstance.ReadMcuData();
            isReadMCU = true;
        }

        DateTime defaultDate = new DateTime(2019, 10, 21);
        private void btnWriteMcuData_Click(object sender, RoutedEventArgs e)
        {
            isWriteMCU = false;
            byte[] mcuData = new byte[256];
            int len = 0;
            H5BmsInfo info = ListSysInfo2.SingleOrDefault(p => p.Description == "生产日期");
            if (info != null)
            {
                DateTime date;
                if (DateTime.TryParse(info.StrValue, out date))
                {
                    if (date == defaultDate)
                    {
                        info.StrValue = DateTime.Now.ToString("yyyy-MM-dd");
                    }
                }
                else
                {
                    MessageBox.Show("生产日期格式不正确，请检查！", "写入 MCU 提示", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
            }
            if (!GetMcuDataBuf(mcuData, ref len))
            {
                if (isUpdateMCUData)
                {
                    isUpdateMCUData = false;
                    UpdateMcuConfigOKEvent?.Invoke(this, new EventArgs<bool>(false));
                }
                else
                {
                    if (isRequireWriteMcu)
                    {
                        WriteMcuOverEvent?.Invoke(this, new EventArgs<bool>(false));
                        isRequireWriteMcu = false;
                    }
                    else
                        MessageBox.Show("写入 MCU 参数失败，请检查输入数据！", "写入 MCU 提示", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                return;
            }

            BqProtocol.BqInstance.m_bIsStopCommunication = true;
            BqProtocol.BqInstance.SendMultiFrame(mcuData, len, 0xB2);
            isWriteMCU = true;
        }

        //public event EventHandler<EventArgs<string>> GetFileEvent;

        private void LoadPara(string filePath)
        {
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                Encoding encoding = System.Text.Encoding.UTF8;

                fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                sr = new StreamReader(fs, encoding);

                //记录每次读取的一行记录
                string strLine = "";

                int nIndex = 0;
                //逐行读取数据
                while ((strLine = sr.ReadLine()) != null)
                {
                    if (string.IsNullOrEmpty(strLine))
                    {
                        continue;
                    }

                    string[] arrVal = strLine.Split(':');

                    if (nIndex < ListSysInfo1.Count)
                    {
                        LoadMcuInfo(arrVal, ListSysInfo1);
                    }
                    else if (nIndex < (ListSysInfo1.Count + ListSysInfo2.Count))
                    {
                        LoadMcuInfo(arrVal, ListSysInfo2);
                    }
                    else
                    {
                        LoadMcuInfo(arrVal, ListChargeInfo);
                        //continue;
                    }

                    nIndex++;
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show("MCU 参数加载失败! ", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        //string FilePath = string.Empty;
        private void btnLoadMcuPara_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "程序文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            ofd.FileName = System.Windows.Forms.Application.StartupPath + "\\ProtocolFiles\\H5_MCU_config.txt";

            bool? result = ofd.ShowDialog();
            if (result != true)
                return;
            LoadPara(ofd.FileName);
            //string filename = string.Format("H5_MCU_{0}_config.txt", DateTime.Now.ToString("yyyyMMddHHmmss"));
            //string filepath = System.Windows.Forms.Application.StartupPath + "\\ProtocolFiles\\" + filename;
            //SavePara(filepath);
            //FilePath = ofd.FileName;
            //GetFileEvent?.Invoke(this, new EventArgs<string>(Path.GetFileName(FilePath)));

            //System.Threading.Thread.Sleep(100);
            //btnWriteMcuData_Click(null, null);//加载完直接下发参数
        }

        private bool LoadMcuInfo(string[] arrVal, List<H5BmsInfo> ListMcuInfo)
        {
            bool bRet = false;
            for (int n = 0; n < ListMcuInfo.Count; n++)
            {
                if (arrVal[0] == ListMcuInfo[n].Description.Trim())
                {
                    ListMcuInfo[n].StrValue = arrVal[1].Trim();
                    bRet = true;
                    break;
                }
            }
            return bRet;
        }

        private void SavePara(string filePath)
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate);
                StreamWriter sw = new StreamWriter(fs);

                sw.WriteLine(SaveParaGetUIData(ListSysInfo1));
                sw.WriteLine();

                sw.WriteLine(SaveParaGetUIData(ListSysInfo2));
                sw.WriteLine();

                sw.WriteLine(SaveParaGetUIData(ListChargeInfo));
                sw.WriteLine();

                sw.Close();
                fs.Close();
                AutoClosedMsgBox.Show("MCU 参数保存成功! ", "提示", 500, 64);

            }
            catch(Exception ex)
            {
                MessageBox.Show("MCU 参数保存失败! ", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private void btnSaveMcuData_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Title = "MCU 参数";
                sfd.Filter = "MCU 参数文件(*.txt)|*.txt|所有文件(*.*)|*.*";
                sfd.FileName = string.Format("MCU参数_{0:yyyyMMdd_HHmm}", DateTime.Now);

                bool? result = sfd.ShowDialog();

                if (result == true)
                {
                    SavePara(sfd.FileName);
                }
            }
            catch(Exception ex)
            {

            }

        }

        public void HandleMcuWndUpdateEvent(object sender, EventArgs e)
        {
            if (SelectCANWnd.m_H5Protocol == H5Protocol.DI_DI)
            {
                return;
            }

            UpdateMcuWnd();
        }

        public void HandleRecvMcuDataEvent(object sender, CustomRecvDataEventArgs e)
        {   if(isReadMCU)
            {
                BqUpdateMcuInfo(e.RecvMsg);
                BqProtocol.BqInstance.m_bIsStopCommunication = false;
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
                    if(isUpdateMCUData)
                    {
                        isUpdateMCUData = false;
                        UpdateMcuConfigOKEvent?.Invoke(this, new EventArgs<bool>(true));
                    }
                    else
                    {
                        if (isRequireWriteMcu)
                        {
                            WriteMcuOverEvent?.Invoke(this, new EventArgs<bool>(true));
                            isRequireWriteMcu = false;
                        }
                        else
                            //MessageBoxForm.Show("写入 MCU 参数成功！", "写入MCU提示", 1000);
                            AutoClosedMsgBox.Show("写入MCU 参数成功! ", "提示", 500, 64);
                    }
                }
                else
                {
                    if(isUpdateMCUData)
                    {
                        isUpdateMCUData = false;
                        UpdateMcuConfigOKEvent?.Invoke(this, new EventArgs<bool>(false));
                    }
                    else
                    {
                        if (isRequireWriteMcu)
                        {
                            WriteMcuOverEvent?.Invoke(this, new EventArgs<bool>(false));
                            isRequireWriteMcu = false;
                        }
                        else
                            AutoClosedMsgBox.Show("写入 MCU 参数失败！", "写入MCU提示", 1000, 64);
                    }
                }
                BqProtocol.BqInstance.m_bIsStopCommunication = false;
                isWriteMCU = false;
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
            catch(Exception ex)
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

        private byte GetConfByteValue(byte byteVal, CheckBox cBox, int bitIndex)
        {
            if (cbChgEnd.IsChecked == true)
            {
                byteVal = (byte)(byteVal | (1 << bitIndex));
            }
            return byteVal;
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

                    if (nodeInfo.StrValue.Contains('.'))
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
                    DateTime.TryParse(nodeInfo.StrValue,out dtVal);
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

        private void MCUWndCheckBox_Click(object sender, RoutedEventArgs e)
        {
            byte chgEnd = GetMcuConfByte(cbChgEnd, 0x01);

            byte dsgEnd = GetMcuConfByte(cbDsgEnd, 0x02);

            byte enEeprom = GetMcuConfByte(cbEnEeprom, 0x08);

            byte isCclb = GetMcuConfByte(cbIsCclb, 0x01);

            byte isPreCharge = GetMcuConfByte(cbIsPreCharge, 0x02);

            ListSysInfo1[0].StrValue = (isCclb | isPreCharge).ToString("X2") + (chgEnd | dsgEnd | enEeprom).ToString("X2");           

        }

        private byte GetMcuConfByte(CheckBox cb, byte byteCheckVal)
        {
            if (cb.IsChecked == true)
            {
                return byteCheckVal;
            }
            else
            {
                return 0x00;
            }            
        }

        private string SaveParaGetUIData(List<H5BmsInfo> ListInfo)
        {
            string strMsg = null;

            for (int n = 0; n < ListInfo.Count; n++)
            {
                strMsg += ListInfo[n].Description + ": " + ListInfo[n].StrValue + "\r\n";
            }

            return strMsg;
        }


        public static bool CheckDateFormat(string StrSource)
        { 
            return Regex.IsMatch(StrSource, @"^(?<year>\\d{2,4})-(?<month>\\d{1,2})-(?<day>\\d{1,2})$");
        }
    }
}
