using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskDone_V2.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Category Name")]
        [StringLength(255)]
        public string Name { get; set; }

        [Display(Name = "# Projects")]
        public int? ProjectCount { get; set; }

       
        public Category()
        {
            ProjectCount = 0;
        }
    }
}