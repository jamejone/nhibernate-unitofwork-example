using FluentNHibernate.Mapping;

namespace nhibernate_unitofwork_example.DAL.NHibernate.Maps
{
    public class BananaMap : ClassMap<Banana>
    {
        public BananaMap()
        {
            Id(x => x.Id);
            Map(x => x.Color);
        }
    }
}
