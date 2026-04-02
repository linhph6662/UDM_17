using System;
using System.Windows.Forms;

namespace CaroGame
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new FormLobby());
        }
    }
}