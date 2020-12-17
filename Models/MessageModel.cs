using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
namespace TheWall.Models
{
    public class Message
    {
        [Key]
        public int MessageID {get;set;}
        [Required]
        [Display(Name = "Post a Message")]
        public string content {get;set;}
        public DateTime CreatedAt {get;set;} = DateTime.Now;
        public DateTime UpdatedAt {get;set;} = DateTime.Now;
        public int UserID {get;set;}
        public User Creator {get;set;}
    }
}