using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime HiringDate { get; }
        public int DepartmentId { get; set; }
        public decimal Salary { get; set; }

        public HashSet<string> Skills { get; } = new(StringComparer.OrdinalIgnoreCase);

        public Employee(int id, string name, int departmentId, decimal salary) 
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException("Employee Name is required. ");

            if (salary < 0)
                throw new ArgumentException("Salary cannot be negative. ");

            Id = id;
            Name = name;
            DepartmentId = departmentId;
            Salary = salary;
            HiringDate = DateTime.Now;
            
        }   

        public virtual string GetInfo()
        {
            return $"ID: {Id}, Name: {Name} -- Department ID: {DepartmentId}, Salary: {Salary}";
        }
    }
}
