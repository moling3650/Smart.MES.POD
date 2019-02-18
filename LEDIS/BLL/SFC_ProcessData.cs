using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;
using System.Data.Objects.SqlClient;

namespace LEDIS.BLL
{
    public class SFC_ProcessData
    {
        //修改&备份SFC绑定关系
        public static string AddSFCprocessDataBack(string json)
        {
            LEDAO.P_SFC_ProcessData_Back PsfcprocessdataBack = JsonConvert.DeserializeObject<LEDAO.P_SFC_ProcessData_Back>(json);
            if (string.IsNullOrWhiteSpace(PsfcprocessdataBack.fguid))
            {
                return "Fail,缺少FailLog fid参数";
            }
            if (string.IsNullOrWhiteSpace(PsfcprocessdataBack.order_no))
            {
                return "Fail,缺少工单参数";
            }
            if (string.IsNullOrWhiteSpace(PsfcprocessdataBack.SFC))
            {
                return "Fail,缺少批次参数";
            }
            if (string.IsNullOrWhiteSpace(PsfcprocessdataBack.val))
            {
                return "Fail,缺少批次值参数";
            }
            if (string.IsNullOrWhiteSpace(PsfcprocessdataBack.mat_code))
            {
                return "Fail,缺少物料号参数";
            }
            if (string.IsNullOrWhiteSpace(PsfcprocessdataBack.New_val))
            {
                return "Fail,缺少批次新值参数";
            }
            
            try
            {
                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    PsfcprocessdataBack.P_Date = context.NewDate().First();
                    {
                        //1.修改P_SFC_State表中原sfc的状态为 -1；
                        var model0 = context.P_SFC_State.Where(X => X.order_no == PsfcprocessdataBack.order_no && X.SFC == PsfcprocessdataBack.val).FirstOrDefault();
                        if (model0 != null)
                        {
                            model0.state = -1;
                        }
                    }
                    {
                        //修改P_SFC_ProcessData表中的绑定关系
                        var model = context.P_SFC_ProcessData.Where(X => X.order_no == PsfcprocessdataBack.order_no && X.SFC == PsfcprocessdataBack.SFC && X.mat_code == PsfcprocessdataBack.mat_code && X.val == PsfcprocessdataBack.val).FirstOrDefault();
                        if (model != null)
                        {
                            model.val = PsfcprocessdataBack.New_val;
                        }
                    }
                    {
                        //增加P_SFC_ProcessData_Back表
                        var model1 = context.P_SFC_ProcessData_Back.Where(X => X.order_no == PsfcprocessdataBack.order_no && X.SFC == PsfcprocessdataBack.SFC && X.mat_code == PsfcprocessdataBack.mat_code && X.val == PsfcprocessdataBack.val).FirstOrDefault();
                        if (model1 != null)
                        {
                            model1.New_val = PsfcprocessdataBack.New_val;
                            model1.P_Date = PsfcprocessdataBack.P_Date;
                        }
                        else
                        {
                            context.P_SFC_ProcessData_Back.AddObject(PsfcprocessdataBack);
                        }
                    }
                    context.SaveChanges();
                }
            }
            catch (Exception exp)
            {
                return "Fail," + exp.Message;
            }
            return "OK";
        }
        //根据工单和测试类型（内阻-NZ/电压-DY/.）获取测试值集合
        public static string GetTestValByOrderAndStepType(string json)
        {
            string strReturn = "";
            try
            {
                if(string.IsNullOrWhiteSpace(json))
                {
                    strReturn = "GetTestValByOrderAndStepType-Fail,传入参数为空";
                    return strReturn;
                }
                string[] arrparitem = json.Split(',');
                if (arrparitem.Length < 2)
                {
                    strReturn = "GetTestValByOrderAndStepType-Fail,传入参数有误";
                    return strReturn;
                }
                List<string> lstorderno = new List<string>();
                string strorderno = arrparitem[0];
                string []arrorderno = strorderno.Split(';');
                if(arrorderno.Length < 1)
                {
                    strReturn = "GetTestValByOrderAndStepType-Fail,传入参数有误";
                    return strReturn;
                }
                for (int j = 0; j < arrorderno.Length;j++ )
                {
                    lstorderno.Add(arrorderno[j]);
                }
                string strtesttype = arrparitem[1];
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from b in context.P_SFC_ProcessData
                           where lstorderno.Contains(b.order_no) && b.mat_code == null && b.pass == 1
                          join dx in context.P_SFC_State.Where(x=>x.state>0)
                               on b.SFC equals dx.SFC
                          join c in context.B_ProcessStep.Where(x => x.step_code == strtesttype)
                              on b.step_id equals c.step_id
                          orderby b.val
                          select new UserDefineData_SFCProcessData()
                          {
                              order_no=b.order_no,
                              mat_code=b.mat_code,
                              SFC = b.SFC,
                              val = b.val,
                              step_type=b.step_type,
                              step_code=c.step_code,
                              step_name=c.step_name,
                          }).ToList();
                if (res.Count() > 0)
                {
                    string aa = JsonConvert.SerializeObject(res);
                    return aa;
                }
                else
                {
                    strReturn = "GetTestValByOrderAndStepType-Fail,测试项为空";
                }
            }
            catch (Exception exp)
            {
                strReturn = "GetTestValByOrderAndStepType-Fail," + exp.Message;
            }
            return strReturn;
        }
        //根据SFC获取工单，物料，已经下级SFC信息(注意，将产品数量临时赋值给step_code备用，没有其他含义)
        public static string GetSFCInfoLVLBySFC(string json)
        {
            string strReturn = "";
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    strReturn = "GetSFCInfoLVLBySFC-Fail,传入参数为空";
                    return strReturn;
                }

                string []arrstritem = json.Split(',');
                if (arrstritem.Length < 2)
                {
                    strReturn = "GetSFCInfoLVLBySFC-Fail,传入参数有误";
                    return strReturn;
                }
                string strsfc = arrstritem[0];
                string strWorkShopCode = arrstritem[1];
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from b in context.P_SFC_ProcessData
                           where b.SFC == strsfc && b.mat_code != null  && b.pass == 1
                           join pw in context.P_WorkOrder
                                on b.order_no equals pw.order_no
                           where pw.workshop_code == strWorkShopCode
                           join bp in context.B_Product 
                                on pw.product_code equals bp.product_code
                           orderby b.val
                           select new UserDefineData_SFCProcessData()
                           {
                               order_no = b.order_no,
                               mat_code = pw.product_code+"_#_" + bp.product_name,
                               SFC = b.SFC,
                               val = b.val,
                               step_type = "",
                               step_code = SqlFunctions.StringConvert(bp.max_qty),
                               step_name = "",
                           }).ToList();
                if (res.Count() > 0)
                {
                    string aa = JsonConvert.SerializeObject(res);
                    return aa;
                }
                else
                {
                    strReturn = "GetSFCInfoLVLBySFC-OK,当前批次号没有下级批次";
                }
            }
            catch (Exception exp)
            {
                strReturn = "GetSFCInfoLVLBySFC-Fail," + exp.Message;
            }
            return strReturn;
        }
        //根据SFC获取工单，物料信息(注意，将产品数量临时赋值给step_code备用，没有其他含义)
        public static string GetOrderInfoBySFC(string json)
        {
            string strReturn = "";
            try
            {
                if (string.IsNullOrWhiteSpace(json))
                {
                    strReturn = "GetOrderInfoBySFC-Fail,传入参数为空";
                    return strReturn;
                }

                string[] arrstritem = json.Split(',');
                if (arrstritem.Length < 2)
                {
                    strReturn = "GetSFCInfoLVLBySFC-Fail,传入参数有误";
                    return strReturn;
                }
                string strsfc = arrstritem[0];
                string strWorkShopCode = arrstritem[1];
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from b in context.P_SFC_ProcessData
                           where b.SFC == strsfc && b.pass == 1 && b.idx==1
                           join pw in context.P_WorkOrder
                                on b.order_no equals pw.order_no
                           where pw.workshop_code == strWorkShopCode
                           join bp in context.B_Product
                                on pw.product_code equals bp.product_code
                           orderby b.val
                           select new UserDefineData_SFCProcessData()
                           {
                               order_no = b.order_no,
                               mat_code = pw.product_code + "_#_" + bp.product_name,
                               SFC = b.SFC,
                               val = b.val,
                               step_type = "",
                               step_code = SqlFunctions.StringConvert(bp.max_qty),
                               step_name = "",
                           }).ToList();
                if (res.Count() > 0)
                {
                    string aa = JsonConvert.SerializeObject(res);
                    return aa;
                }
                else
                {
                    strReturn = "GetOrderInfoBySFC-Fail,获取工单，物料信息失败";
                }
            }
            catch (Exception exp)
            {
                strReturn = "GetOrderInfoBySFC-Fail," + exp.Message;
            }
            return strReturn;
        }
    }
}