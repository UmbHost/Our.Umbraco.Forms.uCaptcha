export const onInit = (_host, extensionRegistry) => {
  extensionRegistry.registerMany([
    {
      type: "formsFieldPreview",
      alias: "uCaptcha.FieldPreview.uCaptcha",
      name: "uCaptcha Field Preview",
      element: () =>
        import("./ucaptcha-field-preview.element.js"),
    },
  ]);
};
