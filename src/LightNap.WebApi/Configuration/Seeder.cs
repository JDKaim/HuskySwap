﻿using LightNap.Core.ClassDesires.Interfaces;
using LightNap.Core.ClassDesires.Request.Dto;
using LightNap.Core.ClassInfos.Interfaces;
using LightNap.Core.ClassInfos.Request.Dto;
using LightNap.Core.ClassUsers.Interfaces;
using LightNap.Core.ClassUsers.Request.Dto;
using LightNap.Core.Configuration;
using LightNap.Core.Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Data;
using System.Runtime.CompilerServices;

namespace LightNap.WebApi.Configuration
{
    /// <summary>
    /// Class responsible for seeding roles and administrators.
    /// </summary>
    public static class Seeder
    {
        /// <summary>
        /// Seeds application content for development purposes.
        /// </summary>
        /// <param name="services">The service provider.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedDevelopmentContentAsync(
        // Suppress IDE0060 warning for unused parameter 'services'. Remove this when actually using the parameter.
#pragma warning disable IDE0060
        IServiceProvider services
#pragma warning restore IDE0060
            )
        {

            var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
            var logger = services.GetRequiredService<ILogger<string>>();
            var classService = services.GetRequiredService<IClassInfoService>();
            var classUserService = services.GetRequiredService<IClassUserService>();
            var classDesireService = services.GetRequiredService<IClassDesireService>();

            var user1 = await Seeder.GetOrCreateUserAsync(userManager, "user1", "user1@user.com", "P@ssw0rd", false, logger);
            var user2 = await Seeder.GetOrCreateUserAsync(userManager, "user2", "user2@user.com", "P@ssw0rd", false, logger);
            var user3 = await Seeder.GetOrCreateUserAsync(userManager, "user3", "user3@user.com", "P@ssw0rd", false, logger);
            var user4 = await Seeder.GetOrCreateUserAsync(userManager, "user4", "user4@user.com", "P@ssw0rd", false, logger);
            var class403 = await classService.CreateClassInfoAsync(new CreateClassInfoDto() { ClassCode = "123", Title = "CSE 403", Description = "ASD", Instructor = "Teacher", Notes = "asd" });
            var class473 = await classService.CreateClassInfoAsync(new CreateClassInfoDto() { ClassCode = "312", Title = "CSE 473", Description = "SAD", Instructor = "Professor", Notes = "asd" });
            var class333 = await classService.CreateClassInfoAsync(new CreateClassInfoDto() { ClassCode = "123", Title = "CSE 333", Description = "AASDSD", Instructor = "Teacher", Notes = "asd" });
            var class413 = await classService.CreateClassInfoAsync(new CreateClassInfoDto() { ClassCode = "312", Title = "CSE 413", Description = "AD", Instructor = "Professor", Notes = "asd" });
            var user1class403 = await classUserService.CreateClassUserAsync(new CreateClassUserDto() { ClassId = class403.Result!.Id, UserId = user1.Id });
            var user2class473 = await classUserService.CreateClassUserAsync(new CreateClassUserDto() { ClassId = class473.Result!.Id, UserId = user2.Id });
            await classDesireService.CreateClassDesireAsync(new CreateClassDesireDto() { ClassId = class473.Result!.Id, UserId = user1.Id });
            await classDesireService.CreateClassDesireAsync(new CreateClassDesireDto() { ClassId = class403.Result!.Id, UserId = user2.Id });

        }

        /// <summary>
        /// Seeds the roles in the application.
        /// </summary>
        /// <param name="roleManager">The role manager.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedRolesAsync(RoleManager<ApplicationRole> roleManager, ILogger logger)
        {
            foreach (ApplicationRole role in ApplicationRoles.All)
            {
                if (!await roleManager.RoleExistsAsync(role.Name!))
                {
                    var result = await roleManager.CreateAsync(role);
                    if (!result.Succeeded)
                    {
                        throw new ArgumentException($"Unable to create role '{role.Name}': {string.Join("; ", result.Errors.Select(error => error.Description))}");
                    }
                    logger.LogInformation("Added role '{roleName}'", role.Name);
                }
            }

            var roleSet = new HashSet<string>(ApplicationRoles.All.Select(role => role.Name!), StringComparer.OrdinalIgnoreCase);

            foreach (var role in roleManager.Roles.Where(role => role.Name != null && !roleSet.Contains(role.Name)))
            {
                var result = await roleManager.DeleteAsync(role);
                if (!result.Succeeded)
                {
                    throw new ArgumentException($"Unable to remove role '{role.Name}': {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }
                logger.LogInformation("Removed role '{roleName}'", role.Name);
            }
        }

        /// <summary>
        /// Seeds the administrators in the application.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="administratorConfigurations">The administrators to create and promote.</param>
        /// <param name="applicationSettings">Settings for the application.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task SeedAdministratorsAsync(UserManager<ApplicationUser> userManager, IOptions<List<AdministratorConfiguration>> administratorConfigurations,
            IOptions<ApplicationSettings> applicationSettings, ILogger logger)
        {
            if (administratorConfigurations.Value is null) { return; }

            foreach (var administrator in administratorConfigurations.Value)
            {
                ApplicationUser user = await Seeder.GetOrCreateUserAsync(userManager, administrator.UserName, administrator.Email, administrator.Password,
                    applicationSettings.Value.RequireTwoFactorForNewUsers, logger);
                await Seeder.AddUserToRole(userManager, user, ApplicationRoles.Administrator.Name!, logger);
            }
        }

        /// <summary>
        /// Creates a new user in the application.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="userName">The user name.</param>
        /// <param name="email">The email address.</param>
        /// <param name="password">The password.</param>
        /// <param name="requireTwoFactor">Indicates whether two-factor authentication is required.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public static async Task<ApplicationUser> GetOrCreateUserAsync(UserManager<ApplicationUser> userManager, string userName, string email, string? password, bool requireTwoFactor, ILogger logger)
        {
            ApplicationUser? user = await userManager.FindByEmailAsync(email);

            if (user is null)
            {
                user = new ApplicationUser(userName, email, requireTwoFactor);

                bool passwordProvided = !string.IsNullOrWhiteSpace(password);
                string passwordToSet = passwordProvided ? password! : $"P@ssw0rd{Guid.NewGuid()}";

                var result = await userManager.CreateAsync(user, passwordToSet);
                if (!result.Succeeded)
                {
                    throw new ArgumentException($"Unable to create user '{userName}' ('{email}'): {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }

                logger.LogInformation("Created user '{userName}' ('{email}')", userName, email);
            }

            return user;
        }

        /// <summary>
        /// Adds a user to a specified role if they're not already in it.
        /// </summary>
        /// <param name="userManager">The user manager.</param>
        /// <param name="user">The user to add to the role.</param>
        /// <param name="role">The role to add the user to.</param>
        /// <param name="logger">The logger.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        private static async Task AddUserToRole(UserManager<ApplicationUser> userManager, ApplicationUser user, string role, ILogger logger)
        {
            if (!await userManager.IsInRoleAsync(user, role))
            {
                var result = await userManager.AddToRoleAsync(user, role);
                if (!result.Succeeded)
                {
                    throw new ArgumentException(
                        $"Unable to add user '{user.UserName}' ('{user.Email}') to role '{role}': {string.Join("; ", result.Errors.Select(error => error.Description))}");
                }
            }

            logger.LogInformation("Added user '{userName}' ('{email}') to role '{roleName}'", user.UserName, user.Email, role);
        }

    }
}
