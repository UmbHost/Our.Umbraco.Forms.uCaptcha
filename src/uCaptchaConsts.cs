﻿namespace Our.Umbraco.Forms.uCaptcha
{
    internal static class uCaptchaConsts
    {
        public static class hCaptcha
        {
            public static string JsResource => "https://hcaptcha.com/1/api.js";
            public static string LocalJsResource => "umbracoforms.hcaptcha.js";
            public static string LocalInvisibleJsResource => "umbracoforms.invisible.hcaptcha.js";
            public static string VerifyUrl => "https://hcaptcha.com/siteverify";
            public static string VerifyPostParameter => "h-captcha-response";
        }

        public static class reCaptcha
        {
            public static string JsResource => "https://www.google.com/recaptcha/api.js";
            public static string LocalJsResource => "umbracoforms.recaptcha.js";
            public static string LocalInvisibleJsResource => "umbracoforms.invisible.recaptcha.js";
            public static string VerifyUrl => "https://www.google.com/recaptcha/api/siteverify";
            public static string VerifyPostParameter => "g-recaptcha-response";
        }

        public static class Turnstile
        {
            public static string JsResource => "https://challenges.cloudflare.com/turnstile/v0/api.js?compat=recaptcha";
            public static string LocalJsResource => "umbracoforms.turnstile.js";
            public static string VerifyUrl => "https://challenges.cloudflare.com/turnstile/v0/siteverify";
            public static string VerifyPostParameter => "cf-turnstile-response";
        }

        public static string uCaptcha => "uCaptcha";
    }
}