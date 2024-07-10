namespace jim.wiki.core.Authentication.Models;

public interface IUserData
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? IP { get; set; }
}
