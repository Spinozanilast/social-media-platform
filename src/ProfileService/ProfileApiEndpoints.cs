namespace ProfileService;

public static class ProfileApiEndpoints
{
    
    private const string BaseEndpoint = "profiles/" + "{userId:guid}";
    
    public const string GetCountries = "profiles/countries";

    public static class ProfileImagesEndpoints
    {
        private const string ProfileImagesBase = BaseEndpoint + "/image";
        public const string Upload = $"{ProfileImagesBase}/upload";
        public const string Remove = $"{ProfileImagesBase}/remove";
        public const string Get = ProfileImagesBase;
    }

    public static class ProfileEndpoints
    {
        private const string ProfileEndpointBase = BaseEndpoint;

        public const string Get = ProfileEndpointBase;
        public const string Init = $"{ProfileEndpointBase}/initialize";
        public const string Update = $"{ProfileEndpointBase}/update";
        public const string Delete = $"{ProfileEndpointBase}/delete";
    }
}