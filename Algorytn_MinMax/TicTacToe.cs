using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Algorytn_MinMax
{
    public class TicTacToe
    {
        public char[,] arr;
        public char playerX = 'X';
        public char playerO = 'O';

        public TicTacToe(int rozmiar)
        {
            arr = new char[rozmiar, rozmiar];
        }

        public bool CzyMozliwyRuch(int x, int y, char[,] arr)
        {
            if (arr[x, y] == 0) return true;

            return false;
        }

        public bool CzyToKoniecGry(char[,] arr)
        {


            if (KtoWygral(arr) != null) return true;

            for (var i = 0; i < arr.GetLength(0); i++)
            {
                for (var j = 0; j < arr.GetLength(1); j++)
                {
                    if (arr[i, j] == 0)
                        return false;
                }
            }


            return true;

        }

        public bool SprawdzCzyJestWygrany(char gracz, char[,] arr)
        {
            if ((arr[0, 0] == gracz && arr[0, 1] == gracz && arr[0, 2] == gracz) || (arr[1, 0] == gracz && arr[1, 1] == gracz && arr[1, 2] == gracz) ||
                (arr[2, 0] == gracz && arr[2, 1] == gracz && arr[2, 2] == gracz) || (arr[0, 0] == gracz && arr[1, 0] == gracz && arr[2, 0] == gracz) ||
                (arr[0, 1] == gracz && arr[1, 1] == gracz && arr[2, 1] == gracz) || (arr[0, 2] == gracz && arr[1, 2] == gracz && arr[2, 2] == gracz) ||
                (arr[0, 0] == gracz && arr[1, 1] == gracz && arr[2, 2] == gracz) || (arr[0, 2] == gracz && arr[1, 1] == gracz && arr[2, 0] == gracz))
                return true;
            return false;
        }

        public char? KtoWygral(char[,] arr)
        {
            char? win = null;
            if (SprawdzCzyJestWygrany(playerO, arr) == true)
                win = playerO;
            if (SprawdzCzyJestWygrany(playerX, arr) == true)
                win = playerX;

            return win;
        }
        public void Ruch(int x, int y, char gracz)
        {

            if (CzyMozliwyRuch(x, y, arr) == true)
            {
                arr[x, y] = gracz;

            }
          
        }
        public void WyswietlJezeliKoniecGry()
        {
            if (CzyToKoniecGry(arr))
            {
                Board();
                var wygrany = KtoWygral(arr);
                if (wygrany == null)
                {
                    Console.WriteLine("Koniec gry! Remis.");
                    return;
                }
                Console.WriteLine($"Koniec gry! Wygrał: {wygrany}");
            }
        }
        public void Board()
        {
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}     {1}    {2}", arr[0, 0], arr[0, 1], arr[0, 2]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}     {1}    {2}", arr[1, 0], arr[1, 1], arr[1, 2]);
            Console.WriteLine("_____|_____|_____ ");
            Console.WriteLine("     |     |      ");
            Console.WriteLine("  {0}     {1}    {2}", arr[2, 0], arr[2, 1], arr[2, 2]);
            Console.WriteLine("     |     |      ");
            Console.WriteLine("====================== ");

            //Console.WriteLine($"{arr[0, 0]} | {arr[0, 1]} | {arr[0, 2]}");
            //Console.WriteLine($"{arr[1, 0]} | {arr[1, 1]} | {arr[1, 2]}");
            //Console.WriteLine($"{arr[2, 0]} | {arr[2, 1]} | {arr[2, 2]}");
        }

        public double PoliczUlozenia(char[,] stan, char gracz)
        {
            var licznik = 0;
            if (stan[0, 0] == gracz && stan[0, 1] == gracz && stan[0, 2] == gracz) licznik++;
            if (stan[1, 0] == gracz && stan[1, 1] == gracz && stan[1, 2] == gracz) licznik++;
            if (stan[2, 0] == gracz && stan[2, 1] == gracz && stan[2, 2] == gracz) licznik++;
            if (stan[0, 0] == gracz && stan[1, 0] == gracz && stan[2, 0] == gracz) licznik++;
            if (stan[0, 1] == gracz && stan[1, 1] == gracz && stan[2, 1] == gracz) licznik++;
            if (stan[0, 2] == gracz && stan[1, 2] == gracz && stan[2, 2] == gracz) licznik++;
            if (stan[0, 0] == gracz && stan[1, 1] == gracz && stan[2, 2] == gracz) licznik++;
            if (stan[0, 2] == gracz && stan[1, 1] == gracz && stan[2, 0] == gracz) licznik++;

            return licznik;
        }

        public List<char[,]> MozliweUlozenia(char[,] rodzic, char gracz)
        {
            var arr = new List<char[,]>();
            for (var i = 0; i < rodzic.GetLength(0); i++)
            {
                for (var j = 0; j < rodzic.GetLength(1); j++)
                {
                    if (rodzic[i, j] == 0)
                    {
                        var kopiaRodzica = (char[,])rodzic.Clone();
                        kopiaRodzica[i, j] = gracz;
                        arr.Add(kopiaRodzica);
                    }
                }
            }
            return arr;
        }
        public double FunkcjaHeurystyczna(char[,] stan)
        {
            var wygranaO = PoliczUlozenia(stan, playerO);
            var wygranaX = PoliczUlozenia(stan, playerX);
            var heurystyka = wygranaX - wygranaO;

            return heurystyka;
        }
        public (double ocena, char[,] plansza) MinMax(char[,] plansza, int glebokosc, char gracz)
        {
            if (glebokosc == 0 || CzyToKoniecGry(plansza) == true)
            { return (FunkcjaHeurystyczna(plansza), null); }
            if (gracz == playerX)
            {
                char[,] wynik = null;
                var ocena = int.MinValue;
                foreach (var przyszlyStan in MozliweUlozenia(plansza, gracz))
                {
                    var nowaOcena = MinMax(przyszlyStan, glebokosc - 1, playerO);

                    wynik = nowaOcena.ocena > ocena ? przyszlyStan : wynik;
                    ocena = nowaOcena.ocena > ocena ? (int)nowaOcena.ocena : ocena;
                }
                return (ocena, wynik);

            }
            else
            {
                char[,] wynik = null;
                var ocena = int.MaxValue;
                foreach (var przyszlyStan in MozliweUlozenia(plansza, gracz))
                {
                    var nowaOcena = MinMax(przyszlyStan, glebokosc - 1, playerX);

                    wynik = nowaOcena.ocena < ocena ? przyszlyStan : wynik;
                    ocena = nowaOcena.ocena < ocena ? (int)nowaOcena.ocena : ocena;

                }
                return (ocena, wynik);
            }

        }
    }
}
