namespace src.Repositories
{
    using src.Domain;

    public sealed class GenericRepository<T> : Repository<T>, IRepository<T> where T : EntityBase
    {
        public GenericRepository(TestDbContext context) : base(context)
        {
        }
    }
}
