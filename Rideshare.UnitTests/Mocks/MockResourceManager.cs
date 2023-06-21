using Microsoft.AspNetCore.Http;
using Moq;
using Rideshare.Application.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.UnitTests.Mocks;
public class MockResourceManager
{
    public static Mock<IResourceManager> GetResourceManager()
    {
        var mockService = new Mock<IResourceManager>();
        mockService.Setup(s => s.UploadPDF(It.IsAny<IFormFile>()))
            .ReturnsAsync((IFormFile pdf) =>
            {
                return new Uri($"http://cloudinary.com/{pdf.FileName}", UriKind.Absolute);
            });
        return mockService;
    }
}
