namespace TheatreTicketSystem.PL.Models
{
    // DTO для отримання списку вистав
    public class PerformanceListDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PerformanceDate { get; set; }
        public decimal BasePrice { get; set; }
        public string AuthorName { get; set; }
        public string GenreName { get; set; }
        public string HallName { get; set; }
    }

    // DTO для детальної інформації про виставу
    public class PerformanceDetailDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PerformanceDate { get; set; }
        public TimeSpan Duration { get; set; }
        public decimal BasePrice { get; set; }

        public AuthorDto Author { get; set; }
        public GenreDto Genre { get; set; }
        public HallDto Hall { get; set; }

        public int AvailableTickets { get; set; }
        public int SoldTickets { get; set; }
        public int TotalTickets { get; set; }
    }

    // DTO для автора
    public class AuthorDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Biography { get; set; }
        public DateTime? BirthDate { get; set; }
    }

    // DTO для жанру
    public class GenreDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }

    // DTO для залу
    public class HallDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public string Description { get; set; }
    }

    // DTO для місця
    public class SeatDto
    {
        public int Id { get; set; }
        public int Row { get; set; }
        public int Number { get; set; }
        public bool IsVIP { get; set; }
        public bool IsAvailable { get; set; }
    }

    // DTO для квитка
    public class TicketDto
    {
        public int Id { get; set; }
        public DateTime? PurchaseDate { get; set; }
        public decimal Price { get; set; }
        public bool IsSold { get; set; }
        public SeatDto Seat { get; set; }
        public PerformanceListDto Performance { get; set; }
    }

    // DTO для списку авторів з кількістю вистав
    public class AuthorListDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public DateTime? BirthDate { get; set; }
        public int PerformancesCount { get; set; }
    }

    // DTO для списку жанрів з кількістю вистав
    public class GenreListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PerformancesCount { get; set; }
    }

    // DTO для списку залів
    public class HallListDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int PerformancesCount { get; set; }
    }
}