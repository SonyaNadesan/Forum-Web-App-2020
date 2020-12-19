namespace Application.Services.Authentication
{
    public class ValidationResult<TValidatedEntity, TStatus>
    {
        public TValidatedEntity ValidatedEntity { get; set; }
        public TStatus Status { get; set; }

        public ValidationResult(TValidatedEntity validatedEntity)
        {
            ValidatedEntity = validatedEntity;
        }
    }
}
