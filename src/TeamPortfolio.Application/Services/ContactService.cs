using System.ComponentModel.DataAnnotations;
using TeamPortfolio.Application.DTOs;
using TeamPortfolio.Application.Interfaces.Repositories;
using TeamPortfolio.Domain.Entities;

namespace TeamPortfolio.Application.Services;

public class ContactService
{
    private readonly IContactMessageRepository _repository;

    public ContactService(IContactMessageRepository repository)
    {
        _repository = repository;
    }

    public async Task<ContactMessage> SubmitContactAsync(ContactMessageDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.FullName))
            throw new ValidationException("نام الزامی است.");
        if (string.IsNullOrWhiteSpace(dto.Email) || !IsValidEmail(dto.Email))
            throw new ValidationException("فرمت ایمیل نامعتبر است.");
        if (string.IsNullOrWhiteSpace(dto.Subject))
            throw new ValidationException("موضوع الزامی است.");
        if (string.IsNullOrWhiteSpace(dto.Body))
            throw new ValidationException("متن پیام الزامی است.");

        var message = new ContactMessage
        {
            FullName = dto.FullName.Trim(),
            Email = dto.Email.Trim(),
            Subject = dto.Subject.Trim(),
            Body = dto.Body.Trim(),
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };
        return await _repository.AddAsync(message);
    }

    private static bool IsValidEmail(string email)
    {
        try { var a = new System.Net.Mail.MailAddress(email); return a.Address == email.Trim(); }
        catch { return false; }
    }
}
