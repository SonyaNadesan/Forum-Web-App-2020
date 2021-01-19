using Application.Data;
using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum
{
    public class ThreadService : IThreadService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ThreadService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResponse<Thread> Get(Guid threadId)
        {
            var response = new ServiceResponse<Thread>();

            var thread = _unitOfWork.ThreadRepository.Get(threadId);

            response.Result = thread;

            if (thread == null)
            {
                response.ErrorMessage = "Thread not found";
                return response;
            }

            return response;
        }

        public ServiceResponse<IEnumerable<Thread>> GetAll()
        {
            var response = new ServiceResponse<IEnumerable<Thread>>();

            var threads = _unitOfWork.ThreadRepository.GetAll().ToList();

            response.Result = threads;

            if (threads == null)
            {
                response.ErrorMessage = "Sorry,something went wrong.";
                return response;
            }

            return response;
        }

        public ServiceResponse<Thread> Edit(Thread thread)
        {
            var response = new ServiceResponse<Thread>(thread);

            var threadFromDb = _unitOfWork.ThreadRepository.Get(thread.Id);

            if(threadFromDb == null)
            {
                response.ErrorMessage = "Thread Not Found.";
                return response;
            }

            try
            {
                _unitOfWork.ThreadRepository.Edit(thread);
                _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Thread> Delete(Thread thread)
        {
            var response = new ServiceResponse<Thread>(thread);

            var threadFromDb = _unitOfWork.ThreadRepository.Get(thread.Id);

            if (threadFromDb == null)
            {
                response.ErrorMessage = "Thread Not Found.";
                return response;
            }

            try
            {
                _unitOfWork.ThreadRepository.Delete(thread.Id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Thread> Create(string email, string heading, string body)
        {
            var response = new ServiceResponse<Thread>();

            var user = _unitOfWork.UserRepository.Get(email);

            if(user == null)
            {
                response.ErrorMessage = "User Not Set";
                return response;
            }

            var newThread = new Thread()
            {
                Id = Guid.NewGuid(),
                Heading = heading,
                Body = body,
                DateTime = DateTime.Now,
                User = user,
                UserId = user.Id
            };

            response.Result = newThread;

            try
            {
                _unitOfWork.ThreadRepository.Add(newThread);
                _unitOfWork.Save();
            }
            catch(Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
                return response;
            }

            return response;
        }
    }
}
