using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Social.Models
{
    public class UserProfile
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Mobile { get; set; }
        public string Interest { get; set; }
        public string Occupation { get; set; }
        public string About { get; set; }
        public string Language { get; set; }
        public string Address { get; set; }
        public string Education { get; set; }
    }
}