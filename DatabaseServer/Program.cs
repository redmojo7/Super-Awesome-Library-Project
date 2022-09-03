using System;
using System.ServiceModel;


namespace DatabaseServer
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("hey so like welcome to data server");
            // This is the actual host service system
            ServiceHost host;
            // This represents a tcp/ip binding in the Windows network stack
            NetTcpBinding tcp = new NetTcpBinding();
            StudentServerImpl studentServerImpl = new StudentServerImpl();
            // Bind server to the implementation of DataServer
            //singletonInstance
            host = new ServiceHost(studentServerImpl);

            //host = new ServiceHost(typeof(StudentServerImpl));
            /* Present the publicly accessible interface to the client. 0.0.0.0 tells .net to
             accept on any interface. :8100 means this will use port 8100. DataService is a name for the
             actual service, this can be any string.*/

            host.AddServiceEndpoint(typeof(StudentServerInterface), tcp, "net.tcp://0.0.0.0:8100/DatabaseServer");
            // And open the host for business!
            host.Open();
            Console.WriteLine("System Online");
            Console.ReadLine();
            // Don't forget to close the host after you're done!
            host.Close();
        }
    }
}
