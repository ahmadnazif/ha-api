using HaApi.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddSingleton<Logger>();

switch (int.Parse(config["DataSource"]))
{
    case 1: builder.Services.AddSingleton<IDb, InMemoryDb>(); break;
    case 2: builder.Services.AddSingleton<IDb, MySqlDb>(); break;
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(x => x.AddDefaultPolicy(y => y.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.WebHost.ConfigureKestrel(x =>
{
    var httpPort = int.Parse(config["Port"]);
    x.ListenAnyIP(httpPort);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
