using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using ILE;

namespace LEMES_POD.BLL
{
    /// <summary>
    /// wip装料
    /// </summary>
    public class WIP
    {
        /// <summary>
        /// 工单验证
        /// </summary>
        /// <param name="process">工序</param>
        /// <param name="workorder">工单</param>
        /// <returns></returns>
        public static ILE.IResult Check_WIP_Work(string process, string workorder)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string Array=process+","+workorder;
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Check_WIP_Work", Array);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
                return res;
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = ex.ToString();
            }
            return res;
        }

        /// <summary>
        /// 自动投料
        /// </summary>
        /// <param name="orderno">工单</param>
        /// <param name="process">工序</param>
        /// <returns></returns>
        public static ILE.IResult Get_WIP_AutoSend(string orderno, string process)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string Array = orderno + "," + process;
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Get_WIP_AutoSend", Array);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
                return res;
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = ex.ToString();
            }
            return res;
        }

        /// <summary>
        /// 物料批次验证
        /// </summary>
        /// <param name="orderno">工单</param>
        /// <param name="lot">批次</param>
        /// <returns></returns>
        public static ILE.IResult Check_WIP_Lot(string orderno, string lot)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string Array = orderno + "," + lot;
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Check_WIP_Lot", Array);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
                return res;
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = ex.ToString();
            }
            return res;
        }


        /// <summary>
        /// 获取wip数据
        /// </summary>
        /// <param name="orderno">工单</param>
        /// <param name="lot">批次</param>
        /// <returns></returns>
        public static ILE.IResult Get_WIP_LotInfo(string orderno, string lot)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string Array = orderno + "," + lot;
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Get_WIP_LotInfo", Array);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
                return res;
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = "Get_WIP_LotInfo程序报错";
            }
            return res;
        }


        /// <summary>
        /// 获取安装点
        /// </summary>
        /// <param name="station">工站</param>
        /// <param name="orderNO">工单</param>
        /// <param name="wip_id">wip表id号</param>
        /// <returns></returns>
        public static ILE.IResult Get_WIP_Point(string station, string orderNO, int? wip_id)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string Array = station + "," + orderNO+","+wip_id.ToString();
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Get_WIP_LotInfo", Array);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
                return res;
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = "Get_WIP_Point程序报错";
            }
            return res;
        }



        /// <summary>
        /// 该物料是否可以拆物料
        /// </summary>
        /// <param name="wip_id">wip表 id号</param>
        /// <param name="lot">批次</param>
        /// <returns></returns>
        public static bool Check_Mat_Split(int? wip_id, string lot)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string Array = wip_id.ToString() + "," + lot;
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Check_Mat_Split", Array);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = ex.ToString();
            }
            return res.Result;
        }
        /// <summary>
        /// 该物料是否最小批次
        /// </summary>
        /// <returns></returns>
        public static bool Check_Mbm(string order_no)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Check_Mbm", order_no);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = ex.ToString();
            }
            return res.Result;
        }

        /// <summary>
        /// 上料提交
        /// </summary>
        /// <param name="wip_id"></param>
        /// <param name="point">安装点</param>
        /// <param name="station">工位</param>
        /// <param name="orderno">工单</param>
        /// <param name="empCode">员工</param>
        /// <param name="inputqty">投入数</param>
        /// <returns></returns>
        public static ILE.IResult Sumit_FeedMatToStation(int wip_id, string point, string station, string orderno, string empCode, decimal? inputqty)
        {
            ILE.IResult res = new LEResult();
            try
            {
                string Array = wip_id.ToString() + "," + point + "," + station + "," + orderno + "," + empCode + "," + inputqty.ToString();
                string strRes = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.WIP", "Sumit_FeedMatToStation", Array);
                res = JsonConvert.DeserializeObject<LEResult>(strRes);
            }
            catch (Exception ex)
            {
                res.Result = false;
                res.ExtMessage = ex.ToString();
            }
            return res;
        }
    }
}
