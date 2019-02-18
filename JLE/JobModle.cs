using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ILE;

namespace JLE
{
    public class JobModle:ILE.IJob
    {

        public IResult DoJob(string str)
        {
            return null;
        }

        public JobModle()
        { 
            
        }

        public void Run()
        { 
            
        }

        public List<IStep> StepList
        {
            get;
            set;
        }

        public string UserCode
        {
            get;
            set;
        }

        public int StepIdx
        {
            get;
            set;
        }

        public string RouteType
        {
            get;
            set;
        }

        public string SFC
        {
            get;
            set;
        }

        public string OrderNO
        {
            get;
            set;
        }

        public string DriveName
        {
            get;
            set;
        }

        public string DriveParemeter
        {
            get;
            set;
        }

        public string StationCode
        {
            get;
            set;
        }

        public string ProcessCode
        {
            get;
            set;
        }

        public string EmpCode
        {
            get;
            set;
        }



    }
}
