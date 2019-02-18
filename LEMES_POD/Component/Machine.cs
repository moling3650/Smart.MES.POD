using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LEDAO;
using Newtonsoft.Json;
using LEMES_POD.Tools;
using System.Windows.Forms;


namespace LEMES_POD.Component
{
    class Machine
    {
        /// <summary>
        /// 开机
        /// </summary>
        /// <param name="machine_code"></param>
        /// <param name="List"></param>
        /// <param name="E_code"></param>
        public static void AddMachine_StopRecordsOpen(string machine_code, List<V_Station_Shop> List, string E_code)
        {
            string RecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineStopRecords_1", machine_code);
            List<P_Sparepart_Records> ListSparepartRecords = JsonConvert.DeserializeObject<List<P_Sparepart_Records>>(RecordsJson);
            if (ListSparepartRecords == null)
            {
            }
            else
            {
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineStopRecord_0", machine_code);
            }

            try
            {
                P_Machine_StopRecords Sparepart = new P_Machine_StopRecords()
                {
                    process_code = List[0].process_code,
                    station_code = List[0].station_code,
                    ws_code = List[0].ws_code,
                    machine_code = machine_code,
                    type = 1,
                    state = 1,
                    isNew = 1,
                    starttime = DateTime.Now,
                    operate_person = E_code,
                };
                string strJson = JsonToolsNet.ObjectToJson(Sparepart);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineStopRecord_0", strJson);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineStateOpen", machine_code);
            }
            catch (Exception ex)
            {
                MessageBox.Show("开机失败", "提示");
                return;
            }

        }
        /// <summary>
        /// 关机
        /// </summary>
        public static void AddMachine_StopRecordsClose(string machine_code, List<V_Station_Shop> List, string Faultreason_code, string E_code)
        {
            //根据关机原因获取关机后状态
            string StopreasonJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetStopreason", Faultreason_code);
            List<B_Machine_Stopreason> ListStopreason = JsonConvert.DeserializeObject<List<B_Machine_Stopreason>>(StopreasonJson);
            int stop_state = 0;
            if (ListStopreason != null)
            {
                stop_state = (int)ListStopreason[0].stop_state;
            }
            string RecordsJson = Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "GetMachineStopRecords_0", machine_code);
            List<P_Sparepart_Records> ListSparepartRecords = JsonConvert.DeserializeObject<List<P_Sparepart_Records>>(RecordsJson);
            if (ListSparepartRecords == null)
            {
                //啥也不干
            }
            else
            {
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineStopRecord_1", machine_code);
            }

            try
            {
                P_Machine_StopRecords Sparepart = new P_Machine_StopRecords()
                {
                    process_code = List[0].process_code,
                    station_code = List[0].station_code,
                    ws_code = List[0].ws_code,
                    machine_code = machine_code,
                    stop_reason = Faultreason_code,
                    type = 0,
                    state = 0,
                    isNew = 1,
                    starttime = DateTime.Now,
                    operate_person = E_code,
                };
                string strJson = JsonToolsNet.ObjectToJson(Sparepart);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "AddMachineStopRecord_0", strJson);
                Tools.ServiceReferenceManager.GetClient().RunServerAPI("BLL.Machine", "UpdateMachineStateClose", machine_code + "," + stop_state.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("关机失败", "提示");
                return;
            }
        }

    }
}
