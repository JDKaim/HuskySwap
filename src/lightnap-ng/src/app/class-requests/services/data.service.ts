
import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT, ApiResponse, PagedResponse } from "@core";
import { tap } from "rxjs";
import {ClassRequestHelper } from "../helpers/class-request.helper";
import { CreateClassRequestRequest } from "../models/request/create-class-request-request";
import { SearchClassRequestsRequest } from "../models/request/search-class-requests-request";
import { UpdateClassRequestRequest } from "../models/request/update-class-request-request";
import { ClassRequest } from "../models/response/class-request";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #apiUrlRoot = `${inject(API_URL_ROOT)}ClassRequests/`;

  getClassRequest(id: number) {
    return this.#http.get<ApiResponse<ClassRequest>>(`${this.#apiUrlRoot}${id}`).pipe(
      tap(response => {
        if (response.result) {
          ClassRequestHelper.rehydrate(response.result);
        }
      })
    );
  }

  searchClassRequests(request: SearchClassRequestsRequest) {
    return this.#http.post<ApiResponse<PagedResponse<ClassRequest>>>(`${this.#apiUrlRoot}search`, request).pipe(
      tap(response => {
        if (response.result) {
          response.result.data.forEach(ClassRequestHelper.rehydrate);
        }
      })
    );
  }

  createClassRequest(request: CreateClassRequestRequest) {
    return this.#http.post<ApiResponse<ClassRequest>>(`${this.#apiUrlRoot}`, request);
  }

  updateClassRequest(id: number, request: UpdateClassRequestRequest) {
    return this.#http.put<ApiResponse<ClassRequest>>(`${this.#apiUrlRoot}${id}`, request);
  }

  deleteClassRequest(id: number) {
    return this.#http.delete<ApiResponse<boolean>>(`${this.#apiUrlRoot}${id}`);
  }
}
