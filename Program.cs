using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Stripe;
using System.Text;
using WebApplication6;
using WebApplication6.Data;
using WebApplication6.Models;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddDbContext<Appcontext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddIdentity<User, IdentityRole>(options =>
{

}).AddEntityFrameworkStores<Appcontext>();
//////////////////////////////////////////////////////
builder.Services.AddAuthentication(options =>
{
    //check jwt token header
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    //[authorize] › Õ«·… «‰ﬂ ⁄«„·… «ﬂ‘‰ Ê·ÌÂ ’·«ÕÌ«  «·„›—Ê÷ ·Ê «·‘Œ’ „·Ê‘ ’·«ÕÌÂ «·”ÿ— «···Ï  Õ  „⁄‰«Â «‰Â Ìÿ·⁄·Â ·Ì” ·Â ’·«ÕÌÂ
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;//unauthirize
    options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>//verified key 
{
    options.SaveToken = true;
    options.RequireHttpsMetadata = false;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:IssuerIP"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:AudienceIP"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:SecretKey"])),
        ValidateLifetime = true


    };
});



builder.Services.AddEndpointsApiExplorer();
// builder.Services.AddSwaggerGen();



// ≈÷«›… Œœ„«  Swagger
builder.Services.AddSwaggerGen(options =>
{
    // ≈÷«›… ≈⁄œ«œ«  JWT Bearer Token
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Please enter the token into the textbox",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        BearerFormat = "JWT",
    });

    // ≈÷«›… «·√„«‰ ≈·Ï Ã„Ì⁄ «·⁄„·Ì« 
    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("Stripe"));
var stripeSettings = builder.Configuration.GetSection("Stripe").Get<StripeSettings>();
StripeConfiguration.ApiKey = stripeSettings.SecretKey;

builder.Services.AddScoped<Ipaymentservice,Paymentservice>();



var app = builder.Build();


//app.UseDeveloperExceptionPage();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();  

app.UseAuthorization();

app.MapControllers();

app.Run();
