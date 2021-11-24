using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace Pathfinder
{
    class Dijkstra
    {
        double[] dist;
        Level gamelevel;
        ArrayList S; // visited vertices
        ArrayList V;

        int[] parents;
        ArrayList Path; // Final Path will be stored in this ArrayList

        public string[] Path2Grid;
        public Dijkstra(Level level)
        {
            gamelevel = level;
            S = new ArrayList();
            V = new ArrayList();
            Path = new ArrayList();
        }
        public void Build(double[,] graph, AiBotBase bot, Player plr)
        {
            int nVertices = graph.GetLength(0);

            dist = new double[nVertices];
            parents = new int[nVertices];

            // initialize distance of each vertices with high number
            for (int i = 0; i < graph.GetLength(0); i++)
            {
                dist[i] = int.MaxValue;
            }
            // initial start vertex distance as 0
            int starting_vertex = (bot.GridPosition.Y) * gamelevel.tiles.GetLength(0) + bot.GridPosition.X;
            dist[starting_vertex] = 0;
            parents[starting_vertex] = -1;

            for (int i = 0; i < graph.GetLength(0); i++)
                V.Add(i);

            while (V.Count > 0)
            {
                int V_min = getMinimum();
                S.Add(V_min);

                //Adj Vertices
                for (int i = 0; i < graph.GetLength(0); i++)
                    if (graph[V_min, i] >= 1) // Remember 1 or 1.4 
                    {
                        if (dist[i] > graph[V_min, i] + dist[V_min])
                        {
                            // Store the Parent node - which makes the distance shorter.
                            parents[i] = V_min;
                            dist[i] = graph[V_min, i] + dist[V_min];
                        }
                    }
                //remove it from V
                V.Remove(V_min);
            }

            //target vertex - not used by Dijkstra - only used to generate the path form source to destination
            int targetVertex = (plr.GridPosition.Y) * gamelevel.tiles.GetLength(0) + plr.GridPosition.X;

            //generate the path from source to distination
            calculatePath(starting_vertex, dist, parents, targetVertex);
        }

        private int getMinimum()
        {
            double min = int.MaxValue;
            int V_min = -1;
            foreach (int v in V)
            {
                if (dist[v] <= min)
                {
                    min = dist[v];
                    V_min = v;
                }
            }
            return V_min;
        }

        private void calculatePath(int startVertex, double[] distances, int[] parents, int targetVertex)
        {
            generatePath(targetVertex, parents);
            Path2Grid = new string[Path.Count];
            int i = 0;

            foreach (int v in Path)
            {
                int Vx = (v % gamelevel.tiles.GetLength(0));
                int Vy = (v / gamelevel.tiles.GetLength(0));

                Path2Grid[i] = Vx + "," + Vy;
                i++;
            }
        }

        private void generatePath(int currentVertex, int[] parents)
        {

            // Base case : Starting Vertex Parent will be -1 
            if (currentVertex == -1)
            {
                return;
            }

            //Start from target vertex and back tracks the path to the starting vertex 
            generatePath(parents[currentVertex], parents);
            Path.Add(currentVertex);
        }
    }
}
