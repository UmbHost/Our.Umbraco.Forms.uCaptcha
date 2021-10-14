using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;
using UmbracoForms.uCaptcha.Configuration;
using UmbracoForms.uCaptcha.UmbracoForms;

namespace UmbracoForms.uCaptcha.Extensions
{
    public static class UmbracoBuilderExtensions
    {
        public static IUmbracoBuilder AdduCaptcha(this IUmbracoBuilder builder)
        {
            builder.WithCollectionBuilder<FieldCollectionBuilder>().Add<uCaptchaField>();
            builder.Services.Configure<uCaptchaSettings>((IConfiguration)builder.Config.GetSection(uCaptchaConsts.uCaptcha));
            return builder;
        }
    }
}
