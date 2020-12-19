namespace Application.Services.Shared
{
    public interface IRandomStringGeneratorService
    {
        string Generate(int length = -1);
    }
}