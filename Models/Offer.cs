using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskDone_V2.Models
{
    public class Offer
    {
        public int Id { get; set; }

        [Display(Name = "Offer Creator Email")]
        public string UserEmail { get; set; }
                     
        public int ProjectId { get; set; }

        [ForeignKey("ProjectId")]
        public virtual Project Project { get; set; }

        [Required]
        [Display(Name ="Offer Price")]
        [RegularExpression("(?!\\D+$)\\+?\\d*?(?:\\.\\d*)?$")]
        public double OfferedPrice { get; set; }

        [Required]
        [Display(Name = "Offer Date")]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime OfferedDate { get; set; }

        
        [Display(Name = "Accepted ?")] 
        public bool? Accepted { get; set; }
    }
}