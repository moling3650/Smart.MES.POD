using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LEDAO;
using Newtonsoft.Json;
using System.Data.SqlClient;

namespace BLL
{
    public class DataPoint
    {
        public static string GetStandardPoint(string machineCode)
        {
            
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var var = (from a in context.B_Machine_Standard_Point 
                           join b in context.B_Machine_DataPoint on a.point_id equals b.point_id 
                           where a.machine_code == machineCode
                           select new
                           {
                               a.run_at,
                               a.task_drive_code,
                               a.trigger_condition,
                               a.trigger_type,
                               a.point_id,
                               task_parameter=a.parameter,
                               b.dataPoint_name,
                               b.dc_drive_code,
                               b.parameter,
                               a.business_name,
                               a.business_code,
                               a.machine_code,
                               a.id
                           }).ToList();;
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var);
                }
            }
            return null;
        }

        public static string GetAnalogPoint(string machineCode)
        {
            var context = LEDAO.APIGateWay.GetEntityContext();
            using (context)
            {
                var var = (from a in context.B_Machine_Analog_Point
                           join b in context.B_ProcessControlItem_Detail on a.control_id equals b.control_id
                           join c in context.B_Machine_DataPoint on a.point_id equals c.point_id 
                           where
                                a.machine_code == machineCode
                           select new
                           {
                               a.run_at,
                               a.to_monitor,
                               a.task_drive_code,
                               a.trigger_condition,
                               a.trigger_type,
                               a.point_id,
                               c.dataPoint_name,
                               c.dc_type,
                               c.dc_drive_code,
                               c.parameter,
                               a.business_name,
                               a.business_code,
                               a.machine_code,
                               a.id,
                               b.ucl,
                               b.lcl,
                               b.group_count
                           }).ToList();
                if (var.Count() > 0)
                {
                    return JsonConvert.SerializeObject(var);
                }
            }
            return null;
        }
    }
}