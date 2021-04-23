using System;
using System.Windows;
using System.Windows.Controls;
//using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;
using BoqiangH5Entity;
using BoqiangH5Repository;
using BoqiangH5.ISO15765;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Drawing;

namespace BoqiangH5
{
    public partial class SelectCANWnd : Window
    {
        public event EventHandler RaiseCloseEvent;
        
        public static ObservableCollection<ZLGFuction> tabSource = new ObservableCollection<ZLGFuction> { };
        ZLGFuction m_zlgFun;

        static H5Protocol h5Protocol = H5Protocol.BO_QIANG;
        static int handShakeInterval = 3;
        static int wakeInterval = 5;
        static int recordInterval = 1;
        static bool isUsingKP182 = true;
        static bool isUsingVoltmeter = true;
        static bool isAutoSetting = true;
        static bool isAutoCheck = true;
        static bool isAutoChargeorDischarge = true;
        static int currentValue = 10000;
        static int waitTime = 10000;
        static int currentError = 10000;
        static int currentValue_2 = 10000;
        static int waitTime_2 = 10000;
        static int currentError_2 = 10000;
        static bool isUsingKP182_2 = true;

        static bool isUsingAmperemeter = true;
        static bool isUsingTH10SB = true;
        public static H5Protocol m_H5Protocol
        {
            get 
            {
                if (XmlHelper.m_strProtocol == "0")
                {
                    h5Protocol = H5Protocol.BO_QIANG;
                }
                else
                {
                    h5Protocol = H5Protocol.DI_DI;
                }
                return h5Protocol; 
            }
        }

