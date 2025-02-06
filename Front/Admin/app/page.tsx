"use client";

import { useRouter } from "next/navigation";
import {
  Typography,
  Button,
  Container,
  Paper,
  IconButton,
  Grid,
  Box,
} from "@mui/material";
import { LibraryBooks, ArrowForward, Info } from "@mui/icons-material";
import { motion } from "framer-motion";
import "@fontsource/poppins";

export default function Home() {
  const router = useRouter();

  const handleGetStarted = () => {
    router.push("/books/getallbooks");
  };

  return (
    <main
      style={{
        display: "flex",
        alignItems: "center",
        justifyContent: "center",
        height: "100vh",
        background: "linear-gradient(135deg, #2a5298, #1e3c72)",
        color: "#fff",
        textAlign: "center",
        padding: "20px",
        fontFamily: "Poppins, sans-serif",
      }}
    >
      <Container maxWidth="lg">
        <motion.div
          initial={{ opacity: 0, scale: 0.8 }}
          animate={{ opacity: 1, scale: 1 }}
          transition={{ duration: 0.6 }}
        >
          <Paper
            elevation={12}
            sx={{
              padding: "50px",
              borderRadius: "20px",
              background: "rgba(255, 255, 255, 0.1)",
              backdropFilter: "blur(15px)",
              boxShadow: "0px 12px 40px rgba(0, 0, 0, 0.5)",
              maxWidth: "700px",
              margin: "0 auto",
              transition: "0.3s",
              "&:hover": {
                transform: "translateY(-10px)",
                boxShadow: "0px 20px 40px rgba(0, 0, 0, 0.6)",
              },
            }}
          >
            <IconButton
              sx={{
                backgroundColor: "rgba(255, 255, 255, 0.2)",
                color: "#fff",
                marginBottom: "16px",
                "&:hover": { backgroundColor: "rgba(255, 255, 255, 0.3)" },
              }}
            >
              <LibraryBooks fontSize="large" />
            </IconButton>
            <Typography
              variant="h3"
              component="h1"
              gutterBottom
              sx={{
                color: "white",
                textShadow: "3px 3px 8px rgba(0, 0, 0, 0.6)",
                fontWeight: 700,
                fontSize: "2.5rem",
              }}
            >
              Welcome to Book Manager!
            </Typography>
            <Typography
              variant="body1"
              sx={{
                marginBottom: "30px",
                color: "white",
                fontSize: "1.2rem",
                lineHeight: "1.8",
                textShadow: "1px 1px 4px rgba(0, 0, 0, 0.5)",
              }}
            >
              Effortlessly organize, manage, and enhance your book collection.
              Whether you’re a book enthusiast or a librarian, Book Manager is
              perfect for anyone looking to manage their library with ease!
            </Typography>
            <Button
              variant="contained"
              size="large"
              endIcon={<ArrowForward />}
              sx={{
                background: "white",
                color: "black",
                fontWeight: "bold",
                textTransform: "none",
                padding: "14px 28px",
                borderRadius: "12px",
                transition: "0.4s",
                "&:hover": {
                  transform: "scale(1.05)",
                  background: "#ffa726",
                },
              }}
              onClick={handleGetStarted}
            >
              Get Started
            </Button>
          </Paper>
        </motion.div>

        <Box sx={{ marginTop: "60px", textAlign: "center", color: "white" }}>
          <Typography
            variant="h4"
            sx={{
              fontWeight: "600",
              textDecoration: "underline",
              marginBottom: "30px",
              fontSize: "2rem",
            }}
          >
            Why Choose Book Manager?
          </Typography>
          <Grid container spacing={4} justifyContent="center">
            <Grid item xs={12} sm={4}>
              <motion.div
                initial={{ opacity: 0, x: -200 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.6, delay: 0.2 }}
              >
                <Paper
                  sx={{
                    padding: "25px",
                    backgroundColor: "rgba(255, 255, 255, 0.1)",
                    borderRadius: "15px",
                    boxShadow: "0px 8px 25px rgba(0, 0, 0, 0.3)",
                  }}
                >
                  <IconButton sx={{ color: "#ffa726", marginBottom: "15px" }}>
                    <Info fontSize="large" />
                  </IconButton>
                  <Typography
                    variant="h6"
                    sx={{ marginBottom: "12px", color: "#fff" }}
                  >
                    Simple and Intuitive
                  </Typography>
                  <Typography variant="body2" color="#fff">
                    Our platform is designed for simplicity, allowing you to
                    add, edit, or remove books in just a few clicks.
                  </Typography>
                </Paper>
              </motion.div>
            </Grid>

            <Grid item xs={12} sm={4}>
              <motion.div
                initial={{ opacity: 0, x: 200 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.6, delay: 0.4 }}
              >
                <Paper
                  sx={{
                    padding: "25px",
                    backgroundColor: "rgba(255, 255, 255, 0.1)",
                    borderRadius: "15px",
                    boxShadow: "0px 8px 25px rgba(0, 0, 0, 0.3)",
                  }}
                >
                  <IconButton sx={{ color: "#ffa726", marginBottom: "15px" }}>
                    <LibraryBooks fontSize="large" />
                  </IconButton>
                  <Typography
                    variant="h6"
                    sx={{ marginBottom: "12px", color: "#fff" }}
                  >
                    Manage Your Library
                  </Typography>
                  <Typography variant="body2" color="#fff">
                    Categorize your books, search by title, and keep your
                    library organized for easy access.
                  </Typography>
                </Paper>
              </motion.div>
            </Grid>

            <Grid item xs={12} sm={4}>
              <motion.div
                initial={{ opacity: 0, x: -200 }}
                animate={{ opacity: 1, x: 0 }}
                transition={{ duration: 0.6, delay: 0.6 }}
              >
                <Paper
                  sx={{
                    padding: "25px",
                    backgroundColor: "rgba(255, 255, 255, 0.1)",
                    borderRadius: "15px",
                    boxShadow: "0px 8px 25px rgba(0, 0, 0, 0.3)",
                  }}
                >
                  <IconButton sx={{ color: "#ffa726", marginBottom: "15px" }}>
                    <ArrowForward fontSize="large" />
                  </IconButton>
                  <Typography
                    variant="h6"
                    sx={{ marginBottom: "12px", color: "#fff" }}
                  >
                    Access Anywhere
                  </Typography>
                  <Typography variant="body2" color="#fff">
                    Access your book collection from any device, anytime, and
                    stay updated with ease.
                  </Typography>
                </Paper>
              </motion.div>
            </Grid>
          </Grid>
        </Box>
      </Container>
    </main>
  );
}
