import { Author } from "./author";

export interface Book {
  id?: string;
  title: string;
  author: Author;
  year: number;
  genre: string;
}
