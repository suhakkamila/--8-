using System;
using System.Collections.Generic;
using System.Linq;

namespace StudentLibrary
{
    /// <summary>
    /// 2. Абстрактний клас документа
    /// </summary>
    public abstract class Document
    {
        /// <summary>Назва документа</summary>
        public string Title { get; set; }

        /// <summary>Автор документа</summary>
        public string Author { get; set; }

        protected Document(string title, string author)
        {
            Title = title;
            Author = author;
        }

        /// <summary>Повертає інформацію про документ</summary>
        public abstract string GetInfo();
    }

    /// <summary>
    /// 2.1 Клас книги
    /// </summary>
    public class Book : Document
    {
        public Book(string title, string author) : base(title, author) { }

        public override string GetInfo() => $"Книга: {Title}, автор: {Author}";
    }

    /// <summary>
    /// 2.2 Клас журналу
    /// </summary>
    public class Magazine : Document
    {
        public Magazine(string title, string author) : base(title, author) { }

        public override string GetInfo() => $"Журнал: {Title}, автор: {Author}";
    }

    /// <summary>
    /// 2.3 Клас дипломної роботи
    /// </summary>
    public class Thesis : Document
    {
        public Thesis(string title, string author) : base(title, author) { }

        public override string GetInfo() => $"Дипломна: {Title}, автор: {Author}";
    }

    /// <summary>
    /// 3.2 Формуляр користувача
    /// </summary>
    public class LibraryCard
    {
        /// <summary>Список документів, які взяв користувач</summary>
        public List<Document> BorrowedDocuments { get; private set; } = new List<Document>();

        /// <summary>3.1 Видача документа (макс. 5)</summary>
        public void Borrow(Document doc)
        {
            if (BorrowedDocuments.Count >= 5)
                throw new InvalidOperationException("Не можна брати більше 5 документів.");
            BorrowedDocuments.Add(doc);
        }

        /// <summary>3.4 Повернення документа</summary>
        public void Return(Document doc)
        {
            BorrowedDocuments.Remove(doc);
        }

        /// <summary>Перевірка, чи документ у формулярі</summary>
        public bool HasDocument(Document doc) => BorrowedDocuments.Contains(doc);
    }

    /// <summary>
    /// Клас користувача бібліотеки
    /// </summary>
    public class User
    {
        /// <summary>Ім’я користувача</summary>
        public string FirstName { get; set; }

        /// <summary>Прізвище користувача</summary>
        public string LastName { get; set; }

        /// <summary>Академічна група</summary>
        public string Group { get; set; }

        /// <summary>Формуляр користувача</summary>
        public LibraryCard Card { get; private set; } = new LibraryCard();

        public User(string firstName, string lastName, string group)
        {
            FirstName = firstName;
            LastName = lastName;
            Group = group;
        }

        /// <summary>Повертає повну інформацію про користувача</summary>
        public string GetInfo() => $"{FirstName} {LastName}, група: {Group}";
    }

    /// <summary>
    /// 1. Управління користувачами
    /// </summary>
    public class UserManager
    {
        private readonly List<User> users = new();

        /// <summary>1.1 Додавання користувача</summary>
        public void AddUser(User user) => users.Add(user);

        /// <summary>1.2 Видалення користувача</summary>
        public void RemoveUser(User user) => users.Remove(user);

        /// <summary>1.3 Зміна даних користувача</summary>
        public void UpdateUser(User user, string firstName, string lastName, string group)
        {
            user.FirstName = firstName;
            user.LastName = lastName;
            user.Group = group;
        }

        /// <summary>1.4 Отримати користувача за прізвищем</summary>
        public User GetUser(string lastName) => users.FirstOrDefault(u => u.LastName == lastName);

        /// <summary>1.5 Отримати список всіх користувачів</summary>
        public List<User> GetAllUsers() => users;

        /// <summary>4.2 Пошук користувачів за ключовим словом</summary>
        public List<User> SearchUsers(string keyword) =>
            users.Where(u => u.FirstName.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                             u.LastName.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

        /// <summary>1.5.1 Сортування за ім’ям</summary>
        public List<User> SortByFirstName() => users.OrderBy(u => u.FirstName).ToList();

        /// <summary>1.5.2 Сортування за прізвищем</summary>
        public List<User> SortByLastName() => users.OrderBy(u => u.LastName).ToList();

        /// <summary>1.5.3 Сортування за групою</summary>
        public List<User> SortByGroup() => users.OrderBy(u => u.Group).ToList();
    }

    /// <summary>
    /// 2. Управління документами
    /// </summary>
    public class DocumentManager
    {
        private readonly List<Document> documents = new();

        /// <summary>2.1 Додавання документа</summary>
        public void AddDocument(Document doc) => documents.Add(doc);

        /// <summary>2.2 Видалення документа</summary>
        public void RemoveDocument(Document doc) => documents.Remove(doc);

        /// <summary>2.3 Зміна даних документа</summary>
        public void UpdateDocument(Document doc, string title, string author)
        {
            doc.Title = title;
            doc.Author = author;
        }

        /// <summary>2.4 Отримати документ за назвою</summary>
        public Document GetDocument(string title) => documents.FirstOrDefault(d => d.Title == title);

        /// <summary>2.5 Отримати всі документи</summary>
        public List<Document> GetAllDocuments() => documents;

        /// <summary>4.1 Пошук документів за ключовим словом</summary>
        public List<Document> SearchDocuments(string keyword) =>
            documents.Where(d => d.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                                 d.Author.Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();

        /// <summary>2.5.1 Сортування за назвою</summary>
        public List<Document> SortByTitle() => documents.OrderBy(d => d.Title).ToList();

        /// <summary>2.5.2 Сортування за автором</summary>
        public List<Document> SortByAuthor() => documents.OrderBy(d => d.Author).ToList();
    }

    /// <summary>
    /// 3. Управління видачами документів
    /// </summary>
    public class BorrowManager
    {
        private readonly UserManager userManager;

        public BorrowManager(UserManager userManager)
        {
            this.userManager = userManager;
        }

        /// <summary>3.1 Видача документа користувачу</summary>
        public void BorrowDocument(User user, Document doc)
        {
            user.Card.Borrow(doc);
        }

        /// <summary>3.4 Повернення документа в бібліотеку</summary>
        public void ReturnDocument(User user, Document doc)
        {
            user.Card.Return(doc);
        }

        /// <summary>3.3 Пошук користувача, в якого документ</summary>
        public User GetOwnerOfDocument(Document doc)
        {
            return userManager.GetAllUsers().FirstOrDefault(u => u.Card.HasDocument(doc));
        }

        /// <summary>3.2 Перегляд усіх документів користувача</summary>
        public List<Document> GetDocumentsOfUser(User user) => user.Card.BorrowedDocuments;
    }

    /// <summary>
    /// Центральна точка доступу до менеджерів
    /// </summary>
    public class Library
    {
        /// <summary>Менеджер користувачів</summary>
        public UserManager UserManager { get; private set; } = new();

        /// <summary>Менеджер документів</summary>
        public DocumentManager DocumentManager { get; private set; } = new();

        /// <summary>Менеджер видач</summary>
        public BorrowManager BorrowManager { get; private set; }

        public Library()
        {
            BorrowManager = new BorrowManager(UserManager);
        }
    }
}

