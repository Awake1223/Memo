export enum NoteLifetime {
  OneHour = 1,
  OneDay = 2,
  Forever = 3
}

export interface CreateNoteRequest {
  content: string;
  lifetime: NoteLifetime;
  isBurnAfterReading?: boolean;
}

export interface NoteResponse {
  shortCode: string;
  content: string;
  expiresAt: string | null;
  createdAt: string;
  isExpired: boolean;
  isBurnAfterReading?: boolean;
}

export interface CreateNoteResponse {
  shortCode: string;
  expiresAt: string | null;
}
