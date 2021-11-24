using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class AStar
    {
        private readonly Level gamelevel;


        private Coord2 start;
        private Coord2 target;
        private readonly List<Tile> openList;
        private readonly List<Tile> closedList;
        private readonly List<Coord2> Path;

        public List<string> Path2Grid;
        public List<string> PathSearched;

        //Constructor for the A* class
        public AStar(Level level)
        {
            gamelevel = level;
            openList = new List<Tile>();
            closedList = new List<Tile>();
            Path = new List<Coord2>();
        }

        //Finds the Heuristics by using Diagonal Distance
        private double GetH(Coord2 currentLocation)
        {
            int x = Math.Abs(currentLocation.X - target.X);
            int y = Math.Abs(currentLocation.Y - target.Y);
            double H = 1 * (x + y) + (1.414 - 2 * 1) * Math.Min(x, y);
            return H;
        }

        //Takes the current positon of the search and returns all coords of positons of valid grid spaces
        private List<Coord2> GetValidAdjacentTiles(Coord2 current)
        {
            //Creates a list for valid and invalid tile possitions
            List<Coord2> adjacentTiles = new List<Coord2>();
            List<Coord2> invalidTiles = new List<Coord2>();

            //Fills the list with possible valid tiles
            adjacentTiles.Add(new Coord2(current.X - 1, current.Y + 1));
            adjacentTiles.Add(new Coord2(current.X, current.Y + 1));
            adjacentTiles.Add(new Coord2(current.X + 1, current.Y + 1));
            adjacentTiles.Add(new Coord2(current.X - 1, current.Y));
            adjacentTiles.Add(new Coord2(current.X + 1, current.Y));
            adjacentTiles.Add(new Coord2(current.X - 1, current.Y - 1));
            adjacentTiles.Add(new Coord2(current.X, current.Y - 1));
            adjacentTiles.Add(new Coord2(current.X + 1, current.Y - 1));

            //Finds all invalid tiles
            foreach (Coord2 tile in adjacentTiles)
            {
                if (!gamelevel.ValidPosition(tile))
                {
                    invalidTiles.Add(tile);
                    continue;
                }

                if (closedList.FirstOrDefault(index => index.Position == tile) != null)
                {
                    invalidTiles.Add(tile);
                    continue;
                }
            }

            //Removes all invalid tiles
            for (int index = 0; index < invalidTiles.Count(); index++)
            {
                adjacentTiles.Remove(invalidTiles[index]);
            }
            return adjacentTiles;
        }

        //Creates or changes Tiles with the designated coords
        private void MakeValidTiles(List<Coord2> adjecentCoords, double[,] graph, Tile current)
        {
            foreach (Coord2 tile in adjecentCoords)
            {
                //Creates new G value from current tiles information
                double G = current.G + graph[current.Position.X + current.Position.Y * 40, tile.X + tile.Y * 40];

                //Creates a new tile
                Tile newTile = new Tile(tile, G, GetH(tile));

                //If valid tile is not in the open list, add it to open list
                if (openList.FirstOrDefault(index => index.Position == tile) == null)
                {
                    newTile.lastTile = current;
                    openList.Insert(0, newTile);
                }
                //If the the new F would be smaller than the old Tile, update the tile
                else if (newTile.F < openList.First(index => index.Position == tile).F)
                {
                    openList.First(index => index.Position == tile).G = newTile.G;
                    openList.First(index => index.Position == tile).CaculateF();
                    openList.First(index => index.Position == tile).lastTile = current;
                }
            }
        }

        //Fills the Path2Grid, PathSearched and the Path lists
        private void ReturnPath(Tile current)
        {
            Path2Grid = new List<string>();
            PathSearched = new List<string>();
            while (current.lastTile != null)
            {
                Path.Insert(0, current.Position);
                Path2Grid.Insert(0, current.Position.X + "," + current.Position.Y);
                current = current.lastTile;
            }

            foreach (Tile tile in closedList)
            {
                if (!Path2Grid.Contains(current.Position.X + "," + current.Position.Y))
                {
                    PathSearched.Add(tile.Position.X + "|" + tile.Position.Y);
                }
            }
        }

        //Method only to send back the preduced path
        public List<Coord2> GetPath()
        {
            return Path;
        }

        //Builds path to the target useing A* algrithem 
        public void Build(double[,] graph, AiBotBase bot, Player player)
        {
            //Sets starting values to track where to current search
            double lowest;
            Tile current = null;
            List<Coord2> adjecentCoords;

            //Positions for the starting point and the target point
            start = bot.GridPosition;
            target = player.GridPosition;

            //Adds the starting node to the open list
            openList.Add(new Tile(start, 0, GetH(start)));

            //While openList is not empty, run search
            while (openList.Count != 0)
            {
                //Finds current lowest F value in all open tiles and sets it to current tile
                lowest = openList.Min(low => low.F);
                current = openList.First(low => low.F == lowest);

                //Adds current tile to the closed tiles list to indicated its now been checked and removes it from open list
                closedList.Add(current);
                openList.Remove(current);

                //Break if the path has been found
                if (closedList.FirstOrDefault(index => index.Position == target) != null)
                {
                    ReturnPath(current);
                    break;
                }

                //Finds all valid adjecent coords and makes those coords to turn them to vaild tiles
                adjecentCoords = GetValidAdjacentTiles(current.Position);
                MakeValidTiles(adjecentCoords, graph, current);
            }
        }
    }

    //Tile class to track searching
    class Tile
    {
        public Coord2 Position;
        public double F;
        public double H;
        public double G;
        public Tile lastTile = null;

        public Tile(Coord2 position, double G, double H)
        {
            this.Position = position;
            this.G = G;
            this.H = H;
            CaculateF();
        }

        public void CaculateF()
        {
            F = G + H;
        }
    }
}
