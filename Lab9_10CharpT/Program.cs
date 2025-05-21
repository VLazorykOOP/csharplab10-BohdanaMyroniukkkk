using System;
using System.Collections.Generic;

// Делегат для обробки подій
public delegate void FacultyEventHandler(string eventName, string details);

// Клас для представлення соціальної події
public class SocialEvent
{
    public string EventName { get; set; }
    public DateTime StartTime { get; set; }
    public string Location { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }

    public SocialEvent(string name, DateTime startTime, string location, string description)
    {
        EventName = name;
        StartTime = startTime;
        Location = location;
        Description = description;
        IsActive = false; // Подія не активна за замовчуванням
    }

    public override string ToString()
    {
        return $"Захiд: {EventName}\nЧас: {StartTime}\nМiсце: {Location}\nОпис: {Description}\nСтатус: {(IsActive ? "Триває" : "Заплановано")}\n";
    }
}

// Клас факультету
public class Faculty
{
    private List<SocialEvent> events;
    public event FacultyEventHandler? OnEventOccur; // Nullable подія

    public Faculty()
    {
        events = new List<SocialEvent>();
    }

    // Додавання нової події
    public void AddEvent(SocialEvent newEvent)
    {
        events.Add(newEvent);
        NotifyEvent(newEvent);
    }

    // Сповіщення про подію
    private void NotifyEvent(SocialEvent socialEvent)
    {
        OnEventOccur?.Invoke(socialEvent.EventName, socialEvent.ToString());
    }

    // Перевірка активних подій на основі поточного часу
    public void CheckActiveEvents(DateTime currentTime)
    {
        foreach (var evt in events)
        {
            // Вважаємо подію активною, якщо поточний час близький до часу початку (наприклад, протягом години)
            bool isActive = (currentTime >= evt.StartTime && currentTime <= evt.StartTime.AddHours(1));
            if (isActive != evt.IsActive)
            {
                evt.IsActive = isActive;
                OnEventOccur?.Invoke(evt.EventName, $"Подiя '{evt.EventName}' {(isActive ? "розпочалася!" : "завершилася!")}\n{evt}");
            }
        }
    }

    // Отримання всіх подій
    public List<SocialEvent> GetEvents()
    {
        return events;
    }
}

// Головний клас програми
class Program
{
    static void Main(string[] args)
    {
        Faculty faculty = new Faculty();

        // Підписка на подію
        faculty.OnEventOccur += (eventName, details) =>
        {
            Console.WriteLine($"Сповiщення: {details}");
            Console.WriteLine("------------------------");
        };

        // Додавання соціальних заходів
        faculty.AddEvent(new SocialEvent(
            "Студентська вечiрка",
            new DateTime(2025, 5, 21, 18, 0, 0),
            "Клуб 'Веселий декан'",
            "Вечiрка з музикою та танцями для всiх студентiв"
        ));

        faculty.AddEvent(new SocialEvent(
            "Кiнопоказ",
            new DateTime(2025, 5, 21, 20, 0, 0),
            "Аудиторiя 101",
            "Перегляд фiльму 'Код да Вiнчi' з попкорном"
        ));

        faculty.AddEvent(new SocialEvent(
            "Футбольний турнiр",
            new DateTime(2025, 5, 22, 14, 0, 0),
            "Стадiон кампусу",
            "Турнiр мiж факультетами з призами"
        ));

        // Симуляція поточного часу 
        DateTime currentTime = new DateTime(2025, 5, 21, 18, 30, 0);
        Console.WriteLine($"\nПоточний час: {currentTime}\n");
        faculty.CheckActiveEvents(currentTime);

        // Виведення всіх подій
        Console.WriteLine("\nУсi запланованi заходи на факультетi:");
        foreach (var evt in faculty.GetEvents())
        {
            Console.WriteLine(evt);
        }
    }
}