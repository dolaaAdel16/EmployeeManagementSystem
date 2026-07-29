using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmployeeManagementSystem.Models
{
    public class Manager : Employee
    {
        public List<Employee> TeamMembers { get; } = new();

        public Manager(
            int id,
            string name,
            int departmentId,
            decimal salary) : base(id, name, departmentId, salary) { }

        public override string GetInfo()
        {
            return $"Manager - {base.GetInfo()},Team Members: {TeamMembers.Count}";
        }

        public void AddTeamMember(Employee employee)
        {
            if (employee is null)
                throw new ArgumentNullException(nameof(employee));

            foreach (Employee teamMember in TeamMembers)
            {
                if (teamMember.Id == employee.Id)
                {
                    throw new ArgumentException("Employee is already in this manager's team");
                }
            }
            TeamMembers.Add(employee);
        }
        
        
    }
}
