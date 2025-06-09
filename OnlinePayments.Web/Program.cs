using OnlinePayments.Repositories;
using OnlinePayments.Repositories.Implementation;
using OnlinePayments.Repositories.Interfaces;
using OnlinePayments.Services.Implementation.Account;
using OnlinePayments.Services.Implementation.Authentication;
using OnlinePayments.Services.Implementation.Payment;
using OnlinePayments.Services.Interfaces.Account;
using OnlinePayments.Services.Interfaces.Authentication;
using OnlinePayments.Services.Interfaces.Payment;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<IPaymentService, PaymentService>();
builder.Services.AddScoped<IAccountService, AccountService>();

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<IUserAndAccountRepository, UserAndAccountRepository>();

ConnectionFactory.Initialize(
    builder.Configuration.GetConnectionString("DefaultConnection"));


builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
