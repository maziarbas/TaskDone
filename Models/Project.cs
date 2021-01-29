using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskDone_V2.Models
{
    public class Project
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Project Creator Email")]
        public string UserEmail { get; set; }

        [Required]
        [Display(Name = "Project Name")]
        [StringLength(75)]
        public string Name { get; set; }

        [Display(Name = "Volunteer Project?")]
        public bool IsVolunteer { get; set; }

        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DateCreated { get; set; }

        [Display(Name = "project Completion Date")]
        [CheckDateRange]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? DateFinished { get; set; }

        [Required(ErrorMessage = "Proposed price required")]
        [Display(Name = "Project Proposed Price")]
        public double ProposedPrice { get; set; }

        [Required(ErrorMessage = "Description required")]
        [Display(Name = "Project Description")]
        [MaxLength(555)]
        public string Description { get; set; }

        [Display(Name = "Project Category")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }

        [Display(Name = "Project Status")]
        public int StatusId { get; set; }
        
        [ForeignKey("StatusId")]
        public virtual Status Status { get; set; }
        
        [Display(Name = "Accepted Price")]
        public double? AcceptedPrice { get; set; }
    }
}