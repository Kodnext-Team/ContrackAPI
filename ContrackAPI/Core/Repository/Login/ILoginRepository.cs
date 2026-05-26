namespace ContrackAPI
{
    public interface ILoginRepository
    {
        Result ValidateLogin(LoginDTO login);
        UserDTO GetUserByID(int userId);
        List<UserDTO> GetUserLoginList(UserFilter filter);

    }
}
