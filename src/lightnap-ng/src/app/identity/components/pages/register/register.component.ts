
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BlockUiService } from "@core";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { confirmPasswordValidator } from "@core/helpers/form-helpers";
import { RouteAliasService, RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CheckboxModule } from "primeng/checkbox";
import { InputTextModule } from "primeng/inputtext";
import { PasswordModule } from "primeng/password";
import { IdentityService } from "src/app/identity/services/identity.service";
import { AppConfigComponent } from "src/app/layout/components/controls/app-config/app-config.component";
import { FocusContentLayout } from "src/app/layout/components/layouts/focus-content-layout/focus-content-layout.component";
import { LayoutService } from "src/app/layout/services/layout.service";

@Component({
  standalone: true,
  templateUrl: "./register.component.html",
  imports: [
    ReactiveFormsModule,
    RouterModule,
    InputTextModule,
    ButtonModule,
    PasswordModule,
    CheckboxModule,
    AppConfigComponent,
    RoutePipe,
    ErrorListComponent,
    FocusContentLayout
],
})
export class RegisterComponent {
  #identityService = inject(IdentityService);
    #blockUi = inject(BlockUiService);
  #fb = inject(FormBuilder);
  #routeAlias = inject(RouteAliasService);
  layoutService = inject(LayoutService);

  form = this.#fb.nonNullable.group({
    email: this.#fb.control("", [Validators.required, Validators.email]),
    password: this.#fb.control("", [Validators.required]),
    confirmPassword: this.#fb.control("", [Validators.required]),
    userName: this.#fb.control("", [Validators.required]),
    agreedToTerms: this.#fb.control(false, [Validators.requiredTrue]),
    rememberMe: this.#fb.control(true),
  },
  { validators: [confirmPasswordValidator("password", "confirmPassword")] }
);

  errors: Array<string> = [];

  register() {
    this.#blockUi.show({ message: "Registering..." });

    this.#identityService.register({
        email: this.form.value.email,
        password: this.form.value.password,
        confirmPassword: this.form.value.confirmPassword,
        deviceDetails: navigator.userAgent,
        rememberMe: this.form.value.rememberMe,
        userName: this.form.value.userName
      }).subscribe({
      next: response => {
        if (!response?.result) {
          if (response?.errorMessages?.length) {
            this.errors = response.errorMessages;
          } else {
            this.errors = ["An unexpected error occurred."];
          }
        } else if (response.result.twoFactorRequired) {
          this.#routeAlias.navigate("verify-code", this.form.value.email);
        } else {
          this.#routeAlias.navigate("user-home");
        }
      },
      complete: () => this.#blockUi.hide(),
    });
  }
}
