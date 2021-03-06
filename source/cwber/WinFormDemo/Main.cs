﻿using com.zy.entity.table;
using per.cz.bean;
using per.cz.frame.bridge;
using per.cz.util;
using Sashulin;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using UHFReaderModule;
using WinFormDemo.per.cz.bean;
using WinFormDemo.per.cz.util;

namespace WinFormDemo
{
    public partial class Main : Form
    {

        CDictionary<string, object> appSettings = new CDictionary<string, object>();
        CDictionary<string, object> runtimeSettings = new CDictionary<string, object>();
        Reader reader = new Reader();
        List<TagInfo> tags = new List<TagInfo>();
        byte[] antList = new byte[4];
        private int fCmdRet = 30;
        [DllImport("User32.dll", EntryPoint = "FindWindow")]
        private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        //  private byte fComAdr = 0xff;
        public Main()
        {
            //fComAdr:0,Qvalue:132,Session:0,
            //MaskMem:0,MaskAdr:System.Byte[],
            //MaskLen:0,MaskData:System.Byte[],
            //ReadMem:2, ReadAdr:System.Byte[], 
            //ReadLen:4, Psd:System.Byte[], 
            //Target:0, InAnt:131, Scantime:20, FastFlag:1
            InitializeComponent();
            loadAppSettings();
            initRuntimeSettings();
            int ants = appSettings.GetInt("ants");
            for (int i = 0; i < 4; i++)
            {
                antList[i] = (byte)(ants % 10);
                ants /= 10;
            }
            //appSettings["fComAdr"] = 0xff;
            ////appSettings["qValue"] = 132;
            //appSettings["qValue"] = 0x82;
            //appSettings["session"] = 0x00;
            //appSettings["readMem"] = 2;
            //appSettings["readAdr"] =2;//*
            //appSettings["readLen"] = 4;
            ////appSettings["psd"] = HexHelper.HexStringToByteArray("00000000");
            //appSettings["psd"] =0;//*
            //appSettings["target"] = 0;
            //appSettings["inAnt"] = 0x83;
            //appSettings["scanTime"] = 20;
            //appSettings["fastFlag"] = 1;
            //appSettings["tIDFlag"] = 1;
        }
        private void Main_Load(object sender, EventArgs e)
        {
            this.Click += new System.EventHandler(this.click);
            chromeWebBrowser.SetChromeWebBrowserBridge("chrome_web_browser_bridge", new ChromeWebBrowserBridge(this));

        }
        public void loadAppSettings()
        {
            AppSettingsReader settingsReader = new AppSettingsReader();
            appSettings["fComAdr"] = settingsReader.GetValue("fComAdr", typeof(String));
            appSettings["qValue"] = settingsReader.GetValue("qValue", typeof(String));
            appSettings["session"] = settingsReader.GetValue("session", typeof(String));
            appSettings["readMem"] = settingsReader.GetValue("readMem", typeof(String));
            appSettings["readAdr"] = settingsReader.GetValue("readAdr", typeof(String));
            appSettings["readLen"] = settingsReader.GetValue("readLen", typeof(String));
            appSettings["psd"] = settingsReader.GetValue("psd", typeof(String));
            appSettings["maskFlag"] = settingsReader.GetValue("maskFlag", typeof(String));
            appSettings["target"] = settingsReader.GetValue("target", typeof(String));
            appSettings["ants"] = settingsReader.GetValue("ants", typeof(String));
            appSettings["scanTime"] = settingsReader.GetValue("scanTime", typeof(String));
            appSettings["fastFlag"] = settingsReader.GetValue("fastFlag", typeof(String));
            appSettings["qValue"] = settingsReader.GetValue("qValue", typeof(String));
            appSettings["qValue"] = settingsReader.GetValue("qValue", typeof(String));
        }
        public void initRuntimeSettings()
        {
            runtimeSettings["rwdIsConnect"] = false;
        }
        private void click(object sender, EventArgs e)
        {

        }
        public string test()
        {
            Card c = new Card();
            c.epc = "xxxxxxxx";
            test2();
            return JsonUtils.ToJson(c);
        }
        public void test2()
        {

            ThreadStart thr_start_func = new ThreadStart(First_Thread);
            Console.WriteLine("Creating the first thread ");
            Thread fThread = new Thread(thr_start_func);
            fThread.Name = "first_thread";
            fThread.Start(); //starting the thread
            // chromeWebBrowser.ExecuteScript("alert('executeJavaScript');");

        }

