using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Services.Forum
{
    public interface IThreadService
    {
        ServiceResponse<IEnumerable<Thread>> GetAll();
        ServiceResponse<Thread> Get(Guid threadId);
        ServiceResponse<Thread> Edit(Thread thread);
        ServiceResponse<Thread> Delete(Thread thread);
        ServiceResponse<Thread> Create(string email, string heading, string body, string topic, string[] categories);
    }
}
