using System.ComponentModel.DataAnnotations.Schema;

namespace dynamic_authorization.domain.Entities;

public class User
{
    public Guid Id { get; set; }
    
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    
    public bool Is_Admin { get; set; }
    
    [Column("created_on")]
    public DateTime Created_On { get; set; }
}