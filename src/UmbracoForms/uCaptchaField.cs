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

namespace Our.Umbraco.Forms.uCaptcha.UmbracoForms
{
    [Serializable]
    public sealed class uCaptchaField : FieldType
    {
        private readonly uCaptchaSettings _config;
        private readonly ILogger<uCaptchaField> _logger;
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
        }

        public override string GetDesignView()
        {
            return "~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Backoffice/Common/FieldTypes/ucaptchafield.html";
        }

        [Setting("Show Label", Description = "Show the property label", View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/checkbox.html")]
        public string ShowLabel { get; set; }

        [Setting("Theme", Description = "uCaptcha theme", PreValues = "dark,light", View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/dropdownlist.html")]
        public string Theme { get; set; }

        [Setting("Size", Description = "uCaptcha size", PreValues = "normal,compact,invisible", View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/dropdownlist.html")]
        public string Size { get; set; }

        [Setting("reCaptcha Badge Position", Description = "Reposition the reCAPTCHA badge", PreValues = "bottomright,bottomleft,inline", View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/dropdownlist.html")]
        public string reCaptchaBadgePosition { get; set; }

        [Setting("Error Message", Description = "The error message to display when the user does not pass the uCaptcha check, the default message is: \"You must check the \"I am human\" checkbox to continue\"", View = "~/App_Plugins/UmbracoForms/backoffice/Common/SettingTypes/textfield.html")]
        public string ErrorMessage { get; set; }

        public override bool HideLabel => !Parse.Bool(ShowLabel);

        public override IEnumerable<string> RequiredJavascriptFiles(Field field)
        {
            var javascriptFiles = base.RequiredJavascriptFiles(field).ToList();
            if (_config.Provider == Provider.Name.hCaptcha.ToString())
            {
                javascriptFiles.Add(uCaptchaConsts.hCaptcha.JsResource);
            }
            else if (_config.Provider == Provider.Name.reCaptcha.ToString())
            {
                javascriptFiles.Add(uCaptchaConsts.reCaptcha.JsResource);
            }

            if (field.Settings.ContainsKey("Size") && field.Settings["Size"] == "invisible")
            {
                if (_config.Provider == Provider.Name.hCaptcha.ToString())
                {
                    javascriptFiles.Add($"~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Assets/{uCaptchaConsts.hCaptcha.LocalInvisibleJsResource}");
                }
                else if (_config.Provider == Provider.Name.reCaptcha.ToString())
                {
                    javascriptFiles.Add($"~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Assets/{uCaptchaConsts.reCaptcha.LocalInvisibleJsResource}");
                }
            }
            else
            {
                if (_config.Provider == Provider.Name.hCaptcha.ToString())
                {
                    javascriptFiles.Add($"~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Assets/{uCaptchaConsts.hCaptcha.LocalJsResource}");
                }
                else if (_config.Provider == Provider.Name.reCaptcha.ToString())
                {
                    javascriptFiles.Add($"~/App_Plugins/Our.Umbraco.Forms.uCaptcha/Assets/{uCaptchaConsts.reCaptcha.LocalJsResource}");
                }
            }

            return javascriptFiles;
        }

        public override IEnumerable<string> ValidateField(Form form, Field field, IEnumerable<object> postedValues, HttpContext context, IPlaceholderParsingService placeholderParsingService)
        {
            if (string.IsNullOrWhiteSpace(_config.SecretKey))
            {
                string message = "ERROR: uCaptcha is missing the Secret Key.  Please update the configuration to include a value at: " + uCaptchaConsts.uCaptcha + ":SecretKey'";
                this._logger.LogWarning(message);
                return (IEnumerable<string>)new string[1]
                {
                    message
                };
            }
            string verifyUrl = null;
            string verifyPostParameter = null;
            if (_config.Provider == Provider.Name.hCaptcha.ToString())
            {
                verifyUrl = uCaptchaConsts.hCaptcha.VerifyUrl;
                verifyPostParameter = uCaptchaConsts.hCaptcha.VerifyPostParameter;
            }
            else if (_config.Provider == Provider.Name.reCaptcha.ToString())
            {
                verifyUrl = uCaptchaConsts.reCaptcha.VerifyUrl;
                verifyPostParameter = uCaptchaConsts.reCaptcha.VerifyPostParameter;
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
            var returnStrings = new List<string>();

            var secretKey = _config.SecretKey;

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
            client.DefaultRequestHeaders.Add("Accept", "*/*");

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("response", context.Request.Form[verifyPostParameter]),
                new KeyValuePair<string, string>("secret", secretKey),
                new KeyValuePair<string, string>("remoteip", context.Features.Get<IHttpConnectionFeature>()?.RemoteIpAddress?.ToString())
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

                if ( result is { Success: false })
                {
                    returnStrings.Add(errorMessage);
                }
            }

            if (!response.IsSuccessStatusCode)
            {
                returnStrings.Add(errorMessage);
            }

            return returnStrings;
        }
    }
}
