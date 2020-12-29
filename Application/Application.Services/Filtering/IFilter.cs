namespace Application.Services.Filtering
{
    public interface IFilter<T>
    {
        string Description { get; }

        bool IsValid(T item);
    }
}
