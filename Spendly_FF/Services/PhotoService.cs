using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.Media;

namespace Spendly_FF.Services
{
    public class PhotoService : IPhotoService
    {
        public async Task<FileResult> CapturePhotoAsync()
        {
            // Engedélyek ellenőrzése és kérése...
            var status = await Permissions.RequestAsync<Permissions.Camera>();

            if (status == PermissionStatus.Granted && MediaPicker.Default.IsCaptureSupported)
            {
                return await MediaPicker.Default.CapturePhotoAsync();
            }
            return null;
        }
    }
}
