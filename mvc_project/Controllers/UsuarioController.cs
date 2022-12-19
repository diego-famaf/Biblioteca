﻿using System;
using System.Collections.Generic;
using biblioteca.dao_library;
using dao_library;
using dao_library.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using mvc_project.Models;
using mvc_project.Models.Common;
using mvc_project.Models.Login;
using mvc_project.Models.Usuario;
using service_library;

namespace mvc_project.Controllers
{
    public class UsuarioController : Controller    
    {
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(ILogger<UsuarioController> logger)
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

            UsuarioViewModel usuarioViewModel = new UsuarioViewModel
            {
                apellidoPersona = "",
                id = 0,
                nombrePersona = "",
                nombreUsuario = "",
                accion = CodigosAccion.Nuevo
            };

            return View("~/Views/Usuario/Usuario.cshtml", usuarioViewModel);
        }

        public IActionResult Editar(long idUsuario)
        {
            transversal_library.IUserService userService = new UserService();
            userService.GetUser("", "");

            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            using(DAOFactory df = new DAOFactory())
            {
                entity_library.Sistema.User user = df.DAOUser.ObtenerUsuario(idUsuario);

                UsuarioViewModel usuarioViewModel = new UsuarioViewModel 
                {
                    accion = CodigosAccion.Editar,
                    apellidoPersona = "",
                    id = 0,
                    nombrePersona = "",
                    nombreUsuario = ""
                };

                if(user != null)
                {
                    usuarioViewModel.apellidoPersona = user.Name;
                    usuarioViewModel.id = user.Id;
                    usuarioViewModel.nombrePersona = user.Name;
                    usuarioViewModel.nombreUsuario = user.UserName;
                }

                return View("~/Views/Usuario/Usuario.cshtml", usuarioViewModel);
            }

        }

