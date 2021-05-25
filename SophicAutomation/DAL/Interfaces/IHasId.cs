using System;

namespace DAL.Interfaces
{
    public interface IHasId<out T> where T : IEquatable<T>
    {
        T Id { get; }
    }
}
