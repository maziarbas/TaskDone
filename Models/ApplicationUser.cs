using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace TaskDone_V2.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [Display(Name = "First Name")]
        [StringLength(225)]
        public string FirstName { get; set; }

        [Required]
        [Display(Name = "Last Name")]
        [StringLength(225)]
        public string LastName { get; set; }

        [Required]
        [Display(Name = "Social Security")]
        public string SocialSecurity { get; set; }

        [Display(Name = "Company Name")]
        [StringLength(75)]
        public string CompanyName { get; set; }

        [Required]
        [DataType(DataType.Date), DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Member Date of Birth")]
        //[UserAgeValidation]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; }
        [Display(Name = "Street Address 2")]
        public string StreetAddress2 { get; set; }

        [Required(ErrorMessage = "Zip code should be 5 digits")]
        [Display(Name = "Zip Code")]
        [MinLength(5), MaxLength(5)]
        [RegularExpression("\\d{5}-?(\\d{4})?$", ErrorMessage = "Please enter 5 digit")]

        public string ZipCode { get; set; }

        [Required]
        [Display(Name = "State")]
        public string State { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public string Gender { get; set; }
              
        public byte[] UserPhoto { get; set; }

        [Display(Name = "Rate")]
        [Range(0, 5), UIHint("NumericSlider")]
        public int Rate { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

  
}