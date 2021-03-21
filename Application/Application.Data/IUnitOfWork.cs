using Application.Data.Repositories;

namespace Application.Data
{
    public interface IUnitOfWork
    {
        IUserRepository UserRepository { get; }

        IThreadRepository ThreadRepository { get; }

        IPostRepository PostRepository { get; }

        IReactionRepository ReactionRepository { get; }

        ICategoryRepository CategoryRepository { get; }

        ITopicRepository TopicRepository { get; }

        IThreadCategoryRepository ThreadCategoryRepository { get; }

        void Save();
    }
}