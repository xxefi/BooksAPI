"use client";

import { Box, Typography } from "@mui/material";
import { useAddBookForm } from "@/app/hooks/useAddBookForm";
import { AlertMessage } from "@/app/components/AlertMessage";
import { BookAddForm } from "@/app/components/BookAddForm";

export default function AddBook() {
  const { formData, handleChange, onSubmit, loading, error, success } =
    useAddBookForm();

  return (
    <Box
      sx={{
        display: "flex",
        flexDirection: "column",
        alignItems: "center",
        justifyContent: "center",
        minHeight: "100vh",
        background: "linear-gradient(120deg, #6a11cb, #2575fc)",
        color: "#fff",
        padding: 3,
      }}
    >
      <Box
        sx={{
          background: "white",
          borderRadius: "8px",
          padding: "40px 30px",
          maxWidth: "500px",
          width: "100%",
          boxShadow: "0px 6px 15px rgba(0, 0, 0, 0.2)",
          transition: "transform 0.3s ease",
          "&:hover": {
            transform: "translateY(-10px)",
          },
        }}
      >
        <Typography
          variant="h4"
          component="h1"
          sx={{
            marginBottom: 3,
            textAlign: "center",
            fontWeight: "600",
            color: "#333",
            fontFamily: "Roboto, sans-serif",
          }}
        >
          Add New Book
        </Typography>

        {success && <AlertMessage message={success} severity="success" />}
        {error && <AlertMessage message={error} severity="error" />}

        <BookAddForm
          formData={formData}
          loading={loading}
          handleChange={handleChange}
          onSubmit={onSubmit}
        />
      </Box>
    </Box>
  );
}
