using System;
using System.Collections.Generic;
using SwinGameSDK;
using System.IO;
using RobotNavigationAssignment;

namespace MyGame.src
{
    public class RobotNavigation
    {
        public static void Main()
        {
            //Open the game window
            SwinGame.OpenGraphicsWindow("RobotNavigation GUI - 102578350", 800, 380);

            //Generate maze from file
            MazeGenerator mazeGenerator = new MazeGenerator();
            StreamReader rdr = new StreamReader("mapconfig.txt");
            Maze maze = mazeGenerator.GetMaze(rdr);
            rdr.Close();

            //Initialize MazeDrawer Object
            MazeHandler MazeHandler = new MazeHandler(maze);

            //timer variables
            int timer = 500;

            //Run the game loop
            while (false == SwinGame.WindowCloseRequested())
            {
                //Fetch the next batch of UI interaction
                SwinGame.ProcessEvents();
                
                //Clear the screen and draw the framerate
                SwinGame.ClearScreen(Color.White);

                if (SwinGame.MouseClicked(MouseButton.LeftButton))
                {
                    MazeHandler.CheckAction();
                }

                if(SwinGame.AnyKeyPressed())
                {
                    MazeHandler.CheckOption();
                }

                if(SwinGame.GetTicks() > timer)
                {
                    MazeHandler.Step();
                    timer += 500;
                }
                

                MazeHandler.Draw();

                //Draw onto the screen
                SwinGame.RefreshScreen(60);
            }
        }
    }
}