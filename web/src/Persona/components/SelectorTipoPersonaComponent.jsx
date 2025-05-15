/* eslint-disable react-hooks/exhaustive-deps */
import { AlertaService } from "../../Services/AlertaService";
import { SelectorComponent } from "../../components/SelectorComponent";
import { useEffect, useState } from "react";
import PersonaService from "../../Services/PersonaService";

export const SelectorTipoPersonaComponent = ({
  isRequired = false,
  label = "Tipo Persona",
  nameTipoPersona = "",
  setValorSeleccionado = () => {},
  valorSeleccionado = "",
  error = "",
  deshabilitar = false,
}) => {
  const [cargando, setCargando] = useState(false);
  const [filtro, setFiltro] = useState(valorSeleccionado);
  const [tipoPersonas, setTipoPersonas] = useState([]);

  const personaService = new PersonaService();

  const buscarTipoPersonas = async () => {
    setCargando(true);
    try {
      const data = await personaService.listaSelectorTipoPersona(filtro);

      const datosTransformados = transformarDatos(data);

      const nuevosDatos = [
        {
          value: "null",
          label: "Seleccione",
        },
        ...datosTransformados,
      ];

      setTipoPersonas(nuevosDatos);
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
        value: item?.id,
        label: item?.tipoPersona,
      };

      nuevosDatos.push(nuevosRegistros);
    });

    return nuevosDatos;
  };

  const handleSelect = (event) => {
    const selectedValue = event.target.value;

    setValorSeleccionado((prevData) => ({
      ...prevData,
      [nameTipoPersona]: selectedValue,
    }));
  };

  useEffect(() => {
    buscarTipoPersonas();
  }, [filtro]);

  return (
    <div>
      <SelectorComponent
        options={tipoPersonas}
        loading={cargando}
        onSearch={setFiltro}
        value={valorSeleccionado}
        onSelect={handleSelect}
        placeholder="Buscar"
        isRequired={isRequired}
        error={error}
        deshabilitar={deshabilitar}
        label={label}
      />
    </div>
  );
};
