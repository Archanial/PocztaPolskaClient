﻿using System.Globalization;
using poczta.Sledzenie;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using ZXing.ImageSharp;

namespace poczta;

public sealed class PostApi(IPostClient client) : IPostApi
{
    private const string DateTimeFormat = "yyyy-MM-dd";
    
    public async Task<string> GetWelcomeMessage(string name)
    {
        var response = await client.GetWelcomeMessage(name);
        return response.@return;
    }

    public async Task<string> GetVersion()
    {
        var response = await client.GetVersion();
        return response.@return;
    }
    
    public async Task<Komunikat> CheckLocalShipments(string[] numbers)
    {
        var response = await client.CheckLocalShipments(numbers);
        return response.@return;
    }
    
    public async Task<Komunikat> CheckShipments(string[] numbers)
    {
        var response = await client.CheckShipments(numbers);
        return response.@return;
    }
    
    public async Task<Przesylka> CheckSingleShipment(string number)
    {
        var response = await client.CheckSingleShipment(number);
        return response.@return;
    }
    
    public async Task<Przesylka> CheckSingleLocalShipment(string number)
    {
        var response = await client.CheckSingleLocalShipment(number);
        return response.@return;
    }
    
    public async Task<Komunikat> CheckShipmentsByDate(string[] numbers, DateTime startDate, DateTime endDate)
    {
        var response = await client.CheckShipmentsByDate(numbers,
            startDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture),
            endDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
        return response.@return;
    }
    
    public async Task<Komunikat> CheckLocalShipmentsByDate(string[] numbers, DateTime startDate, DateTime endDate)
    {
        var response = await client.CheckLocalShipmentsByDate(numbers,
            startDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture),
            endDate.ToString(DateTimeFormat, CultureInfo.InvariantCulture));
        return response.@return;
    }
    
    public async Task<int> GetMaxShipments()
    {
        var response = await client.GetMaxShipments();
        return response.@return;
    }

    public async Task<Przesylka> GetSingleShipmentByBarCode(byte[] image)
    {
        var reader = new BarcodeReader<Rgba32>();
        using var ms = new MemoryStream(image);
        var bitmap = Image.Load<Rgba32>(image);
        var decodeResult = reader.Decode(bitmap);
        if (decodeResult == null)
            throw new ArgumentException("Cannot parse image");

        if (string.IsNullOrEmpty(decodeResult.Text))
            throw new ArgumentException("Empty or missing code");

        var response = await client.CheckSingleShipment(decodeResult.Text);
        return response.@return;
    }
}