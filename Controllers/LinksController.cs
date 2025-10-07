using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoleManagementApi.Data;
using RoleManagementApi.Models;
using RoleManagementApi.Services;
using RoleManagementApi.Dtos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace RoleManagementApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LinksController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ILinkCheckerService _checker;

        public LinksController(ApplicationDbContext context, ILinkCheckerService checker)
        {
            _context = context;
            _checker = checker;
        }

        // ------------------- URL Endpoints -------------------

        [HttpPost]
        public async Task<ActionResult<LinkReadDto>> AddLink(LinkCreateDto dto)
        {
            var link = new Link
            {
                Url = dto.Url,
                Type = dto.Type,
                Status = "Unknown",
                LastCheckedAt = null
            };

            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            // Immediately check status
            var status = await _checker.CheckLinkStatusAsync(link.Url, link.Type);
            link.Status = status;
            link.LastCheckedAt = DateTime.UtcNow;

            _context.LinkChecks.Add(new LinkCheck
            {
                LinkId = link.LinkId,
                Status = status,
                CheckedAt = link.LastCheckedAt.Value,
                Message = status
            });

            await _context.SaveChangesAsync();

            return Ok(new LinkReadDto
            {
                LinkId = link.LinkId,
                Url = link.Url,
                Type = link.Type,
                Status = link.Status,
                LastCheckedAt = link.LastCheckedAt
            });
        }

        [HttpGet]
        public async Task<ActionResult<List<LinkReadDto>>> GetAll()
        {
            var links = await _context.Links
                .Where(l => l.Type != "File")
                .Select(l => new LinkReadDto
                {
                    LinkId = l.LinkId,
                    Url = l.Url,
                    Type = l.Type,
                    Status = l.Status ?? "Unknown",
                    LastCheckedAt = l.LastCheckedAt
                })
                .ToListAsync();

            return Ok(links);
        }

        // ------------------- File / Document Endpoints -------------------

        [HttpPost("file")]
        public async Task<ActionResult<LinkReadDto>> UploadFile([FromForm] FileUploadDto dto)
        {
            if (dto.File == null || dto.File.Length == 0)
                return BadRequest("No file uploaded.");

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            Directory.CreateDirectory(uploadsFolder);

            string uniqueFileName = $"{Guid.NewGuid()}_{dto.File.FileName}";
            string filePath = Path.Combine(uploadsFolder, uniqueFileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await dto.File.CopyToAsync(stream);
            }

            // ✅ Create absolute URL
            var baseUrl = $"{Request.Scheme}://{Request.Host}";
            var absoluteUrl = $"{baseUrl}/Uploads/{uniqueFileName}";

            var link = new Link
            {
                Type = "File",
                FileName = dto.File.FileName,
                FilePath = absoluteUrl, // store absolute path
                ContentType = dto.File.ContentType,
                Status = "Uploaded",
                LastCheckedAt = DateTime.UtcNow
            };

            _context.Links.Add(link);
            await _context.SaveChangesAsync();

            _context.LinkChecks.Add(new LinkCheck
            {
                LinkId = link.LinkId,
                Status = link.Status,
                CheckedAt = link.LastCheckedAt.Value,
                Message = link.Status
            });

            await _context.SaveChangesAsync();

            return Ok(new LinkReadDto
            {
                LinkId = link.LinkId,
                FileName = link.FileName,
                FilePath = link.FilePath, // already absolute
                ContentType = link.ContentType,
                Type = link.Type,
                Status = link.Status,
                LastCheckedAt = link.LastCheckedAt
            });
        }

        [HttpGet("documents")]
        public async Task<ActionResult<List<LinkReadDto>>> GetDocuments()
        {
            var baseUrl = $"{Request.Scheme}://{Request.Host}";

            var files = await _context.Links
                .Where(l => l.Type == "File")
                .ToListAsync();

            var result = files.Select(l => new LinkReadDto
            {
                LinkId = l.LinkId,
                FileName = l.FileName,
                // ✅ Ensure full absolute path returned
                FilePath = l.FilePath.StartsWith("http") ? l.FilePath : $"{baseUrl}{l.FilePath}",
                ContentType = l.ContentType,
                Type = l.Type,
                Status = l.Status ?? "Unknown",
                LastCheckedAt = l.LastCheckedAt
            }).ToList();

            return Ok(result);
        }

        [HttpGet("documents/{id}")]
        public async Task<IActionResult> DownloadDocument(int id)
        {
            var link = await _context.Links.FindAsync(id);
            if (link == null || link.Type != "File") return NotFound();

            string relativePath = link.FilePath.Contains("Uploads/")
                ? link.FilePath.Split("Uploads/").Last()
                : link.FilePath;

            string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", relativePath);
            if (!System.IO.File.Exists(path)) return NotFound();

            var contentType = link.ContentType ?? "application/octet-stream";
            var bytes = await System.IO.File.ReadAllBytesAsync(path);

            return File(bytes, contentType, link.FileName ?? "file");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLink(int id)
        {
            var link = await _context.Links.FindAsync(id);
            if (link == null) return NotFound();

            if (link.Type == "File" && !string.IsNullOrEmpty(link.FilePath))
            {
                string relativePath = link.FilePath.Contains("Uploads/")
                    ? link.FilePath.Split("Uploads/").Last()
                    : link.FilePath;

                string path = Path.Combine(Directory.GetCurrentDirectory(), "Uploads", relativePath);
                if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
            }

            _context.Links.Remove(link);
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
