using FileManager.Models;
using FileManager.Services;
using Microsoft.AspNetCore.Mvc;

namespace FileManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FileController : Controller
    {
        private readonly FileService fileService;

        public FileController(FileService fileService)
        {
            this.fileService = fileService;
        }

        [HttpGet("drives")]
        public IActionResult GetDrives()
        {
            try
            {
                var drives = fileService.GetDrives();
                return Ok(drives);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении дисков:  {ex.Message}");
            }
        }

        [HttpGet("files")]
        public IActionResult GetDirectoryFile([FromQuery] string path = "")
        {
            try
            {
                if (string.IsNullOrEmpty(path))
                {
                    var drives = fileService.GetDrives().Select(d => new FileItemDTO
                    {
                        Name = d,
                        Path = d,
                        IsDirectory = true,
                        Size = 0,
                        LastUpdate = DateTime.Now
                    }).ToList();
                    return Ok(drives);
                }

                var items = fileService.GetDirectoryFile(path);
                return Ok(items);
            }
            catch (DirectoryNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка при получении файлов:  {ex.Message}");
            }
        }

        [HttpPost("move")]
        public IActionResult Move([FromQuery] string sourcePath, string targetPath)
        {
            if (string.IsNullOrWhiteSpace(sourcePath) || string.IsNullOrWhiteSpace(targetPath))
            {
                return BadRequest("Не указаны пути source и/или target");
            }

            try
            {
                fileService.Move(sourcePath, targetPath);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Ошибка перемещения: {ex.Message}");
            }
        }

        [HttpPost("delete")]
        public IActionResult Delete([FromQuery] string sourcePath)
        {
            if (string.IsNullOrEmpty(sourcePath))
            {
                return BadRequest("Не указан путь для удаления");
            }

            try
            {
                fileService.Delete(sourcePath);
                return Ok();
            }
            catch(Exception ex)
            {
                return StatusCode(500, $"Ошибка удаления: {ex.Message}");
            }
        }

    }
}
