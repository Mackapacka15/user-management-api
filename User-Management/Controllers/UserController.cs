using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserManagement.Exceptions;
using UserManagement.Services;

namespace UserManagement.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet(Name = "GetUsers")]
        public ActionResult<IEnumerable<User>> Get()
        {
            return Ok(_userService.GetUsers());
        }

        [HttpGet("{name}", Name = "GetUser")]
        public ActionResult<User> GetUser(string name)
        {
            try
            {
                return Ok(_userService.GetUser(name));
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpPost(Name = "CreateUser")]
        public User Post([FromBody] User user)
        {
            try
            {
                return _userService.CreateUser(user);
            }
            catch (BadRequestException e)
            {
                throw new ArgumentException(e.Message);
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        [HttpPut("{name}", Name = "UpdateUser")]
        public ActionResult Put(string name, [FromBody] User user)
        {
            try
            {
                _userService.UpdateUser(name, user);
                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpDelete("{name}", Name = "DeleteUser")]
        public ActionResult Delete(string name)
        {
            try
            {
                _userService.DeleteUser(name);
                return Ok();
            }
            catch (NotFoundException e)
            {
                return NotFound(e.Message);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }
    }
}
