using com.zy.entity.table;
using com.zy.service;
using cwber;
using per.cz.bean;
using per.cz.util;
using Sashulin;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using WinFormDemo;

namespace per.cz.frame.bridge
{

    [ComVisible(true)]
    // [ComVisibleAttribute(true)]
    public class ChromeWebBrowserBridge:Bridge
    {
        public static Dictionary<string, string> user_infos = new Dictionary<string, string>();
        private ChromeWebBrowser chromeWebBrowser;

        public ChromeWebBrowserBridge(Main m)
        {
            this.chromeWebBrowser = m.chromeWebBrowser;
            init_event();
            init();
        }
        private void init_event()
        {
            this.chromeWebBrowser.Load += new System.EventHandler(this.chromeWebBrowser_Load);
        }

        private void chromeWebBrowser_Load(object sender, EventArgs e)
        {
            Console.WriteLine("chromeWebBrowser_Load");
        }
        private void init()
        {
            CSharpBrowserSettings settings = new CSharpBrowserSettings();
            //settings.DefaultUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()) + @"\www\html.html";
            settings.DefaultUrl = System.IO.Directory.GetParent(System.IO.Directory.GetCurrentDirectory()) + @"\www\page\login\login1.html";
            //settings.DefaultUrl = "www.baidu.com";
            settings.UserAgent = "Mozilla/5.0 (Linux; Android 4.2.1; en-us; Nexus 4 Build/JOP40D) AppleWebKit/535.19 (KHTML, like Gecko) Chrome/18.0.1025.166 Mobile Safari/535.19";
            Console.WriteLine(settings.DefaultUrl);
            settings.CachePath = @"C:\temp\caches";
            chromeWebBrowser.Initialize(settings);
        }
        public String test(string p)
        {
            Dictionary<string, Object> t = new Dictionary<string, Object>();
            t["t"] = "name";
            t["t1"] = "age";
            t["in"] = JsonUtils.JsonToDictionary(p);
            return JsonUtils.ToJson(t);
        }
        public string saveCard(string jsonParam)
        {
             Result<Object> result = new Result<object>();
            Card c=JsonUtils.FromJson<Card>(jsonParam);
            if (c != null)
            {
               Result<int> res= CardService.saveCard(c);
                return JsonUtils.ToJson(res);
            }
            result.status="error";
            return result.toJson();

        }
        public string findCardByParams(string jsonParam)
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
            Result<List<Card>> res = CardService.findCardByParams(p);
            return res.toJson();
        }
        public string findCardByTid(string jsonParam)
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
            if (p.ContainsKey("tid"))
            {
                String tid = p["tid"].ToString();
                Result<Card> res = CardService.findCardByTid(tid);
                return res.toJson();
            }
            result.status = "error";
            result.message = "tid 参数不存在";
            return result.toJson();

        }

        //{
        //key:字段名称
        //value:字段值
        //}
        public string delCardByXX(string jsonParam)
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
            if (p.ContainsKey("key") &&p["key"]!=null&& p.ContainsKey("value")&&p["value"]!=null)
            {

                String key = p["key"].ToString().Trim();
                string value=p["value"].ToString().Trim();
                Result<int> res = CardService.delCardByXX(key,value);
                return res.toJson();
            }
            result.status = "error";
            result.message = "参数不完整";
            return result.toJson();
        }
        public string updateCardByParams(string jsonOld, string jsonNew)
        {
            Result<Object> result = new Result<object>();
            Dictionary<string, Object> old;
            Dictionary<string, Object> _new;
            try
            {
                old = JsonUtils.FromJson<Dictionary<string, Object>>(jsonOld);
                _new = JsonUtils.FromJson<Dictionary<string, Object>>(jsonNew);
            }
            catch (Exception ex)
            {
                result.message = ex.Message;
                result.status = "error";
                result.result = ex.StackTrace;
                return result.toJson();
            }
            return CardService.updateCardByParams(old, _new).toJson();
        }

        
              //{epc:11111}
        public string findCardByEpc(string jsonParam)
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
            if (p.ContainsKey("epc"))
            {
                String epc=p["epc"].ToString();
                Result<Card> res=CardService.findCardByEpc(epc);
                return res.toJson();
            }
            result.status = "error";
            result.message = "epc 参数不存在";
            return result.toJson();

        }
    }
}
