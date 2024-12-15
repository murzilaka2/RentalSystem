using RentalSystem.Interfaces;

namespace DomainLayer
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(string name = "DefaultConnection");
    }
}
 

