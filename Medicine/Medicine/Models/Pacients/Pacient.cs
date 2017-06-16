using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Medicine.Models.Pacients
{
    public class Pacient : RecordHistory
    {
        [Key]
        public int PacientId { get; set; }
        [Required(ErrorMessage = "Name is required.")]
        [Display(Name = "Name")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Surname is required.")]
        [Display(Name = "Surname")]
        public string Surname { get; set; }
        [Required(ErrorMessage = "Gender is required.")]
        [Display(Name = "Gender")]
        public string Sex { get; set; }
        [Required(ErrorMessage = "Date is required.")]
        [Display(Name = "Birthday")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "Address is required.")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        [Required(ErrorMessage = "Mobile number is required.")]
        [Display(Name = "Mobile number")]
        public string Mobile { get; set; }
        [Display(Name = "Email")]
        public string Email { get; set; }
        [Display(Name = "Photo")]
        public string Picture { get; set; }
        [Required(ErrorMessage = "Unique number is required.")]
        [StringLength(11, ErrorMessage = "Max lenght is 11 symbols.")]
        [Display(Name = "Unique number")]
        public string UniqueNumber { get; set; }
        [Required(ErrorMessage = "Age is required.")]
        [Display(Name = "Age")]
        public string Age { get; set; }

        public string UserName { get; set; }

        public virtual ICollection<DiseaseHistory> DiseaseHistory { get; set; }

    }
}