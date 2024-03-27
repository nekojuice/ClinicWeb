namespace ClinicWeb.Areas.Drugs.ViewModels
{
    public class DrugDetailsViewModel
    {
        //用來溝通主表的關聯關係
       // public int DrugId { get; set; }

        //ClinicalUseList表
        public List<ClinicalUseList> ClinicalUseLists { get; set; }

        //初始化藥品是適應症列表
        public DrugDetailsViewModel() 
        {
            ClinicalUseLists = new List<ClinicalUseList>();
        }

        public class ClinicalUseList
        {
            public int ClinicalUseId {  get; set; }
            public string ClinicalUseCode {  get; set; }
            public string ClinicalUseName { get; set; }

            public ClinicalUseList()
            {
                ClinicalUseCode = "";
                ClinicalUseName = "";
            }
        }

    }
}
