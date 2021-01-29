using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TaskDone_V2.Models
{
    public class Status
    {
        public int Id { get; set; }

        [Display(Name = "Project Status")]
        public string Name { get; set; }
    }
}