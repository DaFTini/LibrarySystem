namespace AutoLibraryUI.Interfaces;

public interface IAuthService<TUser>
{
    public bool Register(TUser user, string password);
    public TUser? Login(string username, string password);
}