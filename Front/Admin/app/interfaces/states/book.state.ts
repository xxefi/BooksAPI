import { Book } from "@/app/models/book";

export interface BookState {
  books: Book[];
  loading: boolean;
  error: string | null;
  success: string | null;
  selectedBook: Book | null;
  warning: string | null;
  updatedFields: Partial<Book>;
}

export const initialState: BookState = {
  books: [],
  loading: false,
  error: null,
  success: null,
  selectedBook: null,
  warning: null,
  updatedFields: {},
};
