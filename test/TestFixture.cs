using nhibernate_unitofwork_example;
using NUnit.Framework;
using nhibernate_unitofwork_example.DAL;
using nhibernate_unitofwork_example.DAL.NHibernate;
using System.Collections.Generic;
using System.Linq;
using NHibernate;

namespace test
{
    [TestFixture]
    public class IntegrationTest
    {
        [SetUp]
        public void GenerateSchema()
        {
            var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr);

            uow.GenerateSchema();
        }

        [Test]
        public void TestDBPersistence()
        {
            var m = new Monkey();
            m.Name = "George";
            m.FlingsPoo = true;
            m.Bananas = new List<Banana>();
            m.Bananas.Add(new Banana() { Color = "green" });
            m.Bananas.Add(new Banana() { Color = "yellow" });

            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                uow.Save(m);
            }

            m.Name = "Rachael";

            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                uow.Save(m);
            }

            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                uow.Delete(m);
            }
        }
    }
}
