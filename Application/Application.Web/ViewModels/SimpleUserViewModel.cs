using Application.Domain.ApplicationEntities;

namespace Application.Web.ViewModels
{
    public class SimpleUserViewModel
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string AvatarSrc { get; set; }

        public SimpleUserViewModel(User user)
        {
            Id = user.Id;
            Name = user.FirstName + " " + user.LastName;
            AvatarSrc = string.IsNullOrEmpty(user.ProfilePictureImageSrc) ? "" : user.ProfilePictureImageSrc;
            Email = user.Email;
        }
    }
}
