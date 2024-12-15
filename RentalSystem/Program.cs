using DomainLayer;
using DomainLayer.Interfaces;
using InfrastructureLayer.Repository;
using Microsoft.AspNetCore.Authentication.Cookies;
using RentalSystem.Data;
using RentalSystem.Interfaces;
using RentalSystem.RazorHelpers;
using RentalSystem.Repository;

/*

System.Data.SqlClient

//������ ����������� �� ���� �������!

������:
1. https://yesicon.app/mage  (������ �������������: <iconify-icon icon="mage:image-plus" class="text-xl"></iconify-icon>  mage:image-plus)
2. https://yesicon.app/solar (������ �������������: <iconify-icon icon="solar:diagram-down-bold" class="text-xl"></iconify-icon> solar:bus-bold)

0. �� �������� Employee - ���������� ��� ������ �� ���� ����� � ���������.
1. ������� �������� � ������� �������� � �� ���������� ����, � ������������
��������� ����������� ������������� �������. (Toyota � �.�.)
2. ������� �������� � ���������� � ����������� ����������� �������� ����� �� �����,
������� ����� ������� ����� � ���������� �����.

����������� ��� ������:

3. ����������� ������������ / �� ������������ �����.
4. ���������� ����� ������ �������� (���������� �� ������� �������� AdminDashBoard).
 */


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/login";
       });
builder.Services.AddSingleton<IConnectionStringProvider, ConnectionStringProvider>();
builder.Services.AddScoped<RentalSystem.Services.QueryBuilder>();
builder.Services.AddScoped<IUser, UserRepository>();
builder.Services.AddScoped<IRole, RoleRepository>();
builder.Services.AddScoped<ICar, CarRepository>();
builder.Services.AddScoped<IRental, RentalRepository>();
builder.Services.AddScoped<IReview, ReviewRepository>();
builder.Services.AddScoped<IDealer, DealerRepository>();
builder.Services.AddScoped<IWishList, WishListRepository>();
builder.Services.AddScoped<ITestDrive, TestDriveRepository>();

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var services = scope.ServiceProvider;
//    try
//    {
//        //��� �������� ���� ������, ������ � ���������� ������, ����������������� ��� ������:
//        var connectionStringProvider = services.GetRequiredService<IConnectionStringProvider>();
//        string connection = connectionStringProvider.GetConnectionString();
//        await DbInit.CreateDatabaseAndTablesAsync(connection);

//        var users = services.GetRequiredService<IUser>();
//        var roles = services.GetRequiredService<IRole>();
//        var cars = services.GetRequiredService<ICar>();
//        var dealers = services.GetRequiredService<IDealer>();
//        await DbInit.InitializeAsync(users, roles, cars, dealers);

//        //��������� �������� ������
//        TestData testData = new TestData();
//        await testData.GenerateUsersAsync(connection, 50);
//        await testData.GenerateCarsAsync(connection, 50);
//    }
//    catch (Exception ex)
//    {
//        var logger = services.GetRequiredService<ILogger<Program>>();
//        logger.LogError(ex, "An error occurred while seeding the database.");
//    }
//}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();
app.MapRazorPages();

app.Use(async (context, next) =>
{
    await next.Invoke();

    if (context.Response.StatusCode == 404)
        context.Response.Redirect("/404");
});


app.Run();


