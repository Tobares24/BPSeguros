/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { AlertaService } from "../../Services/AlertaService";
import { ButtonComponent } from "../../components/ButtonComponent";
import { DatePickerComponent } from "../../components/DatePickerComponent";
import { formatearFechaGuardar } from "../../utils/FormateadorFecha";
import { FormularioComponent } from "../../components/FormularioComponent";
import { InputComponent } from "../../components/InputComponent";
import { SelectorTipoPersonaComponent } from "./SelectorTipoPersonaComponent";
import { SpinnerComponent } from "../../components/SpinnerComponent";
import { useForm } from "../../hooks/useForm";
import PersonaService from "../../Services/PersonaService";

const initialForm = {
  cedulaAsegurado: "",
  nombre: "",
  primerApellido: "",
  fechaNacimiento: "",
  idTipoPersona: "",
};

const initialValidateModel = {
  cedulaAsegurado: false,
  nombre: false,
  primerApellido: false,
  segundoApellido: false,
  idTipoPersona: false,
};

const initialErrorModel = {
  cedulaAsegurado: "",
  nombre: "",
  primerApellido: "",
  idTipoPersona: "",
  segundoApellido: "",
};

export const FormularioPersonaComponent = ({ onCancel, setRefrescarTabla }) => {
  const [cargando, setCargando] = useState(false);

  const {
    errorModel,
    formState,
    onInputChange,
    onResetForm,
    onValidateForm,
    setErrorModel,
    setFormState,
    setValidateModel,
    validateModel,
  } = useForm(initialForm, initialValidateModel, initialErrorModel);

  const personaService = new PersonaService();

  const crearPersona = async () => {
    setCargando(true);
    try {
      const fechaFormateada = formatearFechaGuardar(formState.fechaNacimiento);

      const nuevoObjeto = {
        ...formState,
        fechaNacimiento: fechaFormateada,
      };

      await personaService.crear(nuevoObjeto);

      setRefrescarTabla(true);

      AlertaService.success("Exitoso", "La persona ha sido creada con éxito.");
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
    } finally {
      setCargando(false);
      setRefrescarTabla(false);
    }
  };

  const onSubtmit = (event) => {
    event.preventDefault();

    const formularioInvalido = onValidateForm();

    if (formularioInvalido) return;

    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea guardar la persona?",
      (respuesta) => {
        if (respuesta) {
          crearPersona();
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
          onResetForm();
          onCancel(true);
        }
      }
    );
  };

  useEffect(() => {
    if (formState.idTipoPersona) {
      setValidateModel((prevData) => ({
        ...prevData,
        ["idTipoPersona"]: false,
      }));
      setErrorModel((prevData) => ({
        ...prevData,
        ["idTipoPersona"]: "",
      }));
    }
  }, [formState.idTipoPersona]);

  return (
    <>
      <SpinnerComponent show={cargando} />
      <FormularioComponent>
        <div className="col-6">
          <InputComponent
            label={"Cédula Asegurado"}
            name={"cedulaAsegurado"}
            onChange={onInputChange}
            placeholder={"Cédula Asegurado"}
            value={formState.cedulaAsegurado}
            requerido={true}
            error={errorModel.cedulaAsegurado}
          />
        </div>
        <div className="col-6">
          <InputComponent
            label={"Nombre"}
            name={"nombre"}
            onChange={onInputChange}
            placeholder={"Nombre"}
            value={formState.nombre}
            requerido={true}
            error={errorModel.nombre}
          />
        </div>
        <div className="col-6">
          <InputComponent
            label={"Primer Apellido"}
            name={"primerApellido"}
            onChange={onInputChange}
            placeholder={"Primer Apellido"}
            value={formState.primerApellido}
            requerido={true}
            error={errorModel.primerApellido}
          />
        </div>
        <div className="col-6">
          <InputComponent
            label={"Segundo Apellido"}
            name={"segundoApellido"}
            onChange={onInputChange}
            placeholder={"Segundo Apellido"}
            value={formState.segundoApellido}
            requerido={true}
            error={errorModel.segundoApellido}
          />
        </div>
        <div className="col-6">
          <SelectorTipoPersonaComponent
            isRequired={validateModel.idTipoPersona}
            nameTipoPersona="idTipoPersona"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.idTipoPersona}
            error={errorModel.idTipoPersona}
          />
        </div>
        <div className="col-6">
          <DatePickerComponent
            label="Fecha Nacimiento"
            value={formState.fechaNacimiento}
            onChange={onInputChange}
            name={"fechaNacimiento"}
          />
        </div>
      </FormularioComponent>
      <ButtonComponent
        onClickGuardar={onSubtmit}
        onClickCancelar={onCancelar}
      />
    </>
  );
};
