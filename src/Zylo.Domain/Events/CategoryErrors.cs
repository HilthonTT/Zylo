using SharedKernel;

namespace Zylo.Domain.Events;

public static class CategoryErrors
{
    public static Error NotFound(int id) => Error.NotFound(
        "Category.NotFound",
        $"The category with the Id = '{id}' was not found.");
}
