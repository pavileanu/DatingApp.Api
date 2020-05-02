using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApi.Data;
using MyApi.Helpers;
using AutoMapper;
using Microsoft.Extensions.Options;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using System.Threading.Tasks;
using MyApi.Dtos;
using System.Security.Claims;
using MyApi.Models;
using System.Linq;

namespace MyApi.Controllers
{
    [Authorize]
    [Route("api/users/{userId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private readonly IDatingRepository _repo;
        private readonly IMapper _mapper;

        private readonly IOptions<CloudinarySettings> _cloudinaryConfig; 

        private readonly Cloudinary _cloudinary;
        public PhotosController(IDatingRepository repo, IMapper mapper, IOptions<CloudinarySettings> cloudinaryConfig)
        {
            _repo = repo;
            _mapper = mapper;
            _cloudinaryConfig = cloudinaryConfig;

            Account account = new Account(
                _cloudinaryConfig.Value.CloudName,
                _cloudinaryConfig.Value.APIKey,
                _cloudinaryConfig.Value.APISecret
            );

            _cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo =  await _repo.GetPhoto(id);

            var photoForReturn = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

            return Ok(photoForReturn);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotoUser(int userId, [FromForm]PhotoForCreationDbo photoForCreationDbo)
        {
            if(userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return Unauthorized();
            
            var userFromRepo = await _repo.GetUser(userId);

            var file = photoForCreationDbo.File;

            var uploadResult = new ImageUploadResult();

            if(file.Length > 0)
            {
                using(var stream = file.OpenReadStream())
                {
                    var uplodParams = new ImageUploadParams()
                    {
                        File = new FileDescription(file.Name, stream),
                        Transformation = new Transformation()
                            .Width(500).Height(500).Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uplodParams);
                }

                photoForCreationDbo.Url = uploadResult.Uri.ToString();
                photoForCreationDbo.PublicId = uploadResult.PublicId;

                var photo = _mapper.Map<Photo>(photoForCreationDbo);

                if(!userFromRepo.Photos.Any(u => u.isMain))
                    photo.isMain = true;
                
                userFromRepo.Photos.Add(photo);
                
                if(await _repo.SaveAll())
                {
                    //var photoForReturn = _mapper.Map<PhotoForReturnDto>(photo);
                    //return CreatedAtAction(nameof(GetPhoto), new { id = photo.Id }, photoForReturn);   
                    {
                        var photoFromRepo =  await _repo.GetPhoto(photo.Id);

                        var photoForReturn = _mapper.Map<PhotoForReturnDto>(photoFromRepo);

                        return Ok(photoForReturn);
                    }
                }            
                                
            }
            return BadRequest("Could not add the photo");
        }
        


    }
}