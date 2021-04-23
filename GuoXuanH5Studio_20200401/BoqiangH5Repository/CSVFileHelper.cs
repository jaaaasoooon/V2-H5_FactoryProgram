using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using BoqiangH5Entity;

namespace BoqiangH5Repository
{
    public class CSVFileHelper
    {

        /// <summary>
        /// 将数据写入到CSV文件中
        /// </summary>
        /// <param name="listData">提供保存数据的list</param>
        /// <param name="path">CSV的文件路径</param>
        public static void SaveCSV(List<string> listData, string path)
        {
            bool isCreate = false;
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();  
                }

                if (!File.Exists(path))
                {
                    fs = File.Create(path);//创建该文件
                    isCreate = true;
                }
                else
                {
                    fs = new FileStream(path, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }
      
                sw = new StreamWriter(fs, System.Text.Encoding.Default);

                //写出列名称
                if (isCreate)
                {
                    string strColumnsName = "序号,时间,收发,ID,CAN数据类型,协议数据类型,服务类型，数据";
                    sw.WriteLine(strColumnsName);
                }

                //写出各行数据
                if (null != listData)
                {
                    for (int i = 0; i < listData.Count; i++)
                    {
                        sw.WriteLine(listData[i], false);
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (null != sw)
                    sw.Close();
                if (null != fs)
                    fs.Close();
            }

        }

        /// <summary>
        /// 将CSV文件的数据读取到DataTable中
        /// </summary>
        /// <param name="fileName">CSV文件路径</param>
        /// <returns>返回读取了CSV数据的DataTable</returns>
        public static DataTable OpenCSV(string filePath)
        {
            DataTable dt = new DataTable();
            FileStream fs = null;
            StreamReader sr = null;

            try
            {
                Encoding encoding = System.Text.Encoding.Default; // System.Text.Encoding.GetEncoding(936); // System.Data.Common.GetType(filePath); //Encoding.ASCII;//
                
                fs = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read);

                //StreamReader sr = new StreamReader(fs, Encoding.UTF8);
                sr = new StreamReader(fs, encoding);
            
                string strLine = "";
          
                string[] aryLine = null;
                string[] tableHead = null;
    
                int columnCount = 10;
          
                bool IsFirst = true;
     
                while ((strLine = sr.ReadLine()) != null)
                {

                    if (IsFirst == true)
                    {
                        tableHead = strLine.Split(',');
                        IsFirst = false;
                        columnCount = tableHead.Length;
                        //创建列
                        for (int i = 0; i < columnCount; i++)
                        {
                            DataColumn dc = new DataColumn(tableHead[i]);
                            dt.Columns.Add(dc);
                        }
                    }
                    else
                    {
                        aryLine = strLine.Split(',');
                        DataRow dr = dt.NewRow();
                        for (int j = 0; j < columnCount; j++)
                        {
                            dr[j] = aryLine[j];
                        }
                        dt.Rows.Add(dr);
                    }
                }
                if (aryLine != null && aryLine.Length > 0)
                {
                    dt.DefaultView.Sort = tableHead[0] + " " + "asc";
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (null != sr)
                    sr.Close();
                if (null != fs)
                    fs.Close();
            }
            return dt;
        }


        /// <summary>
        /// 读取Excel文件到DataSet中
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static DataSet LoadExcel(string filePath)
        {
            string connStr = "";
            string fileType = System.IO.Path.GetExtension(filePath);
            if (string.IsNullOrEmpty(fileType)) return null;

            if (fileType == ".xls")
                connStr = "Provider=Microsoft.Jet.OLEDB.4.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 8.0;HDR=YES;IMEX=1\"";
            else
                connStr = "Provider=Microsoft.ACE.OLEDB.12.0;" + "Data Source=" + filePath + ";" + ";Extended Properties=\"Excel 12.0;HDR=YES;IMEX=1\"";
            string sql_F = "Select * FROM [{0}]";

            OleDbConnection conn = null;
            OleDbDataAdapter da = null;
            System.Data.DataTable dtSheetName = null;

            DataSet ds = new DataSet();
            try
            {
                conn = new OleDbConnection(connStr);
                conn.Open();
                    
                string SheetName = "";
                dtSheetName = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });

                // 初始化适配器
                da = new OleDbDataAdapter();
                //for (int i = 0; i < dtSheetName.Rows.Count; i++)
                {
                    SheetName = (string)dtSheetName.Rows[0]["TABLE_NAME"];
                    
                    if (SheetName.Contains("$") && !SheetName.Replace("'", "").EndsWith("$"))
                    {
                        //continue;
                    }

                    da.SelectCommand = new OleDbCommand(String.Format(sql_F, SheetName), conn);
                    DataSet dsItem = new DataSet();
                   
                    da.Fill(dsItem);

                    ds.Tables.Add(dsItem.Tables[0].Copy());
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                // 关闭连接
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                    da.Dispose();
                    conn.Dispose();
                }
            }
            return ds;
        }


