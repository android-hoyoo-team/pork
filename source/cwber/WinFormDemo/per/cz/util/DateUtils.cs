using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
//using System.Runtime.Serialization.Json;
using System.Configuration;
using System.IO;
using System.Web.Script.Serialization;
using System.Net;
using per.cz.bean;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;

namespace per.cz.util
{
    public class DateUtils
    {
        public static double GetTime()
        {
            DateTime d1 = DateTime.SpecifyKind(DateTime.Now, DateTimeKind.Utc);
            DateTime d2 = DateTime.SpecifyKind(new DateTime(1970, 1, 1), DateTimeKind.Utc); 
            double d = d1.Subtract(d2).TotalMilliseconds;
            return d;
        }
        public static string format(DateTime date, String foramt)
        {
            return String.Format(foramt, date); 
        }
        //
        //yyyy-MM-dd hh:mm:ss
        //
        public static string format(DateTime date)
        {
            // DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            //dtFormat.ShortDatePattern = format;
            //Convert.ToString(date,)
            return String.Format("yyyy-MM-dd hh:mm:ss", date);
        }
        //
        //yyyy-MM-dd hh:mm:ss
        //
        public static DateTime parse(string date)
        {
            return Convert.ToDateTime(date);
        }
        //
        public static DateTime parse(string date,String format)
        {
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = format;
            return Convert.ToDateTime(date, dtFormat);
        }
                    
    }
}
