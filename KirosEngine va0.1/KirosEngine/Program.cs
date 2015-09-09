using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using SlimDX;
using SlimDX.Windows;

[assembly: AssemblyVersionAttribute("0.0.0.1")]
namespace KirosEngine
{
    class Program
    {
        //should eventualy receive a file path to the game data folder
        /// <summary>
        /// Main entry function for the program
        /// </summary>
        /// <param name="args">First parameter must be "dx" or "gl" to specify directx or opengl respectivly. The second parameter should be the path to the 
        /// data for a game. No parameters defaults to library mode with directX</param>
        [STAThread]
        static void Main(string[] args)
        {
            if (args.Length == 2 && args[0].Equals("dx"))
            {
                Client client;

                client = new Client("Kiros Client");
                client.Initialize();
                try
                {
                    client.Load(args[1]);
                }
                catch(FileNotFoundException ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine("Unable to continue, now exiting.");
                    return;
                }
                client.Run();
                client.Unload();
            }
            else if(args.Length == 2 && args[0].Equals("gl"))
            {
                //TODO: start in game mode with opengl
            }
        }
    }
}