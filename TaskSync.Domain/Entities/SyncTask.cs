using TaskSync.Domain.Entities.ValueObjects;
using TaskSync.Domain.ResultPattern;
namespace TaskSync.Domain.Entities;

public sealed class SyncTask
{
    public Guid Id { get; }
    public string Title { get; private set;}
    public string Description { get; private set;}
    public DueDateUtc DueDateUtc { get; private set;}
    public bool IsCompleted { get; private set;} = false;
    public Guid UserId { get; }
    public User User { get; } = null!;
    public DateTimeOffset CreatedAtUtc { get; }

    private SyncTask(Guid id, string title, string description, DueDateUtc dueDateUtc, bool isCompleted, Guid userId, DateTimeOffset createdAtUtc)
    {
        Id = id;
        Title = title;
        Description = description;
        DueDateUtc = dueDateUtc;
        IsCompleted = isCompleted;
        UserId = userId;
        CreatedAtUtc = createdAtUtc;
    }

    public static Result<SyncTask> Create(string title, string description, DueDateUtc dueDateUtc, Guid userId, DateTimeOffset createdAtUtc)
    {
        List<Error> errors = new();

        if (string.IsNullOrWhiteSpace(title))
        {
            errors.Add(Error.Validation("SyncTask.TitleEmpty", "Title is empty or null"));
        }
        if (string.IsNullOrWhiteSpace(description))
        {
            errors.Add(Error.Validation("SyncTask.DescriptionEmpty", "Description is empty or null"));
        }
        if (userId == Guid.Empty)
        {
            errors.Add(Error.Validation("SyncTask.IdEmpty", "Id is empty or null"));
        }

        return errors.Count > 0 ? errors : new SyncTask(Guid.NewGuid(), title, description, dueDateUtc, default, userId, createdAtUtc);

    }
}