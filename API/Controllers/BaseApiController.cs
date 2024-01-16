using API.Data;
using API.Entities;
using API.Helpers;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;

namespace API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Route("api/[controller]")]
    [ApiController]
    public class BaseApiController   : ControllerBase
    {

       

    }
}
