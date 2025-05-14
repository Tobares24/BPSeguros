/* eslint-disable react-hooks/exhaustive-deps */
import { alertService } from "../../Services/AlertaService";
import { SelectorComponent } from "../../components/SelectorComponent";
import { useEffect, useState } from "react";
import PersonaService from "../../Services/PersonaService";

export const SelectorPersonaComponent = ({
  valorSeleccionado = "",
  setValorSeleccionado = () => {},
  namePersona = "",
}) => {
  const [filtro, setFiltro] = useState(valorSeleccionado);
  const [personas, setPersonas] = useState([]);
  const [cargando, setCargando] = useState(false);

  const personaService = new PersonaService();

  const buscarPersonas = async () => {
    setCargando(true);
    try {
      const data = await personaService.listaSelectorPersona(filtro);
      setPersonas(data);
    } catch (e) {
      alertService.error(
        "Error",
        e?.Message ?? "Ha ocurrido un error interno."
      );
    } finally {
      setCargando(false);
    }
  };

  const handleSelect = (selected) => {
    setValorSeleccionado((prevData) => ({
      ...prevData,
      [namePersona]: selected,
    }));
  };

  useEffect(() => {
    buscarPersonas();
  }, [filtro]);

  return (
    <div>
      <label>Personas</label>
      <SelectorComponent
        options={personas}
        loading={cargando}
        onSearch={setFiltro}
        value={filtro}
        onSelect={handleSelect}
        placeholder="Buscar"
      />
    </div>
  );
};
