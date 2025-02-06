export interface AlertMessageProps {
  message: string;
  severity: "error" | "warning" | "info" | "success";
  size?: "small" | "medium" | "large";
}
