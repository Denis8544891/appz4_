using Microsoft.Extensions.DependencyInjection;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.ConsoleUI
{
    public class TicketMenu
    {
        private readonly TicketService _ticketService;
        private readonly PerformanceService _performanceService;

        public TicketMenu(IServiceProvider serviceProvider)
        {
            _ticketService = serviceProvider.GetRequiredService<TicketService>();
            _performanceService = serviceProvider.GetRequiredService<PerformanceService>();
        }

        public void Show()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("======= УПРАВЛІННЯ КВИТКАМИ =======");
                Console.WriteLine("1. Переглянути всі квитки на виставу");
                Console.WriteLine("2. Переглянути доступні квитки на виставу");
                Console.WriteLine("3. Переглянути продані квитки на виставу");
                Console.WriteLine("4. Продати квиток");
                Console.WriteLine("5. Повернути квиток");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowTicketsForPerformance(false);
                        break;
                    case "2":
                        ShowTicketsForPerformance(true, false);
                        break;
                    case "3":
                        ShowTicketsForPerformance(true, true);
                        break;
                    case "4":
                        SellTicket();
                        break;
                    case "5":
                        ReturnTicket();
                        break;
                    case "0":
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Невірний вибір. Натисніть будь-яку клавішу, щоб продовжити...");
                        Console.ReadKey();
                        break;
                }
            }
        }

        private void ShowTicketsForPerformance(bool filtered, bool soldOnly = false)
        {
            Console.Clear();
            string header = filtered
                ? (soldOnly ? "ПРОДАНІ КВИТКИ НА ВИСТАВУ" : "ДОСТУПНІ КВИТКИ НА ВИСТАВУ")
                : "УСІ КВИТКИ НА ВИСТАВУ";

            Console.WriteLine($"======= {header} =======");

            var performances = _performanceService.GetAllPerformancesWithDetails();
            if (!performances.Any())
            {
                Console.WriteLine("Список вистав порожній.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Оберіть виставу:");
            foreach (var performance in performances)
            {
                Console.WriteLine($"{performance.Id}. {performance.Title} ({performance.PerformanceDate.ToShortDateString()})");
            }

            Console.Write("Введіть ID вистави: ");
            if (!int.TryParse(Console.ReadLine(), out int performanceId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var selectedPerformance = performances.FirstOrDefault(p => p.Id == performanceId);
            if (selectedPerformance == null)
            {
                Console.WriteLine("Виставу з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"======= КВИТКИ НА ВИСТАВУ: {selectedPerformance.Title} ({selectedPerformance.PerformanceDate.ToShortDateString()}) =======");

            IEnumerable<Ticket> tickets;
            if (filtered)
            {
                tickets = soldOnly
                    ? _ticketService.GetSoldTicketsForPerformance(performanceId)
                    : _ticketService.GetAvailableTicketsForPerformance(performanceId);
            }
            else
            {
                tickets = _ticketService.GetTicketsForPerformance(performanceId);
            }

            if (!tickets.Any())
            {
                string message = filtered
                    ? (soldOnly ? "Для цієї вистави ще немає проданих квитків." : "Для цієї вистави немає доступних квитків.")
                    : "Для цієї вистави ще не створено квитків.";

                Console.WriteLine(message);

                if (!filtered || !soldOnly)
                {
                    Console.Write("Бажаєте створити квитки для цієї вистави? (так/ні): ");
                    if (Console.ReadLine()?.ToLower() == "так")
                    {
                        try
                        {
                            _ticketService.CreateTicketsForPerformance(performanceId);
                            Console.WriteLine("Квитки для вистави успішно створено!");

                            // Отримуємо створені квитки
                            tickets = filtered && !soldOnly
                                ? _ticketService.GetAvailableTicketsForPerformance(performanceId)
                                : _ticketService.GetTicketsForPerformance(performanceId);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Помилка при створенні квитків: {ex.Message}");
                            Console.ReadKey();
                            return;
                        }
                    }
                    else
                    {
                        Console.ReadKey();
                        return;
                    }
                }
                else
                {
                    Console.ReadKey();
                    return;
                }
            }

            // Групуємо квитки за рядами для зручнішого відображення
            var ticketsByRow = tickets.GroupBy(t => t.Seat.Row)
                                      .OrderBy(g => g.Key);

            foreach (var row in ticketsByRow)
            {
                Console.WriteLine($"Ряд {row.Key}:");

                foreach (var ticket in row.OrderBy(t => t.Seat.Number))
                {
                    string status = ticket.IsSold ? "Продано" : "Доступний";
                    string vipStatus = ticket.Seat.IsVIP ? " (VIP)" : "";

                    Console.WriteLine($"  ID: {ticket.Id}, Місце: {ticket.Seat.Number}{vipStatus}, Ціна: {ticket.Price} грн, Статус: {status}");

                    if (ticket.IsSold && ticket.PurchaseDate.HasValue)
                    {
                        Console.WriteLine($"    Дата продажу: {ticket.PurchaseDate.Value.ToString()}");
                    }
                }

                Console.WriteLine();
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void SellTicket()
        {
            Console.Clear();
            Console.WriteLine("======= ПРОДАЖ КВИТКА =======");

            var performances = _performanceService.GetUpcomingPerformances();
            if (!performances.Any())
            {
                Console.WriteLine("Немає майбутніх вистав для продажу квитків.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine("Оберіть виставу:");
            foreach (var performance in performances)
            {
                Console.WriteLine($"{performance.Id}. {performance.Title} ({performance.PerformanceDate.ToShortDateString()})");
            }

            Console.Write("Введіть ID вистави: ");
            if (!int.TryParse(Console.ReadLine(), out int performanceId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var selectedPerformance = performances.FirstOrDefault(p => p.Id == performanceId);
            if (selectedPerformance == null)
            {
                Console.WriteLine("Виставу з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            var availableTickets = _ticketService.GetAvailableTicketsForPerformance(performanceId).ToList();
            if (!availableTickets.Any())
            {
                Console.WriteLine("Для цієї вистави немає доступних квитків.");
                Console.Write("Бажаєте створити квитки для цієї вистави? (так/ні): ");

                if (Console.ReadLine()?.ToLower() == "так")
                {
                    try
                    {
                        _ticketService.CreateTicketsForPerformance(performanceId);
                        Console.WriteLine("Квитки для вистави успішно створено!");

                        // Отримуємо створені квитки
                        availableTickets = _ticketService.GetAvailableTicketsForPerformance(performanceId).ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Помилка при створенні квитків: {ex.Message}");
                        Console.ReadKey();
                        return;
                    }
                }
                else
                {
                    Console.ReadKey();
                    return;
                }
            }

            // Групуємо квитки за рядами для зручнішого відображення
            Console.Clear();
            Console.WriteLine($"======= ПРОДАЖ КВИТКА НА ВИСТАВУ: {selectedPerformance.Title} ({selectedPerformance.PerformanceDate.ToShortDateString()}) =======");
            Console.WriteLine("Доступні квитки:");

            var ticketsByRow = availableTickets.GroupBy(t => t.Seat.Row)
                                              .OrderBy(g => g.Key);

            foreach (var row in ticketsByRow)
            {
                Console.WriteLine($"Ряд {row.Key}:");

                foreach (var ticket in row.OrderBy(t => t.Seat.Number))
                {
                    string vipStatus = ticket.Seat.IsVIP ? " (VIP)" : "";
                    Console.WriteLine($"  ID: {ticket.Id}, Місце: {ticket.Seat.Number}{vipStatus}, Ціна: {ticket.Price} грн");
                }

                Console.WriteLine();
            }

            Console.Write("Введіть ID квитка для продажу: ");
            if (!int.TryParse(Console.ReadLine(), out int ticketId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var selectedTicket = availableTickets.FirstOrDefault(t => t.Id == ticketId);
            if (selectedTicket == null)
            {
                Console.WriteLine("Квиток з таким ID не знайдено або він уже проданий.");
                Console.ReadKey();
                return;
            }

            bool result = _ticketService.SellTicket(ticketId);

            if (result)
            {
                Console.WriteLine($"Квиток успішно продано! Сума до сплати: {selectedTicket.Price} грн");
            }
            else
            {
                Console.WriteLine("Помилка при продажу квитка.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void ReturnTicket()
        {
            Console.Clear();
            Console.WriteLine("======= ПОВЕРНЕННЯ КВИТКА =======");

            Console.Write("Введіть ID квитка для повернення: ");
            if (!int.TryParse(Console.ReadLine(), out int ticketId))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var ticket = _ticketService.GetTicketWithDetails(ticketId);
            if (ticket == null)
            {
                Console.WriteLine("Квиток з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            if (!ticket.IsSold)
            {
                Console.WriteLine("Цей квиток ще не проданий, тому його не можна повернути.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Інформація про квиток:");
            Console.WriteLine($"Вистава: {ticket.Performance.Title}");
            Console.WriteLine($"Дата: {ticket.Performance.PerformanceDate.ToShortDateString()} {ticket.Performance.PerformanceDate.ToShortTimeString()}");
            Console.WriteLine($"Зал: {ticket.Performance.Hall.Name}");
            Console.WriteLine($"Ряд: {ticket.Seat.Row}, Місце: {ticket.Seat.Number}");
            Console.WriteLine($"Ціна: {ticket.Price} грн");
            Console.WriteLine($"Дата продажу: {ticket.PurchaseDate?.ToString() ?? "Невідомо"}");

            Console.Write("Ви дійсно хочете повернути цей квиток? (так/ні): ");
            if (Console.ReadLine()?.ToLower() != "так")
            {
                Console.WriteLine("Повернення скасовано.");
                Console.ReadKey();
                return;
            }

            bool result = _ticketService.ReturnTicket(ticketId);

            if (result)
            {
                Console.WriteLine($"Квиток успішно повернуто! Сума до повернення: {ticket.Price} грн");
            }
            else
            {
                Console.WriteLine("Помилка при поверненні квитка. Можливо, занадто пізно для повернення (менше ніж за день до вистави).");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }
    }
}