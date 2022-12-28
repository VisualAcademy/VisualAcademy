using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Application.Common.Interfaces
{
    public interface IDatabaseService
    {
        DbSet<Todo> Todos { get; }

        void Save();
    }
}
