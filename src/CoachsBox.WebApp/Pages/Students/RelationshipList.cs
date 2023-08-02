using System.Collections.Generic;
using CoachsBox.Coaching.StudentModel;
using CoachsBox.WebApp.AppFacade.Students;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CoachsBox.WebApp.Pages.Students
{
  public class RelationshipList : SelectList
  {
    public const string EmptyRelationship = nameof(EmptyRelationship);

    public RelationshipList()
      : base(new List<SelectListItem>()
      {
        new SelectListItem("Выберите из списка", null),
        new SelectListItem(Relationship.Mother.GetLocalization(), Relationship.Mother.Name),
        new SelectListItem(Relationship.Father.GetLocalization(), Relationship.Father.Name),
        new SelectListItem(Relationship.Grandmother.GetLocalization(), Relationship.Grandmother.Name),
        new SelectListItem(Relationship.Grandfather.GetLocalization(), Relationship.Grandfather.Name)
      }, nameof(SelectListItem.Value), nameof(SelectListItem.Text))
    {

    }
  }
}
