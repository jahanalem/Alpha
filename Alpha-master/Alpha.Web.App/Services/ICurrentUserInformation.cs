using Alpha.Infrastructure;

namespace Alpha.Web.App.Services
{
    public interface ICurrentUserInformation
    {
        CurrentUser GetCurrentUserInfo();
    }
}