        private void First_Thread()
        {
            Card c = new Card();
            c.epc = "8888888";
            Console.WriteLine("12121221212121");
            chromeWebBrowser.ExecuteScript("bridge_map.test1(" + JsonUtils.ToJson(c) + ")");
        }

        private void button_Click(object sender, EventArgs e)
        {
            Console.WriteLine("cliek");
            Card c = new Card();
            c.epc = "8888888";
            Console.WriteLine("1231231123123123");
            chromeWebBrowser.ExecuteScript("bridge_map.test()");
        }
        /**
         * 获取文件大小
         */
        public string get_file_size(string fileName)
        {
            Result<Object> result = new Result<object>();
            FileInfo info = new FileInfo(fileName);
            if (info.Exists)
            {
                result.status = "success";
                result.result = info.Length;
                return result.toJson();
            }
            result.message = "文件不存在";
            result.status = "error";
            return result.toJson();

        }
        public static string defaultfilePath
            /**
             * 弹出框选择文件夹路径
             * 参数： default_file_path：默认路径
             */ ;
        public string folder_browser_dialog(string jsonParam)
        {
            Result<Object> result = new Result<object>();
            Dictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<Dictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            if (p.ContainsKey("default_file_path") && p["default_file_path"] != null)
            {
                string dffp = p["default_file_path"].ToString();
                if (!dffp.Trim().Equals(""))
                {
                    defaultfilePath = dffp;
                }

                //首次defaultfilePath为空，按FolderBrowserDialog默认设置（即桌面）选择  
                if (!defaultfilePath.Trim().Equals(""))
                {
                    //设置此次默认目录为上一次选中目录  
                    dialog.SelectedPath = defaultfilePath;
                }
            }

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                //记录选中的目录  
                defaultfilePath = dialog.SelectedPath;
                result.status = "success";
                result.result = defaultfilePath;
                return result.toJson();
            }
            else
            {
                result.status = "cancle";
                return result.toJson();
            }

        }
        /*
         * 文件弹出框，获取文件
         * initial_directory ：初始化路径
         * filter：//"文本文件|*.*|C#文件|*.cs|所有文件|*.*";
         * filter_index：设置索引
         */
        public string open_file_dialog(string jsonParam)
        {
            Result<Object> result = new Result<object>();
            Dictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<Dictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            OpenFileDialog openFiledDialog = new OpenFileDialog();
            if (p.ContainsKey("initial_directory") && p["initial_directory"] != null)
            {
                openFiledDialog.InitialDirectory = p["initial_directory"].ToString();//"D:\\";
            }
            if (p.ContainsKey("filter") && p["filter"] != null)
            {
                openFiledDialog.Filter = p["filter"].ToString();//"文本文件|*.*|C#文件|*.cs|所有文件|*.*";

            }
            if (p.ContainsKey("filter_index") && p["filter_index"] != null)
            {
                string _filter_index = p["filter_index"].ToString();
                int filter_index = 1;
                try { filter_index = Convert.ToInt32(_filter_index); }
                catch (Exception ex) { }
                openFiledDialog.FilterIndex = filter_index;

            }
            string fname = null;
            if (openFiledDialog.ShowDialog() == DialogResult.OK)
            {
                //打开文件对话框中选择的文件名
                fname = openFiledDialog.FileName;
                result.status = "success";
                result.result = fname;
                return result.toJson();
            }
            else
            {
                result.status = "cancle";
                return result.toJson();
            }
        }
        public string get_system_parameters(string jsonParam)
        {
            Dictionary<string, Object> p = new Dictionary<string, object>();
            p.Add("primary_screen_width", SystemParameters.PrimaryScreenWidth);
            p.Add("primary_screen_height", SystemParameters.PrimaryScreenHeight);
            // p.Add("workarea_width", SystemParameters.WorkArea.Width);
            // p.Add("workarea_height", SystemParameters.WorkArea.Height);
            p.Add("full_primary_screen_width", SystemParameters.FullPrimaryScreenWidth);
            p.Add("full_primary_screen_height", SystemParameters.FullPrimaryScreenHeight);
            Console.WriteLine("xxx");
            return JsonUtils.ToJson(p);
        }

