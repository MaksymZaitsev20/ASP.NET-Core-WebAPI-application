namespace Task2.Data;

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        {
            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<BookContext>();
                if (context.Database.EnsureCreated() || !context.Books.Any())
                {
                    DbInitializer.Initialize(context);
                }
            }
        }
    }
}