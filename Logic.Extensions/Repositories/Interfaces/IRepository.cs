using System;
using System.Collections.Generic;

namespace Logic.Extensions.Repositories.Interfaces
{
    public interface IRepository<T> : IDisposable
        where T : class
    {
        IEnumerable<T> GetList(); // получение всех объектов
        T GetItem(int id); // получение одного объекта по id
        void Add(T item); // создание объекта
        void Update(T item); // обновление объекта
        void Remove(int id); // удаление объекта по id
        void Save();  // сохранение изменений
    }
}
