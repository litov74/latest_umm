using System;
using API.Common.Enums;
using API.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public static class ModelBuilderExtensions
    {
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CompanyEntity>().HasData(new CompanyEntity
            {
                Id = "9881C482-63CB-40B9-9AE9-B60351CBC3DE",
                CompanyName = "NOIS",
                CreatedAt = new DateTime(),
                //IsActive = true,
            });

            modelBuilder.Entity<UserEntity>().HasData(new UserEntity
            {
                Id = "2911461D-B77E-E911-B2F3-0A645F4F4675",
                Email = "TestMay25@yopmail.com",
                Password = "AFEkr9ErV4EfvLMeLqwB3OK2/wO3UTKDIQ153lOz1grrxXnakYh2caoY2E7DYLxkzA==",
                IsEmailConfirm = true,
                FirstName = "Test",
                LastName = "May 25",
                CreatedDate = new DateTime()
            });

            modelBuilder.Entity<CompanyUserMappingEntity>().HasData(new CompanyUserMappingEntity
            {
                AccessLevel = CompanyAccessLevelEnum.Owner,
                CompanyId = "9881C482-63CB-40B9-9AE9-B60351CBC3DE",
                UserId = "2911461D-B77E-E911-B2F3-0A645F4F4675"
            });
        }
    }

}
