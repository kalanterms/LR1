using System.Text.Json;

class Library
{
    private List<Book> books = new();
    private List<User> users = new();
    private const string BooksFile = "books.json";
    private const string UsersFile = "users.json";

    public Library()
    {
        LoadData();
    }

    public void AddBook(string title, string author)
    {
        books.Add(new Book { Title = title, Author = author, IsAvailable = true });
        SaveData();
    }

    public void RegisterUser(string name)
    {
        users.Add(new User { Name = name, BorrowedBooks = new List<string>() });
        SaveData();
    }

    public void BorrowBook(string userName, string bookTitle)
    {
        var user = users.Find(u => u.Name == userName);
        var book = books.Find(b => b.Title == bookTitle && b.IsAvailable);

        if (user != null && book != null)
        {
            book.IsAvailable = false;
            user.BorrowedBooks.Add(bookTitle);
            SaveData();
            Console.WriteLine($"{userName} borrowed '{bookTitle}'");
        }
        else
        {
            Console.WriteLine("Book is not available or user not found.");
        }
    }

    public void ReturnBook(string userName, string bookTitle)
    {
        var user = users.Find(u => u.Name == userName);
        var book = books.Find(b => b.Title == bookTitle);

        if (user != null && book != null && user.BorrowedBooks.Contains(bookTitle))
        {
            book.IsAvailable = true;
            user.BorrowedBooks.Remove(bookTitle);
            SaveData();
            Console.WriteLine($"{userName} returned '{bookTitle}'");
        }
        else
        {
            Console.WriteLine("Book not found in user's borrowed list.");
        }
    }

    public void ShowBooks()
    {
        foreach (var book in books)
        {
            Console.WriteLine($"{book.Title} by {book.Author} - {(book.IsAvailable ? "Available" : "Borrowed")}");
        }
    }

    private void LoadData()
    {
        if (File.Exists(BooksFile))
            books = JsonSerializer.Deserialize<List<Book>>(File.ReadAllText(BooksFile)) ?? new();
        if (File.Exists(UsersFile))
            users = JsonSerializer.Deserialize<List<User>>(File.ReadAllText(UsersFile)) ?? new();
    }

    private void SaveData()
    {
        File.WriteAllText(BooksFile, JsonSerializer.Serialize(books));
        File.WriteAllText(UsersFile, JsonSerializer.Serialize(users));
    }
}

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public bool IsAvailable { get; set; }
}

class User
{
    public string Name { get; set; }
    public List<string> BorrowedBooks { get; set; }
}

class Program
{
    static void Main()
    {
        Library library = new();
        library.AddBook("Незнайка на Луне", "Николай Носов");
        library.RegisterUser("Артём");
        library.BorrowBook("Артём", "Незнайка на Луне");
        library.ShowBooks();
        library.ReturnBook("Артём", "Незнайка на Луне");
        library.ShowBooks();

        //New comment
    }
}
