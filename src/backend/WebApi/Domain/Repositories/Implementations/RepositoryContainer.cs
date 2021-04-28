using ActivityScheduler.Domain.DataAccess;
using Domain.Repositories.Interfaces;


namespace Domain.Repositories.Implementations
{
    public class RepositoryContainer : IRepositoryContainer
    {
        public RepositoryContainer(ActivitySchedulerDbContext dbContext)
        {
            this._dbContext = dbContext;
        }


        private IActivityEntityRepository _activityEntityRepo;
        private IActivityRepository _activityRepo;
        private IUserRepository _userRepo;
        private ActivitySchedulerDbContext _dbContext;

        public IActivityEntityRepository ActivityEntityRepo
        {
            get
            {
                if (_activityEntityRepo == null)
                {
                    _activityEntityRepo = new ActivityEntityRepository(_dbContext);
                }

                return _activityEntityRepo;
            }
        }

        public IActivityRepository ActivityRepo
        {
            get
            {
                if (_activityRepo == null)
                {
                    _activityRepo = new ActivityRepository(_dbContext);
                }

                return _activityRepo;
            }
        }

        public IUserRepository UserRepo
        {
            get
            {
                if (_userRepo == null)
                {
                    _userRepo = new UserRepository(_dbContext);
                }

                return _userRepo;
            }
        }

        public void SaveChanges()
        {
            _dbContext.SaveChanges();
        }
    }
}
