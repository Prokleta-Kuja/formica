using System.Text;

namespace formica;

public class Data
{
    public Steps Step { get; set; } = Steps.WhoAreYou;
    // WhoAreYou
    public Who From { get; set; } = Who.NotSet;
    public string? CompanyName { get; set; }
    public bool WithholdingCompanyName { get; set; }
    public string? FromName { get; set; }
    public string? Contact { get; set; }
    // Test
    public string MyProperty { get; set; } = "Test";

    public string GetText()
    {
        var sb = new StringBuilder();

        // WhoAreYou
        switch (From)
        {
            case Who.ExternalRecruiter:
                sb.Append($"External recruiter {FromName}");
                if (WithholdingCompanyName)
                    sb.Append($"from {CompanyName}, without disclosing client name,");
                else
                    sb.Append($"on behalf of his client {CompanyName}");
                break;
            case Who.InternalRecruiter:
                sb.Append($"Internal recruiter {FromName} from {CompanyName}");
                break;
            case Who.Freelancer:
                sb.Append($"Freelancer {FromName}");
                break;
        }
        sb.AppendLine(" reached out you.");

        return sb.ToString();
    }
}
public enum Steps { WhoAreYou = 0, }
[Flags] public enum Who { NotSet = 1, ExternalRecruiter = 2, InternalRecruiter = 4, Freelancer = 8, }