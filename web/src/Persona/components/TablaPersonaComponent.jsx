/* eslint-disable react-hooks/exhaustive-deps */
import { useEffect, useState } from "react";
import { AccordionComponent } from "../../components/AccordionComponent";
import { AlertaService } from "../../Services/AlertaService";
import { FiltroComponent } from "./FiltroComponent";
import { TablaComponent } from "../../components/TablaComponent";
import PersonaService from "../../Services/PersonaService";

const columnas = [
  { nombre: "cedulaAsegurado", titulo: "Cédula" },
  { nombre: "nombre", titulo: "Nombre" },
  { nombre: "primerApellido", titulo: "Primer Apellido" },
  { nombre: "segundoApellido", titulo: "Segundo Apellido" },
  { nombre: "tipoPersona", titulo: "Tipo Persona" },
  {
    nombre: "fechaNacimiento",
    titulo: "Fecha Nacimiento",
    render: (fecha) => {
      return new Date(fecha).toLocaleDateString("es-ES", {
        year: "numeric",
        month: "long",
        day: "numeric",
      });
    },
  },
];

// const personasFicticias = [
//   {
//     cedulaAsegurado: "001-150394-0001",
//     fechaNacimiento: "1994-03-15",
//     nombre: "Carlos",
//     primerApellido: "Gómez",
//     segundoApellido: "Ramírez",
//     tipoPersona: "Física",
//   },
//   {
//     cedulaAsegurado: "002-221280-0002",
//     fechaNacimiento: "1980-12-22",
//     nombre: "Ana",
//     primerApellido: "Martínez",
//     segundoApellido: "Lopez",
//     tipoPersona: "Física",
//   },
//   {
//     cedulaAsegurado: "003-051077-0003",
//     fechaNacimiento: "1977-10-05",
//     nombre: "Luis",
//     primerApellido: "Fernández",
//     segundoApellido: "Castillo",
//     tipoPersona: "Jurídica",
//   },
//   {
//     cedulaAsegurado: "004-010191-0004",
//     fechaNacimiento: "1991-01-01",
//     nombre: "María",
//     primerApellido: "Vargas",
//     segundoApellido: "Mendoza",
//     tipoPersona: "Física",
//   },
//   {
//     cedulaAsegurado: "005-310598-0005",
//     fechaNacimiento: "1998-05-31",
//     nombre: "José",
//     primerApellido: "Rojas",
//     segundoApellido: "Pérez",
//     tipoPersona: "Jurídica",
//   },
// ];

export const TablaPersonaComponent = ({ children, refrescarTabla }) => {
  const [filtro, setFiltro] = useState({
    cedulaAsegurado: "",
    nombre: "",
    primerApellido: "",
    segundoApellido: "",
    idTipoPersona: "",
  });
  const [cargando, setCargando] = useState(false);
  const [bloquearBoton, setBloquearBoton] = useState(false);
  const [data, setData] = useState([]);
  const [paginaActual, setPaginaActual] = useState(1);
  const [cantidadPaginas, setCantidadPaginas] = useState(10);
  const [cantidadRegistros, setCantidadRegistros] = useState(0);

  const personaService = new PersonaService();

  const obtener = async () => {
    setCargando(true);
    setBloquearBoton(true);
    try {
      const respuesta = await personaService.obtener({
        ...filtro,
        cantidadRegistros,
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
    setCantidadPaginas(Number(selectedValue));
  };

  const acciones = [
    {
      label: "Editar",
      color: "primary",
      onClick: () => {
        console.log("click Editar");
      },
    },
    {
      label: "Eliminar",
      color: "danger",
      onClick: () => {
        console.log("click Eliminar");
      },
    },
  ];

  useEffect(() => {
    obtener();
  }, [filtro, cantidadPaginas, cantidadRegistros, refrescarTabla]);

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
        titulo="Listado de Personas"
        columnas={columnas}
        datos={data}
        paginaActual={paginaActual}
        cantidadPaginas={cantidadPaginas}
        onPaginaCambio={setPaginaActual}
        cargando={cargando}
        acciones={acciones}
      >
        <div className="d-flex justify-content-between mb-3">
          <div>
            <label htmlFor="cantidadRegistros">Registros por página: </label>
            <select
              id="cantidadRegistros"
              value={cantidadPaginas}
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
