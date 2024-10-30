/**
 * Represents a request to log in to the application.
 */
export interface LoginRequest {
    /**
     * The email address of the user attempting to log in.
     */
    email: string;

    /**
     * The password of the user attempting to log in.
     */
    password: string;

    /**
     * Indicates whether the user should be remembered on the device.
     */
    rememberMe: boolean;

    /**
     * Details about the device from which the login request is made, such as the user agent,
     * so that the user can recognize it later on if they want to revoke the associated token.
     */
    deviceDetails: string;
}
