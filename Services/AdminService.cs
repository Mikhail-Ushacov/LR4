using LR4.Interfaces;
using System.IO;
using System.Linq;

using System;

namespace LR4.Services
{
    public class AdminService : IAdminService
    {
        private const string UserFilePath = "user.txt";
        private const string VerifiedFilePath = "verified.txt";

        public string[] GetPendingRegistrations()
        {
            if (!File.Exists(UserFilePath))
                return new string[0];

            return File.ReadAllLines(UserFilePath)
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .ToArray();
        }

        public void ApproveRegistration(string userData)
        {
            if (!File.Exists(VerifiedFilePath))
                File.WriteAllText(VerifiedFilePath, userData + Environment.NewLine);
            else
                File.AppendAllText(VerifiedFilePath, userData + Environment.NewLine);

            RemoveFromPending(userData);
        }

        public void RejectRegistration(string userData)
        {
            RemoveFromPending(userData);
        }

        private void RemoveFromPending(string userData)
        {
            var allLines = File.ReadAllLines(UserFilePath)
                .Where(line => line != userData)
                .ToArray();
            File.WriteAllLines(UserFilePath, allLines);
        }
    }
}
