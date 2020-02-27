using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Models
{
    public class CountyForCreationDto
    {
        [Required(ErrorMessage ="FIPS number is required")]
        public int Id { get; set; }
        [MaxLength(100)]
        public string Name { get; set; }
    }
}