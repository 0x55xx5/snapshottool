using RegionSelector.Model;
using System.Threading.Tasks;

namespace RegionSelector.Services;

public interface IWorkflowApiService
{
    /// <summary>
    /// Uploads an image file to the Elsa 3.0 workflow API.
    /// </summary>
    Task UploadImageAsync(ScreenshotData data);
}
