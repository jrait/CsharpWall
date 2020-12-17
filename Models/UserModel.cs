using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TheWall.Models
{
    public class User
    {
        [Key]
        public int UserID {get;set;}
        [Required]
        [Display(Name = "First Name")]
        public string FirstName {get;set;}
        [Required]
        [Display(Name = "Last Name")]
        public string LastName {get;set;}
        [Required]
        [EmailAddress]
        public string Email {get;set;}
        [Required]
        [MinLength(8)]
        [DataType(DataType.Password)]
        public string Password {get;set;}
        [NotMapped]
        [Required]
        [Compare("Password")]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm Password")]
        public string Confirm {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
    }
    public class LoginUser
    {
        [Required]
        public string Email {get;set;}
        [Required]
        public string Password {get;set;}
    }
}