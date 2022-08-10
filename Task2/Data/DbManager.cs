using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Task2.Data
{
    public static class DbManager
    {
        private static SqliteConnection connection;
        public static BooksContext Context { get; private set; }
        public static string SecretKey { get; private set; }

        static DbManager()
        {
            var configurationBuilder = new ConfigurationBuilder();

            configurationBuilder.SetBasePath(Directory.GetCurrentDirectory());
            configurationBuilder.AddJsonFile("appsettings.json");

            connection = new SqliteConnection(
                configurationBuilder.Build()
                .GetConnectionString("BooksContext"));

            SecretKey = configurationBuilder.Build().GetValue<string>("AppSettings:SecretKey");

            connection.Open();

            Context = new BooksContext(
                new DbContextOptionsBuilder<BooksContext>()
                .UseSqlite(connection)
                .Options);

            if (Context.Database.EnsureCreated())
                DbInitializer.Initialize(Context);
        }
    }
}
