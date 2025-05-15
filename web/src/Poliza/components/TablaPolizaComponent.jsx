/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { AccordionComponent } from "../../components/AccordionComponent";
import { AlertaService } from "../../Services/AlertaService";
import { FiltroComponent } from "./FiltroComponent";
import { TablaComponent } from "../../components/TablaComponent";
import PolizaService from "../../Services/PolizaService";

const columnas = [
  { nombre: "numeroPoliza", titulo: "Número Póliza" },
  { nombre: "cedulaAsegurado", titulo: "Cédula Asegurado" },
  { nombre: "nombreCompleto", titulo: "Nombre Asegurado" },
  { nombre: "tipoPoliza", titulo: "Tipo Póliza" },
  {
    nombre: "fechaVencimiento",
    titulo: "Fecha Vencimiento",
    render: (fecha) => {
      if (!fecha) return "";
      return new Date(fecha).toLocaleDateString("es-ES", {
        year: "numeric",
        month: "long",
        day: "numeric",
      });
    },
  },
  { nombre: "polizaEstado", titulo: "Estado" },
];

export const TablaPolizaComponent = ({
  refrescarTabla,
  setRefrescarTabla,
  setIdSeleccionado,
  children,
}) => {
  const [filtro, setFiltro] = useState({
    cedulaAsegurado: "",
    numeroPoliza: "",
    fechaVencimiento: "",
    idTipoPoliza: "",
  });
  const [cargando, setCargando] = useState(false);
  const [bloquearBoton, setBloquearBoton] = useState(false);
  const [data, setData] = useState([]);
  const [paginaActual, setPaginaActual] = useState(1);
  const [registroPorPagina, setRegistroPorPagina] = useState(10);
  const [cantidadRegistros, setCantidadRegistros] = useState(0);
  const [cantidadPaginas, setCantidadPaginas] = useState(0);

  const polizaService = new PolizaService();

  const obtener = async () => {
    setCargando(true);
    setBloquearBoton(true);
    try {
      const respuesta = await polizaService.obtener({
        ...filtro,
        registroPorPagina,
        paginaActual,
      });

      if (respuesta && respuesta.registros) {
        setData(respuesta.registros);
        setCantidadPaginas(respuesta.cantidadPaginas);
        setPaginaActual(respuesta.paginaActual);
        setCantidadRegistros(respuesta.cantidadRegistros);
      }
    } catch (error) {
      AlertaService.error("¡Error!", `${error?.message}`);
    } finally {
      setCargando(false);
      setBloquearBoton(false);
    }
  };

  const onSelectChange = (e) => {
    const selectedValue = e.target.value;
    setRegistroPorPagina(Number(selectedValue));
  };

  const onUpdate = ({ id }) => {
    setIdSeleccionado(id);
  };

  const acciones = [
    {
      label: "Editar",
      color: "primary",
      onClick: (objeto) => {
        onUpdate(objeto);
      },
    },
    {
      label: "Eliminar",
      color: "danger",
      onClick: (objeto) => {},
    },
  ];

  useEffect(() => {
    setRefrescarTabla(false);
    obtener();
  }, [filtro, refrescarTabla, registroPorPagina, paginaActual]);

  return (
    <>
      <AccordionComponent title={"Filtros de Búsqueda"}>
        <FiltroComponent
          filtro={filtro}
          setFiltro={setFiltro}
          bloquearBoton={bloquearBoton}
        />
      </AccordionComponent>
      <TablaComponent
        titulo="Listado de Pólizas"
        columnas={columnas}
        datos={data}
        paginaActual={paginaActual}
        cantidadPaginas={cantidadPaginas}
        cantidadRegistros={cantidadRegistros}
        onPaginaCambio={setPaginaActual}
        cargando={cargando}
        acciones={acciones}
      >
        <div className="d-flex justify-content-between mb-3">
          <div>
            <label htmlFor="registroPorPagina" className="mx-2">
              Registros por página:{" "}
            </label>
            <select
              id="registroPorPagina"
              value={registroPorPagina}
              onChange={onSelectChange}
            >
              <option value={10}>10</option>
              <option value={25}>25</option>
              <option value={50}>50</option>
              <option value={100}>100</option>
            </select>
          </div>
          <div className="ml-auto">{children}</div>
        </div>
      </TablaComponent>
    </>
  );
};
