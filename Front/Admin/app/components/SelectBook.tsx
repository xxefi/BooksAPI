"use client";
import {
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  SelectChangeEvent,
} from "@mui/material";
import { SelectBookProps } from "../interfaces/props/selectbook.props";

export const SelectBook = ({
  books,
  selectedBook,
  handleSelectBook,
  loading,
}: SelectBookProps) => {
  const handleChange = (e: SelectChangeEvent<string>) => {
    const selectedId = e.target.value as string;
    const selected = books.find((book) => book.id === selectedId) || null;
    handleSelectBook(selected);
  };
  return (
    <FormControl fullWidth sx={{ marginBottom: 2 }}>
      <InputLabel>Select book</InputLabel>
      <Select
        value={selectedBook ? selectedBook.id : ""}
        onChange={handleChange}
        label="Select book"
        disabled={loading}
      >
        <MenuItem value="">
          <em>No select</em>
        </MenuItem>
        {books.map((book) => (
          <MenuItem key={book.id} value={book.id}>
            {book.title}
          </MenuItem>
        ))}
      </Select>
    </FormControl>
  );
};
