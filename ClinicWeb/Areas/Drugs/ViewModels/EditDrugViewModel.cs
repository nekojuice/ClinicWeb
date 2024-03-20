using ClinicWeb.Areas.Drugs.Models;

namespace ClinicWeb.Areas.Drugs.ViewModels
{
    public class EditDrugViewModel
    {
        public PharmacyTMedicinesList PharmacyTMedicinesList { get; set; }
        
        //劑型：一對一不需要使用List<>
        public PharmacyTTypeDetails PharmacyTTypeDetails { get; set; }  
        public List<PharmacyTTypeList> PharmacyTTypeList { get; set; }

        public List <PharmacyTClinicalUseList> PharmacyTClinicalUseList { get; set; }
  
        public List <PharmacyTClinicalUseDetails> PharmacyTClinicalUseDetails { get; set; }
        public List<PharmacyTSideEffectList> PharmacyTSideEffectLists { get; set; }
        public List<PharmacyTSideEffectDetails> PharmacyTSideEffectDetails { get; set; }

        //初始化非空屬性
        public EditDrugViewModel() 
        {
            PharmacyTMedicinesList=new PharmacyTMedicinesList();
            PharmacyTTypeList = new List<PharmacyTTypeList>();
            PharmacyTTypeDetails=new PharmacyTTypeDetails();
            PharmacyTClinicalUseList=new List<PharmacyTClinicalUseList>();
            PharmacyTClinicalUseDetails=new List<PharmacyTClinicalUseDetails>();
            PharmacyTSideEffectLists = new List<PharmacyTSideEffectList>();
            PharmacyTSideEffectDetails=new List<PharmacyTSideEffectDetails>();
        }
    }

}
