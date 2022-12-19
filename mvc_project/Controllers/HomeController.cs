using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc_project.Models;
using mvc_project.Models.Login;


namespace mvc_project.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            mvc_project.Models.Login.LoginViewModel loginViewModel =
                new Models.Login.LoginViewModel();
            {
                loginViewModel.isLogged = true;
                loginViewModel.message = "";
            };
            return View(loginViewModel);
        }

        public IActionResult Login(LoginModel loginModel)
        {
            mvc_project.Models.Login.LoginViewModel loginViewModel =
                new Models.Login.LoginViewModel();

            if(string.IsNullOrEmpty(loginModel.userName) || string.IsNullOrEmpty(loginModel.password))
            {
                loginViewModel.isLogged = false;
                loginViewModel.message = "Debe ingresar un nombre de usuario o password";

                return View("~/Views/Home/Index.cshtml",loginViewModel);
            }
            
            if(loginModel.userName.Equals("Admin") && (loginModel.password.Equals("Admin")))
            {
            HttpContext.Session.Set<LoginModel>(
                                "UsuarioLogueado",
                                loginModel);
                
                return Redirect("~/Panel/Index");
            }
            
            loginViewModel.isLogged = false;
            loginViewModel.message = "Nombre de usuario o contraseña incorrecto";

            return View("~/Views/Home/Index.cshtml",loginViewModel);
        }
        

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

