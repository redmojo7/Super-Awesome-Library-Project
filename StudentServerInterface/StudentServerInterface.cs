using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Drawing;

namespace DatabaseServer
{
    [ServiceContract]
    public interface StudentServerInterface
    {
        //Each of these are service function contracts. They need to be tagged as OperationContracts.
        [OperationContract]
        int GetNumEntries();

        [OperationContract]
        [FaultContract(typeof(ArgumentOutOfRangeException))]
        void GetValuesForEntry(int index, out uint acctNo, out uint pin, out int bal, out string fName, out string lName, out Bitmap profileBitmap);
    }
}
