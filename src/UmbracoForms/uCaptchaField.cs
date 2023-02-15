using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Our.Umbraco.Forms.uCaptcha.Configuration;
using Our.Umbraco.Forms.uCaptcha.Enums;
using Our.Umbraco.Forms.uCaptcha.Helpers;
using Our.Umbraco.Forms.uCaptcha.UmbracoForms.Models;
using Umbraco.Forms.Core;
using Umbraco.Forms.Core.Attributes;
using Umbraco.Forms.Core.Enums;
using Umbraco.Forms.Core.Models;
using Umbraco.Forms.Core.Services;

namespace Our.Umbraco.Forms.uCaptcha.UmbracoForms;

[Serializable]
public sealed class uCaptchaField : FieldType
{
    private readonly uCaptchaSettings _config;
    private readonly ILogger<uCaptchaField> _logger;
    private bool ishCaptcha;
    private bool isreCaptcha;
    private bool isTurnstile;

    public uCaptchaField(IOptionsMonitor<uCaptchaSettings> config, ILogger<uCaptchaField> logger)
    {
        _config = config.CurrentValue;
        _logger = logger;
        Id = new Guid("76fc6a38-4517-4fea-b928-9ff20c626adb");
        Name = "uCaptcha";
        Description = "hCaptcha or Google reCaptcha bot protection";
        Icon = "icon-eye";
        DataType = FieldDataType.Bit;
        SortOrder = 10;
        SupportsRegex = false;
        ishCaptcha = _config.Provider.Equals(Provider.Name.hCaptcha.ToString(), StringComparison.InvariantCultureIgnoreCase); 
        isreCaptcha = _config.Provider.Equals(Provider.Name.reCaptcha.ToString(), StringComparison.InvariantCultureIgnoreCase);
        isTurnstile = _config.Provider.Equals(Provider.Name.Turnstile.ToString(), StringComparison.InvariantCultureIgnoreCase);
    }

    public override string GetDesignView()
    {
        return "~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Backoffice/Common/FieldTypes/ucaptchafield.html";
    }

