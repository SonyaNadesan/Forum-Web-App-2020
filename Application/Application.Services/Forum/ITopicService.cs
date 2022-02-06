using Application.Domain.ApplicationEntities;
using Sonya.AspNetCore.Common;
using System;
using System.Collections.Generic;

namespace Application.Services.Forum
{
    public interface ITopicService
    {
        ServiceResponse<IEnumerable<Topic>> GetAll();
        ServiceResponse<Topic> Get(Guid topicId);
        ServiceResponse<Topic> Edit(Topic topic);
        ServiceResponse<Topic> Delete(Topic topic);
        ServiceResponse<Topic> Create(string nameInUrl, string displayName);
    }
}