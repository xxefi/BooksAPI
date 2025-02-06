"use client";
import { TextField, Button } from "@mui/material";
import { AlertMessage } from "./AlertMessage";
import { BookUpdateFormProps } from "../interfaces/props/bookupdateform.props";

export const BookUpdateForm = ({
  selectedBook,
  updatedFields,
  handleChange,
  handleSubmit,
  loading,
  warning,
  success,
  error,
}: BookUpdateFormProps) => {
  if (!selectedBook) return null;
  return (
    <>
      <form onSubmit={handleSubmit} style={{ marginTop: "20px" }}>
        <TextField
          label="Title"
          variant="outlined"
          fullWidth
          value={updatedFields.title ?? selectedBook.title}
          onChange={handleChange}
          name="title"
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Author"
          variant="outlined"
          fullWidth
          value={updatedFields.author ?? selectedBook.author.name}
          onChange={handleChange}
          name="author"
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Year"
          variant="outlined"
          fullWidth
          value={updatedFields.year ?? selectedBook.year}
          onChange={handleChange}
          name="year"
          sx={{ marginBottom: 2 }}
        />
        <TextField
          label="Genre"
          variant="outlined"
          fullWidth
          value={updatedFields.genre ?? selectedBook.genre}
          onChange={handleChange}
          name="genre"
          sx={{ marginBottom: 2 }}
        />
        <Button
          type="submit"
          variant="contained"
          color="primary"
          sx={{ textTransform: "none", background: "black", width: "100%" }}
          disabled={loading}
        >
          Update book
        </Button>
      </form>
      {warning && <AlertMessage severity="warning" message={warning} />}
      {error && <AlertMessage severity="error" message={error} />}
      {success && <AlertMessage severity="success" message={success} />}
    </>
  );
};
