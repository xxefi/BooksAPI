"use client";
import { Button, ListItem, ListItemText } from "@mui/material";
import DeleteOutlineIcon from "@mui/icons-material/DeleteOutline";
import { BookItemProps } from "../interfaces/props/bookitem.props";

export const BookRemoveItem = ({ book, onDelete, disabled }: BookItemProps) => {
  return (
    <ListItem
      sx={{
        display: "flex",
        justifyContent: "space-between",
        alignItems: "center",
      }}
    >
      <ListItemText
        primary={book.title}
        secondary={`Author: ${book.author.name} | Year: ${book.year}`}
      />
      <Button
        variant="contained"
        color="error"
        sx={{
          textTransform: "none",
          fontWeight: "bold",
          padding: "8px 16px",
          borderRadius: "8px",
          transition: "0.3s",
          display: "flex",
          alignItems: "center",
          gap: "8px",
          boxShadow: "0px 4px 10px rgba(255, 0, 0, 0.3)",
          "&:hover": {
            backgroundColor: "#d32f2f",
            boxShadow: "0px 6px 12px rgba(255, 0, 0, 0.5)",
          },
          "&:disabled": {
            backgroundColor: "#ffcccb",
            color: "#999",
            boxShadow: "none",
          },
        }}
        onClick={() => onDelete(book.id!)}
        disabled={disabled}
      >
        <DeleteOutlineIcon sx={{ fontSize: 20 }} />
        Delete
      </Button>
    </ListItem>
  );
};