    [Setting("Show Label", Description = "Show the property label",
        View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/checkbox.html")]
    public string ShowLabel { get; set; }

    [Setting("Theme", Description = "uCaptcha theme", PreValues = "dark,light",
        View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/dropdownlist.html")]
    public string Theme { get; set; }

    [Setting("Size", Description = "uCaptcha size", PreValues = "normal,compact,invisible",
        View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/dropdownlist.html")]
    public string Size { get; set; }

    [Setting("reCaptcha Badge Position", Description = "Reposition the reCAPTCHA badge",
        PreValues = "bottomright,bottomleft,inline",
        View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/dropdownlist.html")]
    public string reCaptchaBadgePosition { get; set; }

    [Setting("Error Message",
        Description =
            "The error message to display when the user does not pass the uCaptcha check, the default message is: \"You must check the \"I am human\" checkbox to continue\"",
        View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/textfield.html")]
    public string ErrorMessage { get; set; }

    public override bool HideLabel => !Parse.Bool(ShowLabel);

    public override IEnumerable<string> RequiredJavascriptFiles(Field field)
    {
        var javascriptFiles = base.RequiredJavascriptFiles(field).ToList();
        if (isTurnstile)
        {
            javascriptFiles.Add(uCaptchaConsts.Turnstile.JsResource);
        }
        else
        {
            if (field.Settings.TryGetValue("Size", out var size) && size.Equals("invisible"))
            {
                if (ishCaptcha)
                {
                    javascriptFiles.Add(
                        $"~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Assets/{uCaptchaConsts.hCaptcha.LocalInvisibleJsResource}");
                }
                else if (isreCaptcha)
                {
                    javascriptFiles.Add(
                        $"~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Assets/{uCaptchaConsts.reCaptcha.LocalInvisibleJsResource}");
                }
            }
            else
            {
                if (ishCaptcha)
                {
                    javascriptFiles.Add(uCaptchaConsts.hCaptcha.JsResource);
                    javascriptFiles.Add(uCaptchaConsts.hCaptcha.LocalJsResource);
                }
                else if (isreCaptcha)
                {
                    javascriptFiles.Add(uCaptchaConsts.reCaptcha.JsResource);
                    javascriptFiles.Add(uCaptchaConsts.reCaptcha.LocalJsResource);
                }
            }
        }

        return javascriptFiles;
    }

    public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContext context,
        IPlaceholderParsingService placeholderParsingService, IFieldTypeStorage fieldTypeStorage, List<string> errors)
    {
        if (field.Values.Contains("false"))
        {
            string errorMessage;
            if (field.Settings.ContainsKey("ErrorMessage") && !string.IsNullOrEmpty(field.Settings["ErrorMessage"]))
            {
                errorMessage = field.Settings["ErrorMessage"];
            }
            else
            {
                errorMessage = "You must check the \"I am human\" checkbox to continue";
            }

            return new List<string> { errorMessage };
        }

        errors.AddRange(ValidateFieldWithCaptcha(field, context, errors));

        if (!errors.Any())
        {
            return base.ValidateField(form, field, postedValues, context, placeholderParsingService, fieldTypeStorage, errors);
        }

        return errors;
    }

    private List<string> ValidateFieldWithCaptcha(Field field, HttpContext context, List<string> errors)
    {
        if (!errors.Any())
        {
            if (string.IsNullOrWhiteSpace(_config.SecretKey))
            {
                string message =
                    "ERROR: uCaptcha is missing the Secret Key.  Please update the configuration to include a value at: " +
                    uCaptchaConsts.uCaptcha + ":SecretKey'";
                _logger.LogWarning(message);
                errors.Add(message);
            }

            string verifyUrl = null;
            string verifyPostParameter = null;
            if (ishCaptcha)
            {
                verifyUrl = uCaptchaConsts.hCaptcha.VerifyUrl;
                verifyPostParameter = uCaptchaConsts.hCaptcha.VerifyPostParameter;
            }
            else if (isreCaptcha)
            {
                verifyUrl = uCaptchaConsts.reCaptcha.VerifyUrl;
                verifyPostParameter = uCaptchaConsts.reCaptcha.VerifyPostParameter;
            }
            else if (isTurnstile)
            {
                verifyUrl = uCaptchaConsts.Turnstile.VerifyUrl;
                verifyPostParameter = uCaptchaConsts.Turnstile.VerifyPostParameter;
            }

            if (verifyUrl == null)
            {
                throw new Exception("\"uCaptchaProvider\" is missing or incorrect in AppSettings.");
            }

            string errorMessage;
            if (field.Settings.ContainsKey("ErrorMessage") && !string.IsNullOrEmpty(field.Settings["ErrorMessage"]))
            {
                errorMessage = field.Settings["ErrorMessage"];
            }
            else
            {
                errorMessage = "You must check the \"I am human\" checkbox to continue";
            }

            var secretKey = _config.SecretKey;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            client.DefaultRequestHeaders.Add("Accept", "*/*");

            var parameters = new List<KeyValuePair<string, string>>
            {
                new("response", context.Request.Form[verifyPostParameter]),
                new("secret", secretKey),
                new("remoteip",
                    context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString())
            };

            var request = new HttpRequestMessage(HttpMethod.Post, verifyUrl)
            {
                Content = new FormUrlEncodedContent(parameters)
            };

            var response = client.SendAsync(request).Result;

            if (response.IsSuccessStatusCode)
            {
                var jsonString = response.Content.ReadAsStringAsync();
                jsonString.Wait();

                var result = JsonConvert.DeserializeObject<uCaptchaVerifyResponse>(jsonString.Result);
                if (result is { Success: false })
                {
                    errors.Add(errorMessage);
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                errors.Add(errorMessage);
            }
        }

        return errors;
    }
}