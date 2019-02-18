using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using Newtonsoft.Json;

namespace BLL
{
    public class Fail_Detail
    {
        public static string AddFail_Detail(string parstrPFailDetail)
        {
            LEDAO.P_Fail_Detail parPFailDetail = JsonConvert.DeserializeObject<LEDAO.P_Fail_Detail>(parstrPFailDetail);
            if (parPFailDetail == null)
            {
                return "0";
            }
            if (string.IsNullOrWhiteSpace(parPFailDetail.order_no)
                || string.IsNullOrWhiteSpace(parPFailDetail.sfc)
                || string.IsNullOrWhiteSpace(parPFailDetail.ng_code))
            {
                return "0";
            }
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                parPFailDetail.input_time = context.NewDate().First();
                context.P_Fail_Detail.AddObject(parPFailDetail);
                context.SaveChanges();
            }
            return "1";
        }
        //跟新记录，根据id，更新原因代码，原因类型代码，完成时间
        public static string UpdateFail_Detail(string parstrPFailDetail)
        {
            List<P_Fail_Detail> lstPFailDetail = new List<P_Fail_Detail>();
            lstPFailDetail = JsonConvert.DeserializeObject<List<P_Fail_Detail>>(parstrPFailDetail);
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                for (int i = 0; i < lstPFailDetail.Count; i++)
                {
                    if (lstPFailDetail[i].id == null
                    || lstPFailDetail[i].id < 1)
                    {
                        continue;
                    }
                    try
                    {
                        int iidTmp = lstPFailDetail[i].id;
                        var var = (from u in context.P_Fail_Detail where u.id == iidTmp select u).FirstOrDefault();
                        if (var != null)
                        {
                            var.reason_code = lstPFailDetail[i].reason_code;
                            var.reasontype_code = lstPFailDetail[i].reasontype_code;
                            var.finish_time = context.NewDate().First();
                            context.SaveChanges();
                        }
                    
                    }
                    catch (Exception exp)
                    {
                        continue ;
                    }
                }
            }
            
            return "1";
        }
        public static string GetNgCode(string parstrfid)
        {
            //var context = LEDAO.APIGateWay.GetEntityContext();
            //using (context)
            //{
            //    var rec = (from a in context.P_FailLog
            //               join b in context.P_Fail_Detail on a.fguid equals b.fguid
            //               join c in context.B_NG_Code on b.ng_code equals c.ng_code
            //               where (a.fid == int.Parse(parstrfid))
            //               select new
            //               {
            //                   ng_code=c.ng_code,
            //                   ng_name=c.ng_name
            //               }).ToList();
            //    if (rec.Count > 0)
            //    {
            //        return JsonConvert.SerializeObject(rec);
            //    }
            //}
            //return null;
            string []arrpar = parstrfid.Split(',');
            if (arrpar.Length < 2)
            {
                return null;
            }
            int ifid = int.Parse(arrpar[0]);
            string strtypecode = arrpar[1];
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = context.V_Fail_Detail_NGName.Where(x => x.fid == ifid && x.typecode == strtypecode);
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
    }
}