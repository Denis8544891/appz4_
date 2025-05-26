using TheatreTicketSystem.DAL.Repositories;

namespace TheatreTicketSystem.DAL.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly TheatreDbContext _context;

        public IAuthorRepository Authors { get; private set; }
        public IGenreRepository Genres { get; private set; }
        public IHallRepository Halls { get; private set; }
        public IPerformanceRepository Performances { get; private set; }
        public ISeatRepository Seats { get; private set; }
        public ITicketRepository Tickets { get; private set; }

        public UnitOfWork(TheatreDbContext context)
        {
            _context = context;
            Authors = new AuthorRepository(_context);
            Genres = new GenreRepository(_context);
            Halls = new HallRepository(_context);
            Performances = new PerformanceRepository(_context);
            Seats = new SeatRepository(_context);
            Tickets = new TicketRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}