using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PWC.UI.Filters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PWC.UI.Base
{
    [NoCache]
    [BasicAuthenticationFilter()]
    //[SessionState(SessionStateBehavior.ReadOnly)]
    public class BaseApiController : Controller
    {

    }
}