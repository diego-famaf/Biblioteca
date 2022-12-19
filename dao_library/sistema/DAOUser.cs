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

using NHibernate;
using NHibernate.Criterion;
using System.Collections.Generic;

using entity_library.Sistema;
using System;

namespace dao_library.sistema
{
    public class DAOUser
    {
        private ISession session;

        public DAOUser(ISession session)
        {
            this.session = session;
        }
  
        public void EliminarUsuario(entity_library.Sistema.User usuario)
		{
			try
			{
				this.session.Delete(usuario);
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.EliminarUsuario(User usuario): Error al eliminar el usuario", ex);
			}
		}

		public IList<entity_library.Sistema.User> ObtenerListaUsuario(
			string query,
			List<dao_library.Utils.AtributoBusqueda> queryCols,
			dao_library.Utils.Paginado paginado,
			dao_library.Utils.Ordenamiento ordenamiento,
			out long cantidadTotal)
		{
			try
			{
				ICriteria lista = this.session.CreateCriteria<entity_library.Sistema.User>("User");
				ICriteria cantidad = this.session.CreateCriteria<entity_library.Sistema.User>("User");

				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, cantidad);

				dao_library.Utils.UtilidadesNHibernate.AgregarOrdenamiento(ordenamiento, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarPaginado(paginado, lista);

				cantidadTotal = dao_library.Utils.UtilidadesNHibernate.ObtenerCantidad(cantidad);

				IList<entity_library.Sistema.User> retorno = lista.List<entity_library.Sistema.User>();

				return retorno;
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.ObtenerListaUsuario: Error al obtener el listado de items", ex);
			}
		}

        public User ObtenerUsuario(string userName, string password)
        {
            ICriteria lista = this.session.CreateCriteria<entity_library.Sistema.User>("User");

			lista.Add(Restrictions.Eq("User.NombreUsuario", userName));
			lista.Add(Restrictions.Eq("User.Password", password));

			IList<entity_library.Sistema.User> retorno = lista.List<entity_library.Sistema.User>();

			if(retorno != null && retorno.Count > 0)
			{
				return retorno[0];
			}

			return null;
        }

        public IList<entity_library.Sistema.User> ObtenerListaUsuario(
			string query,
			List<dao_library.Utils.AtributoBusqueda> queryCols,
			dao_library.Utils.Paginado paginado,
			dao_library.Utils.Ordenamiento ordenamiento,
			List<dao_library.Utils.Asociacion> asociaciones,
			out long cantidadTotal)
		{
			try
			{
				ICriteria lista = this.session.CreateCriteria<entity_library.Sistema.User>("User");
				ICriteria cantidad = this.session.CreateCriteria<entity_library.Sistema.User>("User");

				dao_library.Utils.UtilidadesNHibernate.CrearAsociaciones(asociaciones, lista);
				dao_library.Utils.UtilidadesNHibernate.CrearAsociaciones(asociaciones, cantidad);

				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarCriteriosDeBusqueda(queryCols, query, cantidad);

				dao_library.Utils.UtilidadesNHibernate.AgregarOrdenamiento(ordenamiento, lista);
				dao_library.Utils.UtilidadesNHibernate.AgregarPaginado(paginado, lista);

				cantidadTotal = dao_library.Utils.UtilidadesNHibernate.ObtenerCantidad(cantidad);

				IList<entity_library.Sistema.User> retorno = lista.List<entity_library.Sistema.User>();

				return retorno;
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.ObtenerListaUsuario: Error al obtener el listado de items", ex);
			}
		}

		public void Guardar(entity_library.Sistema.User item)
        {
			try
			{
				this.session.Save(item);
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUsuario.Guardar: Error al guardar el item.", ex);
			}
		}

        public User ObtenerUsuario(long idUsuario)
        {
            try
			{
				return this.session.Get<entity_library.Sistema.User>(idUsuario);
			}
			catch (Exception ex)
			{
				throw new Exception("dao_library.Sistema.DAOUser.ObtenerUsuario(long idUsuario):Error al obtener el item con idUsuario =" + idUsuario.ToString(),ex);
			}
        }
    }
}