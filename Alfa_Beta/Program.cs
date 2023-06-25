using System.Reflection;

namespace Algorytm_Alfa_Beta
{
  public class Program
    {
        

        public static void Main(String[] args)
        {   

           double[] values = { 10, 5, 7, 11, 12, 8, 9, 8, 5, 12, 11, 12, 9, 8, 7,10 };
            
            Console.WriteLine("Optymalna wartosc to : " + Alfa_Beta.minimax(0, 0, true, values, Alfa_Beta.MIN, Alfa_Beta.MAX));

        }
    }
}