using EmployeeManagementSystem.Services;

namespace EmployeeManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Company company = new Company();

            company.SeedData();

            bool running = true;

            do
            {
                DisplayMenu();

                Console.Write("Choose an option: ");
                string? choice = Console.ReadLine();

                Console.WriteLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddDepartment(company);
                            break;

                        case "2":
                            AddEmployee(company);
                            break;

                        case "3":
                            company.ProcessNextOnboarding();
                            break;

                        case "4":
                            AddEmployeeSkill(company);
                            break;

                        case "5":
                            SearchEmployees(company);
                            break;

                        case "6":
                            DisplayDepartmentEmployees(company);
                            break;

                        case "7":
                            company.DisplayDepartmentSalaryAverages();
                            break;

                        case "8":
                            company.DisplayDepartmentEmployeeCounts();
                            break;

                        case "9":
                            company.DisplayActionHistory();
                            break;

                        case "10":
                            company.UndoLastAction();
                            break;

                        case "11":
                            company.DisplayCompanySkills();
                            break;

                        case "12":
                            company.DisplayDepartments();
                            break;

                        case "0":
                            running = false;
                            Console.WriteLine("Program closed.");
                            break;

                        default:
                            Console.WriteLine("Invalid option.");
                            break;
                    }
                }
                catch (Exception exception)
                {
                    Console.WriteLine($"Error: {exception.Message}");
                }

                if (running)
                {
                    Console.WriteLine();
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();

                    Console.Clear();
                }

            } while (running);
        }

       
        // Display menu
        

        static void DisplayMenu()
        {
            Console.WriteLine("======================================");
            Console.WriteLine("     Employee Management System");
            Console.WriteLine("======================================");
            Console.WriteLine("1.  Add Department");
            Console.WriteLine("2.  Add Employee");
            Console.WriteLine("3.  Process Next Onboarding");
            Console.WriteLine("4.  Add Employee Skill");
            Console.WriteLine("5.  Search Employees");
            Console.WriteLine("6.  Display Department Employees");
            Console.WriteLine("7.  Salary Average Report");
            Console.WriteLine("8.  Department Employee Count");
            Console.WriteLine("9.  Display Action History");
            Console.WriteLine("10. Undo Last Action");
            Console.WriteLine("11. Display Company Skills");
            Console.WriteLine("12. Display Departments");
            Console.WriteLine("0.  Exit");
            Console.WriteLine("======================================");
        }

        
        // Add department
    

        static void AddDepartment(Company company)
        {
            Console.Write("Enter department name: ");
            string name = Console.ReadLine() ?? string.Empty;

            company.AddDepartment(name);

            Console.WriteLine("Department added successfully.");
        }

        
        // Add employee
    

        static void AddEmployee(Company company)
        {
            Console.Write("Enter employee name: ");
            string name = Console.ReadLine() ?? string.Empty;

            int departmentId = ReadInt(
                "Enter department ID: "
            );

            decimal salary = ReadDecimal(
                "Enter employee salary: "
            );

            bool isManager = ReadYesOrNo(
                "Is the employee a manager? (y/n): "
            );

            company.AddEmployee(
                name,
                departmentId,
                salary,
                isManager
            );

            Console.WriteLine("Employee added successfully.");
            Console.WriteLine(
                "The employee was added to the onboarding queue."
            );
        }

       
        // Add skill
       

        static void AddEmployeeSkill(Company company)
        {
            int employeeId = ReadInt(
                "Enter employee ID: "
            );

            Console.Write("Enter skill name: ");
            string skill = Console.ReadLine() ?? string.Empty;

            company.AddSkillToEmployee(
                employeeId,
                skill
            );

            Console.WriteLine("Skill added successfully.");
        }

        
        // Search employees
       

        static void SearchEmployees(Company company)
        {
            Console.Write(
                "Enter employee ID or employee name: "
            );

            string searchValue =
                Console.ReadLine() ?? string.Empty;

            company.SearchEmployees(searchValue);
        }

        
        // Display department employees
      

        static void DisplayDepartmentEmployees(
            Company company)
        {
            int departmentId = ReadInt(
                "Enter department ID: "
            );

            company.DisplayEmployeesByDepartment(
                departmentId
            );
        }

       
        // Read integer safely
       

        static int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                string? input = Console.ReadLine();

                bool isValid = int.TryParse(
                    input,
                    out int value
                );

                if (isValid && value > 0)
                {
                    return value;
                }

                Console.WriteLine(
                    "Please enter a valid positive integer."
                );
            }
        }

        
        // Read decimal safely
       

        static decimal ReadDecimal(string message)
        {
            while (true)
            {
                Console.Write(message);

                string? input = Console.ReadLine();

                bool isValid = decimal.TryParse(
                    input,
                    out decimal value
                );

                if (isValid && value >= 0)
                {
                    return value;
                }

                Console.WriteLine(
                    "Please enter a valid positive number."
                );
            }
        }

       
        // Read yes or no safely
        

        static bool ReadYesOrNo(string message)
        {
            while (true)
            {
                Console.Write(message);

                string answer =
                    Console.ReadLine()?.Trim().ToLower()
                    ?? string.Empty;

                if (answer == "y" ||
                    answer == "yes")
                {
                    return true;
                }

                if (answer == "n" ||
                    answer == "no")
                {
                    return false;
                }

                Console.WriteLine(
                    "Please enter y or n."
                );
            }
        }
    }
}