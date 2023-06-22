using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rideshare.UnitTests.Mocks;
public class MockIMG
{
    public static IFormFile GetMockImage()
        {
            var content = File.ReadAllBytes("../../../TestResources/test.png");
            var fileName = "test.png";

            IFormFile image = new FormFile(new MemoryStream(content), 0, content.Length, "id_from_form", fileName);
            return image;
        }

}
