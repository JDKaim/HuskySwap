
import { inject, Injectable } from "@angular/core";
import { CreateClassRequestRequest } from "../models/request/create-class-request-request";
import { SearchClassRequestsRequest } from "../models/request/search-class-requests-request";
import { UpdateClassRequestRequest } from "../models/request/update-class-request-request";
import { DataService } from "./data.service";

@Injectable({
  providedIn: "root",
})
export class ClassRequestService {
  #dataService = inject(DataService);

    getClassRequest(id: number) {
        return this.#dataService.getClassRequest(id);
    }

    searchClassRequests(request: SearchClassRequestsRequest) {
        return this.#dataService.searchClassRequests(request);
    }

    createClassRequest(request: CreateClassRequestRequest) {
        return this.#dataService.createClassRequest(request);
    }

    updateClassRequest(id: number, request: UpdateClassRequestRequest) {
        return this.#dataService.updateClassRequest(id, request);
    }

    deleteClassRequest(id: number) {
        return this.#dataService.deleteClassRequest(id);
    }
}
