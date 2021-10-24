using Microsoft.Extensions.DependencyInjection;
using Our.Umbraco.Forms.uCaptcha.Configuration;
using Our.Umbraco.Forms.uCaptcha.Extensions;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

namespace Our.Umbraco.Forms.uCaptcha.Composers
{
    public class uCaptchaComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder)
        {
            builder.Services.AddSingleton<uCaptchaSettings>();
            builder.AdduCaptcha();
        }
    }
}