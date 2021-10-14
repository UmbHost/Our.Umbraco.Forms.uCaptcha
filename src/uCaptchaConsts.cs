namespace UmbracoForms.uCaptcha
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

        public static string uCaptcha => "uCaptcha";
    }
}