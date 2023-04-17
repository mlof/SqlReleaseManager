namespace SqlReleaseManager.Web.Models;

public record DatabaseViewModel
{
    public string Name { get; set; }
    public string CompatibilityLevel { get; set; }
    public string Collation { get; set; }
    public string RecoveryModel { get; set; }
    public string Status { get; set; }
    public string CreateDate { get; set; }
    public int ServerId { get; set; }
}