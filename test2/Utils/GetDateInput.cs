using System.Globalization;

namespace MedicalRecordsSystem.Utils;

internal class GetDateInput
{
    public static DateTime GetDateFromUser(string dateFormat)
    {
        while (true)
        {
            Console.Write($"Please enter a date following this format: ({dateFormat}) ");
            string? input = Console.ReadLine();

            if (DateTime.TryParseExact(input, dateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date))
            {
                Console.WriteLine($"You entered a valid date: {date:yyyy-MM-dd}");
                return date;
            }
            else
            {
                Console.WriteLine($"Invalid date format. Please enter a date following this format: ({dateFormat}) ");
                return DateTime.MinValue;
            }
        }
    }
}
