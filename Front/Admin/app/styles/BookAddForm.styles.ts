import { SxProps } from "@mui/material";

export const textFieldStyle: SxProps = {
  marginBottom: 3,
  borderRadius: "12px",
  "& .MuiInputBase-root": {
    background: "#2b2b2b",
    color: "#fff",
    borderRadius: "12px",
  },
  "& .MuiInputLabel-root": { color: "#ccc" },
  "& .MuiOutlinedInput-notchedOutline": {
    borderColor: "#444",
    borderRadius: "12px",
  },
  "&:hover .MuiOutlinedInput-notchedOutline": {
    borderColor: "#ff9800",
  },
};

export const formControlStyle: SxProps = {
  marginBottom: 3,
  borderRadius: "12px",
  "& .MuiInputBase-root": {
    background: "#2b2b2b",
    color: "#fff",
    borderRadius: "12px",
  },
  "& .MuiInputLabel-root": { color: "#ccc" },
  "& .MuiOutlinedInput-notchedOutline": {
    borderColor: "#444",
    borderRadius: "12px",
  },
  "&:hover .MuiOutlinedInput-notchedOutline": {
    borderColor: "#ff9800",
  },
};

export const buttonStyle: SxProps = {
  padding: "12px",
  fontWeight: "bold",
  backgroundColor: "white",
  color: "black",
  textTransform: "none",
  borderRadius: "8px",
  "&:hover": {
    backgroundColor: "#e68900",
    boxShadow: "0px 4px 15px rgba(255, 152, 0, 0.5)",
  },
};
