
public partial class CrawlState : MovementState
{
    public CrawlState() {
        speed = 1.25f;
        addedFov = 0.0f;
        playerHeight = 0.3f;
    }

    public override string ToString()
    {
        return "Crawl";
    }
}