using mvc_project.Models.Common;

namespace mvc_project.Models.Books
{
    public class BooksViewModel
    {
        public long id { get; set; }

        public string nombreBooks { get; set; }

        public string nombreAuthor { get; set; }

        public string apellidoAuthor { get; set; }

        

        public string editorial { get; set; }

        public string collection { get; set; }

        public int score { get; set; }

        public string review { get; set; }

        public string typeBooks {get; set; }

        public CodigosAccion accion { get; set; }
    }
}
