using CoachsBox.Coaching.Accounting.CoachingServiceAgreementModel;
using CoachsBox.Coaching.Accounting.GroupAccountModel;
using CoachsBox.Core;

namespace CoachsBox.Coaching.Accounting.AgreementRegistryEntryModel
{
  /// <summary>
  /// Реестр соглашений.
  /// </summary>
  public class AgreementRegistryEntry : BaseEntity
  {
    public AgreementRegistryEntry(int serviceAgreementId, int groupId, int groupAccountId)
    {
      this.AgreementId = serviceAgreementId;
      this.GroupId = groupId;
      this.GroupAccountId = groupAccountId;
    }

    public AgreementRegistryEntry(CoachingServiceAgreement serviceAgreement, GroupAccount groupAccount)
      : this(serviceAgreement.Id, groupAccount.GroupId, groupAccount.Id)
    {
      this.Agreement = serviceAgreement;
      this.GroupAccount = groupAccount;
    }

    /// <summary>
    /// Получить идентификатор соглашения.
    /// </summary>
    public int AgreementId { get; private set; }

    /// <summary>
    /// Получить соглашение.
    /// </summary>
    public CoachingServiceAgreement Agreement { get; private set; }

    /// <summary>
    /// Получить идентификатор группы для которой определено соглашение.
    /// </summary>
    public int GroupId { get; private set; }

    /// <summary>
    /// Получить идентификатор счета группы.
    /// </summary>
    public int? GroupAccountId { get; private set; }

    /// <summary>
    /// Получить счет группы.
    /// </summary>
    public GroupAccount GroupAccount { get; private set; }

    /// <summary>
    /// Изменить соглашение.
    /// </summary>
    /// <param name="agreement">Новое соглашение.</param>
    public void ChangeAgreement(CoachingServiceAgreement agreement)
    {
      this.AgreementId = agreement.Id;
      this.Agreement = agreement;
    }

    private AgreementRegistryEntry()
    {
      // Требует Entity framework core
    }
  }
}
