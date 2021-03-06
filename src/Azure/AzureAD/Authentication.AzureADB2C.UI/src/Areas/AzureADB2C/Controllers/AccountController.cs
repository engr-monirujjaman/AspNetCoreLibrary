// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Microsoft.AspNetCore.Authentication.AzureADB2C.UI.AzureADB2C.Controllers.Internal;

[NonController]
[AllowAnonymous]
[Area("AzureADB2C")]
[Route("[area]/[controller]/[action]")]
[Obsolete("This is obsolete and will be removed in a future version. Use Microsoft.Identity.Web instead. See https://aka.ms/ms-identity-web.")]
internal class AccountController : Controller
{
    private readonly IOptionsMonitor<AzureADB2COptions> _options;

    public AccountController(IOptionsMonitor<AzureADB2COptions> AzureADB2COptions)
    {
        _options = AzureADB2COptions;
    }

    [HttpGet("{scheme?}")]
    public IActionResult SignIn([FromRoute] string scheme)
    {
        scheme = scheme ?? AzureADB2CDefaults.AuthenticationScheme;
        var redirectUrl = Url.Content("~/");
        return Challenge(
            new AuthenticationProperties { RedirectUri = redirectUrl },
            scheme);
    }

    [HttpGet("{scheme?}")]
    public IActionResult ResetPassword([FromRoute] string scheme)
    {
        scheme = scheme ?? AzureADB2CDefaults.AuthenticationScheme;
        var options = _options.Get(scheme);

        var redirectUrl = Url.Content("~/");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        properties.Items[AzureADB2CDefaults.PolicyKey] = options.ResetPasswordPolicyId;
        return Challenge(properties, scheme);
    }

    [HttpGet("{scheme?}")]
    public async Task<IActionResult> EditProfile([FromRoute] string scheme)
    {
        scheme = scheme ?? AzureADB2CDefaults.AuthenticationScheme;
        var authenticated = await HttpContext.AuthenticateAsync(scheme);
        if (!authenticated.Succeeded)
        {
            return Challenge(scheme);
        }

        var options = _options.Get(scheme);

        var redirectUrl = Url.Content("~/");
        var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
        properties.Items[AzureADB2CDefaults.PolicyKey] = options.EditProfilePolicyId;
        return Challenge(properties, scheme);
    }

    [HttpGet("{scheme?}")]
    public async Task<IActionResult> SignOut([FromRoute] string scheme)
    {
        scheme = scheme ?? AzureADB2CDefaults.AuthenticationScheme;
        var authenticated = await HttpContext.AuthenticateAsync(scheme);
        if (!authenticated.Succeeded)
        {
            return Challenge(scheme);
        }

        var options = _options.Get(scheme);

        var callbackUrl = Url.Page("/Account/SignedOut", pageHandler: null, values: null, protocol: Request.Scheme);
        return SignOut(
            new AuthenticationProperties { RedirectUri = callbackUrl },
            options.AllSchemes);
    }
}
