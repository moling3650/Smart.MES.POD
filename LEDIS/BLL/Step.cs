using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace BLL
{
    public class Step
    {

        /// <summary>
        /// 加载后段工步
        /// </summary>
        /// <param name="PID">PID</param>
        /// <returns></returns>
        public static string GetStepAll(string pid)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    int intpid = Convert.ToInt32(pid);
                    //var res = context.B_ProcessStep.Where(x => x.pid == intpid).OrderBy(x => x.idx).ToList();

                    var res = (from a in context.B_ProcessStep 
                               join b in context.S_DriveList on a.drive_code equals b.drive_code
                               where
                                 a.pid == intpid 
                               select new
                               {
                                   a.step_id,
                                   a.Unit,
                                   a.parameter,
                                   a.parameter2,
                                   a.mat_code,
                                   a.ctrl_type,
                                   a.triger,
                                   a.is_record,
                                   a.idx,
                                   a.auto_restart,
                                   a.autorun,
                                   a.consume_type,
                                   a.consume_percent,
                                   a.allow_reuse,
                                   a.format,
                                   a.time_out,
                                   a.drive_code,
                                   a.type_id,
                                   a.flow_code,
                                   a.step_type,
                                   a.step_name,
                                   a.step_code,
                                   a.process_code,
                                   b.file_name
                               }).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);
                    }
                    return ConResult.GetJsonResult(null, true, null); ;
                    //else
                    //{
                    //    return ConResult.GetJsonResult(null, false, "步骤下没有工步");
                    //}
                }



            }
            catch (Exception ex)
            {
                return ConResult.GetJsonResult(null, false, ex.ToString());
            }
        }

        /// <summary>
        /// 加载前段工步
        /// </summary>
        /// <param name="process">工艺</param>
        /// <returns></returns>
        public static string GetPreStepAll(string process)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    //var res = context.B_PreStep.Where(x => x.process_code == process).OrderBy(x => x.idx);

                    var res = (from a in context.B_PreStep
                               join b in context.S_DriveList on a.drive_code equals b.drive_code
                               where
                                 a.process_code == process
                               select new
                               {
                                   a.IsKeySteps,
                                   a.Unit,
                                   a.parameter2,
                                   a.triger,
                                   a.is_record,
                                   a.idx,
                                   a.auto_restart,
                                   a.autorun,
                                   a.allow_reuse,
                                   a.format,
                                   a.time_out,
                                   a.parameter,
                                   a.drive_code,
                                   a.type_id,
                                   a.flow_code,
                                   a.step_type,
                                   a.step_name,
                                   a.step_code,
                                   a.process_code,
                                   a.pre_id,
                                   b.file_name
                               }).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);
                    }
                    else
                    {
                        return ConResult.GetJsonResult(null, false, null);
                    }
                }



            }
            catch (Exception ex)
            {
                return ConResult.GetJsonResult(null, false, ex.ToString());
            }
        }

        /// <summary>
        /// 获取mat_code
        /// </summary>
        /// <param name="matcode"></param>
        /// <returns></returns>
        public static string GetMatCodebyName(string matcode)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                string str = context.V_ProductMaterial.Where(c => c.p_code == matcode).Select(c => c.p_name).First();
                return str;
            }
            catch (Exception)
            {

                return "";
            }
        }
        /// <summary>
        /// 获取工步名称
        /// </summary>
        /// <param name="matcode"></param>
        /// <returns></returns>
        public static string GetStepNmae(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                string _step_code = Arrdata[0];
                string _product_code = Arrdata[1];
                var context = LEDAO.APIGateWay.GetEntityContext();
                string str = context.V_Step_Flow.Where(c => c.step_code == _step_code && c.product_code == _product_code).Select(c => c.step_name).First();
                return str;
            }
            catch (Exception)
            {

                return "";
            }
        }
        /// <summary>
        /// 加载子工步
        /// </summary>
        /// <param name="pid"></param>
        /// <returns></returns>
        public static string GetSonStepAll(string json)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    string[] data = json.Split(',');
                    string step_code = data[0].ToString();
                    string flow_code = data[1].ToString();
                    var res = context.B_ProcessSonStep.Where(x => x.parentstepid == step_code && x.flow_code == flow_code).OrderBy(x => x.idx).ToList();
                    if (res.Count() > 0)
                    {
                        return ConResult.GetJsonResult(res, true, null);
                    }
                    return ConResult.GetJsonResult(null, true, null);
                }
            }
            catch (Exception ex)
            {
                return ConResult.GetJsonResult(null, false, ex.ToString());
            }
        }
        //补料保存数据
        public static void SubSonStepData(string _json)
        {
            try
            {
                using (var context = LEDAO.APIGateWay.GetEntityContext())
                {
                    string[] json = _json.Split(',');
                    LEDAO.P_SFC_ProcessSonData sonStep = new LEDAO.P_SFC_ProcessSonData();
                    sonStep.orderno = json[0].ToString();
                    sonStep.sfc = json[1].ToString();
                    sonStep.parentstepid = json[2].ToString();
                    sonStep.stepid = json[3].ToString();
                    sonStep.stepval = json[4].ToString();
                    sonStep.stepconclude = json[5].ToString();
                    sonStep.isfedbatch = 1;
                    sonStep.inputtime = DateTime.Now;
                    context.P_SFC_ProcessSonData.AddObject(sonStep);
                    context.SaveChanges();
                }
            }
            catch (Exception e)
            {
                string err = e.Message;
                return;
            }
        }
    }
}