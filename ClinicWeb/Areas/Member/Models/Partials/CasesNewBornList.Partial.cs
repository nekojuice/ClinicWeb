//using ClinicWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Member.Models.Models
{
    //寫一個方法來覆蓋新生兒資料 避免導覽屬性出現迴圈 
    [MetadataType(typeof(CasesNewBornListMetadata))]
    public partial class CasesNewBornList
    {

    }

    internal class CasesNewBornListMetadata
    {
        [Newtonsoft.Json.JsonIgnore]
        //以上程式碼意思是叫他轉換成json的時候不要導覽回來 讓生成的檔案的那行不見
        //要使用include才會要這樣做 用linq就不會
        public virtual ICollection<MemberCare> MemberCare { get; set; } = new List<MemberCare>();
    }
}
