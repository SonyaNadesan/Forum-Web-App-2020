namespace Application.Domain.ApplicationEntities
{
    public class User
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string ProfilePictureImageSrc { get; set; }

        public User()
        {
        }

        public User(string userId)
        {
            Id = userId;
        }
    }
}
