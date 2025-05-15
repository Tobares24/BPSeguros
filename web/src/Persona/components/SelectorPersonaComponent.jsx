/* eslint-disable react-hooks/exhaustive-deps */
import {  useEffect, useState } from "react";
import { AlertaService } from "../../Services/AlertaService";
import { SelectorComponent } from "../../components/SelectorComponent";
import PersonaService from "../../Services/PersonaService";

export const SelectorPersonaComponent = ({
  isRequired = false,
  label = "Personas",
  namePersona = "",
  setValorSeleccionado = () => {},
  valorSeleccionado = "",
  error = "",
  deshabilitar = false,
}) => {
  const [cargando, setCargando] = useState(false);
  const [filtro, setFiltro] = useState(valorSeleccionado);
  const [personas, setPersonas] = useState([]);

  const personaService = new PersonaService();

  const buscarPersonas = async () => {
    setCargando(true);
    try {
      const data = await personaService.listaSelectorPersona(filtro);

      const datosTransformados = transformarDatos(data);

      const nuevosDatos = [
        {
          value: "null",
          label: "Seleccione",
        },
        ...datosTransformados,
      ];

      setPersonas(nuevosDatos);
    } catch (e) {
      AlertaService.error(
        "Error",
        e?.Message ?? "Ha ocurrido un error interno."
      );
    } finally {
      setCargando(false);
    }
  };

  const transformarDatos = (datos) => {
    let nuevosDatos = [];

    datos?.map((item) => {
      const nuevosRegistros = {
        value: item?.cedulaAsegurado,
        label: `${item?.cedulaAsegurado} - ${item?.nombre} ${item?.primerApellido} ${item?.segundoApellido}`,
      };

      nuevosDatos.push(nuevosRegistros);
    });

    return nuevosDatos;
  };

  const handleSelect = (event) => {
    const selectedValue = event.target.value;

    setValorSeleccionado((prevData) => ({
      ...prevData,
      [namePersona]: selectedValue,
    }));
  };

  useEffect(() => {
    buscarPersonas();
  }, [filtro]);

  return (
    <div>
      <SelectorComponent
        options={personas}
        loading={cargando}
        onSearch={setFiltro}
        value={filtro || valorSeleccionado}
        onSelect={handleSelect}
        placeholder="Buscar"
        label={label}
        deshabilitar={deshabilitar}
        error={error}
        isRequired={isRequired}
      />
    </div>
  );
};
