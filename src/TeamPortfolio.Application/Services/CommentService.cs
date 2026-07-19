using System.ComponentModel.DataAnnotations;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Application.Interfaces.Services;
using TeamPortfolio.Domain.Entities;
using TeamPortfolio.Domain.Enums;

namespace TeamPortfolio.Application.Services;

public class CommentService : ICommentService
{
    private readonly ICommentRepository _repository;

    public CommentService(ICommentRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<CommentDto>> GetApprovedForPostAsync(int postId)
    {
        var comments = await _repository.GetApprovedForPostAsync(postId);
        return comments.Select(MapToDto);
    }

    public async Task<IEnumerable<CommentDto>> GetPendingAsync()
    {
        var comments = await _repository.GetPendingAsync();
        return comments.Select(MapToDto);
    }

    public async Task<CommentDto> SubmitAsync(CommentDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.CommenterName))
            throw new ValidationException("نام الزامی است.");
        if (string.IsNullOrWhiteSpace(dto.CommenterEmail) || !IsValidEmail(dto.CommenterEmail))
            throw new ValidationException("فرمت ایمیل نامعتبر است.");
        if (string.IsNullOrWhiteSpace(dto.Body))
            throw new ValidationException("متن نظر الزامی است.");

        var comment = new Comment
        {
            CommenterName = dto.CommenterName.Trim(),
            CommenterEmail = dto.CommenterEmail.Trim(),
            Body = dto.Body.Trim(),
            PostId = dto.PostId,
            Status = CommentStatus.Pending,
            CreatedAt = DateTime.UtcNow
        };
        var created = await _repository.AddAsync(comment);
        return MapToDto(created);
    }

    public async Task ApproveAsync(int id)
    {
        var comment = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Comment {id} not found.");
        comment.Status = CommentStatus.Approved;
        comment.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(comment);
    }

    public async Task DeleteAsync(int id)
    {
        var comment = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Comment {id} not found.");
        await _repository.DeleteAsync(comment);
    }

    public async Task UpdateBodyAsync(int id, string newBody)
    {
        var comment = await _repository.GetByIdAsync(id)
            ?? throw new InvalidOperationException($"Comment {id} not found.");
        comment.Body = newBody;
        comment.UpdatedAt = DateTime.UtcNow;
        await _repository.UpdateAsync(comment);
    }

    private static bool IsValidEmail(string email)
    {
        try { var a = new System.Net.Mail.MailAddress(email); return a.Address == email.Trim(); }
        catch { return false; }
    }

    private static CommentDto MapToDto(Comment c) => new()
    {
        Id = c.Id,
        CommenterName = c.CommenterName,
        CommenterEmail = c.CommenterEmail,
        Body = c.Body,
        Status = c.Status.ToString(),
        PostId = c.PostId,
        CreatedAt = c.CreatedAt
    };
}
