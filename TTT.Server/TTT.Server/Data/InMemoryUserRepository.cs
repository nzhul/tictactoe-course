using System.Collections.Generic;
using System.Linq;

namespace TTT.Server.Data
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<User> _entities;

        public InMemoryUserRepository()
        {
            _entities = new List<User>()
            {
                new User
                {
                    Id = "dido1",
                    Password = "eee",
                    IsOnline= true,
                    Score = 10
                },
                new User
                {
                    Id = "dido2",
                    Password = "eee",
                    IsOnline= true,
                    Score = 35
                },
                new User
                {
                    Id = "dido3",
                    Password = "eee",
                    IsOnline= true,
                    Score = 21
                },
            };
        }

        public void Add(User entity)
        {
            _entities.Add(entity);
        }

        public void Delete(string id)
        {
            var entity = _entities.FirstOrDefault(x => x.Id == id);
            _entities.Remove(entity);
        }

        public User Get(string id)
        {
            return _entities.FirstOrDefault(x => x.Id == id);
        }

        public IQueryable<User> GetQuery()
        {
            return _entities.AsQueryable();
        }

        public ushort GetTotalCount()
        {
            return (ushort)_entities.Count(x => x.IsOnline);
        }

        public void SetOffline(string id)
        {
            _entities.FirstOrDefault(e => e.Id == id).IsOnline = false;
        }

        public void SetOnline(string id)
        {
            _entities.FirstOrDefault(e => e.Id == id).IsOnline = true;
        }

        public void Update(User entity)
        {
            var dbIndex = _entities.FindIndex(e => e.Id == entity.Id);
            _entities[dbIndex] = entity;
        }
    }
}
