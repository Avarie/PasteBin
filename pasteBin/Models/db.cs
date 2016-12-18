using System.Data.Entity;

namespace pasteBin.Models
{
    public class db : DbContext
    {
        public db() : base("name=db") { }

        public DbSet<Item> Items { get; set; }
    }
}