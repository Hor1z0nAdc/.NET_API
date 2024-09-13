using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models.Domain
{
    public class Project
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public required String Name { get; set; }

        [Range(1, 100, ErrorMessage = "The {0} must be min. {1} and max. {2}")]
        [Display(Name = "Number of developers")]
        public required int NumOfDevelopers { get; set; }

        public ICollection<ProjectTask> Tasks { get; set; }
    }
}
