using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;

namespace Application.Services.Forum
{
    public interface IThreadService
    {
        ServiceResponse<List<Thread>> GetAll();
        ServiceResponse<Thread> Get(Guid threadId);
        ServiceResponse<Thread> Edit(Thread thread);
        ServiceResponse<Thread> Delete(Thread thread);
    }
}
