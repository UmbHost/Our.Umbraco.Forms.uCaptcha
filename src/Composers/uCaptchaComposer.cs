using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using UmbracoForms.uCaptcha.Extensions;

namespace UmbracoForms.uCaptcha.Composers
{
    public class uCaptchaComposer : IComposer
    {
        public void Compose(IUmbracoBuilder builder) => builder.AdduCaptcha();
    }
}