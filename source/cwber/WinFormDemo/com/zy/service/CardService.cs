
using com.zy.entity.table;
using per.cz.bean;
using per.cz.db;
using per.cz.util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.zy.service
{
    class CardService
    {
        /**
         *      public int id;
        public string epc;
        public string tid;
        public string uid;
        public string viewNum;//0-9999 可视化标签号 
        public long addTime;
        public long updateTime;
        public long deadTime;
         */
        public static Result<Card> findCardByEpc(string epc)
        {
            return findFirstCardByXX("epc", epc);
        }
        public static Result<Card> findCardByTid(string tid)
        {
            return findFirstCardByXX("tid", tid);
        }
        public static Result<Card> findFirstCardByXX(string key,string xx)
        {
            Result<Card> result = new Result<Card>();
            Dictionary<string, object> param = new Dictionary<string, object>();
            param[key] = xx;
            Result<List<Card>> res = findCardByParams(param);
            if (res.status.Equals("success"))
            {
                if (res.result != null && res.result.Count > 0)
                {
                    result.result = res.result[0];
                    result.status = "success";
                    return result;
                }
                else
                {
                    result.status = "success";
                    result.message = "没有找到";
                    return result;
                }
            }
            result.status = "error";
            result.message = res.message;
            return result;
        }

        public static Result<List<Card>> findCardByParams(Dictionary<String,object> param)
        {
            if (param == null)
                param = new Dictionary<string, object>();
            String sql = "SELECT  id,epc,tid,uid,viewNum,addTime,updateTime,deadTime from card  where 1=1 {epc} {tid} {viewNum} and ( deadTime=null or deadTime <" + DateUtils.GetTime() + ")";
            sql = sql.Replace("{epc}", param.ContainsKey("epc") ? " and epc = '" + param["epc"] + "'" : "");
            sql = sql.Replace("{tid}", param.ContainsKey("tid") ? " and tid = '" + param["tid"] + "'" : "");
            sql = sql.Replace("{viewNum}", param.ContainsKey("viewNum") ? " and viewNum = '" + param["viewNum"] + "'" : "");
            Result<List<Card>> res = new Result<List<Card>>();
            Result<DataTable> r = DB.executeQuery(sql);
            if (r.status.Equals("error"))
            {
                res.status = "error";
                res.message = r.message;
                return res;
            }
            DataTable d = r.result;
            if (d.Rows.Count == 0)
            {
                res.status = "success";
                res.message = "没有找到";
                return res;
            }
            else
            {
                List<Card> list = new List<Card>();
                for (int i = 0; i < d.Rows.Count; i++)
                { 
                    Card u = new Card();
                    u.id = Convert.ToInt32(d.Rows[i][0]);
                    u.epc = Convert.ToString(d.Rows[i][1]);
                    u.tid = Convert.ToString(d.Rows[i][2]);
                    u.uid = Convert.ToString(d.Rows[i][3]);
                    u.viewNum = Convert.ToString(d.Rows[i][4]);
                    u.addTime = Convert.ToInt64(d.Rows[i][5]);
                    u.updateTime = Convert.ToInt64(d.Rows[i][6]);
                    u.deadTime = Convert.ToInt64(d.Rows[i][7]);
                    list.Add(u);
                }
                res.status = "success";
                res.result = list;
                return res;
            }

        }
        public static Result<int> saveCard(Card c)
        {
            Result<int> res = new Result<int>();
            if (c == null)
            {
                res.status = "error";
                res.result = 0;
                return res;
            }
            Result<Card> res4Tid = findCardByTid(c.tid);
            if (res4Tid.status.Equals("success") && res4Tid.result!=null)
            {
                res.status = "error";
                res.message = "tid 为" + c.tid+"的标签已经保存";
                res.result = 0;
                return res;
            }
            String sql = "INSERT INTO `card`" +
            "(epc,tid,uid,viewNum,addTime,updateTime,deadTime)" +
            "VALUES ('" + c.epc + "','" + c.tid + "','" + c.uid + "','" + c.viewNum + "'," + DateUtils.GetTime() + ",0,0)";
            return DB.executeInsert(sql);
        }
        public static Result<int> updateCardByParams(Dictionary<String, object> old, Dictionary<String, object> _new)
        {
            Result<int> res = new Result<int>();
            if (old == null)
                old = new Dictionary<string, object>();
            if (_new == null || _new.Count <= 0)
            {
                res.message = "条件参数不能为空";
                res.status = "error";
                return res;
            }
            String sql = "update card set updateTime=" + DateUtils.GetTime() + " :epc: :tid: :uid: :viewNum:  where 1=1 {epc} {tid} {viewNum} {uid} and ( deadTime=null or deadTime <" + DateUtils.GetTime() + ")";

            sql = sql.Replace(":epc:", _new.ContainsKey("epc") ? " ,epc = '" + _new["epc"] + "'" : "");
            sql = sql.Replace(":tid:", _new.ContainsKey("tid") ? " ,tid = '" + _new["tid"] + "'" : "");
            sql = sql.Replace(":uid:", _new.ContainsKey("uid") ? " ,uid = '" + _new["uid"] + "'" : "");
            sql = sql.Replace(":viewNum:", _new.ContainsKey("viewNum") ? " ,viewNum = '" + _new["viewNum"] + "'" : "");

            sql = sql.Replace("{epc}", old.ContainsKey("epc") ? " and epc = '" + old["epc"] + "'" : "");
            sql = sql.Replace("{tid}", old.ContainsKey("tid") ? " and tid = '" + old["tid"] + "'" : "");
            sql = sql.Replace("{uid}", old.ContainsKey("uid") ? " and uid = '" + old["uid"] + "'" : "");
            sql = sql.Replace("{viewNum}", old.ContainsKey("viewNum") ? " and viewNum = '" + old["viewNum"] + "'" : "");
            Result<int> r = DB.executeUpdate(sql);
            return r;
        }
        public static Result<int> delCardByParams(Dictionary<String, object> param)
        {
            if (param == null)
                param = new Dictionary<string, object>();
            String sql = "update card set deadTime=" + DateUtils.GetTime() + " where 1=1 {epc} {tid} {viewNum} and ( deadTime=null or deadTime >" + DateUtils.GetTime() + ")";
            sql = sql.Replace("{epc}", param.ContainsKey("epc") ? " and epc = '" + param["epc"] + "'" : "");
            sql = sql.Replace("{tid}", param.ContainsKey("tid") ? " and tid = '" + param["tid"] + "'" : "");
            sql = sql.Replace("{viewNum}", param.ContainsKey("viewNum") ? " and viewNum = '" + param["viewNum"] + "'" : "");
            Result<int> r = DB.executeUpdate(sql);
            return r;
        }
        public static Result<int> delCardByXX(string key, string xx)
        {
            Dictionary<string, object> param = new Dictionary<string, object>();
            param[key] = xx;
           return delCardByParams(param);
        }
        public static Result<int> delCardByEpc(String epc)
        {
            return delCardByXX("epc", epc);
        }
        public static Result<int> delCardByTid(String tid)
        {
            return delCardByXX("tid", tid);
        }
        public static Result<int> delCardByviewNum(String viewNum)
        {
            return delCardByXX("viewNum", viewNum);
        }

    }
}
