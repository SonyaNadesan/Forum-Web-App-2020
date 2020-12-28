using System.IO;

namespace Application.Services.Documents
{
    public interface IMemoryStreamGeneratorService<T>
    {
        MemoryStream Generate(T content);
    }
}