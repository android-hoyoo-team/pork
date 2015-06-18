using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using Cef3;
using Sashulin.common;
using Sashulin.Core;
using cwber;
namespace Sashulin
{
    internal class Global
    {
        internal static List<ChromeWebBrowser> BrowserList = new List<ChromeWebBrowser>();
        internal static Dictionary<int, CwbElement> RootList = new Dictionary<int,CwbElement>();
        internal static ClientApp app;
        internal static ChromeWebBrowser instance;
        internal static string Result;
        internal static bool flag;
        internal static object JsEvaResult;

        const string ERROR_CALL_NOTFOUND = "error: this method can not be found.";
        const string ERROR_CALL_PARAMETER = "error: parameters is not match";

        internal static string CallMethod(CefBrowser browser, string methodName, object[] paramValues)
        {
            Type t = null;
            object form = null;
            foreach(ChromeWebBrowser c in BrowserList)
            {
                if (c == null) continue;
                if (c.browser.Identifier == browser.Identifier)
                {
                    if (methodName.IndexOf(".") > 0)
                    {
                        String key = methodName.Substring(0, methodName.IndexOf("."));
                        Bridge brigde = c.GetChromeWebBrowserBridge(key);
                        if (brigde != null)
                        { 
                             t = brigde.GetType();
                             form = brigde;
                             methodName = methodName.Substring(methodName.IndexOf(".")+1);
                             break;
                        }
                    }
                    else {
                        t = c.FindForm().GetType();
                        form = c.FindForm();
                        break;
                    }

                }
            }

            if (t == null)
            {
                return ERROR_CALL_NOTFOUND;
            }

            MethodInfo m = t.GetMethod(methodName);
            if (m == null)
            {
                return ERROR_CALL_NOTFOUND;
            }
            object[] objArray = null;
            //string[] values = new string[0];
            //if (paramValues != null)
            //    values = paramValues.Split(new char[] { ',' });
            objArray = new object[paramValues.Length];
            ParameterInfo[] pa = m.GetParameters();

            if (objArray.Length != pa.Length)
            {
                return ERROR_CALL_PARAMETER;
            }

            int i = 0;
            foreach (ParameterInfo p in pa)
            {
                switch (p.ParameterType.Name)
                {
                    case "String":
                        objArray[i] = paramValues[i];
                        break;
                    case "Int32":
                        objArray[i] = Int32.Parse(paramValues[i].ToString());
                        break;
                    case "Boolean":
                        objArray[i] = Boolean.Parse(paramValues[i].ToString());
                        break;
                    case "Double":
                        objArray[i] = Double.Parse(paramValues[i].ToString());
                        break;
                }
                i++;
            }
            object o = m.Invoke(form, objArray);
            string retVal = string.Empty;
            if (o != null)
                retVal = o.ToString();
            return retVal;
        }
    }

    enum CwbBusinStyle
    {
        bsGetElementValue = 0,
        bsSetElementValue = 1,
        bsAddElementEvent = 2,
        bsVisitDocument = 3,
        bsFocusElement = 4,
        bsAttachElementEvent = 5,
        bsNone = -1
    }

    enum CwbCookieStyle
    {
        csDeleteAllCookie = 0,
        csVisitUrlCookie = 1
    }


}
