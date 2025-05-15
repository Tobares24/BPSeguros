import { useEffect, useState } from "react";
import { AlertaService } from "../../Services/AlertaService";
import { FormularioPolizaComponent } from "./FormularioPolizaComponent";
import { ModalComponent } from "../../components/ModalComponent";

export const PolizaComponent = () => {
  const [mostrarModal, setMostrarModal] = useState(false);
  const [refrescarTabla, setRefrescarTabla] = useState(false);
  const [idSeleccionado, setIdSeleccionado] = useState("");

  const onCancelar = (event) => {
    event.preventDefault();

    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea salir?",
      (respuesta) => {
        if (respuesta) {
          setMostrarModal(false);
          setIdSeleccionado("");
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
      <button className="btn btn-primary" onClick={() => setMostrarModal(true)}>
        Crear
      </button>
      {mostrarModal && (
        <ModalComponent title={"Formulario Póliza"} onCancel={onCancelar} tamnnioModal="modal-xl">
          <FormularioPolizaComponent
            onCancel={() => setMostrarModal(false)}
            setRefrescarTabla={setRefrescarTabla}
            id={idSeleccionado}
          />
        </ModalComponent>
      )}
    </>
  );
};
