using System;
using System.Threading.Tasks;
using Microsoft.JSInterop;

namespace API_Document_Generator.Pages.Services
{
    public static class FileUtil
    {
        public async static Task SaveAs(IJSRuntime js, string filename, byte[] data)
        {
            await js.InvokeVoidAsync(
                "BlazorFileSaver.saveAsBase64",
                filename,
                Convert.ToBase64String(data),
                "application/msword"
                );
        }
    }
}