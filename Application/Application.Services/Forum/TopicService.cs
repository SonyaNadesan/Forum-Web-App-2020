using Application.Data;
using Application.Domain.ApplicationEntities;
using Application.Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Application.Services.Forum
{
    public class TopicService : ITopicService
    {
        private readonly IUnitOfWork _unitOfWork;

        public TopicService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public ServiceResponse<Topic> Create(string nameInUrl, string displayName)
        {
            var response = new ServiceResponse<Topic>();

            nameInUrl = UrlParamFriednlyGeneratorService.GetTextForParamUse(nameInUrl);
            displayName = UrlParamFriednlyGeneratorService.GetTextForParamUse(displayName);

            var topic = _unitOfWork.TopicRepository.GetAll().SingleOrDefault(t => t.NameInUrl == nameInUrl || t.DisplayName == displayName);

            if (topic != null)
            {
                response.ErrorMessage = "Topic already exists. Name In URL; " + nameInUrl + ", Display Name: " + displayName;
                return response;
            }

            var newTopic = new Topic()
            {
                Id = Guid.NewGuid(),
                NameInUrl = nameInUrl,
                DisplayName = displayName
            };

            response.Result = newTopic;

            try
            {
                _unitOfWork.TopicRepository.Add(newTopic);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
                return response;
            }

            return response;
        }

        public ServiceResponse<Topic> Delete(Topic topic)
        {
            var response = new ServiceResponse<Topic>(topic);

            var topicFromDb = _unitOfWork.TopicRepository.Get(topic.Id);

            if (topicFromDb == null)
            {
                response.ErrorMessage = "Topic Not Found.";
                return response;
            }

            try
            {
                _unitOfWork.TopicRepository.Delete(topic.Id);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Topic> Edit(Topic topic)
        {
            var response = new ServiceResponse<Topic>(topic);

            var topicFromDb = _unitOfWork.TopicRepository.Get(topic.Id);

            if (topicFromDb == null)
            {
                response.ErrorMessage = "Topic Not Found.";
                return response;
            }

            try
            {
                _unitOfWork.TopicRepository.Edit(topic);
                _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                response.ErrorMessage = "Sorry, something went wrong.";
            }

            return response;
        }

        public ServiceResponse<Topic> Get(Guid topicId)
        {
            var response = new ServiceResponse<Topic>();

            var topic = _unitOfWork.TopicRepository.Get(topicId);

            response.Result = topic;

            if (topic == null)
            {
                response.ErrorMessage = "Topic not found";
                return response;
            }

            return response;
        }

        public ServiceResponse<IEnumerable<Topic>> GetAll()
        {
            var response = new ServiceResponse<IEnumerable<Topic>>();

            var topics = _unitOfWork.TopicRepository.GetAll().ToList();

            response.Result = topics;

            if (topics == null)
            {
                response.ErrorMessage = "Sorry,something went wrong.";
                return response;
            }

            return response;
        }
    }
}
