using System;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure;

namespace src.Infrastructure.EF.Models;

public class Otp : BaseEntity
{
    public required string UserEmail { get; set; }
    public required string Code { get; set; }
    public required DateTime ExpiryAt { get; set; }
}