using FirstProject.Models.Domain;
using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models.DTO
{
    public class InsertProjectDto
    {
        [Required]
        public String Name { get; set; }

        [Range(1, 100, ErrorMessage = "The {0} must be min. {1} and max. {2}")]
        [Display(Name = "Number of developers")]
        public double NumOfDevelopers { get; set; }

    }
}
