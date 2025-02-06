import type { Metadata } from "next";
import "./globals.css";
import { CssBaseline, ThemeProvider } from "@mui/material";
import { theme } from "./theme/theme";
import { Navbar } from "./components/Navbar";
import { StoreProvider } from "./redux/providers/StoreProvider";

export const metadata: Metadata = {
  title: "Books manager",
};

export default function RootLayout({
  children,
}: Readonly<{
  children: React.ReactNode;
}>) {
  return (
    <html lang="en">
      <body>
        <ThemeProvider theme={theme}>
          <StoreProvider>
            <CssBaseline />
            <Navbar />
            {children}
          </StoreProvider>
        </ThemeProvider>
      </body>
    </html>
  );
}
