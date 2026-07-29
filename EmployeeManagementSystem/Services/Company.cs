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

        private HashSet<string> companySkills = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

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

        public void DisplayDepartments()
        {
            if (departments.Count == 0)
            {
                Console.WriteLine("No departments found. ");
                return;
            }

            foreach(KeyValuePair<int, Department> item in departments)
            {
                Console.WriteLine(item.Value);
            }
        }

        private Department GetDepartmentById(int departmentId)
        {
            if (!departments.TryGetValue(departmentId, out Department? department))
            {
                throw new InvalidOperationException("Department not found. ");
            }
            return department;
        }

        private Employee GetEmployeeById(int id)
        {
            foreach (Employee employee in employees)
            {
                if (employee.Id == id)
                {
                    return employee;
                }
            }

            throw new Exception("Employee not found.");
        }

        public Employee AddEmployee(string name, int departmentId, decimal salary, bool isManager)
        {
            GetDepartmentById(departmentId);

            Employee employee;

            if (isManager)
            {
                employee = new Manager(nextEmployeeId++, name, departmentId, salary);
            }
            else 
            {
                employee = new Employee(nextEmployeeId++, name, departmentId, salary);
            }
            employees.Add(employee);

            onboardingQueue.Enqueue(employee);

            actionHistory.Push($"Added employee: {employee.Name}");

            return employee;
        }

        public void ProcessNextOnboarding()
        {
            if (onboardingQueue.Count == 0)
            {
                Console.WriteLine("There are no employees eaiting for onboarding. ");
                return;
            }
            Employee employee = onboardingQueue.Dequeue();

            Console.WriteLine($"Onboarding completed for: {employee.Name}");

            actionHistory.Push($"Completed onboarding for: {employee.Name}");
        }
        
        public void  DisplayOnboardingQueue()
        {
            if (onboardingQueue.Count == 0)
            {
                Console.WriteLine("Onboarding queue is empty.");
                return;
            }

            foreach (Employee employee in onboardingQueue)
            {
                Console.WriteLine(employee.GetInfo());
            }
        }

        public void AddSkillToEmployee(int employeeId, string skill)
        {
            if (string.IsNullOrWhiteSpace(skill))
                throw new ArgumentException("Skill is required");

            Employee employee = GetEmployeeById(employeeId);

            bool addedToEmployee = employee.Skills.Add(skill);
            if(!addedToEmployee)
            {
                Console.WriteLine("This employee already has that skill.");
                return;
            }

            companySkills.Add(skill);

            actionHistory.Push($"Added skill{skill} to {employee.Name}");
        }

        public void DisplayCompanySkills()
        {
            if (companySkills.Count == 0)
            {
                Console.WriteLine("No company skills found");
                return;
            }
            foreach (string skill in companySkills)
            {
                Console.WriteLine($"- {skill}");
            }
        }




    }
}
