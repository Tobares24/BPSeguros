import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import { IniciarSesionComponent } from "./Seguridad/components/IniciarSesionComponent";
import { RegistrarComponent } from "./Seguridad/components/RegistrarComponent";
import { PersonaComponent } from "./Persona/components/PersonaComponent";
import { PolizaComponent } from "./Poliza/components/PolizaComponent";
import { SideBarComponent } from "./components/SideBarComponent";

const isAuthenticated = () => {
  return !!localStorage.getItem("token");
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
          path="/*"
          element={
            <PrivateRoute>
              <SideBarComponent>
                <div className="container mt-4">
                  <Routes>
                    <Route path="persona" element={<PersonaComponent />} />
                    <Route path="polizas" element={<PolizaComponent />} />
                    <Route
                      path="*"
                      element={<Navigate to="persona" replace />}
                    />
                  </Routes>
                </div>
              </SideBarComponent>
            </PrivateRoute>
          }
        />
        <Route
          path="/"
          element={
            isAuthenticated() ? (
              <Navigate to="/persona" replace />
            ) : (
              <Navigate to="/login" replace />
            )
          }
        />
      </Routes>
    </BrowserRouter>
  );
};
