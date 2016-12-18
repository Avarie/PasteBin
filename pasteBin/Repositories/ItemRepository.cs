using System.Data.Entity;
using pasteBin.Models;

namespace pasteBin.Repositories
{
    public class ItemRepository : EntityFrameworkRepository<Item>
    {
        public ItemRepository(DbContext context) : base(context) { }
    }
}