# Unit of Work with NHibernate #

This project serves as an example on how to implement the Unit of Work [facade](http://en.wikipedia.org/wiki/Facade_pattern) over NHibernate (with FluentNHibernate). There is no official definition of the Unit of Work facade and many different implementations exist on the web. The implementation I have provided here aims to provide a convenient means to utilize NHibernate's transactional features.

You are provided the basic 4 CRUD operations plus a LINQ-compatible `GetAll<T>()`.

## Comparison to plain NHibernate ##

**Plain NHibernate**

```
ISessionFactory sessionFactory = GetSessionFactory();
using (ISession session = sessionFactory.OpenSession())
{
    using (ITransaction transaction = session.BeginTransaction())
    {
        session.SaveOrUpdate(myObject);

        transaction.Commit();
    }
    session.Close();
}
```

**Unit of Work**

```
using (var uow = new NHUnitOfWork(Properties.Settings.Default.DBConnStr))
{
    uow.Save(myObject);
}
```