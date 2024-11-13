
import { HttpClient } from "@angular/common/http";
import { Injectable, inject } from "@angular/core";
import { API_URL_ROOT, ApiResponse, PagedResponse, throwIfApiError } from "@core";
import { tap } from "rxjs";
import {TradeRequestHelper } from "../helpers/trade-request.helper";
import { CreateTradeRequestRequest } from "../models/request/create-trade-request-request";
import { SearchTradeRequestsRequest } from "../models/request/search-trade-requests-request";
import { UpdateTradeRequestRequest } from "../models/request/update-trade-request-request";
import { TradeRequest } from "../models/response/trade-request";

@Injectable({
  providedIn: "root",
})
export class DataService {
  #http = inject(HttpClient);
  #apiUrlRoot = `${inject(API_URL_ROOT)}TradeRequests/`;

  getTradeRequest(id: number) {
    return this.#http.get<ApiResponse<TradeRequest>>(`${this.#apiUrlRoot}${id}`).pipe(
      tap(response => {
        if (response.result) {
          TradeRequestHelper.rehydrate(response.result);
        }
      })
    );
  }

  searchTradeRequests(request: SearchTradeRequestsRequest) {
    return this.#http.post<ApiResponse<PagedResponse<TradeRequest>>>(`${this.#apiUrlRoot}search`, request).pipe(
      tap(response => {
        if (response.result) {
          response.result.data.forEach(TradeRequestHelper.rehydrate);
        }
      })
    );
  }

  createTradeRequest(request: CreateTradeRequestRequest) {
    return this.#http.post<ApiResponse<TradeRequest>>(`${this.#apiUrlRoot}`, request);
  }

  makeATradeRequest(request: CreateTradeRequestRequest) {
    return this.#http.post<ApiResponse<TradeRequest>>(`${this.#apiUrlRoot}my-trades`, request);
  }

  updateTradeRequest(id: number, request: UpdateTradeRequestRequest) {
    return this.#http.put<ApiResponse<TradeRequest>>(`${this.#apiUrlRoot}${id}`, request);
  }

  deleteTradeRequest(id: number) {
    return this.#http.delete<ApiResponse<boolean>>(`${this.#apiUrlRoot}${id}`);
  }

  getMyTradeRequestsSent() {
    return this.#http.get<ApiResponse<TradeRequest[]>>(`${this.#apiUrlRoot}sent`);
  }

  acceptMyTradeRequest(id: number) {
    return this.#http.post<ApiResponse<boolean>>(`${this.#apiUrlRoot}${id}/accept`, null);
  }

  rejectMyTradeRequest(id: number) {
    return this.#http.post<ApiResponse<boolean>>(`${this.#apiUrlRoot}${id}/reject`, null);
  }

  getMyTradeRequestsReceived() {
    return this.#http.get<ApiResponse<TradeRequest[]>>(`${this.#apiUrlRoot}received`);
  }
}
