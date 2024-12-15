using DomainLayer.Models;
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
        Task<(List<TestDrive> Reviews, int TotalCount)> GetAllTestDrivesAsync(int page, int pageSize = 10);
    }
}
