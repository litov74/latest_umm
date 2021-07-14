using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API.Controllers.Models.Company
{
    public class UserViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool IsBaseUserOfCompany { get; set; }
    }
    public class UserVm
    {
        public string search { get; set; }
        public int page { get; set; }
        public int length { get; set; }
    }

    public class UserAddVm
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool DisableNotifications { get; set; }
        public string DivisionHierarchyItemId { get; set; }
        public string SubDivisionHierarchyItemId { get; set; }
        public string TeamHierarchyItemId { get; set; }
        public string CompanyHierarchyItemId { get; set; }
        public bool IsChangingPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public bool IsFirstLogin { get; set; }

        public string Role { get; set; }
    }
    public class UserVmNew
    {
        public Guid id { get; set; }
        public string returnUrl { get; set; }
        public Guid moveToId { get; set; }
    }
    public class UserValidationVm
    {
        public Guid Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public Guid? MoveToEntityId { get; set; }

        public Guid? UserId { get; set; }
    }
    public class _UserListViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        public bool IsBaseUserOfCompany { get; set; }

        public bool isAdmin { get; set; }
        public string Division { get; set; }
    }

    public class UpdateUserTypeModel
    {
        // public List<Guid> UserId { get; set; }
        // public bool isAdmin { get; set; }
        public List<IdAndIsAdmin> dataForUpdate { get; set; }
    }

    public class IdAndIsAdmin
    {
        public Guid Id { get; set; }
        public bool IsAdmin { get; set; }
        public string FirstName { get; set; }
    }

    public class UserValidationResponseVm
    {
        public bool Response { get; set; }
        public string Email { get; set; }
    }

    public class AdminUserAddViewModel
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public bool IsAdmin { get; set; }
        public bool DisableNotifications { get; set; }
        public string DivisionHierarchyItemId { get; set; }
        public string SubDivisionHierarchyItemId { get; set; }
        public string TeamHierarchyItemId { get; set; }
        public string CompanyHierarchyItemId { get; set; }
        public bool IsChangingPassword { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string ConfirmNewPassword { get; set; }
        public bool IsFirstLogin { get; set; }

        public Guid CompanyId { get; set; }
    }
}
