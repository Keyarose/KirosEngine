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
            if ((args.Length == 0) || (args.Length == 1 && args[0].Equals("dx")))
            {
                //Scrapped for now
                Client client;
                
                client = new Client("Kiros Client");
                client.Initialize();
                client.Load();
                client.Run();
                client.Unload();

                //relaunch with argument when starting a game
                //ProcessStartInfo pStartInfo = new ProcessStartInfo();
                //pStartInfo.Arguments = string.Join(" ", args); //replace args with the directory for the chosen game and the graphics mode used
                //pStartInfo.FileName = Application.ExecutablePath;
                //Process.Start(pStartInfo);
                //Application.Exit();

                //Console.WriteLine("No directory specified for the game data.");
            }
            else if(args.Length == 1 && args[0].Equals("gl"))
            {
                //TODO: start in library mode with opengl
            }
            else if (args.Length == 2 && args[0].Equals("dx"))
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