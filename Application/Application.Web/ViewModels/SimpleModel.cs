using System;

namespace Application.Web.ViewModels
{
    public class SimpleModel
    {
        public Guid Id { get; set; }
        public string NameInUrl { get; set; }
        public string DisplayName { get; set; }

        public SimpleModel(Guid id, string nameInUrl, string displayName)
        {
            Id = id;
            NameInUrl = nameInUrl;
            DisplayName = displayName;
        }
    }
}
