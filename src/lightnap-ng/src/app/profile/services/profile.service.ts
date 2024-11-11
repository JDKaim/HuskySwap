import { inject, Injectable } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { IdentityService } from "@identity";
import { ApplicationSettings, ChangePasswordRequest, StyleSettings, UpdateProfileRequest } from "@profile";
import { filter, of, switchMap, tap } from "rxjs";
import { DataService } from "./data.service";
import { ApiResponse, SuccessApiResponse } from "@core";

@Injectable({
  providedIn: "root",
})
/**
 * @class ProfileService
 * @description
 * The ProfileService class provides methods to manage user profiles, devices, and application settings.
 * It interacts with the DataService and IdentityService to perform various operations such as fetching
 * and updating profiles, managing devices, and handling application settings.
 */
export class ProfileService {
  #dataService = inject(DataService);
  #identityService = inject(IdentityService);

  // This should be kept in sync with the server-side BrowserSettings class.
  #defaultApplicationSettings: ApplicationSettings = {
    style: {
      ripple: true,
      inputStyle: "outlined",
      menuMode: "static",
      colorScheme: "light",
      theme: "saga-purple",
      scale: 14,
    },
    extended: {},
    features: {},
    preferences: {},
  };

  #settings?: ApplicationSettings;

  /**
   * Constructs the ProfileService and sets up the subscription to handle user logout.
   */
  constructor() {
    this.#identityService
      .watchLoggedIn$()
      .pipe(
        takeUntilDestroyed(),
        filter(loggedIn => !loggedIn)
      )
      .subscribe(() => {
        this.#settings = undefined;
      });
  }

  /**
   * @method getProfile
   * @description Fetches the user profile.
   * @returns {Observable<ApiResponse<Profile>>} An observable containing the user profile.
   */
  getProfile() {
    return this.#dataService.getProfile();
  }

  /**
   * @method updateProfile
   * @description Updates the user profile.
   * @param {UpdateProfileRequest} updateProfileRequest - The request object containing profile update information.
   * @returns {Observable<ApiResponse<Profile>>} An observable containing the updated profile.
   */
  updateProfile(updateProfileRequest: UpdateProfileRequest) {
    return this.#dataService.updateProfile(updateProfileRequest);
  }

  /**
   * @method getDevices
   * @description Fetches the list of devices associated with the user.
   * @returns {Observable<Array<Device>>} An observable containing the list of devices.
   */
  getDevices() {
    return this.#dataService.getDevices();
  }

  /**
   * @method revokeDevice
   * @description Revokes a device by its ID.
   * @param {string} deviceId - The ID of the device to revoke.
   * @returns {Observable<ApiResponse<boolean>>} An observable containing true if successful.
   */
  revokeDevice(deviceId: string) {
    return this.#dataService.revokeDevice(deviceId);
  }

  /**
   * @method changePassword
   * @description Changes the user's password.
   * @param {ChangePasswordRequest} changePasswordRequest - The request object containing password change information.
   * @returns {Observable<ApiResponse<boolean>>} An observable containing true if successful.
   */
  changePassword(changePasswordRequest: ChangePasswordRequest) {
    return this.#dataService.changePassword(changePasswordRequest);
  }

  /**
   * @method getSettings
   * @description Fetches the application settings. If settings are already loaded, returns them from memory.
   * @returns {Observable<ApiResponse<ApplicationSettings>>} An observable containing the application settings.
   */
  getSettings() {
    if (this.#settings) return of(new SuccessApiResponse(this.#settings) as ApiResponse<ApplicationSettings>);

    return this.#dataService.getSettings().pipe(
      tap(response => {
        this.#settings = JSON.parse(JSON.stringify(response));
      })
    );
  }

  /**
   * @method updateSettings
   * @description Updates the application settings.
   * @param {ApplicationSettings} browserSettings - The new application settings to be updated.
   * @returns {Observable<ApiResponse<boolean>>} An observable containing true if successful.
   */
  updateSettings(browserSettings: ApplicationSettings) {
    if (this.#settings) {
      this.#settings = browserSettings;
    }
    return this.#dataService.updateSettings(browserSettings);
  }

  /**
   * @method updateStyleSettings
   * @description Updates the style settings of the application.
   * @param {StyleSettings} styleSettings - The new style settings to be updated.
   * @returns {Observable<ApiResponse<boolean>>} An observable containing true if successful.
   */
  updateStyleSettings(styleSettings: StyleSettings) {
    return this.getSettings().pipe(
      switchMap(response => {
        if (!response.result || JSON.stringify(response.result.style) === JSON.stringify(styleSettings)) return of(response);
        return this.updateSettings({ ...response.result, style: styleSettings });
      })
    );
  }

  /**
   * @method getDefaultStyleSettings
   * @description Retrieves the default style settings.
   * @returns {StyleSettings} The default style settings.
   */
  getDefaultStyleSettings() {
    return JSON.parse(JSON.stringify(this.#defaultApplicationSettings.style)) as StyleSettings;
  }

  /**
   * @method hasLoadedStyleSettings
   * @description Checks if the style settings have been loaded.
   * @returns {boolean} True if the style settings have been loaded, false otherwise.
   */
  hasLoadedStyleSettings() {
    return !!this.#settings;
  }
}
