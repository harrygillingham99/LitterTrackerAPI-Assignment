﻿using System.Threading.Tasks;
using litter_tracker.Objects.ApiObjects;
using Microsoft.AspNetCore.Http;

namespace litter_tracker.Services.GoogleCloudStorage
{
    public interface IGoogleCloudStorage
    {
        Task UploadFile(string uploadedByUid, long markerDatastoreId, string base64Image);
    }
}