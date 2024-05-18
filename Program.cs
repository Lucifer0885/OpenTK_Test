
namespace Circle
{
    internal class Program
    {
        [Obsolete]
        private static void Main(string[] args)
        {
            using (Game game = new Game(800, 600, "Circle"))
            {
                game.Run();
            }
        }
    }
}


