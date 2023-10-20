using HaApi.Services;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

builder.Services.AddSingleton<Logger>();

switch (config["DataSource"])
{
    case "MySqlDb": builder.Services.AddSingleton<IDb, MySqlDb>(); break;
    case "InMemory": builder.Services.AddSingleton<IDb, InMemoryDb>(); break;
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(x => x.AddDefaultPolicy(y => y.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.WebHost.ConfigureKestrel(x =>
{
    var httpPort = int.Parse(config["port"]);
    x.ListenAnyIP(httpPort);
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseCors();

app.UseAuthorization();

app.MapControllers();

await app.RunAsync();
