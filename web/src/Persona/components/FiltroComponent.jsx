import { useState } from "react";
import { ButtonComponent } from "../../components/ButtonComponent";
import { FormularioComponent } from "../../components/FormularioComponent";
import { InputComponent } from "../../components/InputComponent";
import { SelectorTipoPersonaComponent } from "./SelectorTipoPersonaComponent";

const initialForm = {
  cedulaAsegurado: "",
  nombre: "",
  primerApellido: "",
  segundoApellido: "",
  idTipoPersona: "",
};

export const FiltroComponent = ({ filtro, setFiltro, bloquearBoton }) => {
  const [filtroInterno, setFiltroInterno] = useState(initialForm);

  const onInputChange = ({ target }) => {
    const { name, value } = target;
    setFiltroInterno({
      ...filtro,
      [name]: value,
    });
  };

  const onSubmit = () => {
    const formularioCambio = {
      ...filtroInterno,
      _dummy_change: new Date().getTime(),
    };

    setFiltro(formularioCambio);
  };

  const onResetForm = () => {
    setFiltro(initialForm);
    setFiltroInterno(initialForm);
  };

  return (
    <>
      <FormularioComponent>
        <div className="col-4">
          <InputComponent
            label={"Cédula Asegurado"}
            name={"cedulaAsegurado"}
            onChange={onInputChange}
            placeholder={"Cédula Asegurado"}
            value={filtroInterno?.cedulaAsegurado}
          />
        </div>
        <div className="col-4">
          <InputComponent
            label={"Nombre"}
            name={"nombre"}
            onChange={onInputChange}
            placeholder={"Nombre"}
            value={filtroInterno?.nombre}
          />
        </div>
        <div className="col-4">
          <InputComponent
            label={"Primer Apellido"}
            name={"primerApellido"}
            onChange={onInputChange}
            placeholder={"Primer Apellido"}
            value={filtroInterno?.primerApellido}
          />
        </div>
        <div className="col-4">
          <InputComponent
            label={"Segundo Apellido"}
            name={"segundoApellido"}
            onChange={onInputChange}
            placeholder={"Segundo Apellido"}
            value={filtroInterno?.segundoApellido}
          />
        </div>
        <div className="col-4">
          <SelectorTipoPersonaComponent
            nameTipoPersona="idTipoPersona"
            setValorSeleccionado={setFiltroInterno}
            valorSeleccionado={filtroInterno?.idTipoPersona}
          />
        </div>
      </FormularioComponent>
      <ButtonComponent
        onClickGuardar={onSubmit}
        onClickCancelar={onResetForm}
        labelBotonCancelar="Limpiar"
        labelBotonGuardar="Buscar"
        deshabilitarBotonGuardar={bloquearBoton}
      />
    </>
  );
};
