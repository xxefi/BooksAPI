"use client";

import { Box, CircularProgress, Typography, Grid } from "@mui/material";
import { useBooks } from "@/app/hooks/useBooks";
import { BookCard } from "@/app/components/BookCard";
import { AlertMessage } from "@/app/components/AlertMessage";

export default function GetAllBooks() {
  const { books, loading, error } = useBooks();

  return (
    <Box
      sx={{
        minHeight: "100vh",
        padding: 4,
        background: "linear-gradient(135deg, #2a5298, #1e3c72)",
        color: "#fff",
      }}
    >
      <Typography
        variant="h3"
        sx={{
          textAlign: "center",
          marginBottom: 5,
          fontWeight: "600",
          letterSpacing: "1.5px",
          color: "#ff9800",
        }}
      >
        All Books
      </Typography>
      {loading && (
        <Box sx={{ display: "flex", justifyContent: "center", paddingTop: 5 }}>
          <CircularProgress
            sx={{
              display: "block",
              color: "#ff9800",
              transform: "scale(1.8)",
            }}
          />
        </Box>
      )}
      {error && <AlertMessage severity="error" message={error} size="small" />}

      {!loading && !error && books && books.length === 0 && (
        <Typography
          variant="h6"
          sx={{
            textAlign: "center",
            color: "#ffcc00",
            fontWeight: "500",
            marginTop: 3,
            fontStyle: "italic",
          }}
        >
          No books found
        </Typography>
      )}
      {!loading && !error && books && books.length > 0 && (
        <Grid container spacing={3} justifyContent="center">
          {books.map((book) => (
            <Grid item xs={12} sm={6} md={4} lg={3} key={book.id}>
              <BookCard book={book} />
            </Grid>
          ))}
        </Grid>
      )}
    </Box>
  );
}
