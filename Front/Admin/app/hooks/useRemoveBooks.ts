import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../redux/store";
import { useEffect } from "react";
import { fetchBooks, removeBookAction } from "../redux/actions/bookActions";

export const useRemoveBooks = () => {
  const dispatch = useDispatch<AppDispatch>();

  useEffect(() => {
    dispatch(fetchBooks());
  }, [dispatch]);

  const { books, loading, error, success } = useSelector(
    (state: RootState) => state.book
  );

  const handleDelete = (bookId: string) => {
    dispatch(removeBookAction(bookId));
  };

  return {
    books,
    loading,
    error,
    success,
    handleDelete,
  };
};
