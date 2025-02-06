"use client";

import { Box, Typography, CircularProgress, List } from "@mui/material";
import { useRemoveBooks } from "@/app/hooks/useRemoveBooks";
import { BookRemoveItem } from "@/app/components/BookRemoveItem";
import { AlertMessage } from "@/app/components/AlertMessage";

export default function RemoveBook() {
  const { books, loading, error, success, handleDelete } = useRemoveBooks();
  return (
    <Box sx={{ padding: 3 }}>
      <Typography
        variant="h4"
        sx={{ marginBottom: 2, textAlign: "center", fontWeight: "bold" }}
      >
        Remove Book
      </Typography>

      {loading && (
        <CircularProgress sx={{ display: "block", margin: "20px auto" }} />
      )}

      {error && <AlertMessage severity="error" message={error} />}
      {success && <AlertMessage severity="success" message={success} />}

      {!loading && !error && books.length === 0 && (
        <Typography variant="h6">No books found</Typography>
      )}

      {!loading && !error && books.length > 0 && (
        <List>
          {books.map((book) => (
            <BookRemoveItem
              key={book.id}
              book={book}
              onDelete={handleDelete}
              disabled={loading}
            />
          ))}
        </List>
      )}
    </Box>
  );
}
