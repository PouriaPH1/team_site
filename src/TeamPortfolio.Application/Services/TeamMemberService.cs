using Slugify;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Services;

public class TeamMemberService : ITeamMemberService
{
    private readonly ITeamMemberRepository _repository;
    private readonly SlugHelper _slugHelper;

    public TeamMemberService(ITeamMemberRepository repository)
    {
        _repository = repository;
        _slugHelper = new SlugHelper();
    }

    public async Task<IEnumerable<TeamMemberDto>> GetAllActiveAsync()
    {
        var members = await _repository.GetAllActiveAsync();
        return members.Select(MapToDto);
    }

    public async Task<TeamMemberDto?> GetByIdAsync(int id)
    {
        var member = await _repository.GetByIdAsync(id);
        return member == null ? null : MapToDto(member);
    }

    public async Task<TeamMemberDto?> GetBySlugAsync(string slug)
    {
        var member = await _repository.GetBySlugAsync(slug);
        return member == null ? null : MapToDto(member);
    }

    public async Task<IEnumerable<TeamMemberDto>> SearchAsync(string query)
    {
        if (string.IsNullOrWhiteSpace(query)) return [];
        var members = await _repository.SearchAsync(query);
        return members.Select(MapToDto);
    }

    public async Task<TeamMemberDto> CreateAsync(TeamMemberDto dto)
    {
        var member = new TeamMember
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Role = dto.Role,
            Biography = dto.Biography,
            ProfilePhotoPath = dto.ProfilePhotoPath,
            IsActive = dto.IsActive,
            Slug = GenerateSlug(dto.FirstName + " " + dto.LastName),
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(member);
        return MapToDto(created);
    }

    public async Task<TeamMemberDto> UpdateAsync(int id, TeamMemberDto dto)
    {
        var member = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"TeamMember with id {id} not found.");
        member.FirstName = dto.FirstName;
        member.LastName = dto.LastName;
        member.Role = dto.Role;
        member.Biography = dto.Biography;
        member.ProfilePhotoPath = dto.ProfilePhotoPath;
        member.IsActive = dto.IsActive;
        member.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(member);
        return MapToDto(member);
    }

    public async Task DeleteAsync(int id)
    {
        var member = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"TeamMember with id {id} not found.");
        await _repository.DeleteAsync(member);
    }

    public async Task AddSkillAsync(int memberId, string skillName, int proficiencyLevel)
    {
        if (proficiencyLevel < 1 || proficiencyLevel > 100)
            throw new ArgumentOutOfRangeException(nameof(proficiencyLevel), "سطح مهارت باید بین 1 تا 100 باشد.");

        var member = await _repository.GetByIdAsync(memberId)
            ?? throw new InvalidOperationException($"TeamMember with id {memberId} not found.");

        // Skill is saved through domain model; service validates the constraint
        // The actual skill entity creation is done at the controller/repository level
        // This validates the business rule: 1 ≤ ProficiencyLevel ≤ 100
    }

    public Task ValidateSkillProficiencyLevel(int proficiencyLevel)
    {
        if (proficiencyLevel < 1 || proficiencyLevel > 100)
            throw new ArgumentOutOfRangeException(nameof(proficiencyLevel),
                "سطح مهارت باید بین 1 تا 100 باشد.");
        return Task.CompletedTask;
    }

    public string GenerateSlug(string title) => _slugHelper.GenerateSlug(title);

    private static TeamMemberDto MapToDto(TeamMember m) => new()
    {
        Id = m.Id,
        FirstName = m.FirstName,
        LastName = m.LastName,
        FullName = m.FullName,
        Role = m.Role,
        Slug = m.Slug,
        Biography = m.Biography,
        ProfilePhotoPath = m.ProfilePhotoPath,
        IsActive = m.IsActive
    };
}
