using System;
using System.Globalization;

class SalesmanAnnealing
{

    private static Random rand = new Random();
    private const double Tmin = 1e-10;
    private const double Tstart = 100.0;
    private const double alpha = 0.995;
    private const int MaxIterations = 100000;

    public static int[] SimulatedAnnealing(double[,] matrix)
    {
        int[] route = GenerateRandomRoute(matrix.GetLength(0));
        double T = Tstart;
        double cost = CalculateCost(route, matrix);
        int[] bestRoute = route.Clone() as int[];
        double bestCost = cost;

        for (int i = 0; i < MaxIterations && T > Tmin; i++)
        {
            int[] newRoute = MutateRoute(route);
            double newCost = CalculateCost(newRoute, matrix);

            if (newCost < cost || Math.Exp(-(newCost - cost) / T) > rand.NextDouble())
            {
                route = newRoute;
                cost = newCost;
            }

            if (cost < bestCost)
            {
                bestRoute = route.Clone() as int[];
                bestCost = cost;
            }

            T *= alpha;
        }

        return bestRoute;
    }

    private static double CalculateCost(int[] route, double[,] matrix)
    {
        var res = 0d;
        for (var i = 0; i < route.Length - 1; i++)
        {
            res += matrix[route[i], route[i + 1]];
        }

        return res;
    }

    private static int[] GenerateRandomRoute(int length)
    {
        int[] route = new int[length];
        for (int i = 0; i < route.Length; i++)
        {
            route[i] = i;
        }
        for (int i = 0; i < length; i++)
        {
            var index1 = rand.Next(i, length);
            var tmp = route[index1];
            route[index1] = route[i];
            route[i] = tmp;

        }
        return route;
    }

    private static int[] MutateRoute(int[] route)
    {
        int[] newRoute = route.Clone() as int[];

        int i1, i2;
        do
        {
            i1 = (rand.Next(route.Length));
            i2 = (rand.Next(route.Length));
        } while (i1 == i2);

        var tmp = newRoute[i1];
        newRoute[i1] = newRoute[i2];
        newRoute[i2] = tmp;
        return newRoute;
    }
}