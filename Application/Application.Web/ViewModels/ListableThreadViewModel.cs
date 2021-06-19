using Application.Domain.ApplicationEntities;
using System;

namespace Application.Web.ViewModels
{
    public class ListableThreadViewModel
    {
        public Guid ThreadId { get; set; }
        public string Heading { get; set; }
        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string UserEmail { get; set; }
        public DateTime DateTime { get; set; }

        public ListableThreadViewModel()
        {

        }

        public ListableThreadViewModel(Thread model)
        {
            ThreadId = model.Id;
            Heading = model.Heading;
            UserFirstName = model.User.FirstName;
            UserLastName = model.User.LastName;
            UserEmail = model.User.Email;
            DateTime = model.DateTime;
        }
    }
}