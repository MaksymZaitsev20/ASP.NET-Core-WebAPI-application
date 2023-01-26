namespace Task2.Data;

public static class Extensions
{
    public static void CreateDbIfNotExists(this IHost host)
    {
        {
            using IServiceScope scope = host.Services.CreateScope();
            IServiceProvider services = scope.ServiceProvider;
            BookContext context = services.GetRequiredService<BookContext>();

            if (context.Database.EnsureCreated() || !context.Books.Any())
            {
                DbInitializer.Initialize(context);
            }
        }
    }
}