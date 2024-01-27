namespace Tests;

public class Blog
{
    private readonly List<Post> _posts = new();
    public int BlogId { get; set; }
    public string Url { get; set; }
    public IReadOnlyCollection<Post> Posts => _posts;
}
