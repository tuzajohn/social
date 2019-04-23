using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Social.Models
{
    public class Network
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int InitiatorId { get; set; }
        public int AcceptorId { get; set; }
        public DateTime Time { get; set; }
    }
}