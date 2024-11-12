
import { CommonModule } from "@angular/common";
import { Component, inject, input, OnInit } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ApiResponse, ApiResponseComponent } from "@core";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { Observable } from "rxjs";
import { ClassRequest } from "src/app/class-requests/models/response/class-request";
import { ClassRequestService } from "src/app/class-requests/services/class-request.service";

@Component({
  standalone: true,
  templateUrl: "./get.component.html",
  imports: [CommonModule, CardModule, RouterLink, ApiResponseComponent, ButtonModule],
})
export class GetComponent implements OnInit {
  #classRequestService = inject(ClassRequestService);
  errors = new Array<string>();

  readonly id = input<number>(undefined);
  classRequest$?: Observable<ApiResponse<ClassRequest>>;

  ngOnInit() {
    this.classRequest$ = this.#classRequestService.getClassRequest(this.id());
  }
}
