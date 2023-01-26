using ShoppingMart.Common.Models;

namespace ShoppingMart.Common.Repositories;

public interface IBuilder<T>
{
    T Build();
}
