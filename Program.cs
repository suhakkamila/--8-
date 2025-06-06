using StudentLibrary;
using System;
using System.Linq;

class Program
{
    static Library library = new Library();

    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        while (true)
        {
            Console.WriteLine("\n=== Студентська бібліотека ===");
            Console.WriteLine("1.1 Додати користувача");
            Console.WriteLine("1.2 Видалити користувача");
            Console.WriteLine("1.3 Змінити дані користувача");
            Console.WriteLine("1.4 Переглянути користувача");
            Console.WriteLine("1.5 Переглянути список усіх користувачів");
            Console.WriteLine("1.5.1 Сортувати по імені");
            Console.WriteLine("1.5.2 Сортувати по прізвищу");
            Console.WriteLine("1.5.3 Сортувати по групі");
            Console.WriteLine("2.1 Додати документ");
            Console.WriteLine("2.2 Видалити документ");
            Console.WriteLine("2.3 Змінити документ");
            Console.WriteLine("2.4 Переглянути документ");
            Console.WriteLine("2.5 Переглянути список усіх документів");
            Console.WriteLine("2.5.1 Сортувати по назві");
            Console.WriteLine("2.5.2 Сортувати по автору");
            Console.WriteLine("3.1 Видати документ користувачу");
            Console.WriteLine("3.2 Переглянути документи користувача");
            Console.WriteLine("3.3 Хто має документ?");
            Console.WriteLine("3.4 Повернути документ");
            Console.WriteLine("4.1 Пошук серед документів");
            Console.WriteLine("4.2 Пошук серед користувачів");
            Console.WriteLine("0. Вийти\n");

            Console.Write("Виберіть пункт: ");
            string input = Console.ReadLine();
            Console.WriteLine();

            switch (input)
            {
                case "1.1": AddUser(); break;
                case "1.2": RemoveUser(); break;
                case "1.3": UpdateUser(); break;
                case "1.4": ViewUser(); break;
                case "1.5": ListUsers(); break;
                case "1.5.1": SortUsers("first"); break;
                case "1.5.2": SortUsers("last"); break;
                case "1.5.3": SortUsers("group"); break;
                case "2.1": AddDocument(); break;
                case "2.2": RemoveDocument(); break;
                case "2.3": UpdateDocument(); break;
                case "2.4": ViewDocument(); break;
                case "2.5": ListDocuments(); break;
                case "2.5.1": SortDocuments("title"); break;
                case "2.5.2": SortDocuments("author"); break;
                case "3.1": IssueDocument(); break;
                case "3.2": ListUserDocuments(); break;
                case "3.3": WhoHasDocument(); break;
                case "3.4": ReturnDocument(); break;
                case "4.1": SearchDocuments(); break;
                case "4.2": SearchUsers(); break;
                case "0": return;
                default: Console.WriteLine("Невірна команда"); break;
            }
        }
    }

    // --- Користувачі ---
    static void AddUser()
    {
        Console.Write("Введіть ім'я: ");
        string firstName = Console.ReadLine();

        Console.Write("Введіть прізвище: ");
        string lastName = Console.ReadLine();

        Console.Write("Введіть групу: ");
        string group = Console.ReadLine();

        var user = new User(firstName, lastName, group);
        library.UserManager.AddUser(user);

        Console.WriteLine("Користувача додано.");
    }

    static void RemoveUser()
    {
        Console.Write("Введіть прізвище користувача для видалення: ");
        string lastName = Console.ReadLine();

        var user = library.UserManager.GetUser(lastName);
        if (user != null)
        {
            library.UserManager.RemoveUser(user);
            Console.WriteLine("Користувача видалено.");
        }
        else Console.WriteLine("Користувача не знайдено.");
    }

    static void UpdateUser()
    {
        Console.Write("Введіть прізвище користувача для зміни: ");
        string lastName = Console.ReadLine();

        var user = library.UserManager.GetUser(lastName);
        if (user != null)
        {
            Console.Write("Введіть нове ім'я: ");
            string firstName = Console.ReadLine();

            Console.Write("Введіть нове прізвище: ");
            string newLastName = Console.ReadLine();

            Console.Write("Введіть нову групу: ");
            string group = Console.ReadLine();

            library.UserManager.UpdateUser(user, firstName, newLastName, group);
            Console.WriteLine("Дані користувача оновлено.");
        }
        else Console.WriteLine("Користувача не знайдено.");
    }

    static void ViewUser()
    {
        Console.Write("Введіть прізвище користувача: ");
        string lastName = Console.ReadLine();

        var user = library.UserManager.GetUser(lastName);
        if (user != null)
        {
            Console.WriteLine(user.GetInfo());
        }
        else Console.WriteLine("Користувача не знайдено.");
    }

    static void ListUsers()
    {
        var users = library.UserManager.GetAllUsers();
        if (!users.Any())
            Console.WriteLine("Список користувачів порожній.");
        else
            foreach (var u in users)
                Console.WriteLine(u.GetInfo());
    }

    static void SortUsers(string by)
    {
        var sorted = by switch
        {
            "first" => library.UserManager.SortByFirstName(),
            "last" => library.UserManager.SortByLastName(),
            "group" => library.UserManager.SortByGroup(),
            _ => library.UserManager.GetAllUsers()
        };

        foreach (var u in sorted)
            Console.WriteLine(u.GetInfo());
    }

    // --- Документи ---
    static void AddDocument()
    {
        Console.WriteLine("Оберіть тип документа:");
        Console.WriteLine("1. Книга");
        Console.WriteLine("2. Журнал");
        Console.WriteLine("3. Дипломна робота");
        Console.Write("Ваш вибір: ");
        string choice = Console.ReadLine();

        Console.Write("Введіть назву: ");
        string title = Console.ReadLine();

        Console.Write("Введіть автора: ");
        string author = Console.ReadLine();

        Document doc = choice switch
        {
            "1" => new Book(title, author),
            "2" => new Magazine(title, author),
            "3" => new Thesis(title, author),
            _ => null
        };

        if (doc == null)
        {
            Console.WriteLine("Невірний тип документа.");
            return;
        }

        library.DocumentManager.AddDocument(doc);
        Console.WriteLine("Документ додано.");
    }

    static void RemoveDocument()
    {
        Console.Write("Введіть назву документа для видалення: ");
        string title = Console.ReadLine();

        var doc = library.DocumentManager.GetDocument(title);
        if (doc != null)
        {
            library.DocumentManager.RemoveDocument(doc);
            Console.WriteLine("Документ видалено.");
        }
        else Console.WriteLine("Документ не знайдено.");
    }

    static void UpdateDocument()
    {
        Console.Write("Введіть назву документа для зміни: ");
        string title = Console.ReadLine();

        var doc = library.DocumentManager.GetDocument(title);
        if (doc != null)
        {
            Console.Write("Введіть нову назву: ");
            string newTitle = Console.ReadLine();

            Console.Write("Введіть нового автора: ");
            string author = Console.ReadLine();

            library.DocumentManager.UpdateDocument(doc, newTitle, author);
            Console.WriteLine("Дані документа оновлено.");
        }
        else Console.WriteLine("Документ не знайдено.");
    }

    static void ViewDocument()
    {
        Console.Write("Введіть назву документа: ");
        string title = Console.ReadLine();

        var doc = library.DocumentManager.GetDocument(title);
        if (doc != null)
            Console.WriteLine(doc.GetInfo());
        else Console.WriteLine("Документ не знайдено.");
    }

    static void ListDocuments()
    {
        var docs = library.DocumentManager.GetAllDocuments();
        if (!docs.Any())
            Console.WriteLine("Список документів порожній.");
        else
            foreach (var d in docs)
                Console.WriteLine(d.GetInfo());
    }

    static void SortDocuments(string by)
    {
        var sorted = by switch
        {
            "title" => library.DocumentManager.SortByTitle(),
            "author" => library.DocumentManager.SortByAuthor(),
            _ => library.DocumentManager.GetAllDocuments()
        };

        foreach (var d in sorted)
            Console.WriteLine(d.GetInfo());
    }

    // --- Видача документів ---
 // 1. Видача документа
