import { BrowserRouter, Route, Routes } from "react-router-dom";
import { PersonaComponent } from "./Persona/components/PersonaComponent";
import { PolizaComponent } from "./Poliza/components/PolizaComponent";
import { SideBarComponent } from "./components/SideBarComponent";

export const AppComponent = () => {
  return (
    <>
      <BrowserRouter>
        <SideBarComponent>
          <div className="container mt-4">
            <Routes>
              <Route path="/persona" element={<PersonaComponent />} />
              <Route path="/polizas" element={<PolizaComponent />} />
            </Routes>
          </div>
        </SideBarComponent>
      </BrowserRouter>
    </>
  );
};
