namespace API.Configuration
{
    public class AppSettings
    {
        public CoreLogicSettingModel CoreLogicSettings { get; set; }
        public JwtSettingsModel JwtSettings { get; set; }
        public CryptographySettingsModel CryptographySettings { get; set; }
        public EmailSettingModel EmailSettings { get; set; }
        public StripeSettingsModel StripeSettings { get; set; }
        public ActiveCompaignsModel ActiveCompaignSettings { get; set; }

        public class JwtSettingsModel
        {
            public string Secret { get; set; }
            public int ExpiredInSeconds { get; set; }
            public int ExpiredForRememberMeInSeconds { get; set; }
            public string Algorithm { get; set; }
            public string Issuer { get; set; }
        }
        public class CryptographySettingsModel
        {
            public string AesPrivateKeyBase64 { get; set; }
        }
        public class CoreLogicSettingModel
        {
            public int EmailVerifyExpriedInSeconds { get; set; }
            public string FrontendUrl { get; set; }
            public string FrontendPathVerifyForgotPassword { get; set; }
            public string FrontendPathVerifyEmail { get; set; }
            public string SendgirdTemplateVerifyEmailId { get; set; }
            public string SendgirdTemplateRecoveryPasswordId { get; set; }
            public string GlobalAdminEmail { get; set; }
        }
        public class EmailSettingModel
        {
            public string SendGirdApiKey { get; set; }
            public string FromEmailAddress { get; set; }
            public bool useSendGrid { get; set; }
        }
        public class StripeSettingsModel
        {
            public string StripeApiKey { get; set; }
            public string StripePublicKey { get; set; }
            public string StripeApiSign { get; set; }
            public string StripeUserCountParamName { get; set; }
            public string StripeNewPlanMarkerParamName { get; set; }
        }
        public class ActiveCompaignsModel
        {
            public string ActiveCampaignApiUrl { get; set; }
            public string ActiveCampaignApiKey { get; set; }
            public int ActiveCampaignListTrialId { get; set; }
        }
    }
}
