"use client";

import { Alert, SxProps } from "@mui/material";
import { AlertMessageProps } from "../interfaces/props/alertmessage.props";

export const AlertMessage = ({
  message,
  severity,
  size = "medium",
}: AlertMessageProps) => {
  const sizeStyles: SxProps = {
    small: { fontSize: "0.8rem", padding: "6px 12px" },
    medium: { fontSize: "1rem", padding: "8px 16px" },
    large: { fontSize: "1.2rem", padding: "10px 20px" },
  }[size || "medium"];

  return (
    <Alert
      severity={severity}
      sx={{ marginBottom: 2, marginTop: 2, ...sizeStyles }}
    >
      {message}
    </Alert>
  );
};
