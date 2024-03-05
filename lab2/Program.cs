using System;
using System.Data;
using System.Data.Odbc;
using System.Linq;

class Acaunt
{
    public string id { get; set; }
    public string name { get; set; }
    public string surname { get; set; }
    public string patronymic { get; set; }
    public DateTime birthday { get; set; }

    public Acaunt(string id, string name, string surname, string patronymic, DateTime birthday)
    {
        this.id = id;
        this.name = name;
        this.surname = surname;
        this.patronymic = patronymic;
        this.birthday = birthday.Date; // Устанавливаем время как начало дня
    }
}

class Program
{
    static void Main()
    {
        string dsnName = "DrovaMySQL"; 

        string connectionString = $"DSN={dsnName};";

        using (OdbcConnection connection = new OdbcConnection(connectionString))
        {
            connection.Open();
            Console.WriteLine("Соединение установлено успешно.");
            string query = "SELECT * FROM acaunt";
            OdbcCommand command = new OdbcCommand(query, connection);
            OdbcDataReader reader = command.ExecuteReader();

            var accounts = from IDataRecord record in reader
                           select new Acaunt(
                               record["id"].ToString(),
                               record["name"].ToString(),
                               record["surname"].ToString(),
                               record["patronymic"].ToString(),
                               Convert.ToDateTime(record["birthday"]).Date
                           );

            
            var filteredAccounts = accounts.Where(account => account.surname == "Smith");

            foreach (var account in filteredAccounts)
            {
                Console.WriteLine($"ID: {account.id}, Name: {account.name}, Surname: {account.surname}, Patronymic: {account.patronymic}, Birthday: {account.birthday.ToShortDateString()}");
            }
        }
    }
}
