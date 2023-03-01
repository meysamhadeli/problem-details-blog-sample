using System.Net;
using Microsoft.AspNetCore.Mvc;
using WebApp;

[Route("api/[controller]/[action]")]
[ApiController]
public class Values2Controller : ControllerBase
{
    // api/values2/SimpleTest/0
    [HttpGet("{number}")]
    public IActionResult SimpleTest(int number)
    {
        if (number <= 0)
        {
            throw new Exception("The number is less than or equal to 0!");
        }

        return Ok(number);
    }
    
    // api/values2/CustomErrorTest/0
    [HttpGet("{number}")]
    public IActionResult CustomErrorTest(int number)
    {
        if (number <= 0)
        {
            var errorType = new ErrorFeature
            {
                Error = ErrorType.ArgumentException
            };
            HttpContext.Features.Set(errorType);
            return BadRequest();
        }

        return Ok(number);
    }
    
    // api/values2/CustomExceptionTest/0
    [HttpGet("{number}")]
    public IActionResult CustomExceptionTest(int number)
    {
        if (number <= 0)
        {
            throw new CustomException("Some custom exception!");
        }

        return Ok(number);
    }
}