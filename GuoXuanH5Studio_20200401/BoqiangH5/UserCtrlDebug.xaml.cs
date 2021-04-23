using BoqiangH5.BQProtocol;
using BoqiangH5.DDProtocol;
using BoqiangH5Entity;
using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using BoqiangH5Repository;
using System.Linq;
using System.Threading;

namespace BoqiangH5
{

    public partial class UserCtrlDebug : UserControl
    {
        public static bool m_bIsNotUpdateBmsInfo = false;
        public static List<H5BmsInfo> m_ListBmsInfo = new List<H5BmsInfo>();
        public static List<H5BmsInfo> m_ListCellVoltage = new List<H5BmsInfo>();
        static byte[] btSysStatus = new byte[2];
        static byte[] btPackStatus = new byte[2];
        static byte[] btBalanceStatus = new byte[4];
        public UserCtrlDebug()
        {
            InitializeComponent();

            InitDebugWnd();
        }

        private void InitDebugWnd()
        {

            userCtrlDebug.IsEnabled = false;

            if (SelectCANWnd.m_H5Protocol == H5Protocol.BO_QIANG)
            {
                gbDidi.IsEnabled = false;
                m_ListCellVoltage.Clear();
                m_ListBmsInfo.Clear();

                string strConfigFile = XmlHelper.m_strCfgFilePath;

                XmlHelper.LoadXmlConfig(strConfigFile, "bms_info/cell_votage_info", m_ListCellVoltage);
                XmlHelper.LoadXmlConfig(strConfigFile, "bms_info/bms_info_node", m_ListBmsInfo);
            }
            else
            {
                gbBoQiang.IsEnabled = false;
            }
        }


        private void UpdateDebugWndStatus()
        {
            if (SelectCANWnd.m_H5Protocol == H5Protocol.BO_QIANG)
            {
                gbDidi.IsEnabled = false;
            }
            else
            {
                gbBoQiang.IsEnabled = false;
            }

            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                userCtrlDebug.IsEnabled = true;
            }
            else
            {
                userCtrlDebug.IsEnabled = false;
            }

        }

        public void HandleDebugWndUpdateEvent(object sender, EventArgs e)
        {
            UpdateDebugWndStatus();
        }

