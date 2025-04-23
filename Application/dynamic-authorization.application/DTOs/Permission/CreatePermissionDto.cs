using System.ComponentModel.DataAnnotations;

namespace dynamic_authorization.application.DTOs.Permission;

public class CreatePermissionDto
{
    [Required]
    public int permissionId { get; set; }
    [Required]
    public string resource { get; set; }
    [Required]
    public Guid userId { get; set; }
}