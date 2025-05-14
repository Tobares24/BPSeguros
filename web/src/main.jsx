import 'bootstrap/dist/css/bootstrap.min.css';
import 'bootstrap/dist/js/bootstrap.bundle.min.js';
import { AppComponent } from "./AppComponent.jsx";
import { createRoot } from "react-dom/client";
import { StrictMode } from "react";

createRoot(document.getElementById("root")).render(
  <StrictMode>
    <AppComponent />
  </StrictMode>
);
