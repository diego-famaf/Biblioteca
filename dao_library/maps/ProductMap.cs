using biblioteca.entity_library.product;
using FluentNHibernate.Mapping;

/***********************************************************************************************************
    Copyright (C) 2021 ITSC - Ing. de Software

    Este programa es software libre: usted puede redistribuirlo y/o modificarlo 
    bajo los términos de la Licencia Pública General GNU publicada 
    por la Fundación para el Software Libre, ya sea la versión 3 
    de la Licencia, o (a su elección) cualquier versión posterior.

    Este programa se distribuye con la esperanza de que sea útil, pero 
    SIN GARANTÍA ALGUNA; ni siquiera la garantía implícita 
    MERCANTIL o de APTITUD PARA UN PROPÓSITO DETERMINADO. 
    Consulte los detalles de la Licencia Pública General GNU para obtener 
    una información más detallada. 

    Debería haber recibido una copia de la Licencia Pública General GNU 
    junto a este programa. 
    En caso contrario, consulte http://www.gnu.org/licenses/gpl-3.0.html
 **********************************************************************************************************/

namespace biblioteca.dao_library.product
{
    public class ProductMap : ClassMap<Product>
    {
        public ProductMap()
        {
            Table("producto");

            Id(x => x.Id, "id").GeneratedBy.Increment();

            Map(x => x.Author, "author");
            
            Map(x => x.Title, "title");

            Map(x => x.Review, "review");

            Map(x => x.Editorial, "editorial");

            Map(x => x.Collection, "collection");

            Map(x => x.Score, "score");

            Map(x => x.NumberPages, "number_pages");
            
            DiscriminateSubClassesOnColumn("discriminador")
                .AlwaysSelectWithValue()
                .Length(20)
                .Not.Nullable()
                .CustomType<string>();
        }
    }

    public class BookMap : SubclassMap<Book>
    {
        public BookMap()
        {
            DiscriminatorValue("Book");
            
        }
    }

    public class ComicMap : SubclassMap<Comic>
    {
        public ComicMap()
        {
            DiscriminatorValue("Comic");
         
            Map(x => x.Artists, "artists");
        }
    }
}
