/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { useForm } from "../../hooks/useForm";
import { AlertaService } from "../../Services/AlertaService";
import { ButtonComponent } from "../../components/ButtonComponent";
import { DatePickerComponent } from "../../components/DatePickerComponent";
import {
  formatearFechaGuardar,
  formatearFechaISO,
} from "../../utils/FormateadorFecha";
import { FormularioComponent } from "../../components/FormularioComponent";
import { InputComponent } from "../../components/InputComponent";
import { SelectorTipoPersonaComponent } from "./SelectorTipoPersonaComponent";
import { SpinnerComponent } from "../../components/SpinnerComponent";
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
  idTipoPersona: false,
};

const initialErrorModel = {
  cedulaAsegurado: "",
  nombre: "",
  primerApellido: "",
  idTipoPersona: "",
};

const ACCION_CREAR = "CREAR";
const ACCION_MODIFICAR = "MODIFICAR";

export const FormularioPersonaComponent = ({
  onCancel,
  setRefrescarTabla,
  cedulaSeleccionada,
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
      setState();
      AlertaService.success("Exitoso", "La persona ha sido creada con éxito.");
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
    } finally {
      setCargando(false);
    }
  };

  const obtenerPorId = async () => {
    setCargando(true);
    try {
      const objeto = await personaService.obtenerPorId(cedulaSeleccionada);

      const fechaFormateada = formatearFechaISO(objeto?.fechaNacimiento);

      const nuevoObjeto = {
        ...objeto,
        fechaNacimiento: fechaFormateada,
      };

      setFormState(nuevoObjeto);
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
    } finally {
      setCargando(false);
    }
  };

  const actualizarPersona = async () => {
    setCargando(true);
    try {
      const fechaFormateada = formatearFechaGuardar(formState.fechaNacimiento);

      const nuevoObjeto = {
        ...formState,
        fechaNacimiento: fechaFormateada,
      };

      await personaService.actualizar(nuevoObjeto, cedulaSeleccionada);

      setRefrescarTabla(true);
      setState();
      AlertaService.success(
        "Exitoso",
        "La persona ha sido actualizada con éxito."
      );
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
    } finally {
      setCargando(false);
    }
  };

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
            await crearPersona();
          } else if (tipoAccion === ACCION_MODIFICAR) {
            await actualizarPersona();
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

  useEffect(() => {
    if (cedulaSeleccionada) {
      setTipoAccion(ACCION_MODIFICAR);
      setDeshabilitar(true);
      obtenerPorId();
    }
  }, [cedulaSeleccionada]);

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
            deshabilitar={deshabilitar}
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
            deshabilitar={deshabilitar}
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
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <InputComponent
            label={"Segundo Apellido"}
            name={"segundoApellido"}
            onChange={onInputChange}
            placeholder={"Segundo Apellido"}
            value={formState.segundoApellido}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <SelectorTipoPersonaComponent
            isRequired={true}
            nameTipoPersona="idTipoPersona"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.idTipoPersona}
            error={errorModel.idTipoPersona}
            deshabilitar={deshabilitar}
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
      <ButtonComponent onClickGuardar={onSubmit} onClickCancelar={onCancelar} />
    </>
  );
};