        public IActionResult Ver(long idUsuario)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Redirect("~/Home/Index");
            }

            using(DAOFactory df = new DAOFactory())
            {
                entity_library.Sistema.User user = df.DAOUser.ObtenerUsuario(idUsuario);

                UsuarioViewModel usuarioViewModel = new UsuarioViewModel 
                {
                    accion = CodigosAccion.Ver,
                    apellidoPersona = "",
                    id = 0,
                    nombrePersona = "",
                    nombreUsuario = ""
                };

                if(user != null)
                {
                    usuarioViewModel.apellidoPersona = user.Name;
                    usuarioViewModel.id = user.Id;
                    usuarioViewModel.nombrePersona = user.Name;
                    usuarioViewModel.nombreUsuario = user.UserName;
                }

                return View("~/Views/Usuario/Usuario.cshtml", usuarioViewModel);
            }

        }

        [HttpPost]
        public JsonResult Listar(QueryGridModel queryGridModel)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            try
            {
                long cantidadTotal = 0;
                List<UsuarioModel> listaUsarios = new List<UsuarioModel>();

                using (DAOFactory df = new DAOFactory())
                {
                    Ordenamiento ordenamiento = obtenerOrdenamientoUsuario(queryGridModel);
                    List<Asociacion> asociaciones = obtenerAsociacionesUsuario();
                    List<AtributoBusqueda> atributosBusqueda = obtenerAtributosBusquedaUsuario();

                    Paginado paginado = new Paginado
                    {
                        Comienzo = queryGridModel.start,
                        Cantidad = queryGridModel.length
                    };

                    IList<entity_library.Sistema.User> usuarios = df.DAOUser.ObtenerListaUsuario(
                        queryGridModel.search != null ? queryGridModel.search.value : "",
                        atributosBusqueda,
                        paginado,
                        ordenamiento,
                        asociaciones,
                        out cantidadTotal);

                    foreach (entity_library.Sistema.User usuario in usuarios)
                    {
                        listaUsarios.Add(new UsuarioModel
                        {
                            id = usuario.Id,
                            apellidoPersona = usuario.Name,
                            nombrePersona = usuario.Name,
                            nombreUsuario = usuario.UserName,
                            password = usuario.Password
                        });
                    }

                    return Json(JsonReturn.SuccessWithInnerObject(new
                    {
                        draw = queryGridModel.draw,
                        recordsFiltered = cantidadTotal,
                        recordsTotal = cantidadTotal,
                        data = listaUsarios
                    }));
                }
            }
            catch (System.Exception)
            {
                return Json(JsonReturn.ErrorWithSimpleMessage("Hubo un error"));
            }
        }

        private static List<AtributoBusqueda> obtenerAtributosBusquedaUsuario()
        {
            List<AtributoBusqueda> atributosBusqueda = new List<AtributoBusqueda>();

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "Usuario.NombreUsuario",
                TipoDato = TipoDato.String
            });

            atributosBusqueda.Add(new AtributoBusqueda
            {
                NombreAtributo = "Usuario.NombreCompleto",
                TipoDato = TipoDato.String
            });

            return atributosBusqueda;
        }

        private static List<Asociacion> obtenerAsociacionesUsuario()
        {
            List<Asociacion> asociaciones = new List<Asociacion>();

            return asociaciones;
        }

        private static Ordenamiento obtenerOrdenamientoUsuario(
            QueryGridModel modeloConsulta)
        {
            Ordenamiento ordenamiento = new Ordenamiento
            {
                Atributo = "Usuario.NombreCompleto",
                Direccion = "asc"
            };

            if (modeloConsulta.order != null &&
                modeloConsulta.order.Count > 0)
            {
                int columnIndex = modeloConsulta.order[0].column;
                string col = modeloConsulta.columns[columnIndex].data;

                if (col == "nombrePersona") col = "Usuario.NombreCompleto";
                else if(col == "nombreUsuario") col = "Usuario.NombreUsuario";
                else col = "Usuario.NombreCompleto";

                ordenamiento.Atributo = col;
                ordenamiento.Direccion =
                    modeloConsulta.order[0].dir == DirectionModel.desc ? "desc" : "asc";
            }

            return ordenamiento;
        }    

        [HttpPost]
        public JsonResult Guardar(UsuarioModel usuarioModel)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            try
            {
                using (DAOFactory df = new DAOFactory())
                {
                    entity_library.Sistema.User usuario = df.DAOUser.ObtenerUsuario(usuarioModel.id);
                   
                    if(usuario == null)
                    {
                        usuario = new entity_library.Sistema.User();
                        
                    }

                    usuario.Name = usuarioModel.nombrePersona;
                    usuario.UserName = usuarioModel.nombreUsuario;
                    
                    if((usuarioModel.id != 0 && !string.IsNullOrEmpty(usuarioModel.password)) ||
                        usuarioModel.id == 0)
                    {
                        //Si estoy editando y la pass cambió, o si el usuario es nuevo.
                        usuario.Password = usuarioModel.password;
                    }

                    df.BeginTrans();
                    df.DAOUser.Guardar(usuario);
                    df.Commit();

                    return Json(JsonReturn.SuccessWithoutInnerObject());
                }
            }
            catch (Exception)
            {
                return Json(JsonReturn.ErrorWithSimpleMessage("Hubo un error"));
            }
        }

        [HttpPost]
        public JsonResult Eliminar(long id)
        {
            LoginModel loginModel = HttpContext.Session.Get<LoginModel>("UsuarioLogueado");

            if(loginModel == null)
            {
                return Json(Models.Common.JsonReturn.Redirect("Home/Index"));
            }

            try
            {
                using(DAOFactory df = new DAOFactory())
                {
                    entity_library.Sistema.User usuario = df.DAOUser.ObtenerUsuario(id);

                    if(usuario == null)
                    {
                        return Json(JsonReturn.ErrorWithSimpleMessage("El usuario no existe"));
                    }

                    df.BeginTrans();
                    df.DAOUser.EliminarUsuario(usuario);
                    df.Commit();
                }

                return Json(JsonReturn.SuccessWithoutInnerObject());
            }
            catch(Exception)
            {
                return Json(JsonReturn.ErrorWithSimpleMessage("Se generó un error"));
            }
        }
    }
}
