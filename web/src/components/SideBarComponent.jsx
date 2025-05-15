import { useState } from "react";
import { NavLink } from "react-router-dom";

export const SideBarComponent = ({ children }) => {
  const [collapsed, setCollapsed] = useState(false);

  const toggleCollapsed = () => {
    setCollapsed(!collapsed);
  };

  return (
    <>
      <link
        href="https://fonts.googleapis.com/icon?family=Material+Icons"
        rel="stylesheet"
      />
      <div className="d-flex">
        <div
          className={`bg-light border-end vh-100 p-3 ${
            collapsed ? "collapsed-sidebar" : "sidebar"
          }`}
          style={{
            width: collapsed ? "80px" : "250px",
            transition: "width 0.3s",
          }}
        >
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
              to="/polizas"
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

        <div className="flex-grow-1 p-3">{children}</div>

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
          /* Material icons size & alignment */
          .material-icons {
            font-size: 24px;
          }
        `}</style>
      </div>
    </>
  );
};
