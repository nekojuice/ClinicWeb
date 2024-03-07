using ClinicWeb.Models;
using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Drugs.Models.Partials
{
    [MetadataType(typeof(PharmacyTClinicalUseListMetadata))]
    public partial class PharmacyTClinicalUseList
    {
    }
    internal class PharmacyTClinicalUseListMetadata
    {
        [Newtonsoft.Json.JsonIgnore]
        public virtual ICollection<PharmacyTClinicalUseDetails> PharmacyTClinicalUseDetails { get; set; } = new List<PharmacyTClinicalUseDetails>();
        
    }
}
