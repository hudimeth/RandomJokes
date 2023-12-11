using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReactRandomJokes.Data;
using ReactRandomJokes.Web.ViewModels;
using System.Security.Claims;
using System.Security.Cryptography.Xml;

namespace ReactRandomJokes.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private string _connectionString;
        public AccountController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("ConStr");
        }

        [HttpPost]
        [Route("signup")]
        public void SignUp(SignUpViewModel vm)
        {
            var user = new User
            {
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
            };
            var repo = new UsersRepo(_connectionString);
            repo.AddUser(user, vm.Password);
        }

        [HttpPost]
        [Route("login")]
        public User LogIn(LogInViewModel vm)
        {
            var repo = new UsersRepo(_connectionString);
            var user = repo.LogIn(vm.Email, vm.Password);
            if(user == null)
            {
                return null;
            }
            var claims = new List<Claim>
            {
                new Claim("user", vm.Email)
            };
            HttpContext.SignInAsync(new ClaimsPrincipal(
                new ClaimsIdentity(claims, "Cookies", "user", "role"))).Wait();
            return user;
        }

        [HttpPost]
        [Route("logout")]
        public void LogOut()
        {
            HttpContext.SignOutAsync().Wait();
        }

        [HttpGet]
        [Route("getcurrentuser")]
        public User GetCurrentUser()
        {
            if (!User.Identity.IsAuthenticated)
            {
                return null;
            }
            var repo = new UsersRepo(_connectionString);
            return repo.GetByEmail(User.Identity.Name);
        }
    }
}
