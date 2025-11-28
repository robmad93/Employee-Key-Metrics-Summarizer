using System.ComponentModel;
using System.Net;
using System.Numerics;

class Program
{
    public static void Main(string[] args)
    {
        string filePath = "./resources/SD-TA-001-A_OrganisationWeeklyTimesheet.csv"; // Sets filepath to CSV file in resources folder.
        string[] fileInfo = GetLinesFromFile(filePath);
        Employee[] employees = ConvertToArray(fileInfo);
        StartUpProcess(filePath);
        CreateTextFiles(employees);

        /*
        Test to write every department's total/average hours and employee(s) with most hours worked to the console.
        */
        // Console.WriteLine(totalFinanceDepHours);
        // Console.WriteLine(totalMarketingDepHours);
        // Console.WriteLine(totalManagementDepHours);
        // Console.WriteLine(totalHRDepHours);
        // Console.WriteLine(totalEngineeringDepHours);
        // Console.WriteLine(totalManagementDepHours);
        // Console.WriteLine(averageFinanceDepHours);
        // Console.WriteLine(averageMarketingDepHours);
        // Console.WriteLine(averageHRDepHours);
        // Console.WriteLine(averageEngineeringDepHours);
        // Console.WriteLine(averageManagementDepHours);
        // Console.WriteLine(topFinanceDepEmployee);
        // Console.WriteLine(topMarketingDepEmployee);
        // Console.WriteLine(topHRDepEmployee);
        // Console.WriteLine(topEngineeringDepEmployee);
        // Console.WriteLine(topManagementDepEmployee);
    }

    public static void StartUpProcess(string filePath)
    /*
    Function that takes a file path, then asks the user to verify whether they have placed a CSV file
    in the resources folder. If they input "no", it asks them to add a CSV then asks them again if they have
    added a CSV file. If they input "yes", it checks whether the file ends with ".csv". If it doesn't, the
    program outputs an invalid input message to the console and quits the program. If it does, the program
    outputs a processing message to the console and makes a string array of the CSV file.
    */
    {
        Console.WriteLine("Welcome to the Employee Key Metrics Summarizer (EKMS).");
        Console.WriteLine("Have you placed an employee metrics CSV file in the resources folder? (y/n)");
        string response = Console.ReadLine().Trim().ToLower(); // Allows for both uppercase and lowercase inputs to be accepted.
        bool correctFileType = filePath.EndsWith(".csv");
        while (response != "y" && response != "yes")
        {
            Console.WriteLine("Please place a CSV file in the resources folder.");
            Console.WriteLine("Have you placed an employee metrics CSV file in the resources folder? (y/n)");
            response = Console.ReadLine().Trim().ToLower();
        }

        if (correctFileType != true) // Somewhat redundant, as program will not detect file change while running. 
        {
            Console.WriteLine("Invalid file type detected.");
            Console.WriteLine("Please restart the program and try again");
        }
        else
        {
            Console.WriteLine("Processing file. Please wait.");
        }

    }

