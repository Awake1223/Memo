export interface TagDto {
  id: number;
  name: string;
  count: number;
}

export interface NoteTagDto {
  noteId: number;
  tagId: number;
  tag: TagDto;
}
