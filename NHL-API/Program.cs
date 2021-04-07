namespace NHL_API
{
    class Program
    {
        static void Main(string[] args)
        {
            var again = true;

            while (again)
            {
                again = UserInputToCsvLoop.RunProgramLoop();
            }
        }
    }
}
