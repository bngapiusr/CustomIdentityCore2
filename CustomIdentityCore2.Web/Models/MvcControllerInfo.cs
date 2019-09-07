using System.Collections.Generic;

namespace CustomIdentityCore2.Web.Services
{
    public class MvcControllerInfo
    {
        public string Id => $"{AreaName}:{Name}";
        public string Name { get; set; }
        public string DisplayName { get; set; }
        public string AreaName { get; set; }
        public IEnumerable<MvcControllerInfo> Actions { get; set; }

    }
}