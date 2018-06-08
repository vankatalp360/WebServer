namespace MyWebServer.GameStore.Services.Contracts
{
    public interface IUserService
    {
        bool Create(string email, string name, string password);

        bool FindUser(string email, string password);

        bool IsAdmin(string email);

        int? GatUserId(string email);
    }
}