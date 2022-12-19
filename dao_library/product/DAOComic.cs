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

using System;
using NHibernate;

namespace biblioteca.dao_library.product
{
    public class DAOComic
    {
        private ISession session;

        public DAOComic(ISession session)
        {
            this.session = session;
        }

        public entity_library.product.Comic ObtenerComic(long id)
		{
			try
			{
				return this.session.Get<entity_library.product.Comic>(id);
			}
			catch (Exception ex)
			{
				throw new Exception("biblioteca.dao_library.product.ObtenerComic(long id): Error al obtener el item con id = " + id.ToString(), ex);
			}
		}

        public void Guardar(entity_library.product.Comic item)
		{
			try
			{
				this.session.Save(item);
			}
			catch (Exception ex)
			{
				throw new Exception("biblioteca.dao_library.product.Guardar: Error al guardar el item.", ex);
			}
		}
    }
}