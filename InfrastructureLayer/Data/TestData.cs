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
                    NULL, -- Без изображения
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
    }
}
