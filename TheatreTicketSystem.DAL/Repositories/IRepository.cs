using System.Linq.Expressions;

namespace TheatreTicketSystem.DAL.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        // Отримання всіх записів
        IEnumerable<TEntity> GetAll();

        // Отримання записів за умовою
        IEnumerable<TEntity> Find(Expression<Func<TEntity, bool>> predicate);

        // Отримання одного запису за умовою
        TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate);

        // Додавання нового запису
        void Add(TEntity entity);

        // Додавання багатьох записів
        void AddRange(IEnumerable<TEntity> entities);

        // Оновлення запису
        void Update(TEntity entity);

        // Видалення запису
        void Remove(TEntity entity);

        // Видалення багатьох записів
        void RemoveRange(IEnumerable<TEntity> entities);
    }
}