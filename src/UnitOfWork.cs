namespace src
{
    using Microsoft.AspNetCore.Http;
    using src.Domain;
    using src.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class UnitOfWork : IDisposable
    {
        protected readonly HttpContext _httpContext;

        private readonly TestDbContext _context;

        private int CreatedBy
        {
            get
            {
                var token = _httpContext.Request.Headers["Authorization"].ToString();

                return Convert.ToInt32(token);
            }
        }

        public UnitOfWork(IHttpContextAccessor httpContextAccessor,
            TestDbContext context)
        {
            _httpContext = httpContextAccessor.HttpContext;

            _context = context;

            OrderRepository = new GenericRepository<Order>(_context);

            OrderDetailRepository = new GenericRepository<OrderDetail>(_context);

        }

        public IRepository<Order> OrderRepository { get; private set; }

        public IRepository<OrderDetail> OrderDetailRepository { get; private set; }

        public async Task CommitAsync()
        {
            ProcessFieldsChanged();

            await _context.SaveChangesAsync();

        }

        private void ProcessFieldsChanged()
        {
            var entriesFieldsChanged = _context
               .ChangeTracker.Entries()
               .Where(e => e.Entity is IEntityChanged)
               .Select(e => new
               {
                   entityState = e.State,
                   entity = e.Entity,
                   entityChanged = e.Entity as IEntityChanged,
               });

            var histories = new List<History>();

            foreach (var e in entriesFieldsChanged)
            {
                if (e.entityChanged.FieldsChanged == null) continue;

                if (e.entityChanged.FieldsChanged.Count() == 0) continue;

                foreach (var fieldChanged in e.entityChanged.FieldsChanged)
                {
                    histories.Add(new History(fieldChanged, CreatedBy));
                }

            }

            if (histories.Count() > 0) _context.Histories.AddRange(histories);

        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
