using com.zy.entity.table;
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

            //Console.WriteLine(inAnt);
            //Console.WriteLine();
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
            ChromeWebBrowserBridge chromeWebBrowserBridge = new ChromeWebBrowserBridge(chromeWebBrowser);
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
            appSettings["antList"] = settingsReader.GetValue("antList", typeof(String));
            appSettings["scanTime"] = settingsReader.GetValue("scanTime", typeof(String));
            appSettings["fastFlag"] = settingsReader.GetValue("fastFlag", typeof(String));
            appSettings["qValue"] = settingsReader.GetValue("qValue", typeof(String));
            appSettings["qValue"] = settingsReader.GetValue("qValue", typeof(String));
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
                //byte[] readAdr = IntegerHelper.intToBytes(appSettings.GetInt("readAdr"),2);
                byte[] readAdr = new byte[] { 0, 2 };
                byte readLen = appSettings.GetByte("readLen");
                byte[] psd = IntegerHelper.intToBytes(appSettings.GetInt("psd"), 4);
                byte target = appSettings.GetByte("target");
                int antList = appSettings.GetInt("antList");
                byte inAnt = 0;
                if (antList % 10 == 1)
                {
                    inAnt = 0x80;
                }
                else if ((antList /= 10) % 10 == 1)
                {
                    inAnt = 0x81;
                }
                else if ((antList /= 100) % 10 == 1)
                {
                    inAnt = 0x82;
                }
                else if ((antList /= 1000) % 10 == 1)
                {
                    inAnt = 0x83;
                }
                else
                {

                }
                byte scanTime = appSettings.GetByte("scanTime");
                byte fastFlag = appSettings.GetByte("fastFlag");
                reader.ReceiveCallback = receiveData;
                while (runtimeSettings.GetBoolean("inventory_flag"))
                {
                    //self:  fComAdr:255,Qvalue:132,Session:2,MaskMem:0,MaskAdr:System.Byte[],MaskLen:0,MaskData:System.Byte[], ReadMem:2, ReadAdr:System.Byte[], ReadLen:4, Psd:System.Byte[], Target:0, InAnt:131, Scantime:20, FastFlag:1
                    //           fComAdr:0,Qvalue:132,Session:0,MaskMem:0,MaskAdr:System.Byte[],MaskLen:0,MaskData:System.Byte[], ReadMem:2, ReadAdr:System.Byte[], ReadLen:4, Psd:System.Byte[], Target:0, InAnt:131, Scantime:20, FastFlag:1
                    //string _psd = String.Join(",", Array.ConvertAll(psd, (Converter<byte, string>)Convert.ToString));
                    //string _readAdr = String.Join(",", Array.ConvertAll(readAdr, (Converter<byte, string>)Convert.ToString));
                    //Console.WriteLine("fComAdr:{0},Qvalue:{1},Session:{2},MaskMem:{3},MaskAdr:{4},MaskLen:{5},MaskData:{6}, ReadMem:{7}, ReadAdr:{8}, ReadLen:{9}, Psd:{10}, Target:{11}, InAnt:{12}, Scantime:{13}, FastFlag:{14}", fComAdr, qValue, session, maskMem, maskAdr, maskLen, maskData, readMem, _readAdr, readLen, _psd, target, inAnt, scanTime, fastFlag);
                    fCmdRet = reader.InventoryMix_G2(ref fComAdr, qValue, session, maskMem, maskAdr, maskLen, maskData, maskFlag, readMem, readAdr, readLen, psd, target, inAnt, scanTime, fastFlag);
                }
            }
            catch (Exception e)
            {
                string m = e.Message;
                Console.WriteLine(m);
                throw;
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
                byte inAnt = appSettings.GetByte("inAnt");
                byte scanTime = appSettings.GetByte("scanTime");
                byte fastFlag = appSettings.GetByte("fastFlag");
                reader.ReceiveCallback = receiveData;
                while (runtimeSettings.GetBoolean("inventory_flag"))
                    fCmdRet = reader.Inventory_G2(ref fComAdr, qValue, session, maskMem, maskAdr, maskLen, maskData, maskFlag, adrTID, lenTID, tIDFlag, target, inAnt, scanTime, fastFlag);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public static string call_back;
        public static string error;
        public string inventory(string jsonParam)
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
            call_back = null;
            runtimeSettings["inventory_flag"] = true;
            call_back = p.GetString("call_back");
            error = p.GetString("error");
            Thread thread = new Thread(flash_mix_g2);
            thread.Start();
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
            Console.WriteLine(stemp);
            if (call_back != null)
            {
                string[] strs = stemp.Split(new Char[] { ':' });
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
                            TagInfo tagInfo = new TagInfo();
                            tagInfo.Ant = ant;
                            tagInfo.Epc = epc;
                            if (!CheckTag(tagInfo))
                            {
                                chromeWebBrowser.ExecuteScript(call_back + "(" + tagInfo.toJson() + ")");
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
                            if (gnum < 0x80)//epc
                            {

                            }
                            else//tid
                            {

                            }
                        }
                    }
                }
            }
            if (error != null)
            {
                // Console.WriteLine("stemp:"+stemp);
                chromeWebBrowser.ExecuteScript(error + "('" + stemp + "')");
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
            return result.toJson();
        }
        List<TagInfo> tagInfos = new List<TagInfo>();
        public bool CheckTag(TagInfo tag)
        {
            return tagInfos.Any(p => p.Epc == tag.Epc);
        }
    }
}
