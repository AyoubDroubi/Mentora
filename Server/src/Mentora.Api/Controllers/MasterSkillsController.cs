using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mentora.Application.Interfaces.Repositories;
using Mentora.Domain.Entities;

namespace Mentora.Api.Controllers
{
    /// <summary>
    /// Controller for Master Skills Library Management
    /// Provides CRUD operations for the shared skills repository
    /// Per SRS 3.3.2: Shared Skill Repository for system-wide analytics
    /// </summary>
    [ApiController]
    [Route("api/skills")]
    public class MasterSkillsController : ControllerBase
    {
        private readonly ISkillRepository _skillRepository;
        private readonly ILogger<MasterSkillsController> _logger;

        public MasterSkillsController(
            ISkillRepository skillRepository,
            ILogger<MasterSkillsController> logger)
        {
            _skillRepository = skillRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all skills from master library
        /// GET /api/skills
        /// Used for skill selection in user profiles
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> GetAllSkills([FromQuery] string? category = null, [FromQuery] string? search = null)
        {
            try
            {
                List<Skill> skills;

                // Filter by category if provided
                if (!string.IsNullOrEmpty(category) && Enum.TryParse<SkillCategory>(category, true, out var skillCategory))
                {
                    skills = await _skillRepository.GetByCategoryAsync(skillCategory);
                }
                else
                {
                    skills = await _skillRepository.GetAllAsync();
                }

                // Apply search filter if provided
                if (!string.IsNullOrEmpty(search))
                {
                    skills = skills.Where(s => 
                        s.Name.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        s.Description.Contains(search, StringComparison.OrdinalIgnoreCase)
                    ).ToList();
                }

                var result = skills.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    category = s.Category.ToString(),
                    description = s.Description
                });

                return Ok(new
                {
                    success = true,
                    data = result,
                    count = result.Count()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving master skills");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred while retrieving skills"
                });
            }
        }

        /// <summary>
        /// Get skill by ID
        /// GET /api/skills/{id}
        /// </summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(object), 200)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetSkillById(Guid id)
        {
            try
            {
                var skill = await _skillRepository.GetByIdAsync(id);

                if (skill == null)
                    return NotFound(new
                    {
                        success = false,
                        message = "Skill not found"
                    });

                return Ok(new
                {
                    success = true,
                    data = new
                    {
                        id = skill.Id,
                        name = skill.Name,
                        category = skill.Category.ToString(),
                        description = skill.Description
                    }
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving skill {id}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Get skills by category
        /// GET /api/skills/category/{category}
        /// </summary>
        [HttpGet("category/{category}")]
        [ProducesResponseType(typeof(IEnumerable<object>), 200)]
        public async Task<IActionResult> GetSkillsByCategory(string category)
        {
            try
            {
                if (!Enum.TryParse<SkillCategory>(category, true, out var skillCategory))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Invalid category: {category}. Valid categories are: {string.Join(", ", Enum.GetNames<SkillCategory>())}"
                    });
                }

                var skills = await _skillRepository.GetByCategoryAsync(skillCategory);

                var result = skills.Select(s => new
                {
                    id = s.Id,
                    name = s.Name,
                    category = s.Category.ToString(),
                    description = s.Description
                });

                return Ok(new
                {
                    success = true,
                    data = result,
                    count = result.Count()
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error retrieving skills for category {category}");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Get skill categories
        /// GET /api/skills/categories
        /// </summary>
        [HttpGet("categories")]
        [ProducesResponseType(typeof(IEnumerable<string>), 200)]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = Enum.GetNames<SkillCategory>()
                    .Select(c => new
                    {
                        name = c,
                        value = c
                    });

                return Ok(new
                {
                    success = true,
                    data = categories
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving skill categories");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }

        /// <summary>
        /// Create a new skill (Admin only - optional authorization)
        /// POST /api/skills
        /// </summary>
        [HttpPost]
        [Authorize] // Optional: Add [Authorize(Roles = "Admin")] if you want admin-only
        [ProducesResponseType(typeof(object), 201)]
        public async Task<IActionResult> CreateSkill([FromBody] CreateSkillDto dto)
        {
            try
            {
                // Check if skill already exists
                var existing = await _skillRepository.GetByNameAsync(dto.Name);
                if (existing != null)
                {
                    return Conflict(new
                    {
                        success = false,
                        message = "A skill with this name already exists"
                    });
                }

                // Parse category
                if (!Enum.TryParse<SkillCategory>(dto.Category, true, out var category))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = $"Invalid category. Valid categories are: {string.Join(", ", Enum.GetNames<SkillCategory>())}"
                    });
                }

                var skill = new Skill
                {
                    Name = dto.Name.Trim(),
                    Category = category,
                    Description = dto.Description ?? string.Empty
                };

                var created = await _skillRepository.CreateAsync(skill);

                return CreatedAtAction(
                    nameof(GetSkillById),
                    new { id = created.Id },
                    new
                    {
                        success = true,
                        message = "Skill created successfully",
                        data = new
                        {
                            id = created.Id,
                            name = created.Name,
                            category = created.Category.ToString(),
                            description = created.Description
                        }
                    });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating skill");
                return StatusCode(500, new
                {
                    success = false,
                    message = "An error occurred"
                });
            }
        }
    }

    /// <summary>
    /// DTO for creating a new skill
    /// </summary>
    public class CreateSkillDto
    {
        public string Name { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string? Description { get; set; }
    }
}
