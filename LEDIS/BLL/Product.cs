using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using Newtonsoft.Json;
using ILE;

namespace BLL
{
    public class Product
    {

        //获取产品的批次量上限.
        public static string GetProductLotCount(string order)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_WorkOrder
                       join b in context.B_Product on new { product_code = a.product_code } equals new { product_code = b.product_code }
                       where
                         a.order_no == order
                       select new
                       {
                           b.product_code,
                           b.max_qty
                       }).ToList();

            if (res.Count < 1)
            {
                return null;
            }

            return res[0].max_qty.ToString();
        }

        /// <summary>
        /// 通过工单获取工艺、批次最大数、工单数，工单
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string GetProductFlow(string order)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    var res = context.V_ProductFlow.Where(x => x.order_no == order).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res.First(), true, null);
                    }
                    else
                    {
                        return ConResult.GetJsonResult(null, false, "不存在此工单号");
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 根据产品编码获取产品详情
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        public static string GetproductInfoByCode(string Product_code)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.B_Product.Where(X => X.product_code == Product_code) select a).FirstOrDefault();
       
            return JsonConvert.SerializeObject(res);

        }
        /// <summary>
        /// 在Product表中通过产品编码获取批次上限
        /// </summary>
        /// <returns></returns>
        public static string GetMaxQtyByProductCode(string Product_code)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.B_Product.Where(X => X.product_code == Product_code) select new { product_code = a.product_code, max_qty = a.max_qty }).ToList();
            if (res.Count > 0)
            {
                return res[0].max_qty.ToString();
            }
            return null;
        }
        /// <summary>
        /// 在Product表中通过产品编码获取批次上限
        /// </summary>
        /// <returns></returns>
        public static string GetWip_validByProductCode(string Product_code)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.V_ProductMaterial.Where(X => X.p_code == Product_code) select new { wip_valid = a.wip_valid }).ToList();
            if (res.Count > 0)
            {
                return res[0].wip_valid.ToString();
            }
            return null;
        }
        /// <summary>
        /// 在Product表中通过产品编码获取产品条码规则
        /// </summary>
        /// <returns></returns>
        public static string GetRuleByProductCode(string Product_code)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            //var res = (from a in context.B_Product.Where(X => X.product_code == Product_code) select new { code_rule = a.code_rule }).ToList();
            var res=(from a in context.B_Product where a.product_code==Product_code select a).FirstOrDefault();

            if (res!=null)
            {
                return res.code_rule.ToString();
            }
            return null;
        }
        /// <summary>
        /// 根据工单关联查询P_WorkOrder，B_Product，B_Product_Type 等到对应的产品信息，产品类别
        /// </summary>
        /// <returns></returns>
        public static string GetProductInfoByOrder(string strparOrder)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.V_Order_Produc_Type_Name.Where(X => X.order_no == strparOrder) select a).ToList();
            if (res.Count() > 0)
            {
                return JsonConvert.SerializeObject(res);
            }
            return null;
        }
        /// <summary>
        /// 根据产品类别获取产品级别
        /// </summary>
        /// <returns></returns>
        public static string GetProductGradeByType(string parstrProductTypeCode)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.B_Product_Grade.Where(X => X.type_code == parstrProductTypeCode) orderby (a.idx) select a).ToList();
            if (res.Count() > 0)
            {
                return JsonConvert.SerializeObject(res);
            }
            return null;
        }
        /// <summary>
        /// 根据产品类别获取待判编码
        /// </summary>
        /// <returns></returns>
        //public static string GetPendingJudgmentByType(string parstrProductTypeCode)
        //{
        //    var context = LEDAO.APIGateWay.GetEntityContext();
        //    var res = (from a in context.B_Judge_code.Where(X => X.typecode == parstrProductTypeCode) orderby (a.id) select a).ToList();
        //    if (res.Count() > 0)
        //    {
        //        return JsonConvert.SerializeObject(res);
        //    }
        //    return null;
        //}
        //获取物料qty
        public static string GetMaterialQty(string data)
        {
            string[] Arrdata = data.Split(',');
            string mat_code = Arrdata[0].ToString();
            string Product_code = Arrdata[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.V_Bom_Detail
                       where a.mat_code == mat_code &&
    a.product_code == Product_code
                       select new
                       {
                           a.qty,
                       }).ToList();

            if (res.Count < 1)
            {
                return null;
            }
            return res[0].qty.ToString();
        }
        //获取物料基数
        public static string GetMaterialbaseQty(string data)
        {
            string[] Arrdata = data.Split(',');
            string mat_code = Arrdata[0].ToString();
            string Product_code = Arrdata[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.V_Bom_Detail
                       where a.mat_code == mat_code &&
    a.product_code == Product_code
                       select new
                       {
                           a.base_qty,
                       }).ToList();

            if (res.Count < 1)
            {
                return null;
            }
            return res[0].base_qty.ToString();
        }
        //获取物料是否允许超越
        public static string GetMatIsEnable(string data)
        {
            string[] Arrdata = data.Split(',');
            string mat_code = Arrdata[0].ToString();
            string Product_code = Arrdata[1].ToString();
            int Bom_id = int.Parse(Arrdata[2].ToString());
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.V_Bom_Detail
                       where a.mat_code == mat_code &&
    a.product_code == Product_code && a.enable == 1 && a.bom_id == Bom_id
                       select new
                       {
                           a.enable_beyond
                       }).ToList();

            if (res.Count < 1)
            {
                return null;
            }
            return res[0].enable_beyond.ToString();
        }
        /// <summary>
        /// 获取BOM_id
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetBomByFlow(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.B_Process_Flow
                       where a.flow_code == data
                       select new
                       {
                           a.bom_id
                       }).ToList();

            if (res.Count < 1)
            {
                return null;
            }
            return res[0].bom_id.ToString();
        }
    }
}