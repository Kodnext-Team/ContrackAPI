namespace ContrackAPI
{
    public class LoginService : CustomException, ILoginService
    {
        private readonly ILoginRepository _repo;
        private readonly IConfiguration _configuration;
        private readonly IHubRepository _HubRepository;

        public LoginService(ILoginRepository repo, IConfiguration configuration, IHubRepository hubRepository)
        {
            _repo = repo;
            _configuration = configuration;
            _HubRepository = hubRepository;
        }

        public APIResponse ValidateLogin(LoginUI loginui)
        {
            var response = new APIResponse();
            if (string.IsNullOrWhiteSpace(loginui.UserName) || string.IsNullOrWhiteSpace(loginui.Password))
            {
                response.Result = Common.ErrorMessage("Invalid Username/Password");
                return response;
            }
            var loginDto = new LoginDTO
            {
                UserName = loginui.UserName,
                Password = loginui.Password
            };

            Result validationResult = _repo.ValidateLogin(loginDto);
            response.Result = validationResult ?? Common.ErrorMessage("Login validation failed");

            if (validationResult?.ResultId == 1)
            {
                response.Data = new Login
                {
                    authToken = JwtTokenService.GenerateToken(
                        loginDto.UserID.NumericValue,
                        loginDto.HubID,
                        _configuration
                    ),
                    login = loginDto,
                    HubInfo = _HubRepository.GetHubByID(loginDto.HubID)
                };
            }
            else
            {
                response.Data = null;
            }
            return response;
        }

        public UserDTO GetUserById(int userId)
        {
            return _repo.GetUserByID(userId);
        }
    }
}