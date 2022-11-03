﻿using _0_Framework.Application;
using _0_Framework.Infrastructure;
using AccountManagement.Application.Contracts.Role;
using AccountManagement.Domain.RoleAgg;
using AccountManagement.Infrastructure.EFCore.Context;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AccountManagement.Infrastructure.EFCore.Repository
{
    public class RoleRepository : RepositoryBase<int, Role>, IRoleRepository
    {
        private readonly AccountContext _accountContext;

        public RoleRepository(AccountContext accountContext) : base(accountContext)
        {
            _accountContext = accountContext;
        }

        public EditRole GetDetails(int id)
        {
            return _accountContext.Roles.Select(x => new EditRole
            {
                Id = x.Id,
                Name = x.Name
            }).First(x => x.Id == id);
        }

        public List<RoleViewModel> List()
        {
            return _accountContext.Roles.Select(x => new RoleViewModel()
            {
                Id = x.Id,
                Name = x.Name,
                CreationDate = x.CreationDate.ToFarsi(),
                IsDeleted = x.IsDeleted
            }).ToList();
        }

        public List<PermissionViewModel> GetAllPermissions()
        {
            return _accountContext.Permissions.Select(x => new PermissionViewModel()
            {
                PermissionId = x.Id,
                PermissionTitle = x.PermissionTitle,
                ParentId = x.ParentId
            }).ToList();
        }

        public bool AddPermissionsToRole(int roleId, List<int> permissions)
        {
            foreach (var perm in permissions)
            {
                try
                {
                    _accountContext.RolePermissions.Add(new RolePermissions()
                    {
                        PermissionId = perm,
                        RoleId = roleId
                    });
                }
                catch (Exception e)
                {
                    return false;
                }

            }

            _accountContext.SaveChanges();

            return true;
        }
    }
}
