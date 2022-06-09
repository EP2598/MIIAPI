using API.Models;
using API.Models.API;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Repository.Interface
{
    interface IEmployeeRepository
    {
        IEnumerable<Employee> Get();
        Employee Get(string NIK);
        int Insert(InsertEmployeeObj obj);
        int Update(Employee emp);
        int Delete(string NIK);
    }
}