static void IssueDocument()
{
    Console.Write("Введіть прізвище користувача: ");
    string lastName = Console.ReadLine();
    var user = library.UserManager.GetUser(lastName);
    if (user == null)
    {
        Console.WriteLine("Користувача не знайдено.");
        return;
    }

    Console.Write("Введіть назву документа: ");
    string title = Console.ReadLine();
    var doc = library.DocumentManager.GetDocument(title);
    if (doc == null)
    {
        Console.WriteLine("Документ не знайдено.");
        return;
    }

    var holder = library.BorrowManager.GetOwnerOfDocument(doc);
    if (holder != null)
    {
        Console.WriteLine("Документ вже виданий іншому користувачу.");
        return;
    }

    try
    {
        library.BorrowManager.BorrowDocument(user, doc);
        Console.WriteLine("Документ видано користувачу.");
    }
    catch (InvalidOperationException ex)
    {
        Console.WriteLine(ex.Message);
    }
}

// 2. Список документів користувача
static void ListUserDocuments()
{
    Console.Write("Введіть прізвище користувача: ");
    string lastName = Console.ReadLine();
    var user = library.UserManager.GetUser(lastName);

    if (user == null)
    {
        Console.WriteLine("Користувача не знайдено.");
        return;
    }

    var userDocs = library.BorrowManager.GetDocumentsOfUser(user);
    if (!userDocs.Any())
        Console.WriteLine("Користувач не має виданих документів.");
    else
        foreach (var d in userDocs)
            Console.WriteLine(d.GetInfo());
}

