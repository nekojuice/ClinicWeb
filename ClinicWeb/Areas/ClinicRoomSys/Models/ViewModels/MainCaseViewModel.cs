using System.ComponentModel.DataAnnotations.Schema;

namespace ClinicWeb.Areas.ClinicRoomSys.Models.ViewModels
{
    public class MainCaseViewModel
    {
        public string PastHistory { get; set; }

        public string AllergyRecord { get; set; }

        public double? Height { get; set; }

        public double? Weight { get; set; }
    }
    public class Variable
    {
        public string Variable1 { get; set; }
        public string Variable2 { get; set; }
    }

}
