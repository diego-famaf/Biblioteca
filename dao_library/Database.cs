using System;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;

namespace dao_library
{	public class Database
	{
		//Modificar la cadena de conexión
        private static string connectionString = 
            "Server=localhost;" +
            "Database=bd_biblioteca;" +
            "Uid=root;" + 
			"Pwd=password;" +
			"SSL Mode=None;";

		#region Métodos del singleton
		private static Database instance = null;
		public static Database Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new Database();
				}

				return instance;
			}
		}

		private Database()
		{
			this.sessionFactory = this.CreateSessionFactory();
		}
		#endregion

		#region SessionFactory, único método público
		private ISessionFactory sessionFactory = null;

		public ISessionFactory SessionFactory
		{
			get
			{
				return this.sessionFactory;
			}
		}
		#endregion

		#region Método que deben cambiar si no usan MySQL
		private ISessionFactory CreateSessionFactory()
		{
			return Fluently.Configure()
				.Database(
					MySQLConfiguration.Standard.ConnectionString(
						entity_library.Comun.Configuracion.Instance.DefaultStringConnection)
				)
				.Mappings(m => m.FluentMappings
					.AddFromAssemblyOf<Database>())
				.BuildSessionFactory();
		}
		#endregion
	}
}