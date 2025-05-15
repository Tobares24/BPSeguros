import { useState } from "react";
import { useForm } from "../../hooks/useForm";
import { SpinnerComponent } from "../../components/SpinnerComponent";
import { FormularioComponent } from "../../components/FormularioComponent";
import { InputComponent } from "../../components/InputComponent";
import { SelectorTipoPolizaComponent } from "./SelectorTipoPolizaComponent";
import { SelectorCoberturaComponent } from "./SelectorCoberturaComponent";
import { SelectorEstadoComponent } from "./SelectorEstadoComponent";
import { SelectorPeriodoComponent } from "./SelectorPeriodoComponent";
import { SelectorPersonaComponent } from "../../Persona/components/SelectorPersonaComponent";
import { InputNumericComponent } from "../../components/InputNumericComponent";
import { AlertaService } from "../../Services/AlertaService";

const initialForm = {
  numeroPoliza: "",
  idTipoPoliza: "",
  cedulaAsegurado: "",
  montoAsegurado: "",
  fechaVencimiento: "",
  fechaEmision: "",
  idCobertura: "",
  idPolizaEstado: "",
  prima: "",
  periodo: "",
  fechaInclusion: "",
  aseguradora: "",
  idPeriodo: "",
};

const initialValidateModel = {
  numeroPoliza: "",
  idTipoPoliza: "",
  cedulaAsegurado: "",
  fechaVencimiento: "",
  idPolizaEstado: "",
};

const initialErrorModel = {
  numeroPoliza: "",
  idTipoPoliza: "",
  cedulaAsegurado: "",
  fechaVencimiento: "",
  idPolizaEstado: "",
};

const ACCION_CREAR = "CREAR";
const ACCION_MODIFICAR = "MODIFICAR";

export const FormularioPolizaComponent = ({
  onCancel,
  setRefrescarTabla,
  id,
}) => {
  const [cargando, setCargando] = useState(false);
  const [tipoAccion, setTipoAccion] = useState(ACCION_CREAR);
  const [deshabilitar, setDeshabilitar] = useState(false);

  const {
    errorModel,
    formState,
    onInputChange,
    onResetForm,
    onValidateForm,
    setErrorModel,
    setFormState,
    setValidateModel,
  } = useForm(initialForm, initialValidateModel, initialErrorModel);

  const setState = () => {
    onResetForm();
    onCancel(true);
  };

  const onSubmit = (event) => {
    event.preventDefault();

    const formularioInvalido = onValidateForm();

    if (formularioInvalido) return;

    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea guardar la persona?",
      async (respuesta) => {
        if (respuesta) {
          if (tipoAccion === ACCION_CREAR) {
          } else if (tipoAccion === ACCION_MODIFICAR) {
          }
        }
      }
    );
  };

  const onCancelar = (event) => {
    event.preventDefault();

    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea salir?",
      (respuesta) => {
        if (respuesta) {
          setState();
        }
      }
    );
  };

  return (
    <>
      <SpinnerComponent show={cargando} />
      <FormularioComponent>
        <div className="col-6">
          <InputComponent
            label={"Número Póliza"}
            name={"numeroPoliza"}
            onChange={onInputChange}
            placeholder={"Número Póliza"}
            value={formState.numeroPoliza}
            requerido={true}
            error={errorModel.numeroPoliza}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <SelectorTipoPolizaComponent
            isRequired={true}
            nameTipoPoliza="idTipoPoliza"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.idTipoPoliza}
            error={errorModel.idTipoPoliza}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <SelectorPersonaComponent
            isRequired={true}
            nameTipoPoliza="cedulaAsegurado"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.cedulaAsegurado}
            error={errorModel.cedulaAsegurado}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <InputNumericComponent
            decimales={4}
            label="Monto Asegurado"
            placeholder="Monto Asegurado"
            name="montoAsegurado"
            setForm={setFormState}
            valor={formState.montoAsegurado}
          />
        </div>
        <div className="col-6">
          <DatePickerComponent
            label="Fecha Vencimiento"
            value={formState.fechaVencimiento}
            onChange={onInputChange}
            name={"fechaVencimiento"}
          />
        </div>
        <div className="col-6">
          <DatePickerComponent
            label="Fecha Emisión"
            value={formState.fechaEmision}
            onChange={onInputChange}
            name={"fechaEmision"}
          />
        </div>
        <div className="col-6">
          <SelectorCoberturaComponent
            isRequired={true}
            nameTipoPoliza="idCobertura"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.idCobertura}
            error={errorModel.idCobertura}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <SelectorEstadoComponent
            isRequired={true}
            nameTipoPoliza="idPolizaEstado"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.idPolizaEstado}
            error={errorModel.idPolizaEstado}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <InputNumericComponent
            decimales={4}
            label="Prima"
            placeholder="Prima"
            name="prima"
            setForm={setFormState}
            valor={formState.prima}
          />
        </div>
        <div className="col-6">
          <SelectorPeriodoComponent
            isRequired={true}
            nameTipoPoliza="idPeriodo"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.idPeriodo}
            error={errorModel.idPeriodo}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <DatePickerComponent
            label="Fecha Inclusión"
            value={formState.fechaInclusion}
            onChange={onInputChange}
            name={"fechaInclusion"}
          />
        </div>
        <div className="col-6">
          <InputComponent
            label={"Aseguradora"}
            name={"aseguradora"}
            onChange={onInputChange}
            placeholder={"Aseguradora"}
            value={formState.aseguradora}
            error={errorModel.aseguradora}
            deshabilitar={deshabilitar}
          />
        </div>
      </FormularioComponent>
      <ButtonComponent onClickGuardar={onSubmit} onClickCancelar={onCancelar} />
    </>
  );
};
