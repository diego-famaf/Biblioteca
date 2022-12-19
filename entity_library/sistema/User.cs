

namespace entity_library.Sistema
{
    public partial class User
    {
        public virtual long Id { get; set; }

        public virtual string UserName { get; set; }

        public virtual string Password { get; set; }

        public virtual string Name { get; set; }
    }
}
