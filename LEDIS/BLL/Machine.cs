using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace BLL
{
    public class Machine
    {
        /// <summary>
        /// 获取工位下的设备
        /// </summary>
        /// <param name="station_code"></param>
        /// <returns></returns>
        public static string GetMachineInfo(string station_code)
        {
            //V_Station_Machine
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.V_Station_Machine
                           where a.station_code == station_code
                           && a.state != null
                           select new
                           {
                               a.model_code,
                               a.machine_code,
                               a.machine_name,
                               a.machine_state,
                               a.state,
                               a.expect_nexttime,
                               a.processingQty,
                               a.maintain_cycle,
                               a.img
                           }).FirstOrDefault();
                
                    return JsonConvert.SerializeObject(res);
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="station_code"></param>
        /// <returns></returns>
        public static string GetDataInfo(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                string station_code = Arrdata[0].ToString();
                string ip = Arrdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.V_Station_Shop
                           where a.station_code == station_code && a.ip_address == ip
                           select new
                           {
                               a.process_code,
                               a.station_code,
                               a.ws_code
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取报障数据
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineExceptionData(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Machine_Exception
                           where a.machine_code == machine_code && a.state == 1
                           select new
                           {
                               a.faultphenomenon_code,
                               a.exception_code,
                               a.state,
                               a.submit_person,
                               a.submit_time,
                               a.isdelete,
                               a.grade,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 根据设备编号获取设备状态
        /// </summary>
        /// <param name="station_code"></param>
        /// <returns></returns>
        public static string GetMachine(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.V_Station_Machine
                           where a.machine_code == machine_code
                           select new
                           {
                               a.type_id,
                               a.machine_state
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取设备当前安装的模具
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineInstallMoulds(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.V_MachineMouldInstall
                           where
                             a.machine_code == machine_code
                           select a).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取故障原因
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineFaultphenomenon(string model)
        {
            try
            {

                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.B_Machine_Faultphenomenon
                           where a.model == model
                           select new
                           {
                               a.faultphenomenon_code,
                               a.faultphenomenon_name
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取关机原因
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineOFFFaultreason(string model)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.B_Machine_Stopreason
                           select new
                           {
                               a.stopreason_code,
                               a.stopreason_name
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取设备关联模具类别
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineMouldKinds(string machineCode)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                //var res = (from a in context.B_Machine
                //           join b in context.B_Machine_Model on a.model_code equals b.model_code
                //           join c in context.B_Machine_Kinds on new { kind_id = (Int32)b.kind_id } equals new { kind_id = c.kind_id }
                //           join d in context.B_Machine_Mould_Kinds on new { kind_id = c.kind_id } equals new { kind_id = (Int32)d.machine_kind_id }
                //           join e in context.B_Mould_Kinds on new { mould_kind_id = (Int32)d.mould_kind_id } equals new { mould_kind_id = e.kind_id }
                //           where
                //             a.machine_code == machineCode
                //           select new
                //           {
                //               a.machine_code,
                //               a.machine_name,
                //               b.model_code,
                //               kind_id = (Int32?)c.kind_id,
                //               c.kind_name,
                //               mould_kind_id = (Int32?)d.mould_kind_id,
                //               mould_kind_name = e.kind_name,
                //               d.qty
                //           }).ToList();
                //if (res.Count() > 0)
                //{
                //    return JsonConvert.SerializeObject(res.ToList());
                //}

                var res = (from a in context.B_Process_Mould_List
                join b in context.B_Mould_Model on a.model_code equals b.model_code
                join c in context.B_Mould_Kinds on b.kind_id equals c.kind_id
                select new {
                  a.model_code,
                  b.manufacturer,
                  mould_kind_id = (Int32?)b.kind_id,
                  mould_kind_name = c.kind_name,
                  a.qty
                  }).ToList(); 
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
                
            catch (Exception ex)
            {
                return "EXCEPTION:"+ex.Message;
            }
            return null;
        }

        /// <summary>
        /// 查询关机数据
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static string GetMachineStopRecords(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Machine_StopRecords
                           where a.machine_code == machine_code
                           select new
                           {
                               a.machine_code,
                               a.endtime,
                               a.starttime,
                               a.stop_reason,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 全开
        /// </summary>
        /// <param name="machine_code"></param>
        public static void UpdateMachineStateOpen(string machine_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.B_Machine.Where(X => X.machine_code == machine_code).ToList();
                foreach (var mod in model)
                {
                    mod.state = 1;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 全关
        /// </summary>
        /// <param name="machine_code"></param>
        public static void UpdateMachineStateClose(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] arrdata = data.Split(',');
                string machine_code = arrdata[0].ToString();
                int stop_state = int.Parse(arrdata[1].ToString());
                var model = context.B_Machine.Where(X => X.machine_code == machine_code).ToList();
                foreach (var mod in model)
                {
                    mod.state = stop_state;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 修改设备状态
        /// </summary>
        /// <param name="machine_code"></param>
        public static void UpdateMachineState(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] arrdata = data.Split(',');
                string machine_code = arrdata[0].ToString();
                int state = int.Parse(arrdata[1].ToString());
                var model = context.B_Machine.Where(X => X.machine_code == machine_code).FirstOrDefault();
                if (model != null)
                {
                    model.state = state;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 添加关机数据（关机时）
        /// </summary>
        /// <param name="machine_code"></param>
        public static void AddMachineStopRecordsClose(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] Arrdata = data.Split(',');
                string machine_code = Arrdata[0].ToString();
                string Faultreason_code = Arrdata[1].ToString();
                int machine_state = int.Parse(Arrdata[2].ToString());
                var mod = context.P_Machine_StopRecords.Where(X => X.machine_code == machine_code).ToList();
                P_Machine_StopRecords StopRecords = new P_Machine_StopRecords();
                if (machine_state == 1)
                {
                    StopRecords.machine_code = machine_code;
                    StopRecords.starttime = DateTime.Now;
                    StopRecords.stop_reason = Faultreason_code;
                }
                else
                {
                    StopRecords.machine_code = machine_code;
                    StopRecords.starttime = DateTime.Now;
                }
                context.P_Machine_StopRecords.AddObject(StopRecords);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 修改关机数据（关机时）
        /// </summary>
        /// <param name="machine_code"></param>
        public static void UpdateMachineStopRecordsClose(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] Arrdata = data.Split(',');
                string machine_code = Arrdata[0].ToString();
                string Faultreason_code = Arrdata[1].ToString();
                int machine_state = int.Parse(Arrdata[2].ToString());
                var model = context.P_Machine_StopRecords.Where(X => X.machine_code == machine_code).ToList();
                foreach (var mod in model)
                {
                    if (machine_state == 1)
                    {
                        mod.starttime = DateTime.Now;
                        mod.stop_reason = Faultreason_code;
                    }
                    else
                    {
                        mod.endtime = DateTime.Now;
                    }
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 上报故障记录
        /// </summary>
        /// <param name="json"></param>
        public static void AddMachineException(string json)
        {
            LEDAO.P_Machine_Exception tem = JsonConvert.DeserializeObject<LEDAO.P_Machine_Exception>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_Machine_Exception.AddObject(tem);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 添加设备正在进行的工单
        /// </summary>
        /// <param name="json"></param>
        public static void AddMachineOrder(string json)
        {
            LEDAO.P_Order_Machine tem = JsonConvert.DeserializeObject<LEDAO.P_Order_Machine>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.ExecuteStoreCommand("delete p_order_machine where machine_code=@machine",new SqlParameter[]{new SqlParameter("@machine",tem.machine_code) });
                context.P_Order_Machine.AddObject(tem);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 获取上报故障记录
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineException(Guid exception_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Machine_Exception
                           where a.exception_code == exception_code
                           select new
                           {
                               a.machine_code,
                               a.faultphenomenon_code,
                               a.grade,
                               a.remarks,
                               a.state,
                               a.submit_person,
                               a.submit_time
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下的备件
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineSparepart(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                string Sparepart_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.B_Sparepart
                           where a.sparepart_code == Sparepart_code && a.ofmachine == machine_code
                           select new
                           {
                               a.ofmachine,
                               a.model,
                               a.sparepart_code,
                               a.sparepart_name,
                               a.type_id,
                               a.ws_code,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        ///获取设备在某批次中工单数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMachineOrder(string data)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Order_Machine
                           where a.machine_code == data
                           select new
                           {
                               a.main_order
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }

        /// <summary>
        /// 获取设备下的配件型号(单个)
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineAccessory(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                int type_id = Convert.ToInt32(Arrdata[0].ToString());
                string machine_code = Arrdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.B_Machine_Accessories
                           where a.accessory_type == type_id && a.machine_code == machine_code
                           select new
                           {
                               a.accessory_count,
                               a.machine_code,
                               a.accessory_code,
                               a.accessory_type,
                               a.description,
                               a.accessory_isload,
                               a.accessory_state,
                           }).ToList();
                
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下的配件(全部)
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryAll(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.V_Machine_Accessory
                           where a.machine_code == machine_code
                           //&& a.accessory_state == 0
                           orderby a.accessory_type
                           select new
                           {
                               a.accessory_name,
                               a.machine_code,
                               a.accessory_code,
                               a.accessory_type,
                               a.description,
                               a.accessory_isload,
                               a.accessory_count,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备所有的配件型号
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryAllType(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.V_Machnie_Accessory_type
                           where a.machine_code == machine_code
                           //&& a.accessory_state == 0
                           orderby a.accessory_type
                           select new
                           {
                               a.type_name,
                               a.type_id,
                               a.machine_code,
                               a.accessory_code,
                               a.accessory_type,
                               a.description,
                               a.accessory_isload,
                               a.accessory_count,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取配件信息
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetAccessory(string Accessory_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.B_Accessories
                           where a.accessory_code == Accessory_code
                           select new
                           {
                               a.state,
                               a.accessory_code,
                               a.accessory_name,
                               a.maintain_cycle,
                               a.maintain_nexttime,
                               a.maintain_quality,
                               a.maintian_type,
                               a.manufacturer,
                               a.Userqty,
                               a.ws_code,
                               a.description,
                               a.type_id,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        public static string GetAccessoryInfo(string Accessory_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.V_Accesssory_type
                           where a.accessory_code == Accessory_code
                           select new
                           {
                               a.accessory_code,
                               a.accessory_name,
                               a.type_id,
                               a.type_name,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下的备件，卸载的
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineSparepartRecords_1(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                string Sparepart_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Sparepart_Records
                           where a.sparepart_code == Sparepart_code && a.machine_code == machine_code && a.type == 1 && a.state == 0
                           select new
                           {
                               a.sparepart_code,
                               a.machine_code,
                               a.type,
                               a.loadtime,
                               a.downloadtime,
                               a.operate_person,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下的配件，卸载的
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryRecords_1(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                string Accessory_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Accessory_Records
                           where a.Accessory_code == Accessory_code && a.machine_code == machine_code && a.type == 1 && a.state == 0
                           select new
                           {
                               a.Accessory_code,
                               a.machine_code,
                               a.type,
                               a.loadtime,
                               a.downtime,
                               a.Accessory_preson,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 开机
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineStopRecords_1(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Machine_StopRecords
                           where a.machine_code == machine_code && a.type == 0 && a.state == 0
                           select new
                           {
                               a.process_code,
                               a.machine_code,
                               a.type,
                               a.starttime,
                               a.endtime,
                               a.operate_person,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 关机
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetMachineStopRecords_0(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Machine_StopRecords
                           where a.machine_code == machine_code && a.type == 1 && a.state == 1
                           select new
                           {
                               a.process_code,
                               a.machine_code,
                               a.type,
                               a.starttime,
                               a.endtime,
                               a.operate_person,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 根据关机原因获取关机状态
        /// </summary>
        /// <param name="machine_code"></param>
        /// <returns></returns>
        public static string GetStopreason(string stopreason_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.B_Machine_Stopreason
                           where a.stopreason_code == stopreason_code
                           select new
                           {
                               a.stopreason_code,
                               a.stopreason_name,
                               a.stopreason_type,
                               a.stop_state,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下的备件，装载的
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineSparepartRecords_0(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                string Sparepart_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Sparepart_Records
                           where a.sparepart_code == Sparepart_code && a.machine_code == machine_code && a.type == 0 && a.state == 1
                           select new
                           {
                               a.sparepart_code,
                               a.machine_code,
                               a.type,
                               a.loadtime,
                               a.downloadtime,
                               a.operate_person,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下的配件，装载的
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryRecords_0(string data)
        {
            try
            {
                string[] Arrdata = data.Split(',');
                string Accessory_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Accessory_Records
                           where a.Accessory_code == Accessory_code && a.machine_code == machine_code && a.type == 0 && a.state == 1
                           select new
                           {
                               a.Accessory_code,
                               a.machine_code,
                               a.type,
                               a.loadtime,
                               a.downtime,
                               a.Accessory_preson,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备加载的配件（根据配件类型）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryLoad(string data)
        {
            try
            {
                int accessory_type = Convert.ToInt32(data);
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Accessoris_Load
                           where a.accessory_type == accessory_type
                           select new
                           {
                               a.accessory_name,
                               a.accessory_type,
                               a.machine_code,
                               a.accessory_code,
                               a.description,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 根据设备编号获取该设备下的所有配件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryLoad_2(string data)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Accessoris_Load
                           where a.machine_code == data
                           orderby a.accessory_type
                           select new
                           {
                               a.accessory_name,
                               a.accessory_type,
                               a.machine_code,
                               a.accessory_code,
                               a.description,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下配件类型的配件
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryLoad_0(string data)
        {
            try
            {
                string[] Strdata = data.Split(',');
                string machine_code = Strdata[0].ToString();
                int accessory_type = Convert.ToInt32(Strdata[1]);
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Accessoris_Load
                           where a.accessory_type == accessory_type && a.machine_code == machine_code
                           orderby a.accessory_type
                           select new
                           {
                               a.accessory_name,
                               a.accessory_type,
                               a.machine_code,
                               a.accessory_code,
                               a.description,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取设备下配件编号（根据配件编号）
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMachineAccessoryLoad_1(string data)
        {
            try
            {
                string[] Strdata = data.Split(',');
                string machine_code = Strdata[0].ToString();
                string accessory_code = Strdata[1].ToString();
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = (from a in context.P_Accessoris_Load
                           where a.accessory_code == accessory_code && a.machine_code == machine_code
                           orderby a.accessory_type
                           select new
                           {
                               a.accessory_name,
                               a.accessory_type,
                               a.machine_code,
                               a.accessory_code,
                               a.description,
                           }).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 设备备件装载
        /// </summary>
        /// <param name="json"></param>
        public static void AddMachineSparepartRecord_0(string json)
        {
            LEDAO.P_Sparepart_Records tem = JsonConvert.DeserializeObject<LEDAO.P_Sparepart_Records>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_Sparepart_Records.AddObject(tem);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 设备配件装载
        /// </summary>
        /// <param name="json"></param>
        public static void AddMachineAccessoryRecord_0(string json)
        {
            LEDAO.P_Accessory_Records tem = JsonConvert.DeserializeObject<LEDAO.P_Accessory_Records>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_Accessory_Records.AddObject(tem);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 添加数据（加载配件）
        /// </summary>
        /// <param name="json"></param>
        public static void AddMachineAccessoryLoad(string json)
        {
            LEDAO.P_Accessoris_Load tem = JsonConvert.DeserializeObject<LEDAO.P_Accessoris_Load>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_Accessoris_Load.AddObject(tem);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="json"></param>
        public static void AddMachineStopRecord_0(string json)
        {
            LEDAO.P_Machine_StopRecords tem = JsonConvert.DeserializeObject<LEDAO.P_Machine_StopRecords>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                context.P_Machine_StopRecords.AddObject(tem);
                context.SaveChanges();
            }
        }

        public static void AddMachineStateRecord(string json)
        {
            P_Machine_State_Record tem = JsonConvert.DeserializeObject<LEDAO.P_Machine_State_Record>(json);
            //LEDAO.P_Machine_StopRecords tem = JsonConvert.DeserializeObject<LEDAO.P_Machine_StopRecords>(json);
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var record = (from a in context.P_Machine_State_Record
                              where a.machine_code == tem.machine_code && a.be_current == 1
                              select a).FirstOrDefault();
                if (record != null)
                {
                    record.be_current = 0;
                    record.end_time = tem.start_time;
                }
                context.P_Machine_State_Record.AddObject(tem);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 装载时给前一次卸载的开始时间赋值
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMachineSparepartRecord_0(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] Arrdata = data.Split(',');
                string Sparepart_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var model = context.P_Sparepart_Records.Where(X => X.machine_code == machine_code && X.sparepart_code == Sparepart_code && X.type == 1 && X.state == 0).ToList();
                foreach (var mod in model)
                {
                    mod.downloadtime = DateTime.Now;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        ///（配件） 装载时给前一次卸载的开始时间赋值
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMachineAccessoryRecord_0(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] Arrdata = data.Split(',');
                string Accessory_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var model = context.P_Accessory_Records.Where(X => X.machine_code == machine_code && X.Accessory_code == Accessory_code && X.type == 1 && X.state == 0).ToList();
                foreach (var mod in model)
                {
                    mod.downtime = DateTime.Now;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 开机时修改
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMachineStopRecord_0(string machine_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_Machine_StopRecords.Where(X => X.machine_code == machine_code && X.type == 0 && X.state == 0).ToList();
                foreach (var mod in model)
                {
                    mod.endtime = DateTime.Now;
                    mod.isNew = 0;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 关机时修改
        /// </summary>
        /// <param name="machine_code"></param>
        public static void UpdateMachineStopRecord_1(string machine_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_Machine_StopRecords.Where(X => X.machine_code == machine_code && X.type == 1 && X.state == 1).ToList();
                foreach (var mod in model)
                {
                    mod.endtime = DateTime.Now;
                    mod.state = 0;
                    mod.isNew = 0;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 装载时给前一次卸载的开始时间赋值
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMachineSparepartRecord_1(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] Arrdata = data.Split(',');
                string Sparepart_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var model = context.P_Sparepart_Records.Where(X => X.machine_code == machine_code && X.sparepart_code == Sparepart_code && X.type == 0 && X.state == 1).ToList();
                foreach (var mod in model)
                {
                    mod.downloadtime = DateTime.Now;
                    mod.state = 0;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// (配件)装载时给前一次卸载的开始时间赋值
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMachineAccessoryRecord_1(string data)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                string[] Arrdata = data.Split(',');
                string Accessory_code = Arrdata[0].ToString();
                string machine_code = Arrdata[1].ToString();
                var model = context.P_Accessory_Records.Where(X => X.machine_code == machine_code && X.Accessory_code == Accessory_code && X.type == 0 && X.state == 1).ToList();
                foreach (var mod in model)
                {
                    mod.downtime = DateTime.Now;
                    mod.state = 0;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 获取设备下的备件（用于页面显示）
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineSparepartRecord(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = context.P_GetSparepartRecords(machine_code).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 获取配件信息（用于页面显示）
        /// </summary>
        /// <param name="exception_code"></param>
        /// <returns></returns>
        public static string GetMachineAccessory_Records(string machine_code)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                var res = context.P_GetAccessoryRecords(machine_code).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }
            }
            catch (Exception ex)
            {
                return ex.ToString();
            }
            return null;
        }
        /// <summary>
        /// 装载成功更改isload
        /// </summary>
        /// <param name="Accessory_code">配件编号</param>
        public static void UpdateMachineAccessoryIsLoad(string Accessory_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.B_Machine_Accessories.Where(X => X.accessory_code == Accessory_code).ToList();
                foreach (var mod in model)
                {
                    mod.accessory_isload = 1;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 卸载成功更改isload
        /// </summary>
        /// <param name="Accessory_code"></param>
        public static void UpdateMachineAccessoryIsLoad_1(string Accessory_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.B_Machine_Accessories.Where(X => X.accessory_code == Accessory_code).ToList();
                foreach (var mod in model)
                {
                    mod.accessory_isload = 0;
                    //mod.accessory_state = 1;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 卸载配件，数据删除
        /// </summary>
        /// <param name="accessory_code"></param>
        public static void DeleteAccessoryLoad(string accessory_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.P_Accessoris_Load where u.accessory_code == accessory_code select u).FirstOrDefault();
                context.P_Accessoris_Load.DeleteObject(var);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="accessory_code"></param>
        public static void DeleteMachineOrder(string machine_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var var = (from u in context.P_Order_Machine where u.machine_code == machine_code select u).FirstOrDefault();
                context.P_Order_Machine.DeleteObject(var);
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 报障更改配件状态
        /// </summary>
        /// <param name="Accessory_code"></param>
        public static void UpdateAccessorystate(string Accessory_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.B_Accessories.Where(X => X.accessory_code == Accessory_code).ToList();
                foreach (var mod in model)
                {
                    mod.state = 2;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 卸载更改配件改为0
        /// </summary>
        /// <param name="Accessory_code"></param>
        public static void UpdateAccessorystate_0(string Accessory_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.B_Accessories.Where(X => X.accessory_code == Accessory_code).ToList();
                foreach (var mod in model)
                {
                    mod.state = 0;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 装载载更改配件改为1
        /// </summary>
        /// <param name="Accessory_code"></param>
        public static void UpdateAccessorystate_1(string Accessory_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.B_Accessories.Where(X => X.accessory_code == Accessory_code).ToList();
                foreach (var mod in model)
                {
                    mod.state = 1;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// 装载载更改配件改为1
        /// </summary>
        /// <param name="Accessory_code"></param>
        public static string GetMachinePPT_Detail(string machine_code)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var res = (from a in context.V_Machine_PPT_Detail
                           where a.machine_code == machine_code && a.ppt_type == 1
                           select a).ToList();
                if (res.Count() > 0)
                {
                    return JsonConvert.SerializeObject(res.ToList());
                }

                return null;
            }
        }

        /// <summary>
        /// 装载载更改配件改为1
        /// </summary>
        /// <param name="Accessory_code"></param>
        //public static string GetMachineDataPoints(string machine_code)
        //{
        //    using (var context = LEDAO.APIGateWay.GetEntityContext())
        //    {
        //        var res = (from a in context.B_Machine_DataPoint 
        //                   where a.machine_code == machine_code && a.r == 1
        //                   select a).ToList();
        //        if (res.Count() > 0)
        //        {
        //            return JsonConvert.SerializeObject(res.ToList());
        //        }

        //        return null;
        //    }
        //}
    }
}