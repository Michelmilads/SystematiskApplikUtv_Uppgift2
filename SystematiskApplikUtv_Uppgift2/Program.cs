using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SystematiskApplikUtv_Uppgift2.Repository;
using SystematiskApplikUtv_Uppgift2.Repository.Interfaces;
using SystematiskApplikUtv_Uppgift2.Repository.Repos;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAuthentication(opt =>
{
    opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(opt =>
{
    opt.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "http://localhost:5265",
        ValidAudience = "http://localhost:5265",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("Kaninburenärborta2001"))
    };
});


builder.Services.AddControllers();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<IRatingRepo, RatingRepo>();
builder.Services.AddScoped<IFoodCategoryRepo, FoodCategoryRepo>();
builder.Services.AddScoped<IRecipeRepo, RecipeRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
builder.Services.AddSingleton<IDatabaseConnection, DatabaseConnection>();


var app = builder.Build();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

app.UseSwagger();
app.UseSwaggerUI();

app.Run();

