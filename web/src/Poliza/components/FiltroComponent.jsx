import { useState } from "react";
import { InputComponent } from "../../components/InputComponent";
import { SelectorPersonaComponent } from "../../Persona/components/SelectorPersonaComponent";
import { SelectorTipoPolizaComponent } from "./SelectorTipoPolizaComponent";
import { DatePickerComponent } from "../../components/DatePickerComponent";
import { ButtonComponent } from "../../components/ButtonComponent";
import { FormularioComponent } from "../../components/FormularioComponent";

const initialForm = {
  cedulaAsegurado: "",
  numeroPoliza: "",
  fechaVencimiento: "",
  idTipoPoliza: "",
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
        <div className="col-6">
          <SelectorPersonaComponent
            namePersona="cedulaAsegurado"
            setValorSeleccionado={setFiltroInterno}
            valorSeleccionado={filtroInterno.cedulaAsegurado}
          />
        </div>
        <div className="col-6">
          <SelectorTipoPolizaComponent
            nameTipoPoliza="idTipoPoliza"
            setValorSeleccionado={setFiltroInterno}
            valorSeleccionado={filtroInterno.idTipoPoliza}
          />
        </div>
        <div className="col-6">
          <InputComponent
            label={"Número Póliza"}
            name={"numeroPoliza"}
            onChange={onInputChange}
            placeholder={"Número Póliza"}
            value={filtroInterno?.numeroPoliza}
          />
        </div>
        <div className="col-6">
          <DatePickerComponent
            label="Fecha Vencimiento"
            value={filtroInterno.fechaVencimiento}
            onChange={onInputChange}
            name={"fechaVencimiento"}
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