        public static int m_HandShakeTime 
        { 
            get 
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strHandShakeInterval, out val);
                if (val != -1)
                {
                    handShakeInterval = val;
                }
                return handShakeInterval;
            } 
        }
        public static int m_WakeInterval 
        { 
            get 
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strWakeInterval, out val);
                if (val != -1)
                {
                    wakeInterval = val;
                }
                return wakeInterval;
            } 
        }
        public static int m_RecordInterval 
        { 
            get 
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strDataRecordInterval, out val);
                if (val != -1)
                {
                    if (val == 0) recordInterval = 1;
                    else if (val == 1) recordInterval = 2;
                    else if (val == 2) recordInterval = 3;
                    else if (val == 3) recordInterval = 4;
                    else if (val == 4) recordInterval = 5;
                    else recordInterval = 1;
                }
                return recordInterval;
            } 
        }

        public static bool m_IsUsingKP182
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strIsUsingKP182, out val);
                if (val != -1)
                {
                    if(val == 1)
                    {
                        isUsingKP182 = true;
                    }
                    else
                    {
                        isUsingKP182 = false;
                    }
                }
                return isUsingKP182;
            }
        }

        public static bool m_IsUsingVoltmeter
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strIsUsingVoltmeter, out val);
                if (val != -1)
                {
                    if (val == 1)
                    {
                        isUsingVoltmeter = true;
                    }
                    else
                    {
                        isUsingVoltmeter = false;
                    }
                }
                return isUsingVoltmeter;
            }
        }

        public static bool m_IsUsingAmperemeter
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strIsUsingAmperemeter, out val);
                if (val != -1)
                {
                    if (val == 1)
                    {
                        isUsingAmperemeter = true;
                    }
                    else
                    {
                        isUsingAmperemeter = false;
                    }
                }
                return isUsingAmperemeter;
            }
        }

        public static bool m_IsUsingTH10SB
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strIsUsingTH10SB, out val);
                if (val != -1)
                {
                    if (val == 1)
                    {
                        isUsingTH10SB = true;
                    }
                    else
                    {
                        isUsingTH10SB = false;
                    }
                }
                return isUsingTH10SB;
            }
        }

        public static bool m_IsAutoSetting
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strAutoFactorySetting, out val);
                if (val != -1)
                {
                    if (val == 1)
                    {
                        isAutoSetting = true;
                    }
                    else
                    {
                        isAutoSetting = false;
                    }
                }
                return isAutoSetting;
            }
        }

        public static bool m_IsAutoCheck
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strAutoFactoryCheck, out val);
                if (val != -1)
                {
                    if (val == 1)
                    {
                        isAutoCheck = true;
                    }
                    else
                    {
                        isAutoCheck = false;
                    }
                }
                return isAutoCheck;
            }
        }

        public static bool m_IsAutoChargeOrDischarge
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strAutoChargeOrDischarge, out val);
                if (val != -1)
                {
                    if (val == 1)
                    {
                        isAutoChargeorDischarge = true;
                    }
                    else
                    {
                        isAutoChargeorDischarge = false;
                    }
                }
                return isAutoChargeorDischarge;
            }
        }

        public static int m_CurrentValue
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCurrentValue, out val);
                if (val != -1)
                {
                    currentValue = val;
                }
                return currentValue;
            }
        }

        public static int m_WaitTime
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strWaitTime, out val);
                if (val != -1)
                {
                    waitTime = val;
                }
                return waitTime;
            }
        }
        public static int m_CurrentError
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCurrentError, out val);
                if (val != -1)
                {
                    currentError = val;
                }
                return currentError;
            }
        }
        public static bool m_IsUsingKP182_2
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strIsUsingKP182_2, out val);
                if (val != -1)
                {
                    if (val == 1)
                    {
                        isUsingKP182_2 = true;
                    }
                    else
                    {
                        isUsingKP182_2 = false;
                    }
                }
                return isUsingKP182_2;
            }
        }
        public static int m_CurrentValue_2
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCurrentValue_2, out val);
                if (val != -1)
                {
                    currentValue_2 = val;
                }
                return currentValue_2;
            }
        }

        public static int m_WaitTime_2
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strWaitTime_2, out val);
                if (val != -1)
                {
                    waitTime_2 = val;
                }
                return waitTime_2;
            }
        }
        public static int m_CurrentError_2
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCurrentError_2, out val);
                if (val != -1)
                {
                    currentError_2 = val;
                }
                return currentError_2;
            }
        }
        public SelectCANWnd()        
        {
            m_zlgFun = DataLinkLayer.DllZLGFun;

            InitializeComponent();
            List<String> listPorts = new List<String>();
            foreach (String portName in System.IO.Ports.SerialPort.GetPortNames())
            {
                if (portName != null)
                    listPorts.Add(portName);
            }
            cbKP182ComNum.ItemsSource = listPorts;
            cbAmperemeterComNum.ItemsSource = listPorts;
            cbVoltmeterComNum.ItemsSource = listPorts;
            cbTH10SBComNum.ItemsSource = listPorts;
            cbKP182ComNum_2.ItemsSource = listPorts;
            cbKP182BaudRate.ItemsSource = new List<string> { "1200","2400","4800","9600","14400","19200","38400","57600","115200" };
            cbVoltmeterBaudRate.ItemsSource = new List<string> { "1200", "2400", "4800", "9600" };
            cbAmperemeterBaudRate.ItemsSource = new List<string> { "1200", "2400", "4800", "9600" };
            cbTH10SBBaudRate.ItemsSource = new List<string> { "1200", "2400", "4800", "9600", "14400", "19200" };
            cbKP182BaudRate_2.ItemsSource = new List<string> { "1200", "2400", "4800", "9600", "14400", "19200", "38400", "57600", "115200" };
            InitConfigInfo();
        }

        private void InitConfigInfo()
        {
            XmlHelper.LoadConfigInfo(true);

            if (string.IsNullOrEmpty(XmlHelper.m_strCanType) || (string.IsNullOrEmpty(XmlHelper.m_strBaudrate)))
            {
                MessageBox.Show("请设置CAN类型和波特率！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
                        
            int nIndex = -1;

            Int32.TryParse(XmlHelper.m_strCanType, out nIndex);
            if (nIndex != -1)
            {
                cbCanType.SelectedIndex = nIndex;
            }

            Int32.TryParse(XmlHelper.m_strCanIndex, out nIndex);
            if (nIndex != -1)
            {
                cbCanIndex.SelectedIndex = nIndex;
            }
            else
            {
                cbCanIndex.SelectedIndex = 0;
            }
            
            Int32.TryParse(XmlHelper.m_strCanChannel, out nIndex);
            if (nIndex != -1)
            {
                cbCanChannel.SelectedIndex = nIndex;
            }
            else
            {
                cbCanChannel.SelectedIndex = 0;
            }

            Int32.TryParse(XmlHelper.m_strBaudrate, out nIndex);
            if (nIndex != -1)
            {
                cbBaudRate.SelectedIndex = nIndex;
            }

            Int32.TryParse(XmlHelper.m_strProtocol, out nIndex);
            if (nIndex >= 0 && nIndex < Enum.GetNames(typeof(H5Protocol)).GetLength(0))
            {
                cbProtocol.SelectedIndex = nIndex;
                if (nIndex == 0)
                {
                    h5Protocol = H5Protocol.BO_QIANG;
                }
                else
                {
                    h5Protocol = H5Protocol.DI_DI;
                }
            }

            Int32.TryParse(XmlHelper.m_strCanFD, out nIndex);
            if (nIndex != -1)
            {
                cbCANFD.SelectedIndex = nIndex;
            }

            Int32.TryParse(XmlHelper.m_strArbitration, out nIndex);
            if (nIndex != -1)
            {
                cbArbitrationBaudRate.SelectedIndex = nIndex;
            }

            Int32.TryParse(XmlHelper.m_strDataBaudRate, out nIndex);
            if (nIndex != -1)
            {
                cbDataBaudRate.SelectedIndex = nIndex;
            }

            Int32.TryParse(XmlHelper.m_strTerminalResistance, out nIndex);
            if (nIndex != -1)
            {
                if(nIndex == 1)
                {
                    chbIsTerminalResistance.IsChecked = true;
                }
                else
                {
                    chbIsTerminalResistance.IsChecked = false;
                }
            }

            Int32.TryParse(XmlHelper.m_strDataRecordInterval, out nIndex);
            if (nIndex != -1)
            {
                cbRecordInterval.SelectedIndex = nIndex;
                if (nIndex == 0) recordInterval = 1;
                else if (nIndex == 1) recordInterval = 2;
                else if (nIndex == 2) recordInterval = 3;
                else if (nIndex == 3) recordInterval = 4;
                else if (nIndex == 4) recordInterval = 5;
                else recordInterval = 1;
            }

            Int32.TryParse(XmlHelper.m_strCurrentValue, out nIndex);
            if (nIndex != -1)
            {
                currentValue = nIndex;
                tbCurrentValue.Text = nIndex.ToString();
            }

            Int32.TryParse(XmlHelper.m_strWaitTime, out nIndex);
            if (nIndex != -1)
            {
                waitTime = nIndex;
                tbWaitTime.Text = nIndex.ToString();
            }
            Int32.TryParse(XmlHelper.m_strCurrentError, out nIndex);
            if (nIndex != -1)
            {
                currentError = nIndex;
                tbCurrentError.Text = nIndex.ToString();
            }
            //Int32.TryParse(XmlHelper.m_strKP182Com, out nIndex);
            //if (nIndex != -1)
            //{
            //    cbKP182ComNum.SelectedIndex = nIndex;
            //}
            cbKP182ComNum.SelectedItem = XmlHelper.m_strKP182Com;
            cbKP182BaudRate.SelectedItem = XmlHelper.m_strKP182BaudRate;
            //Int32.TryParse(XmlHelper.m_strKP182BaudRate, out nIndex);
            //if (nIndex != -1)
            //{
            //    cbKP182BaudRate.SelectedIndex = nIndex;
            //}
            //Int32.TryParse(XmlHelper.m_strKP182DateBit, out nIndex);
            //if (nIndex != -1)
            //{
            //    cbKP182DataBit.SelectedIndex = nIndex;
            //}
            //Int32.TryParse(XmlHelper.m_strKP182ParityBit, out nIndex);
            //if (nIndex != -1)
            //{
            //    cbKP182ParityBit.SelectedIndex = nIndex;
            //}
            //Int32.TryParse(XmlHelper.m_strKP182StopBit, out nIndex);
            //if (nIndex != -1)
            //{
            //    cbKP182StopBit.SelectedIndex = nIndex;
            //}
            tbKP182DeviceAddress.Text = XmlHelper.m_strKP182DeviceAddress;

            Int32.TryParse(XmlHelper.m_strIsUsingKP182, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsUseKP182.IsChecked = true;
                }
                else
                {
                    chbIsUseKP182.IsChecked = false;
                }
            }

            Int32.TryParse(XmlHelper.m_strCurrentValue_2, out nIndex);
            if (nIndex != -1)
            {
                currentValue_2 = nIndex;
                tbCurrentValue_2.Text = nIndex.ToString();
            }

            Int32.TryParse(XmlHelper.m_strWaitTime_2, out nIndex);
            if (nIndex != -1)
            {
                waitTime_2 = nIndex;
                tbWaitTime_2.Text = nIndex.ToString();
            }
            Int32.TryParse(XmlHelper.m_strCurrentError_2, out nIndex);
            if (nIndex != -1)
            {
                currentError_2 = nIndex;
                tbCurrentError_2.Text = nIndex.ToString();
            }
            cbKP182ComNum_2.SelectedItem = XmlHelper.m_strKP182Com_2;
            cbKP182BaudRate_2.SelectedItem = XmlHelper.m_strKP182BaudRate_2;
            tbKP182Address_2.Text = XmlHelper.m_strKP182DeviceAddress_2;
            Int32.TryParse(XmlHelper.m_strIsUsingKP182_2, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsUseKP182_2.IsChecked = true;
                }
                else
                {
                    chbIsUseKP182_2.IsChecked = false;
                }
            }

            Int32.TryParse(XmlHelper.m_strIsUsingVoltmeter, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsUseVoltmeter.IsChecked = true;
                }
                else
                {
                    chbIsUseVoltmeter.IsChecked = false;
                }
            }

            cbVoltmeterComNum.SelectedItem = XmlHelper.m_strVoltmeterCom;
            cbVoltmeterBaudRate.SelectedItem = XmlHelper.m_strVoltmeterBaudRate;
            tbVoltmeterAddress.Text = XmlHelper.m_strVoltmeterAddress;

            cbAmperemeterComNum.SelectedItem = XmlHelper.m_strAmperemeterCom;
            cbAmperemeterBaudRate.SelectedItem = XmlHelper.m_strAmperemeterBaudRate;
            tbAmperemeterAddress.Text = XmlHelper.m_strAmperematerAddress;
            Int32.TryParse(XmlHelper.m_strIsUsingAmperemeter, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsUseAmperemeter.IsChecked = true;
                }
                else
                {
                    chbIsUseAmperemeter.IsChecked = false;
                }
            }

            Int32.TryParse(XmlHelper.m_strAutoFactorySetting, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsAutoSetting.IsChecked = true;
                }
                else
                {
                    chbIsAutoSetting.IsChecked = false;
                }
            }

            Int32.TryParse(XmlHelper.m_strIsUsingTH10SB, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsUseTH10SB.IsChecked = true;
                }
                else
                {
                    chbIsUseTH10SB.IsChecked = false;
                }
            }

            Int32.TryParse(XmlHelper.m_strAutoFactoryCheck, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsAutoCheck.IsChecked = true;
                }
                else
                {
                    chbIsAutoCheck.IsChecked = false;
                }
            }

            Int32.TryParse(XmlHelper.m_strAutoChargeOrDischarge, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsAutoChargeOrDischarge.IsChecked = true;
                }
                else
                {
                    chbIsAutoChargeOrDischarge.IsChecked = false;
                }
            }

            cbTH10SBComNum.SelectedItem = XmlHelper.m_strTH10SBCom;
            cbTH10SBBaudRate.SelectedItem = XmlHelper.m_strTH10SBBaudRate;
            tbTH10SBAddress.Text = XmlHelper.m_strTH10SBAddress;
            tbHardwareVersion.Text = XmlHelper.m_strHardwareVersion;
            tbSoftwareVersion.Text = XmlHelper.m_strSoftwareVersion;
        }

        public virtual void OnRaiseCloseEvent(EventArgs e)
        {
            EventHandler handler = RaiseCloseEvent;

            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbCanIndex.SelectedIndex == -1 || cbCanChannel.SelectedIndex == -1)
                {
                    MessageBox.Show((string)Application.Current.Resources["tePromptText1"], (string)FindResource("tePrompt"), MessageBoxButton.OK, MessageBoxImage.Warning);

                }
                else
                {
                    m_zlgFun.zlgInfo.ConnObject = cbCanType.SelectedItem.ToString();
                    tabSource.Add(m_zlgFun);

                    //保存设置
                    XmlHelper.m_strCanType = cbCanType.SelectedIndex.ToString();
                    XmlHelper.m_strCanIndex = cbCanIndex.SelectedIndex.ToString();
                    XmlHelper.m_strCanChannel = cbCanChannel.SelectedIndex.ToString();
                    XmlHelper.m_strBaudrate = cbBaudRate.SelectedIndex.ToString();
                    XmlHelper.m_strDataRecordInterval = cbRecordInterval.SelectedIndex.ToString();

                    m_zlgFun.zlgInfo.DevIndex = uint.Parse(XmlHelper.m_strCanIndex);
                    m_zlgFun.zlgInfo.DevChannel = uint.Parse(XmlHelper.m_strCanChannel);

                    if (cbProtocol.SelectedIndex == 0)
                    {
                        h5Protocol = H5Protocol.BO_QIANG;
                        XmlHelper.m_strProtocol = "0";
                    }
                    else
                    {
                        h5Protocol = H5Protocol.DI_DI;
                        XmlHelper.m_strProtocol = "1";
                    }

                    XmlHelper.m_strCanFD = cbCANFD.SelectedIndex.ToString();
                    XmlHelper.m_strArbitration = cbArbitrationBaudRate.SelectedIndex.ToString();
                    XmlHelper.m_strDataBaudRate = cbDataBaudRate.SelectedIndex.ToString();
                    XmlHelper.m_strTerminalResistance = chbIsTerminalResistance.IsChecked == true ? "1" : "0";

                    int val;
                    bool ret = int.TryParse(tbCurrentValue.Text.Trim(), out val);
                    if (ret == false)
                    {
                        MessageBox.Show("输入的充放电电流值有误，请重新输入！", "输入提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    XmlHelper.m_strCurrentValue = val.ToString();
                    uint value;
                    ret = uint.TryParse(tbWaitTime.Text.Trim(), out value);
                    if (ret == false)
                    {
                        MessageBox.Show("输入的等待时间有误，请重新输入！", "输入提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    XmlHelper.m_strWaitTime = value.ToString().Trim();
                    ret = uint.TryParse(tbCurrentError.Text.Trim(), out value);
                    if (ret == false)
                    {
                        MessageBox.Show("输入的充放电电流误差有误，请重新输入！", "输入提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    XmlHelper.m_strCurrentError = value.ToString().Trim();
                    XmlHelper.m_strKP182Com = cbKP182ComNum.Items.Count == 0 ? "" : cbKP182ComNum.SelectedItem.ToString();
                    XmlHelper.m_strKP182BaudRate = cbKP182BaudRate.SelectedItem.ToString();
                    //XmlHelper.m_strKP182ParityBit = cbKP182ParityBit.SelectedIndex.ToString();
                    //XmlHelper.m_strKP182DateBit = cbKP182DataBit.SelectedIndex.ToString();
                    //XmlHelper.m_strKP182StopBit = cbKP182StopBit.SelectedIndex.ToString();

                    //判断十六进制数
                    string regexStr = @"([^A-Fa-f0-9]|\s+?)+";
                    string kp182Addr = tbKP182DeviceAddress.Text.Trim();
                    if (IsMatch(kp182Addr, regexStr, "KP184"))
                    {
                        XmlHelper.m_strKP182DeviceAddress = kp182Addr;
                    }
                    else
                    {
                        return;
                    }



                    XmlHelper.m_strIsUsingKP182 = chbIsUseKP182.IsChecked == true ? "1" : "0";

                    int val2;
                    bool ret2 = int.TryParse(tbCurrentValue_2.Text.Trim(), out val2);
                    if (ret2 == false)
                    {
                        MessageBox.Show("输入的充放电电流值有误，请重新输入！", "输入提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    XmlHelper.m_strCurrentValue_2 = val2.ToString();
                    uint value2;
                    ret2 = uint.TryParse(tbWaitTime_2.Text.Trim(), out value2);
                    if (ret2 == false)
                    {
                        MessageBox.Show("输入的等待时间有误，请重新输入！", "输入提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    XmlHelper.m_strWaitTime_2 = value2.ToString().Trim();
                    ret2 = uint.TryParse(tbCurrentError_2.Text.Trim(), out value2);
                    if (ret2 == false)
                    {
                        MessageBox.Show("输入的充放电电流误差有误，请重新输入！", "输入提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                    XmlHelper.m_strCurrentError_2 = value2.ToString().Trim();
                    XmlHelper.m_strKP182Com_2 = cbKP182ComNum_2.Items.Count == 0 ? "" : cbKP182ComNum_2.SelectedItem.ToString();
                    XmlHelper.m_strKP182BaudRate_2 = cbKP182BaudRate_2.SelectedItem.ToString();

                    //判断十六进制数
                    string regexStr2 = @"([^A-Fa-f0-9]|\s+?)+";
                    string kp182Addr2 = tbKP182Address_2.Text.Trim();
                    if (IsMatch(kp182Addr2, regexStr2, "KP184"))
                    {
                        XmlHelper.m_strKP182DeviceAddress_2 = kp182Addr2;
                    }
                    else
                    {
                        return;
                    }



                    XmlHelper.m_strIsUsingKP182_2 = chbIsUseKP182_2.IsChecked == true ? "1" : "0";


                    XmlHelper.m_strIsUsingVoltmeter = chbIsUseVoltmeter.IsChecked == true ? "1" : "0";
                    XmlHelper.m_strVoltmeterCom = cbVoltmeterComNum.Items.Count == 0 ? "" : cbVoltmeterComNum.SelectedItem.ToString();
                    XmlHelper.m_strVoltmeterBaudRate = cbVoltmeterBaudRate.SelectedItem.ToString();
                    string voltmeterAddr = tbVoltmeterAddress.Text.Trim();
                    if (IsMatch(voltmeterAddr, regexStr, "电压表"))
                    {
                        XmlHelper.m_strVoltmeterAddress = voltmeterAddr;
                    }
                    else
                    {
                        return;
                    }

                    XmlHelper.m_strIsUsingAmperemeter = chbIsUseAmperemeter.IsChecked == true ? "1" : "0";
                    XmlHelper.m_strAmperemeterCom = cbAmperemeterComNum.Items.Count == 0 ? "" : cbAmperemeterComNum.SelectedItem.ToString();
                    XmlHelper.m_strAmperemeterBaudRate = cbAmperemeterBaudRate.SelectedItem.ToString();
                    string amperemeterAddr = tbAmperemeterAddress.Text.Trim();
                    if (IsMatch(amperemeterAddr, regexStr, "电流表"))
                    {
                        XmlHelper.m_strAmperematerAddress = amperemeterAddr;
                    }
                    else
                    {
                        return;
                    }
                    XmlHelper.m_strAutoFactorySetting = chbIsAutoSetting.IsChecked == true ? "1" : "0";
                    XmlHelper.m_strIsUsingTH10SB = chbIsUseTH10SB.IsChecked == true ? "1" : "0";
                    XmlHelper.m_strTH10SBCom = cbTH10SBComNum.Items.Count == 0 ? "" : cbTH10SBComNum.SelectedItem.ToString();
                    XmlHelper.m_strTH10SBBaudRate = cbTH10SBBaudRate.SelectedItem.ToString();
                    string th10sbAddr = tbTH10SBAddress.Text.Trim();
                    if (IsMatch(th10sbAddr, regexStr, "温湿度仪"))
                    {
                        XmlHelper.m_strTH10SBAddress = th10sbAddr;
                    }
                    else
                    {
                        return;
                    }
                    XmlHelper.m_strHardwareVersion = tbHardwareVersion.Text;
                    XmlHelper.m_strSoftwareVersion = tbSoftwareVersion.Text;
                    XmlHelper.m_strAutoFactoryCheck = chbIsAutoCheck.IsChecked == true ? "1" : "0";
                    XmlHelper.m_strAutoChargeOrDischarge = chbIsAutoChargeOrDischarge.IsChecked == true ? "1" : "0";
                    XmlHelper.SaveConfigInfo(true);
                    this.Close();

                    OnRaiseCloseEvent(null);

                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        bool IsMatch(string address,string regexStr,string device)
        {
            if (!string.IsNullOrEmpty(address))
            {
                address = address.Replace(" ", "");
                if ((address.Length % 2) != 0)
                    address += " ";
                if (!Regex.IsMatch(address, regexStr))
                {
                    if (address.Length <= 2)
                    {
                        return true;
                    }
                    else
                    {
                        MessageBox.Show(string.Format("请输入一个字节的{0}地址！",device), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return false;
                    }
                }
                else
                {
                    MessageBox.Show(string.Format("输入的{0}地址包含非十六进制数字，请检查！",device), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return false;
                }
            }
            else
            {
                MessageBox.Show(string.Format("请输入{0}的设备地址！",device), "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return false;
            }
        }

        private void Canel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }


        #region CAN 类型
        private void cbCanType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ZLGInfo.DevType = GetCanType(cbCanType.SelectedIndex);
            if(ZLGInfo.DevType == (uint)ZLGType.VCI_USBCANFD_100U)
            {
                cbCANFD.IsEnabled = true;
                cbArbitrationBaudRate.IsEnabled = true;
                cbDataBaudRate.IsEnabled = true;
                chbIsTerminalResistance.IsEnabled = true;
                lbCanFD.IsEnabled = true;
                lbArbitrationBaudRate.IsEnabled = true;
                lbDataBaudRate.IsEnabled = true;
                lbIsTerminalResistance.IsEnabled = true;

                lbBaudRate.IsEnabled = false;
                cbBaudRate.IsEnabled = false;
            }
            else
            {
                cbCANFD.IsEnabled = false;
                cbArbitrationBaudRate.IsEnabled = false;
                cbDataBaudRate.IsEnabled = false;
                chbIsTerminalResistance.IsEnabled = false;
                lbCanFD.IsEnabled = false;
                lbArbitrationBaudRate.IsEnabled = false;
                lbDataBaudRate.IsEnabled = false;
                lbIsTerminalResistance.IsEnabled = false;

                lbBaudRate.IsEnabled = true;
                cbBaudRate.IsEnabled = true;
            }
        }

        public static uint GetCanType(int selectedIndex)
        {
            uint unCanType = 0;
            switch (selectedIndex)
            {
                case 0:
                    unCanType = (uint)ZLGType.VCI_USBCAN_E_U;
                    break;

                case 1:
                    unCanType = (uint)ZLGType.VCI_PCI5010U; ;
                    break;

                case 2:
                    unCanType = (uint)ZLGType.VCI_PCI9810;
                    break;

                case 3:
                    unCanType = (uint)ZLGType.VCI_USBCAN1;
                    break;

                case 4:
                    unCanType = (uint)ZLGType.VCI_PCI5110;
                    break;

                case 5:
                    unCanType = (uint)ZLGType.VCI_CANLITE;
                    break;

                case 6:
                    unCanType = (uint)ZLGType.VCI_PC104CAN;
                    break;

                case 7:
                    unCanType = (uint)ZLGType.VCI_DNP9810;
                    break;

                case 8:
                    unCanType = (uint)ZLGType.VCI_USBCANFD_100U;
                    break;

                case 9:
                    unCanType = (uint)ZLGType.VCI_USBCAN_2E_U;
                    break;

                case 10:
                    unCanType = (uint)ZLGType.VCI_PCI5020U;
                    break;

                case 11:
                    unCanType = (uint)ZLGType.VCI_PCI5121;
                    break;

                case 12:
                    unCanType = (uint)ZLGType.VCI_USBCAN2;
                    break;

                case 13:
                    unCanType = (uint)ZLGType.VCI_USBCAN2A;
                    break;

                case 14:
                    unCanType = (uint)ZLGType.VCI_PCI9820;
                    break;

                case 15:
                    unCanType = (uint)ZLGType.VCI_ISA9620;
                    break;

                case 16:
                    unCanType = (uint)ZLGType.VCI_ISA5420;
                    break;

                case 17:
                    unCanType = (uint)ZLGType.VCI_PC104CAN2;
                    break;

                case 18:
                    unCanType = (uint)ZLGType.VCI_PCI9820I;
                    break;

                case 19:
                    unCanType = (uint)ZLGType.VCI_PEC9920;
                    break;

                case 20:
                    unCanType = (uint)ZLGType.VCI_PCIE9221;
                    break;

                case 21:
                    unCanType = (uint)ZLGType.VCI_PCI9840;
                    break;

                default:
                    break;

            }

            return unCanType;
        }

        #endregion

        #region 波特率
        private void cbBaudRate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ZLGInfo.Baudrate = GetSelectBaudRate(cbBaudRate.SelectedIndex);
            SetBaudRateTimer(ZLGInfo.Baudrate);
        }

        public static BaudRate GetSelectBaudRate(int nInxex)
        {
            BaudRate br = BaudRate._500Kbps;
            switch (nInxex)
            {
                case 0:
                    br = BaudRate._5Kbps;
                    break;

                case 1:
                    br = BaudRate._10Kbps;
                    break;

                case 2:
                    br = BaudRate._20Kbps;
                    break;

                case 3:
                    br = BaudRate._40Kbps;
                    break;

                case 4:
                    br = BaudRate._50Kbps;
                    break;
                case 5:
                    br = BaudRate._80Kbps;
                    break;

                case 6:
                    br = BaudRate._100Kbps;
                    break;

                case 7:
                    br = BaudRate._125Kbps;
                    break;

                case 8:
                    br = BaudRate._200Kbps;
                    break;

                case 9:
                    br = BaudRate._250Kbps;
                    break;

                case 10:
                    br = BaudRate._400Kbps;
                    break;


                case 11:
                    br = BaudRate._500Kbps;
                    break;

                case 12:
                    br = BaudRate._666Kbps;
                    break;

                case 13:
                    br = BaudRate._800Kbps;
                    break;

                case 14:
                    br = BaudRate._1000Kbps;
                    break;
            }
            return br;
        }

        public static ArbitrationBaudRate GetSelectArbitrationBaudRate(int nInxex)
        {
            ArbitrationBaudRate abr = ArbitrationBaudRate._500Kbps;
            switch (nInxex)
            {
                case 0:
                    abr = ArbitrationBaudRate._1Mbps;
                    break;
                case 1:

                    abr = ArbitrationBaudRate._500Kbps;
                    break;

                case 2:
                    abr = ArbitrationBaudRate._500Kbps;
                    break;

                case 3:
                    abr = ArbitrationBaudRate._250Kbps;
                    break;

                case 4:
                    abr = ArbitrationBaudRate._125Kbps;
                    break;
                case 5:
                    abr = ArbitrationBaudRate._100Kbps;
                    break;

                case 6:
                    abr = ArbitrationBaudRate._50Kbps;
                    break;
            }
            return abr;
        }

        public static DataBaudRate GetSelectDataBaudRate(int nInxex)
        {
            DataBaudRate dbr = DataBaudRate._5Mbps;
            switch (nInxex)
            {
                case 0:
                    dbr = DataBaudRate._5Mbps;
                    break;

                case 1:
                    dbr = DataBaudRate._4Mbps;
                    break;

                case 2:
                    dbr = DataBaudRate._2Mbps;
                    break;

                case 3:
                    dbr = DataBaudRate._1Mbps;
                    break;
            }
            return dbr;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="baudRate">波特率</param>
        private void SetBaudRateTimer(BaudRate value)
        {
            switch (value)
            {
                case BaudRate._5Kbps:             
                    SetTimerBasedOnBaudRate(0xBF, 0xFF);
                    break;
                case BaudRate._10Kbps:        
                    SetTimerBasedOnBaudRate(0x31, 0x1C);
                    break;
                case BaudRate._20Kbps:                 
                    SetTimerBasedOnBaudRate(0x18, 0x1C);
                    break;
                case BaudRate._40Kbps:                     
                    SetTimerBasedOnBaudRate(0x87, 0xFF);
                    break;
                case BaudRate._50Kbps:                     
                    SetTimerBasedOnBaudRate(0x09, 0x1C);
                    break;
                case BaudRate._80Kbps:
                    SetTimerBasedOnBaudRate(0x83, 0xFF);
                    break;
                case BaudRate._100Kbps:
                    SetTimerBasedOnBaudRate(0x04, 0x1C);
                    break;
                case BaudRate._125Kbps:
                    SetTimerBasedOnBaudRate(0x03, 0x1C);
                    break;
                case BaudRate._200Kbps:
                    SetTimerBasedOnBaudRate(0x81, 0xFA);
                    break;
                case BaudRate._250Kbps:
                    SetTimerBasedOnBaudRate(0x01, 0x1C);
                    break;
                case BaudRate._400Kbps:
                    SetTimerBasedOnBaudRate(0x80, 0xFA);
                    break;
                case BaudRate._500Kbps:
                    SetTimerBasedOnBaudRate(0x00, 0x1C);
                    break;
                case BaudRate._666Kbps:
                    SetTimerBasedOnBaudRate(0x80, 0xB6);
                    break;
                case BaudRate._800Kbps:
                    SetTimerBasedOnBaudRate(0x00, 0x16);
                    break;
                case BaudRate._1000Kbps:
                    SetTimerBasedOnBaudRate(0x00, 0x14);
                    break;
                default:
                    SetTimerBasedOnBaudRate(0x00, 0x1C);
                    break;

            }
        }

        private void SetTimerBasedOnBaudRate(byte timer0, byte timer1)
        {
            ZLGInfo.Timing0 = timer0;
            ZLGInfo.Timing1 = timer1;
            teTimer0.Text = Convert.ToString(timer0, 16).ToUpper().PadLeft(2,'0');
            teTimer1.Text = Convert.ToString(timer1, 16).ToUpper().PadLeft(2, '0');
        }
        #endregion

        private void chbIsAutoSetting_Click(object sender, RoutedEventArgs e)
        {
            if(chbIsAutoSetting.IsChecked == true)
            {
                chbIsAutoCheck.IsChecked = false;
                chbIsAutoChargeOrDischarge.IsChecked = false;
            }
        }

        private void chbIsAutoCheck_Click(object sender, RoutedEventArgs e)
        {
            if (chbIsAutoCheck.IsChecked == true)
            {
                chbIsAutoSetting.IsChecked = false;
                chbIsAutoChargeOrDischarge.IsChecked = false;
            }
        }

        private void chbIsAutoChargeOrDischarge_Click(object sender, RoutedEventArgs e)
        {
            if (chbIsAutoChargeOrDischarge.IsChecked == true)
            {
                chbIsAutoCheck.IsChecked = false;
                chbIsAutoSetting.IsChecked = false;
            }
        }
    }
}
