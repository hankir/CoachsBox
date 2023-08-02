namespace CoachsBox.WebApp.AppFacade.Accounting.DTO
{
  public class GroupBalanceDTO
  {
    public int GroupId { get; set; }

    public string GroupName { get; set; }

    public decimal Balance { get; set; }

    public decimal Income { get; set; }

    public decimal Depts { get; set; }

    public int AgreementId { get; set; }

    public string AgreementName { get; set; }

    public int BranchId { get; set; }

    public string BranchCity { get; set; }

    public int CoachId { get; set; }
  }
}
