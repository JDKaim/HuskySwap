
export interface ClassRequest {
	// TODO: Update these fields to match the server's ClassRequestDto.
	id: number;
	requestedClassId: number;
	offeredClassId: number;
	userId: string;
	isActive: boolean;
}
