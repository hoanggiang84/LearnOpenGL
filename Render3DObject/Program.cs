using Render3DObject.Components;
using System;

namespace Render3DObject
{
    class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            new MainWindow().Run(60);
        }
    }
}
