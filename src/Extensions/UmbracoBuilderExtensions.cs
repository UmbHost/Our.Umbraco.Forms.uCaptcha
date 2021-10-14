using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Forms.uCaptcha.Configuration;
using Our.Umbraco.Forms.uCaptcha.UmbracoForms;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Forms.Core.Providers;

namespace Our.Umbraco.Forms.uCaptcha.Extensions
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
