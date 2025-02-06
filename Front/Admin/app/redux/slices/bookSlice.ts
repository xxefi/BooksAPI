import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { Book } from "@/app/models/book";
import { initialState } from "@/app/interfaces/states/book.state";

const bookSlice = createSlice({
  name: "book",
  initialState,
  reducers: {
    setLoading: (state) => {
      state.loading = true;
      state.error = null;
      state.success = null;
      state.warning = null;
    },
    setError: (state, action: PayloadAction<string>) => {
      state.error = action.payload;
      state.loading = false;
      state.success = null;
    },
    setSuccess: (state, action: PayloadAction<string>) => {
      state.success = action.payload;
      state.loading = false;
      state.warning = null;
      state.error = null;
      state.updatedFields = {};
    },

    setBooks: (state, action: PayloadAction<Book[]>) => {
      state.books = action.payload;
      state.error = null;
      state.loading = false;
      state.warning = null;
    },

    selectBook: (state, action: PayloadAction<Book | null>) => {
      state.selectedBook = action.payload;
    },
    setWarning: (state, action: PayloadAction<string | null>) => {
      state.warning = action.payload;
      state.success = null;
      state.error = null;
      state.loading = false;
    },
    setUpdatedFields: (state, action: PayloadAction<Partial<Book>>) => {
      state.updatedFields = { ...state.updatedFields, ...action.payload };
    },
    clearNotifications: (state) => {
      state.success = null;
      state.error = null;
      state.warning = null;
    },
  },
  extraReducers: (builder) => {
    builder.addCase("book/clearNotificationsAsync", (state) => {
      state.success = null;
      state.error = null;
      state.warning = null;
    });
  },
});

export const {
  setLoading,
  setError,
  setSuccess,
  setBooks,
  selectBook,
  setWarning,
  setUpdatedFields,
  clearNotifications,
} = bookSlice.actions;
export default bookSlice.reducer;
