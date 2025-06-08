using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using Tasco.TaskService.Repository.Entities;
using Tasco.TaskService.Repository.UnitOfWork;

namespace Tasco.TaskService.Service
{
	public abstract class BaseService<T> where T : class
	{
		protected readonly IUnitOfWork<TaskManagementDbContext> _unitOfWork;
		protected readonly ILogger<T> _logger;
		protected readonly IMapper _mapper;
		protected readonly IHttpContextAccessor _httpContextAccessor;

		protected BaseService(
			IUnitOfWork<TaskManagementDbContext> unitOfWork,
			ILogger<T> logger,
			IMapper mapper,
			IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_logger = logger;
			_mapper = mapper;
			_httpContextAccessor = httpContextAccessor;
		}

		protected string GetUserIdFromJwt()
		{
			try
			{
				var userId = _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier")?.Value;
				if (string.IsNullOrEmpty(userId))
				{
					_logger.LogWarning("User ID not found in JWT token");
					return null;
				}
				return userId;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting user ID from JWT token");
				return null;
			}
		}

		protected string GetUserEmailFromJwt()
		{
			try
			{
				var email = _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress")?.Value;
				if (string.IsNullOrEmpty(email))
				{
					_logger.LogWarning("Email not found in JWT token");
					return null;
				}
				return email;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting email from JWT token");
				return null;
			}
		}

		protected string GetRoleFromJwt()
		{
			try
			{
				var role = _httpContextAccessor.HttpContext?.User?.FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
				if (string.IsNullOrEmpty(role))
				{
					_logger.LogWarning("Role not found in JWT token");
					return null;
				}
				return role;
			}
			catch (Exception ex)
			{
				_logger.LogError(ex, "Error getting role from JWT token");
				return null;
			}
		}
	}
}
