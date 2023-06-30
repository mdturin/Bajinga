namespace Core.Entities.Identity;

public class IdentityAddress : Address
{
    public int Id { get; set; }
    public string AppUserId { get; set; }
    public AppUser AppUser { get; set; }
}
