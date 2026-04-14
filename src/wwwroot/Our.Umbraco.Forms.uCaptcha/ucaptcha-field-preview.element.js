import { css, html } from "@umbraco-cms/backoffice/external/lit";
import { UmbLitElement } from "@umbraco-cms/backoffice/lit-element";

class uCaptchaFieldPreviewElement extends UmbLitElement {
  render() {
    return html`<div class="preview">
      <img
        src="/App_Plugins/Our.Umbraco.Forms.uCaptcha/Images/uCaptcha.png"
        alt="uCaptcha"
      />
    </div>`;
  }

  static styles = css`
    .preview {
      display: flex;
      align-items: center;
      justify-content: center;
      padding: var(--uui-size-4);
    }
    img {
      max-height: 90px;
    }
  `;
}

customElements.define("ucaptcha-field-preview", uCaptchaFieldPreviewElement);

export default uCaptchaFieldPreviewElement;
