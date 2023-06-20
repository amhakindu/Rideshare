using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.Application.Contracts.Services;
public interface IResourceManager
{
    public Task<Uri> UploadPDF(IFormFile pdf);
}
