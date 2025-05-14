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

            var votingService = new VotingService();
            var voteService = new VoteService();
            var resultsService = new VotingResultsService();

            Application.Run(new Form4(votingService, voteService, resultsService));
        }
    }
}
