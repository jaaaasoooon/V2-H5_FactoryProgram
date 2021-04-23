using System;
using System.Windows;
using System.Windows.Controls;
//using DevExpress.Xpf.Core;
using System.Collections.ObjectModel;
using BoqiangH5Entity;
using BoqiangH5Repository;
using BoqiangH5.ISO15765;
using System.IO;
using Microsoft.Win32;
using System.Text.RegularExpressions;

namespace BoqiangH5
{
    public partial class OneClickFactorySetting : Window
    {
        static int synTimeSpan = 0;
        public static int m_SynTimeSpan
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strSynTimeSpan, out val);
                if (val != -1)
                {
                    synTimeSpan = val;
                }
                return synTimeSpan;
            }
        }
        static string componentModel = string.Empty;
        public static string m_ComponentModel
        {
            get
            {
                return XmlHelper.m_strComponentModel;
            }
        }
        static int voltageBase = 0;
        public static int m_VoltageBase
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strVoltageBase, out val);
                if (val != -1)
                {
                    voltageBase = val;
                }
                return voltageBase;
            }
        }
        static int voltageError = 0;
        public static int m_VoltageError
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strVoltageError, out val);
                if (val != -1)
                {
                    voltageError = val;
                }
                return voltageError;
            }
        }

        static int humidityError = 0;
        public static int m_HumidityError
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strHumidityError, out val);
                if (val != -1)
                {
                    humidityError = val;
                }
                return humidityError;
            }
        }
        static int temperatureError = 0;
        public static int m_TemperatureError
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strTemperatureError, out val);
                if (val != -1)
                {
                    temperatureError = val;
                }
                return temperatureError;
            }
        }
        static int socAdjust = 0;
        public static int m_SOCAdjust
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strSOCAdjust, out val);
                if (val != -1)
                {
                    socAdjust = val;
                }
                return socAdjust;
            }
        }
        static int socValue = 0;
        public static int m_SOCValue
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strSOCValue, out val);
                if (val != -1)
                {
                    socValue = val;
                }
                return socValue;
            }
        }
        static int zeroAdjust = 0;
        public static int m_ZeroAdjust
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strZeroCurrentAdjust, out val);
                if (val != -1)
                {
                    zeroAdjust = val;
                }
                return zeroAdjust;
            }
        }
        static int current10AAdjust = 0;
        public static int m_Current10AAdjust
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_str10AAdjust, out val);
                if (val != -1)
                {
                    current10AAdjust = val;
                }
                return current10AAdjust;
            }
        }
        static int writeEeprom = 0;
        public static int m_WriteEeprom
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strWriteEeprom, out val);
                if (val != -1)
                {
                    writeEeprom = val;
                }
                return writeEeprom;
            }
        }
        static string eepromFilePath = string.Empty;
        public static string m_EepromFilePath
        {
            get
            {
                return XmlHelper.m_strEepromFilePath;
            }
        }
        static int writeMCU = 0;
        public static int m_WriteMCU
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strWriteMCU, out val);
                if (val != -1)
                {
                    writeMCU = val;
                }
                return writeMCU;
            }
        }
        static string mcuFilePath = string.Empty;
        public static string m_MCUFilePath
        {
            get
            {
                return XmlHelper.m_strMCUFilePath;
            }
        }

        static int rtcAdjust = 0;
        public static int m_RTCAdjust
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strRTCAdjust, out val);
                if (val != -1)
                {
                    rtcAdjust = val;
                }
                return rtcAdjust;
            }
        }
        static int voltageCheck = 0;
        public static int m_VoltageCheck
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCheckVoltageDiff, out val);
                if (val != -1)
                {
                    voltageCheck = val;
                }
                return voltageCheck;
            }
        }

        static int temperatureCheck = 0;
        public static int m_TemperatureCheck
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCheckTemperatureDiff, out val);
                if (val != -1)
                {
                    temperatureCheck = val;
                }
                return temperatureCheck;
            }
        }

        static int humidityCheck = 0;
        public static int m_HumidityCheck
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCheckHumidity, out val);
                if (val != -1)
                {
                    humidityCheck = val;
                }
                return humidityCheck;
            }
        }

        static int eepromCheck = 0;
        public static int m_EepromCheck
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCheckEepromParam, out val);
                if (val != -1)
                {
                    eepromCheck = val;
                }
                return eepromCheck;
            }
        }

        static int mcuCheck = 0;
        public static int m_McuCheck
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strCheckMCUParam, out val);
                if (val != -1)
                {
                    mcuCheck = val;
                }
                return mcuCheck;
            }
        }
        static int producedDateError = 0;
        public static int m_ProducedDateError
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strProducedDateError, out val);
                if (val != -1)
                {
                    producedDateError = val;
                }
                return producedDateError;
            }
        }

        static int shallowSleep = 0;
        public static int m_ShallowSleep
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strShallowSleep, out val);
                if (val != -1)
                {
                    shallowSleep = val;
                }
                return shallowSleep;
            }
        }

        static int deepSleep = 0;
        public static int m_DeepSleep
        {
            get
            {
                int val = 0;
                Int32.TryParse(XmlHelper.m_strDeepSleep, out val);
                if (val != -1)
                {
                    deepSleep = val;
                }
                return deepSleep;
            }
        }

        static float minWaterResistance = 0;
        public static float m_minWaterResistance
        {
            get
            {
                float val = 0;
                float.TryParse(XmlHelper.m_strMinWaterResistance, out val);
                if (val != -1)
                {
                    minWaterResistance = val;
                }
                return minWaterResistance;
            }
        }
        static float maxWaterResistance = 0;
        public static float m_maxWaterResistance
        {
            get
            {
                float val = 0;
                float.TryParse(XmlHelper.m_strMaxWaterResistance, out val);
                if (val != -1)
                {
                    maxWaterResistance = val;
                }
                return maxWaterResistance;
            }
        }
        public OneClickFactorySetting()        
        {

            InitializeComponent();

            InitConfigInfo();
        }

        private void InitConfigInfo()
        {
            XmlHelper.LoadConfigInfo(false);
            int nIndex = -1;

            Int32.TryParse(XmlHelper.m_strSynTimeSpan, out nIndex);
            if (nIndex != -1)
            {
                tbSynTimeSpan.Text = nIndex.ToString();
            }

            tbComponentModel.Text = XmlHelper.m_strComponentModel;

            Int32.TryParse(XmlHelper.m_strVoltageBase, out nIndex);
            if (nIndex != -1)
            {
                tbVoltageBase.Text = nIndex.ToString();
            }
            Int32.TryParse(XmlHelper.m_strVoltageError, out nIndex);
            if (nIndex != -1)
            {
                tbVoltageError.Text = nIndex.ToString();
            }
            Int32.TryParse(XmlHelper.m_strHumidityError, out nIndex);
            if (nIndex != -1)
            {
                tbHumidityError.Text = nIndex.ToString();
            }
            Int32.TryParse(XmlHelper.m_strTemperatureError, out nIndex);
            if (nIndex != -1)
            {
                tbTemperatureError.Text = nIndex.ToString();
            }
            double val;
            double.TryParse(XmlHelper.m_strPowerCurrentBase, out val);
            if (val != -1)
            {
                tbPowerCurrentBase.Text = val.ToString();
            }
            Int32.TryParse(XmlHelper.m_strPowerCurrentError, out nIndex);
            if (nIndex != -1)
            {
                tbPowerCurrentError.Text = nIndex.ToString();
            }

            double.TryParse(XmlHelper.m_strResistanceVoltageBase, out val);
            if (val != -1)
            {
                tbResistanceVoltageBase.Text = val.ToString();
            }
            Int32.TryParse(XmlHelper.m_strResistanceVoltageError, out nIndex);
            if (nIndex != -1)
            {
                tbResistanceVoltageError.Text = nIndex.ToString();
            }

            Int32.TryParse(XmlHelper.m_strSOCAdjust, out nIndex);
            if (nIndex != -1)
            {
                if(nIndex == 1)
                {
                    chbIsAdjustSOC.IsChecked = true;
                }
                else
                {
                    chbIsAdjustSOC.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strSOCValue, out nIndex);
            if (nIndex != -1)
            {
                tbSocAdjustValue.Text = nIndex.ToString();
            }
            Int32.TryParse(XmlHelper.m_strZeroCurrentAdjust, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsZeroAdjust.IsChecked = true;
                }
                else
                {
                    chbIsZeroAdjust.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_str10AAdjust, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIs10AAdjust.IsChecked = true;
                }
                else
                {
                    chbIs10AAdjust.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strWriteEeprom, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsEepromWrite.IsChecked = true;
                }
                else
                {
                    chbIsEepromWrite.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strWriteMCU, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsMCUWrite.IsChecked = true;
                }
                else
                {
                    chbIsMCUWrite.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strRTCAdjust, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsRTCAdjust.IsChecked = true;
                }
                else
                {
                    chbIsRTCAdjust.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strCheckVoltageDiff, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsCheckVoltage.IsChecked = true;
                }
                else
                {
                    chbIsCheckVoltage.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strCheckTemperatureDiff, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsCheckTemperature.IsChecked = true;
                }
                else
                {
                    chbIsCheckTemperature.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strCheckEepromParam, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsCheckEeprom.IsChecked = true;
                }
                else
                {
                    chbIsCheckEeprom.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strCheckHumidity, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsCheckHumidity.IsChecked = true;
                }
                else
                {
                    chbIsCheckHumidity.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strCheckMCUParam, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsCheckMcu.IsChecked = true;
                }
                else
                {
                    chbIsCheckMcu.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strShallowSleep, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsShallowSleep.IsChecked = true;
                }
                else
                {
                    chbIsShallowSleep.IsChecked = false;
                }
            }
            Int32.TryParse(XmlHelper.m_strDeepSleep, out nIndex);
            if (nIndex != -1)
            {
                if (nIndex == 1)
                {
                    chbIsDeepSleep.IsChecked = true;
                }
                else
                {
                    chbIsDeepSleep.IsChecked = false;
                }
            }
            tbEepromFile.Text = XmlHelper.m_strEepromFilePath;
            eepromFilePath = XmlHelper.m_strEepromFilePath;
            tbMCUFile.Text = XmlHelper.m_strMCUFilePath;
            mcuFilePath = XmlHelper.m_strMCUFilePath;
            Int32.TryParse(XmlHelper.m_strProducedDateError, out nIndex);
            if (nIndex != -1)
            {
                tbProducedDateError.Text = nIndex.ToString();
            }
            tbMinResistance.Text = XmlHelper.m_strMinWaterResistance;
            tbMaxResistance.Text = XmlHelper.m_strMaxWaterResistance;
        }
        private void EepromFileSelect_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "程序文件(*.dat)|*.dat|所有文件(*.*)|*.*";
            ofd.FileName = System.Windows.Forms.Application.StartupPath + "\\ProtocolFiles\\H5_EEPROM_config.dat";
            bool? result = ofd.ShowDialog();
            if (result != true)
                return;
            eepromFilePath = ofd.FileName;
            tbEepromFile.Text = eepromFilePath;
        }
        private void McuFileSelect_Clicked(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "程序文件(*.txt)|*.txt|所有文件(*.*)|*.*";
            ofd.FileName = System.Windows.Forms.Application.StartupPath + "\\ProtocolFiles\\H5_EEPROM_config.dat";
            bool? result = ofd.ShowDialog();
            if (result != true)
                return;
            mcuFilePath = ofd.FileName;
            tbMCUFile.Text = mcuFilePath;
        }
        private void SelectAll_Clicked(object sender, RoutedEventArgs e)
        {
            chbIs10AAdjust.IsChecked = true;
            chbIsAdjustSOC.IsChecked = true;
            chbIsEepromWrite.IsChecked = true;
            chbIsMCUWrite.IsChecked = true;
            chbIsZeroAdjust.IsChecked = true;
            chbIsRTCAdjust.IsChecked = true;
            chbIsCheckVoltage.IsChecked = true;
            chbIsCheckTemperature.IsChecked = true;
            chbIsCheckHumidity.IsChecked = true;
            chbIsCheckEeprom.IsChecked = true;
            chbIsCheckMcu.IsChecked = true;
        }
        private void OK_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string str = @"^[0-9]{1,3}$";
                if (!Regex.IsMatch(tbSocAdjustValue.Text, str))
                {
                    MessageBox.Show("请输入正确的 SOC 值！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                byte socVal = byte.Parse(tbSocAdjustValue.Text);

                if (socVal < 0 || socVal > 100)
                {
                    MessageBox.Show("请输入正确的 SOC 值！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (chbIsEepromWrite.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(tbEepromFile.Text.Trim()))
                    {
                        MessageBox.Show("请选择要写入的Eeprom文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                if (chbIsMCUWrite.IsChecked == true)
                {
                    if (string.IsNullOrEmpty(tbMCUFile.Text.Trim()))
                    {
                        MessageBox.Show("请选择要写入的MCU文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                        return;
                    }
                }
                if (chbIsShallowSleep.IsChecked == true && chbIsDeepSleep.IsChecked == true)
                {
                    MessageBox.Show("浅休眠和深休眠不可同时选择！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                XmlHelper.m_strSynTimeSpan = tbSynTimeSpan.Text.Trim();
                XmlHelper.m_strComponentModel = tbComponentModel.Text.Trim();
                XmlHelper.m_strVoltageBase = tbVoltageBase.Text.Trim();
                XmlHelper.m_strVoltageError = tbVoltageError.Text.Trim();
                XmlHelper.m_strHumidityError = tbHumidityError.Text.Trim();
                XmlHelper.m_strTemperatureError = tbTemperatureError.Text.Trim();
                XmlHelper.m_strPowerCurrentBase = tbPowerCurrentBase.Text.Trim();
                XmlHelper.m_strPowerCurrentError = tbPowerCurrentError.Text.Trim();
                XmlHelper.m_strResistanceVoltageBase = tbResistanceVoltageBase.Text.Trim();
                XmlHelper.m_strResistanceVoltageError = tbResistanceVoltageError.Text.Trim();
                XmlHelper.m_strSOCAdjust = chbIsAdjustSOC.IsChecked == true ? "1" : "0";
                XmlHelper.m_strSOCValue = tbSocAdjustValue.Text.Trim();
                XmlHelper.m_strZeroCurrentAdjust = chbIsZeroAdjust.IsChecked == true ? "1" : "0";
                XmlHelper.m_str10AAdjust = chbIs10AAdjust.IsChecked == true ? "1" : "0";
                XmlHelper.m_strWriteEeprom = chbIsEepromWrite.IsChecked == true ? "1" : "0";
                XmlHelper.m_strEepromFilePath = eepromFilePath;
                XmlHelper.m_strWriteMCU = chbIsMCUWrite.IsChecked == true ? "1" : "0";
                XmlHelper.m_strMCUFilePath = mcuFilePath;
                XmlHelper.m_strRTCAdjust = chbIsRTCAdjust.IsChecked == true ? "1" : "0";
                XmlHelper.m_strCheckVoltageDiff = chbIsCheckVoltage.IsChecked == true ? "1" : "0";
                XmlHelper.m_strCheckTemperatureDiff = chbIsCheckTemperature.IsChecked == true ? "1" : "0";
                XmlHelper.m_strCheckHumidity = chbIsCheckHumidity.IsChecked == true ? "1" : "0";
                XmlHelper.m_strCheckEepromParam = chbIsCheckEeprom.IsChecked == true ? "1" : "0";
                XmlHelper.m_strCheckMCUParam = chbIsCheckMcu.IsChecked == true ? "1" : "0";
                XmlHelper.m_strProducedDateError = tbProducedDateError.Text.Trim();
                XmlHelper.m_strShallowSleep = chbIsShallowSleep.IsChecked == true ? "1" : "0";
                XmlHelper.m_strDeepSleep = chbIsDeepSleep.IsChecked == true ? "1" : "0";
                float nIndex = 0;
                ;
                if (float.TryParse(tbMinResistance.Text.Trim(), out nIndex))
                {
                    XmlHelper.m_strMinWaterResistance = tbMinResistance.Text.Trim();
                }
                else
                {
                    MessageBox.Show("进水阻抗最小值设置不正确，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                if (float.TryParse(tbMaxResistance.Text.Trim(), out nIndex))
                {
                    XmlHelper.m_strMaxWaterResistance = tbMaxResistance.Text.Trim();
                }
                else
                {
                    MessageBox.Show("进水阻抗最大值设置不正确，请检查！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
                XmlHelper.SaveConfigInfo(false);
                this.Close();
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message, "提示", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Canel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        //private void chbIsShallowSleep_Clicked(object sender, RoutedEventArgs e)
        //{
        //    if(chbIsShallowSleep.IsChecked == true)
        //    {
        //        chbIsShallowSleep.IsChecked = false;
        //        chbIsDeepSleep.IsChecked = true;
        //    }
        //    else
        //    {
        //        chbIsShallowSleep.IsChecked = true;
        //        chbIsDeepSleep.IsChecked = false;
        //    }
        //}

        //private void chbIsDeepSleep_Clicked(object sender, RoutedEventArgs e)
        //{
        //    if (chbIsDeepSleep.IsChecked == true)
        //    {
        //        chbIsShallowSleep.IsChecked = true;
        //        chbIsDeepSleep.IsChecked = false;
        //    }
        //    else
        //    {
        //        chbIsShallowSleep.IsChecked = false;
        //        chbIsDeepSleep.IsChecked = true;
        //    }
        //}
    }
}
