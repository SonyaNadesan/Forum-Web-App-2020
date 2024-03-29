﻿using Application.Domain.ApplicationEntities;
using Sonya.AspNetCore.Common;
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
        ServiceResponse<Thread> Create(string email, string heading, string body, Topic topic, List<Category> categories);
    }
}
