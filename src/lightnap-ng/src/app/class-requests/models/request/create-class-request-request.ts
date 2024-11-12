
export interface CreateClassRequestRequest {
	// TODO: Update these fields to match the server's CreateClassRequestDto.
	requestedClassId: number;
	offeredClassId: number;
	userId: string;
	isActive: boolean;
}
