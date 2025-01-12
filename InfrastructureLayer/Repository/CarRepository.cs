using RentalSystem.Interfaces;
using RentalSystem.Models;
using System.Data;
using RentalSystem.Services;
using System.Data.SqlClient;
using DomainLayer.Models;

namespace RentalSystem.Repository
{
    public class CarRepository : ICar
    {
        private readonly QueryBuilder _queryBuilder;

        public CarRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<bool> AddCarAsync(Car car)
        {
            string query = @"
            INSERT INTO [Cars] 
            ([Brand], [Model], [Color], [Image], [EngineDisplacement], [Year], [CarType], [Transmission], 
             [CurrentMileage], [MileageLimit], [SeatsCount], [Price], [DealerId])
             OUTPUT INSERTED.Id
            VALUES 
            (@Brand, @Model, @Color, @Image, @EngineDisplacement, @Year, @CarType, @Transmission, 
             @CurrentMileage, @MileageLimit, @SeatsCount, @Price, @DealerId)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@Brand", SqlDbType.NVarChar) { Value = car.Brand },
                new SqlParameter("@Model", SqlDbType.NVarChar) { Value = car.Model },
                new SqlParameter("@Color", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(car.Color) ? (object)DBNull.Value : car.Color },
                new SqlParameter("@Image", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(car.Image) ? (object)DBNull.Value : car.Image },
                new SqlParameter("@EngineDisplacement", SqlDbType.Float) { Value = car.EngineDisplacement },
                new SqlParameter("@Year", SqlDbType.Int) { Value = car.Year },
                new SqlParameter("@CarType", SqlDbType.Int) { Value = (int)car.CarType },
                new SqlParameter("@Transmission", SqlDbType.Int) { Value = (int)car.Transmission },
                new SqlParameter("@CurrentMileage", SqlDbType.Float) { Value = car.CurrentMileage },
                new SqlParameter("@MileageLimit", SqlDbType.Int) { Value = car.MileageLimit },
                new SqlParameter("@SeatsCount", SqlDbType.Int) { Value = car.SeatsCount },
                new SqlParameter("@Price", SqlDbType.Decimal) { Value = car.Price },
                new SqlParameter("@DealerId", SqlDbType.Int) { Value = car.DealerId }
            };

