namespace SqlReleaseManager.Core.Models;

public record DatabaseDto
{
    public string Name { get; set; }
    public string CompatibilityLevel { get; set; }
    public string Collation { get; set; }
    public string RecoveryModel { get; set; }
    public string Status { get; set; }
    public string CreateDate { get; set; }
}