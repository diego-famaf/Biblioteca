/***********************************************************************************************************
    Copyright (C) 2022 ITSC - Ing. de Software

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


using System.Collections.Generic;
using entity_library;

namespace biblioteca.entity_library.product
{
    public abstract class Product
    {
        public virtual long Id { get; set; }

        public virtual string Title { get; set;}

        public virtual string Review { get; set;}

        public virtual string Editorial { get; set;}

        public virtual string Collection { get; set;}

        public virtual long Score { get; set;}

        public virtual int NumberPages {get; set;}

        public virtual string Author {get; set;}

        public List<AuthorForBook> Authors {get; set;} 
    }
}