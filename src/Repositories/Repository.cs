namespace src.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using src.Attributes;
    using src.Domain;
    using src.Specifications;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    public abstract class Repository<T> : IRepository<T> where T : EntityBase
    {
        private readonly TestDbContext _context;

        protected const string FieldsChanged = "FieldsChanged";

        public Repository(TestDbContext context)
        {
            _context = context;
        }

        public void Add(T entity)
        {
            if (entity is IEntityChanged)
            {
                entity.SetPropValue(FieldsChanged, GetAddedPropertyNames(entity));
            }

            _context.Set<T>().Add(entity);

        }

        public void AddRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities) Add(entity);
        }

        public void Update(T entity)
        {
            if (entity is IEntityChanged)
            {
                entity.SetPropValue(FieldsChanged, GetModifiedPropertyNames(entity));
            }

            _context.Set<T>().Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities) Update(entity);
        }

        public IQueryable<T> FindAll(Expression<Func<T, bool>> pression = null)
        {
            IQueryable<T> query = pression != null ? _context.Set<T>().Where(pression) : _context.Set<T>();

            return query;
        }

        public IQueryable<T> FindAllWithSpecification(ISpecification<T> spec)
        {
            return ApplySpecification(spec);
        }

        public T FindBy(int id)
        {
            return _context.Set<T>().FirstOrDefault(x => x.Id.Equals(id));
        }

        public async Task<T> FindByAsync(int id)
        {
            return await _context.Set<T>().FirstOrDefaultAsync(x => x.Id.Equals(id));
        }

        public void Remove(T entity)
        {
            if (entity is IEntityChanged)
            {
                entity.SetPropValue(FieldsChanged, GetDeletedPropertyNames(entity));
            }

            _context.Set<T>().Remove(entity);
        }

        public void RemoveRange(IEnumerable<T> entities)
        {
            foreach (var entity in entities) Remove(entity);
        }

        private IQueryable<T> ApplySpecification(ISpecification<T> spec)
        {
            return SpecificationEvaluator<T>.GetQuery(_context.Set<T>().AsQueryable(), spec);
        }

        protected virtual IEnumerable<FieldChanged> GetModifiedPropertyNames(T entity)
        {
            var fieldsChanged = new List<FieldChanged>();

            var properties = GetTrackerProperties();

            if (properties.Count() == 0) return null;

            var entry = _context.Entry(entity);

            foreach (var prop in properties)
            {
                if (prop.Value.Any(x => x == ActionFieldChangedEnum.Modified))
                {
                    var entityProperty = entry.Property(prop.Key);

                    if (!entityProperty.IsModified) continue;

                    fieldsChanged.Add(new FieldChanged(prop.Key,
                        typeof(T),
                        ActionFieldChangedEnum.Modified)
                    {
                        OldValue = entityProperty.OriginalValue?.ToString(),
                        NewValue = entityProperty.CurrentValue?.ToString(),
                    });
                }

            }

            return fieldsChanged;
        }

        protected virtual IEnumerable<FieldChanged> GetAddedPropertyNames(T entity)
        {
            var fieldsChanged = new List<FieldChanged>();

            var properties = GetTrackerProperties();

            if (properties.Count() == 0) return null;

            foreach (var prop in properties)
            {
                if (prop.Value.Any(x => x == ActionFieldChangedEnum.Added))
                {
                    fieldsChanged.Add(new FieldChanged(prop.Key,
                        typeof(T),
                        ActionFieldChangedEnum.Added)
                    {
                        NewValue = entity.GetPropValue<object>(prop.Key)?.ToString(),
                    });
                }

            }

            return fieldsChanged;
        }

        protected virtual IEnumerable<FieldChanged> GetDeletedPropertyNames(T entity)
        {
            var fieldsChanged = new List<FieldChanged>();

            var properties = GetTrackerProperties();

            if (properties.Count() == 0) return null;

            foreach (var prop in properties)
            {
                if (prop.Value.Any(x => x == ActionFieldChangedEnum.Deleted))
                {
                    fieldsChanged.Add(new FieldChanged(prop.Key,
                        typeof(T),
                        ActionFieldChangedEnum.Deleted)
                    {
                        NewValue = entity.GetPropValue<object>(prop.Key)?.ToString(),
                    });
                }

            }

            return fieldsChanged;
        }

        protected virtual IDictionary<string, ActionFieldChangedEnum[]> GetTrackerProperties()
        {
            var result = new Dictionary<string, ActionFieldChangedEnum[]>();

            var properties = typeof(T).GetProperties()
               .Where(attr => attr.CustomAttributes.Any(s => s.AttributeType == typeof(TrackerAttribute)))
               .ToArray();

            foreach (var prop in properties)
            {
                object[] attrs = prop.GetCustomAttributes(true);

                foreach (object attr in attrs)
                {
                    TrackerAttribute trackerAttr = attr as TrackerAttribute;
                    if (trackerAttr != null)
                    {
                        string propName = prop.Name;
                        result.Add(propName, trackerAttr.Actions);
                    }
                }
            }

            return result;
        }
    }
}
