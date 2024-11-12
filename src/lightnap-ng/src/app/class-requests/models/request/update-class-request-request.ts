
export interface UpdateClassRequestRequest {
	// TODO: Update these fields to match the server's UpdateClassRequestDto.
	requestedClassId: number;
	offeredClassId: number;
	userId: string;
	isActive: boolean;
}