using DomainLayer.Interfaces;
using DomainLayer.Models;
using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.Data.SqlClient;

namespace RentalSystem.Data
{
    public class DbInit
    {
        public static async Task CreateDatabaseAndTablesAsync(string connectionString)
        {
            string localConnectionString = $"Server=(localdb)\\mssqllocaldb;Database=master;Trusted_Connection=True;";
            using (SqlConnection connection = new SqlConnection(localConnectionString))
            {
                await connection.OpenAsync();

                SqlCommand command = new SqlCommand("IF EXISTS (SELECT * FROM sys.databases WHERE name = 'rentalCarDb') DROP DATABASE [rentalCarDb]", connection);
                await command.ExecuteNonQueryAsync();

                command = new SqlCommand("CREATE DATABASE [rentalCarDb]", connection);
                await command.ExecuteNonQueryAsync();
            }

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                //Создание таблицы для ролей
                SqlCommand command = new SqlCommand($"""
                    CREATE TABLE [Roles] (
                        [Id] INT IDENTITY(1,1) PRIMARY KEY,
                        [Name] NVARCHAR(255) NOT NULL
                    );
                    """, connection);
                await command.ExecuteNonQueryAsync();

                //Создание таблицы для пользователей
                command.CommandText = """
                    CREATE TABLE [Users] (
                        [Id] INT IDENTITY(1,1) PRIMARY KEY, -- Уникальный идентификатор пользователя
                        [Email] NVARCHAR(255) UNIQUE, 
                        [FullName] NVARCHAR(255) NOT NULL, -- Полное имя пользователя
                        [HashPassword] NVARCHAR(255) NOT NULL, -- Хэш пароля пользователя
                        [Salt] NVARCHAR(255) NOT NULL, -- Соль для хэширования пароля
                        [RoleId] INT NOT NULL, -- Идентификатор роли пользователя

                        -- Определение внешнего ключа
                        FOREIGN KEY (RoleId) REFERENCES Roles(Id)
                    );              
                    """;
                await command.ExecuteNonQueryAsync();

                //Создание таблицы для настроек пользователей
                command.CommandText = """
                     CREATE TABLE [UserProfiles] (
                        [Id] INT PRIMARY KEY IDENTITY(1,1),             -- Уникальный идентификатор профиля
                        [PhoneNumber] NVARCHAR(20) NULL,                -- Номер телефона (опционально)
                        [Passport] NVARCHAR(50) NULL,                   -- Паспорт (опционально)
                        [DrivingExperience] INT NOT NULL DEFAULT(1),    -- Опыт вождения
                        [Description] NVARCHAR(255) NULL,               -- Описание профиля (опционально)
                        [ProfileImage] NVARCHAR(555) NULL,              -- Изображение профиля (опционально)
                        [UserId] INT NOT NULL,                          -- Внешний ключ на таблицу пользователей

                        CONSTRAINT FK_UserProfiles_User FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]) ON DELETE CASCADE,
                        CONSTRAINT UQ_UserProfiles_UserId UNIQUE ([UserId]) -- Уникальный внешний ключ для связи один к одному
                    );
                    """;
                await command.ExecuteNonQueryAsync();

                //Создание таблицы для диллеров
                command.CommandText = """
                     CREATE TABLE [Dealers] (
                        [Id] INT IDENTITY(1,1) PRIMARY KEY,
                        [FirstName] NVARCHAR(100) NOT NULL,
                        [LastName] NVARCHAR(100) NOT NULL,
                        [WorkExperience] INT NOT NULL,
                        [Mobile] NVARCHAR(20) NOT NULL,
                        [Email] NVARCHAR(255) NOT NULL UNIQUE,
                        [WhatsApp] NVARCHAR(20) NULL,
                        [Fax] NVARCHAR(20) NULL
                    );                                               
                    """;
                await command.ExecuteNonQueryAsync();

                //Создание таблицы для автомобилей
                command.CommandText = """
                     CREATE TABLE [Cars] (
                        [Id] INT PRIMARY KEY IDENTITY(1,1),     -- Уникальный идентификатор с автоинкрементом
                        [Brand] NVARCHAR(100) NOT NULL,         -- Бренд автомобиля (например, Toyota, BMW)
                        [Model] NVARCHAR(100) NOT NULL,         -- Модель автомобиля
                        [Color] NVARCHAR(50),                   -- Цвет автомобиля
                        [Image] NVARCHAR(455),                  -- Ссылка на изображение автомобиля
                        [EngineDisplacement] FLOAT NOT NULL,    -- Объем двигателя (литры)
                        [Year] INT NOT NULL,                    -- Год выпуска
                        [CarType] INT NOT NULL,                 -- Значение CarType (enum)
                        [Transmission] INT NOT NULL,            -- Значение Transmission (enum)
                        [CurrentMileage] FLOAT NOT NULL,        -- Текущий пробег (в милях)
                        [MileageLimit] INT NOT NULL DEFAULT -1, -- Лимит пробега (-1 = без ограничений)
                        [SeatsCount] INT NOT NULL,              -- Количество сидений
                        [Price] DECIMAL(10, 2) NOT NULL,        -- Цена аренды в сутки
                        [RentalStatus] INT NOT NULL DEFAULT 0,  -- Статус аренды
                        [DealerId] INT NOT NULL DEFAULT 1,      -- Идентификатор дилера (связь с таблицей Dealers)

                        -- Определение внешнего ключа для связи с таблицей Dealers
                        CONSTRAINT FK_Cars_Dealers FOREIGN KEY ([DealerId]) REFERENCES [Dealers]([Id])
                            ON DELETE CASCADE  -- Удаление автомобиля при удалении связанного дилера
                            ON UPDATE CASCADE  -- Обновление связей при изменении ключа дилера
                    );                                       
                    """;
                await command.ExecuteNonQueryAsync();

                //Создание таблицы для информации об аренде авто
                command.CommandText = """
                     CREATE TABLE [Rentals] (
                        [Id] INT PRIMARY KEY IDENTITY(1,1),
                        [StartDate] DATE NOT NULL,
                        [EndDate] DATE NULL,
                        [StartMileage] FLOAT NOT NULL,
                        [EndMileage] FLOAT NULL,
                        [IsGPSNavigationSystem] BIT DEFAULT 0,
                        [ChildSeat] BIT DEFAULT 0,
                        [AdditionalDriver] BIT DEFAULT 0,
                        [InsuranceCoverage] BIT DEFAULT 0,
                        [CarId] INT NOT NULL,
                        [UserId] INT NOT NULL,
                        [Status] INT DEFAULT 0 NOT NULL,
                        [PaymentStatus] INT DEFAULT 0 NOT NULL

                        -- Добавление внешних ключей для связи с таблицами Car и User
                        CONSTRAINT FK_Rentals_Car FOREIGN KEY ([CarId]) REFERENCES [Cars]([Id]),
                        CONSTRAINT FK_Rentals_User FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
                    );                              
                    """;
                await command.ExecuteNonQueryAsync();

                //Создание таблицы для хранения отзывов об автомобилях
                command.CommandText = """
                     CREATE TABLE [Reviews] (
                        [Id] INT PRIMARY KEY IDENTITY(1,1),
                        [CreatedAt] DATETIME NOT NULL DEFAULT(GETDATE()),
                        [Rating] INT NOT NULL,
                        [Message] NVARCHAR(MAX) NOT NULL,
                        [UserId] INT NOT NULL,
                        [CarId] INT NOT NULL,
                        CONSTRAINT [FK_Reviews_User] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
                        CONSTRAINT [FK_Reviews_Car] FOREIGN KEY ([CarId]) REFERENCES [Cars]([Id])
                    );                                                
                    """;
                await command.ExecuteNonQueryAsync();
            }
        }
        public static async Task InitializeAsync(IUser users, IRole roles, ICar cars, IDealer dealers)
        {
            //Roles
            await roles.AddRoleAsync(new Role { Name = "Admin" });
            await roles.AddRoleAsync(new Role { Name = "Employee" });
            await roles.AddRoleAsync(new Role { Name = "Customer" });

            //Users

            //Admin
            User adminUser = new User
            {
                Email = "admin@gmail.com",
                FullName = "John Smith",
                RoleId = 1,
                HashPassword = "qwerty",
                Profile = new UserProfile
                {
                    Passport = "AE45678PR",
                    DrivingExperience = 5,
                    PhoneNumber = "+380967976548",
                }
            };
            await users.AddUserAsync(adminUser);

            //Employee
            User employeeUser = new User
            {
                Email = "marry@gmail.com",
                FullName = "Marry Bollow",
                RoleId = 2,
                HashPassword = "192837",
                Profile = new UserProfile
                {
                    Passport = "CE34567PL",
                    DrivingExperience = 16,
                    PhoneNumber = "+380975329488",
                }
            };
            await users.AddUserAsync(employeeUser);

            //Customer
            User customerUser = new User
            {
                Email = "alex@gmail.com",
                FullName = "Alex Morrow",
                RoleId = 3,
                HashPassword = "192837",
                Profile = new UserProfile
                {
                    Passport = "AP34128ZM",
                    DrivingExperience = 2,
                    PhoneNumber = "+380507698711",
                }
            };
            await users.AddUserAsync(customerUser);

            //Dealers
            await dealers.AddDealerAsync(new Dealer
            {
                Email = "emily-rose@gmail.com",
                Fax = "1-222-333-4444",
                FirstName = "Emily",
                LastName = "Rose",
                Mobile = "1-222-333-4444",
                WhatsApp = "1-222-333-4444",
                WorkExperience = 5,
            });

            await dealers.AddDealerAsync(new Dealer
            {
                Email = "jacob-brown@gmail.com",
                Fax = "1-321-432-3190",
                FirstName = "Jacob",
                LastName = "Brown",
                Mobile = "1-321-432-3190",
                WhatsApp = "1-321-432-3190",
                WorkExperience = 2,
            });


            //Cars
            Car camry = new Car
            {
                Brand = "Toyota",
                Model = "Camry",
                Color = "White",
                Image = "/uploads/cars/camry.jpg",
                EngineDisplacement = 2.5,
                Year = 2022,
                CarType = CarType.Sedan,
                Transmission = Transmission.Automatic,
                CurrentMileage = 12000,
                MileageLimit = -1,
                SeatsCount = 5,
                Price = 50.99m,
                DealerId = 1
            };
            await cars.AddCarAsync(camry);

            Car bmw5 = new Car
            {
                Brand = "BMW",
                Model = "X5",
                Color = "Black",
                Image = "/uploads/cars/bmwX5.jpg",
                EngineDisplacement = 3.0,
                Year = 2021,
                CarType = CarType.Suv,
                Transmission = Transmission.SemiAutomatic,
                CurrentMileage = 30000,
                MileageLimit = 1000,
                SeatsCount = 7,
                Price = 99.99m,
                DealerId = 1

            };
            await cars.AddCarAsync(bmw5);

            Car audiA6 = new Car
            {
                Brand = "Audi",
                Model = "A6",
                Color = "Gray",
                Image = "/uploads/cars/audiA6.jpg",
                EngineDisplacement = 2.8,
                Year = 2020,
                CarType = CarType.Sedan,
                Transmission = Transmission.Manual,
                CurrentMileage = 25000,
                MileageLimit = 1500,
                SeatsCount = 5,
                Price = 80.50m,
                DealerId = 1
            };
            await cars.AddCarAsync(audiA6);
        }
    }
}
