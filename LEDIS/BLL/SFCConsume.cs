using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BLL
{
    public class SFCConsume
    {
        //获取物料信息，自动
        public static string GetMaterialConsumptionAuto(string str)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string[] strArray = str.Split(',');
            try
            {
                using (context)
                {
                    var res = context.P_GetMaterialConsumptionAuto(strArray[0], strArray[1], strArray[2]).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);
                    }
                    else
                    {
                        return ConResult.GetJsonResult(null, false, "该工站没有物料或工单"); 
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        //第一次验证，判断该物料是否存在
        public static string GetMaterialConsumptionManual(string str)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string[] strArray = str.Split(',');
            try
            {
                using (context)
                {
                    var res = context.P_GetMaterialConsumptionManual(strArray[0], strArray[1], strArray[2], strArray[3]).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);
                    }
                    else
                    {
                        return ConResult.GetJsonResult(null, false, "该物料[" + strArray[3] + "]不存在");
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        //第二次验证，判断是否已使用
        public static string GetMaterialConsumptionManual1(string str)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string[] strArray = str.Split(',');
            try
            {
                using (context)
                {
                    var res = context.P_GetMaterialConsumptionManual1(strArray[0], strArray[1], strArray[2], strArray[3]).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);
                    }
                    else
                    {
                        return ConResult.GetJsonResult(null, false, "该物料[" + strArray[3] + "]已被使用");
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        //第三次验证，判断该物料是否存在于该车间
        public static string GetMaterialConsumptionManual2(string str)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string[] strArray = str.Split(',');
            try
            {
                using (context)
                {
                    var res = context.P_GetMaterialConsumptionManual2(strArray[0], strArray[1], strArray[2], strArray[3], strArray[4]).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);
                    }
                    else
                    {
                        return ConResult.GetJsonResult(null, false, "该物料[" + strArray[3] + "]不属于[" + strArray[4] + "]车间");
                    }
                }


            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetSFC(string str)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string[] strArray = str.Split(',');
            try
            {
                using (context)
                {
                    var res = context.P_GetSFC(strArray[0], strArray[1]).ToList();
                    if (res.Count() > 0)
                    {
                        return JsonConvert.SerializeObject(res);
                    }
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        //通过批次获取电压(DY)
        public static string GetDYValbySFC(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string step_code1 = "DY";
            var var = from b in context.V_ProcessData_Step where (b.SFC == SFC && b.step_code == step_code1 && b.pass == 1) select new { val = b.val, SFC = b.SFC, step_name = b.step_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //通过批次获取电压(DY1)
        public static string GetDY1ValbySFC(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string step_code1 = "DY1";
            var var = from b in context.V_ProcessData_Step where (b.SFC == SFC && b.step_code == step_code1 && b.pass == 1) select new { val = b.val, SFC = b.SFC, step_name = b.step_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //通过批次获取内阻(NZ)
        public static string GetNZValbySFC(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string step_code1 = "NZ";
            var var = from b in context.V_ProcessData_Step where (b.SFC == SFC && b.step_code == step_code1 && b.pass == 1) select new { val = b.val, SFC = b.SFC, step_name = b.step_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
        //通过批次获取内阻(NZ1)
        public static string GetNZ1ValbySFC(string SFC)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            string step_code1 = "NZ1";
            var var = from b in context.V_ProcessData_Step where (b.SFC == SFC && b.step_code == step_code1 && b.pass == 1) select new { val = b.val, SFC = b.SFC, step_name = b.step_name };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;
        }
       //
        public static string GetMaterialOrProductMbm(string matCode)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var var = (from b in context.V_ProductMaterial where (b.p_code ==matCode ) select new { mbm = b.mbm }).ToList();
            if (var.Count() > 0)
            {
                return var[0].mbm.ToString();
            }
            return null;
        }
    }
}