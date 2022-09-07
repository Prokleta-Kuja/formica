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


        return sb.ToString();
    }
}
public enum Steps { WhoAreYou = 0, }
[Flags] public enum Who { NotSet = 1, ExternalRecruiter = 2, InternalRecruiter = 4, Freelancer = 8, }