        /// <summary>
        /// 将BMS数据或单体数据写入到CSV文件中               lipeng   2020.3.26,增加实时信息记录
        /// </summary>
        /// <param name="listData">提供保存数据的list</param>
        /// <param name="path">CSV的文件路径</param>
        public static void SaveBmsORCellCSV(List<H5BmsInfo> listBmsData, string path, List<H5BmsInfo> listCellData)
        {
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                if (File.Exists(path))
                {
                    fs = new FileStream(path, System.IO.FileMode.Append, System.IO.FileAccess.Write);

                    sw = new StreamWriter(fs, System.Text.Encoding.Default);
                    //写出各行数据
                    if (null != listBmsData  && null != listCellData)
                    {
                        sw.WriteLine(string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35},{36},{37},{38},{39},{40},{41},{42},{43},{44},{45},{46},{47}",
                        DateTime.Now.ToString("yyyy年MM月dd日 hh时mm分ss秒"), listBmsData[0].StrValue, listBmsData[1].StrValue, listBmsData[2].StrValue, listBmsData[3].StrValue, listBmsData[4].StrValue,
                        listBmsData[5].StrValue, listBmsData[6].StrValue, listBmsData[7].StrValue, listBmsData[8].StrValue, listBmsData[9].StrValue, listBmsData[10].StrValue, listBmsData[11].StrValue,
                        listBmsData[12].StrValue, listBmsData[13].StrValue, listBmsData[14].StrValue, listBmsData[15].StrValue, listBmsData[16].StrValue, listBmsData[17].StrValue, listBmsData[18].StrValue,
                        listBmsData[19].StrValue, listBmsData[20].StrValue, listBmsData[21].StrValue, listBmsData[22].StrValue, listBmsData[23].StrValue, listBmsData[24].StrValue,
                        listBmsData[25].StrValue, listBmsData[26].StrValue, listBmsData[27].StrValue, listBmsData[28].StrValue, listCellData[0].StrValue, listCellData[1].StrValue, listCellData[2].StrValue,
                        listCellData[3].StrValue, listCellData[4].StrValue, listCellData[5].StrValue, listCellData[6].StrValue, listCellData[7].StrValue, listCellData[8].StrValue, listCellData[9].StrValue,
                        listCellData[10].StrValue, listCellData[11].StrValue, listCellData[12].StrValue, listCellData[13].StrValue, listCellData[14].StrValue, listCellData[15].StrValue, listCellData[16].StrValue, listCellData[17].StrValue));
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (null != sw)
                    sw.Close();
                if (null != fs)
                    fs.Close();
            }

        }

        /// <summary>
        /// 写BMS数据或单体数据CSV文件标题             lipeng   2020.3.26,增加实时信息记录
        /// </summary>
        /// <param name="listData">提供保存数据的list</param>
        /// <param name="path">CSV的文件路径</param>
        public static void SaveBmsORCellCSVTitle(string path,bool isBqProtocol,List<H5BmsInfo> listBMS, List<H5BmsInfo> listCell, List<H5BmsInfo> listDevice)
        {
            StreamWriter sw = null;
            FileStream fs = null;
            try
            {
                if (File.Exists(path))
                {
                    fs = new FileStream(path, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                    sw = new StreamWriter(fs, System.Text.Encoding.Default);

                    if(isBqProtocol == true)
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("测试时间,");
                        foreach (var item in listBMS)
                        {
                            sb.Append(string.Format("{0}({1})",item.Description,item.Unit));
                            sb.Append(",");
                        }
                        sb.Append("总电压(mV)");
                        sb.Append(",");
                        sb.Append("实时电流(mA)");
                        sb.Append(",");
                        foreach (var item in listCell)
                        {
                            if(item.Description == "总电压" || item.Description == "实时电流")
                            {
                                continue;
                            }
                            sb.Append(string.Format("{0}({1})", item.Description, item.Unit));
                            sb.Append(",");
                        }
                        sw.WriteLine(sb.ToString());
                    }
                    else
                    {
                        StringBuilder sb = new StringBuilder();
                        sb.Append("测试时间,");
                        foreach (var item in listBMS)
                        {
                            sb.Append(string.Format("{0}({1})", item.Description, item.Unit));
                            sb.Append(",");
                        }
                        foreach (var item in listCell)
                        {
                            sb.Append(string.Format("{0}({1})", item.Description, item.Unit));
                            sb.Append(",");
                        }
                        sw.WriteLine(sb.ToString());
                    }
                }

            }
            catch (Exception ex)
            { }
            finally
            {
                if (null != sw)
                    sw.Close();
                if (null != fs)
                    fs.Close();
            }

        }

        public static void SaveRecordDataCSV(System.Collections.ObjectModel.ObservableCollection<H5RecordInfo> listData, string path)
        {
            bool isCreate = false;
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }

                if (!File.Exists(path))
                {
                    fs = File.Create(path);//创建该文件
                    isCreate = true;
                }
                else
                {
                    fs = new FileStream(path, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }

                sw = new StreamWriter(fs, System.Text.Encoding.Default);

                //写出列名称
                if (isCreate)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("序号,");
                    sb.Append("时间,");
                    sb.Append("记录类型,");
                    sb.Append("Pack状态,");
                    sb.Append("电池状态,");
                    sb.Append("满充容量(mAh),");
                    sb.Append("剩余容量(mAh),");
                    sb.Append("SOC(%),");
                    sb.Append("Cell1(mV),");
                    sb.Append("Cell2(mV),");
                    sb.Append("Cell3(mV),");
                    sb.Append("Cell4(mV),");
                    sb.Append("Cell5(mV),");
                    sb.Append("Cell6(mV),");
                    sb.Append("Cell7(mV),");
                    sb.Append("Cell8(mV),");
                    sb.Append("Cell9(mV),");
                    sb.Append("Cell10(mV),");
                    sb.Append("Cell11(mV),");
                    sb.Append("Cell12(mV),");
                    sb.Append("Cell13(mV),");
                    sb.Append("Cell14(mV),");
                    sb.Append("Cell15(mV),");
                    sb.Append("Cell16(mV),");
                    sb.Append("总压(V),");
                    sb.Append("电流(mA),");
                    sb.Append("环境温度(℃),");
                    sb.Append("温度保留1(℃),");
                    sb.Append("温度保留2(℃),");
                    sb.Append("电芯温度1(℃),");
                    sb.Append("电芯温度2(℃),");
                    sb.Append("电芯温度3(℃),");
                    sb.Append("电芯温度4(℃),");
                    sb.Append("湿度(%),");
                    sb.Append("功率温度(℃)");
                    sw.WriteLine(sb.ToString());
                }

                //写出各行数据
                if (null != listData)
                {
                    foreach(var item in listData)
                    {
                        string strLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34}",
                            item.Index, item.RecordTime, item.RecordType, item.PackStatus, item.BatteryStatus, item.FCC, item.RC, item.SOC, item.Cell1Voltage, item.Cell2Voltage,
                            item.Cell3Voltage, item.Cell4Voltage, item.Cell5Voltage, item.Cell6Voltage, item.Cell7Voltage, item.Cell8Voltage, item.Cell9Voltage, item.Cell10Voltage, 
                            item.Cell11Voltage, item.Cell12Voltage, item.Cell13Voltage, item.Cell14Voltage, item.Cell15Voltage, item.Cell16Voltage, item.TotalVoltage, item.Current, 
                            item.AmbientTemp, item.Cell1Temp, item.Cell2Temp,item.Cell3Temp, item.Cell4Temp,item.Cell5Temp,item.Cell6Temp,item.Cell7Temp,item.PowerTemp);
                        sw.WriteLine(strLine);
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (null != sw)
                    sw.Close();
                if (null != fs)
                    fs.Close();
            }

        }

        public static void SaveDdRecordDataCSV(System.Collections.ObjectModel.ObservableCollection<H5DidiRecordInfo> listData, string path,bool isReadAll,string uid,List<string> list,string mcuMsg)
        {
            bool isCreate = false;
            FileStream fs = null;
            StreamWriter sw = null;

            try
            {
                FileInfo fi = new FileInfo(path);
                if (!fi.Directory.Exists)
                {
                    fi.Directory.Create();
                }

                if (!File.Exists(path))
                {
                    fs = File.Create(path);//创建该文件
                    isCreate = true;
                }
                else
                {
                    fs = new FileStream(path, System.IO.FileMode.Append, System.IO.FileAccess.Write);
                }

                sw = new StreamWriter(fs, System.Text.Encoding.Default);

                sw.WriteLine(string.Format("UID：{0}", uid));
                sw.WriteLine(string.Format("设备类型：{0}", list[0]));
                sw.WriteLine(string.Format("制造厂信息：{0}", list[3]));
                sw.WriteLine(string.Format("设备SN号：{0}", list[4]));
                sw.WriteLine(string.Format("硬件型号编号.客户型号编号：{0}", list[5]));
                sw.WriteLine(string.Format("固件版本号：{0}", list[6]));
                sw.WriteLine(string.Format("硬件版本号：{0}", list[7]));
                sw.WriteLine(string.Format("当前程序状态：{0}", list[8]));
                string[] msgs = mcuMsg.Split('$');
                if(msgs.Count() < 3)
                {
                    sw.WriteLine(string.Format("软件版本号：{0}", mcuMsg));
                    sw.WriteLine(string.Format("硬件版本号：{0}", mcuMsg));
                    sw.WriteLine(string.Format("生产日期：{0}", mcuMsg));
                }
                else
                {
                    sw.WriteLine(string.Format("软件版本号：{0}", msgs[0]));
                    sw.WriteLine(string.Format("硬件版本号：{0}", msgs[1]));
                    sw.WriteLine(string.Format("生产日期：{0}", msgs[2]));
                }
                sw.WriteLine(string.Empty);
                //写出列名称
                if (isCreate)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.Append("序号,");
                    sb.Append("时间,");
                    if(isReadAll)
                    {
                        sb.Append("记录类型(BQ),");
                        sb.Append("Pack状态(BQ),");
                        sb.Append("电池状态(BQ),");
                    }
                    sb.Append("总压(mV),");
                    sb.Append("电流(mA),");
                    sb.Append("SOC(%),");
                    sb.Append("电池组状态,");
                    sb.Append("充电MOS状态,");
                    sb.Append("放电MOS状态,");
                    sb.Append("DET状态,");
                    //sb.Append("剩余容量(mAh),");
                    //sb.Append("满充容量(mAh),");
                    sb.Append("循环次数,");
                    sb.Append("均衡,");
                    sb.Append("电芯温度1(℃),");
                    sb.Append("电芯温度2(℃),");
                    sb.Append("MOS温度(℃),");
                    sb.Append("电芯温度3(℃),");
                    sb.Append("电芯温度4(℃),");
                    sb.Append("Cell1(mV),");
                    sb.Append("Cell2(mV),");
                    sb.Append("Cell3(mV),");
                    sb.Append("Cell4(mV),");
                    sb.Append("Cell5(mV),");
                    sb.Append("Cell6(mV),");
                    sb.Append("Cell7(mV),");
                    sb.Append("Cell8(mV),");
                    sb.Append("Cell9(mV),");
                    sb.Append("Cell10(mV),");
                    sb.Append("Cell11(mV),");
                    sb.Append("Cell12(mV),");
                    sb.Append("Cell13(mV),");
                    sb.Append("Cell14(mV),");
                    sb.Append("Cell15(mV),");
                    sb.Append("Cell16(mV),");
                    sb.Append("湿度(%)");
                    sw.WriteLine(sb.ToString());
                }

                //写出各行数据
                if (null != listData)
                {
                    foreach (var item in listData)
                    {
                        string strLine = string.Empty;
                        if(isReadAll)
                        {
                            strLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32},{33},{34},{35}",
                                        item.Index, item.RecordTime,item.RecordType,item.PackStatus,item.BatStatus, item.TotalVoltage, item.Current, item.SOC, item.BatteryStatus, item.ChargeMOSStatus, item.DischargeMOSStatus, item.DetStatus, 
                                        item.LoopNumber, item.Balance, item.Cell1Voltage, item.Cell2Voltage, item.Cell3Voltage, item.Cell4Voltage, item.Cell5Voltage, item.Cell6Voltage, item.Cell7Voltage, item.Cell8Voltage, item.Cell9Voltage,
                                        item.Cell10Voltage, item.Cell11Voltage, item.Cell12Voltage, item.Cell13Voltage, item.Cell14Voltage, item.Cell15Voltage, item.Cell16Voltage,
                                        item.Cell1Temp, item.Cell2Temp, item.Cell3Temp, item.Cell4Temp, item.Cell5Temp, item.Humidity);
                        }
                        else
                        {
                            strLine = string.Format("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21},{22},{23},{24},{25},{26},{27},{28},{29},{30},{31},{32}",
                                        item.Index, item.RecordTime, item.TotalVoltage, item.Current, item.SOC, item.BatteryStatus, item.ChargeMOSStatus, item.DischargeMOSStatus, item.DetStatus, /*item.RC, item.FCC,*/ item.LoopNumber, item.Balance,
                                        item.Cell1Voltage, item.Cell2Voltage, item.Cell3Voltage, item.Cell4Voltage, item.Cell5Voltage, item.Cell6Voltage, item.Cell7Voltage, item.Cell8Voltage, item.Cell9Voltage,
                                        item.Cell10Voltage, item.Cell11Voltage, item.Cell12Voltage, item.Cell13Voltage, item.Cell14Voltage, item.Cell15Voltage, item.Cell16Voltage,
                                        item.Cell1Temp, item.Cell2Temp, item.Cell3Temp, item.Cell4Temp, item.Cell5Temp, item.Humidity);
                        }
                        sw.WriteLine(strLine);
                    }
                }
            }
            catch (Exception ex)
            { }
            finally
            {
                if (null != sw)
                    sw.Close();
                if (null != fs)
                    fs.Close();
            }

        }

        public static void WriteLogs(string fileName, string type, string content)
        {
            string logPath = AppDomain.CurrentDomain.BaseDirectory;
            if (!string.IsNullOrEmpty(logPath))
            {
                logPath = AppDomain.CurrentDomain.BaseDirectory + fileName;
                if (!Directory.Exists(logPath))
                {
                    Directory.CreateDirectory(logPath);
                }
                logPath = logPath + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                if (!File.Exists(logPath))
                {
                    FileStream fs = File.Create(logPath);
                    fs.Close();
                }
                if (File.Exists(logPath))
                {
                    FileInfo info = new FileInfo(logPath);
                    if(info.Length > 3 * 1024 * 1024)
                    {
                        logPath = AppDomain.CurrentDomain.BaseDirectory + "log" + "\\" + DateTime.Now.ToString("yyyyMMdd") + ".txt";
                        if(!File.Exists(logPath))
                        {
                            FileStream fs = File.Create(logPath);
                            fs.Close();
                        }
                    }
                    StreamWriter sw = new StreamWriter(logPath, true, System.Text.Encoding.Default);
                    sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff") + "-->" + type + "-->" + content);
                    sw.Close();
                }
            }
        }
        public static string ToHexStrFromByte(byte[] byteDatas, bool isSpacing)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < byteDatas.Length; i++)
            {
                if (isSpacing)
                {
                    builder.Append(string.Format("{0:X2}", byteDatas[i]));
                }
                else
                {
                    builder.Append(string.Format("{0:X2} ", byteDatas[i]));
                }
            }
            return builder.ToString().Trim();
        }
    }
}
