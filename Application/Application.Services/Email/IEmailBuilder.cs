namespace Application.Services.Email
{
    public interface IEmailBuilder
    {
        EmailConfigurationBuilder SetRecipientsAndFromAddress(string recipients, string fromAddress);
    }
}
