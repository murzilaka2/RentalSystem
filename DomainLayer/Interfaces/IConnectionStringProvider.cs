namespace RentalSystem.Interfaces
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string name = "DefaultConnection");
    }
}
