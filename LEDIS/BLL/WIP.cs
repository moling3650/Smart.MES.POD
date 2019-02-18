using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using LEDAO;
using System.Data;

namespace BLL
{
    public class WIP
    {
        //工单验证
        public static string Check_WIP_Work(string Array)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();

            string[] arr = Array.Split(',');
            try
            {
                using (context)
                {
                    var v = context.sp_Check_WIP_Work(arr[0], arr[1]).First();
                    res.Result = v.Result == "true" ? true : false;
                    res.ExtMessage = v.ExtMessage;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        //自动投料
        public static string Get_WIP_AutoSend(string Array)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();

            string[] arr = Array.Split(',');
            try
            {
                using (context)
                {
                    var v = context.sp_Get_WIP_AutoSend(arr[0], arr[1]).ToList();
                    res.Result = true;
                    if (v.Count < 1)
                    {
                        res.Result = false;
                        res.ExtMessage = "没有数据";
                    }
                    res.obj = v;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        // 物料批次验证
        public static string Check_WIP_Lot(string Array)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();

            string[] arr = Array.Split(',');
            try
            {
                using (context)
                {
                    var v = context.sp_Check_WIP_Lot(arr[0], arr[1]).First();
                    res.Result = v.Result == "true" ? true : false;
                    res.ExtMessage = v.ExtMessage;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        //获取wip数据 
        public static string Get_WIP_LotInfo(string Array)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();

            string[] arr = Array.Split(',');
            try
            {
                using (context)
                {
                    var v = context.sp_Get_WIP_LotInfo(arr[0], arr[1]).ToList();
                    res.Result = true;
                    if (v.Count < 1)
                    {
                        res.Result = false;
                        res.ExtMessage = "没有数据";
                    }
                    res.obj = v;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        // 该物料是否可以拆物料
        public static string Check_Mat_Split(string Array)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();

            string[] arr = Array.Split(',');
            int? point = int.Parse(arr[0]);
            try
            {
                using (context)
                {
                    var v = context.sp_Check_Mat_Split(point, arr[1]).First();
                    res.Result = v.Result == "true" ? true : false;
                    res.ExtMessage = v.ExtMessage;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        // 该物料是否是最小批次
        public static string Check_Mbm(string OrderNo)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();
            try
            {
                using (context)
                {
                    var v = context.sp_Check_Mbm(OrderNo).First();
                    res.Result = v.Result == "true" ? true : false;
                    res.ExtMessage = v.ExtMessage;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        //获取安装点
        public static string Get_Get_WIP_Point(string Array)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();

            string[] arr = Array.Split(',');
            int? point = int.Parse(arr[2]);
            try
            {
                using (context)
                {
                    var v = context.sp_Get_WIP_Point(arr[0], arr[1], point).ToList();
                    res.Result = true;
                    if (v.Count < 1)
                    {
                        res.Result = false;
                        res.ExtMessage = "没有数据";
                    }
                    res.obj = v;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }

        //上料提交
        public static string Sumit_FeedMatToStation(string Array)
        {
            ILE.IResult res = new ILE.LEResult();
            var context = LEDAO.APIGateWay.GetEntityContext();

            string[] arr = Array.Split(',');
            int? point = int.Parse(arr[0]);
            decimal? inputqty = decimal.Parse(arr[5]);
            try
            {
                using (context)
                {
                    var v = context.sp_Sumit_FeedMatToStation(point, arr[1], arr[2], arr[3], arr[4], inputqty).First();
                    res.Result = v.Result == "true" ? true : false;
                    res.ExtMessage = v.ExtMessage;
                }
            }
            catch (Exception ex)
            {

                res.Result = false;
                res.ExtMessage = ex.ToString();
            }

            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
        /// <summary>
        /// 通过工站获取全部已装料数据
        /// </summary>
        /// <param name="json"></param>
        /// <returns></returns>
        public static string GetWIPState(string json)
        {

            string[] strAry = json.Split(',');
            try
            {
                string station = strAry[0];
                var context = LEDAO.APIGateWay.GetEntityContext();
                List<LEDAO.V_WIP_Seed> list = context.V_WIP_Seed.Where(c => c.station_code == station).ToList();
                return JsonConvert.SerializeObject(list);
            }
            catch (Exception)
            {

                throw;
            }

        }


        /// <summary>
        /// 删除投料记录
        /// </summary>
        /// <param name="strid"></param>
        /// <returns></returns>
        public static string DeleteWIPSeed(string strid)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            ILE.LEResult res = new ILE.LEResult();
            try
            {

                using (context)
                {
                    int id = Convert.ToInt32(strid);
                    P_Mat_WIP_Seed seed = context.P_Mat_WIP_Seed.Where(c => c.id == id).First();
                    context.ObjectStateManager.ChangeObjectState(seed, EntityState.Deleted);
                    //long wip_id = Convert.ToInt64(seed.wip_id);
                    //P_Material_WIP wip = context.P_Material_WIP.Where(c => c.id == wip_id).First();
                    //wip.lot_qty += seed.lot_qty;
                    //context.ObjectStateManager.ChangeObjectState(wip, EntityState.Modified);
                    context.SaveChanges();
                }
                res.Result = true;
                return JsonConvert.SerializeObject(res);

            }
            catch (Exception)
            {
                res.Result = false;
                return JsonConvert.SerializeObject(res);
            }
        }


        /// <summary>
        /// 修改WIP数据
        /// </summary>
        /// <param name="wip"></param>
        /// <returns></returns>
        public static string UpdateWIPSeed(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            ILE.LEResult res = new ILE.LEResult();
            try
            {

                using (context)
                {
                    string[] arrdata = data.Split(',');
                    int id = Convert.ToInt32(arrdata[0]);
                    decimal qty = decimal.Parse(arrdata[1]);
                    P_Mat_WIP_Seed seed = context.P_Mat_WIP_Seed.Where(c => c.id == id).First();
                    if (qty == 0)
                    {
                        seed.state = 2;
                    }
                    seed.lot_qty = qty;
                    seed.unload_time = context.NewDate().First();
                    context.ObjectStateManager.ChangeObjectState(seed, EntityState.Modified);
                    context.SaveChanges();
                }
                res.Result = true;
                return JsonConvert.SerializeObject(res);

            }
            catch (Exception)
            {
                res.Result = false;
                return JsonConvert.SerializeObject(res);
            }

        }
        /// <summary>
        /// 修改WIP_Material数据
        /// </summary>
        /// <param name="wip"></param>
        /// <returns></returns>
        public static string UpdateWIPMaterial(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            ILE.LEResult res = new ILE.LEResult();
            try
            {
                string[] Arr = data.Split(',');
                string SFC = Arr[0].ToString();
                decimal lot_qty = Convert.ToDecimal(Arr[1].ToString());
                string order_no = Arr[3].ToString();
                string mat_code = Arr[2].ToString();
                using (context)
                {
                    P_Material_WIP materil = context.P_Material_WIP.Where(c => c.Parent_order == order_no && c.lot_no == SFC && c.mat_code == mat_code).First();
                    materil.lot_qty = lot_qty;
                    materil.state = 0;
                    context.ObjectStateManager.ChangeObjectState(materil, EntityState.Modified);
                    context.SaveChanges();
                }
                res.Result = true;
                return JsonConvert.SerializeObject(res);

            }
            catch (Exception)
            {
                res.Result = false;
                return JsonConvert.SerializeObject(res);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string UpdateWIPMaterialCom(string data)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            ILE.LEResult res = new ILE.LEResult();
            try
            {
                string[] Arr = data.Split(',');
                string SFC = Arr[0].ToString();
                decimal lot_qty = Convert.ToDecimal(Arr[1].ToString());
                string mat_code = Arr[2].ToString();
                using (context)
                {
                    P_Material_WIP materil = context.P_Material_WIP.Where(c => c.Parent_order == "" && c.lot_no == SFC && c.mat_code == mat_code).First();
                    materil.lot_qty = lot_qty;
                    materil.state = 0;
                    context.ObjectStateManager.ChangeObjectState(materil, EntityState.Modified);
                    context.SaveChanges();
                }
                res.Result = true;
                return JsonConvert.SerializeObject(res);

            }
            catch (Exception)
            {
                res.Result = false;
                return JsonConvert.SerializeObject(res);
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lotno"></param>
        /// <returns></returns>
        public static string GetWIPDisminPack(string lotno)
        {
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    var v = context.V_WIPRank.Where(c => c.parent_sfc == lotno);

                    return JsonConvert.SerializeObject(v.ToList());
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string WIPDisminSubmit(string wip)
        {
            ILE.LEResult res = new ILE.LEResult();
            try
            {
                var context = LEDAO.APIGateWay.GetEntityContext();
                using (context)
                {
                    LEDAO.V_WIPRank rank = JsonConvert.DeserializeObject<LEDAO.V_WIPRank>(wip);
                    LEDAO.P_SFC_Rank sfcrank = new P_SFC_Rank();
                    sfcrank.child_qty = rank.child_qty;
                    sfcrank.child_sfc = rank.child_sfc;
                    sfcrank.input_date = DateTime.Now;
                    sfcrank.lot_type = rank.lot_type;
                    sfcrank.main_sfc = rank.main_sfc;
                    sfcrank.number = rank.number;
                    sfcrank.order_no = rank.order_no;
                    sfcrank.parent_qty = rank.parent_qty;
                    sfcrank.parent_sfc = rank.parent_sfc;
                    context.P_SFC_Rank.AddObject(sfcrank);

                    LEDAO.P_Material_WIP addwip = new P_Material_WIP();
                    addwip.bill_no = rank.order_no;
                    addwip.lot_no = rank.child_sfc;
                    addwip.lot_qty = rank.child_qty;
                    addwip.mat_code = rank.mat_code;
                    addwip.order_no = rank.order_no;
                    addwip.Parent_order = rank.Parent_order;
                    addwip.parent_station = rank.parent_station;
                    addwip.state = 0;
                    context.P_Material_WIP.AddObject(addwip);

                    LEDAO.P_Material_WIP updwip = context.P_Material_WIP.Where(c => c.lot_no == rank.parent_sfc).First();
                    updwip.lot_qty = updwip.lot_qty - sfcrank.child_qty;
                    context.ObjectStateManager.ChangeObjectState(updwip, EntityState.Modified);

                    context.SaveChanges();

                }
                res.Result = true;
            }
            catch (Exception)
            {

                res.Result = false;
                res.ExtMessage = "保存失败";
            }
            return JsonConvert.SerializeObject(res);
        }

        /// <summary>
        /// 在WIP表中通过lot_no获取批次state
        /// </summary>
        /// <returns></returns>
        public static string GetStateBylot_no(string lot_no)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_Material_WIP.Where(X => X.lot_no == lot_no) select new { state = a.state }).ToList();
            if (res.Count > 0)
            {
                return res[0].state.ToString();
            }
            return null;
        }

        /// <summary>
        /// 在WIP表中通过lot_no获取批次lot_qty
        /// </summary>
        /// <returns></returns>
        public static string GetWipQtyByLot(string data)
        {
            string[] Arrdata = data.Split(',');
            string lot_no = Arrdata[0];
            string mat_code = Arrdata[1];
            string order_no = Arrdata[2];
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_Material_WIP.Where(X => X.Parent_order == order_no && X.lot_no == lot_no && X.mat_code == mat_code) select new { qty = a.lot_qty }).ToList();
            if (res.Count > 0)
            {
                return res[0].qty.ToString();
            }
            return null;
        }
        /// <summary>
        /// 根据批次及工单获取mat_code
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetMat_codeBylot(string data)
        {
            string[] Arrdata = data.Split(',');
            string lot_no = Arrdata[0];
            string order_no = Arrdata[1];
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_Material_WIP.Where(X => X.Parent_order == order_no && X.lot_no == lot_no) select new { mat_code = a.mat_code }).ToList();
            if (res.Count > 0)
            {
                return res[0].mat_code.ToString();
            }
            return null;
        }
        /// <summary>
        /// 公用料获取去qty
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetWipComQtyByLot(string data)
        {
            string[] Arrdata = data.Split(',');
            string lot_no = Arrdata[0];
            string mat_code = Arrdata[1];
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_Material_WIP.Where(X => X.Parent_order == "" && X.lot_no == lot_no && X.mat_code == mat_code) select new { qty = a.lot_qty }).ToList();
            if (res.Count > 0)
            {
                return res[0].qty.ToString();
            }
            return null;
        }
        /// <summary>
        /// 根据lot_no获取wip数量
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetWipQty1ByLot(string lot_no)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_Material_WIP.Where(X => X.lot_no == lot_no) select new { qty = a.lot_qty }).ToList();
            if (res.Count > 0)
            {
                return res[0].qty.ToString();
            }
            return null;
        }
        /// <summary>
        /// 根据批次及物料编号获取物料写入wip的时间
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string GetWipInputTimeByLot(string data)
        {
            string[] Arrdata = data.Split(',');
            string lot_no = Arrdata[0];
            string mat_code = Arrdata[1];
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_Material_WIP.Where(X => X.lot_no == lot_no && X.mat_code == mat_code) select new { input_time = a.input_time }).ToList();
            if (res.Count > 0)
            {
                return res[0].input_time.ToString();
            }
            return null;
        }
        /// <summary>
        /// 在WIP表中通过lot_no获取批次state
        /// </summary>
        /// <returns></returns>
        public static string GetSFCBylot_no(string lot_no)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.P_Material_WIP.Where(X => X.lot_no == lot_no) select new { lot_no = a.lot_no }).ToList();
            if (res.Count > 0)
            {
                return res[0].lot_no.ToString();
            }
            return null;
        }


        /// <summary>
        /// 修改Material_WIP的state为2
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMaterialWIPState(string lot_no)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_Material_WIP.Where(X => X.lot_no == lot_no).FirstOrDefault();
                model.state = 2;
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 修改Material_WIP的state为0
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMaterialWIPStateForBack(string lot_no)
        {
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = context.P_Material_WIP.Where(X => X.lot_no == lot_no).FirstOrDefault();
                model.state = 0;
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 根据产品编码，修改Material_WIP的state为0
        /// </summary>
        /// <param name="data"></param>
        public static void UpdateMaterialWIPStateByMat_code(string data)
        {
            string[] data1 = data.Split(',');
            string mat_code = data1[0].ToString();
            string lot_no = data1[1].ToString();
            using (var context = LEDAO.APIGateWay.GetEntityContext())
            {
                var model = from a in context.P_Material_WIP
                            where
                            (a.mat_code == mat_code && a.lot_no == lot_no)
                            select a;
                foreach (var b in model)
                {
                    b.state = 0;
                }
                context.SaveChanges();
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="lot_no"></param>
        /// <returns></returns>
        public static string GetLot_noBylot_no(string lot_no)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            var res = (from a in context.V_WIP_Product.Where(X => X.lot_no == lot_no) select new { max_qty = a.max_qty }).ToList();
            if (res.Count > 0)
            {
                return res[0].max_qty.ToString();
            }
            return null;
        }
        /// <summary>
        /// 修改WIP_seed的qty
        /// </summary>
        /// <param name="wip"></param>
        /// <returns></returns>
        public static string UpdateWIPSeedQty(string data)
        {
            string[] arrdata = data.Split(',');
            decimal usedqty = decimal.Parse(arrdata[0].ToString());
            string orderno = arrdata[2].ToString();
            string matcode = arrdata[3].ToString();
            string station = arrdata[4].ToString();
            string lot_no = arrdata[1].ToString();
            var context = LEDAO.APIGateWay.GetEntityContext();
            ILE.LEResult res = new ILE.LEResult();
            try
            {
                using (context)
                {
                    P_Mat_WIP_Seed seed = context.P_Mat_WIP_Seed.Where(c => c.lot_no == lot_no && c.order_no == orderno && c.mat_code == matcode && c.station_code == station).First();
                    seed.lot_qty -= usedqty;
                    context.ObjectStateManager.ChangeObjectState(seed, EntityState.Modified);
                    context.SaveChanges();
                }
                res.Result = true;
                return JsonConvert.SerializeObject(res);
            }
            catch (Exception)
            {
                res.Result = false;
                return JsonConvert.SerializeObject(res);
            }

        }
        /// <summary>
        /// 获取补料清单
        /// </summary>
        /// <param name="sfc"></param>
        /// <returns></returns>
        public static string GetFedBatchList(string sfc)
        {
            var db = LEDAO.APIGateWay.GetEntityContext();
            var var = from v_fedbatch in db.V_FedBatch
                      where
                          ((from b_processsonstep in db.B_ProcessSonStep
                            select new
                            {
                                b_processsonstep.parentstepid
                            }).Distinct()).Contains(new { parentstepid = (System.String)v_fedbatch.step_code }) &&
                        v_fedbatch.flow_code ==
                          ((from p_workorder in db.P_WorkOrder
                            where
                              p_workorder.order_no ==
                                ((from p_sfc_state in db.P_SFC_State
                                  where
                                    p_sfc_state.SFC == sfc
                                  select new
                                  {
                                      p_sfc_state.order_no
                                  }).FirstOrDefault().order_no)
                            select new
                            {
                                p_workorder.flow_code
                            }).FirstOrDefault().flow_code)
                      select new
                      {
                          v_fedbatch.step_code,
                          v_fedbatch.step_name,
                          v_fedbatch.mat_code,
                          v_fedbatch.flow_name,
                          v_fedbatch.mat_name,
                          v_fedbatch.product_name,
                          v_fedbatch.flow_code
                      };
            if (var.Count() > 0)
            {
                return JsonConvert.SerializeObject(var.ToList());
            }
            return null;

        }
    }
}