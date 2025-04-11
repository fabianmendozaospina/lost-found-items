
using System.Reflection;
using LostAndFoundItems.BLL;
using LostAndFoundItems.DAL;
using Microsoft.EntityFrameworkCore;

namespace LostAndFoundItems
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Register dbcontext with the connection string from appsettings.json.
            builder.Services.AddDbContext<LostAndFoundDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // Add services to the container.
            builder.Services.AddScoped<CategoryRepository>();
            builder.Services.AddScoped<LocationRepository>();
            builder.Services.AddScoped<RoleRepository>();
            builder.Services.AddScoped<ClaimStatusRepository>();
            builder.Services.AddScoped<MatchStatusRepository>();
            builder.Services.AddScoped<UserRepository>();
            builder.Services.AddScoped<ClaimRequestRepository>();
            builder.Services.AddScoped<FoundItemRepository>();
            builder.Services.AddScoped<LostItemRepository>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<LocationService>();
            builder.Services.AddScoped<RoleService>();
            builder.Services.AddScoped<ClaimStatusService>();
            builder.Services.AddScoped<MatchStatusService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<FoundItemService>();
            builder.Services.AddScoped<LostItemService>();
            builder.Services.AddScoped<ClaimRequestService>();

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            builder.Services.AddAutoMapper(typeof(MappingProfile));

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
