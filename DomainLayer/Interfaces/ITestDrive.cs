using DomainLayer.Models;
using RentalSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Interfaces
{
    public interface ITestDrive
    {
        Task<bool> AddTestDriveAsync(TestDrive testDrive);
        Task<bool> RemoveTestDriveAsync(int testDriveId);
        Task<bool> ChangeTestDriveStatusAsync(int testDriveId, TestDriveStatus testDriveStatus);
        Task<(IEnumerable<TestDrive> TestDrives, int TotalCount)> GetAllTestDrivesAsync(FilterModel filterModel);
    }
}
