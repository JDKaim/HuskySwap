﻿using LightNap.Core.Data.Entities;
using System.Net.Mail;

namespace LightNap.Core.Interfaces
{
    /// <summary>
    /// Interface for email services. Besides the basic SendEmailAsync method, it also includes methods for sending 
    /// user lifecycle messages (like 2FA) so that it's easier to override that functionality with templates from
    /// email service providers without having to touch the identity code.
    /// </summary>
    public interface IEmailService
    {
        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="message">The email message to send.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendEmailAsync(MailMessage message);

        /// <summary>
        /// Sends a two-factor authentication email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="code">The two-factor authentication code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendTwoFactorEmailAsync(ApplicationUser user, string code);

        /// <summary>
        /// Sends a password reset email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="passwordResetUrl">The URL for resetting the password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendPasswordResetEmailAsync(ApplicationUser user, string passwordResetUrl);

        /// <summary>
        /// Sends a registration email to the specified user.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        Task SendRegistrationEmailAsync(ApplicationUser user);
    }
}