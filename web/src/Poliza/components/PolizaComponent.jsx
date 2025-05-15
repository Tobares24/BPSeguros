import { useEffect, useState } from "react";
import { AlertaService } from "../../Services/AlertaService";
import { FormularioPolizaComponent } from "./FormularioPolizaComponent";
import { ModalComponent } from "../../components/ModalComponent";
import { TablaPolizaComponent } from "./TablaPolizaComponent";

export const PolizaComponent = () => {
  const [mostrarModal, setMostrarModal] = useState(false);
  const [refrescarTabla, setRefrescarTabla] = useState(false);
  const [idSeleccionado, setIdSeleccionado] = useState("");

  const onSetEstados = () => {
    setMostrarModal(false);
    setIdSeleccionado("");
  };

  const onCancelar = (event) => {
    event.preventDefault();

    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea salir?",
      (respuesta) => {
        if (respuesta) {
          onSetEstados();
        }
      }
    );
  };

  useEffect(() => {
    if (idSeleccionado) {
      setMostrarModal(true);
    }
  }, [idSeleccionado]);

  return (
    <>
      <div className="container mt-5">
        <TablaPolizaComponent
          refrescarTabla={refrescarTabla}
          setIdSeleccionado={setIdSeleccionado}
          setRefrescarTabla={setRefrescarTabla}
        >
          <button
            className="btn btn-primary"
            onClick={() => setMostrarModal(true)}
          >
            Crear
          </button>
        </TablaPolizaComponent>
        {mostrarModal && (
          <ModalComponent
            title={"Formulario Póliza"}
            onCancel={onCancelar}
            tamnnioModal="modal-xl"
          >
            <FormularioPolizaComponent
              onCancel={onSetEstados}
              setRefrescarTabla={setRefrescarTabla}
              id={idSeleccionado}
            />
          </ModalComponent>
        )}
      </div>
    </>
  );
};