            int carId = await _queryBuilder.ExecuteScalarAsync<int>(query, parameters);
            return carId > 0;
        }
        public async Task<IEnumerable<string>> GetCarBrandAsync(bool isAll = true)
        {
            string query = @"
                SELECT DISTINCT [Brand]
                FROM [Cars]
                ORDER BY [Brand];";

            List<string> brands = new List<string>();
            if (isAll)
            {
                brands.Add("All");
            }

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    brands.Add(reader.GetString(0));
                }
            });

            return brands;
        }
        public async Task<(IEnumerable<Car> Cars, int TotalCount)> GetAllCarsAsync(FilterModel filterModel)
        {
            string query = @"
    WITH FilteredCars AS (
        SELECT 
            c.[Id],
            c.[Brand],
            c.[Model],
            c.[Color],
            c.[Image],
            c.[EngineDisplacement],
            c.[Year],
            c.[CarType],
            c.[Transmission],
            c.[CurrentMileage],
            c.[MileageLimit],
            c.[SeatsCount],
            c.[Price]
        FROM [Cars] c
        WHERE (@Filter IS NULL OR (c.Brand LIKE '%' + @Filter + '%' OR c.Model LIKE '%' + @Filter + '%'))
        AND (@Brand = 'ALL' OR @Brand IS NULL OR c.Brand = @Brand)
    )
    SELECT 
        *, 
        COUNT(*) OVER() AS TotalCount
    FROM FilteredCars
    ORDER BY Brand, Model
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            SqlParameter[] parameters = {
        new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)filterModel.Filter ?? DBNull.Value },
        new SqlParameter("@Brand", SqlDbType.NVarChar) { Value = (object)filterModel.Status ?? DBNull.Value },
        new SqlParameter("@Offset", SqlDbType.Int) { Value = (filterModel.Page - 1) * filterModel.PageSize },
        new SqlParameter("@PageSize", SqlDbType.Int) { Value = filterModel.PageSize }
    };

            List<Car> cars = new List<Car>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var car = new Car
                    {
                        Id = reader.GetInt32(0),
                        Brand = reader.GetString(1),
                        Model = reader.GetString(2),
                        Color = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                        EngineDisplacement = reader.GetDouble(5),
                        Year = reader.GetInt32(6),
                        CarType = (CarType)reader.GetInt32(7),
                        Transmission = (Transmission)reader.GetInt32(8),
                        CurrentMileage = reader.GetDouble(9),
                        MileageLimit = reader.GetInt32(10),
                        SeatsCount = reader.GetInt32(11),
                        Price = reader.GetDecimal(12)
                    };
                    cars.Add(car);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(13);
                    }
                }
            }, parameters);

            return (cars, totalCount);
        }
        public async Task<(IEnumerable<Car> Cars, int TotalCount)> GetAllClientCarsAsync(PaginationModel pagination)
        {
            if (pagination.SortItem is null || pagination.SortItem.Equals("Date"))
            {
                pagination.SortItem = "Id";
            }
            string sortDirection = pagination.Desc ? "DESC" : "ASC";

            string carTypeCondition = pagination.CarTypesInt != null && pagination.CarTypesInt.Any()
                ? " AND c.CarType IN (" + string.Join(", ", pagination.CarTypesInt.Select((_, i) => $"@CarType{i}")) + ")"
                : "";

            string brandCondition = pagination.CarBrands != null && pagination.CarBrands.Any()
                ? " AND c.Brand IN (" + string.Join(", ", pagination.CarBrands.Select((_, i) => $"@Brand{i}")) + ")"
                : "";

            string dealerCondition = pagination.DealerId.HasValue
                ? " AND c.DealerId = @DealerId"
                : ""; // Условие для фильтрации по DealerId, если он передан

            string query = $@"
WITH FilteredCars AS (
    SELECT 
        c.[Id],
        c.[Brand],
        c.[Model],
        c.[Color],
        c.[Image],
        c.[EngineDisplacement],
        c.[Year],
        c.[CarType],
        c.[Transmission],
        c.[CurrentMileage],
        c.[MileageLimit],
        c.[SeatsCount],
        c.[Price]
    FROM [Cars] c
    WHERE (@Filter IS NULL OR (c.Brand LIKE '%' + @Filter + '%' OR c.Model LIKE '%' + @Filter + '%')) 
    AND c.Price < @PriceRange
    {carTypeCondition}
    {brandCondition}
    {dealerCondition}  -- добавлено условие для фильтрации по DealerId
),
CarStats AS (
    SELECT 
        r.CarId,
        AVG(CAST(r.Rating AS FLOAT)) AS AverageRating,
        COUNT(r.Id) AS ReviewCount
    FROM [Reviews] r
    GROUP BY r.CarId
)
SELECT 
    fc.*, 
    cs.AverageRating,
    cs.ReviewCount,
    COUNT(*) OVER() AS TotalCount
FROM FilteredCars fc
LEFT JOIN CarStats cs ON fc.Id = cs.CarId
ORDER BY [{pagination.SortItem}] {sortDirection}
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            List<SqlParameter> parameters = new List<SqlParameter>() {
        new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)pagination.Filter ?? DBNull.Value },
        new SqlParameter("@Offset", SqlDbType.Int) { Value = (pagination.Page - 1) * pagination.PageSize },
        new SqlParameter("@PageSize", SqlDbType.Int) { Value = pagination.PageSize },
        new SqlParameter("@PriceRange", SqlDbType.Decimal) { Value = pagination.PriceRange },
    };

            // Добавление параметров для фильтрации по типам машин и брендам
            if (pagination.CarTypesInt != null)
            {
                for (int i = 0; i < pagination.CarTypesInt.Count; i++)
                {
                    parameters.Add(new SqlParameter($"@CarType{i}", SqlDbType.Int) { Value = pagination.CarTypesInt[i] });
                }
            }

            if (pagination.CarBrands != null)
            {
                for (int i = 0; i < pagination.CarBrands.Count; i++)
                {
                    parameters.Add(new SqlParameter($"@Brand{i}", SqlDbType.NVarChar) { Value = pagination.CarBrands[i] });
                }
            }

            // Добавление параметра для фильтрации по DealerId, если он передан
            if (pagination.DealerId.HasValue)
            {
                parameters.Add(new SqlParameter("@DealerId", SqlDbType.Int) { Value = pagination.DealerId.Value });
            }

            List<Car> cars = new List<Car>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var car = new Car
                    {
                        Id = reader.GetInt32(0),
                        Brand = reader.GetString(1),
                        Model = reader.GetString(2),
                        Color = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                        EngineDisplacement = reader.GetDouble(5),
                        Year = reader.GetInt32(6),
                        CarType = (CarType)reader.GetInt32(7),
                        Transmission = (Transmission)reader.GetInt32(8),
                        CurrentMileage = reader.GetDouble(9),
                        MileageLimit = reader.GetInt32(10),
                        SeatsCount = reader.GetInt32(11),
                        Price = reader.GetDecimal(12),
                        AverageRating = reader.IsDBNull(13) ? 0 : reader.GetDouble(13),
                        ReviewCount = reader.IsDBNull(14) ? 0 : reader.GetInt32(14)
                    };
                    cars.Add(car);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(15);
                    }
                }
            }, parameters.ToArray());

            return (cars, totalCount);
        }

        public async Task<bool> RemoveCarAsync(int carId)
        {
            string queryDelete = "DELETE FROM [Cars] WHERE [Id] = @Id";
            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(queryDelete,
            new SqlParameter("@Id", SqlDbType.NVarChar) { Value = carId });
            return rowsAffected > 0;
        }
        public async Task<Car> GetCarAsync(int carId)
        {
            Car? car = null;

            string query = @"
            SELECT [c].[Id], [c].[Brand], [c].[Model], [c].[Color], [c].[Image], [c].[EngineDisplacement], 
                   [c].[Year], [c].[CarType], [c].[Transmission], [c].[CurrentMileage], 
                   [c].[MileageLimit], [c].[SeatsCount], [c].[Price], [c].[DealerId],
                   [d].[Id] AS DealerId, 
                   [d].[FirstName] AS DealerFirstName,
                   [d].[LastName] AS DealerLastName,
                   [d].[WorkExperience] AS DealerWorkExperience,
                   [d].[Mobile] AS DealerMobile,
                   [d].[Email] AS DealerEmail,
                   [d].[WhatsApp] AS DealerWhatsApp,
                   [d].[Fax] AS DealerFax,
                   d.[PhotoUrl] AS DealerPhotoUrl
            FROM [Cars] AS [c]
            LEFT JOIN [Dealers] AS [d] ON [c].[DealerId] = [d].[Id]
            WHERE [c].[Id] = @CarId";

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    car = new Car
                    {
                        Id = reader.GetInt32(0),
                        Brand = reader.GetString(1),
                        Model = reader.GetString(2),
                        Color = reader.IsDBNull(3) ? null : reader.GetString(3),
                        Image = reader.IsDBNull(4) ? null : reader.GetString(4),
                        EngineDisplacement = reader.GetDouble(5),
                        Year = reader.GetInt32(6),
                        CarType = (CarType)reader.GetInt32(7),
                        Transmission = (Transmission)reader.GetInt32(8),
                        CurrentMileage = reader.GetDouble(9),
                        MileageLimit = reader.GetInt32(10),
                        SeatsCount = reader.GetInt32(11),
                        Price = reader.GetDecimal(12),
                        DealerId = reader.GetInt32(13),
                        Dealer = new Dealer 
                        {
                            Id = reader.IsDBNull(14) ? 0 : reader.GetInt32(14),
                            FirstName = reader.IsDBNull(15) ? null : reader.GetString(15),
                            LastName = reader.IsDBNull(16) ? null : reader.GetString(16),
                            WorkExperience = reader.GetInt32(17),
                            Mobile = reader.IsDBNull(18) ? null : reader.GetString(18),
                            Email = reader.IsDBNull(19) ? null : reader.GetString(19),
                            WhatsApp = reader.IsDBNull(20) ? null : reader.GetString(20),
                            Fax = reader.IsDBNull(21) ? null : reader.GetString(21),
                            PhotoUrl = reader.IsDBNull(22) ? null : reader.GetString(22)
                        }
                    };
                }
            }, new SqlParameter("@CarId", SqlDbType.Int) { Value = carId });

            return car;
        }
        public async Task<bool> UpdateCarAsync(Car car)
        {
            string query = @"
            UPDATE [Cars]
            SET 
                [Brand] = @Brand,
                [Model] = @Model,
                [Color] = @Color,
                [Image] = @Image,
                [EngineDisplacement] = @EngineDisplacement,
                [Year] = @Year,
                [CarType] = @CarType,
                [Transmission] = @Transmission,
                [CurrentMileage] = @CurrentMileage,
                [MileageLimit] = @MileageLimit,
                [SeatsCount] = @SeatsCount,
                [Price] = @Price,
                [DealerId] = @DealerId
            WHERE [Id] = @CarId";

            SqlParameter[] parameters =
            {
                new SqlParameter("@CarId", SqlDbType.Int) { Value = car.Id }, // Идентификатор автомобиля для обновления
                new SqlParameter("@Brand", SqlDbType.NVarChar) { Value = car.Brand },
                new SqlParameter("@Model", SqlDbType.NVarChar) { Value = car.Model },
                new SqlParameter("@Color", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(car.Color) ? (object)DBNull.Value : car.Color },
                new SqlParameter("@Image", SqlDbType.NVarChar) { Value = string.IsNullOrEmpty(car.Image) ? (object)DBNull.Value : car.Image },
                new SqlParameter("@EngineDisplacement", SqlDbType.Float) { Value = car.EngineDisplacement },
                new SqlParameter("@Year", SqlDbType.Int) { Value = car.Year },
                new SqlParameter("@CarType", SqlDbType.Int) { Value = (int)car.CarType },
                new SqlParameter("@Transmission", SqlDbType.Int) { Value = (int)car.Transmission },
                new SqlParameter("@CurrentMileage", SqlDbType.Float) { Value = car.CurrentMileage },
                new SqlParameter("@MileageLimit", SqlDbType.Int) { Value = car.MileageLimit },
                new SqlParameter("@SeatsCount", SqlDbType.Int) { Value = car.SeatsCount },
                new SqlParameter("@Price", SqlDbType.Decimal) { Value = car.Price },
                new SqlParameter("@DealerId", SqlDbType.Int) { Value = car.DealerId }
            };

            int affectedRows = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return affectedRows > 0;
        }
        public async Task<(IEnumerable<CarRentalInfo> Cars, int TotalCount)> GetMostRentedCarsAsync(FilterModel filterModel)
        {
            string query = @"
WITH FilteredCars AS (
    SELECT 
        c.Id,
        c.Brand,
        c.Model,
        c.Image,
        COUNT(r.Id) AS RentalsCount,
        SUM(DATEDIFF(DAY, r.StartDate, r.EndDate)) AS TotalDaysRented, -- Общее количество дней аренды
        SUM(DATEDIFF(DAY, r.StartDate, r.EndDate) * c.Price) AS TotalEarnings, -- Общий доход
        MIN(r.StartDate) AS FirstRentalDate, -- Самая первая дата аренды
        MAX(r.EndDate) AS LastRentalDate -- Самая последняя дата аренды
    FROM Cars AS c
    LEFT JOIN Rentals AS r ON c.Id = r.CarId
    WHERE r.Id IS NOT NULL
    AND (@Filter IS NULL OR (c.Brand LIKE '%' + @Filter + '%' OR c.Model LIKE '%' + @Filter + '%'))
    GROUP BY c.Id, c.Brand, c.Model, c.Image, c.Price
)
SELECT 
    *, 
    COUNT(*) OVER() AS TotalCount
FROM FilteredCars
ORDER BY RentalsCount DESC
OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            SqlParameter[] parameters = {
        new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)filterModel.Filter ?? DBNull.Value },
        new SqlParameter("@Offset", SqlDbType.Int) { Value = (filterModel.Page - 1) * filterModel.PageSize },
        new SqlParameter("@PageSize", SqlDbType.Int) { Value = filterModel.PageSize }
    };

            List<CarRentalInfo> cars = new List<CarRentalInfo>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var car = new CarRentalInfo
                    {
                        CarId = reader.GetInt32(0),
                        Brand = reader.GetString(1),
                        Model = reader.GetString(2),
                        Image = reader.IsDBNull(3) ? null : reader.GetString(3),
                        RentalsCount = reader.GetInt32(4),
                        TotalDaysRented = reader.IsDBNull(5) ? 0 : reader.GetInt32(5),
                        TotalEarnings = reader.IsDBNull(6) ? 0 : reader.GetDecimal(6),
                        FirstRentalDate = reader.IsDBNull(7) ? null : reader.GetDateTime(7), 
                        LastRentalDate = reader.IsDBNull(8) ? null : reader.GetDateTime(8)
                    };
                    cars.Add(car);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(9);
                    }
                }
            }, parameters);

            return (cars, totalCount);
        }

    }
}
