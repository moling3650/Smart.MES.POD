using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using Newtonsoft.Json;
using System.Globalization;
using System.Reflection;


namespace BLL
{
    public class SSW
    {
        //获取工单详细信息
        public static string GetWorkOrderInfo(string data)
        {
            string[] data1 = data.Split(',');
            string cid = data1[0].ToString();
            string Stime = data1[1].ToString();
            string Etime = data1[2].ToString();
            DateTimeFormatInfo dtFormat = new System.Globalization.DateTimeFormatInfo();
            dtFormat.ShortDatePattern = "yyyy/MM/dd";
            DateTime Stime1 = Convert.ToDateTime(Stime, dtFormat);
            DateTime Etime1 = Convert.ToDateTime(Etime, dtFormat);

            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder join c in context.P_SSW_PrintList on a.order_no equals c.order_no into d from e in d.DefaultIfEmpty() join f in context.P_SSW_TemplateList on a.product_code equals f.product_code into j from k in j.DefaultIfEmpty() join m in context.B_Product on a.product_code equals m.product_code into n from p in n.DefaultIfEmpty() where (a.product_code.IndexOf(cid) != -1 && a.input_time >= Stime1 && a.input_time <= Etime1 && a.state == 1) select new { Order_No = a.order_no, product_code = a.product_code, Qty = a.qty, State = e.state == null ? "在制" : "打印完成", InputTime = a.input_time, completed = e.completed == null ? 0 : e.completed, Template_id = k.Template_id, product_name = p.product_name };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //为产品编号下拉框获取数据
        public static string GetProductCode(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product where b.product_code != data select new { Product_Code = b.product_code, Product_Name = b.product_code + "_" + b.product_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //获取产品详情
        public static string GetProductInfo(string data)
        {

            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product where b.product_code != data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //获取产品详情
        public static string GetProduct(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = (from b in context.B_Product where b.product_code == data select b).FirstOrDefault();
            if (var!=null)
            {
                return JsonConvert.SerializeObject(var);
            }
            return null;
        }
        //精确查询
        public static string GetProductByName(string data)
        {

            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product where b.product_name == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //获取工单打印详细信息
        public static string GetPrintInfo(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();

            //var var = from a in context.P_WorkOrder join c in context.P_SSW_PrintList on a.product_code equals c.product_code into d from e in d.DefaultIfEmpty()   where a.product_code == data select new { Order_No = a.order_no, product_code = a.product_code, Qty = e.qty, State = e.state == "1" ? "在制" : "未制", InputTime = e.inputTime, completed = e.completed };
            var var = from a in context.P_WorkOrder
                      join c in context.P_SSW_PrintList on a.order_no
                          equals c.order_no into d
                      from e in d.DefaultIfEmpty()
                      join f in context.P_SSW_TemplateList on a.product_code
                          equals f.product_code into j
                      from k in j.DefaultIfEmpty()
                      join m in context.B_Product on a.product_code
                          equals m.product_code into n
                      from p in n.DefaultIfEmpty()
                      where a.order_no == data
                      select new { Order_No = a.order_no, Qty = System.Data.Objects.SqlClient.SqlFunctions.StringConvert(a.qty / p.max_qty).IndexOf(".") == -1 ? Math.Round(((decimal)((a.qty) / (p.max_qty))), 0) + 1 : a.qty / p.max_qty, completed = e.completed, Template_id = k.Template_id, currentSN = k.currentSN, product_code = a.product_code };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //为模板下拉框获取数据
        public static string GetTemplateList(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_SSW_TemplateList where b.Template_id != data select new { Template_id = b.Template_id, Template_name = b.Template_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //
        public static string GetTemplateInfo(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_SSW_TemplateList where b.Template_id == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //获取Bom_Detail信息
        public static string GetBom_Detail(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Bom_Detail where b.bom_code == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //获取Bom信息
        public static string GetBom(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Bom where b.product_code == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //获取产品类型信息
        public static string GetProductType(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product_Type select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //根据产品类型获取所对应的产品
        public static string GetProductByType(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product where b.type_code==data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //
        public static string GetBomByCode(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Bom join c in context.B_Product on b.product_code equals c.product_code into d from e in d.DefaultIfEmpty() where b.product_code == data && b.enable == 1 select new { bom_code = b.bom_code, product_code = b.product_code, product_name = e.product_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //
        public static string GetBomByNmae(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Bom join c in context.B_Product on b.product_code equals c.product_code into d from e in d.DefaultIfEmpty() where e.product_name == data select new { bom_code = b.bom_code, product_code = b.product_code, product_name = e.product_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //获取Bom信息
        public static string GetBomByBom_code(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Bom where b.bom_code == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //通过product_code获取模板数据
        public static string GetTemplate(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_SSW_TemplateList join c in context.B_Product on b.product_code equals c.product_code into d from e in d.DefaultIfEmpty() where b.product_code == data select new { b.product_code, b.RuleStr, b.Template_id, b.maxSN, b.currentSN, b.inputTime, b.setZero, e.product_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //通过Template_id获取模板数据
        public static string GetTemplateByTemplate_id(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_SSW_TemplateList where b.Template_id == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //保存模板数据
        public static void InsertTemplate(string json)
        {
            LEDAO.P_SSW_TemplateList tem = JsonConvert.DeserializeObject<LEDAO.P_SSW_TemplateList>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_SSW_TemplateList.AddObject(tem);
                context.SaveChanges();
            }
        }
        //修改模板数据
        public static void UpdateTemplate(string data)
        {
            string[] str = data.Split('|');
            string Mid = str[0];
            string json = str[1];
            LEDAO.P_SSW_TemplateList tem = JsonConvert.DeserializeObject<LEDAO.P_SSW_TemplateList>(json);
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_SSW_TemplateList.Where(X => X.Template_id == Mid).FirstOrDefault();
                model.Template_id = tem.Template_id;
                model.Template_name = tem.Template_name;
                model.product_code = tem.product_code;
                model.RuleStr = tem.RuleStr;
                model.inputTime = tem.inputTime;
                model.currentSN = tem.currentSN;
                model.maxSN = tem.maxSN;
                model.setZero = tem.setZero;
                model.zeroDate = tem.zeroDate;
                model.TemplatePath = tem.TemplatePath;
                model.faxType = tem.faxType;
                model.checkCode = tem.checkCode;
                model.printCopies = tem.printCopies;
                model.variable = tem.variable;
                context.SaveChanges();
            }
        }
        //删除模板数据
        public static void DeleteTemplate(string P_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.P_SSW_TemplateList where u.product_code == P_code select u).Single();
                context.P_SSW_TemplateList.DeleteObject(var);
                context.SaveChanges();
            }
        }
        //修改模板表的currentSN  
        public static void UpdateTemplateByMid(string data)
        {
            string[] str = data.Split('|');
            string Mid = str[0];
            int num = Convert.ToInt32(decimal.Parse(str[1]));
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_SSW_TemplateList.Where(X => X.Template_id == Mid).FirstOrDefault();
                model.currentSN = num;
                context.SaveChanges();
            }
        }
        //修改工单表的状态  
        public static void UpdateWorkorder_State(string data)
        {
            string[] str = data.Split('|');
            string Order_No = str[0];
            string state = str[1];
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_SSW_PrintList.Where(X => X.order_no == Order_No).FirstOrDefault();
                model.state = state;
                context.SaveChanges();
            }
        }
        //
        public static string GetPrint_orderNo(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_SSW_PrintList where b.order_no == data select new { completed = b.completed, Product_code = b.product_code, order_no = b.order_no };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //
        public static string GetPrintList1(string data)
        {
            string[] _data = data.Split(',');
            string main_order = _data[0].ToString();
            string val = _data[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = (from b in context.V_WorkOrder_Product where b.order_no == main_order && b.parent_order == "" select b).ToList();
            if (var.Count() > 0)
            {
                V_WorkOrder_Product vtmp = new V_WorkOrder_Product();
                Type t = var[0].GetType();
                FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.Name == ("_" + val.ToLower().ToString()))
                    {
                        return field.GetValue(var[0]).ToString();
                    }

                }
            }
            return null;
        }

        //
        public static string GetPrintList(string data)
        {
            string[] _data = data.Split(',');
            string Order_No = _data[0].ToString();
            string val = _data[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = (from b in context.V_WorkOrder_Product where b.order_no == Order_No select b).ToList();
            if (var.Count() > 0)
            {
                V_WorkOrder_Product vtmp = new V_WorkOrder_Product();
                Type t = var[0].GetType();
                FieldInfo[] fields = t.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
                foreach (FieldInfo field in fields)
                {
                    if (field.Name == ("_" + val.ToLower().ToString()))
                    {
                        if (field.Name != "_input_time")
                        {
                            return field.GetValue(var[0]).ToString();
                        }
                        else
                        {
                            return DateTime.Now.ToString("yyyy-MM-dd hh:mm");
                        }
                    }

                }
            }
            return null;
        }

        //
        public static string GetPrint_ProductCode(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_SSW_PrintList where b.product_code == data select new { Product_code = b.product_code, order_no = b.order_no };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //修改打印表的completed  
        public static void UpdatePrintByOrder_no(string data)
        {
            string[] str = data.Split('|');
            string order_No = str[0];
            int num = Convert.ToInt32(decimal.Parse(str[1]));
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_SSW_PrintList.Where(X => X.order_no == order_No).FirstOrDefault();
                model.completed = num;
                context.SaveChanges();
            }
        }
        //当完成打印数量时修改  P_SSW_PrintList的状态State
        public static void UpdateWorkOderState(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {

                var mod = context.P_SSW_PrintList.Where(X => X.order_no == data).FirstOrDefault();
                mod.state = "1";
                context.SaveChanges();
            }

        }
        //保存打印数据
        public static void InsertPrint(string json)
        {
            LEDAO.P_SSW_PrintList tem = JsonConvert.DeserializeObject<LEDAO.P_SSW_PrintList>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_SSW_PrintList.AddObject(tem);
                context.SaveChanges();
            }
        }
        //删除产品数据
        public static void DeleteProduct(string P_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.B_Product where u.product_code == P_code select u).Single();
                context.B_Product.DeleteObject(var);
                context.SaveChanges();
            }
        }
        //根据产品编号获取工单详细信息--套打
        public static string GetWorkOrderByProduct_code(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder join c in context.P_SSW_PrintList on a.order_no equals c.order_no into d from e in d.DefaultIfEmpty() join f in context.P_SSW_TemplateList on a.product_code equals f.product_code into j from k in j.DefaultIfEmpty() join m in context.B_Product on a.product_code equals m.product_code into p from q in p.DefaultIfEmpty() where (a.product_code == data && a.state == 1) select new { parent_order = a.parent_order, Order_No = a.order_no, product_code = a.product_code, Qty = a.qty, State = e.state == null ? "在制" : "打印完成", InputTime = a.input_time, completed = e.completed == null ? 0 : e.completed, Template_id = k.Template_id, RuleStr = k.RuleStr, product_name = q.product_name };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //根据工单时间获取最近三天工单详细信息
        public static string GetWorkOrderBytime(string data)
        {
            DateTime time = Convert.ToDateTime(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder
                      join c in context.P_SSW_PrintList on a.order_no
                          equals c.order_no into d
                      from e in d.DefaultIfEmpty()
                      join f in context.P_SSW_TemplateList on a.product_code
                          equals f.product_code into j
                      from k in j.DefaultIfEmpty()
                      join m in context.B_Product on a.product_code
                          equals m.product_code into n
                      from p in n.DefaultIfEmpty()
                      where (a.planned_time >= time && (a.state == 0 || a.state == 1))
                      orderby a.planned_time descending
                      select new { Order_No = a.order_no, product_code = a.product_code, Qty = System.Data.Objects.SqlClient.SqlFunctions.StringConvert(a.qty / p.max_qty).IndexOf(".") == -1 ? (int)(Math.Ceiling((decimal)(a.qty / p.max_qty))) : a.qty / p.max_qty, State = e.state == null ? "在制" : "打印完成", InputTime = a.planned_time, completed = e.completed == null ? 0 : e.completed, Template_id = k.Template_id, product_name = p.product_name };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //根据工单id获取工单详细信息
        public static string GetWorkOrderByOrder_id(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder join c in context.P_SSW_PrintList on a.order_no equals c.order_no into d from e in d.DefaultIfEmpty() join f in context.P_SSW_TemplateList on a.product_code equals f.product_code into j from k in j.DefaultIfEmpty() join m in context.B_Product on a.product_code equals m.product_code into n from p in n.DefaultIfEmpty() where (a.order_no == data && (a.state == 0 || a.state == 1)) select new { Order_No = a.order_no, product_code = a.product_code, Qty = a.qty, State = e.state == null ? "在制" : "打印完成", InputTime = a.input_time, completed = e.completed == null ? 0 : e.completed, Template_id = k.Template_id, product_name = p.product_name };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //
        public static string GetWorkOrderByMainOrder_id(string data)
        {
            //DateTime time = Convert.ToDateTime(DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd"));
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder
                      join c in context.P_SSW_PrintList on a.order_no
                          equals c.order_no into d
                      from e in d.DefaultIfEmpty()
                      join f in context.P_SSW_TemplateList on a.product_code
                          equals f.product_code into j
                      from k in j.DefaultIfEmpty()
                      join m in context.B_Product on a.product_code
                          equals m.product_code into n
                      from p in n.DefaultIfEmpty()
                      where (//a.planned_time >= time && 
                      a.main_order.Contains(data) && (a.state == 0 || a.state == 1))
                      orderby a.planned_time descending
                      select new
                      {
                          Order_No = a.order_no,
                          product_code = a.product_code,
                          Qty = System.Data.Objects.SqlClient.SqlFunctions.StringConvert(a.qty / p.max_qty).IndexOf(".") == -1 ? Math.Ceiling((decimal)(a.qty / p.max_qty)) : a.qty / p.max_qty,
                          State = e.state == null ? "在制" : "打印完成",
                          InputTime = a.planned_time,
                          completed = e.completed == null ? 0 : e.completed,
                          Template_id = k.Template_id,
                          product_name = p.product_name
                      };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //根据父工单id获取工单详细信息
        public static string GetWorkOrderByParentOrder_id(string data)
        {
            //string[] strates = "-1,0".ToString().Split(',');
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder join c in context.P_SSW_PrintList on a.order_no equals c.order_no into d from e in d.DefaultIfEmpty() join f in context.P_SSW_TemplateList on a.product_code equals f.product_code into j from k in j.DefaultIfEmpty() join m in context.B_Product on a.product_code equals m.product_code into n from p in n.DefaultIfEmpty() where (a.parent_order == data && (a.state == 0 || a.state == 1)) select new { Order_No = a.order_no, product_code = a.product_code, Qty = a.qty, State = e.state == null ? "在制" : "打印完成", InputTime = a.input_time, completed = e.completed == null ? 0 : e.completed, Template_id = k.Template_id, product_name = p.product_name };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //
        public static string GetWorder(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_WorkOrder where b.order_no.Contains(data) select new { product_code = b.product_code, parent_order = b.parent_order, order_no = b.order_no, main_order = b.main_order };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //
        public static string GetMainOrderByOrderNo(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_WorkOrder where b.order_no == data select new { product_code = b.product_code, parent_order = b.parent_order, order_no = b.order_no, main_order = b.main_order };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //模糊查询，主工单
        public static string GetWorderByMain_order(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_WorkOrder where b.main_order.Contains(data) select new { product_code = b.product_code, parent_order = b.parent_order, order_no = b.order_no, main_order = b.main_order };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //模糊查询，根据产品编号获取产品详情
        public static string GetProductByCode(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product where b.product_code.Contains(data) select new { product_code = b.product_code, product_name=b.product_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //模糊查询，根据产品名称获取产品详情
        public static string GetProductByName1(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product where b.product_name.Contains(data) select new { product_code = b.product_code, product_name = b.product_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //主工单
        public static string GetByMain_order(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_WorkOrder where b.main_order == data select new { product_code = b.product_code, parent_order = b.parent_order, order_no = b.order_no, main_order = b.main_order };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        public static string GetIndent(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder join c in context.P_SSW_PrintList on a.order_no equals c.order_no into d from e in d.DefaultIfEmpty() join f in context.P_SSW_TemplateList on a.product_code equals f.product_code into j from k in j.DefaultIfEmpty() join m in context.B_Product on a.product_code equals m.product_code into n from p in n.DefaultIfEmpty() where (a.CO == data && a.state == 1) select new { Order_No = a.order_no, product_code = a.product_code, Qty = a.qty, State = e.state == null ? "在制" : "打印完成", InputTime = a.input_time, completed = e.completed == null ? 0 : e.completed, Template_id = k.Template_id, product_name = p.product_name };
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //根据产品编号获取工单详细信息--套打
        public static string GetWorkOrderByMain_order(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from a in context.P_WorkOrder
                      join b in context.B_Product on a.product_code equals b.product_code into b_join
                      from b in b_join.DefaultIfEmpty()
                      join c in context.P_SSW_TemplateList on a.product_code equals c.product_code into c_join
                      from c in c_join.DefaultIfEmpty()
                      join d in context.P_SSW_PrintList on a.order_no equals d.order_no into d_Join
                      from d in d_Join.DefaultIfEmpty()
                      where
                        a.main_order == data &&
                        b.print_bind == 1
                      orderby a.parent_order
                      select new
                      {
                          a.order_no,
                          product_code = b.product_code,
                          product_name = b.product_name,
                          a.qty,
                          Template_id = c.Template_id,
                          completed = d.completed == null ? 0 : d.completed
                      };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //保存条码数据
        public static void InsertBarCode(string json)
        {
            LEDAO.P_BarCodeBing tem = JsonConvert.DeserializeObject<LEDAO.P_BarCodeBing>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_BarCodeBing.AddObject(tem);
                context.SaveChanges();
            }
        }
        //根据条码查询数据
        public static string GetBarCode(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_BarCodeBing where b.barcode == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //根据条码查询数据
        public static string GetWorkerOrderbyBar(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = (from b in context.P_BarCodeBing where b.barcode == data select b).Distinct();
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //查询主工单列表
        public static string GetMainOrder(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_MainWorkOrder where b.PrintState == 0 select new { Main_order = b.Main_order, PlanTime = b.PlanTime, PrintState = b.PrintState, Product_code = b.Product_code };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //当完成打印修改  p_MainworkOrder的状态State
        public static void UpdateMainWorkOderState(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var mod = context.P_MainWorkOrder.Where(X => X.Main_order == data).FirstOrDefault();
                mod.PrintState = 1;
                context.SaveChanges();
            }
        }
        //查询产品表的全部数据
        public static string GetProductAll(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.B_Product where b.type_code == data select new { product_code = b.product_code };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //查询P_Print_currentSn
        public static string GetPrint_currentSn(string data)
        {
            string[] rootPrefixSuffix = data.Split(',');
            string prefix = rootPrefixSuffix[0];
            string suffix = rootPrefixSuffix[1];
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = (from b in context.P_Print_CurrentSN where b.prefix == prefix && b.suffix == suffix select b).Union(from b in context.P_Print_CurrentSN where b.prefix == prefix && b.suffix == null select b).Union(from b in context.P_Print_CurrentSN where b.prefix == null && b.suffix == suffix select b).Union(from b in context.P_Print_CurrentSN where b.prefix == null && b.suffix == null select b);
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //保存当前sn的前缀后缀
        public static void InsertPrint_CurrentSn(string json)
        {
            LEDAO.P_Print_CurrentSN tem = JsonConvert.DeserializeObject<LEDAO.P_Print_CurrentSN>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_Print_CurrentSN.AddObject(tem);
                context.SaveChanges();
            }
        }
        //修改P_Print_currentSn的currentSN  
        public static void UpdateP_Print_currentSn(string data)
        {
            string[] str = data.Split('|');
            string prefix = str[0];
            string suffix = str[1];
            int num = Convert.ToInt32(decimal.Parse(str[2]));
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_Print_CurrentSN.Where(X => (X.prefix == prefix && X.suffix == suffix) || (X.prefix == prefix && X.suffix == null) || (X.prefix == null && X.suffix == suffix) || (X.prefix == null && X.suffix == null)).FirstOrDefault();
                model.current_sn = num;
                context.SaveChanges();
            }
        }
        //查询P_Print_currentSn
        public static string GetAlreadyPrint(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = from b in context.P_SSW_PrintList where b.state == "1" && b.Main_order == data select b;
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }

        //删除模板数据
        public static void DeleteNoPrintOrder(string Main_order)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.P_MainWorkOrder where u.Main_order == Main_order select u).Single();
                context.P_MainWorkOrder.DeleteObject(var);
                context.SaveChanges();
            }
        }


        //根据工序和主工单获取子工单
        public static string GetOrderNo(string split)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    string[] Array = split.Split(',');
                    string process_code = Array[0];
                    string main_order = Array[1];
                    var var = (from b in context.V_WorkOrder_Product where b.process_code == process_code && b.main_order == main_order select b).ToList();
                    if (var.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(var.ToList());
                    }
                    return null;
                }


            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}