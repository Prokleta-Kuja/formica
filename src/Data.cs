using System.Text;

namespace formica;

public class Data
{
    public DateTime StartedAt { get; set; } = DateTime.UtcNow;
    public DateTime? FinishedAt { get; set; }
    public Steps Step { get; set; } = Steps.WhoAreYou;
    // WhoAreYou
    public Who From { get; set; } = Who.NotSet;
    public string? CompanyName { get; set; }
    public bool WithholdingCompanyName { get; set; }
    public bool CompanyCroBased { get; set; }
    public bool CompanyCroOffice { get; set; }
    public string? FromName { get; set; }
    public string? Contact { get; set; }
    // LookingFor
    public For For { get; set; }
    public string HrsPerWeek { get; set; } = string.Empty;
    public string Duration { get; set; } = string.Empty;
    public string TechStack { get; set; } = string.Empty;
    public Relationship Relationship { get; set; }
    public Place Place { get; set; }
    // SelectionProcess
    public Selection Process { get; set; }
    public string SelectionDuration { get; set; } = string.Empty;
    public string WorkAssignmentCompensation { get; set; } = string.Empty;
    public bool? Feedback { get; set; }
    // Compensation
    public bool? WillingToShareCompensation { get; set; }
    public string Compensation { get; set; } = string.Empty;
    public string ShareCompensationStep { get; set; } = string.Empty;

    public string GetText()
    {
        if (From == Who.NotSet)
            return string.Empty;

        var sb = new StringBuilder();
        sb.AppendLine($"Started at: {StartedAt}");
        sb.AppendLine($"Finished at: {FinishedAt}");
        sb.AppendLine();
        sb.AppendLine("# Who");
        // WhoAreYou
        switch (From)
        {
            case Who.ExternalRecruiter:
                sb.Append($"External recruiter {FromName}");
                if (WithholdingCompanyName)
                    sb.Append($" from {CompanyName}, without disclosing client name,");
                else
                    sb.Append($" on behalf of his client {CompanyName}");
                break;
            case Who.InternalRecruiter:
                sb.Append($"Internal recruiter {FromName} from {CompanyName}");
                break;
            case Who.Freelancer:
                sb.Append($"Freelancer {FromName}");
                break;
        }
        sb.AppendLine(" reached out to you.");

        if (CompanyCroBased)
        {
            sb.Append(From == Who.ExternalRecruiter ? "Client company" : "Company");
            sb.AppendLine(" is based in Croatia.");
        }
        if (CompanyCroOffice)
        {
            sb.Append(From == Who.ExternalRecruiter ? "Client company" : "Company");
            sb.AppendLine(" has office(s) in Croatia.");
        }
        sb.AppendLine(Contact);

        sb.AppendLine(); sb.AppendLine("# Looking for");
        foreach (var position in Enum.GetValues<For>())
            if (For.HasFlag(position))
                sb.AppendLine(position.ToString());
        sb.AppendLine();

        if (!string.IsNullOrWhiteSpace(HrsPerWeek))
            sb.AppendLine($"Hrs per week: {HrsPerWeek}");
        sb.AppendLine($"Duration: {Duration}");
        sb.AppendLine("Tech stack:"); sb.AppendLine(TechStack);
        sb.AppendLine("Relationship:");
        foreach (var relation in Enum.GetValues<Relationship>())
            if (Relationship.HasFlag(relation))
                sb.AppendLine($"\t{relation}");
        sb.AppendLine();
        sb.AppendLine("Work from:");
        foreach (var place in Enum.GetValues<Place>())
            if (Place.HasFlag(place))
                sb.AppendLine($"\t{place}");

        sb.AppendLine(); sb.AppendLine("# Selection process");
        foreach (var step in Enum.GetValues<Selection>())
            if (Process.HasFlag(step))
                sb.AppendLine($"\t{step}");

        sb.AppendLine($"Estimated selection process duration: {SelectionDuration}");
        if (Feedback.HasValue)
            sb.AppendLine(Feedback.Value ? "Will provide feedback." : "Won't provide feedback.");
        if (!string.IsNullOrWhiteSpace(WorkAssignmentCompensation))
            sb.AppendLine($"Work assignment compensation: {WorkAssignmentCompensation}");

        sb.AppendLine(); sb.AppendLine("# Compensation");
        if (WillingToShareCompensation.HasValue)
            if (WillingToShareCompensation.Value)
                sb.AppendLine(Compensation);
            else
                sb.AppendLine($"Will be shared {ShareCompensationStep}");

        return sb.ToString();
    }
}
public enum Steps { WhoAreYou = 0, LookingFor = 1, SelectionProcess = 2, Compensation = 3, Sending = 4, ThankYou = 5, }
[Flags] public enum Who { NotSet = 1, ExternalRecruiter = 2, InternalRecruiter = 4, Freelancer = 8, }
[Flags] public enum For { FullTime = 1, PartTime = 2, Project = 4, }
[Flags] public enum Relationship { Employment = 1, B2B = 2, }
[Flags] public enum Place { Office = 1, Remote = 2, Combination = 4, }
[Flags] public enum Selection { GeneralInterview = 1, TechInterview = 2, WhiteboardInterview = 4, EnglishTest = 8, PersonalityTest = 16, IntelligenceTest = 32, CodeTest = 64, CodeReview = 128, WorkAssignment = 256, }