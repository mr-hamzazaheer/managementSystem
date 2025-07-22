using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Common
{
    public static class Message
    {
        public static string LoginFaild = "Login failed. Incorrect username or password.";
        public static string EmailNotConformed = "Login failed. Please confirm your email address before signing in.";
        public static string Success = "Your request was processed successfully.";
        public static string UpdateSuccessfully = "Changes saved successfully.";
        public static string SubmitedSuccessfully = "Data submitted successfully.";
        public static string Exception = "An unexpected error occurred. Please try again later.";
        public static string Error = "Something went wrong. Please try again.";
        public static string NotFound = "The requested resource was not found.";
        public static string Unauthorized = "You are not authorized to perform this action.";
        public static string InvalidInput = "Invalid input. Please check your data and try again.";
        public static string AlreadyExist = "A record with the provided information already exists.";
    }
}