        /// <summary>
        /// 打开读写器
        /// </summary>
        /// <param name="jsonParam"></param>
        /// <returns></returns>
        public string open_by_com(string jsonParam)
        {
            Result<Object> result = new Result<object>();
            CDictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<CDictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }

            try
            {
                int port = p.GetInt("port");
                byte Baud = p.GetByte("Baud");
                byte fComAdr = HexHelper.HexStringToByte(appSettings.GetString("fComAdr"));
                fCmdRet = reader.OpenByCom(port, ref fComAdr, Baud);
                if (fCmdRet == 0)
                {
                    runtimeSettings["rwdIsConnect"] = true;
                    result.status = "success";
                    return result.toJson();
                }
                else
                {
                    result.status = "error";
                    result.message = "读写器连接失败";
                    return result.toJson();
                }
            }
            catch (Exception e)
            {
                result.status = "error";
                result.message = e.Message;
                return result.toJson();
            }
        }
        /// <summary>
        /// 关闭读写器
        /// </summary>
        /// <returns></returns>
        public string close_by_com()
        {
            Result<Object> result = new Result<object>();
            try
            {
                fCmdRet = reader.CloseByCom();
                if (fCmdRet == 0)
                {
                    appSettings[" frmcomportindex"] = -1;
                    result.status = "success";
                    return result.toJson();
                }
                result.status = "error";
                result.message = "读写器关闭失败";
                return result.toJson();
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
        }

        public string open_by_tcp(string jsonParam)
        {
            Result<Object> result = new Result<object>();
            CDictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<CDictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            try
            {
                int port = p.GetInt("port");
                string ipAddress = p.GetString("ipAddress");
                byte fComAdr = HexHelper.HexStringToByte(appSettings.GetString("fComAdr"));
                fCmdRet = reader.OpenByTcp(ipAddress, port, ref fComAdr);
                if (fCmdRet == 0)
                {
                    result.message = "读写器连接成功";
                    result.status = "success";
                    return result.toJson();
                }
                else
                {
                    result.message = "读写器连接失败";
                    result.status = "error";
                    return result.toJson();
                }
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                return result.toJson();
            }
        }
        public string close_by_tcp()
        {
            Result<Object> result = new Result<object>();
            fCmdRet = reader.CloseByTcp();
            if (fCmdRet == 0)
            {
                result.status = "success";
                result.message = "读写器关闭成功";
            }
            else
            {
                result.status = "error";
                result.message = "读写器关闭失败";
            }
            return result.toJson();
        }

        public string beep_setting(string jsonParam)
        {
            Result<Object> result = new Result<object>();
            CDictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<CDictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            try
            {
                byte beepEn = p.GetByte("beepEn");
                byte fComAdr = HexHelper.HexStringToByte(appSettings.GetString("fComAdr"));
                fCmdRet = reader.SetBeepNotification(ref fComAdr, beepEn);
                if (fCmdRet == 0)
                {
                    result.status = "success";
                    result.message = "蜂鸣器设置成功";
                }
                else
                {
                    result.status = "error";
                    result.message = "蜂鸣器设置失败";
                }
                return result.toJson();
            }
            catch (Exception ex)
            {
                result.status = "error";
                result.message = ex.Message;
                return result.toJson();
            }
        }

        public void flash_mix_g2()
        {
            try
            {
                byte fComAdr = HexHelper.HexStringToByte(appSettings.GetString("fComAdr"));
                byte qValue = HexHelper.HexStringToByte(appSettings.GetString("qValue"));
                byte session = appSettings.GetByte("session");
                byte maskMem = 0;
                byte[] maskAdr = new byte[2];
                byte maskLen = 0;
                byte[] maskData = new byte[100];
                byte maskFlag = appSettings.GetByte("maskFlag");
                byte readMem = appSettings.GetByte("readMem");
                byte[] readAdr = IntegerHelper.intToBytes(appSettings.GetInt("readAdr"), 2);
                byte readLen = appSettings.GetByte("readLen");
                byte[] psd = IntegerHelper.intToBytes(appSettings.GetInt("psd"), 4);
                byte target = appSettings.GetByte("target");
                byte scanTime = appSettings.GetByte("scanTime");
                byte fastFlag = appSettings.GetByte("fastFlag");
                fCmdRet = reader.InventoryMix_G2(ref fComAdr, qValue, session, maskMem, maskAdr, maskLen, maskData, maskFlag, readMem, readAdr, readLen, psd, target, inAnt, scanTime, fastFlag);
            }
            catch (Exception e)
            {
                receiveData("5:" + e.ToString());
            }
        }

        private void flash_g2()
        {
            try
            {
                byte fComAdr = HexHelper.HexStringToByte(appSettings.GetString("fComAdr"));
                byte qValue = appSettings.GetByte("qValue");
                byte session = appSettings.GetByte("session");
                byte maskMem = 0;
                byte[] maskAdr = new byte[2];
                byte maskLen = 0;
                byte[] maskData = new byte[100];
                byte maskFlag = 0;
                byte adrTID = 0;
                byte lenTID = 4;
                byte tIDFlag = appSettings.GetByte("tIDFlag");
                byte target = appSettings.GetByte("target");
                byte scanTime = appSettings.GetByte("scanTime");
                byte fastFlag = appSettings.GetByte("fastFlag");
         
                fCmdRet = reader.Inventory_G2(ref fComAdr, qValue, session, maskMem, maskAdr, maskLen, maskData, maskFlag, adrTID, lenTID, tIDFlag, target, inAnt, scanTime, fastFlag);
            }
            catch (Exception e)
            {
                receiveData("5:" + e.ToString());
            }
        }

        public static string call_back;
        public static string error;
        private string INVENTORY_MODE;
        private byte inAnt;
        public void inventory()
        {
            reader.ReceiveCallback = receiveData;
            while (runtimeSettings.GetBoolean("inventory_flag"))
            {
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            inAnt = 0x80;
                            break;
                        case 1:
                            inAnt = 0x81;
                            break;
                        case 2:
                            inAnt = 0x82;
                            break;
                        case 3:
                            inAnt = 0x83;
                            break;
                    }
                    if (antList[i] == 1)
                    {
                        if (!CheckRuntimeStatus()) return;
                        if (INVENTORY_MODE == "0")
                        { //EPC查询
                            flash_g2();
                        }
                        else { //混合查询
                            flash_mix_g2();
                        }
                    }
                }
            }
        }
        public string start_inventory(string jsonParam)
        {
            Result<object> result = new Result<object>();
            CDictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<CDictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            try
            {
                    runtimeSettings["inventory_flag"] = true;
                    call_back = p.GetString("call_back");
                    error = p.GetString("error");
                    INVENTORY_MODE = p.GetString("mode");
                    Thread thread = new Thread(inventory);
                    thread.Start();
                    result.status = "success";
            }
            catch (Exception)
            {
                
                throw;
            }
            return result.toJson();

        }
        public string stop_inventory(string jsonParam)
        {
            Result<object> result = new Result<object>();
            CDictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<CDictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            runtimeSettings["inventory_flag"] = false;
            tags.Clear();
            return result.toJson();
        }

        /// <summary>
        /// 获取读写器序列号
        /// </summary>
        /// <returns></returns>
        public string get_rwd_serial_no()
        {
            Result<object> result = new Result<object>();
            byte[] serialNo = new byte[4];
            try
            {
                byte fComAdr = HexHelper.HexStringToByte(appSettings.GetString("fComAdr"));
                fCmdRet = reader.GetSeriaNo(ref fComAdr, serialNo);
                if (fCmdRet == 0)
                {
                    result.status = "success";
                    result.message = "获取读写器序列号成功";
                    result.result = HexHelper.ByteArrayToHexString(serialNo);
                    return result.toJson();
                }
                else
                {
                    result.status = "error";
                    result.message = "获取读写器序列号失败";
                    return result.toJson();
                }
            }
            catch (Exception e)
            {
                result.status = "error";
                result.message = e.Message;
                return result.toJson();
            }
        }
        private string epc;
        private string tid;
        private void receiveData(string stemp)
        {
            Result<object> result = new Result<object>();
            string[] strs = stemp.Split(new Char[] { ':' });
            if (call_back != null)
            {
                if (strs != null && strs.Length > 1)
                {
                    if ("1".Equals(strs[0]))
                    {
                        string[] infos = strs[1].Split(new Char[] { ',' });
                        if (infos != null && infos.Length >= 3)
                        {
                            string ant = infos[0];
                            string epc = infos[1];
                            string count = infos[2];
                            TagInfo tag = new TagInfo();
                            tag.Ant = ant;
                            tag.Epc = epc;
                            if (!CheckTag(tag))
                            {
                                result.status = "success";
                                result.message = "查询成功";
                                result.result = tag;
                                chromeWebBrowser.ExecuteScript(call_back + "(" + result.toJson() + ")");
                                tags.Add(tag);
                            }
                            return;
                        }
                    }
                    if ("4".Equals(strs[0]))
                    {
                        string[] infos = strs[1].Split(new Char[] { ',' });
                        if (infos != null && infos.Length > 0)
                        {
                            int gnum = Convert.ToInt32(infos[0], 16);
                            string ant = null;
                            if (gnum < 0x80)//epc
                            {
                                epc = infos[1];
                                ant = infos[3];
                            }
                            else//tid
                            {
                                tid = infos[1];
                                ant = infos[3];
                                if (epc != null && tid != null && ant != null)
                                {
                                    TagInfo tag = new TagInfo();
                                    tag.Epc = epc;
                                    tag.Ant = ant;
                                    tag.TId = tid;
                                    if (!CheckTag(tag))
                                    {
                                        result.status = "success";
                                        result.message = "查询成功";
                                        result.result = tag;
                                        chromeWebBrowser.ExecuteScript(call_back + "(" + result.toJson() + ")");
                                        tags.Add(tag);
                                    }
                                    return;
                                }
                            }
                        }
                    }

                }
            }
            if (error != null)
            {
                if ("5".Equals(strs[0]))
                {
                    result.status = "error";
                    result.message = strs[1];
                    chromeWebBrowser.ExecuteScript(error + "('" + result.toJson() + "')");
                }
            }
        }
        public string set_rwd_power(string jsonParam)
        {
            Result<object> result = new Result<object>();
            CDictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<CDictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            byte power = 0;
            if (p.ContainsKey("power"))
            {
                power = p.GetByte("power");
            }
            else
            {
                power = appSettings.GetByte("power");
            }
            byte fComAdr = HexHelper.HexStringToByte(appSettings.GetString("fComAdr"));

            fCmdRet = reader.SetRfPower(ref fComAdr, power);
            if (fCmdRet == 0)
            {
                result.status = "success";
                result.message = "设置读写器功率成功";
                //    result.result = HexHelper.ByteArrayToHexString(serialNo);
                return result.toJson();
            }
            else
            {
                result.status = "error";
                result.message = "设置读写器功率失败";
                //    result.result = HexHelper.ByteArrayToHexString(serialNo);
                return result.toJson();
            }
        }

        public string write_data_g2(string jsonParam)
        {
            Result<object> result = new Result<object>();
            CDictionary<string, Object> p;
            try
            {
                p = JsonUtils.FromJson<CDictionary<string, Object>>(jsonParam);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            //reader.WriteData_G2()
            return result.toJson();
        }
        public bool CheckTag(TagInfo tag)
        {
            return tags.Any(p => p.Epc == tag.Epc);
        }
        public bool CheckRuntimeStatus() {
            string message = null;
            bool status = true;
            if (!runtimeSettings.GetBoolean("rwdIsConnect")) { message = "读写器未连接"; status = false; }
            if (!status) { 
                    receiveData("5:"+message);
            }
            return status;
        }
    }
}
