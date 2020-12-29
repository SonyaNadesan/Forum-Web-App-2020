namespace Application.Services.Files.Filters
{
    public interface IFileAndSizeRestriction
    {
        int MaximumSizeAllowed { get; set; }
    }
}