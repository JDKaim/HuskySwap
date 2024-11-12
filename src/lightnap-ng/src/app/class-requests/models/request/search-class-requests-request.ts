
import { PaginationRequest } from "@core";
export interface SearchClassRequestsRequest extends PaginationRequest {
	// TODO: Update these fields to match the server's SearchClassRequestDto.
	requestedClassId?: number;
	offeredClassId?: number;
	userId?: string;
	isActive?: boolean;
}
