﻿using Microsoft.AspNetCore.Identity;

namespace VisualAcademy.Areas.Identity.Models;

/// <summary>
/// 사용자의 역할과 관련된 정보를 정의합니다.
/// </summary>
public class ApplicationRole : IdentityRole
{
    /// <summary>
    /// 역할에 대한 설명
    /// </summary>
    public string? Description { get; set; }
}
