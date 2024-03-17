using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CardReaderTest
{

    public class ApptDataModel
    {
        public string department { get; set; }
        public string shift { get; set; }
        public string doctor { get; set; }
        public int number { get; set; }
        public int state { get; set; }

        public ApptDataModel()
        {
            department = "";
            shift = "";
            doctor = "";
            number = -1;
            state = -1;
        }
    }
}
