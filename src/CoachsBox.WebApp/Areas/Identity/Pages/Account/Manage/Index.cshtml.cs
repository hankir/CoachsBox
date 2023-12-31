﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using CoachsBox.WebApp.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CoachsBox.WebApp.Areas.Identity.Pages.Account.Manage
{
  public partial class IndexModel : PageModel
  {
    private readonly UserManager<CoachsBoxWebAppUser> _userManager;
    private readonly SignInManager<CoachsBoxWebAppUser> _signInManager;
    private readonly IEmailSender _emailSender;

    public IndexModel(
        UserManager<CoachsBoxWebAppUser> userManager,
        SignInManager<CoachsBoxWebAppUser> signInManager,
        IEmailSender emailSender)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _emailSender = emailSender;
    }

    [Display(Name = "Имя пользователя")]
    public string Username { get; set; }

    public bool IsEmailConfirmed { get; set; }

    [TempData]
    public string StatusMessage { get; set; }

    [BindProperty]
    public InputModel Input { get; set; }

    public class InputModel
    {
      [Required]
      [EmailAddress]
      [Display(Name = "Эл. почта")]
      public string Email { get; set; }

      [Phone]
      [Display(Name = "Телефон")]
      public string PhoneNumber { get; set; }
    }

    public async Task<IActionResult> OnGetAsync()
    {
      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      var userName = await _userManager.GetUserNameAsync(user);
      var email = await _userManager.GetEmailAsync(user);
      var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

      Username = userName;

      Input = new InputModel
      {
        Email = email,
        PhoneNumber = phoneNumber
      };

      IsEmailConfirmed = await _userManager.IsEmailConfirmedAsync(user);

      return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }

      var email = await _userManager.GetEmailAsync(user);
      if (Input.Email != email)
      {
        var setEmailResult = await _userManager.SetEmailAsync(user, Input.Email);
        if (!setEmailResult.Succeeded)
        {
          var userId = await _userManager.GetUserIdAsync(user);
          throw new InvalidOperationException($"Unexpected error occurred setting email for user with ID '{userId}'.");
        }
      }

      var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
      if (Input.PhoneNumber != phoneNumber)
      {
        var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
        if (!setPhoneResult.Succeeded)
        {
          var userId = await _userManager.GetUserIdAsync(user);
          throw new InvalidOperationException($"Unexpected error occurred setting phone number for user with ID '{userId}'.");
        }
      }

      await _signInManager.RefreshSignInAsync(user);
      this.StatusMessage = "Ваш профиль обновлен";
      return RedirectToPage();
    }

    public async Task<IActionResult> OnPostSendVerificationEmailAsync()
    {
      if (!ModelState.IsValid)
      {
        return Page();
      }

      var user = await _userManager.GetUserAsync(User);
      if (user == null)
      {
        return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
      }


      var userId = await _userManager.GetUserIdAsync(user);
      var email = await _userManager.GetEmailAsync(user);
      var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
      var callbackUrl = Url.Page(
          "/Account/ConfirmEmail",
          pageHandler: null,
          values: new { userId = userId, code = code },
          protocol: Request.Scheme);
      await _emailSender.SendEmailAsync(
          email,
          "Подвердите вашу электронную почту",
          $"Пожалуйста, подтвердите учетную запись. Перейдите по <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>ссылке</a>." +
          $"<br />Если учетную запись в цифровой тренерской создавали не вы, то просто проигнорируйте это письмо.");

      this.StatusMessage = "Письмо с подтверждением отправлено. Пожалуйтса, проверьте вашу эл. почту.";
      return RedirectToPage();
    }
  }
}
