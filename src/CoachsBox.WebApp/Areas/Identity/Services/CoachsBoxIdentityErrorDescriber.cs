using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace CoachsBox.WebApp.Areas.Identity.Services
{
  public class CoachsBoxIdentityErrorDescriber : IdentityErrorDescriber
  {
    public override IdentityError DuplicateEmail(string email)
    {
      var result = base.DuplicateEmail(email);
      result.Description = "Пользователь с указаной эл. почтой уже зарегистрирован";
      return result;
    }

    public override IdentityError PasswordRequiresDigit()
    {
      var result = base.PasswordRequiresDigit();
      result.Description = "Пароль должен содержать хотя бы одну цифру";
      return result;
    }

    public override IdentityError PasswordRequiresLower()
    {
      var result = base.PasswordRequiresLower();
      result.Description = "Пароль должен содержать хотя бы одну прописную букву";
      return result;
    }

    public override IdentityError PasswordRequiresNonAlphanumeric()
    {
      var result = base.PasswordRequiresNonAlphanumeric();
      result.Description = "Пароль должен содержать хотя бы один символ не из алфавита";
      return result;
    }

    public override IdentityError PasswordRequiresUpper()
    {
      var result = base.PasswordRequiresUpper();
      result.Description = "Пароль должен содержать хотя бы одну заглавную букву";
      return result;
    }

    public override IdentityError PasswordTooShort(int length)
    {
      var result = base.PasswordTooShort(length);
      result.Description = "Пароль слишком короткий";
      return result;
    }
  }
}
