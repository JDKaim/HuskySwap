
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ApiResponseComponent, ErrorListComponent, ToastService } from "@core";
import { RoutePipe } from "@routing";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { TableModule } from "primeng/table";
import { ClassUserService } from "src/app/class-users/services/class-user.service";
import { ConfirmPopupComponent } from "../../../../core/components/controls/confirm-popup/confirm-popup.component";
import { ConfirmationService } from "primeng/api";

@Component({
  standalone: true,
  templateUrl: "./my-classes.component.html",
  imports: [CommonModule, CardModule, RouterLink, ApiResponseComponent, ButtonModule, TableModule, RoutePipe, ErrorListComponent, ConfirmPopupComponent],
})
export class MyClassesComponent {
  #classUserService = inject(ClassUserService);
  classUsers$ = this.#classUserService.getMyClasses();
  #toast = inject(ToastService);
  errors = new Array<string>();
  #confirmationService = inject(ConfirmationService);

  #loadClasses() {
    this.classUsers$ = this.#classUserService.getMyClasses();
  }

  removeClassForMe(event: any, classId: number) {
    this.errors = [];

    this.#confirmationService.confirm({
        header: "Confirm Role Removal",
      message: `Are you sure that you want to remove this class?`,
      target: event.target,
      key: classId as any,
      accept: () => {
        this.#classUserService.removeMeFromClass(classId).subscribe({
          next: (response) => {
            if (!response.result) {
              this.errors = response.errorMessages;
              return;
            }
            this.#toast.success("Class removed successfully.");
            this.#loadClasses();
          },
        });
      },
    });
    
  }
}