namespace API.Extensions
{
    public static class DateTimeExtensions
    {
        public static int CalculateAge(this DateOnly dob)
        {
            var today = DateOnly.FromDateTime(DateTime.Now);
            var age = today.Year - dob.Year;

            if (dob.DayOfYear > today.DayOfYear)
            {
                age--;
            }

            Console.WriteLine($"DOB: {dob}");
            Console.WriteLine($"Today: {today}");
            Console.WriteLine($"Calculated Age: {age}");

            return age;

        }
    }
}
