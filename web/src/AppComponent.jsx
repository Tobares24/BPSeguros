import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import { IniciarSesionComponent } from "./Seguridad/components/IniciarSesionComponent";
import { RegistrarComponent } from "./Seguridad/components/RegistrarComponent";
import { PersonaComponent } from "./Persona/components/PersonaComponent";
import { PolizaComponent } from "./Poliza/components/PolizaComponent";
import { SideBarComponent } from "./components/SideBarComponent";
import { InicioComponent } from "./components/InicioComponent";

const isAuthenticated = () => {
  return !!sessionStorage.getItem("token");
};

const PrivateRoute = ({ children }) => {
  return isAuthenticated() ? children : <Navigate to="/login" replace />;
};

export const AppComponent = () => {
  return (
    <BrowserRouter>
      <Routes>
        <Route path="/login" element={<IniciarSesionComponent />} />
        <Route path="/registro" element={<RegistrarComponent />} />
        <Route
          path="*"
          element={
            <PrivateRoute>
              <SideBarComponent />
            </PrivateRoute>
          }
        >
          <Route path="inicio" element={<InicioComponent />} />
          <Route path="persona" element={<PersonaComponent />} />
          <Route path="poliza" element={<PolizaComponent />} />
          <Route path="*" element={<Navigate to="/" replace />} />
        </Route>
        <Route
          path="/"
          element={
            isAuthenticated() ? (
              <Navigate to="/" replace />
            ) : (
              <Navigate to="/login" replace />
            )
          }
        />
      </Routes>
    </BrowserRouter>
  );
};
