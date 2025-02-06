"use client";

import { Box, CircularProgress, Typography, Paper } from "@mui/material";
import { useBooks } from "@/app/hooks/useBooks";
import { useUpdateBook } from "@/app/hooks/useUpdateBook";
import { SelectBook } from "@/app/components/SelectBook";
import { ChangeEvent, FormEvent } from "react";
import { BookUpdateForm } from "@/app/components/BookUpdateForm";
import { Book } from "@/app/models/book";

export default function UpdateBook() {
  const {
    books,
    selectedBook,
    handleSelectBook,
    loading,
    error,
    success,
    warning,
  } = useBooks();
  const {
    loading: updateLoading,
    error: updateError,
    warning: updateWarning,
    success: updateSuccess,
    updatedFields,
    handleUpdate,
    handleFieldChange,
  } = useUpdateBook(selectedBook);

  const handleChange = (e: ChangeEvent<HTMLInputElement>) => {
    const { name, value } = e.target;
    handleFieldChange(name as keyof Book, value);
  };

  const handleSubmit = (e: FormEvent) => {
    e.preventDefault();
    handleUpdate();
  };

  return (
    <Box
      sx={{
        padding: 4,
        maxWidth: 600,
        margin: "auto",
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
      }}
    >
      <Typography
        variant="h4"
        sx={{
          marginBottom: 3,
          fontWeight: "bold",
          textAlign: "center",
          color: "#333",
        }}
      >
        Update Book
      </Typography>

      {loading && <CircularProgress sx={{ marginBottom: 3 }} />}

      {!loading && !error && books.length === 0 && (
        <Typography variant="h6" sx={{ color: "gray" }}>
          Books not found
        </Typography>
      )}

      <Paper
        elevation={6}
        sx={{
          padding: 3,
          borderRadius: 4,
          width: "100%",
          backgroundColor: "#f9f9f9",
          marginBottom: 3,
        }}
      >
        <SelectBook
          books={books}
          selectedBook={selectedBook}
          loading={loading}
          handleSelectBook={handleSelectBook}
        />
      </Paper>

      {selectedBook && (
        <BookUpdateForm
          selectedBook={selectedBook}
          updatedFields={updatedFields}
          handleChange={handleChange}
          handleSubmit={handleSubmit}
          loading={updateLoading || loading}
          warning={updateWarning || warning}
          error={updateError || error}
          success={updateSuccess || success}
        />
      )}
    </Box>
  );
}
