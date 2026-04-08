using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace ContrackAPI
{
    public class ValidateUserStatusFilter : ActionFilterAttribute
    {
        private readonly ILoginRepository _loginRepository;

        public ValidateUserStatusFilter(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var claim = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);

            if (claim == null)
                return;

            int userId = int.Parse(claim.Value);

            var user = _loginRepository.GetUserByID(userId);

            if (user.Status == 0)
            {
                context.Result = new JsonResult(new Result
                {
                    ResultId = 0,
                    ResultMessage = "User inactive",
                });
            }
        }
    }
}