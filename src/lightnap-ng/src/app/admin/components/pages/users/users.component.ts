import { AdminUser, SearchAdminUsersSortBy } from "@admin";
import { AdminService } from "@admin/services/admin.service";
import { CommonModule } from "@angular/common";
import { Component, inject } from "@angular/core";
import { takeUntilDestroyed } from "@angular/core/rxjs-interop";
import { FormBuilder, ReactiveFormsModule } from "@angular/forms";
import { RouterModule } from "@angular/router";
import { ApiResponseComponent, ConfirmPopupComponent, EmptyPagedResponse, ErrorListComponent, ListItem, SuccessApiResponse } from "@core";
import { RoutePipe } from "@routing";
import { ConfirmationService, LazyLoadEvent } from "primeng/api";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { DropdownModule } from "primeng/dropdown";
import { InputTextModule } from "primeng/inputtext";
import { TableLazyLoadEvent, TableModule } from "primeng/table";
import { debounceTime, startWith, Subject, switchMap } from "rxjs";

@Component({
  standalone: true,
  templateUrl: "./users.component.html",
  imports: [
    CommonModule,
    ReactiveFormsModule,
    CardModule,
    ApiResponseComponent,
    TableModule,
    ButtonModule,
    RouterModule,
    RoutePipe,
    DropdownModule,
    ErrorListComponent,
    InputTextModule,
    ConfirmPopupComponent,
  ],
})
export class UsersComponent {
  pageSize = 25;

  #adminService = inject(AdminService);
  #confirmationService = inject(ConfirmationService);

  #fb = inject(FormBuilder);

  form = this.#fb.group({
    email: this.#fb.control(""),
    userName: this.#fb.control(""),
  });

  errors = new Array<string>();

  #lazyLoadEventSubject = new Subject<TableLazyLoadEvent>();
  users$ = this.#lazyLoadEventSubject.pipe(
    switchMap(event =>
      this.#adminService.searchUsers({
        sortBy: (event.sortField as SearchAdminUsersSortBy) ?? "userName",
        reverseSort: event.sortOrder === -1,
        pageSize: this.pageSize,
        pageNumber: event.first / this.pageSize + 1,
        email: this.form.value.email?.length > 0 ? this.form.value.email : undefined,
        userName: this.form.value.userName?.length > 0 ? this.form.value.userName : undefined,
      })
    ),
    // We need to bootstrap the p-table with a response to get the whole process running. We do it this way to fake an empty response
    // so we can avoid a redundant call to the API.
    startWith(new SuccessApiResponse(new EmptyPagedResponse<AdminUser>()))
  );

  sortBys = [
    new ListItem<SearchAdminUsersSortBy>("userName", "User Name", "Sort by user name."),
    new ListItem<SearchAdminUsersSortBy>("email", "Email", "Sort by email."),
    new ListItem<SearchAdminUsersSortBy>("createdDate", "Created", "Sort by created date."),
    new ListItem<SearchAdminUsersSortBy>("lastModifiedDate", "Last Modified", "Sort by last modified date."),
  ];

  constructor() {
    this.form.valueChanges.pipe(takeUntilDestroyed(), debounceTime(1000)).subscribe(() => {
      this.#lazyLoadEventSubject.next({ first: 0 });
    });
  }

  loadUsersLazy(event: TableLazyLoadEvent) {
    this.#lazyLoadEventSubject.next(event);
  }

  deleteUser(event: any, userId: string) {
    this.#confirmationService.confirm({
      header: "Confirm Delete",
      message: `Are you sure that you want to delete this user?`,
      key: userId,
      target: event.target,
      accept: () => {
        this.#adminService.deleteUser(userId).subscribe(response => {
          if (!response.result) {
            this.errors = response.errorMessages;
            return;
          }
          this.#lazyLoadEventSubject.next({ first: 0 });
        });
      },
    });
  }
}
