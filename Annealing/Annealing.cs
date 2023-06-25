using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Annealing
{
    internal class Annealing
    {

        private static Random rand = new Random(); // obiekt do generowania liczb losowych
        private const double Tmin = 1e-10; // minimalna wartość temperatury
        private const double Tstart = 100.0; // początkowa wartość temperatury
        private const double alpha = 0.995; // współczynnik zmniejszania temperatury
        private const int MaxIterations = 100000; // maksymalna liczba iteracji

        // Funkcja, której minimum chcemy znaleźć. Można ją zmienić na inną.
        private static double Function(double x)
        {
            return x * x - 4 * x + 5;
        }

        public static double SimulatedAnnealing()
        {
            double w = rand.NextDouble() * 10; // punkt startowy
            double T = Tstart; // temperatura początkowa
            double min = Function(w); // wartość funkcji w punkcie startowym
            double bestW = w; // najlepszy punkt
            double bestMin = min; // najlepsza wartość funkcji

            for (int i = 0; i < MaxIterations && T > Tmin; i++)
            {
                double deltaW = rand.NextDouble() * T; // losowa zmiana punktu
                double w2 = w + deltaW;
                double min2 = Function(w2); // wartość funkcji w nowym punkcie

                if (min2 < min || Math.Exp(-(min2 - min) / T) > rand.NextDouble())
                {
                    w = w2;
                    min = min2;
                }

                if (min < bestMin)
                {
                    bestW = w;
                    bestMin = min;
                }

                T *= alpha; // zmniejszenie temperatury
            }

            return bestW;
        }

    }
}
