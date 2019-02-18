using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BLL;
using Newtonsoft.Json;

namespace BLL
{
    public class BarCodeBing
    {
        /// <summary>
        /// GetSFClstbyOrderNo 通过工单获取sfc条码
        /// </summary>
        /// <param name="orderNo">工单</param>
        /// <returns></returns>
        public static string GetSFClstbyOrderNo(string orderNo)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    var res = from b in context.P_BarCodeBing orderby b.barcode where (b.order==orderNo)
                      select new 
                      {
                          order=b.order,
                          barcode=b.barcode
                      };
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);   //返回P_BarCodeBing列表
                    }
                    else
                    {
                        return ConResult.GetJsonResult(null, false, "当前工单[" + orderNo+ "]下未找到批次号");
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// GetBarCodeBingstbySFC 通过sfc获取sfc及下级sfc属性
        /// </summary>
        /// <param name="orderNo">sfc</param>
        /// <returns></returns>
        public static string GetBarCodeBingstbySFC(string parstrsfc)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = from b in  context.P_BarCodeBing
                                where b.barcode == parstrsfc
                            join c in  context.P_BarCodeBing
                                on b.main_order equals c.main_order
                              select new{
                                  order=c.order,
                                  parent_order= c.parent_order,
                                  main_order=c.main_order,
                                  barcode=c.barcode,
                                  product_code=c.product_code
                              };
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }  
            }
            catch (Exception)
            {
                //throw;
            }
            return null;
        }
    }
}