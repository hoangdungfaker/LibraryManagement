using LibraryManagement.Data;
using Microsoft.EntityFrameworkCore;
using LibraryManagement.Services;
using LibraryManagement.Services;



var builder = WebApplication.CreateBuilder(args);


builder.Services.AddScoped<AccountService>();
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<LibraryDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<ReaderService>();
builder.Services.AddScoped<StaffService>();
builder.Services.AddScoped<LibraryCardService>();
builder.Services.AddScoped<BorrowService>();
builder.Services.AddScoped<ReportService>();
builder.Services.AddScoped<NotificationService>();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseSession();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();