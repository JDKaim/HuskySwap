﻿using LightNap.Core.Data.Entities;
using LightNap.Core.Interfaces;
using System.Diagnostics;
using System.Net.Mail;

namespace LightNap.Core.Services
{
    /// <summary>
    /// Service for logging email details to the console instead of sending them.
    /// </summary>
    public class LogToConsoleEmailService : IEmailService
    {
        /// <summary>
        /// Logs the email details to the console asynchronously.
        /// </summary>
        /// <param name="message">The email message to log.</param>
        /// <returns>A completed task.</returns>
        public Task SendEmailAsync(MailMessage message)
        {
            Trace.TraceInformation($"Not sending email to '{message.To}' with subject '{message.Subject}' and body '{message.Body}'");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Logs the password reset email details to the console asynchronously.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="passwordResetUrl">The URL for resetting the password.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendPasswordResetEmailAsync(ApplicationUser user, string passwordResetUrl)
        {
            await SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Reset your password", $"You may reset your password at: {passwordResetUrl}"));
        }

        /// <summary>
        /// Logs the registration email details to the console asynchronously.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendRegistrationEmailAsync(ApplicationUser user)
        {
            await SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Welcome to our site", $"Thank you for registering."));
        }

        /// <summary>
        /// Logs the two-factor authentication email details to the console asynchronously.
        /// </summary>
        /// <param name="user">The user to send the email to.</param>
        /// <param name="code">The two-factor authentication code.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public async Task SendTwoFactorEmailAsync(ApplicationUser user, string code)
        {
            await SendEmailAsync(new MailMessage("noreply@sharplogic.com", user.Email!, "Your login security code", $"Your login code is: {code}"));
        }
    }
}
