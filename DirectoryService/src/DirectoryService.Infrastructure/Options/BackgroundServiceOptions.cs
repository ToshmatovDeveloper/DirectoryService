namespace DirectoryService.Infrastructure.Options;

public record BackgroundServiceOptions
{
    public const string SectionName = "BackgroundService";

    public int DepartmentCleanIntervalHours { get; set; }
    public int HardDeleteThresholdMonths { get; set; }
}