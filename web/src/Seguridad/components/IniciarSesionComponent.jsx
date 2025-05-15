import { useState } from "react";
import { Link } from "react-router-dom";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";

export const IniciarSesionComponent = () => {
  const [email, setEmail] = useState("");
  const [password, setPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const togglePassword = () => setShowPassword(!showPassword);

  const handleSubmit = (e) => {
    e.preventDefault();
    // Aquí iría la lógica para autenticar
  };

  return (
    <div className="d-flex justify-content-center align-items-center vh-100">
      <div className="border p-4 rounded shadow" style={{ width: "420px" }}>
        <div className="text-center mb-4">
          <AccountCircleIcon style={{ fontSize: 64, color: "#0d6efd" }} />
        </div>
        <div className="mb-3">
          <label htmlFor="email" className="form-label">
            Email
          </label>
          <input
            id="email"
            type="email"
            className="form-control"
            placeholder="usuario@correo.com"
            autoComplete="off"
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            required
          />
        </div>
        <div className="mb-3 d-flex align-items-center">
          <div style={{ flex: 1 }}>
            <label htmlFor="password" className="form-label">
              Contraseña
            </label>
            <input
              id="password"
              type={showPassword ? "text" : "password"}
              className="form-control"
              placeholder="********"
              value={password}
              onChange={(e) => setPassword(e.target.value)}
              required
            />
          </div>
          <button
            type="button"
            onClick={togglePassword}
            className="btn btn-link p-0 ms-2 mt-4"
            style={{
              border: "none",
              background: "transparent",
              cursor: "pointer",
              height: "38px",
              display: "flex",
              alignItems: "center",
              color: "#6c757d",
            }}
            aria-label={
              showPassword ? "Ocultar contraseña" : "Mostrar contraseña"
            }
          >
            {showPassword ? (
              <VisibilityOffIcon style={{ fontSize: "24px" }} />
            ) : (
              <VisibilityIcon style={{ fontSize: "24px" }} />
            )}
          </button>
        </div>

        <button className="btn btn-primary w-100 mt-3" onClick={handleSubmit}>
          Iniciar sesión
        </button>

        <div className="mt-3 text-center">
          <Link to="/registro" className="text-decoration-none">
            ¿No tienes cuenta? Regístrate aquí
          </Link>
        </div>
      </div>
    </div>
  );
};
