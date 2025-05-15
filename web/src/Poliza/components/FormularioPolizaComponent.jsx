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
import { InputNumericComponent } from "../../components/InputNumericComponent";
import { SelectorCoberturaComponent } from "./SelectorCoberturaComponent";
import { SelectorEstadoComponent } from "./SelectorEstadoComponent";
import { SelectorPersonaComponent } from "../../Persona/components/SelectorPersonaComponent";
import { SelectorTipoPolizaComponent } from "./SelectorTipoPolizaComponent";
import { SpinnerComponent } from "../../components/SpinnerComponent";
import PolizaService from "../../Services/PolizaService";

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
};

const initialValidateModel = {
  numeroPoliza: false,
  idTipoPoliza: false,
  cedulaAsegurado: false,
  fechaVencimiento: false,
  idPolizaEstado: false,
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

  const polizaService = new PolizaService();

  const crearPoliza = async () => {
    setCargando(true);
    try {
      const fechaEmision = formatearFechaGuardar(formState.fechaEmision);
      const fechaInclusion = formatearFechaGuardar(formState.fechaInclusion);
      const fechaVencimiento = formatearFechaGuardar(
        formState.fechaVencimiento
      );
      const periodo = formatearFechaGuardar(formState.periodo);
      const montoAsegurado = formState.montoAsegurado
        ? Number(formState.montoAsegurado)
        : 0;
      const prima = formState.prima ? Number(formState.prima) : 0;

      const nuevoObjeto = {
        ...formState,
        fechaEmision,
        fechaInclusion,
        fechaVencimiento,
        periodo,
        montoAsegurado,
        prima,
      };

      await polizaService.crear(nuevoObjeto);

      setRefrescarTabla(true);
      setState();
      AlertaService.success("Exitoso", "La póliza ha sido creada con éxito.");
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
    } finally {
      setCargando(false);
    }
  };

  const obtenerPorId = async () => {
    setCargando(true);
    try {
      const objeto = await polizaService.obtenerPorId(id);

      const fechaEmision = formatearFechaISO(objeto?.fechaEmision);
      const fechaInclusion = formatearFechaISO(objeto?.fechaInclusion);
      const fechaVencimiento = formatearFechaISO(objeto?.fechaVencimiento);

      const nuevoObjeto = {
        ...objeto,
        fechaEmision,
        fechaInclusion,
        fechaVencimiento,
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
      const fechaEmision = formatearFechaGuardar(formState.fechaEmision);
      const fechaInclusion = formatearFechaGuardar(formState.fechaInclusion);
      const fechaVencimiento = formatearFechaGuardar(
        formState.fechaVencimiento
      );
      const periodo = formatearFechaGuardar(formState.periodo);
      const montoAsegurado = formState.montoAsegurado
        ? Number(formState.montoAsegurado)
        : 0;
      const prima = formState.prima ? Number(formState.prima) : 0;

      const nuevoObjeto = {
        ...formState,
        fechaEmision,
        fechaInclusion,
        fechaVencimiento,
        periodo,
        montoAsegurado,
        prima,
      };

      await polizaService.actualizar(nuevoObjeto, id);

      setRefrescarTabla(true);
      setState();
      AlertaService.success(
        "Exitoso",
        "La póliza ha sido actualizada con éxito."
      );
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
    } finally {
      setCargando(false);
    }
  };

  const setState = () => {
    onCancel(true);
    onResetForm();
  };

  const onSetRequeridos = (name) => {
    if (formState[name]) {
      setValidateModel((prevData) => ({
        ...prevData,
        [name]: false,
      }));
      setErrorModel((prevData) => ({
        ...prevData,
        [name]: "",
      }));
    }
  };

  const onSubmit = (event) => {
    event.preventDefault();

    const formularioInvalido = onValidateForm();

    if (formularioInvalido) return;

    AlertaService.confirmation(
      "Advertencia",
      "¿Está seguro que desea guardar la póliza?",
      async (respuesta) => {
        if (respuesta) {
          if (tipoAccion === ACCION_CREAR) {
            await crearPoliza();
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
    if (id) {
      setTipoAccion(ACCION_MODIFICAR);
      setDeshabilitar(true);
      obtenerPorId();
    }
  }, [id]);

  return (
    <>
      <SpinnerComponent show={cargando} />
      <FormularioComponent>
        <div className="col-6">
          <SelectorPersonaComponent
            isRequired={true}
            namePersona="cedulaAsegurado"
            setValorSeleccionado={(valor) => {
              setFormState(valor);
              onSetRequeridos("cedulaAsegurado");
            }}
            valorSeleccionado={formState.cedulaAsegurado}
            error={errorModel.cedulaAsegurado}
            deshabilitar={deshabilitar}
          />
        </div>
        <div className="col-6">
          <SelectorTipoPolizaComponent
            isRequired={true}
            nameTipoPoliza="idTipoPoliza"
            setValorSeleccionado={(valor) => {
              setFormState(valor);
              onSetRequeridos("idTipoPoliza");
            }}
            valorSeleccionado={formState.idTipoPoliza}
            error={errorModel.idTipoPoliza}
          />
        </div>
        <div className="col-6">
          <InputComponent
            label={"Número Póliza"}
            name={"numeroPoliza"}
            onChange={onInputChange}
            placeholder={"Número Póliza"}
            value={formState.numeroPoliza}
            requerido={true}
            error={errorModel.numeroPoliza}
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
            error={errorModel.fechaVencimiento}
            requerido={true}
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
            nameCobertura="idCobertura"
            setValorSeleccionado={setFormState}
            valorSeleccionado={formState.idCobertura}
            error={errorModel.idCobertura}
          />
        </div>
        <div className="col-6">
          <SelectorEstadoComponent
            isRequired={true}
            nameEstado="idPolizaEstado"
            setValorSeleccionado={(valor) => {
              setFormState(valor);
              onSetRequeridos("idPolizaEstado");
            }}
            valorSeleccionado={formState.idPolizaEstado}
            error={errorModel.idPolizaEstado}
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
          <DatePickerComponent
            label="Periodo"
            value={formState.periodo}
            onChange={onInputChange}
            name={"periodo"}
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
          />
        </div>
      </FormularioComponent>
      <ButtonComponent onClickGuardar={onSubmit} onClickCancelar={onCancelar} />
    </>
  );
};
