using TheatreTicketSystem.DAL.Repositories;  

namespace TheatreTicketSystem.DAL.UoW
{
    public interface IUnitOfWork : IDisposable
    {
        IAuthorRepository Authors { get; }
        IGenreRepository Genres { get; }
        IHallRepository Halls { get; }
        IPerformanceRepository Performances { get; }
        ISeatRepository Seats { get; }
        ITicketRepository Tickets { get; }

        int Complete();
    }
}