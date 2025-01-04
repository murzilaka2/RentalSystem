using DomainLayer.Interfaces;
using DomainLayer.Models;
using RentalSystem.Services;
using System.Data.SqlClient;
using System.Data;
using RentalSystem.Models;

namespace InfrastructureLayer.Repository
{
    public class DealerRepository : IDealer
    {
        private readonly QueryBuilder _queryBuilder;
        public DealerRepository(QueryBuilder queryBuilder)
        {
            _queryBuilder = queryBuilder;
        }

        public async Task<IEnumerable<Dealer>> GetDealersAsync()
        {
            string query = "SELECT [Id],[FirstName],[LastName],[WorkExperience],[Mobile],[Email],[PhotoUrl] FROM [Dealers]";
            List<Dealer> dealers = new List<Dealer>();

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var dealer = new Dealer
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        WorkExperience = reader.GetInt32(3),
                        Mobile = reader.GetString(4),
                        Email = reader.GetString(5),
                        PhotoUrl = reader.GetString(6),
                    };
                    dealers.Add(dealer);
                }
            });

            return dealers;
        }
        public async Task<bool> AddDealerAsync(Dealer dealer)
        {
            string query = @"INSERT INTO [Dealers] ([FirstName], [LastName], [WorkExperience], [Mobile], [Email], [WhatsApp], [Fax], [PhotoUrl]) 
                     VALUES (@FirstName, @LastName, @WorkExperience, @Mobile, @Email, @WhatsApp, @Fax, @PhotoUrl)";

            SqlParameter[] parameters =
            {
                new SqlParameter("@FirstName", SqlDbType.NVarChar) { Value = dealer.FirstName },
                new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = dealer.LastName },
                new SqlParameter("@WorkExperience", SqlDbType.Int) { Value = dealer.WorkExperience },
                new SqlParameter("@Mobile", SqlDbType.NVarChar) { Value = dealer.Mobile },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = dealer.Email },
                new SqlParameter("@WhatsApp", SqlDbType.NVarChar) { Value = (object?)dealer.WhatsApp ?? DBNull.Value },
                new SqlParameter("@Fax", SqlDbType.NVarChar) { Value = (object?)dealer.Fax ?? DBNull.Value },
                new SqlParameter("@PhotoUrl", SqlDbType.NVarChar) { Value = (object?)dealer.PhotoUrl ?? DBNull.Value }
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
        public async Task<(IEnumerable<Dealer> Dealers, int TotalCount)> GetAllDealersAsync(FilterModel filterModel)
        {

            string orderByColumn = filterModel.Status switch
            {
                "First Name" => "FirstName",
                "Last Name" => "LastName",
                _ => "WorkExperience"
            };

            string query = $@"
            WITH FilteredDealers AS (
                SELECT 
                    d.[Id],
                    d.[FirstName],
                    d.[LastName],
                    d.[WorkExperience],
                    d.[Mobile],
                    d.[Email],
                    d.[WhatsApp],
                    d.[Fax],
                    d.[PhotoUrl]
                FROM [Dealers] d
                WHERE (@Filter IS NULL OR (d.FirstName LIKE '%' + @Filter + '%' OR d.LastName LIKE '%' + @Filter + '%'))
            )
            SELECT 
                *, 
                COUNT(*) OVER() AS TotalCount
            FROM FilteredDealers
            ORDER BY {orderByColumn} DESC
            OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;";

            SqlParameter[] parameters = {
                new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)filterModel.Filter ?? DBNull.Value },
                new SqlParameter("@Offset", SqlDbType.Int) { Value = (filterModel.Page - 1) * filterModel.PageSize },
                new SqlParameter("@PageSize", SqlDbType.Int) { Value = filterModel.PageSize }
            };

            List<Dealer> dealers = new List<Dealer>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var dealer = new Dealer
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        WorkExperience = reader.GetInt32(3),
                        Mobile = reader.GetString(4),
                        Email = reader.GetString(5),
                        WhatsApp = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Fax = reader.IsDBNull(7) ? null : reader.GetString(7),
                        PhotoUrl = reader.IsDBNull(8) ? null : reader.GetString(8)
                    };
                    dealers.Add(dealer);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(9);
                    }
                }
            }, parameters);

            return (dealers, totalCount);
        }
        public async Task<(IEnumerable<Dealer> Dealers, int TotalCount)> GetAllDealersWithCarsCountAsync(FilterModel filterModel)
        {
            string orderByColumn = filterModel.Status switch
            {
                "First Name" => "FirstName",
                "Last Name" => "LastName",
                _ => "WorkExperience"
            };

            string query = $@"
    WITH FilteredDealers AS (
        SELECT 
            d.[Id],
            d.[FirstName],
            d.[LastName],
            d.[WorkExperience],
            d.[Mobile],
            d.[Email],
            d.[WhatsApp],
            d.[Fax],
            d.[PhotoUrl]
        FROM [Dealers] d
        WHERE (@Filter IS NULL OR (d.FirstName LIKE '%' + @Filter + '%' OR d.LastName LIKE '%' + @Filter + '%'))
    ),
    DealerWithCarsCount AS (
        SELECT 
            d.Id,
            COUNT(c.Id) AS CarsCount
        FROM [Dealers] d
        LEFT JOIN [Cars] c ON d.Id = c.DealerId
        GROUP BY d.Id
    )
    SELECT 
        d.*, 
        COALESCE(c.CarsCount, 0) AS CarsCount,
        COUNT(*) OVER() AS TotalCount
    FROM FilteredDealers d
    LEFT JOIN DealerWithCarsCount c ON d.Id = c.Id
    ORDER BY {orderByColumn} DESC
    OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY;
    SELECT COUNT(*) FROM [Dealers];";

            SqlParameter[] parameters = {
        new SqlParameter("@Filter", SqlDbType.NVarChar) { Value = (object)filterModel.Filter ?? DBNull.Value },
        new SqlParameter("@Offset", SqlDbType.Int) { Value = (filterModel.Page - 1) * filterModel.PageSize },
        new SqlParameter("@PageSize", SqlDbType.Int) { Value = filterModel.PageSize }
    };

            List<Dealer> dealers = new List<Dealer>();
            int totalCount = 0;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                while (reader.Read())
                {
                    var dealer = new Dealer
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        WorkExperience = reader.GetInt32(3),
                        Mobile = reader.GetString(4),
                        Email = reader.GetString(5),
                        WhatsApp = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Fax = reader.IsDBNull(7) ? null : reader.GetString(7),
                        PhotoUrl = reader.GetString(8),
                        CarsCount = reader.GetInt32(9) 
                    };
                    dealers.Add(dealer);

                    if (totalCount == 0)
                    {
                        totalCount = reader.GetInt32(10);
                    }
                }
            }, parameters);

            return (dealers, totalCount);
        }
        public async Task<Dealer?> GetDealerAsync(int id)
        {
            string query = @"
    SELECT 
        d.[Id],
        d.[FirstName],
        d.[LastName],
        d.[WorkExperience],
        d.[Mobile],
        d.[Email],
        d.[WhatsApp],
        d.[Fax],
        d.[PhotoUrl],
        ISNULL(COUNT(c.[Id]), 0) AS CarCount
    FROM [Dealers] d
    LEFT JOIN [Cars] c ON c.[DealerId] = d.[Id]
    WHERE d.[Id] = @Id
    GROUP BY 
        d.[Id], d.[FirstName], d.[LastName], d.[WorkExperience], d.[Mobile], 
        d.[Email], d.[WhatsApp], d.[Fax], d.[PhotoUrl];";

            SqlParameter[] parameters = {
        new SqlParameter("@Id", SqlDbType.Int) { Value = id }
    };

            Dealer? dealer = null;

            await _queryBuilder.ExecuteQueryAsync(query, reader =>
            {
                if (reader.Read())
                {
                    dealer = new Dealer
                    {
                        Id = reader.GetInt32(0),
                        FirstName = reader.GetString(1),
                        LastName = reader.GetString(2),
                        WorkExperience = reader.GetInt32(3),
                        Mobile = reader.GetString(4),
                        Email = reader.GetString(5),
                        WhatsApp = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Fax = reader.IsDBNull(7) ? null : reader.GetString(7),
                        PhotoUrl = reader.IsDBNull(8) ? null : reader.GetString(8),
                        CarsCount = reader.GetInt32(9) 
                    };
                }
            }, parameters);

            return dealer;
        }

        public async Task<bool> RemoveDealerAsync(int id)
        {
            string query = @"
            DELETE FROM [Dealers]
            WHERE [Id] = @Id;";

            SqlParameter[] parameters = {
                new SqlParameter("@Id", SqlDbType.Int) { Value = id }
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
        public async Task<bool> UpdateDealerAsync(Dealer dealer)
        {
            string query = @"
            UPDATE [Dealers]
            SET 
                [FirstName] = @FirstName,
                [LastName] = @LastName,
                [WorkExperience] = @WorkExperience,
                [Mobile] = @Mobile,
                [Email] = @Email,
                [WhatsApp] = @WhatsApp,
                [Fax] = @Fax
            WHERE [Id] = @Id;";

            SqlParameter[] parameters = {
                new SqlParameter("@FirstName", SqlDbType.NVarChar) { Value = dealer.FirstName },
                new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = dealer.LastName },
                new SqlParameter("@WorkExperience", SqlDbType.Int) { Value = dealer.WorkExperience },
                new SqlParameter("@Mobile", SqlDbType.NVarChar) { Value = dealer.Mobile },
                new SqlParameter("@Email", SqlDbType.NVarChar) { Value = dealer.Email },
                new SqlParameter("@WhatsApp", SqlDbType.NVarChar) { Value = (object?)dealer.WhatsApp ?? DBNull.Value },
                new SqlParameter("@Fax", SqlDbType.NVarChar) { Value = (object?)dealer.Fax ?? DBNull.Value },
                new SqlParameter("@Id", SqlDbType.Int) { Value = dealer.Id }
            };

            int rowsAffected = await _queryBuilder.ExecuteQueryAsync(query, parameters);
            return rowsAffected > 0;
        }
    }
}
