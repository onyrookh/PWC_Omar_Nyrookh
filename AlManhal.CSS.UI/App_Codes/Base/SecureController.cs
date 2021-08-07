using PWC.UI.Filters;
using Microsoft.AspNetCore.Mvc;

namespace PWC.UI.Base
{
    [NoCache]
    [CheckUserSession]
    public class SecureController : BaseController
    {
    }
}