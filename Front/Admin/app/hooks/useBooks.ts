import { useEffect } from "react";
import { useDispatch, useSelector } from "react-redux";
import { AppDispatch, RootState } from "../redux/store";
import { fetchBooks } from "../redux/actions/bookActions";
import { Book } from "../models/book";
import { selectBook } from "../redux/slices/bookSlice";

export const useBooks = () => {
  //const [books, setBooks] = useState<Book[]>([]);
  //const [loading, setLoading] = useState(false);
  //const [error, setError] = useState<string | null>(null);
  //const [selectedBook, setSelectedBook] = useState<Book | null>(null);
  //const [success, setSuccess] = useState<string | null>(null);
  //const [warning, setWarning] = useState<string | null>(null);
  const dispatch = useDispatch<AppDispatch>();
  const { books, loading, error, selectedBook, success, warning } = useSelector(
    (state: RootState) => state.book
  );

  useEffect(() => {
    dispatch(fetchBooks());
  }, [dispatch]);

  const handleSelectBook = (book: Book | null) => {
    dispatch(selectBook(book));
  };

  return {
    books,
    loading,
    error,
    selectedBook,
    success,
    warning,
    handleSelectBook,
  };
};
