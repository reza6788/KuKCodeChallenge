using Autofac;
using Autofac.Extensions.DependencyInjection;
using Kuk.Data;
using Kuk.Data.IRepositories;
using Kuk.Data.Repositories;
using Kuk.Services.Configuration;
using Kuk.Services.Services.Note.Implementation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

//Register Services and Repositories by AutoFac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory())
    .ConfigureContainer<ContainerBuilder>(x => { x.RegisterModule(new ServiceModule()); });

//builder.Services.AddScoped<INoteRepository, NoteRepository>();
//builder.Services.AddScoped<INoteService, NoteService>();

// Add services to the container.
builder.Services.AddDbContext<KukDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("WebApiDatabase"),
        b => b.MigrationsAssembly(typeof(KukDbContext).Assembly.FullName)));



builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllersWithViews();

var app = builder.Build();

ILifetimeScope autofacContainer = ((IApplicationBuilder)app).ApplicationServices.GetAutofacRoot();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
