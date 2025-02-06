import { Book } from "@/app/models/book";

export interface BookItemProps {
  book: Book;
  onDelete: (bookId: string) => void;
  disabled: boolean;
}
