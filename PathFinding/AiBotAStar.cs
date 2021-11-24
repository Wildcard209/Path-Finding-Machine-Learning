using Pathfinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PathFinding
{
    class AiBotAStar : AiBotBase
    {
        public List<string> DrawPath = new List<string>();
        public List<string> DrawSearch = new List<string>();
        private List<Coord2> Path = new List<Coord2>();
        private int index = 0;
        public AiBotAStar(int x, int y) : base(x, y)
        {

        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            if (Path.Count == 0)
            {
                Graph graph = new Graph(level);
                double[,] graph_matrix = graph.GenerateGraph();

                AStar aStar = new AStar(level);
                aStar.Build(graph_matrix, this, plr);

                Path = aStar.GetPath();

                DrawPath = aStar.Path2Grid;
                DrawSearch = aStar.PathSearched;
            }

            if (index < Path.Count)
            {
                Coord2 tile = Path[index];
                index++;
                SetNextGridPosition(tile, level);
            } 
        }
    }
}
