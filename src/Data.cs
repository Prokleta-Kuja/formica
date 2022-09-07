namespace formica;

public class Data
{
    public Steps Step { get; set; } = Steps.WhoAreYou;
    // WhoAreYou
    public bool Internal { get; set; }
    public bool External { get; set; }
    public string? CompanyName { get; set; }
    public string? EmployeeName { get; set; }
    public string? Contact { get; set; }
    // Test
    public string MyProperty { get; set; } = "Test";

}