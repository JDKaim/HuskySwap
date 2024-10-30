import { ApiResponse } from "./api-response";
import { ApiResponseType } from "./api-response-type";

/**
 * Represents an error response from an API. This is useful for scenarios where the error occurs on the client-side,
 * Such as during parameter validation or post-response processing.
 *
 * @template T - The type of the result expected from the API response.
 */
export class ErrorApiResponse<T> implements ApiResponse<T> {
    /**
     * The type of the API response.
     * Always set to "Error" for error responses.
     */
    type: ApiResponseType = "Error";

    /**
     * Indicates whether the response requires reauthorization.
     * Defaults to `false`.
     */
    requiresReauthorization = false;

    /**
     * The result of the API call.
     * Always set to `undefined` for error responses.
     */
    result = undefined;

    /**
     * Creates an instance of ErrorApiResponse.
     *
     * @param errorMessages - An array of user-friendly error messages.
     */
    constructor(public errorMessages: Array<string>) {}
}
