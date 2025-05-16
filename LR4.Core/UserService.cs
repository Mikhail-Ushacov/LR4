using LR4.Interfaces;
using LR4.Models;
using System;
using System.IO;
using System.Linq;

namespace LR4.Services
{
    public class UserService : IUserService
    {
        private const string UserFilePath = "user.txt";
        private const string VerifiedFilePath = "verified.txt";
        private const string ActiveFilePath = "active.txt";

        public bool RegisterUser(User user)
        {
            if (!ValidateUser(user))
                return false;

            try
            {
                File.AppendAllText(UserFilePath, user.ToString() + Environment.NewLine);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool ValidateUser(User user)
        {
            return !string.IsNullOrWhiteSpace(user.Name) &&
                   !string.IsNullOrWhiteSpace(user.Surname) &&
                   !string.IsNullOrWhiteSpace(user.Passport) &&
                   !string.IsNullOrWhiteSpace(user.Password);
        }

        public bool AuthenticateAdmin(string passport, string password)
        {
            return passport == "admin" && password == "1234";
        }

        public bool AuthenticateUser(string passport, string password)
        {
            if (!File.Exists(VerifiedFilePath))
                return false;

            return File.ReadLines(VerifiedFilePath)
                .Any(line => {
                    var parts = line.Split(';');
                    return parts.Length >= 4 && 
                           parts[2] == passport && 
                           parts[3] == password;
                });
        }

        public void ActivateUser(string passport)
        {
            try
            {
                if (!File.Exists(VerifiedFilePath))
                    throw new FileNotFoundException("Файл підтверджених користувачів не знайдено");

                var userLine = File.ReadLines(VerifiedFilePath)
                    .FirstOrDefault(line => line.Contains(passport));

                if (userLine == null)
                    throw new Exception("Користувач не знайдений у підтверджених");

                File.AppendAllText(ActiveFilePath, userLine + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}
