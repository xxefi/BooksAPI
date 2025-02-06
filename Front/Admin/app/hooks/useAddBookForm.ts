import { ChangeEvent, FormEvent } from "react";
import { Book } from "@/app/models/book";
import { SelectChangeEvent } from "@mui/material";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../redux/store";
import { addBookAction } from "../redux/actions/bookActions";
import { setUpdatedFields } from "../redux/slices/bookSlice";

export const useAddBookForm = () => {
  const dispatch = useDispatch<AppDispatch>();
  const { loading, error, success, updatedFields } = useSelector(
    (state: RootState) => state.book
  );
  const initialBookData: Book = {
    title: "",
    author: undefined,
    year: 0,
    genre: undefined,
  };

  const handleChange = (
    e:
      | ChangeEvent<
          HTMLInputElement | { name?: string | undefined; value: unknown }
        >
      | SelectChangeEvent<string>
  ) => {
    dispatch(
      setUpdatedFields({
        [e.target.name as string]: e.target.value,
      })
    );
  };

  const onSubmit = (e: FormEvent) => {
    e.preventDefault();
    dispatch(
      addBookAction({
        ...initialBookData,
        ...updatedFields,
      })
    );
  };

  return {
    formData: { ...initialBookData, ...updatedFields },
    handleChange,
    onSubmit,
    loading,
    error,
    success,
  };
};
