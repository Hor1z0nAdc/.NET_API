using FirstProject.CustomValidators;
using System.ComponentModel.DataAnnotations;

namespace FirstProject.Models.DTO
{
    public class ProjectTaskWithoutProjectDto
    {
        [Key]
        public Guid Id { get; set; }


        [Required(ErrorMessage = "*** The {0} of the task must be given! ***")]
        [StringLength(30, MinimumLength = 2, ErrorMessage = "The length of the {0} must be between {2} and {1}")]
        public String Title { get; set; }

        [Required(ErrorMessage = "*** The {0} of the task must be given! ***")]
        [Description("Description:")]
        public String Description { get; set; } = null!;
    }
}
