using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class ExerciseDto : BaseDto
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public int Duration { get; set; }
        public int Repetitions { get; set; }
        public string Difficulty { get; set; }
    }
}
