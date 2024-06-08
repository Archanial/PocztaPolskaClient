using System.Text;
using Microsoft.AspNetCore.Mvc;
using poczta.Sledzenie;

namespace poczta;

[Controller]
[HATEOSFilter]
[Route("poczta")]
public sealed class PocztaController(IPostApi postApi) : ControllerBase
{
    [HttpGet("welcome")]
    public async Task<IActionResult> GetWelcomeMessage(string name)
    {
        if(string.IsNullOrEmpty(name))
            return BadRequest("Name is required");

        var result = await postApi.GetWelcomeMessage(name);
        if (string.IsNullOrEmpty(result))
            return NoContent();
        
        return Ok(new Response<string>
        {
            Content = result
        });
    }
    
    [HttpGet("version")]
    public async Task<IActionResult> GetVersion()
    {
        var result = await postApi.GetVersion();
        if (string.IsNullOrEmpty(result))
            return NoContent();
        
        return Ok(new Response<string>
        {
            Content = result
        });
    }
    
    [HttpGet("checkShipments")]
    public async Task<IActionResult> CheckShipments(string[]? numbers)
    {
        if(numbers == null || numbers.Length == 0)
            return BadRequest("Numbers are required");

        var result = await postApi.CheckShipments(numbers);
        if (result.przesylki.Length == 0)
            return NotFound();
        
        return Ok(new Response<Komunikat>
        {
            Content = result
        });
    }
    
    [HttpGet("checkLocalShipments")]
    public async Task<IActionResult> CheckLocalShipments(string[]? numbers)
    {
        if(numbers == null || numbers.Length == 0)
            return BadRequest("Numbers are required");

        var result = await postApi.CheckLocalShipments(numbers);
        if (result.przesylki.Length == 0)
            return NotFound();
        
        return Ok(new Response<Komunikat>
        {
            Content = result
        });
    }
    
    [HttpGet("checkSingleShipment")]
    public async Task<IActionResult> CheckSingleShipment(string number)
    {
        if(string.IsNullOrEmpty(number))
            return BadRequest("Number is required");

        var result = await postApi.CheckSingleShipment(number);
        if (result.danePrzesylki == null)
            return NotFound();
        
        return Ok(new Response<Przesylka>
        {
            Content = result
        });
    }
    
    [HttpGet("checkSingleLocalShipment")]
    public async Task<IActionResult> CheckSingleLocalShipment(string number)
    {
        if(string.IsNullOrEmpty(number))
            return BadRequest("Number is required");

        var result = await postApi.CheckSingleLocalShipment(number);
        if (result.danePrzesylki == null)
            return NotFound();
        
        return Ok(new Response<Przesylka>
        {
            Content = result
        });
    }
  
    [HttpPost("checkShipmentsByDate")]
    public async Task<IActionResult> CheckShipmentsByDate([FromBody]ByDateRequest? request)
    {
        if(request?.Numbers == null || request.Numbers.Length == 0)
            return BadRequest("At least one number is required");

        var result = await postApi.CheckShipmentsByDate(request.Numbers, request.StartDate, request.EndDate);
        if (result.przesylki.Length == 0)
            return NotFound();
        
        return Ok(new Response<Komunikat>
        {
            Content = result
        });
    }
   
    [HttpPost("checkLocalShipmentsByDate")]
    public async Task<IActionResult> CheckLocalShipmentsByDate([FromBody]ByDateRequest? request)
    {
        if(request?.Numbers == null || request.Numbers.Length == 0)
            return BadRequest("At least one number is required");

        var result = await postApi.CheckLocalShipmentsByDate(request.Numbers, request.StartDate, request.EndDate);
        if (result.przesylki.Length == 0)
            return NotFound();
        
        return Ok(new Response<Komunikat>
        {
            Content = result
        });
    }
    
    [HttpGet("getMaxShipments")]
    public async Task<IActionResult> GetMaxShipments()
    {
        var result = await postApi.GetMaxShipments();
        if (result == default)
            return NotFound();
        
        return Ok(new Response<int>
        {
            Content = result
        });
    }
    
    [HttpPost("getSingleShipmentByBarCode")]
    public async Task<IActionResult> GetSingleShipmentByBarCode([FromBody]ByBarcodeRequest? request)
    {
        if(request?.ImageData == null || request.ImageData.Length == 0)
            return BadRequest("Image data is required");

        if (request.ImageData.StartsWith("data:image/jpeg;base64,"))
            request.ImageData = request.ImageData[23..];
        
        byte[] bytes;
        try
        {
            bytes = Convert.FromBase64String(request.ImageData);
        }
        catch
        {
            return BadRequest("Image data is corrupted");
        }
        
        var result = await postApi.GetSingleShipmentByBarCode(bytes);
        if (result.danePrzesylki == null)
            return NotFound();
        
        return Ok(new Response<Przesylka>
        {
            Content = result
        });
    }
}