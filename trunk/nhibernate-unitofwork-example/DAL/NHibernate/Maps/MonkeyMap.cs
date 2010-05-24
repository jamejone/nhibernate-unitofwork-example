using FluentNHibernate.Mapping;

namespace nhibernate_unitofwork_example.DAL.NHibernate.Maps
{
    public class MonkeyMap : ClassMap<Monkey>
    {
        public MonkeyMap()
        {
            Id(x => x.Id);
            Map(x => x.Name);
            Map(x => x.FlingsPoo);
            HasMany<Banana>(x => x.Bananas)
                .Cascade.All();
        }
    }
}
