using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.Data.SqlClient;

namespace RentalSystem.Data
{
    public class TestData
    {
        public async Task GenerateUsersAsync(string connectionString, int count)
        {
            string createProcedureSql = $@"
            CREATE PROCEDURE dbo.GenerateRandomUsers
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @i INT = 1;

                WHILE @i <= {count}
                BEGIN
                    DECLARE @Email NVARCHAR(255) = CONCAT('user', @i, '@example.com');
                    DECLARE @FullName NVARCHAR(255) = CONCAT('User ', @i);
                    DECLARE @RoleId INT = CAST(RAND() * 3 + 1 AS INT); 
                    DECLARE @HashPassword NVARCHAR(255) = 'password123'; 
                    DECLARE @Salt NVARCHAR(255) = NEWID();
                    DECLARE @DrivingExperience INT = CAST(RAND() * 19 + 1 AS INT);
        
                    DECLARE @Passport NVARCHAR(50) = CONCAT(
                        CHAR(CAST(RAND() * 26 + 65 AS INT)), 
                        CHAR(CAST(RAND() * 26 + 65 AS INT)), 
                        CAST(RAND() * 900000 + 100000 AS INT)
                    );

                    DECLARE @PhoneNumber NVARCHAR(20) = CONCAT(
                        '+380', 
                        CAST(RAND() * 49 + 50 AS INT), 
                        CAST(RAND() * 9000000 + 1000000 AS INT)
                    );

                    -- Вставка пользователя
                    DECLARE @UserId INT;

                    INSERT INTO Users (Email, FullName, HashPassword, Salt, RoleId)
                    VALUES (@Email, @FullName, @HashPassword, @Salt, @RoleId);

                    SET @UserId = SCOPE_IDENTITY(); -- Получаем Id нового пользователя

                    -- Вставка профиля пользователя
                    INSERT INTO UserProfiles (PhoneNumber, Passport, DrivingExperience, UserId)
                    VALUES (@PhoneNumber, @Passport, @DrivingExperience, @UserId);

                    SET @i = @i + 1;
                END;
            END;";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DROP PROCEDURE IF EXISTS dbo.GenerateRandomUsers", connection))
                {
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = createProcedureSql;
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "dbo.GenerateRandomUsers";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task GenerateCarsAsync(string connectionString, int count)
        {
            string createProcedureSql = $@"
        CREATE PROCEDURE dbo.GenerateRandomCars
        AS
        BEGIN
            SET NOCOUNT ON;

            DECLARE @i INT = 1;

            WHILE @i <= {count}
            BEGIN
                INSERT INTO Cars (
                    Brand, Model, Color, Image, EngineDisplacement, Year, 
                    CarType, Transmission, CurrentMileage, MileageLimit, SeatsCount, Price, DealerId
                )
                VALUES (
                    CONCAT('Brand', CAST((RAND() * 10) + 1 AS INT)), 
                    CONCAT('Model', CAST((RAND() * 20) + 1 AS INT)), 
                    CASE CAST(RAND() * 8 AS INT)
                        WHEN 0 THEN 'Red'
                        WHEN 1 THEN 'Blue'
                        WHEN 2 THEN 'White'
                        WHEN 3 THEN 'Black'
                        WHEN 4 THEN 'Green'
                        WHEN 5 THEN 'Gray'
                        WHEN 6 THEN 'Yellow'
                        ELSE 'Silver'
                    END,
                    '/uploads/cars/default.jpg',
                    ROUND((RAND() * 4.0) + 1.0, 1), 
                    CAST((RAND() * 23) + 2000 AS INT), 
                    CAST(RAND() * 3 AS INT), 
                    CAST(RAND() * 2 AS INT), 
                    ROUND(RAND() * 200000, 2), 
                    CASE CAST(RAND() * 2 AS INT) 
                        WHEN 0 THEN -1
                        ELSE CAST(RAND() * 500 AS INT)
                    END,
                    CAST((RAND() * 6) + 2 AS INT), 
                    ROUND((RAND() * 450) + 50, 2),
                    CAST((RAND() * 4) + 1 AS INT)
                );

                SET @i = @i + 1;
            END;
        END;
        ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();

                using (SqlCommand command = new SqlCommand("DROP PROCEDURE IF EXISTS dbo.GenerateRandomCars", connection))
                {
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = createProcedureSql;
                    await command.ExecuteNonQueryAsync();

                    command.CommandText = "dbo.GenerateRandomCars";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task GenerateRentalsAsync(string connectionString, int count)
        {
            string createProcedureSql = $@"
                CREATE PROCEDURE dbo.GenerateRandomRentals
                AS
                BEGIN
                    SET NOCOUNT ON;

                    DECLARE @i INT = 1;

                    WHILE @i <= {count}
                    BEGIN
                        DECLARE @StartDate DATE = DATEADD(DAY, -CAST(RAND() * 365 AS INT), GETDATE()); -- Случайная дата начала в пределах прошлого года
                        DECLARE @EndDate DATE = DATEADD(DAY, CAST(RAND() * 30 + 1 AS INT), @StartDate); -- Случайная дата окончания после даты начала (до 30 дней)

                        INSERT INTO Rentals (
                            UserId, CarId, StartDate, EndDate, StartMileage, EndMileage, 
                            IsGPSNavigationSystem, ChildSeat, AdditionalDriver, InsuranceCoverage, 
                            Status, PaymentStatus
                        )
                        VALUES (
                            CAST(RAND() * 53 + 1 AS INT), -- Случайный пользователь с ID 1-53
                            CAST(RAND() * 53 + 1 AS INT), -- Случайное авто с ID 1-53
                            @StartDate, -- Дата начала аренды
                            @EndDate, -- Дата окончания аренды
                            ROUND(RAND() * 200000, 2), -- Случайный пробег на старте
                            ROUND(RAND() * 200000 + 1000, 2), -- Случайный пробег на конце (больше стартового)
                            CASE WHEN RAND() < 0.5 THEN 1 ELSE 0 END, -- Случайно выбираем, есть ли GPS
                            CASE WHEN RAND() < 0.5 THEN 1 ELSE 0 END, -- Случайно выбираем, есть ли детское кресло
                            CASE WHEN RAND() < 0.5 THEN 1 ELSE 0 END, -- Случайно выбираем, есть ли дополнительный водитель
                            CASE WHEN RAND() < 0.5 THEN 1 ELSE 0 END, -- Случайно выбираем, есть ли страховка
                            CAST(RAND() * 3 AS INT), -- Случайный статус (0 - активен, 1 - завершен, 2 - отменен)
                            CAST(RAND() * 2 AS INT) -- Случайный статус оплаты (0 - не оплачено, 1 - оплачено)
                        );

                        SET @i = @i + 1;
                    END;
                END;
                ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("DROP PROCEDURE IF EXISTS dbo.GenerateRandomRentals", connection))
                {
                    await command.ExecuteNonQueryAsync();
                    command.CommandText = createProcedureSql;
                    await command.ExecuteNonQueryAsync();
                    command.CommandText = "dbo.GenerateRandomRentals";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }
        public async Task GenerateReviewsAsync(string connectionString, int count)
        {
            string createProcedureSql = $@"
            CREATE PROCEDURE dbo.GenerateRandomReviews
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @i INT = 1;

                WHILE @i <= {count}
                BEGIN
                    DECLARE @Rating INT = CAST(RAND() * 5 + 2 AS INT); -- Случайный рейтинг от 2 до 5
                    DECLARE @Message NVARCHAR(MAX) = CASE 
                        WHEN @Rating = 5 THEN 'Great car! I recommend it to everyone!'
                        WHEN @Rating = 4 THEN 'Its a nice car, but there are minor flaws.'
                        WHEN @Rating = 3 THEN 'Its an average car, but theres a lot to pick on.'
                        WHEN @Rating = 2 THEN 'The car is not bad overall, but there are a lot of improvements.'
                        ELSE 'Very bad car, I dont recommend it.'
                    END;

                    INSERT INTO Reviews (
                        CreatedAt, Rating, Message, UserId, CarId
                    )
                    VALUES (
                        GETDATE(), -- Дата создания отзыва (текущая дата)
                        @Rating, -- Рейтинг отзыва
                        @Message, -- Сообщение отзыва
                        CAST(RAND() * 53 + 1 AS INT), -- Случайный пользователь с ID 1-53
                        CAST(RAND() * 53 + 1 AS INT) -- Случайное авто с ID 1-53
                    );

                    SET @i = @i + 1;
                END;
            END;
            ";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (SqlCommand command = new SqlCommand("DROP PROCEDURE IF EXISTS dbo.GenerateRandomReviews", connection))
                {
                    await command.ExecuteNonQueryAsync();
                    command.CommandText = createProcedureSql;
                    await command.ExecuteNonQueryAsync();
                    command.CommandText = "dbo.GenerateRandomReviews";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    await command.ExecuteNonQueryAsync();
                }
            }
        }

    }
}
