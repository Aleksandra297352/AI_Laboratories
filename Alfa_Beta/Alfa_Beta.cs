using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorytm_Alfa_Beta
{
    public static class Alfa_Beta
    {

       public static double MAX = double.PositiveInfinity;
       public static double MIN = double.NegativeInfinity;

        public static double minimax(int depth, int nodeIndex, Boolean maximizingPlayer,double[] values, double alpha, double beta)
        {
            if (depth == 4)
                return values[nodeIndex];

            if (maximizingPlayer)
            {
                double best = MIN;

                for (int i = 0; i < 2; i++)
                {
                    double val = minimax(depth + 1, nodeIndex * 2 + i, false, values, alpha, beta);
                    ColorConsole(depth);
                    Console.Write($"{Tabulations(depth)}Głębokość: {depth}, Wartość Max({best.ToStringEx()},{val.ToStringEx()}) = ");
                    best = Math.Max(best, val);
                    Console.Write($"{best}, ");
                    alpha = Math.Max(alpha, best);
                    Console.Write($"Alfa: {alpha.ToStringEx()}, beta={beta.ToStringEx()}\n");
                    ResetColor();

                    if (beta <= alpha)
                        break;
                }
                return best;
            }
            else
            {
                double best = MAX;


                for (int i = 0; i < 2; i++)
                {

                    double val = minimax(depth + 1, nodeIndex * 2 + i, true, values, alpha, beta);
                    ColorConsole(depth);
                    Console.Write($"{Tabulations(depth)}Głębokość: {depth}, Wartość Min({best.ToStringEx()},{val.ToStringEx()}) = ");
                    best = Math.Min(best, val);
                    Console.Write($"{best}, ");
                    beta = Math.Min(beta, best);
                    Console.Write($"Alfa: {alpha.ToStringEx()}, beta={beta.ToStringEx()}\n");
                    ResetColor();

                    if (beta <= alpha)
                        break;
                }
                return best;
            }
        }

        private static void ResetColor()
        {
            Console.ForegroundColor = ConsoleColor.White;
        }

        private static void ColorConsole(int depth)
        {
            switch (depth)
            {
                default: Console.ForegroundColor = ConsoleColor.White; break;
                case 0:
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case 1:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                case 2:
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case 3:
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
            }
            
        }

        static string Tabulations(int depth)
        {
            var s = "";

            for (var i = 0; i < depth; i++)
            {
                s += " ";
            }

            return s;
        }
        public static string ToStringEx(this double e)
        {
            if (e == double.PositiveInfinity) return "inf";
            if (e == double.NegativeInfinity) return "-inf";
            return e.ToString();
        }
    }
}
