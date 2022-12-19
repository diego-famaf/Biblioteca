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
using biblioteca.dao_library.product;
using NHibernate;
using dao_library.sistema;

namespace dao_library
{
    public class DAOFactory : IDisposable
    {
        #region atributos privados
        private ISession session = null;
        private ITransaction transaction = null;
        #endregion
        
        #region Constructor
        public DAOFactory()
        {
            this.session = Database.Instance.SessionFactory.OpenSession();
        }
        #endregion

        #region métodos de la base de datos
        public bool BeginTrans()
        {
            try
            {
                this.transaction = this.session.BeginTransaction();
                return true;
            }
            catch(System.Exception e)
            {
                throw new System.Exception(
                    "ejemplo.dao.NHibernate.NHibernateDAOFactory.BeginTrans()", 
                    e);
            }
        }

        public bool Commit()
        {
            try
            {
                this.transaction.Commit();

                this.transaction = null;

                return true;
            }
            catch(System.Exception e)
            {
                throw new System.Exception(
                    "ejemplo.dao.NHibernate.NHibernateDAOFactory.Commit()", 
                    e);
            }
        }

        public void Rollback()
        {
            try
            {
                this.transaction.Rollback();

                this.transaction = null;
            }
            catch(System.Exception e)
            {
                throw new System.Exception("ejemplo.dao.NHibernate.NHibernateDAOFactory.Rollback()", e);
            }
        }

        public void Close()
        {
            try
            {
                if(this.transaction != null && this.transaction.IsActive)
                {
                    this.transaction.Rollback();
                }

                this.session.Close();
            }
            catch(System.Exception e)
            {
                throw new System.Exception("ejemplo.dao.NHibernate.NHibernateDAOFactory.Close()", e);
            }
        }
        
        public void Dispose()
        {
            try
            {
                this.Close();
            }
            catch(System.Exception e)
            {
                throw new System.Exception("ejemplo.dao.NHibernate.NHibernateDAOFactory.Dispose()", e);
            }
        }
        #endregion

        #region DAOs: Agregar los DAOs de ustedes
        private DAOUser daoUser = null;

        public DAOUser DAOUser
        { 
            get 
            {
                if(this.daoUser == null)
                {
                    this.daoUser = new DAOUser(this.session);
                }

                return daoUser;
            }
        }

        private DAOProduct daoProduct = null;

        public DAOProduct DAOProduct 
        { 
            get 
            {
                if(this.daoProduct == null)
                {
                    this.daoProduct = new DAOProduct(this.session);
                }

                return daoProduct;
            }
        }
        
        private DAOBook daoBook = null;

        public DAOBook DAOBook 
        { 
            get 
            {
                if(this.daoBook == null)
                {
                    this.daoBook = new DAOBook(this.session);
                }

                return daoBook;
            }
        }

        private DAOComic daoComic = null;

        public DAOComic DAOComic 
        { 
            get 
            {
                if(this.daoComic == null)
                {
                    this.daoComic = new DAOComic(this.session);
                }

                return daoComic;
            }
        }
        #endregion
    }
}