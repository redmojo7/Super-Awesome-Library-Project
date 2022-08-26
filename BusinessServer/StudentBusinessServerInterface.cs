using System;
using System.Drawing;
using System.ServiceModel;

namespace BusinessServer
{
    [ServiceContract]
    public interface StudentBusinessServerInterface
    {
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(ArgumentOutOfRangeException))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int balance, out string firstName, out string lastName, out Bitmap profileBitmap);

        [OperationContract]
        void GetValuesForSearch(string searchText, out uint acctNo, out uint pin, out int balance, out string firstName, out string lastName, out Bitmap profileBitmap);
    }
}