namespace Our.Umbraco.Forms.uCaptcha.Helpers
{
    internal static class Parse
    {
        public static bool Bool(string input)
        {
            if (input == null)
                return false;

            switch (input.ToLower())
            {
                case "":
                    return false;
                case " ":
                    return false;
                case "False":
                    return false;
                case "false":
                    return false;
                default:
                    return true;
            }
        }
    }
}