using ContrackAPI;

namespace ContrackAPI
{
    public interface ILoginService
    {
        APIResponse ValidateLogin(LoginUI login);
        UserDTO GetUserById(int userId);

    }
}
