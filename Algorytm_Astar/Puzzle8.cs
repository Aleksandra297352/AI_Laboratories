using System.Collections.Generic;
using System;
using System.Linq;
namespace Algorytm_Astar
{
    //public class DuplicateKeyComparer<TKey>
    //               :
    //            IComparer<TKey> where TKey : IComparable
    //{
    //    #region IComparer<TKey> Members

    //    public int Compare(TKey x, TKey y)
    //    {
    //        int result = x.CompareTo(y);

    //        if (result == 0)
    //            return 1;   // Handle equality as beeing greater
    //        else
    //            return result;
    //    }

    //    #endregion
    //}
    public class PuzzleBoard
    {
        //który to stan z kolei (czyli ile bylo poprzedzajacych stanow)
        public int Glebokosc { get; private set; }
        //wynik FunkcjiF (czyli koszt dojscia do obecnego stanu)
        public double Koszt { get; set; }
        public double[,] Plansza { get; private set; }

        public PuzzleBoard(double[,] plansza, int glebokosc, double koszt)
        {
            Glebokosc = glebokosc;
            Plansza = plansza;
            Koszt = koszt;
        }

        //zwraca wszystkie mozliwe ustawienia planszy, ktore moga nastapic po przestawieniu pustego pola
        public List<PuzzleBoard> ZwrocKolejnePlansze()
        {
            var plansze = new List<PuzzleBoard>();

            int pustePoleX = -1, pustePoleY = -1;

            for (var i = 0; i < Plansza.GetLength(0); i++)
            {
                for (var j = 0; j < Plansza.GetLength(1); j++)
                {
                    if (Plansza[i, j] == 0)
                    {
                        pustePoleX = i;
                        pustePoleY = j;
                        break;
                    }
                }
                if (pustePoleX != -1 && pustePoleY != -1) break;
            }

            //nowe pozycje pustego pola powstale w wyniku przesuniecia go w kazda strone tj. gora, dol, lewo, prawo
            var nowePozycje = new (int x, int y)[] {
                (pustePoleX, pustePoleY + 1),
                (pustePoleX, pustePoleY - 1),
                (pustePoleX - 1, pustePoleY),
                (pustePoleX + 1, pustePoleY)
            };

            //tworzenie nowych plansz poprzez przesuwanie pustego pola
            foreach (var pozycja in nowePozycje)
            {
                //plansza powstala po przestawieniu pustego pola
                var nowaPlansza = ZwrocNowaPlanszeZPrzestawionymPustymPolem(Plansza, pustePoleX, pustePoleY, pozycja.x, pozycja.y);
                if (nowaPlansza == null) continue;
                plansze.Add(new PuzzleBoard(nowaPlansza, Glebokosc + 1, Koszt));
            }

            return plansze;
        }

        double[,] ZwrocNowaPlanszeZPrzestawionymPustymPolem(double[,] stan, int pustePoleX, int pustePoleY, int nowePoleX, int nowePoleY)
        {

            if (nowePoleX >= 0 && nowePoleX < stan.GetLength(0) && nowePoleY >= 0 && nowePoleY < stan.GetLength(1))
            {
                //kopia obecnego stanu, zeby moc go edytowac
                var nowyStan = (double[,])stan.Clone();

                var tmp = nowyStan[nowePoleX, nowePoleY];
                nowyStan[pustePoleX, pustePoleY] = tmp;
                nowyStan[nowePoleX, nowePoleY] = 0;
                return nowyStan;
            }

            //jezeli nowa pozycja poza ramami tablicy, to nie przestawiamy
            return null;
        }
        public static bool operator ==(PuzzleBoard puzzleBoard1, PuzzleBoard puzzleBoard2)
        {
            for (var i = 0; i < puzzleBoard1.Plansza.GetLength(0); i++)
            {
                for (var j = 0; j < puzzleBoard1.Plansza.GetLength(1); j++)
                {
                    if (puzzleBoard1.Plansza[i, j] != puzzleBoard2.Plansza[i, j]) return false;
                }
            }
            return true;
        }
        public static bool operator !=(PuzzleBoard puzzleBoard1, PuzzleBoard puzzleBoard2) => !(puzzleBoard1 == puzzleBoard2);

    }
    public class Puzzle8
    {
        //liczba zle ulozonych plytek
        int FunctionH(double[,] currentState, double[,] goalState)
        {
            var liczbaZleUlozonychPlytek = 0;
            for (var i = 0; i < currentState.GetLength(0); i++)
            {
                for (var j = 0; j < currentState.GetLength(1); j++)
                {
                    if (currentState[i, j] != goalState[i, j]) liczbaZleUlozonychPlytek++;
                }
            }
            return liczbaZleUlozonychPlytek;
        }
        double FunctionF(PuzzleBoard currentState, double[,] goalState)
        {
            return FunctionH(currentState.Plansza, goalState) + currentState.Glebokosc;
        }
        void Print(double[,] state)
        {
            Console.WriteLine();
            for (var i = 0; i < state.GetLength(0); i++)
            {
                for (var j = 0; j < state.GetLength(1); j++)
                {
                    if (state[i, j] == 0) Console.Write("_ ");
                    else Console.Write(state[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        (PuzzleBoard last, List<PuzzleBoard> all)? GetFirstNotInClosed(SortedList<double, (PuzzleBoard last, List<PuzzleBoard> all)> queue, List<PuzzleBoard> closed)
        {
            while (queue.Count > 0)
            {
                var element = queue.First().Value;
                queue.RemoveAt(0);
                //zwracamy tylko jezeli planszy nie ma na liscie juz odwiedzonych
                if (closed.Any(x => x == element.last) == false)
                {
                    return element;
                }
            }
            return null;
        }
        public List<PuzzleBoard> SolvePuzzle(double[,] startowyStan, double[,] docelowyStan)
        {
            Console.WriteLine("Startowa plansza: ");
            Print(startowyStan);

            Console.WriteLine("\nDocelowa plansza: ");
            Print(docelowyStan);

            Console.WriteLine("\nKolejne kroki algorytmu:");


            var queue = new SortedList<double, (PuzzleBoard last, List<PuzzleBoard> all)>(new DuplicateKeyComparer<double>());

            var startowaPlansza = new PuzzleBoard(startowyStan, 0, 0);
            startowaPlansza.Koszt = FunctionF(startowaPlansza, docelowyStan);

            queue.Add(startowaPlansza.Koszt, (startowaPlansza, new List<PuzzleBoard> { startowaPlansza}));

            var closed = new List<PuzzleBoard>();
            while (true)
            {
                var cur = GetFirstNotInClosed(queue, closed);
           //     Print(cur.Plansza);

                //brak zle ulozonych pol oznacza, ze mamy docelowy stan
                if (FunctionH(cur.Value.last.Plansza, docelowyStan) == 0) return cur.Value.all;

                foreach (var kolejnaPlansza in cur.Value.last.ZwrocKolejnePlansze())
                {
                    kolejnaPlansza.Koszt = FunctionF(kolejnaPlansza, docelowyStan);
                    queue.Add(kolejnaPlansza.Koszt, (kolejnaPlansza, new List<PuzzleBoard>(cur.Value.all) { kolejnaPlansza }));
                }
                closed.Add(cur.Value.last);

            }
        }
    }
}
