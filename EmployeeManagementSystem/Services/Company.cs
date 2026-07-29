using EmployeeManagementSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Services
{
    public class Company
    {
        private  List<Employee> employees = new List<Employee>();

        private Dictionary<int, Department> departments = new Dictionary<int, Department>();

        private Queue<Employee> onboardingQueue = new Queue<Employee>();
        
        private Stack<string> actionHistory = new Stack<string>();  

        private HashSet<string> companySklls = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

        private int nextEmployeeId = 1;
        private int nextDepartmentId = 1;

        public Department AddDepartment(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Department name is required. ");

            foreach (Department department in departments.Values)
            {
                if (department.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                {
                    throw new InvalidOperationException("Department already exists.");
                }
            }

            Department newDepartment = new Department(nextDepartmentId, name);
            departments.Add(nextDepartmentId, newDepartment);
            nextDepartmentId++;

            nextDepartmentId++;

            actionHistory.Push($"Added department: {newDepartment.Name}");

            return newDepartment;
        }


    }
}
