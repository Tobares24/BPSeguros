import { useState } from "react";
import { NavLink, Outlet, useNavigate } from "react-router-dom";
import { AlertaService } from "../Services/AlertaService";

export const SideBarComponent = () => {
  const [collapsed, setCollapsed] = useState(false);
  const navigate = useNavigate();

  const toggleCollapsed = () => setCollapsed(!collapsed);

  const logout = () => {
    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea cerrar sesión?",
      (respuesta) => {
        if (respuesta) {
          sessionStorage.clear();
          navigate("/login");
        }
      },
      "Confirmar"
    );
  };

  const email = sessionStorage.getItem("email");

  return (
    <>
      <link
        href="https://fonts.googleapis.com/icon?family=Material+Icons"
        rel="stylesheet"
      />
      <div className="d-flex">
        <div
          className={`bg-light border-end vh-100 p-3 d-flex flex-column justify-content-between ${
            collapsed ? "collapsed-sidebar" : "sidebar"
          }`}
          style={{
            width: collapsed ? "80px" : "250px",
            transition: "width 0.3s",
          }}
        >
          <div>
            <button
              className="btn btn-outline-primary mb-3 w-100"
              onClick={toggleCollapsed}
              aria-label="Toggle menu"
            >
              <span className="material-icons">
                {collapsed ? "menu" : "close"}
              </span>
            </button>

            <nav className="nav flex-column">
              <NavLink
                to="/inicio"
                className={({ isActive }) =>
                  "nav-link d-flex align-items-center gap-2" +
                  (isActive ? " active" : "")
                }
                title="Inicio"
              >
                <span className="material-icons">home</span>
                {!collapsed && "Inicio"}
              </NavLink>

              <NavLink
                to="/persona"
                className={({ isActive }) =>
                  "nav-link d-flex align-items-center gap-2" +
                  (isActive ? " active" : "")
                }
                title="Personas"
              >
                <span className="material-icons">people</span>
                {!collapsed && "Personas"}
              </NavLink>

              <NavLink
                to="/poliza"
                className={({ isActive }) =>
                  "nav-link d-flex align-items-center gap-2" +
                  (isActive ? " active" : "")
                }
                title="Pólizas"
              >
                <span className="material-icons">assignment</span>
                {!collapsed && "Pólizas"}
              </NavLink>
            </nav>
          </div>
          <div className="border-top pt-3">
            <div className="text-muted small mb-2 text-center">
              {!collapsed && email}
            </div>
            <button
              className="btn btn-outline-danger w-100 d-flex align-items-center justify-content-center gap-2"
              onClick={logout}
              title="Cerrar sesión"
            >
              <span className="material-icons">logout</span>
              {!collapsed && "Cerrar sesión"}
            </button>
          </div>
        </div>

        <div className="flex-grow-1 p-3">
          <Outlet />
        </div>

        <style>{`
          .sidebar .nav-link {
            padding: 10px 15px;
            font-weight: 500;
            color: #000;
            text-decoration: none;
          }
          .sidebar .nav-link.active {
            background-color: #0d6efd;
            color: white !important;
            border-radius: 5px;
          }
          .collapsed-sidebar .nav-link {
            justify-content: center;
            padding: 10px 0;
            font-size: 1.3rem;
          }
          .collapsed-sidebar .nav-link.active {
            background-color: #0d6efd;
            color: white !important;
            border-radius: 5px;
          }
          .material-icons {
            font-size: 24px;
          }
        `}</style>
      </div>
    </>
  );
};
