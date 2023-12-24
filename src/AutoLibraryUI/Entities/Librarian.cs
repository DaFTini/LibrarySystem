namespace AutoLibraryUI.Entities;

public class Librarian
{
    public int Id { get; private set; }
    public string FullName { get; set; }
    
    public string Username { get; set; }
    public string PasswordHash { get; set; }
    public string Role { get; set; }
}