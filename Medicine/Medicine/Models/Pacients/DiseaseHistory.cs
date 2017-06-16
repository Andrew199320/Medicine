using Medicine.Models.Pacients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medicine.Models.Pacients
{
    public class DiseaseHistory:RecordHistory
    {
        [Key]
        public int DiseaseHistoryId { get; set; }
        [Display(Name = "Disease name")]
        [Required(ErrorMessage = "Disease name is required.")]
        public string DiseaseName { get; set; }
        [Required(ErrorMessage = "Start date is required.")]
        [Display(Name = "Disease start")]
        public DateTime StartFrom { get; set; }
        [Display(Name = "Contraindications")]
        [Required(ErrorMessage = "Contraindications are required.")]
        public string Contraindications { get; set; }
        [Display(Name = "Additional information")]
        [Required(ErrorMessage = "Additional information are required.")]
        public string AdditionalInformation { get; set; }

        public int? PacientId { get; set; }
        public virtual ICollection<Pacient>  Pacient{ get; set; }



    }
}