using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NHibernate.Cfg;
using NHibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;

namespace nhibernate_unitofwork_example.DAL
{
    /// <summary>
    /// A façade for 
    /// </summary>
    public class NHUnitOfWork : IDisposable
    {
        /// <summary>
        /// If true, will roll back the transaction upon NHUnitOfWork.Dispose().
        /// </summary>
        /// <seealso cref="NHibernate.ITransaction.Rollback()"/>
        public bool Rollback { get; set; }

        protected static Configuration config;
        protected static ISessionFactory sessionFactory;
        protected ISession Session { get; private set; }
        protected ITransaction transaction;

        /// <summary>
        /// Configures the connection to the database and initializes the unit of work.
        /// </summary>
        /// <param name="databaseConnectionString">The connection string to the database.</param>
        public NHUnitOfWork(string databaseConnectionString)
        {
            if (config == null)
            {
                //TODO: Configure your database settings here.
                config = Fluently.Configure()
                    .Database(
                        MsSqlConfiguration
                        .MsSql2008
                        .ConnectionString(databaseConnectionString))
                        .Mappings(m => m.FluentMappings.AddFromAssemblyOf<NHUnitOfWork>())
                    .BuildConfiguration();

                sessionFactory = config.BuildSessionFactory();
            }

            Session = sessionFactory.OpenSession();
            transaction = Session.BeginTransaction();

            Rollback = false;

            //The following lines are needed in order to force DLL's to copy out during build. (optimization excludes them otherwise)
            var pf = new NHibernate.ByteCode.Castle.ProxyFactory();
            var gn = new Castle.Core.GraphNode();
            var dpb = new Castle.DynamicProxy.DefaultProxyBuilder();
        }

        /// <summary>
        /// Generates the schema for the given configuration.
        /// </summary>
        public void GenerateSchema()
        {
            new SchemaExport(config).Execute(true, true, false);
        }

        /// <summary>
        /// Commits the unit of work transaction.
        /// </summary>
        public void Dispose()
        {
            if (Rollback)
                transaction.Rollback();
            else
                transaction.Commit();

            Session.Close();
        }

        #region "Repository Functions"

        /// <summary>
        /// Saves or updates the object to the database, depending on the value of its identifier property.
        /// </summary>
        /// <param name="value">A transient instance containing a new or updated state.</param>
        public void Save(object value)
        {
            Session.SaveOrUpdate(value);
        }
        public void Save(IList<object> values)
        {
            foreach (var value in values)
                Save(value);
        }

        /// <summary>
        /// Removes a persistent instance from the database.
        /// </summary>
        /// <param name="value">The instance to be removed.</param>
        public void Delete(object value)
        {
            Session.Delete(value);
        }
        public void Delete(IList<object> values)
        {
            foreach (var value in values)
                Delete(value);
        }

        /// <summary>
        /// Returns a strong typed persistent instance of the given named entity with the given identifier, or null if there is no such persistent instance.
        /// </summary>
        /// <typeparam name="T">The type of the given persistant instance.</typeparam>
        /// <param name="id">An identifier.</param>
        public T Get<T>(object id)
        {
            T returnVal = Session.Get<T>(id);
            return returnVal;
        }

        /// <summary>
        /// Returns a list of all instances of type T from the database.
        /// </summary>
        /// <typeparam name="T">The type of the given persistant instance.</typeparam>
        public IList<T> GetAll<T>() where T : class
        {
            IList<T> returnVal = Session.CreateCriteria<T>().List<T>();
            return returnVal;
        }

        #endregion
    }
}
