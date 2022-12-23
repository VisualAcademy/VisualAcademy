using Application.Common.Interfaces.Buffets;
using Domain.Entities.Buffets;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    public class BuffetDatabaseService : DbContext, IBuffetDatabaseService
    {
        public DbSet<Broth> Broths { get; set; }

        public void Save()
        {
            this.SaveChanges();
        }
    }
}
