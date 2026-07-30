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

        public void SearchEmployees (string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
                throw new ArgumentException("Saerch value is required");

            //bool found = false; 

            //if (int.TryParse(searchValue, out int id))
            //{
            //    foreach(Employee employee in employees)
            //    {
            //        if ( employee.Id == id)
            //        {
            //            Console.WriteLine(employee.GetInfo());
            //            found = true;
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (Employee employee in employees)
            //    {
            //        if (employee.Name.Contains(searchValue, StringComparison.OrdinalIgnoreCase))
            //        {
            //            Console.WriteLine(employee.GetInfo());  
            //            found = true;
            //        }
            //    }
            //}
            //if (! found)
            //{
            //    Console.WriteLine("No employees found");
            //}

            if (int.TryParse(searchValue, out int id))
            {
                foreach(Employee employee in employees)
                {
                    if (employee.Id == id)
                    {
                        Console.WriteLine(employee.GetInfo());
                        return;
                    }
                }

                Console.WriteLine("No employees found");
            }
        }

        public void DisplayEmployeesByDepartment(int departmentId)
        {
            Department department = GetDepartmentById(departmentId);    

            List<Employee> departmentEmployees = new List<Employee>();

            foreach (Employee employee in employees)
            {
                if (employee.DepartmentId == departmentId)
                {
                    departmentEmployees.Add(employee);
                }
            }

            Console.WriteLine($"Department : {department.Name}");

            if (departmentEmployees.Count == 0)
            {
                Console.WriteLine("This department has no employees. ");
                return;
            }

            foreach(Employee employee in departmentEmployees)
            {
                Console.WriteLine(employee.GetInfo());  
            }
        }

        public void DisplayDepartmentSalaryAverages()
        {
            Dictionary<int, List<Employee>> departmentGroups = new Dictionary<int, List<Employee>>();

            foreach (Employee employee in employees)
            {
                if (!departmentGroups.ContainsKey(employee.DepartmentId))
                {
                    departmentGroups.Add(employee.DepartmentId, new List<Employee>());
                }
                departmentGroups[employee.DepartmentId].Add(employee);
            }

            if(departmentGroups.Count == 0)
            {
                Console.WriteLine("No salary data found");
                return;
            }

            foreach (KeyValuePair<int, List<Employee>> group in departmentGroups)
            {
                int departmentId = group.Key;
                List<Employee> departmentEmployees = group.Value;

                decimal totalSalary = 0;

                foreach(Employee employee in departmentEmployees)
                {
                    totalSalary  += employee.Salary;
                }

                decimal averageSalary = totalSalary / departmentEmployees.Count;

                Department department = GetDepartmentById(departmentId);

                Console.WriteLine($"{department.Name}: Average Salary = {averageSalary:F2}");
            }
        }

        public void DisplayDepartmentEmployeeCounts()
        {
            if (employees.Count == 0)
            {
                Console.WriteLine("No employees found");
                return;
            }

            Dictionary<int, int> employeeCounts = new Dictionary<int, int>();

            foreach (Employee employee in employees)
            {
                int departmentId = employee.DepartmentId;

                if(employeeCounts.ContainsKey(departmentId))
                {
                    employeeCounts[departmentId]++;
                }
                else
                {
                    employeeCounts.Add(departmentId, 1);
                }
            }

            foreach (KeyValuePair<int, int> item in employeeCounts)
            {
                Department department = GetDepartmentById((int)item.Key);
                Console.WriteLine($"{department.Name}: {item.Value} employee(s)");
            }
        }

        public void DisplayActionHistory()
        {
            if (actionHistory.Count == 0)
            {
                Console.WriteLine("Action history is empty");
                return;
            }
            foreach (string action in actionHistory)
            {
                Console.WriteLine(action);
            }
        }

        public void UndoLastAction()
        {
            if (actionHistory.Count == 0)
            {
                Console.WriteLine("There is no action to undo.");
                return;
            }

            string lastAction = actionHistory.Pop();

            Console.WriteLine(
                $"Removed last action from history: {lastAction}"
            );
        }

        public void AssignEmployeeToManager(
    int managerId,
    int employeeId)
        {
            Employee managerEmployee =
                GetEmployeeById(managerId);

            if (managerEmployee is not Manager manager)
            {
                throw new InvalidOperationException(
                    "The selected employee is not a manager."
                );
            }

            Employee teamMember =
                GetEmployeeById(employeeId);

            if (manager.Id == teamMember.Id)
            {
                throw new InvalidOperationException(
                    "A manager cannot manage themselves."
                );
            }

            manager.AddTeamMember(teamMember);

            actionHistory.Push(
                $"Assigned {teamMember.Name} to manager {manager.Name}"
            );
        }

        public void SeedData()
        {
            Department development =
                AddDepartment("Development");

            Department hr =
                AddDepartment("Human Resources");

            Department finance =
                AddDepartment("Finance");

            Employee manager = AddEmployee(
                "Ahmed",
                development.Id,
                18000,
                true
            );

            Employee employee1 = AddEmployee(
                "Sara",
                development.Id,
                10000,
                false
            );

            Employee employee2 = AddEmployee(
                "Mona",
                hr.Id,
                9000,
                false
            );

            Employee employee3 = AddEmployee(
                "Omar",
                finance.Id,
                11000,
                false
            );

            AddSkillToEmployee(manager.Id, "Leadership");
            AddSkillToEmployee(manager.Id, "C#");
            AddSkillToEmployee(employee1.Id, "C#");
            AddSkillToEmployee(employee1.Id, "SQL");
            AddSkillToEmployee(employee2.Id, "Recruitment");

            AssignEmployeeToManager(
                manager.Id,
                employee1.Id
            );
        }


    }
}