// 3. Хто має документ
static void WhoHasDocument()
{
    Console.Write("Введіть назву документа: ");
    string title = Console.ReadLine();

    var doc = library.DocumentManager.GetDocument(title);
    if (doc == null)
    {
        Console.WriteLine("Документ не знайдено.");
        return;
    }

    var user = library.BorrowManager.GetOwnerOfDocument(doc);
    if (user == null)
        Console.WriteLine("Документ не виданий.");
    else
        Console.WriteLine($"Документ виданий користувачу: {user.GetInfo()}");
}

// 4. Повернення документа
static void ReturnDocument()
{
    Console.Write("Введіть назву документа для повернення: ");
    string title = Console.ReadLine();

    var doc = library.DocumentManager.GetDocument(title);
    if (doc == null)
    {
        Console.WriteLine("Документ не знайдено.");
        return;
    }

    var user = library.BorrowManager.GetOwnerOfDocument(doc);
    if (user == null)
    {
        Console.WriteLine("Документ не був виданий.");
        return;
    }

    library.BorrowManager.ReturnDocument(user, doc);
    Console.WriteLine("Документ повернено.");
}

    // --- Пошук ---
    static void SearchDocuments()
    {
        Console.Write("Введіть текст для пошуку в назві або авторі: ");
        string text = Console.ReadLine();

        var results = library.DocumentManager.SearchDocuments(text);
        if (!results.Any())
            Console.WriteLine("Документи не знайдено.");
        else
            foreach (var d in results)
                Console.WriteLine(d.GetInfo());
    }

    static void SearchUsers()
    {
        Console.Write("Введіть текст для пошуку в імені, прізвищі або групі: ");
        string text = Console.ReadLine();

        var results = library.UserManager.SearchUsers(text);
        if (!results.Any())
            Console.WriteLine("Користувачів не знайдено.");
        else
            foreach (var u in results)
                Console.WriteLine(u.GetInfo());
    }
}





