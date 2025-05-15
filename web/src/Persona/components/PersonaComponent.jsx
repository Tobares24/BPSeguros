import { useEffect, useState } from "react";
import { AlertaService } from "../../Services/AlertaService";
import { ModalComponent } from "../../components/ModalComponent";
import { FormularioPersonaComponent } from "./FormularioPersonaComponent ";
import { TablaPersonaComponent } from "./TablaPersonaComponent";

export const PersonaComponent = () => {
  const [mostrarModal, setMostrarModal] = useState(false);
  const [refrescarTabla, setRefrescarTabla] = useState(false);
  const [cedulaSeleccionada, setCedulaSeleccionada] = useState("");

  const onCancelar = (event) => {
    event.preventDefault();

    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea salir?",
      (respuesta) => {
        if (respuesta) {
          setMostrarModal(false);
          setCedulaSeleccionada("");
        }
      }
    );
  };

  useEffect(() => {
    if (cedulaSeleccionada) {
      setMostrarModal(true);
    }
  }, [cedulaSeleccionada]);

  return (
    <>
      <div className="container mt-5">
        <TablaPersonaComponent
          refrescarTabla={refrescarTabla}
          setRefrescarTabla={setRefrescarTabla}
          setCedulaSeleccionada={setCedulaSeleccionada}
        >
          <button
            className="btn btn-primary"
            onClick={() => setMostrarModal(true)}
          >
            Crear
          </button>
        </TablaPersonaComponent>
        {mostrarModal && (
          <ModalComponent title={"Formulario Persona"} onCancel={onCancelar}>
            <FormularioPersonaComponent
              onCancel={() => setMostrarModal(false)}
              setRefrescarTabla={setRefrescarTabla}
              cedulaSeleccionada={cedulaSeleccionada}
            />
          </ModalComponent>
        )}
      </div>
    </>
  );
};
