using System.Collections.Generic;

namespace Application.Web.ViewModels
{
    public class CreateThreadViewModel
    {
        public string Heading { get; set; }
        public string Body { get; set; }
        public SelectionViewModel Topic { get; set; }
        public List<SelectionViewModel> Categories { get; set; }
        public List<SelectionViewModel> TopicOptions { get; set; }
        public List<SelectionViewModel> CategoryOptions { get; set; }
    }
}
