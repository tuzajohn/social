﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Social.Models
{
    public class Message
    {
        [System.ComponentModel.DataAnnotations.Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }
        public int SourceId { get; set; }
        public int DestinationId { get; set; }
        public string Details { get; set; }
        public DateTime Time { get; set; }
        public bool IsRead { get; set; }
    }
}