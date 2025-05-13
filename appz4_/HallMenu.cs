using Microsoft.Extensions.DependencyInjection;
using TheatreTicketSystem.BLL.Services;
using TheatreTicketSystem.DAL.Entities;

namespace TheatreTicketSystem.ConsoleUI
{
    public class HallMenu
    {
        private readonly HallService _hallService;
        private readonly SeatService _seatService;

        public HallMenu(IServiceProvider serviceProvider)
        {
            _hallService = serviceProvider.GetRequiredService<HallService>();
            _seatService = serviceProvider.GetRequiredService<SeatService>();
        }

        public void Show()
        {
            bool back = false;
            while (!back)
            {
                Console.Clear();
                Console.WriteLine("======= УПРАВЛІННЯ ЗАЛАМИ =======");
                Console.WriteLine("1. Переглянути всі зали");
                Console.WriteLine("2. Додати новий зал");
                Console.WriteLine("3. Редагувати зал");
                Console.WriteLine("4. Видалити зал");
                Console.WriteLine("5. Управління місцями в залі");
                Console.WriteLine("0. Назад");
                Console.Write("Оберіть опцію: ");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ShowAllHalls();
                        break;
                    case "2":
                        AddHall();
                        break;
                    case "3":
                        UpdateHall();
                        break;
                    case "4":
                        DeleteHall();
                        break;
                    case "5":
                        ManageSeats();
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

        private void ShowAllHalls()
        {
            Console.Clear();
            Console.WriteLine("======= СПИСОК ЗАЛІВ =======");

            var halls = _hallService.GetAllHalls();
            if (!halls.Any())
            {
                Console.WriteLine("Список залів порожній.");
            }
            else
            {
                foreach (var hall in halls)
                {
                    Console.WriteLine($"ID: {hall.Id}, Назва: {hall.Name}, Місткість: {hall.Capacity}");
                    if (!string.IsNullOrEmpty(hall.Description))
                    {
                        Console.WriteLine($"Опис: {hall.Description}");
                    }
                    Console.WriteLine(new string('-', 30));
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void AddHall()
        {
            Console.Clear();
            Console.WriteLine("======= ДОДАВАННЯ НОВОГО ЗАЛУ =======");

            Console.Write("Введіть назву залу: ");
            var name = Console.ReadLine();

            Console.Write("Введіть місткість залу: ");
            if (!int.TryParse(Console.ReadLine(), out int capacity) || capacity <= 0)
            {
                Console.WriteLine("Неправильний формат місткості. Місткість повинна бути додатнім числом.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть опис залу (необов'язково): ");
            var description = Console.ReadLine();

            var hall = new Hall
            {
                Name = name,
                Capacity = capacity,
                Description = description
            };

            _hallService.AddHall(hall);

            Console.WriteLine("Зал успішно додано!");

            Console.Write("Бажаєте додати місця в зал зараз? (так/ні): ");
            var addSeats = Console.ReadLine()?.ToLower() == "так";

            if (addSeats)
            {
                ConfigureSeats(hall.Id);
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void UpdateHall()
        {
            Console.Clear();
            Console.WriteLine("======= РЕДАГУВАННЯ ЗАЛУ =======");

            Console.Write("Введіть ID залу для редагування: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var hall = _hallService.GetHallById(id);
            if (hall == null)
            {
                Console.WriteLine("Зал з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Редагування залу: {hall.Name}");

            Console.Write($"Введіть нову назву залу (поточна: {hall.Name}): ");
            var name = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(name))
            {
                hall.Name = name;
            }

            Console.Write($"Введіть нову місткість залу (поточна: {hall.Capacity}): ");
            var capacityStr = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(capacityStr) && int.TryParse(capacityStr, out int capacity) && capacity > 0)
            {
                hall.Capacity = capacity;
            }

            Console.Write($"Введіть новий опис залу (поточний: {hall.Description}): ");
            var description = Console.ReadLine();
            if (description != null) // дозволяємо встановити пустий опис
            {
                hall.Description = description;
            }

            _hallService.UpdateHall(hall);

            Console.WriteLine("Зал успішно оновлено!");
            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void DeleteHall()
        {
            Console.Clear();
            Console.WriteLine("======= ВИДАЛЕННЯ ЗАЛУ =======");

            Console.Write("Введіть ID залу для видалення: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var hall = _hallService.GetHallById(id);
            if (hall == null)
            {
                Console.WriteLine("Зал з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Ви дійсно хочете видалити зал {hall.Name}? (так/ні): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "так")
            {
                _hallService.DeleteHall(id);
                Console.WriteLine("Зал успішно видалено!");
            }
            else
            {
                Console.WriteLine("Видалення скасовано.");
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void ManageSeats()
        {
            Console.Clear();
            Console.WriteLine("======= УПРАВЛІННЯ МІСЦЯМИ В ЗАЛІ =======");

            Console.Write("Введіть ID залу: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("Неправильний формат ID.");
                Console.ReadKey();
                return;
            }

            var hall = _hallService.GetHallWithSeats(id);
            if (hall == null)
            {
                Console.WriteLine("Зал з таким ID не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.Clear();
            Console.WriteLine($"======= МІСЦЯ В ЗАЛІ: {hall.Name} =======");

            if (!hall.Seats.Any())
            {
                Console.WriteLine("У цьому залі ще немає місць.");
                Console.Write("Бажаєте додати місця зараз? (так/ні): ");
                var addSeats = Console.ReadLine()?.ToLower() == "так";

                if (addSeats)
                {
                    ConfigureSeats(hall.Id);
                }
            }
            else
            {
                // Відображаємо інформацію про місця, згруповані по рядах
                var seatsByRow = hall.Seats.GroupBy(s => s.Row)
                                          .OrderBy(g => g.Key);

                foreach (var row in seatsByRow)
                {
                    Console.WriteLine($"Ряд {row.Key}:");
                    foreach (var seat in row.OrderBy(s => s.Number))
                    {
                        string vipStatus = seat.IsVIP ? " (VIP)" : "";
                        Console.WriteLine($"  Місце {seat.Number}{vipStatus}");
                    }
                }

                Console.WriteLine("\nОберіть дію:");
                Console.WriteLine("1. Додати нові місця");
                Console.WriteLine("2. Позначити місце як VIP");
                Console.WriteLine("3. Зняти VIP-статус з місця");
                Console.WriteLine("4. Видалити місце");
                Console.WriteLine("0. Назад");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        ConfigureSeats(hall.Id);
                        break;
                    case "2":
                    case "3":
                        ChangeVipStatus(hall.Id, choice == "2");
                        break;
                    case "4":
                        DeleteSeat(hall.Id);
                        break;
                }
            }

            Console.WriteLine("\nНатисніть будь-яку клавішу, щоб продовжити...");
            Console.ReadKey();
        }

        private void ConfigureSeats(int hallId)
        {
            Console.Clear();
            Console.WriteLine("======= КОНФІГУРАЦІЯ МІСЦЬ В ЗАЛІ =======");

            Console.Write("Введіть кількість рядів: ");
            if (!int.TryParse(Console.ReadLine(), out int rows) || rows <= 0)
            {
                Console.WriteLine("Неправильний формат кількості рядів.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть кількість місць в кожному ряду: ");
            if (!int.TryParse(Console.ReadLine(), out int seatsPerRow) || seatsPerRow <= 0)
            {
                Console.WriteLine("Неправильний формат кількості місць.");
                Console.ReadKey();
                return;
            }

            Console.Write("Чи є VIP-місця? (так/ні): ");
            var hasVipSeats = Console.ReadLine()?.ToLower() == "так";

            List<(int row, int number)> vipSeats = new List<(int row, int number)>();

            if (hasVipSeats)
            {
                Console.WriteLine("Введіть координати VIP-місць у форматі 'ряд,місце' (наприклад, '1,5').");
                Console.WriteLine("Для завершення введення VIP-місць введіть пустий рядок.");

                string input;
                while (!string.IsNullOrWhiteSpace(input = Console.ReadLine()))
                {
                    var coordinates = input.Split(',');
                    if (coordinates.Length == 2 &&
                        int.TryParse(coordinates[0], out int row) &&
                        int.TryParse(coordinates[1], out int number) &&
                        row > 0 && row <= rows &&
                        number > 0 && number <= seatsPerRow)
                    {
                        vipSeats.Add((row, number));
                        Console.WriteLine($"Додано VIP-місце: ряд {row}, місце {number}");
                    }
                    else
                    {
                        Console.WriteLine("Неправильний формат координат. Спробуйте ще раз.");
                    }
                }
            }

            _seatService.CreateSeatsForHall(hallId, rows, seatsPerRow, vipSeats);

            Console.WriteLine("Місця в залі успішно сконфігуровано!");
        }

        private void ChangeVipStatus(int hallId, bool makeVip)
        {
            Console.Clear();
            string action = makeVip ? "ВСТАНОВЛЕННЯ VIP-СТАТУСУ" : "ЗНЯТТЯ VIP-СТАТУСУ";
            Console.WriteLine($"======= {action} =======");

            Console.Write("Введіть ряд: ");
            if (!int.TryParse(Console.ReadLine(), out int row) || row <= 0)
            {
                Console.WriteLine("Неправильний формат ряду.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть номер місця: ");
            if (!int.TryParse(Console.ReadLine(), out int number) || number <= 0)
            {
                Console.WriteLine("Неправильний формат номера місця.");
                Console.ReadKey();
                return;
            }

            var seats = _seatService.GetSeatsForHall(hallId);
            var seat = seats.FirstOrDefault(s => s.Row == row && s.Number == number);

            if (seat == null)
            {
                Console.WriteLine("Місце з такими координатами не знайдено.");
                Console.ReadKey();
                return;
            }

            seat.IsVIP = makeVip;
            _seatService.UpdateSeat(seat);

            string statusText = makeVip ? "встановлено" : "знято";
            Console.WriteLine($"VIP-статус для місця {number} в ряду {row} успішно {statusText}!");
        }

        private void DeleteSeat(int hallId)
        {
            Console.Clear();
            Console.WriteLine("======= ВИДАЛЕННЯ МІСЦЯ =======");

            Console.Write("Введіть ряд: ");
            if (!int.TryParse(Console.ReadLine(), out int row) || row <= 0)
            {
                Console.WriteLine("Неправильний формат ряду.");
                Console.ReadKey();
                return;
            }

            Console.Write("Введіть номер місця: ");
            if (!int.TryParse(Console.ReadLine(), out int number) || number <= 0)
            {
                Console.WriteLine("Неправильний формат номера місця.");
                Console.ReadKey();
                return;
            }

            var seats = _seatService.GetSeatsForHall(hallId);
            var seat = seats.FirstOrDefault(s => s.Row == row && s.Number == number);

            if (seat == null)
            {
                Console.WriteLine("Місце з такими координатами не знайдено.");
                Console.ReadKey();
                return;
            }

            Console.WriteLine($"Ви дійсно хочете видалити місце {number} в ряду {row}? (так/ні): ");
            var confirmation = Console.ReadLine()?.ToLower();

            if (confirmation == "так")
            {
                _seatService.DeleteSeat(seat.Id);
                Console.WriteLine("Місце успішно видалено!");
            }
            else
            {
                Console.WriteLine("Видалення скасовано.");
            }
        }
    }
}