namespace Algorytn_MinMax
{
    internal class Program
    {


        static void Main(string[] args)
        {
            var Gra = new TicTacToe(3);
            var RuchGrcza = true;
            do
            {
                Gra.Board();
               
                if (RuchGrcza == false)
                {
                   var ruchX= Gra.MinMax(Gra.arr, 9, 'X') ;
                    Gra.arr = ruchX.plansza;
                }
                else
                {
                    Console.WriteLine($"Podaj numer wiersza:");
                    var wiersz = Console.ReadLine();
                    Console.WriteLine($"Podaj numer kolumny:");
                    var kolumna = Console.ReadLine();
                    if (Gra.CzyMozliwyRuch(int.Parse(wiersz), int.Parse(kolumna),Gra.arr))
                    {
                        Gra.Ruch(int.Parse(wiersz), int.Parse(kolumna), 'O');
                    }
                    else
                    {
                        Console.WriteLine($"Wybierz inne pole");
                        continue;
                    }
                }
                RuchGrcza=!RuchGrcza; 

            } while(Gra.CzyToKoniecGry(Gra.arr)==false);
            Gra.WyswietlJezeliKoniecGry();
        }
    }
}