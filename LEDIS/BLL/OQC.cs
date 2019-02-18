using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;

namespace BLL
{
    public class OQC
    {
        //public static void DelNGList(string json)
        //{
        //    int id = Convert.ToInt32(json);
        //    using (var context = LEDAO.APIGateWay.GetEntityContext())
        //    {
        //        var var = (from u in context.P_OQC_NGList where u.id == id select u).Single();
        //        context.P_OQC_NGList.DeleteObject(var);
        //        context.SaveChanges();
        //    }
        //}
        //public static void CreateNGList(string json)
        //{
        //    LEDAO.P_OQC_NGList oqc = JsonConvert.DeserializeObject<LEDAO.P_OQC_NGList>(json);
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    using (context)
        //    {
        //        context.P_OQC_NGList.AddObject(oqc);
        //        context.SaveChanges();
        //    }
        //}
        //public static string GetNGList(string json)
        //{
        //    string[] str = json.Split(',');
        //    string oqcno = str[0];
        //    string orderNo = str[1];
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    using (context)
        //    {
        //        var var = context.P_OQC_NGList.Where(X => X.oqc_order_no == oqcno && X.order_no == orderNo);
        //        if (var.Count() > 0)
        //        {
        //            return JsonConvert.SerializeObject(var.ToList());
        //        }
        //    }
        //    return null;
        //}
        //public static void CreateOQC_Order(string json)
        //{
        //    LEDAO.B_OQC_Order oqc = JsonConvert.DeserializeObject<LEDAO.B_OQC_Order>(json);
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    using (context)
        //    {
        //        context.B_OQC_Order.AddObject(oqc);
        //        context.SaveChanges();
        //    }
        //}
        //public static void UpDataOQCOrder(string json)
        //{
        //    string[] str = json.Split(',');
        //    string oqcNo = str[0];
        //    string orderNo = str[1];
        //    int result = Convert.ToInt32(str[2]);
        //    DateTime datatime = Convert.ToDateTime(str[3]);
        //    try
        //    {
        //        using (var context = LEDAO.APIGateWay.GetEntityContext())
        //        {
        //            var model = context.B_OQC_Order.Where(X => X.oqc_order_no == oqcNo && X.order_no == orderNo).FirstOrDefault();
        //            model.oqc_result = result;
        //            model.end_time = datatime;
        //            context.SaveChanges();
        //        }
        //    }
        //    catch (Exception )
        //    {
        //        throw;
        //    }
       
        //}
        //public static string GetOQCIndexDetail(string data)
        //{
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    var var = (from a in context.B_OQC_IndexDetail
        //               select new
        //               {
        //                   a.idx_type
        //               }).Distinct();
        //    if (var.Count() > int.Parse(data))
        //    {
        //        return JsonConvert.SerializeObject(var.ToList());
        //    }
        //    return null;
        //}
        //public static string GetOQCIndex(string data)
        //{
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    var var = from u in context.B_OQC_Index where u.item_char != data select u;

        //    if (var.Count() > 0)
        //    {
        //        return JsonConvert.SerializeObject(var.ToList());
        //    }
        //    return null;
        //}
        //public static string GetOQCType(string data)
        //{
        //    var context = LEDAO.APIGateWay.GetEntityContext();

        //    var var = from u in context.B_OQC_Type where u.oqc_type_item != data select u;

        //    if (var.Count() > 0)
        //    {
        //        return JsonConvert.SerializeObject(var.ToList());
        //    }
        //    return null;
        //}
        public static string GetSFCByorder(string json)
        {
            string[] str = json.Split(',');
            string orderNo = str[0];
            string sfc = str[1];
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var var = context.P_SFC_State.Where(X => X.order_no == orderNo && X.state == 2 && X.SFC == sfc);//2代表已结单
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var.ToList());
                }
            }

            return null;
        }
        //public static string GetStandardCode(string json)
        //{
        //    string[] str = json.Split(',');
        //    int num = Convert.ToInt32(str[0]);
        //    int tpid = Convert.ToInt32(str[1]);

        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    using (context)
        //    {
        //        var var = context.V_OQC_Index_Detail.Where(X => (X.qty1 < num && X.qty2 > num) && (X.oqc_type_id == tpid));
        //        if (var.Count() > 0)
        //        {
        //            return JsonConvert.SerializeObject(var.ToList());
        //        }
        //    }
        //    return null;
        //}
        //public static string GetIndex(string json)
        //{
        //    string[] str = json.Split(',');
        //    int num = Convert.ToInt32(str[0]);
        //    int tpid = Convert.ToInt32(str[1]);
        //    int id = Convert.ToInt32(str[2]);
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    using (context)
        //    {
        //        var var = context.V_OQC_Index_Detail.Where(X => (X.qty1 < num && X.qty2 > num) && (X.oqc_type_id == tpid && X.id == id));
        //        if (var.Count() > 0)
        //        {
        //            return JsonConvert.SerializeObject(var.ToList());
        //        }
        //    }
        //    return null;
        //}
    }
}