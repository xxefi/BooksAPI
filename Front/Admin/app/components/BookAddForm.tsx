import {
  TextField,
  FormControl,
  InputLabel,
  Select,
  MenuItem,
  Button,
  CircularProgress,
} from "@mui/material";
import { BookFormProps } from "../interfaces/props/bookform.props";

export const BookAddForm = ({
  formData,
  loading,
  handleChange,
  onSubmit,
}: BookFormProps) => {
  return (
    <form onSubmit={onSubmit} style={{ width: "100%", marginTop: "20px" }}>
      <TextField
        label="Title"
        name="title"
        value={formData.title}
        onChange={handleChange}
        fullWidth
        required
        sx={{
          marginBottom: "20px",
          borderRadius: "8px",
          "& .MuiInputLabel-root": {
            color: "#6a11cb",
          },
          "& .MuiOutlinedInput-root": {
            borderColor: "#2575fc",
            borderRadius: "8px",
          },
        }}
      />
      <TextField
        label="Author"
        name="author"
        value={formData.author}
        onChange={handleChange}
        fullWidth
        required
        sx={{
          marginBottom: "20px",
          "& .MuiInputLabel-root": {
            color: "#6a11cb",

            borderRadius: "8px",
          },
          "& .MuiOutlinedInput-root": {
            borderColor: "#2575fc",

            borderRadius: "8px",
          },
        }}
      />
      <TextField
        label="Year"
        name="year"
        type="number"
        value={formData.year}
        onChange={handleChange}
        fullWidth
        required
        sx={{
          marginBottom: "20px",
          "& .MuiInputLabel-root": {
            color: "#6a11cb",

            borderRadius: "8px",
          },
          "& .MuiOutlinedInput-root": {
            borderColor: "#2575fc",

            borderRadius: "8px",
          },
        }}
      />
      <FormControl fullWidth sx={{ marginBottom: "20px" }}>
        <InputLabel>Genre</InputLabel>
        <Select
          name="genre"
          value={formData.genre}
          onChange={handleChange}
          required
          sx={{
            "& .MuiInputLabel-root": {
              color: "#6a11cb",

              borderRadius: "8px",
            },
            "& .MuiOutlinedInput-root": {
              borderColor: "#2575fc",

              borderRadius: "8px",
            },
          }}
        >
          <MenuItem value="Fiction">Fiction</MenuItem>
          <MenuItem value="Non-Fiction">Non-Fiction</MenuItem>
          <MenuItem value="Science Fiction">Science Fiction</MenuItem>
          <MenuItem value="Fantasy">Fantasy</MenuItem>
          <MenuItem value="Mystery">Mystery</MenuItem>
          <MenuItem value="Biography">Biography</MenuItem>
        </Select>
      </FormControl>

      <Button
        type="submit"
        variant="contained"
        color="primary"
        fullWidth
        sx={{
          backgroundColor: "#2575fc",
          borderRadius: "30px",
          padding: "12px 16px",
          fontSize: "16px",
          fontWeight: "bold",
          textTransform: "none",
          "&:hover": {
            backgroundColor: "#6a11cb",
          },
        }}
        disabled={loading}
      >
        {loading ? <CircularProgress size={24} color="inherit" /> : "Add Book"}
      </Button>
    </form>
  );
};
