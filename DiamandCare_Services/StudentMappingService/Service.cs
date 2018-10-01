using DiamandCare.Core;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace StudentMappingService
{
    public partial class Service : ServiceBase
    {
        static public bool IsStopping { get; private set; }

        public Service()
        {
            InitializeComponent();
        }

        internal void StartSvc()
        {
            OnStart(null);
        }

        internal void StopSvc()
        {
            OnStop();
        }

        protected override void OnStart(string[] args)
        {
            Task.Run(() =>
            {
                try
                {
                    Service.IsStopping = false;

                    StudentMapping.Start();
                    //UploadedFileParser.Start();
                }
                catch (Exception ex)
                {
                    // stop the service
                    ErrorLog.Write(ex);
                    base.Stop();
                }
            });
        }

        protected override void OnStop()
        {
            try
            {
                Service.IsStopping = true;
                StudentMapping.Stop();
                //UploadedFileParser.Stop();
            }
            catch
            {
                //TODO: Log the error
            }
        }
    }
}
