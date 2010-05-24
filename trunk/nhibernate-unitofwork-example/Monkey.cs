using System.Collections.Generic;

namespace nhibernate_unitofwork_example
{
    public class Monkey
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual bool FlingsPoo { get; set; }
        public virtual IList<Banana> Bananas { get; set; }
    }
}
