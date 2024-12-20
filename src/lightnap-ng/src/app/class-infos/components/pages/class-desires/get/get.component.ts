import { CommonModule } from "@angular/common";
import { Component, inject, Input, OnInit } from "@angular/core";
import { RouterLink } from "@angular/router";
import { ApiResponse, ApiResponseComponent } from "@core";
import { ButtonModule } from "primeng/button";
import { CardModule } from "primeng/card";
import { Observable } from "rxjs";
import { ClassDesire } from "src/app/class-infos/models/response/class-desire";
import { ClassInfoService } from "src/app/class-infos/services/class-info.service";

@Component({
  standalone: true,
  templateUrl: "./get.component.html",
  imports: [CommonModule, CardModule, RouterLink, ApiResponseComponent, ButtonModule],
})
export class GetComponent implements OnInit {
  #classInfoService = inject(ClassInfoService);
  errors = new Array<string>();

  @Input() id: number;
  classDesire$ = new Observable<ClassDesire>();

  ngOnInit() {
    this.classDesire$ = this.#classInfoService.getClassDesire(this.id);
  }
}
