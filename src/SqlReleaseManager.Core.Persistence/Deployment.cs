namespace SqlReleaseManager.Core.Persistence;

public class Deployment
{
    public int Id { get; set; }


    public string FileName { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public DeploymentType DeploymentType { get; set; }

    
    public int DatabaseInstanceId { get; set; }

    public DatabaseInstance DatabaseInstance { get; set; }


}