"use client";

import { AppBar, Box, Button, Toolbar, Typography } from "@mui/material";
import { usePathname, useRouter } from "next/navigation";
import { useEffect } from "react";

export const Navbar = () => {
  const router = useRouter();
  const pathname = usePathname();

  const pages = [
    { label: "Add Book", path: "/books/addbook" },
    { label: "Get All Books", path: "/books/getallbooks" },
    { label: "Remove Book", path: "/books/removebook" },
    { label: "Update Book", path: "/books/updatebook" },
  ];

  const currentPage = pages.find((p) => p.path === pathname);
  useEffect(() => {
    document.title = currentPage?.label || "Books manager";
  }, [currentPage]);

  return (
    <Box sx={{ flexGrow: 1 }}>
      <AppBar position="static" sx={{ bgcolor: "black", boxShadow: "none" }}>
        <Toolbar sx={{ justifyContent: "space-between" }}>
          <Typography
            variant="h6"
            component="div"
            sx={{
              flexGrow: 1,
              cursor: "pointer",
              fontWeight: "bold",
              color: "white",
              textDecoration: "none",
            }}
            onClick={() => router.push("/")}
          >
            Book Manager
          </Typography>

          <Box sx={{ display: "flex", gap: 2 }}>
            {pages.map((page) => (
              <Button
                key={page.path}
                color="inherit"
                onClick={() => router.push(page.path)}
                sx={{
                  textTransform: "none",
                  borderRadius: 0,
                  position: "relative",
                  "&::after": {
                    content: '""',
                    position: "absolute",
                    bottom: 0,
                    left: 0,
                    width: "100%",
                    height: "1.5px",
                    backgroundColor: "white",
                    transform:
                      pathname === page.path ? "scaleX(1)" : "scaleX(0)",
                    transformOrigin: "bottom",
                    transition: "transform 0.3s ease",
                  },
                  "&:hover": {
                    backgroundColor: "rgba(255, 255, 255, 0.1)",
                  },
                }}
              >
                {page.label}
              </Button>
            ))}
          </Box>
        </Toolbar>
      </AppBar>
    </Box>
  );
};
