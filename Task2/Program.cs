using Task2.Data;

var webAppBuilder = WebApplication.CreateBuilder(args);

webAppBuilder.Services.AddControllers();

webAppBuilder.Services.AddEndpointsApiExplorer();
webAppBuilder.Services.AddSwaggerGen();

webAppBuilder.Services.AddDbContext<BooksContext>();

var app = webAppBuilder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();