    public static string GetDepartment()
    /*
    Function that gets a user to select from a list of valid department names. If the user inputs an incorrect option,
    they are prompted to retry. Otherwise, a message declares that the department text file is being created, and the
    users input is passed into a string called "userInput".
    */
    {
        List<string> validOptions = new List<string> { "Finance", "Marketing", "Human Resources", "Engineering", "Management" };
        Console.WriteLine("Please enter a department name (options: Finance, Marketing, Human Resources, Engineering, Management): ");
        string userInput = Console.ReadLine(); // User must enter department name in title case. Hence, no "Trim().ToLower()."

        while (validOptions.Contains(userInput) == false)
        {
            Console.WriteLine("Invalid input. Please enter a suitable option (options: Finance, Marketing, Human Resources, Engineering, Management)");
            userInput = Console.ReadLine(); // User must enter department name in title case. Hence, no "Trim().ToLower()."
        }
        Console.WriteLine($"You have selected the {userInput} department. Creating text file.");
        return userInput;
    }
    private static string[] GetLinesFromFile(string filePath)
    /*
    Function that follows a file path, opens the file, reads all lines from the file to a string array, and
    returns that array.
    */
    {
        string[] lines = File.ReadAllLines(filePath);
        return lines;
    }
    public static Employee[] ConvertToArray(string[] employeeInfo)
    /*
    Function that takes a string array (ideally output from GetLinesFromFile) and creates Employee objects
    for each string array excluding the first one (contains the data column titles).
    */
    {
        Employee[] employees = new Employee[employeeInfo.Length - 1];

        for (int i = 1; i < employeeInfo.Length; ++i)
        {
            string[] employeeDetails = employeeInfo[i].Split(",");
            Employee newEmployee = new Employee();
            newEmployee.Name = employeeDetails[0];
            newEmployee.Department = employeeDetails[1];
            newEmployee.HoursMonday = Int32.Parse(employeeDetails[2]);
            newEmployee.HoursTuesday = Int32.Parse(employeeDetails[3]);
            newEmployee.HoursWednesday = Int32.Parse(employeeDetails[4]);
            newEmployee.HoursThursday = Int32.Parse(employeeDetails[5]);
            newEmployee.HoursFriday = Int32.Parse(employeeDetails[6]);

            employees[i - 1] = newEmployee;
        }
        return employees;
    }
    private static string[] GetOutputMessages(Employee[] employees, string department)
    /*
    Function that takes an Employee class object and department string, then returns a string array of output 
    messages.
    */
    {
        int totalHours = Employee.CalculateTotalDepartmentHours(employees, department);
        double averageHours = Employee.CalculateAverageDepartmentHours(employees, department);
        string departmentMessage = $"Department – {department}\n";
        string totalHoursMessage = $"Total hours worked by the {department} department: {totalHours}\n";
        string averageHoursMessage = $"Average hours worked by the {department} department: {averageHours}\n";
        string topEmployeeMessage = Employee.GetEmployeeWithMostHours(employees, department);
        string[] outputMessages = new string[4];
        outputMessages[0] = departmentMessage;
        outputMessages[1] = totalHoursMessage;
        outputMessages[2] = averageHoursMessage;
        outputMessages[3] = topEmployeeMessage;
        return outputMessages;
    }
    private static void SaveOutputMessages(string fileLocation, Employee[] employees, string department)
    {
    /*
    Function that takes a file location string, Employee class object, and department string, then creates a
    string array of different messages using GetOutputMessages(). It then creates a string from the string array
    that separates the different messages using \n and writes the messages to the file location specified in the
    CreateTextFiles() function.
    */
        string[] textLines = GetOutputMessages(employees, department);
        string fileContents = string.Join("\n", textLines);
        File.WriteAllText(fileLocation, fileContents);
    }
    public static void CreateTextFiles(Employee[] employees)
    /*
    Function that culminates the functionality of GetDepartment() and SaveOutputMessageS(). It takes an Employee
    class object and prompts the user to select a department. It then sets a file location per the user's choice,
    then creates a text file of the department's stats. It then prompts the user if they would like to create another
    text file. If the user prompts "y" or "yes", they can create another file. If "n" or "no" is prompted, the program
    exits. If prompted with anything else, the user is prompted to retry.
    */
    {
        string department = GetDepartment();
        string fileName = $"./resources/Output_{department}.txt";
        SaveOutputMessages(fileName, employees, department);
        Console.WriteLine("Would you like to create a text file for another department? (y/n)");
        string userInput = Console.ReadLine().Trim().ToLower();
        while (true)
        {
            if (userInput == "y" || userInput == "yes")
            {


                department = GetDepartment();
                fileName = $"./resources/Output_{department}.txt";
                SaveOutputMessages(fileName, employees, department);
                Console.WriteLine("Would you like to create a text file for another department? (y/n)");
                userInput = Console.ReadLine().Trim().ToLower();
            }

            else if (userInput == "n" || userInput == "no")
            {
                Console.WriteLine("Thank you for using EKMS. Goodbye!");
                break;
            }
            else
            {
                Console.WriteLine("Invalid input.");
                Console.WriteLine("Would you like to create a text file for another department? (y/n)");
                userInput = Console.ReadLine().Trim().ToLower();
            }
        }
    }
}