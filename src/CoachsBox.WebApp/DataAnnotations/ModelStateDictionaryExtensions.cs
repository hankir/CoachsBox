using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CoachsBox.WebApp.DataAnnotations
{
  /// <summary>
  /// Класс расширений для работы с состоянием модели.
  /// </summary>
  public static class ModelStateDictionaryExtensions
  {
    /// <summary>
    /// Подавить валидацию по указанному выражению.
    /// </summary>
    /// <typeparam name="T">Тип возвращаемого результата.</typeparam>
    /// <param name="modelState">Состояние модели.</param>
    /// <param name="expression">Выражаение для доступа к свойству модели.</param>
    public static void SuppressFor<T>(this ModelStateDictionary modelState, Expression<Func<T>> expression)
    {
      if (modelState == null)
        return;

      if (expression == null)
        return;

      var paths = new List<string>();
      var currentExpression = expression.Body as MemberExpression;
      while (currentExpression != null)
      {
        paths.Add(currentExpression.Member.Name);
        currentExpression = currentExpression.Expression as MemberExpression;
      }

      paths.Reverse();
      var prefix = string.Join('.', paths.ToArray());
      var requiredFields = expression.ReturnType.GetProperties().Where(p => p.GetCustomAttribute<RequiredAttribute>() != null).Select(p => p.Name);
      if (requiredFields.Any())
      {
        foreach (var key in requiredFields)
        {
          var stateKey = $"{prefix}.{key}";
          if (modelState.ContainsKey(stateKey))
            SuppressModelStateEntry(modelState[stateKey], ModelValidationState.Skipped);
        }
      }
      else
      {
        if (modelState.ContainsKey(prefix))
          SuppressModelStateEntry(modelState[prefix], ModelValidationState.Skipped);
      }
    }

    private static void SuppressModelStateEntry(ModelStateEntry modelStateEntry, ModelValidationState state)
    {
      modelStateEntry.ValidationState = state;
      modelStateEntry.Errors.Clear();
    }
  }
}
