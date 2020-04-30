using Microsoft.AspNetCore.Http;
using System;

namespace MyApi.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Error", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Error");
            response.Headers.Add("Access-Control-Allow-Origin", "*");
        }

        public static int CalculateUserAge(this DateTime dateBirth)
        {
            int age = DateTime.Now.Year - dateBirth.Year;

            if(DateTime.Now > dateBirth.AddYears(age))
                age --;

            return age;
        }
    }
}