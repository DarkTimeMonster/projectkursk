using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace JockeyRaces
{
    class Program
    {
        static string racesFile = "races.json";
        static string horsesFile = "horses.json";
        static string jockeysFile = "jockeys.json";

        static void Main()
        {
            List<Race> races;
            List<Horse> horses;
            List<Jockey> jockeys;

            try
            {
                races = LoadFromFile<Race>(racesFile);
                horses = LoadFromFile<Horse>(horsesFile);
                jockeys = LoadFromFile<Jockey>(jockeysFile);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке данных: {ex.Message}");
                Console.WriteLine("Программа будет использовать пустые списки.");
                races = new List<Race>();
                horses = new List<Horse>();
                jockeys = new List<Jockey>();
            }

            while (true)
            {
                Console.WriteLine("\n--- Меню ---");
                Console.WriteLine("1. Вывести данные");
                Console.WriteLine("2. Добавление данных");
                Console.WriteLine("3. Удаление данных");
                Console.WriteLine("4. Запросы");
                Console.WriteLine("0. Сохранить и выйти");
                Console.Write("Выберите: ");

                try
                {
                    string choice = Console.ReadLine();
                    Console.Clear();

                    switch (choice)
                    {
                        case "1":
                            DisplayData(races, horses, jockeys);
                            break;
                        case "2":
                            bool adding = true;
                            while (adding)
                            {
                                Console.WriteLine("\n--- Добавление данных ---");
                                Console.WriteLine("1. Добавить лошадь");
                                Console.WriteLine("2. Добавить скачку");
                                Console.WriteLine("3. Добавить жокея");
                                Console.WriteLine("4. Добавить результаты скачки");
                                Console.WriteLine("0. Назад");
                                Console.Write("Выберите: ");
                                string choiceAdd = Console.ReadLine();
                                Console.Clear();

                                switch (choiceAdd)
                                {
                                    case "1":
                                        AddHorse(horses);
                                        break;
                                    case "2":
                                        AddRace(races);
                                        break;
                                    case "3":
                                        AddJockey(jockeys);
                                        break;
                                    case "4":
                                        AddRaceResult(races, horses, jockeys);
                                        break;
                                    case "0":
                                        adding = false;
                                        break;
                                    default:
                                        Console.WriteLine("Неверный выбор.");
                                        break;
                                }
                            }
                            break;
                        case "3":
                            bool deleting = true;
                            while (deleting)
                            {
                                Console.WriteLine("\n--- Удаление данных ---");
                                Console.WriteLine("1. Удалить лошадей по владельцу");
                                Console.WriteLine("2. Удалить жокея по ID");
                                Console.WriteLine("0. Назад");
                                Console.Write("Выберите: ");
                                string choiceDel = Console.ReadLine();
                                Console.Clear();

                                switch (choiceDel)
                                {
                                    case "1":
                                        Console.Write("Введите имя владельца: ");
                                        RemoveHorse(horses, Console.ReadLine());
                                        break;
                                    case "2":
                                        Console.Write("Введите ID жокея: ");
                                        try
                                        {
                                            int jockeyId = int.Parse(Console.ReadLine());
                                            RemoveJockey(jockeys, jockeyId);
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Неверный формат ID.");
                                        }
                                        break;
                                    case "0":
                                        deleting = false;
                                        break;
                                    default:
                                        Console.WriteLine("Неверный выбор.");
                                        break;
                                }
                            }
                            break;
                        case "4":
                            bool selecting = true;
                            while (selecting)
                            { 
                            Console.WriteLine("\n--- Запросы ---");
                            Console.WriteLine("1. Призёры на дату");
                            Console.WriteLine("2. Скачки по лошади");
                            Console.WriteLine("3. Скачки по жокею");
                            Console.WriteLine("4. Лошадь с наибольшим числом призов");
                            Console.WriteLine("5. Жокей с наибольшим числом призов");
                            Console.WriteLine("6. Самый популярный ипподром");
                            Console.WriteLine("0. Назад");
                            Console.Write("Выберите: ");
                            string selectingAdd = Console.ReadLine();
                            Console.Clear();

                            
                                switch (selectingAdd)
                                {
                                    case "1":
                                        Console.Write("Введите дату (гггг-мм-дд): ");
                                        try
                                        {
                                            DateTime date = DateTime.Parse(Console.ReadLine());
                                            PrizeWinnersByDate(races, horses, jockeys, date);
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Неверный формат даты.");
                                        }

                                        break;
                                    case "2":
                                        Console.Write("Введите ID лошади: ");
                                        try
                                        {
                                            int horseId = int.Parse(Console.ReadLine());
                                            HorseRaces(races, horseId);
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Неверный формат ID.");
                                        }

                                        break;
                                    case "3":
                                        Console.Write("Введите ID жокея: ");
                                        try
                                        {
                                            int jockeyId = int.Parse(Console.ReadLine());
                                            JockeyRaces(races, jockeyId);
                                        }
                                        catch (FormatException)
                                        {
                                            Console.WriteLine("Неверный формат ID.");
                                        }

                                        break;
                                    case "4":
                                        TopHorse(races, horses);
                                        break;
                                    case "5":
                                        TopJockey(races, jockeys);
                                        break;
                                    case "6":
                                        TopHippodrome(races);
                                        break;
                                    case "0":
                                        selecting = false;
                                        break;
                                    default:
                                        Console.WriteLine("Неверный выбор.");
                                        break;
                                }
                            }

                            break;
                            

                        case "0":
                            try
                            {
                                SaveToFile(racesFile, races);
                                SaveToFile(horsesFile, horses);
                                SaveToFile(jockeysFile, jockeys);
                                Console.WriteLine("Данные сохранены.");
                                return;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка при сохранении данных: {ex.Message}");
                                Console.WriteLine("Нажмите любую клавишу для выхода...");
                                Console.ReadKey();
                                return;
                            }
                        default:
                            Console.WriteLine("Неверный выбор.");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }

        // ----- Классы -----
        public class Race
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Hippodrome { get; set; }
            public DateTime Date { get; set; }
            public DateTime Time { get; set; }
            public decimal Prize { get; set; }
            public List<RaceResult> Results { get; set; } = new List<RaceResult>();
        }

        public class Horse
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Color { get; set; }
            public int Age { get; set; }
            public string Owner { get; set; }
        }

        public class Jockey
        {
            public int Id { get; set; }
            public string FullName { get; set; }
            public int Age { get; set; }
        }

        public class RaceResult
        {
            public int HorseId { get; set; }
            public int JockeyId { get; set; }
            public int Place { get; set; }
        }

        // ----- Сохранение / Загрузка -----
        static void SaveToFile<T>(string fileName, List<T> data)
        {
            try
            {
                JsonSerializerOptions options = new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                };
                File.WriteAllText(fileName, JsonSerializer.Serialize(data, options));
            }
            catch (IOException ex)
            {
                throw new Exception($"Ошибка записи в файл {fileName}: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Ошибка сериализации данных: {ex.Message}");
            }
        }

        static List<T> LoadFromFile<T>(string fileName)
        {
            try
            {
                if (!File.Exists(fileName))
                    return new List<T>();

                string json = File.ReadAllText(fileName);

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true, // Игнорировать регистр при чтении
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                };

                List<T> result = JsonSerializer.Deserialize<List<T>>(json, options);
                return result ?? new List<T>();
            }
            catch (IOException ex)
            {
                throw new Exception($"Ошибка чтения файла {fileName}: {ex.Message}");
            }
            catch (JsonException ex)
            {
                throw new Exception($"Ошибка десериализации данных: {ex.Message}");
            }
        }


        // ----- Функции -----
        static void AddHorse(List<Horse> horses)
        {
            try
            {
                Horse horse = new Horse();
                Console.Write("Кличка: ");
                horse.Name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(horse.Name))
                    throw new Exception("Кличка не может быть пустой.");

                Console.Write("Масть: ");
                horse.Color = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(horse.Color))
                    throw new Exception("Масть не может быть пустой.");

                Console.Write("Возраст: ");
                horse.Age = int.Parse(Console.ReadLine());
                if (horse.Age <= 0)
                    throw new Exception("Возраст должен быть положительным.");

                Console.Write("Владелец: ");
                horse.Owner = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(horse.Owner))
                    throw new Exception("Владелец не может быть пустым.");

                horse.Id = horses.Any() ? horses.Max(h => h.Id) + 1 : 1;
                horses.Add(horse);
                Console.WriteLine("Лошадь добавлена.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат возраста.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void RemoveHorse(List<Horse> horses, string owner)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(owner))
                    throw new Exception("Имя владельца не может быть пустым.");

                int count = horses.RemoveAll(h => h.Owner == owner);
                Console.WriteLine($"Удалено лошадей: {count}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void PrizeWinnersByDate(List<Race> races, List<Horse> horses, List<Jockey> jockeys, DateTime date)
        {
            try
            {
                IEnumerable<Race> found = races.Where(r => r.Date.Date == date.Date);
                if (!found.Any())
                {
                    Console.WriteLine("Скачек на указанную дату не найдено.");
                    return;
                }

                foreach (Race race in found)
                {
                    Console.WriteLine($"\nСкачка: {race.Name} ({race.Date:yyyy-MM-dd} {race.Time:HH:mm})");
                    IEnumerable<RaceResult> results = race.Results.Where(r => r.Place <= 3).OrderBy(r => r.Place);
                    if (!results.Any())
                    {
                        Console.WriteLine("  Призёров нет.");
                        continue;
                    }

                    foreach (RaceResult res in results)
                    {
                        Horse horse = horses.FirstOrDefault(h => h.Id == res.HorseId);
                        Jockey jockey = jockeys.FirstOrDefault(j => j.Id == res.JockeyId);
                        Console.WriteLine($"{res.Place} место: {(horse != null ? horse.Name : "Неизвестная лошадь")} / {(jockey != null ? jockey.FullName : "Неизвестный жокей")}");
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при обработке призёров: {ex.Message}");
            }
        }

        static void HorseRaces(List<Race> races, int horseId)
        {
            try
            {
                IEnumerable<Race> list = races.Where(r => r.Results.Any(res => res.HorseId == horseId));
                if (!list.Any())
                {
                    Console.WriteLine("Скачек для указанной лошади не найдено.");
                    return;
                }

                foreach (Race race in list)
                    Console.WriteLine($"{race.Name} ({race.Date:yyyy-MM-dd})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске скачек: {ex.Message}");
            }
        }

        static void JockeyRaces(List<Race> races, int jockeyId)
        {
            try
            {
                IEnumerable<Race> list = races.Where(r => r.Results.Any(res => res.JockeyId == jockeyId));
                if (!list.Any())
                {
                    Console.WriteLine("Скачек для указанного жокея не найдено.");
                    return;
                }

                foreach (Race race in list)
                    Console.WriteLine($"{race.Name} ({race.Date:yyyy-MM-dd})");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске скачек: {ex.Message}");
            }
        }

        static void TopHorse(List<Race> races, List<Horse> horses)
        {
            try
            {
                var top = races.SelectMany(r => r.Results)
                    .Where(r => r.Place <= 3)
                    .GroupBy(r => r.HorseId)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();

                if (top == null)
                {
                    Console.WriteLine("Призёров среди лошадей не найдено.");
                    return;
                }

                Horse horse = horses.FirstOrDefault(h => h.Id == top.Key);
                Console.WriteLine($"Лошадь с наибольшим числом призов: {(horse != null ? horse.Name : "Неизвестная лошадь")} ({top.Count()} раз)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске лучшей лошади: {ex.Message}");
            }
        }

        static void TopJockey(List<Race> races, List<Jockey> jockeys)
        {
            try
            {
                var top = races.SelectMany(r => r.Results)
                    .Where(r => r.Place <= 3)
                    .GroupBy(r => r.JockeyId)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();

                if (top == null)
                {
                    Console.WriteLine("Призёров среди жокеев не найдено.");
                    return;
                }

                Jockey jockey = jockeys.FirstOrDefault(j => j.Id == top.Key);
                Console.WriteLine($"Жокей с наибольшим числом призов: {(jockey != null ? jockey.FullName : "Неизвестный жокей")} ({top.Count()} раз)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске лучшего жокея: {ex.Message}");
            }
        }

        static void TopHippodrome(List<Race> races)
        {
            try
            {
                var top = races.GroupBy(r => r.Hippodrome)
                    .OrderByDescending(g => g.Count())
                    .FirstOrDefault();
                if (top == null)
                {
                    Console.WriteLine("Скачек не найдено.");
                    return;
                }
                Console.WriteLine($"Чаще всего проводились на ипподроме: {top.Key} ({top.Count()} раз)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при поиске ипподрома: {ex.Message}");
            }
        }

        static void DisplayData(List<Race> races, List<Horse> horses, List<Jockey> jockeys)
        {
            try
            {
                Console.WriteLine("----- Все скачки -----");
                if (!races.Any())
                    Console.WriteLine("Скачек нет.");
                foreach (Race race in races.OrderBy(r => r.Date))
                {
                    Console.WriteLine($"ID: {race.Id}, Название: {race.Name}, Ипподром: {race.Hippodrome}, Дата: {race.Date:yyyy-MM-dd}, Время: {race.Time:HH:mm}, Приз: {race.Prize} руб.");
                    if (race.Results.Any())
                    {
                        Console.WriteLine("  Результаты:");
                        foreach (RaceResult result in race.Results)
                        {
                            Horse horse = horses.FirstOrDefault(h => h.Id == result.HorseId);
                            Jockey jockey = jockeys.FirstOrDefault(j => j.Id == result.JockeyId);
                            Console.WriteLine($"    Место: {result.Place}, Лошадь: {(horse != null ? horse.Name : "Неизвестная лошадь")}, Жокей: {(jockey != null ? jockey.FullName : "Неизвестный жокей")}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("  Результатов пока нет.");
                    }
                }

                Console.WriteLine("\n----- Все лошади -----");
                if (!horses.Any())
                    Console.WriteLine("Лошадей нет.");
                foreach (Horse horse in horses)
                {
                    Console.WriteLine($"ID: {horse.Id}, Кличка: {horse.Name}, Масть: {horse.Color}, Возраст: {horse.Age}, Владелец: {horse.Owner}");
                }

                Console.WriteLine("\n----- Все жокеи -----");
                if (!jockeys.Any())
                    Console.WriteLine("Жокеев нет.");
                foreach (Jockey jockey in jockeys)
                {
                    Console.WriteLine($"ID: {jockey.Id}, ФИО: {jockey.FullName}, Возраст: {jockey.Age}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при отображении данных: {ex.Message}");
            }
        }

        static void AddJockey(List<Jockey> jockeys)
        {
            try
            {
                Jockey jockey = new Jockey();
                Console.Write("ФИО: ");
                jockey.FullName = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(jockey.FullName))
                    throw new Exception("ФИО не может быть пустым.");

                Console.Write("Возраст: ");
                jockey.Age = int.Parse(Console.ReadLine());
                if (jockey.Age <= 0)
                    throw new Exception("Возраст должен быть положительным.");

                jockey.Id = jockeys.Any() ? jockeys.Max(j => j.Id) + 1 : 1;
                jockeys.Add(jockey);
                Console.WriteLine("Жокей добавлен.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат возраста.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void RemoveJockey(List<Jockey> jockeys, int id)
        {
            try
            {
                int removed = jockeys.RemoveAll(j => j.Id == id);
                Console.WriteLine(removed > 0 ? "Жокей удалён." : "Жокей не найден.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при удалении жокея: {ex.Message}");
            }
        }

        static void AddRace(List<Race> races)
        {
            try
            {
                Race race = new Race();
                race.Id = races.Any() ? races.Max(r => r.Id) + 1 : 1;

                Console.Write("Название: ");
                race.Name = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(race.Name))
                    throw new Exception("Название не может быть пустым.");

                Console.Write("Ипподром: ");
                race.Hippodrome = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(race.Hippodrome))
                    throw new Exception("Ипподром не может быть пустым.");

                Console.Write("Дата (гггг-мм-дд): ");
                race.Date = DateTime.Parse(Console.ReadLine());

                Console.Write("Время (чч:мм): ");
                TimeSpan time = TimeSpan.Parse(Console.ReadLine());
                race.Time = race.Date.Date + time;

                Console.Write("Приз (в рублях): ");
                race.Prize = decimal.Parse(Console.ReadLine());
                if (race.Prize < 0)
                    throw new Exception("Приз не может быть отрицательным.");

                races.Add(race);
                Console.WriteLine("Скачка добавлена.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат данных (дата, время или приз).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void AddRaceResult(List<Race> races, List<Horse> horses, List<Jockey> jockeys)
        {
            try
            {
                Console.WriteLine(" -- Данные для ввода -- ");
                DisplayData(races, horses, jockeys);
                Console.WriteLine();
                Console.Write("Введите ID скачки: ");
                int raceId = int.Parse(Console.ReadLine());
                Race race = races.FirstOrDefault(r => r.Id == raceId);
                if (race == null)
                    throw new Exception("Скачка не найдена.");

                Console.Write("Сколько результатов добавить: ");
                int count = int.Parse(Console.ReadLine());
                if (count <= 0)
                    throw new Exception("Количество результатов должно быть положительным.");

                for (int i = 0; i < count; i++)
                {
                    Console.WriteLine($"Результат {i + 1}:");
                    Console.Write("  ID лошади: ");
                    int horseId = int.Parse(Console.ReadLine());
                    if (horses.FirstOrDefault(h => h.Id == horseId) == null)
                    {
                        Console.WriteLine("Лошадь с таким ID не найдена. Пропуск результата.");
                        continue;
                    }

                    Console.Write("  ID жокея: ");
                    int jockeyId = int.Parse(Console.ReadLine());
                    if (jockeys.FirstOrDefault(j => j.Id == jockeyId) == null)
                    {
                        Console.WriteLine("Жокей с таким ID не найден. Пропуск результата.");
                        continue;
                    }

                    Console.Write("  Место: ");
                    int place = int.Parse(Console.ReadLine());
                    if (place <= 0)
                    {
                        Console.WriteLine("Место должно быть положительным. Пропуск результата.");
                        continue;
                    }

                    race.Results.Add(new RaceResult
                    {
                        HorseId = horseId,
                        JockeyId = jockeyId,
                        Place = place
                    });
                }

                Console.WriteLine("Результаты добавлены.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Неверный формат ввода (ID, количество или место).");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }
    }
}