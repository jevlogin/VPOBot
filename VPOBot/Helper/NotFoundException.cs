namespace WORLDGAMEDEVELOPMENT
{
    public sealed class NotFoundException : Exception
    {
        public NotFoundException(string message) : base(message)
        {
            Console.WriteLine($"Exception:\n{message}");
        }
    }
}