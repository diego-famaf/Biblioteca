using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc_project.Models;
using mvc_project.Models.Common;
using mvc_project.Models.Login;

using service_library;
using mvc_project.Models.Books;
namespace mvc_project.Controllers
{
    public class BooksController : Controller
    {
        private readonly ILogger<BooksController> _logger;

        public BooksController(ILogger<BooksController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>(
                                "UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            return View();
        }

        public IActionResult Nuevo()
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            BooksViewModel booksViewModel = new BooksViewModel
            {
                apellidoAuthor = "",
                nombreBooks = "",
                id = 0,
                nombreAuthor = "",
                editorial = "",
                collection = "",
                score = 0,
                review = "",
                


                accion = CodigosAccion.Nuevo
            };

            return View("~/Views/Books/Books.cshtml", booksViewModel);
        }

        public IActionResult Editar(long idBooks)
        {
            transversal_library.IUserService userService = new UserService();
            userService.GetUser("", "");

            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            List<BooksModel> list = HttpContext.Session.Get<List<BooksModel>>("ListaLibros");

            if(list == null)
            {
                list = new List<BooksModel>();
            }

            BooksModel booksModel = list.Find(x => x.id == idBooks);
            
            BooksViewModel booksViewModel = new BooksViewModel 
            {
                accion = CodigosAccion.Editar,
                apellidoAuthor = booksModel.apellidoAuthor, 
                id = booksModel.id,
                nombreBooks = booksModel.nombreBooks,
                nombreAuthor = booksModel.nombreAuthor,
                editorial = booksModel.editorial,
                collection = booksModel.collection,
                score = booksModel.score,
                review = booksModel.review,
            };

            return View("~/Views/Books/Books.cshtml", booksViewModel);
        }

        public IActionResult Ver(long idBooks)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            List<BooksModel> list = HttpContext.Session.Get<List<BooksModel>>("ListaLibros");

            if(list == null)
            {
                list = new List<BooksModel>();
            }

            BooksModel booksModel = list.Find(x => x.id == idBooks);

            BooksViewModel booksViewModel = new BooksViewModel 
            {
                accion = CodigosAccion.Ver,
                apellidoAuthor = booksModel.apellidoAuthor,
                id = booksModel.id,
                nombreBooks = booksModel.nombreBooks,
                nombreAuthor = booksModel.nombreAuthor,
                editorial = booksModel.editorial,
                collection = booksModel.collection,
                score = booksModel.score,
                review = booksModel.review,
            };

             return View("~/Views/Books/Books.cshtml", booksViewModel);;
        }

        [HttpPost]
        public JsonResult Listar(QueryGridModel queryGridModel)
        {
            List<BooksModel> list = HttpContext.Session.Get<List<BooksModel>>("ListaLibros");
            
            if(list == null)
            {
                list = new List<BooksModel>();
            }

            IEnumerable<BooksModel> listaLibros = list;
            if(queryGridModel.order != null && queryGridModel.order.Count > 0)
            {
                if(queryGridModel.columns[queryGridModel.order[0].column].name == "nombreBooks")
                {
                    if(queryGridModel.order[0].dir == DirectionModel.asc)
                    {
                        listaLibros = list.OrderBy(x => x.nombreBooks);
                    }
                    else
                    {
                        listaLibros = list.OrderByDescending(x => x.nombreBooks);
                    }
                }
                else if(queryGridModel.columns[queryGridModel.order[0].column].name == "apellidoAuthor")
                {
                    if(queryGridModel.order[0].dir == DirectionModel.asc)
                    {
                        listaLibros = list.OrderBy(x => x.apellidoAuthor);
                    }
                    else
                    {
                        listaLibros = list.OrderByDescending(x => x.apellidoAuthor);
                    }
                }
                else if(queryGridModel.columns[queryGridModel.order[0].column].name == "nombreAuthor")
                {
                    if(queryGridModel.order[0].dir == DirectionModel.asc)
                    {
                        listaLibros = list.OrderBy(x => x.nombreAuthor);
                    }
                    else
                    {
                        listaLibros = list.OrderByDescending(x => x.nombreAuthor);
                    }
                }
                else if(queryGridModel.columns[queryGridModel.order[0].column].name == "score")
                {
                    if(queryGridModel.order[0].dir == DirectionModel.asc)
                    {
                        listaLibros = list.OrderBy(x => x.score);
                    }
                    else
                    {
                        listaLibros = list.OrderByDescending(x => x.score);
                    }
                }
                else if(queryGridModel.columns[queryGridModel.order[0].column].name == "collection")
                {
                    if(queryGridModel.order[0].dir == DirectionModel.asc)
                    {
                        listaLibros = list.OrderBy(x => x.collection);
                    }
                    else
                    {
                        listaLibros = list.OrderByDescending(x => x.collection);
                    }
                }
                else if(queryGridModel.columns[queryGridModel.order[0].column].name == "editorial")
                {
                    if(queryGridModel.order[0].dir == DirectionModel.asc)
                    {
                        listaLibros = list.OrderBy(x => x.editorial);
                    }
                    else
                    {
                        listaLibros = list.OrderByDescending(x => x.editorial);
                    }
                }
            }

            if(queryGridModel.search != null && queryGridModel.search.value != null)
            {
                listaLibros = listaLibros.Where(x => x.nombreBooks.Contains(queryGridModel.search.value));
            }

            return Json(JsonReturn.SuccessWithInnerObject(new
            {
                draw = queryGridModel.draw,
                recordsFiltered = listaLibros.Count(),
                recordsTotal = list.Count,
                data = listaLibros
            }));
        }

        [HttpPost]
        public JsonResult Guardar(BooksModel booksModel)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            List<BooksModel> list = HttpContext.Session.Get<List<BooksModel>>("ListaLibros");

            if(list == null)
            {
                list = new List<BooksModel>();
            }

            if(booksModel.id == 0)
            {
                booksModel.id = list.Count + 1;
                list.Add(booksModel);
            }
            else
            {
                BooksModel libro = list.Find(x => x.id == booksModel.id);
                libro.apellidoAuthor = booksModel.apellidoAuthor;
                libro.nombreBooks = booksModel.nombreBooks;
                libro.nombreAuthor = booksModel.nombreAuthor;
                libro.editorial = booksModel.editorial;
                libro.collection = booksModel.collection;
                libro.score = booksModel.score;
                libro.review = booksModel.review;
            }

            HttpContext.Session.Set<List<BooksModel>>("ListaLibros", list);

            return Json(JsonReturn.SuccessWithoutInnerObject());
        }

        [HttpPost]
        public JsonResult Eliminar(long id)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            List<BooksModel> list = HttpContext.Session.Get<List<BooksModel>>("ListaLibros");

            if(list == null)
            {
                list = new List<BooksModel>();
            }

            BooksModel libro = list.Find(x => x.id == id);
            
            if(libro == null)
            {
                return Json(Models.Common.JsonReturn.ErrorWithSimpleMessage("El libro que desea eliminar no existe más"));
            }
            
            list.Remove(libro);
            
            HttpContext.Session.Set<List<BooksModel>>("ListaLibros", list);

            return Json(JsonReturn.SuccessWithoutInnerObject());
        }
    }
}