        public void HandleDebugEvent(object sender, CustomRecvDataEventArgs e)
        {
            BqProtocol.bReadBqBmsResp = true;
            switch (e.RecvMsg[0])
            {
                case 0xD0:
                    if (isJumpBoot)
                    {
                        MessageBox.Show("跳转boot成功！", "跳转boot提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        isJumpBoot = false;
                    }
                    break;
                case 0xD1:
                    if (isSoftReset)
                    {
                        MessageBox.Show("系统复位成功！", "系统复位提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        isSoftReset = false;
                    }
                    break;
                case 0xD2:
                    if (isAlterSOC)
                    {
                        if(isRequireAlterSOC)
                        {
                            AlterSOCOverEvent?.Invoke(this, new EventArgs<bool>(true));
                            isRequireAlterSOC = false;
                        }
                        else
                            MessageBox.Show("SOC设置成功！", "设置SOC提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        isAlterSOC = false;
                    }
                    break;
                case 0xD3:
                    if (isFactoryReset)
                    {
                        MessageBox.Show("恢复出厂设置成功！", "恢复出厂设置提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        isFactoryReset = false;
                    }
                    break;
                case 0xDB:
                    if (isTestMode)
                    {
                        MessageBox.Show("进入测试模式设置成功！", "进入测试模式提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        isTestMode = false;
                    }
                    if (isExitTestMode)
                    {
                        MessageBox.Show("退出测试模式设置成功！", "退出测试模式提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        isExitTestMode = false;
                    }
                    break;
                //case 0xD4:
                //    //if (e.RecvMsg.Count == 0x03)
                //    {
                //        MessageBox.Show("进入关机状态设置成功！", "进入关机状态提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //    }
                //    break;
                //case 0xD5:
                //    //if (e.RecvMsg.Count == 0x03)
                //    {
                //        MessageBox.Show("进入休眠设置成功！", "进入休眠提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //    }
                //    break;
                default:
                    break;

            }


        }

        private void btnConnect_Click(object sender, RoutedEventArgs e)
        {
            InitDebugWnd();
        }

        bool isBqPowerOn = false;
        bool isBqPowerOff = false;

        public void RequirePowerOn()
        {
            isRequirePowerOn = true;
            btnPowerOn_Click(null, null);
        }
        /// <summary>
        /// 上电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerOn_Click(object sender, RoutedEventArgs e)
        { 
            isBqPowerOn = false;
            BqProtocol.BqInstance.m_bIsStopCommunication = true;
            Thread.Sleep(200);
            DdProtocol.DdInstance.DD_PowerOn();
            isBqPowerOn = true;
        }

        public event EventHandler PowerOnOverEvent;
        public event EventHandler PowerOffOverEvent;
        bool isRequirePowerOn = false;
        bool isRequirePowerOff = false;
        public void RequirePowerOff()
        {
            isRequirePowerOff = true;
            btnPowerOff_Click(null, null);
        }
        /// <summary>
        /// 下电
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPowerOff_Click(object sender, RoutedEventArgs e)
        {
            isBqPowerOff = false;
            DdProtocol.DdInstance.DD_PowerOff();
            isBqPowerOff = true;
        }

        public void HandleRaisePowerOnOffEvent(object sender, EventArgs e)
        {
            DdProtocol.bReadDdBmsResp = true;
            if(SelectCANWnd.m_H5Protocol == H5Protocol.BO_QIANG)
            {
                BqProtocol.BqInstance.m_bIsStopCommunication = false;
            }
            if(isDdPowerOn || isBqPowerOn)
            {
                if (isRequirePowerOn)
                {
                    isRequirePowerOn = false;
                    PowerOnOverEvent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("上电成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                isDdPowerOn = false;
                isBqPowerOn = false;
            }

            if (isDdPowerOff || isBqPowerOff)
            {
                if(isRequirePowerOff)
                {
                    isRequirePowerOff = false;
                    PowerOffOverEvent?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    MessageBox.Show("下电成功！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                isDdPowerOff = false;
                isBqPowerOff = false;
            }


        }

        bool isJumpBoot = false;
        private void btnJumpBoot_Click(object sender, RoutedEventArgs e)
        {
            isJumpBoot = false;
            BqProtocol.BqInstance.BQ_JumpToBoot();
            isJumpBoot = true;
        }

        bool isSoftReset = false;
        private void btnSoftReset_Click(object sender, RoutedEventArgs e)
        {
            isSoftReset = false;
            BqProtocol.BqInstance.BQ_Reset();
            isSoftReset = true;
        }

        bool isAlterSOC = false;
        bool isRequireAlterSOC = false;
        public event EventHandler<EventArgs<bool>> AlterSOCOverEvent;
        public void RequireAlterSOC(string val)
        {
            tbSOC.Text = val;
            isRequireAlterSOC = true;
            btnAlterSOC_Click(null, null);
        }
        private void btnAlterSOC_Click(object sender, RoutedEventArgs e)
        {

            string str = @"^[0-9]{1,3}$";
            if (!Regex.IsMatch(tbSOC.Text, str))    // if (string.IsNullOrEmpty(txtBoxBarcode.Text))
            {
                MessageBox.Show("请输入正确的 SOC 值！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            byte socVal = byte.Parse(tbSOC.Text);

            if (socVal < 0 || socVal > 100)
            {
                MessageBox.Show("请输入正确的 SOC 值！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            isAlterSOC = false;
            BqProtocol.BqInstance.BQ_AlterSOC(socVal);
            isAlterSOC = true;
        }

        public void HandleRecvBmsInfoDataEvent(object sender, CustomRecvDataEventArgs e)
        {
            if (e.RecvMsg[0] != 0xA1 || e.RecvMsg.Count < 0x60)
            {
                return;
            }
            int nBqByteIndex = 1;
            for (int n = 0; n < m_ListCellVoltage.Count; n++)
            {
                int nCellVol = 0;
                for (int m = 0; m < m_ListCellVoltage[n].ByteCount; m++)
                {
                    nCellVol = (nCellVol << 8 | e.RecvMsg[nBqByteIndex + m]);
                }

                m_ListCellVoltage[n].StrValue = nCellVol.ToString();

                nBqByteIndex += m_ListCellVoltage[n].ByteCount;

            }

            for (int i = 0; i < m_ListBmsInfo.Count; i++)
            {
                int nBmsVal = 0;

                if (!m_ListBmsInfo[i].Description.Contains("状态"))
                {
                    for (int j = 0; j < m_ListBmsInfo[i].ByteCount; j++)
                    {
                        nBmsVal = (nBmsVal << 8 | e.RecvMsg[nBqByteIndex + j]);
                    }
                    //lipeng     2020.3.26  修改电芯温度1为环境温度
                    if (m_ListBmsInfo[i].Description == "环境温度" || m_ListBmsInfo[i].Description == "电芯温度2" || m_ListBmsInfo[i].Description == "电芯温度3")
                    {
                        //if (nBmsVal != 0)
                        {
                            double dVal = (nBmsVal - 2731) / 10.0;
                            m_ListBmsInfo[i].StrValue = dVal.ToString("F1");
                        }
                    }
                    else if (m_ListBmsInfo[i].Description == "满充容量" || m_ListBmsInfo[i].Description == "剩余电量")
                    {
                        UInt32 cap = (UInt32)nBmsVal;
                        m_ListBmsInfo[i].StrValue = cap.ToString();
                    }
                    else if (m_ListBmsInfo[i].Description == "最高电压单体号" || m_ListBmsInfo[i].Description == "最低电压单体号")
                    {
                        UInt32 nCellIndex = (UInt32)nBmsVal + 1;
                        m_ListBmsInfo[i].StrValue = nCellIndex.ToString();
                    }
                    else if (m_ListBmsInfo[i].Description == "电芯温度4" || m_ListBmsInfo[i].Description == "电芯温度5" || m_ListBmsInfo[i].Description == "电芯温度6"
                        || m_ListBmsInfo[i].Description == "电芯温度7" || m_ListBmsInfo[i].Description == "功率温度")
                    {
                        //计算一个字节为正负值的问题
                        if (nBmsVal > 127)
                            m_ListBmsInfo[i].StrValue = (nBmsVal - 256).ToString();
                        else
                            m_ListBmsInfo[i].StrValue = nBmsVal.ToString();
                    }
                    else
                    {
                        m_ListBmsInfo[i].StrValue = nBmsVal.ToString();
                    }
                }

                else
                {
                    string strStatus = "";
                    for (int k = 0; k < m_ListBmsInfo[i].ByteCount; k++)
                    {
                        if (m_ListBmsInfo[i].Description == "Pack状态")
                        {
                            btSysStatus[k] = e.RecvMsg[nBqByteIndex + k];
                        }
                        else if (m_ListBmsInfo[i].Description == "电池状态")
                        {
                            btPackStatus[k] = e.RecvMsg[nBqByteIndex + k];
                        }
                        else if (m_ListBmsInfo[i].Description == "均衡状态")
                        {
                            btBalanceStatus[k] = e.RecvMsg[nBqByteIndex + k];
                        }

                        strStatus += e.RecvMsg[nBqByteIndex + k].ToString("X2") + " ";
                    }
                    m_ListBmsInfo[i].StrValue = strStatus;
                }

                nBqByteIndex += m_ListBmsInfo[i].ByteCount;
            }

            m_ListBmsInfo[m_ListBmsInfo.Count - 1].StrValue = (double.Parse(m_ListBmsInfo[18].StrValue) - double.Parse(m_ListBmsInfo[20].StrValue)).ToString();

            System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
            {
                var item = m_ListBmsInfo.SingleOrDefault(p => p.Description == "SOC");
                if (null != item)
                {
                    tbCurrentSOC.Text = item.StrValue.ToString();
                }
            }), null);
        }

        bool isFactoryReset = false;
        private void btnFactoryReset_Click(object sender, RoutedEventArgs e)
        {
            isFactoryReset = false;
            BqProtocol.BqInstance.BQ_FactoryReset();
            isFactoryReset = true;
        }

        // 5
        private void btnShutdown_Click(object sender, RoutedEventArgs e)
        {
            //BqProtocol.BqInstance.BQ_Shutdown();
        }

        // 6
        private void btnSleep_Click(object sender, RoutedEventArgs e)
        {
            //BqProtocol.BqInstance.BQ_Sleep();
        }

        bool isReadBoot = false;
        public void RequireReadBoot(bool _isRequireRead)
        {
            isRequireRead = _isRequireRead;
            btnReadBoot_Click(null, null);
        }
        private void btnReadBoot_Click(object sender, RoutedEventArgs e)
        {
            isReadBoot = false;
            BqProtocol.BqInstance.BQ_ReadBootInfo();
            isReadBoot = true;
        }
        public event EventHandler<EventArgs<string>> ReadBootOverEvent;
        bool isRequireRead = false;
        public void HandleReadBqBootEvent(object sender, CustomRecvDataEventArgs e)
        {
            try
            {
                if(isReadBoot)
                {
                    if (e.RecvMsg[0] != 0xDA || e.RecvMsg.Count < 0x29)
                    {
                        return;
                    }

                    int offset = 1;
                    byte[] array = e.RecvMsg.ToArray();
                    string projectName = System.Text.Encoding.ASCII.GetString(array, offset, 16);
                    offset += 16;
                    string hardwareVersion = System.Text.Encoding.ASCII.GetString(array, offset, 8);
                    offset += 8;
                    string bootVersion = System.Text.Encoding.ASCII.GetString(array, offset, 8);
                    offset += 8;
                    string appNum = System.Text.Encoding.ASCII.GetString(array, offset, 8);

                    List<string> list = new List<string>();
                    list.Add(projectName.Substring(0, projectName.IndexOf('\0')));
                    list.Add(hardwareVersion.Substring(0, hardwareVersion.IndexOf('\0')));
                    list.Add(bootVersion.Substring(0, bootVersion.IndexOf('\0')));
                    list.Add(appNum.Substring(0, appNum.IndexOf('\0')));

                    UpdateBqBootInfo(list);
                    if(isRequireRead)
                    {
                        ReadBootOverEvent?.Invoke(this, new EventArgs<string>(list[0]));
                        isRequireRead = false;
                    }
                    else
                        MessageBox.Show("读取Boot信息成功！", "读取Boot提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    isReadBoot = false;
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void UpdateBqBootInfo(List<string> list)
        {
            if (list.Count == 4)
            {
                System.Windows.Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    tbProjectName.Text = list[0].Trim();
                    tbHardwareVersion.Text = list[1].Trim();
                    tbBootVersion.Text = list[2].Trim();
                    tbAppNum.Text = list[3].Trim();
                }));
            }
        }

        bool isAdjustRTC = false;
        private void btnAdjustRTC_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime dt = DateTime.Parse(tbAlterRTC.Text.Trim());
                if (null != dt)
                {
                    if(dt.Year < 2000 || dt.Year > 2999)
                    {
                        MessageBox.Show("RTC的年份范围为：2000~2999");
                    }
                    else
                    {
                        isAdjustRTC = false;
                        BqProtocol.bReadBqBmsResp = true;
                        BqProtocol.BqInstance.AdjustRTC(dt);
                        isAdjustRTC = true;
                    }
                }
                else
                {
                    MessageBox.Show("请检查RTC时间格式是否正确！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("请检查RTC时间格式是否正确！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        public void HandleAdjustRTCEvent(object sender, CustomRecvDataEventArgs e)
        {
            if(isAdjustRTC)
            {
                if (SelectCANWnd.m_H5Protocol == H5Protocol.BO_QIANG)
                {
                    BqProtocol.bReadBqBmsResp = true;
                    if (e.RecvMsg[0] == 0xB5)
                    {
                        MessageBox.Show("校准RTC成功！", "校准提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        BqProtocol.BqInstance.BQ_ReadRTC();
                    }
                    else
                    {
                        MessageBox.Show("校准RTC失败！", "校准提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                else
                {
                    DDProtocol.DdProtocol.bReadDdBmsResp = true;
                    if (e.RecvMsg[0] == 0x10)
                    {
                        MessageBox.Show("校准UTC成功！", "校准提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        DDProtocol.DdProtocol.DdInstance.Didi_ReadRTC();
                    }
                    else
                    {
                        MessageBox.Show("校准UTC失败！", "校准提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
                isAdjustRTC = false;
            }

        }

        public void HandleReadBqRTCEvent(object sender, CustomRecvDataEventArgs e)
        {
            if (SelectCANWnd.m_H5Protocol == H5Protocol.BO_QIANG)
            {
                BqProtocol.bReadBqBmsResp = true;
                if (e.RecvMsg[0] == 0xA5)
                {
                    string second = (e.RecvMsg[1] & 0xFF).ToString("X2");
                    string minute = (e.RecvMsg[2] & 0xFF).ToString("X2");
                    string hour = (e.RecvMsg[3] & 0xFF).ToString("X2");
                    string week = (e.RecvMsg[4] & 0xFF).ToString("X2");
                    string day = (e.RecvMsg[5] & 0xFF).ToString("X2");
                    string month = (e.RecvMsg[6] & 0xFF).ToString("X2");
                    string year = (e.RecvMsg[7] & 0xFF).ToString("X2");

                    tbCurrentRTC.Text = string.Format("{0}/{1}/{2} {3}:{4}:{5}", "20" + year, month, day, hour, minute, second);
                }
            }
            else
            {
                DDProtocol.DdProtocol.bReadDdBmsResp = true;
                if (e.RecvMsg[0] == 0x03 && e.RecvMsg[1] == 0x04)
                {
                    int nRegister = ((e.RecvMsg[2] << 24) | (e.RecvMsg[3] << 16) | (e.RecvMsg[4] << 8) | e.RecvMsg[5]);
                    tbDdCurrentUTC.Text = nRegister.ToString();
                    TimeSpan ts = new TimeSpan((long)(nRegister * Math.Pow(10, 7)));
                    tbDdUTCRTC.Text = (new DateTime(1970,1,1,8,0,0) + ts).ToString("yyyy/MM/dd HH:mm:ss");
                };
            }
        }

        bool isDdPowerOn = false;
        bool isDdPowerOff = false;
        private void btnDDPowerOff_Click(object sender, RoutedEventArgs e)
        {
            isDdPowerOff = false;
            DdProtocol.DdInstance.DD_PowerOff();
            isDdPowerOff = true;
        }

        private void btnDDPowerOn_Click(object sender, RoutedEventArgs e)
        {
            isDdPowerOn = false;
            DdProtocol.DdInstance.DD_PowerOn();
            isDdPowerOn = true;
        }

        private void btnDdAdjustUTC_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.m_statusBarInfo.IsOnline)
            {
                try
                {
                    uint dt = 0;
                    bool ret = UInt32.TryParse(tbDdAlterUTC.Text.Trim(),out dt);
                    if(dt >= 4294967295)
                    {
                        MessageBox.Show("UTC值的范围0~4294967295，请检查UTC输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        if (ret)
                        {
                            isAdjustRTC = false;
                            DDProtocol.DdProtocol.DdInstance.AdjustDidiRTC(dt);
                            isAdjustRTC = true;
                        }
                        else
                        {
                            MessageBox.Show("请检查修改UTC值是否正确！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("请检查修改UTC值是否正确！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("系统未连接，请连接后再进行操作！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnCalRTC_Click(object sender, RoutedEventArgs e)
        {
            uint dt = 0;
            bool ret = UInt32.TryParse(tbDdAlterUTC.Text.Trim(), out dt);
            if (dt >= 4294967295)
            {
                MessageBox.Show("UTC值的范围0~4294967294，请检查UTC输入！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (ret)
                {
                    TimeSpan ts = new TimeSpan((long)(dt * Math.Pow(10, 7)));
                    tbDdAlterRTC.Text = (new DateTime(1970, 1, 1, 8, 0, 0) + ts).ToString("yyyy/MM/dd HH:mm:ss");
                }
                else
                {
                    MessageBox.Show("请检查修改UTC值是否正确！", "计算时间提示", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
        }

        private void btnCalUTC_Click(object sender, RoutedEventArgs e)
        {
            DateTime dt;
            bool ret = DateTime.TryParse(tbDdAlterRTC.Text.Trim(),out dt);
            if (ret)
            {
                TimeSpan ts = dt - new DateTime(1970, 1, 1, 8, 0, 0);
                long ticks = (long)(ts.Ticks / Math.Pow(10,7));
                if (ticks >= 4294967295)
                {
                    MessageBox.Show("输入时间大于最大UTC值，请输入正确的时间！","计算UTC提示",MessageBoxButton.OK,MessageBoxImage.Information);
                }
                else
                {
                    tbDdAlterUTC.Text = ticks.ToString();
                }
            }
            else
            {
                MessageBox.Show("请检查RTC时间格式是否正确！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void btnWrite_Click(object sender, RoutedEventArgs e)
        {
            byte[] registerAddrBytes;
            byte registerNum;
            byte[] dataBytes;
            string regexStr = @"([^A-Fa-f0-9]|\s+?)+";
            string registerAddrStr = tbRegisterAddr.Text.Trim();
            if(!string.IsNullOrEmpty(registerAddrStr))
            {
                registerAddrStr = registerAddrStr.Replace(" ", "");
                if ((registerAddrStr.Length % 2) != 0)
                    registerAddrStr += " ";
                if (!Regex.IsMatch(registerAddrStr, regexStr))
                {
                    registerAddrBytes = new byte[registerAddrStr.Length / 2];
                    for (int i = 0; i < registerAddrBytes.Length; i++)
                        registerAddrBytes[i] = Convert.ToByte(registerAddrStr.Substring(i * 2, 2), 16);
                }
                else
                {
                    MessageBox.Show("输入的寄存器地址包含非十六进制数字，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入寄存器地址！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string registerNumStr = tbRegisterNum.Text.Trim();
            if (!string.IsNullOrEmpty(registerNumStr))
            {
                uint num = 0;
                if(uint.TryParse(registerNumStr, out num))
                {
                    if(num > 255 || num <= 0)
                    {
                        MessageBox.Show("请输入正确的寄存器个数！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    registerNum = Convert.ToByte(num);
                }
                else
                {
                    MessageBox.Show("请输入正确的寄存器个数！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入寄存器个数！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string dataStr = tbData.Text.Trim();
            if (!string.IsNullOrEmpty(dataStr))
            {
                dataStr = dataStr.Replace(" ", "");
                if ((dataStr.Length % 2) != 0)
                    dataStr += " ";
                //if(Regex.IsMatch(dataStr,regexStr))
                {
                    dataBytes = new byte[dataStr.Length / 2];
                    for (int i = 0; i < dataBytes.Length; i++)
                        dataBytes[i] = Convert.ToByte(dataStr.Substring(i * 2, 2), 16);

                    if (dataBytes.Length != 2 * Convert.ToInt16(registerNum))
                    {
                        MessageBox.Show("输入的寄存器个数和输入的数据不匹配，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                //else
                //{
                //    MessageBox.Show("输入的数据包含非十六进制数字，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                //    return;
                //}

            }
            else
            {
                MessageBox.Show("请输入寄存器个数！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
        }

        public void HandleWriteRegisterEvent(object sender, CustomRecvDataEventArgs e)
        {
            if(e.RecvMsg[0] == 0x10)
            {
                MessageBox.Show("写寄存器成功！", "写寄存器提示", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        bool isTestMode = false;
        private void btnEnterTestMode_Click(object sender, RoutedEventArgs e)
        {
            isTestMode = false;
            BqProtocol.BqInstance.BQ_EnterTestMode();
            isTestMode = true;
        }
        bool isExitTestMode = false;
        private void btnExitTestMode_Click(object sender, RoutedEventArgs e)
        {
            isExitTestMode = false;
            BqProtocol.BqInstance.BQ_ExitTestMode();
            isExitTestMode = true;
        }
        bool isRead = false;
        private void btnRead_Click(object sender, RoutedEventArgs e)
        {
            byte[] registerAddrBytes;
            byte registerNum;
            string regexStr = @"^[A-Fa-f0-9]+$";
            string registerAddrStr = tbRegisterAddr.Text.Trim();
            if (!string.IsNullOrEmpty(registerAddrStr))
            {
                registerAddrStr = registerAddrStr.Replace(" ", "");
                if (Regex.IsMatch(registerAddrStr, regexStr))
                {
                    if ((registerAddrStr.Length % 2) != 0)
                        registerAddrStr += "0";
                    registerAddrBytes = new byte[registerAddrStr.Length / 2];
                    for (int i = 0; i < registerAddrBytes.Length; i++)
                        registerAddrBytes[i] = Convert.ToByte(registerAddrStr.Substring(i * 2, 2), 16);
                }
                else
                {
                    MessageBox.Show("输入的寄存器地址包含非十六进制数字，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入寄存器地址！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            string registerNumStr = tbRegisterNum.Text.Trim();
            if (!string.IsNullOrEmpty(registerNumStr))
            {
                uint num = 0;
                if (uint.TryParse(registerNumStr, out num))
                {
                    if (num > 255 || num <= 0)
                    {
                        MessageBox.Show("请输入正确的寄存器个数！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    registerNum = Convert.ToByte(num);
                }
                else
                {
                    MessageBox.Show("请输入正确的寄存器个数！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            else
            {
                MessageBox.Show("请输入寄存器个数！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            isRead = false;
            DdProtocol.DdInstance.m_bIsStopCommunication = true;
            DdProtocol.DdInstance.DD_ReadRegister(registerAddrBytes, registerNum);
            isRead = true;
        }

        public void HandleReadRegisterEvent(object sender, CustomRecvDataEventArgs e)
        {
            try
            {
                if (isRead)
                {
                    if (e.RecvMsg[0] == 0x03)
                    {
                        string registerNumStr = tbRegisterNum.Text.Trim();
                        int registerNum = int.Parse(registerNumStr);
                        byte[] bytes = new byte[registerNum * 2 + 4];
                        if (e.RecvMsg.Count() >= bytes.Length)
                            Buffer.BlockCopy(e.RecvMsg.ToArray(), 0, bytes, 0, bytes.Length);
                        else
                            Buffer.BlockCopy(e.RecvMsg.ToArray(), 0, bytes, 0, e.RecvMsg.Count());
                        tbData.Text = BitConverter.ToString(bytes);
                        MessageBox.Show("读寄存器成功！", "读寄存器提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        DdProtocol.DdInstance.m_bIsStopCommunication = false;
                    }
                    isRead = false;

                }
            }
            catch (Exception ex)
            {
                isRead = false;
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            tbData.Text = string.Empty;
        }
    }
}
