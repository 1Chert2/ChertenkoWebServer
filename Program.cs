using ChertenkoWebServer.Data;
using ChertenkoWebServer.Endpoints;
using ChertenkoWebServer.Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddLessonCoreServices(builder.Configuration)
    .AddLessonEndpointGroups();

var app = builder.Build();

// Авто-создание и применение миграций БД при старте
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<TaskDbContext>();
    db.Database.Migrate();
}

app.UseLessonPipeline();
app.MapLessonApi();

app.Run();