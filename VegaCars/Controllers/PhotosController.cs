using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VegaCars.Controllers.Resources;
using VegaCars.Core;
using VegaCars.Core.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace VegaCars.Controllers
{
    // /api/vehicles/1/photos/1
    [Route("/api/vehicles/{vehicleId}/photos")]
    public class PhotosController : Controller
    {
        private readonly int MAX_BYTES = 1 * 1024 * 1024;
        private readonly string[] ACCEPTED_FILE_TYPES = new[] { ".jpg", ".jpeg", ".png" };

        private readonly IHostingEnvironment host;
        private readonly IVehicleRepository repository;
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public PhotosController(IHostingEnvironment host, IVehicleRepository repository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.host = host;
            this.repository = repository;
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            //host.WebRootPath; // wwwroot
        }

        [HttpPost]
        public async Task<IActionResult> Upload(int vehicleId, IFormFile file)
        {
            var vehicle = await repository.GetVehicle(vehicleId, includeRelated: false);
            if(vehicle == null)
            {
                return NotFound();
            }

            if (file == null) return BadRequest("Null file"); 

            if (file.Length == 0) return BadRequest("Empty file");

            if (file.Length > MAX_BYTES) return BadRequest("Max file size exceeded");

            if (!ACCEPTED_FILE_TYPES.Any(s => s == Path.GetExtension(file.FileName))) return BadRequest("Invalid file type.");

            var uploadsFolderPath = Path.Combine(host.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }

            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadsFolderPath, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var photo = new Photo { FileName = fileName };
            vehicle.Photos.Add(photo);
            await unitOfWork.CompleteAsync();

            return Ok(mapper.Map<Photo, PhotoResource>(photo));
        }
    }
}
