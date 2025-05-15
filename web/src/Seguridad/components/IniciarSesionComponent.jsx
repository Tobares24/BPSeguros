import { useState } from "react";
import { Link, useNavigate } from "react-router-dom";
import AccountCircleIcon from "@mui/icons-material/AccountCircle";
import VisibilityIcon from "@mui/icons-material/Visibility";
import VisibilityOffIcon from "@mui/icons-material/VisibilityOff";
import { SpinnerComponent } from "../../components/SpinnerComponent";
import SeguridadService from "../../Services/SeguridadService";
import { AlertaService } from "../../Services/AlertaService";

export const IniciarSesionComponent = () => {
  const [showPassword, setShowPassword] = useState(false);
  const [cargando, setCargando] = useState(false);
  const [modelo, setModelo] = useState({
    password: "",
    email: "",
  });
  const [errores, setErrores] = useState({
    email: false,
    password: false,
  });

  const seguridadService = new SeguridadService();
  const navigate = useNavigate();

  const login = async () => {
    setCargando(true);
    try {
      const response = await seguridadService.login(modelo);
      sessionStorage.setItem("token", response.token);
      sessionStorage.setItem("email", response.email);
      AlertaService.success("Exitoso", "Inicio de sesión exitoso.");
      navigate("/inicio");
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
    } finally {
      setCargando(false);
    }
  };

  const togglePassword = () => setShowPassword(!showPassword);

  const handleSubmit = (e) => {
    e.preventDefault();

    const camposVacios = {
      email: !modelo.email.trim(),
      password: !modelo.password.trim(),
    };
    setErrores(camposVacios);

    const tieneErrores = Object.values(camposVacios).some((v) => v);
    if (tieneErrores) return;

    login();
  };

  const onInputChance = ({ target }) => {
    const { name, value } = target;
    setModelo((prevData) => ({
      ...prevData,
      [name]: value,
    }));
    setErrores((prev) => ({
      ...prev,
      [name]: false,
    }));
  };

  return (
    <>
      <SpinnerComponent show={cargando} />
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
              name="email"
              type="email"
              className={`form-control ${errores.email ? "is-invalid" : ""}`}
              placeholder="usuario@correo.com"
              autoComplete="off"
              value={modelo.email}
              onChange={onInputChance}
            />
            {errores.email && (
              <div className="invalid-feedback">El email es requerido.</div>
            )}
          </div>

          <div className="mb-3 d-flex align-items-center">
            <div style={{ flex: 1 }}>
              <label htmlFor="password" className="form-label">
                Contraseña
              </label>
              <input
                id="password"
                name="password"
                type={showPassword ? "text" : "password"}
                className={`form-control ${
                  errores.password ? "is-invalid" : ""
                }`}
                placeholder="********"
                value={modelo.password}
                onChange={onInputChance}
              />
              {errores.password && (
                <div className="invalid-feedback">
                  La contraseña es requerida.
                </div>
              )}
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
            <Link to="/login" className="text-decoration-none">
              ¿No tienes cuenta? Regístrate aquí
            </Link>
          </div>
        </div>
      </div>
    </>
  );
};
