using Application.Data.Repositories;

namespace Application.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        private IUserRepository _userRepository;
        private IThreadRepository _threadRepository;
        private IPostRepository _postRepository;
        private IReactionRepository _reactionRepository;
        private ICategoryRepository _categoryRepository;
        private ITopicRepository _topicRepository;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public IUserRepository UserRepository
        {
            get
            {
                _userRepository ??= new UserRepository(_context);

                return _userRepository;
            }
        }

        public IThreadRepository ThreadRepository
        {
            get
            {
                _threadRepository ??= new ThreadRepository(_context);

                return _threadRepository;
            }
        }

        public IPostRepository PostRepository
        {
            get
            {

                _postRepository ??= new PostRepository(_context);

                return _postRepository;
            }
        }

        public IReactionRepository ReactionRepository
        {
            get
            {
                _reactionRepository ??= new ReactionRepository(_context);

                return _reactionRepository;
            }
        }

        public ICategoryRepository CategoryRepository
        {
            get
            {
                _categoryRepository ??= new CategoryRepository(_context);

                return _categoryRepository;
            }
        }

        public ITopicRepository TopicRepository
        {
            get
            {
                _topicRepository ??= new TopicRepository(_context);

                return _topicRepository;
            }
        }

        public void Save()
        {
            _context.SaveChanges();
        }

    }
}
