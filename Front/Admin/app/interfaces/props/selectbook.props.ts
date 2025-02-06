import { Book } from "@/app/models/book";

export interface SelectBookProps {
  books: Book[];
  selectedBook: Book | null;
  handleSelectBook: (book: Book | null) => void;
  loading: boolean;
}
