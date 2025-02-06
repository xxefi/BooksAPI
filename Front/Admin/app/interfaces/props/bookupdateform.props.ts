import { Book } from "@/app/models/book";
import { ChangeEvent, FormEvent } from "react";

export interface BookUpdateFormProps {
  selectedBook: Book;
  updatedFields: Partial<Book>;
  handleChange: (e: ChangeEvent<HTMLInputElement>) => void;
  handleSubmit: (e: FormEvent) => void;
  loading: boolean;
  warning: string | null;
  error: string | null;
  success: string | null;
}
