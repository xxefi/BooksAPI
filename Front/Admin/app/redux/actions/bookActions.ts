import { Dispatch } from "redux";
import {
  setLoading,
  setError,
  setBooks,
  selectBook,
  setSuccess,
  setWarning,
} from "../slices/bookSlice";
import {
  addBook,
  deleteBook,
  getAllBooks,
  updateBook,
} from "@/app/services/bookService";
import { ThunkAction } from "redux-thunk";
import { RootState } from "../store";
import { PayloadAction } from "@reduxjs/toolkit";
import { Book } from "@/app/models/book";

export const fetchBooks = (): ThunkAction<
  void,
  RootState,
  unknown,
  PayloadAction<Book[]>
> => {
  return async (dispatch: Dispatch) => {
    dispatch(setLoading());
    try {
      const books = await getAllBooks();
      dispatch(setBooks(books));
    } catch (e: unknown) {
      dispatch(setError(e instanceof Error ? e.message : ""));
    }
  };
};

export const addBookAction = (
  bookData: Book
): ThunkAction<void, RootState, unknown, PayloadAction<Book>> => {
  return async (dispatch: Dispatch) => {
    dispatch(setLoading());
    try {
      const newBookData = {
        ...bookData,
        author: String(bookData.author),
        genre: String(bookData.genre),
      };
      const newBook = await addBook(newBookData);
      dispatch(setBooks([newBook]));
      dispatch(setSuccess("Book added"));
    } catch (e: unknown) {
      dispatch(setError(e instanceof Error ? e.message : ""));
    }
  };
};

export const updateBookAction = (
  bookId: string,
  bookData: Partial<Book>
): ThunkAction<void, RootState, unknown, PayloadAction<Book>> => {
  return async (dispatch: Dispatch, getState) => {
    dispatch(setLoading());

    const { books } = getState().book;
    const selectedBook = books.find((b) => b.id === bookId);

    if (!selectedBook) {
      dispatch(setWarning("Book not found"));
      return;
    }

    const hasChanges = Object.keys(bookData).some(
      (key) => bookData[key as keyof Book] !== selectedBook[key as keyof Book]
    );

    if (!hasChanges) {
      dispatch(setWarning("No changes detected"));
      return;
    }

    try {
      const updatedBookData = {
        ...bookData,
        author: bookData.author ? String(bookData.author) : undefined,
        genre: bookData.genre ? String(bookData.genre) : undefined,
      };

      const updatedBook = await updateBook(bookId, updatedBookData);
      const updatedBooks = books.map((b) =>
        b.id === bookId ? updatedBook : b
      );

      dispatch(setBooks(updatedBooks));
      dispatch(setSuccess("Book updated"));
    } catch (e: unknown) {
      dispatch(setError(e instanceof Error ? e.message : ""));
    }
  };
};

export const removeBookAction = (
  bookId: string
): ThunkAction<void, RootState, unknown, PayloadAction<string>> => {
  return async (dispatch: Dispatch, getState) => {
    dispatch(setLoading());
    console.log(bookId);
    try {
      await deleteBook(bookId);
      const { books } = getState().book;
      dispatch(setBooks(books.filter((b) => b.id !== bookId)));
      dispatch(setSuccess("Book deleted"));
    } catch (e: unknown) {
      dispatch(setError(e instanceof Error ? e.message : ""));
    }
  };
};

export const selectBookAction = (
  book: Book | null
): ThunkAction<void, RootState, unknown, PayloadAction<Book | null>> => {
  return async (dispatch: Dispatch) => {
    dispatch(selectBook(book));
  };
};
