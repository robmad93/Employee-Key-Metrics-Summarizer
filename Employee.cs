class Employee
{
    public string Name;
    public string Department;
    public int HoursMonday;
    public int HoursTuesday;
    public int HoursWednesday;
    public int HoursThursday;
    public int HoursFriday;

    public static int CalculateTotalDepartmentHours(Employee[] employees, string department) // Could change to return double values, but CSV files presents hours as integers.

    /*
    Function that takes an array of employee objects and a department string, then calculates the total hours
    worked for each employee in a given department. Returns a string with the total hours worked for that department.
    */
    {
        int totalHours = 0;
        for (int i = 0; i < employees.Length; ++i)
        {
            if (employees[i].Department == department)
            {
                totalHours += Convert.ToInt32(employees[i].HoursMonday + employees[i].HoursTuesday + employees[i].HoursWednesday + employees[i].HoursThursday + employees[i].HoursFriday);
            }
        }
        return totalHours;
    }
    public static double CalculateAverageDepartmentHours(Employee[] employees, string department)
    /*
    Function that takes an array of employee objects and a department string, then calculates the average hours
    worked across a given department. Returns a string with the average hours worked for employees in that department.
    */
    {
        double averageHours = 0;
        int numberOfDepEmployees = 0;
        double totalDepHours = Convert.ToDouble(CalculateTotalDepartmentHours(employees, department));
        for (int i = 0; i < employees.Length; ++i)
        {
            if (employees[i].Department == department)
            {
                numberOfDepEmployees += 1;
            }
        }
        averageHours = totalDepHours / numberOfDepEmployees;
        return averageHours;
    }
    public static string GetEmployeeWithMostHours(Employee[] employees, string department)
    /*
    Function that takes an array of employee objects and a department string, then calculates the total hours
    worked for each employee across a given department and adds their name and hours worked to separate lists.
    It gets the max value for the list of hours worked, then checks through the hours list to see if multiple
    max values exsist (i.e., more than one employee has worked the same amount of hours). It adds the name(s) 
    of the employee(s) with the most hours to a new list. If the list has one entry, it returns a string tailored
    to one entry. If more than one employee has attained the max work hours value, a different string is returned.
    */
    {   
        int currentSelectionHours = 0;
        List<int> hoursList= new List<int>{};
        List<string> employeeNames = new List<string>{};
        List<string> topEmployees = new List<string>{};


        for (int i = 0; i < employees.Length; ++i)
        {
            if (employees[i].Department == department)
            {
                currentSelectionHours = Convert.ToInt32(employees[i].HoursMonday + employees[i].HoursTuesday + employees[i].HoursWednesday + employees[i].HoursThursday + employees[i].HoursFriday);
                hoursList.Add(currentSelectionHours);
                employeeNames.Add(employees[i].Name);
            }
        }
        if (hoursList.Count == 0)
        {
            return $"No employees found in {department}.";     // If there are no empoyees listed in a given department, the function outputs a message.
        
        }
        int maxHours = hoursList.Max(); // Finds max value of hoursList.

        for (int i = 0; i < hoursList.Count; ++i)
        {
            if (hoursList[i] == maxHours)
            {
                topEmployees.Add(employeeNames[i]);
            }
        }
        if (topEmployees.Count == 1)
        {
            return $"{topEmployees[0]} from the {department} department worked the most hours (total hours worked: {maxHours}).";
        }
        else
        {
            string multipleEmployees = string.Join(", ", topEmployees); // Joins the names of the top performing employees using commas.
            return $"Employees from the {department} department with the most hours ({maxHours} hours worked): {multipleEmployees}.";
        }
    }
}

