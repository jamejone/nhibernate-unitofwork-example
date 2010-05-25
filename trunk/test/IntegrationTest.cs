using System.Collections.Generic;
using nhibernate_unitofwork_example;
using nhibernate_unitofwork_example.DAL;
using NUnit.Framework;

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
        public void BasicTest()
        {
            var m = new Monkey();
            m.Name = "George";
            m.FlingsPoo = true;
            m.Bananas = new List<Banana>();
            m.Bananas.Add(new Banana() { Color = "green" });
            m.Bananas.Add(new Banana() { Color = "yellow" });

            //Create new Monkey
            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                uow.Save(m);
            }

            //Delete monkey
            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                uow.Delete(m);
            }
        }

        [Test]
        public void AdvancedTest()
        {
            //Create new Monkey
            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                var temp = new Monkey();
                temp.Name = "George";
                temp.FlingsPoo = true;
                temp.Bananas = new List<Banana>();
                temp.Bananas.Add(new Banana() { Color = "green" });
                temp.Bananas.Add(new Banana() { Color = "yellow" });

                uow.Save(temp);
            }

            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                Monkey temp = uow.Get<Monkey>(1);
                Assert.IsTrue(temp.Name == "George" && temp.Bananas[1].Color == "yellow");
            }

            //Modify monkey
            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                Monkey temp = uow.Get<Monkey>(1);
                temp.Name = "Bill";
                uow.Save(temp);
            }

            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                Monkey temp = uow.Get<Monkey>(1);
                Assert.IsTrue(temp.Name == "Bill");
            }

            //Delete monkey
            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                Monkey temp = uow.Get<Monkey>(1);
                uow.Delete(temp);
            }

            using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
            {
                Monkey temp = uow.Get<Monkey>(1);
                Assert.IsNull(temp);
            }
        }
    }
}
