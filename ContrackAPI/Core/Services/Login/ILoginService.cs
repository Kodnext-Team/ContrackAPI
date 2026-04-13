using ContrackAPI;

namespace ContrackAPI
{
    public interface ILoginService
    {
        LoginResponse ValidateLogin(LoginUI login);
        UserDTO GetUserById(int userId);

    }
}
