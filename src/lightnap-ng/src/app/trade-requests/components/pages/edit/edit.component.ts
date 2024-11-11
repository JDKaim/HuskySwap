
import { CommonModule } from "@angular/common";
import { Component, inject, input, OnInit } from "@angular/core";
import { FormBuilder, ReactiveFormsModule, Validators } from "@angular/forms";
import { ActivatedRoute, Router, RouterLink } from "@angular/router";
import { ApiResponse, ApiResponseComponent, ConfirmPopupComponent, ErrorListComponent, ToastService } from "@core";
import { ConfirmationService } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CardModule } from "primeng/card";
import { CheckboxModule } from "primeng/checkbox";
import { InputNumberModule } from "primeng/inputnumber";
import { InputTextModule } from "primeng/inputtext";
import { Observable, tap } from "rxjs";
import { UpdateTradeRequestRequest } from "src/app/trade-requests/models/request/update-trade-request-request";
import { TradeRequest } from "src/app/trade-requests/models/response/trade-request";
import { TradeRequestService } from "src/app/trade-requests/services/trade-request.service";

@Component({
  standalone: true,
  templateUrl: "./edit.component.html",
  imports: [
    CommonModule,
    CardModule,
    ReactiveFormsModule,
    ApiResponseComponent,
    ConfirmPopupComponent,
    RouterLink,
    CalendarModule,
    ButtonModule,
    InputTextModule,
    InputNumberModule,
    CheckboxModule,
    ErrorListComponent,
  ],
})
export class EditComponent implements OnInit {
  #tradeRequestService = inject(TradeRequestService);
  #router = inject(Router);
  #activeRoute = inject(ActivatedRoute);
  #confirmationService = inject(ConfirmationService);
  #toast = inject(ToastService);
  #fb = inject(FormBuilder);

  errors = new Array<string>();

  form = this.#fb.group({
	// TODO: Update these fields to match the right parameters.
	requestingClassUserId: this.#fb.control(0, [Validators.required]),
	targetClassUserId: this.#fb.control(0, [Validators.required]),
	status: this.#fb.control("string", [Validators.required]),
	notes: this.#fb.control("string", [Validators.required]),
  });

  readonly id = input<number>(undefined);
  tradeRequest$ = new Observable<ApiResponse<TradeRequest>>();

  ngOnInit() {
    this.tradeRequest$ = this.#tradeRequestService.getTradeRequest(this.id()).pipe(
      tap(response => {
        if (response.result) {
          this.form.patchValue(response.result);
        }
      })
    );
  }

  saveClicked() {
    this.errors = [];

    const request = <UpdateTradeRequestRequest>this.form.value;

    this.#tradeRequestService.updateTradeRequest(this.id(), request).subscribe(response => {
      if (!response.result) {
        this.errors = response.errorMessages;
        return;
      }

      this.#toast.success("Updated successfully");
    });
  }

  deleteClicked(event: any) {
    this.errors = [];

    this.#confirmationService.confirm({
      header: "Confirm Delete Item",
      message: `Are you sure that you want to delete this item?`,
      target: event.target,
      key: "delete",
      accept: () => {
        this.#tradeRequestService.deleteTradeRequest(this.id()).subscribe(response => {
          if (!response.result) {
            this.errors = response.errorMessages;
            return;
          }

          this.#toast.success("Deleted successfully");
          this.#router.navigate(["."], { relativeTo: this.#activeRoute.parent });
        });
      },
    });
  }
}