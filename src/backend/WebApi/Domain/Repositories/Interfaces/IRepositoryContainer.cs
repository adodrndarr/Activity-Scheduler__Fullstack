namespace Domain.Repositories.Interfaces
{
    public interface IRepositoryContainer
    {
        public IActivityEntityRepository ActivityEntityRepo { get; }
        public IActivityRepository ActivityRepo { get; }
        public IUserRepository UserRepo { get; }
        void SaveChanges();
    }
}
