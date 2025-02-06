import axios from "axios";

//export const API_URL = process.env.NEXT_PUBLIC_API_URL;
export const API_URL = "http://localhost:3000/api/books";

const handleApiError = (e: unknown) => {
  if (axios.isAxiosError(e)) {
    throw new Error(e.response?.data?.message || "Server error");
  } else if (e instanceof Error) {
    throw new Error(e.message);
  } else {
    throw new Error("An unknown error occurred");
  }
};

export const getAllBooks = async () => {
  try {
    const response = await axios.get(`${API_URL}/getallbooks`);
    return response.data.data;
  } catch (e) {
    handleApiError(e);
  }
};

export const addBook = async (book: {
  title: string;
  author: string;
  year: number;
  genre: string;
}) => {
  try {
    const response = await axios.post(`${API_URL}/addbook`, book, {
      headers: { "Content-Type": "application/json" },
    });
    return response.data.data;
  } catch (e) {
    handleApiError(e);
  }
};

export const updateBook = async (
  id: string,
  updateBook: {
    title?: string;
    author?: string;
    year?: number;
    genre?: string;
  }
) => {
  try {
    const response = await axios.put(`${API_URL}/bookid/${id}`, updateBook, {
      headers: { "Content-Type": "application/json" },
    });
    return response.data.data;
  } catch (e) {
    handleApiError(e);
  }
};

export const deleteBook = async (id: string) => {
  try {
    await axios.delete(`${API_URL}/bookid/${id}`);
    return true;
  } catch (e) {
    handleApiError(e);
  }
};
