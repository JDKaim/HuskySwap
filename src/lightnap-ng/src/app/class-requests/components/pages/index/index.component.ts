
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ApiResponseComponent, EmptyPagedResponse, SuccessApiResponse } from "@core";
import { ButtonModule } from "primeng/button";
import { CalendarModule } from "primeng/calendar";
import { CardModule } from "primeng/card";
import { InputNumberModule } from "primeng/inputnumber";
import { InputTextModule } from "primeng/inputtext";
import { PanelModule } from "primeng/panel";
import { TableLazyLoadEvent, TableModule } from "primeng/table";
import { debounceTime, startWith, Subject, switchMap } from "rxjs";
import { ClassRequest } from "src/app/class-requests/models/response/class-request";
import { ClassRequestService } from "src/app/class-requests/services/class-request.service";

@Component({
  standalone: true,
  templateUrl: "./index.component.html",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    InputTextModule,
    InputNumberModule,
    CalendarModule,
    ApiResponseComponent,
    PanelModule,
    TableModule,
    RouterModule,
    ButtonModule],
})
export class IndexComponent {
  pageSize = 10;

  #classRequestService = inject(ClassRequestService);
  #fb = inject(FormBuilder);

  form = this.#fb.group({
      requestedClassId: this.#fb.control<number>(undefined),
      offeredClassId: this.#fb.control<number>(undefined),
      userId: this.#fb.control<string>(undefined),
      isActive: this.#fb.control<boolean>(undefined),
  });

  #lazyLoadEventSubject = new Subject<TableLazyLoadEvent>();
  searchResults$ = this.#lazyLoadEventSubject.pipe(
    switchMap(event =>
      this.#classRequestService.searchClassRequests({
        ...this.form.value,
        pageSize: this.pageSize,
        pageNumber: event.first / this.pageSize + 1,
      })
    ),
    // We need to bootstrap the p-table with a response to get the whole process running. We do it this way to
    // fake an empty response so we can avoid a redundant call to the API.
    startWith(new SuccessApiResponse(new EmptyPagedResponse<ClassRequest>()))
  );

  constructor() {
    this.form.valueChanges.pipe(takeUntilDestroyed(), debounceTime(1000)).subscribe(() => {
      this.#lazyLoadEventSubject.next({ first: 0 });
    });
  }

  onLazyLoad(event: TableLazyLoadEvent) {
    this.#lazyLoadEventSubject.next(event);
  }
}
