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
    
    [HttpPost("checkShipments")]
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
    
    [HttpPost("checkLocalShipments")]
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
    
    [HttpPost("checkSingleLocalShipment")]
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
    public async Task<IActionResult> CheckShipmentsByDate(string[]? numbers, DateTime startDate, DateTime endDate)
    {
        if(numbers == null || numbers.Length == 0)
            return BadRequest("At least one number is required");

        var result = await postApi.CheckShipmentsByDate(numbers, startDate, endDate);
        if (result.przesylki.Length == 0)
            return NotFound();
        
        return Ok(new Response<Komunikat>
        {
            Content = result
        });
    }
   
    [HttpPost("checkLocalShipmentsByDate")]
    public async Task<IActionResult> CheckLocalShipmentsByDate(string[]? numbers, DateTime startDate, DateTime endDate)
    {
        if(numbers == null || numbers.Length == 0)
            return BadRequest("At least one number is required");

        var result = await postApi.CheckLocalShipmentsByDate(numbers, startDate, endDate);
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
    public async Task<IActionResult> GetSingleShipmentByBarCode(byte[]? imageData)
    {
        if(imageData == null || imageData.Length == 0)
            return BadRequest("Image data is required");

        var result = await postApi.GetSingleShipmentByBarCode(imageData);
        if (result.danePrzesylki == null)
            return NotFound();
        
        return Ok(new Response<Przesylka>
        {
            Content = result
        });
    }
}