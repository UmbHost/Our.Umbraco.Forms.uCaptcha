# Our.Umbraco.Forms.uCaptcha

A simple to use and integrate captcha plugin for Umbraco Forms which supports [hCaptcha](https://www.hcaptcha.com/), [reCaptcha](https://developers.google.com/recaptcha/) and [Cloudflare Turnstile](https://www.cloudflare.com/products/turnstile/).

## Getting started

This package is supported on Umbraco 10+ and Umbraco Forms 10+.

This package has been tested on Umbraco 11 and Umbraco Forms 11, however only LTS versions of Umbraco and Umbraco Forms are officially supported.

## Breaking Changes:

### V4.0.0 15 February 2023:

Cloudflare Turnstile has been reworked to use a signle javascript file removing `umbracoforms.invisible.turnstile.js`
Cloudflare Turnstile has been reworked to use the `cf-` prefix instead of `g-`
All javascript has been rewritten to remove jQuery dependency

### Installation

UmbracoForms.uCaptcha is available from [NuGet](https://www.nuget.org/packages/Our.Umbraco.Forms.uCaptcha) or as a manual download directly from GitHub.

    dotnet add package Our.Umbraco.Forms.uCaptcha

## Usage

This package adds hCaptcha, reCaptcha or Turnstile to Umbraco Forms, configurable in the appsettings.json.

It has the ability to choose from the traditional checkbox or invisible options.


## Getting Started

Before you begin you will need to get your API keys from your preferred provider, you can get these from the links below:

[hCaptcha](https://hCaptcha.com/?r=0d16470cad8d)

[reCaptcha](https://www.google.com/recaptcha/about/)

[Turnstile](https://developers.cloudflare.com/turnstile/)

You will need to add the following settings to your `appsettings.json`

      "uCaptcha": {
        "SiteKey": "YOUR SITE KEY",
        "SecretKey": "YOUR SECRET KEY",
        "Provider": "YOUR CHOSEN PROVIDER"
      }

To select the provider you will need to change the following setting to your `appsettings.json`
    
    "Provider": "YOUR CHOSEN PROVIDER"

The choices are either `hCaptcha` or `reCaptcha` or `Turnstile`

# Required theme changes

You need to ensure you have included `@Html.RenderUmbracoFormDependencies()` in your site:
https://docs.umbraco.com/umbraco-forms/developer/prepping-frontend

Alternatively you can load the packages script dependencies using the method below in your themes `Render.cshtml` around line 60/61

```        
@if (Model.CurrentPage.JavascriptCommands.Any())
{
    <script>
    document.addEventListener("DOMContentLoaded", function() {
        @foreach (var javascriptCommand in Model.CurrentPage.JavascriptCommands)
        {
            @Html.Raw(javascriptCommand)
        }
    });
</script>
}
```

If you are using the default theme you will need to create a custom theme using the zip found in the documentation here:
https://docs.umbraco.com/umbraco-forms/developer/themes

You can change the default theme used by Umbraco in your appsettings.json file
```
"Forms": {
    "FormDesign": {
      "DefaultTheme": "default",
    }
}
```
Reference:
https://docs.umbraco.com/umbraco-forms/developer/configuration

### Turnstile configuration

The invisible option in the Settings Size dropdown does not do anything when using Cloudflare Turnstile, the invisible mode is controlled by the SiteKey.

### Contribution guidelines

To raise a new bug, create an issue on the GitHub repository. To fix a bug or add new features, fork the repository and send a pull request with your changes. Feel free to add ideas to the repository's issues list if you would to discuss anything related to the package.

## License

Copyright &copy; 2023 [UmbHost Limited](https://umbhost.net/).

Licensed under the MIT License.
