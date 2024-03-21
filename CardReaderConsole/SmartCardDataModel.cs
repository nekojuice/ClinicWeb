using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReaderConsole
{
    public class SmartCardDataModel
    {
        public SmartCardDataModel() { }
        public SmartCardDataModel(SmartCardDataModel x)
        {
            cCardNumber = x.cCardNumber;
            cName = x.cName;
            cID = x.cID;
            cBirthday = x.cBirthday;
            cGender = x.cGender;
            cCardPublish = x.cCardPublish;
        }
        public string cCardNumber { get; set; }
        public string cName { get; set; }
        public string cID { get; set; }
        public string cBirthday { get; set; }
        public string cGender { get; set; }
        public string cCardPublish { get; set; }

    }
}
