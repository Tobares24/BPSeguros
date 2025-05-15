/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { AlertaService } from "../../Services/AlertaService";
import { SelectorComponent } from "../../components/SelectorComponent";
import PolizaService from "../../Services/PolizaService";

export const SelectorEstadoComponent = ({
  isRequired = false,
  label = "Estado Póliza",
  nameEstado = "",
  setValorSeleccionado = () => {},
  valorSeleccionado = "",
  error = "",
  deshabilitar = false,
}) => {
  const [cargando, setCargando] = useState(false);
  const [data, setData] = useState([]);
  const [filtro, setFiltro] = useState(valorSeleccionado);

  const polizaService = new PolizaService();

  const buscarTipoPolizas = async () => {
    setCargando(true);
    try {
      const data = await polizaService.listaSelectorPolizaEstado(filtro);

      const datosTransformados = transformarDatos(data);

      const nuevosDatos = [
        {
          value: "null",
          label: "Seleccione",
        },
        ...datosTransformados,
      ];

      setData(nuevosDatos);
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
        label: item?.descripcion ?? "",
      };

      nuevosDatos.push(nuevosRegistros);
    });

    return nuevosDatos;
  };

  const handleSelect = (event) => {
    const selectedValue = event.target.value;

    setValorSeleccionado((prevData) => ({
      ...prevData,
      [nameEstado]: selectedValue,
    }));
  };

  useEffect(() => {
    buscarTipoPolizas();
  }, [filtro]);

  return (
    <div>
      <SelectorComponent
        options={data}
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
