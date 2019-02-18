using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BLL
{
    public class ConResult
    {
        public static string GetJsonResult(object obj, bool result, string ExtMessage)
        {
            ILE.IResult res = new ILE.LEResult();
            res.obj = obj;
            res.Result = result;
            res.ExtMessage = ExtMessage;
            return Newtonsoft.Json.JsonConvert.SerializeObject(res);
        }
    }
}