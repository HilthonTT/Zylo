namespace SharedKernel;

public interface ISoftDeletable
{
    DateTime? DeletedOnUtc { get; set; }

    bool IsDeleted { get; set; }
}
