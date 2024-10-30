import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { BlockUiService } from "@core";
import { ErrorListComponent } from "@core/components/controls/error-list/error-list.component";
import { confirmPasswordValidator } from "@core/helpers/form-helpers";
import { ToastService } from "@core/services/toast.service";
import { ProfileService } from "@profile/services/profile.service";
import { RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { MessagesModule } from "primeng/messages";
import { PasswordModule } from "primeng/password";
import { TableModule } from "primeng/table";

@Component({
  standalone: true,
  templateUrl: "./change-password.component.html",
  imports: [
    CommonModule,
    TableModule,
    ButtonModule,
    ErrorListComponent,
    PasswordModule,
    ReactiveFormsModule,
    RouterModule,
    RoutePipe,
    MessagesModule,
    CardModule,
  ],
})
export class ChangePasswordComponent {
  #profileService = inject(ProfileService);
  #blockUi = inject(BlockUiService);
  #toastService = inject(ToastService);
  #fb = inject(FormBuilder);

  errors = new Array<string>();

  form = this.#fb.nonNullable.group(
    {
      currentPassword: this.#fb.control("", [Validators.required]),
      newPassword: this.#fb.control("", [Validators.required]),
      confirmNewPassword: this.#fb.control("", [Validators.required]),
    },
    { validators: [confirmPasswordValidator("newPassword", "confirmNewPassword")] }
  );

  changePassword() {
    this.#blockUi.show({ message: "Changing password..." });
    this.#profileService
      .changePassword({
        confirmNewPassword: this.form.value.confirmNewPassword,
        currentPassword: this.form.value.currentPassword,
        newPassword: this.form.value.newPassword,
      })
      .subscribe({
        next: response => {
          if (!response.result) {
            this.errors = response.errorMessages;
            return;
          }

          this.#toastService.success("Password changed successfully.");
          this.form.reset();
        },
        complete: () => this.#blockUi.hide(),
      });
  }
}
