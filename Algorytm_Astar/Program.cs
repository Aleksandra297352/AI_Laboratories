namespace Algorytm_Astar
{

    internal class Program
    {
        static void Print(double[,] state)
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
        static void Main(string[] args)
        {
            Console.WriteLine("------Puzzle------");
            var puzzle = new Puzzle8();
            //var initialState = new double[,]
            //{
            //    {0, 2, 3 },
            //    {1, 4, 6 },
            //    {7, 5, 8 }
            //};
            //var finalState = new double[,]
            //{
            //    {1, 2, 3 },
            //    {4, 5, 6 },
            //    {7, 8, 0 }
            //};
               var initialState = new double[,]
            {
                   {1, 2, 3 },
                   {8, 0, 4 },
                   {7, 6, 5 }
            };
            var finalState = new double[,]
            {
                   {2, 8, 1 },
                   {0, 4, 3 },
                   {7, 6, 5 }
            };

            var puzzlePath = puzzle.SolvePuzzle(initialState, finalState);
            foreach(var puzzleBoard in puzzlePath)
            {
                Print(puzzleBoard.Plansza);
                Console.WriteLine(); 
            }

            Console.WriteLine("\n\n------Najkrotsza sciezka z miasta do miasta------");
            var astarCity = new Astar_City();

            var cities = astarCity.ReadFile("input.txt");
            var neighbourMatrix = astarCity.NeighbourMatrix(cities);
            var neighbourMatrixGoogle = astarCity.CreateMatrixFromFile("graf_niepelny.txt", cities, "neighbourMatrix.txt");
            var path = astarCity.Astar(cities, 3, 12, neighbourMatrixGoogle);

            Console.WriteLine($"Koszt sciezki: {path.cost / 1000} km, sciezka: {path.path}");

        }
    }
}