using API.Common.Enums;
using API.Database.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace API.Models.Entities
{
    public class UserEntity : BaseStringIdEntity
    {

        [MaxLength(254)]
        public string Email { get; set; }

        public bool IsEmailConfirm { get; set; }

        /// <summary>
        /// Field save a password hashed
        /// </summary>
        public string Password { get; set; }
        public bool IsBaseUserOfCompany { get; set; }

        [MaxLength(320)]
        public string FirstName { get; set; }

        [MaxLength(320)]
        public string LastName { get; set; }

        [MaxLength(15)]
        public string PhoneNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public string EmailVerifyCode { get; set; }
        public string PasswordForgotCode { get; set; }

        public virtual string FullName
        {
            get
            {
                return $"{FirstName} {LastName}";
            }
        }
        
        public virtual List<CompanyUserMappingEntity> CompanyMappings { get; set; }
        public string  SubscriptionTierId { get; set; }
        [ForeignKey("SubscriptionTierId")]
        public SubscriptionTierEntity SubscriptionTier { get; set; }

    }
}
