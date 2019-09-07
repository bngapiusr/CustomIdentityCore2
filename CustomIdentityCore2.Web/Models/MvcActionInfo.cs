namespace CustomIdentityCore2.Web.Models
{
    public class MvcActionInfo
    {
        public string Id => $"{ControllerId}:{Name}";
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string ControllerId { get; set; }
    }
}