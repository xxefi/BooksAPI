import { Book } from "@/app/models/book";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../redux/store";
import { setUpdatedFields } from "../redux/slices/bookSlice";
import { updateBookAction } from "../redux/actions/bookActions";

export const useUpdateBook = (selectedBook: Book | null) => {
  const dispatch = useDispatch<AppDispatch>();

  const { loading, error, success, warning, updatedFields } = useSelector(
    (state: RootState) => state.book
  );

  const handleFieldChange = (name: keyof Book, value: string) => {
    dispatch({
      type: "book/setUpdatedFields",
      payload: { [name]: value },
    });
  };

  const handleUpdate = () => {
    if (!selectedBook) return;
    const updatedData = { ...updatedFields, id: selectedBook.id };
    dispatch(
      setUpdatedFields(updatedData),
      updateBookAction(selectedBook.id!, updatedFields)
    );
    dispatch(updateBookAction(selectedBook.id!, updatedFields));
  };

  return {
    loading,
    error,
    warning,
    success,
    updatedFields,
    handleUpdate,
    handleFieldChange,
  };
};
