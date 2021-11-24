using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pathfinder
{
    class AiBotSimple2 : AiBotBase
    {
        bool[,] visited;
        public AiBotSimple2(int x, int y) : base(x, y)
        {
            /*
             * visited to store visited locations
            */
            visited = new bool[40, 40];
            // Initialize the visited
            for (int i = 0; i < 40; i++)
                for (int j = 0; j < 40; j++)
                    visited[i,j] = false;
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {            
            /* 
             * Bot Poistion *
              GridPosition.X; 
              GridPosition.Y;
              
             * Player Position *
             * plr.GridPosition.X
             * plr.GridPosition.X
            */
            
             bool ok = false;
             Coord2 pos = new Coord2();
             while (!ok)
             {
                // if Bot and Target are at same position - goal reached!
                if (GridPosition.X == plr.GridPosition.X && GridPosition.Y == plr.GridPosition.Y)
                    break;

                double distance = 1000;
                 double e_distance;
                 Coord2 nextMove = new Coord2(); 

                // Bot can move in four directions Up, Down, Left and Right
                // Calculate which move makes the bot closer to target

                // ** Right **
                 pos.X = GridPosition.X + 1;
                 pos.Y = GridPosition.Y;

                 // euclidean distance Sqrt((X2-X1)*(X2-X1)+(Y2-Y1)*(Y2-Y1)) 
                 // 
                 e_distance = Math.Sqrt(Convert.ToDouble(((plr.GridPosition.X - pos.X) * (plr.GridPosition.X - pos.X))
                     + ((plr.GridPosition.Y - pos.Y) * (plr.GridPosition.Y - pos.Y))));
                 if (e_distance < distance && level.ValidPosition(pos) && visited[pos.X, pos.Y]==false)
                 {
                     distance = e_distance;
                     nextMove = pos;
                 }

                 // ** Left **
                 pos.X = GridPosition.X - 1;
                 pos.Y = GridPosition.Y;

                 e_distance = Math.Sqrt(Convert.ToDouble(((plr.GridPosition.X - pos.X) * (plr.GridPosition.X - pos.X))
                     + ((plr.GridPosition.Y - pos.Y) * (plr.GridPosition.Y - pos.Y))));
                 if (e_distance < distance && level.ValidPosition(pos) && visited[pos.X, pos.Y] == false)
                 {
                     distance = e_distance;
                     nextMove = pos;
                 }

                 // ** UP **
                 pos.X = GridPosition.X;
                 pos.Y = GridPosition.Y + 1;

                 e_distance = Math.Sqrt(Convert.ToDouble(((plr.GridPosition.X - pos.X) * (plr.GridPosition.X - pos.X))
                     + ((plr.GridPosition.Y - pos.Y) * (plr.GridPosition.Y - pos.Y))));
                 if (e_distance < distance && level.ValidPosition(pos) && visited[pos.X, pos.Y] == false)
                 {
                     distance = e_distance;
                     nextMove = pos;
                 }

                 // ** Down **
                 pos.X = GridPosition.X;
                 pos.Y = GridPosition.Y - 1;

                 e_distance = Math.Sqrt(Convert.ToDouble(((plr.GridPosition.X - pos.X) * (plr.GridPosition.X - pos.X))
                     + ((plr.GridPosition.Y - pos.Y) * (plr.GridPosition.Y - pos.Y))));
                 if (e_distance < distance && level.ValidPosition(pos) && visited[pos.X, pos.Y] == false)
                 {
                     distance = e_distance;
                     nextMove = pos;
                 }

                ok = SetNextGridPosition(nextMove, level);

                visited[nextMove.X, nextMove.Y] = true;
            }
        }
    }
}
