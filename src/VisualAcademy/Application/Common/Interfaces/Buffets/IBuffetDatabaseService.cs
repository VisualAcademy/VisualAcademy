using Domain.Entities.Buffets;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces.Buffets
{
    public interface IBuffetDatabaseService
    {
        DbSet<Broth> Broths { get; set; }

        void Save();
    }
}
