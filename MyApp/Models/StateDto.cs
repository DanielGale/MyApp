using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MyApp.API.Models
{
    public class StateDto
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [MaxLength(100)]
        public string Name { get; set; }
        public string PostalCode { get; set; }
        public int NumberOfCounties
        {
            get
            {
                return Counties.Count;
            }
        }

        public ICollection<CountyDto> Counties { get; set; }
         = new List<CountyDto>();
    }
}
