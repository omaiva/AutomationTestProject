using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.Json;
using AutomationTestProject.Application.Interfaces;

namespace AutomationTestProject.Controllers
{

    [Route("/[controller]")]
    public class AutomationController : ControllerBase
    {
        private readonly ILinkedInAutomationService _profilePictureService;
        private readonly IImageStorageService _imageStorageService;

        public AutomationController(ILinkedInAutomationService profilePictureService, IImageStorageService imageStorageService)
        {
            _profilePictureService = profilePictureService;
            _imageStorageService = imageStorageService;
        }

        [HttpGet("ProfilePicture")]
        public async Task<ActionResult> DownloadProfilePicture()
        {
            try
            {
                var imageUrl = await _profilePictureService.GetProfileImage();
                if (!string.IsNullOrEmpty(imageUrl))
                {
                    await _imageStorageService.SaveProfileImage(imageUrl);
                    return Redirect(imageUrl);
                }
                return NotFound("Profile picture not found.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error retrieving profile picture: " + ex.Message);
            }
        }
    }
}
