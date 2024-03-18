namespace ClinicWeb.Areas.Drugs.ViewModels
{
    public class DrugTypeViewModel
    {
        //用來溝通主表的關聯關係
        public int DrugId {  get; set; }
        
        //TypeList表
        public List<DrugType> DrugTypes {  get; set; }

        //TypeDetails表
        public List<TypeDetail> TypeDetails {  get; set; }

        //後台 新增、修改時所選取的
        public int SelectedTypeId { get; set; }= 0;

        //初始化藥品劑型類型列表
        public DrugTypeViewModel()
        {
            DrugTypes = new List<DrugType>();
            TypeDetails = new List<TypeDetail>();
        }
    }

    public class DrugType
    {
        public int TypeId { get; set; }
        public string TypeCode {  get; set; }
        public string TypeName { get; set; }

        //初始化藥品劑型名稱(不可為Null)
        public DrugType() 
        {
            TypeName = "";
            TypeCode = "";
        }

    }

    public class TypeDetail
    {
        public int DrugId { get; set; } 
        public int TypeId { get; set; } 

    }

}

