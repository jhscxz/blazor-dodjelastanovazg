namespace DodjelaStanovaZG.Helpers;

public class PagedResult<T>
{
    public List<T> Items { get; init; } = [];
    public int TotalCount { get; init; }
}