using per.cz.util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinFormDemo.per.cz.bean
{
    public class TagInfo
    {
        private string ant;

        public string Ant
        {
            get { return ant; }
            set { ant = value; }
        }
        private string epc;

        public string Epc
        {
            get { return epc; }
            set { epc = value; }
        }
        public string toJson()
        {
            return JsonUtils.ToJson(this);
        }
        private string tId;

        public string TId
        {
            get { return tId; }
            set { tId = value; }
        }

    }
}
