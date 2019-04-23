using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Social.Models
{
    public class Vote
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public string ElementId { get; set; }
        public string Type { get; set; }
        public string Voting { get; set; }
        public int CommentId { get; set; }
        public int UserId { get; set; }
        public DateTime Time { get; set; }
    }
}