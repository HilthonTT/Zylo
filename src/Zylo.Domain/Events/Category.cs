using SharedKernel;

namespace Zylo.Domain.Events;

public sealed class Category : Enumeration<Category>
{
    public static readonly Category None = new(0, "None");
    public static readonly Category Concert = new(1, "Concert");
    public static readonly Category Sports = new(2, "Sports");
    public static readonly Category Conference = new(3, "Conference");
    public static readonly Category Workshop = new(4, "Workshop");
    public static readonly Category Festival = new(5, "Festival");
    public static readonly Category Meetup = new(6, "Meetup");
    public static readonly Category Webinar = new(7, "Webinar");

    public Category(int id, string name) 
        : base(id, name)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="Category"/> class.
    /// </summary>
    /// <param name="value">The value.</param>
    /// <remarks>
    /// Required by EF Core.
    /// </remarks>
    private Category()
    {
    }
}
