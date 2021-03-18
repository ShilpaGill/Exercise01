using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebApiExercise01.Models
{[Table("Plyers")]
    public class Players
    {  [Key]
        public int PId { get; set; }
        public string PName { get; set; }
        public double BattingAverage { get; set; }
        public double BowlingAverage { get; set; }
        public string PTeam { get; set; }
    }
}