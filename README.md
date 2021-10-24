# Our.Umbraco.Forms.uCaptcha

A simple to use and integrate captcha plugin for Umbraco Forms which supports both hCaptcha and reCaptcha.

## Getting started

This package is supported on Umbraco 9.0.1+ and Umbraco Forms 9.0.1+.

### Installation

UmbracoForms.uCaptcha is available from [NuGet](https://www.nuget.org/packages/Our.Umbraco.Forms.uCaptcha) or as a manual download directly from GitHub.

    dotnet add package Our.Umbraco.Forms.uCaptcha

## Usage

This package adds both hCaptcha or reCaptcha to Umbraco Forms, configurable in the appsettings.json.

It has the ability to choose from the traditional checkbox or invisible options.

This is tested with **Umbraco V9.0.1** and **Umbraco Forms 9.0.1**

## Getting Started

Before you begin you will need to get your API keys from your preferred provider, you can get these from the links below:

[hCaptcha](https://hCaptcha.com/?r=0d16470cad8d)

[reCaptcha](https://www.google.com/recaptcha/about/)

You will need to add the following settings to your `appsettings.json`

      "uCaptcha": {
        "SiteKey": "YOUR SITE KEY",
        "SecretKey": "YOUR SECRET KEY",
        "Provider": "YOUR CHOSEN PROVIDER"
      }

To select the provider you will need to change the following setting to your `appsettings.json`
    
    "Provider": "YOUR CHOSEN PROVIDER"

The choices are either `hCaptcha` or `reCaptcha`

### Contribution guidelines

To raise a new bug, create an issue on the GitHub repository. To fix a bug or add new features, fork the repository and send a pull request with your changes. Feel free to add ideas to the repository's issues list if you would to discuss anything related to the package.

## License

Copyright &copy; 2021 [UmbHost Limited](https://umbhost.net/).

Licensed under the MIT License.
