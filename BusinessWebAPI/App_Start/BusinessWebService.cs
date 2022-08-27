using DatabaseServer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Web;

namespace BusinessWebAPI.App_Start
{
    public class BusinessWebService
    {
        public StudentServerInterface foob;

        //private LogClass log;

        public BusinessWebService()
        {
            ChannelFactory<StudentServerInterface> foobFactory;
            NetTcpBinding tcp = new NetTcpBinding();
            //Set the URL and create the connection!
            string URL = "net.tcp://localhost:8100/DatabaseServer";
            foobFactory = new ChannelFactory<StudentServerInterface>(tcp, URL);
            foob = foobFactory.CreateChannel();
            //log = new LogClass();

            //log.Log("[BusinessServer.StudentBusinessServerImpl]: Initialize DatabaseServer");
        }

    }
}