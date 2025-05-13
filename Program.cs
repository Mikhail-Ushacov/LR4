using System;
using System.Windows.Forms;
using LR4.Interfaces;
using LR4.Services;

namespace LR4
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Initialize required services
            var votingService = new VotingService();
            var voteService = new VoteService();

            // Start with main voting form (Form4)
            Application.Run(new Form4(votingService, voteService));
        }
    }
}
