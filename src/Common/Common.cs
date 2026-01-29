namespace Tailgrab.Common
{
    public static class Common
    {
        public const string ApplicationName = "Tailgrab";
        public const string CompanyName = "DeviousFox";
        public const string ConfigRegistryPath = "Software\\DeviousFox\\Tailgrab\\Config";

        // VRChat Web API registry keys
        public const string Registry_VRChat_Web_UserName = "VRCHAT_USERNAME";
        public const string Registry_VRChat_Web_Password = "VRCHAT_PASSWORD";
        public const string Registry_VRChat_Web_2FactorKey = "VRCHAT_2FA";

        // Ollama API registry keys and defaults
        public const string Registry_Ollama_API_Key = "OLLAMA_API_KEY";
        public const string Registry_Ollama_API_Endpoint = "OLLAMA_API_ENDPOINT";
        public const string Registry_Ollama_API_Prompt = "OLLAMA_API_PROMPT";
        public const string Registry_Ollama_API_Model = "OLLAMA_API_Model";
        public const string Default_Ollama_API_Prompt = "From the following block of text, classify the contents into a single class from the following classes; 'OK', 'Explicit Sexual', 'Harassment & Bullying', 'Self Harm'. If there is not enough information to determine the class, use a default of OK. When replying, return a single line for the Classification and a carriage return, then place the reasoning on subsequent lines: \n";
        public const string Default_Ollama_API_Endpoint = "https://ollama.com";
        public const string Default_Ollama_API_Model = "gemma3:27b";

        // Alert sound registry keys
        public const string Registry_Alert_Avatar = "ALERT_AVATAR_SOUND";
        public const string Registry_Alert_Group = "ALERT_GROUP_SOUND";
        public const string Registry_Alert_Profile = "ALERT_PROFILE_SOUND";

    }
}
