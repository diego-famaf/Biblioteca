Ahora un ejemplo completo de trabajo:

Hay que hacer una ventana que liste a todos los usuarios.

1) Creo una vista Index.cshtml en la carpeta User
2) Creo un archivo UserController.cs y adentro le programo la función:

public IActionResult Index()
{
   return View(); //Devuelve la vista que está en Views/User/Index.cshtml ¿por qué?
}

y programo en ese mismo controller la función:

public JsonResult GetUsersGrid(DataGrid dataGrid, UsersGridExtraParams extraParams)
{
   List<UserDTO> users = new List<UserDTO>();
   users.Add(new UserDTO
   {
      UserName = "jmores",
      Email = "jmores@gmail.com"
   });

   users.Add(new UserDTO
   {
      UserName = "dgomez",
      Email = "dgomez@gmail.com"
   });

   return new GetUsersGridResponse
   {
      totalCount = 20,
      pageSize = 2,
      pageNumber = 1,
      list = users
   };
}

Compilo y pusheo.
Le digo al del front, haceme una grilla bonita cómo la del mock que le pasaste al cliente, yo ya te mockee un par de datos en el backend para que pruebes.
Mientras lo voy haciendo en serio.

Paso 2) Trabajamos en paralelo:
El del front hace un table con un createGrid, que hace una llamada Ajax (cómo en uso de componentes).
Mientras el del back hace:

programa el UserDTO (lo que retorna).

Y cambia el código:

public JsonResult GetUsersGrid(DataGrid dataGrid, UsersGridExtraParams extraParams)
{
   IUserService userService = new UserService();

   return new GetUsersGridResponse
   {
      totalCount = 20,
      pageSize = 2,
      pageNumber = 1,
      list = userService.GetUsersGrid(dataGrid, extraParams)
   };
}

Progama la clase UserService, que implementa la interfaz IUserService, que tiene la funcion GetUsersGrid.
Además programa la clase UserBussiness, que implementa la interfaz IUserBussiness.
Y en UserBussiness hace:

public List<UserDTO> GetUsersGrid(DataGrid dataGrid, ExtraParamsGridUser extraParams)
{
   List<UserDTO> usersReturn = new List<UserDTO>();
   using(IRepository repository = RepositoryFactory.Instance.CreateRepository())
   {
      List<entity_library.User> users = repository.UnitOfWorkUser.GetUsers(dataGrid, extraParams);

      foreach(User user in users)
      {
         usersReturn.Add(new UserDTO{ //Mapeo la entity a DTO
            UserName = user.UserName,
            Email = user.Email
         });
      }
   }

   return usersReturn;
}

Cómo la entrada y salida del controller sigue igual, no cambié nada, entonces viene el paso 3)
Hago commit y push de mi código.
El del front hace lo mismo.
Mergeamos.
Y cómo lo que el manda es lo que yo recibo y lo que yo devuelvo es lo que el espera, todo sale andando a la perfección.

Somos todos felices, y comemos perdices.
--------------------------------------------------------------------------------------------------------------------------------------------