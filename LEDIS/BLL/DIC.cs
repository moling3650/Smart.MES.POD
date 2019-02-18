using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;
using System.Data;
using System.Text.RegularExpressions;
using JsonTools;

namespace BLL
{
    public class DIC
    {
        //将获取的Json转换为DateTable
        public static DataTable JsonToTable(string StrJson)
        {
            //转换json格式
            StrJson = StrJson.Replace(",\"", "*\"").Replace("\":", "\"#").ToString();
            //取出表名   
            var rg = new Regex(@"(?<={)[^:]+(?=:\[)", RegexOptions.IgnoreCase);
            string strName = rg.Match(StrJson).Value;
            DataTable tb = null;
            //去除表名   
            StrJson = StrJson.Substring(StrJson.IndexOf("[") + 1);
            StrJson = StrJson.Substring(0, StrJson.IndexOf("]"));
            //获取数据   
            rg = new Regex(@"(?<={)[^}]+(?=})");
            MatchCollection mc = rg.Matches(StrJson);
            for (int i = 0; i < mc.Count; i++)
            {
                string strRow = mc[i].Value;
                string[] strRows = strRow.Split('*');
                //创建表   
                if (tb == null)
                {
                    tb = new DataTable();
                    tb.TableName = strName;
                    foreach (string str in strRows)
                    {
                        var dc = new DataColumn();
                        string[] strCell = str.Split('#');
                        if (strCell[0].Substring(0, 1) == "\"")
                        {
                            int a = strCell[0].Length;
                            dc.ColumnName = strCell[0].Substring(1, a - 2);
                        }
                        else
                        {
                            dc.ColumnName = strCell[0];
                        }
                        tb.Columns.Add(dc);
                    }
                    tb.AcceptChanges();
                }
                //增加内容   
                DataRow dr = tb.NewRow();
                for (int r = 0; r < strRows.Length; r++)
                {
                    dr[r] = strRows[r].Split('#')[1].Trim().Replace("，", ",").Replace("：", ":").Replace("\"", "");
                }
                tb.Rows.Add(dr);
                tb.AcceptChanges();
            }
            return tb;
        }
        //测试API1
        //public static string InsertDate(string json)
        //{
        //    string StrJson = json.Replace("\\", "\\\\");
        //    DataTable tb = JsonToTable(StrJson);
        //    string order=tb.Rows[0]["order_no"].ToString();
        //    //判断获取数据在本地数据库是否存在
        //    var context1 = LEDAO.APIGateWay.GetEntityContext();
        //    var var = from b in context1.P_SSW_PrintList_test where b.order_no == order select b;
        //    string isAdd = null;
        //    if (var.Count() > 0)
        //    {
        //        isAdd = "2";
        //    }
        //    else
        //    {
        //        isAdd = "1";
        //    }
        //    //根据获取数据生成新的Json
        //    P_SSW_PrintList_test obj = new P_SSW_PrintList_test();
        //    obj.order_no = tb.Rows[0]["order_no"].ToString();
        //    obj.inputTime = Convert.ToDateTime(tb.Rows[0]["inputTime"].ToString());
        //    obj.product_code = tb.Rows[0]["product_code"].ToString();
        //    obj.product_name = tb.Rows[0]["product_name"].ToString();
        //    obj.qty = Convert.ToDecimal(tb.Rows[0]["qty"].ToString());
        //    obj.state = tb.Rows[0]["state"].ToString();
        //    obj.Template_id = tb.Rows[0]["Template_id"].ToString();
        //    if (tb.Rows[0]["finishTime"].ToString() == "")
        //    {
        //        obj.finishTime = null;
        //    }
        //    else
        //    {
        //        obj.finishTime = Convert.ToDateTime(tb.Rows[0]["finishTime"].ToString());
        //    } 
        //    obj.completed = Convert.ToInt32(tb.Rows[0]["completed"].ToString());
        //    string strJson1 = JsonToolsNet.ObjectToJson(obj);
        //    //添加或修改
        //    string STR = "";
        //    //添加
        //    if (isAdd == "1")
        //    {
        //        LEDAO.P_SSW_PrintList_test tem = JsonConvert.DeserializeObject<LEDAO.P_SSW_PrintList_test>(strJson1);
        //        var context = LEDAO.APIGateWay.GetEntityContext();
        //        using (context)
        //        {
        //            context.P_SSW_PrintList_test.AddObject(tem);
        //            context.SaveChanges();
        //        }
        //        STR = "1";
        //    }
        //    //修改
        //    if (isAdd == "2")
        //    {
        //        LEDAO.P_SSW_PrintList_test tem = JsonConvert.DeserializeObject<LEDAO.P_SSW_PrintList_test>(strJson1);
        //        using (var context2 = LEDAO.APIGateWay.GetEntityContext())
        //        {
        //            string No = tb.Rows[0]["order_no"].ToString();
        //            var model = context2.P_SSW_PrintList_test.Where(X => X.order_no == No).FirstOrDefault();
        //            model.Template_id = tem.Template_id;
        //            model.inputTime = tem.inputTime;
        //            model.product_code = tem.product_code;
        //            model.order_no = tem.order_no;
        //            model.product_name = tem.product_name;
        //            model.qty = tem.qty;
        //            model.state = tem.state;
        //            model.finishTime = tem.finishTime;
        //            model.completed = tem.completed;
        //            context2.SaveChanges();
        //        }
        //        STR = "2";
        //    }
        //    return STR;
        //}
        //工单同步
        public static string WorkOrderSyncAPI(string json)
        {
            string StrJson = json.Replace("\\", "\\\\");
            DataTable tb = JsonToTable(StrJson);
            string order = tb.Rows[0]["FBillNo"].ToString();
            //判断获取数据在本地数据库是否存在
            var context1 = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context1.P_WorkOrder where b.order_no == order select b;
            string isAdd = null;
            if (var.Count() > 0)
            {
                isAdd = "2";
            }
            else
            {
                isAdd = "1";
            }
            //根据获取数据生成新的Json
            P_WorkOrder obj = new P_WorkOrder();
            obj.order_no = tb.Rows[0]["FBillNo"].ToString();
            obj.input_time = Convert.ToDateTime(tb.Rows[0]["FCheckDate"].ToString());
            obj.planned_time = Convert.ToDateTime(tb.Rows[0]["FPlanCommitDate"].ToString());
            obj.product_code = tb.Rows[0]["FItemID"].ToString();
            obj.qty = Convert.ToDecimal(tb.Rows[0]["FQty"].ToString());
            obj.parent_order = tb.Rows[0]["FParentInterID"].ToString();
            obj.CO = tb.Rows[0]["FOrderInterID"].ToString();
            obj.state =Convert.ToInt32( tb.Rows[0]["FStatus"].ToString());
            obj.flow_code = tb.Rows[0]["FRoutingID"].ToString();
            string strJson1 = JsonToolsNet.ObjectToJson(obj);
            //添加或修改
            string STR = "";
            //添加
            if (isAdd == "1")
            {
                LEDAO.P_WorkOrder tem = JsonConvert.DeserializeObject<LEDAO.P_WorkOrder>(strJson1);
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    context.P_WorkOrder.AddObject(tem);
                    context.SaveChanges();
                }
                STR = "1";
            }
            //修改
            if (isAdd == "2")
            {
                LEDAO.P_WorkOrder tem = JsonConvert.DeserializeObject<LEDAO.P_WorkOrder>(strJson1);
                using (var context2 = LEDAO.APIGateWay.GetEntityContext())
                {
                    string No = tb.Rows[0]["FBillNo"].ToString();
                    var model = context2.P_WorkOrder.Where(X => X.order_no == No).FirstOrDefault();
                    model.order_no = tem.order_no;
                    model.input_time = tem.input_time;
                    model.product_code = tem.product_code;
                    model.order_no = tem.order_no;
                    model.qty = tem.qty;
                    model.state = tem.state;
                    model.planned_time = tem.planned_time;
                    model.parent_order = tem.parent_order;
                    model.CO = tem.CO;
                    model.property = tem.property;
                    model.urgency = tem.urgency;
                    model.flow_code = tem.flow_code;
                    context2.SaveChanges();
                }
                STR = "2";
            }
            return STR;
        }
    }
}