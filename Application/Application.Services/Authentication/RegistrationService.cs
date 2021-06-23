using Application.Domain;
using Application.Services.Documents;
using Application.Services.Email;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.IO;
using System.Threading.Tasks;

namespace Application.Services.Authentication
{
    public class RegistrationService : IRegistrationService
    {
        private readonly IConfiguration Configuration;
        private readonly IEmailSenderService _emailService;
        private readonly IEmailBuilder _emailBuilder;
        private readonly IPasswordAssignmentService _passwordAssignmentService;
        private readonly IPdfBuilder<string> _pdfGeneratorService;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationService(IConfiguration configuration, IEmailSenderService emailService, IEmailBuilder emailGeneratorService,
                                   IPasswordAssignmentService passwordAssignmentService, IPdfBuilder<string> pdfGeneratorService, 
                                   UserManager<ApplicationUser> userManager)
        {
            Configuration = configuration;
            _emailService = emailService;
            _emailBuilder = emailGeneratorService;
            _passwordAssignmentService = passwordAssignmentService;
            _pdfGeneratorService = pdfGeneratorService;
            _userManager = userManager;
        }

        public async Task<ServiceResponse<ApplicationUser>> RegisterAccount(string email, string firstName, string lastName)
        {
            var response = new ServiceResponse<ApplicationUser>();

            var user = InitializeUser(email, firstName, lastName);

            var passwordChangeResponse = _passwordAssignmentService.AssignRandomlyGeneratedPassword(user, out string password);

            if (passwordChangeResponse.IsValid)
            {
                response.Result = user;

                var updateDbResponse = await _userManager.CreateAsync(user);

                if (updateDbResponse.Succeeded)
                {
                    var emailSent = SendRegistrationConfirmationEmail(email, password);

                    if (!emailSent.IsValid)
                    {
                        response.ErrorMessage = "User was created but email failed to send. Please use the account recovery feature.";
                    }

                    return response;
                }
            }

            var isEmailInUse = await _userManager.FindByNameAsync(email);

            response.ErrorMessage = isEmailInUse != null ? "There is already an account against that email." : "Failed to create user.";

            return response;
        }

        private ApplicationUser InitializeUser(string email, string firstName, string lastName)
        {
            return new ApplicationUser(email)
            {
                Email = email,
                UserName = email,
                EmailConfirmed = false,
                RegistrationConfirmed = false,
                FirstName = firstName,
                LastName = lastName
            };
        }

        private ServiceResponse<bool> SendRegistrationConfirmationEmail(string email, string password)
        {
            var htmlBody = File.ReadAllText(Configuration.GetSection("RegistrationEmail").Value).Replace("#password#", password);

            var attachment1 = new FileStreamAndName() { AttachmentStream = _pdfGeneratorService.Generate(htmlBody), FileName = "Attachment 1" };
            var attachment2 = new FileStreamAndName() { AttachmentStream = _pdfGeneratorService.Generate(htmlBody), FileName = "Attachment 2" };

            var mail = _emailBuilder.SetRecipientsAndFromAddress(email, Configuration.GetSection("FromEmail").Value)
                                    .SetBody(htmlBody, Enums.EmailBodyType.HtmlString)
                                    .SetSubject("Registration")
                                    .AddFile(attachment1)
                                    .AddFile(attachment2)
                                    .Build();

            return _emailService.Send(mail);
        }
    }
}