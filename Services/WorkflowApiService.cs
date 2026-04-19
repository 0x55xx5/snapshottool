using RegionSelector.Model;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace RegionSelector.Services;

public class WorkflowApiService : IWorkflowApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _workflowApiUrl;

    public WorkflowApiService(string apiUrl)
    {
        var handler = new HttpClientHandler
        {
            // 無論憑證發生什麼錯誤 (sslPolicyErrors)，都強制回傳 true 代表驗證通過
            ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => true
        };
        _httpClient = new HttpClient(handler);
        _workflowApiUrl = apiUrl;
    }

    public async Task UploadImageAsync(ScreenshotData data)
    {
        if (!File.Exists(data.ImageFilePath))
        {
            throw new FileNotFoundException("Image file not found.", data.ImageFilePath);
        }

        // Elsa 3.0 workflow accepting binary stream.
        // We will send it as a raw binary POST request.
        using var fileStream = File.OpenRead(data.ImageFilePath);
        using var streamContent = new StreamContent(fileStream);

        // You might need to adjust the Content-Type based on the explicit Elsa trigger requirement
        streamContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("image/png");
        try
        {

        }
        catch (System.Exception)
        {

            throw;
        }
        var internalApiUrl = _workflowApiUrl + $"?x={data.X}&y={data.Y}&w={data.Width}&h={data.Height}";// ?x=100&y=100&w=200&h=200
        var response = await _httpClient.PostAsync(internalApiUrl, streamContent);

        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to upload to workflow API. Status: {response.StatusCode}. Response: {content}");
        }
    }
}
