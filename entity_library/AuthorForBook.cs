using biblioteca.entity_library.product;

namespace entity_library
{
    public class AuthorForBook
    {
        public virtual long Id {get; set;}

        public virtual Author Author {get; set;}

        public virtual Product Product {get; set;}

        
    }
}