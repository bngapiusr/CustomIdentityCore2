using System.Collections.Generic;

namespace CustomIdentityCore2.Web.Services
{
    public interface IMvcControllerDiscovery
    {
        IEnumerable<MvcControllerInfo> GetControllers();
    }
}