namespace DoWork;

public class DoWorkService : IDoWorkService
{
    public DoWorkService() { }

    public async Task DoWorkAsync()
    {
        Console.WriteLine(">>>>Start!");

        Console.WriteLine("<<<<End!");
    }
}
