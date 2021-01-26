using Application.Data;
using Application.Domain.ApplicationEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using static Application.Domain.Enums;

namespace Application.Services.Forum
{
    public class ReactionService : IReactionService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ReactionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IEnumerable<Reaction> GetReactionsByThreadId(Guid threadId)
        {
            return _unitOfWork.ReactionRepository.GetAll().Where(r => r.ThreadId == threadId);
        }

        public void Respond(string email, Guid threadId)
        {
            var reaction = _unitOfWork.ReactionRepository.Get(email, threadId);

            if (reaction.ReactionType == ReactionTypes.NONE.ToString())
            {
                var user = _unitOfWork.UserRepository.Get(email);
                var thread = _unitOfWork.ThreadRepository.Get(threadId);
                var newReaction = new Reaction(user, thread, ReactionTypes.LIKE);
                _unitOfWork.ReactionRepository.Add(newReaction);
            }
            else
            {
                _unitOfWork.ReactionRepository.Delete(reaction.ReactionId);
            }

            _unitOfWork.Save();
        }
    }
}
