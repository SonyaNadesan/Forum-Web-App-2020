namespace Application.Services.Pagination
{
    public class PaginationConfigurationBuilder<T>
    {
        private Pagination<T> _pagination;

        internal PaginationConfigurationBuilder(Pagination<T> pagination)
        {
            _pagination = pagination;
        }

        public PaginationConfigurationBuilder<T> ConfigureForm(string action, string method)
        {
            _pagination.FormAction = action;
            _pagination.FormMethod = method;

            return this;
        }

        public PaginationConfigurationBuilder<T> AdParameterAndValue(string key, string value)
        {
            _pagination.MoreParametersAndValues.Add(key, value);

            return this;
        }

        public Pagination<T> Build()
        {
            return _pagination;
        }
    }
}
