using System.Collections.Generic;

namespace Application.Services.Files
{
    public class ImageFileExtensions : IFileExtensions
    {
        public List<string> Extensions
        {
            get
            {
                return new List<string>()
                {
                    ".jpg",
                    ".png",
                    ".gif",
                    ".jpeg"
                };
            }
        }
    }
}
