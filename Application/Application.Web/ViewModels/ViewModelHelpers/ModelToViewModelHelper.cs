using Application.Domain.ApplicationEntities;

namespace Application.Web.ViewModels.ViewModelHelpers
{
    public class ModelToViewModelHelper
    {
        public static ListableThreadViewModel ThreadToListableThreadViewModel(Thread model)
        {
            var result = new ListableThreadViewModel(model);

            return result;
        }
    }
}