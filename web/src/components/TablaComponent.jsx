export const TablaComponent = ({
  columnas,
  datos,
  paginaActual,
  cantidadPaginas,
  onPaginaCambio,
  children,
  cargando,
  titulo = "",
  acciones = [],
}) => {
  const mostrarAcciones = datos?.length > 0 && acciones.length > 0;

  return (
    <div className="mt-4">
      {titulo && <h5 className="text-center mb-3">{titulo}</h5>}
      {children}

      <table className="table table-bordered table-hover">
        <thead className="table-primary text-center">
          <tr>
            {columnas?.map((col, idx) => (
              <th key={idx}>{col?.titulo}</th>
            ))}
            {mostrarAcciones && <th>Acciones</th>}
          </tr>
        </thead>

        <tbody className="text-center">
          {cargando ? (
            <tr>
              <td colSpan={columnas?.length + (mostrarAcciones ? 1 : 0)}>
                <div className="d-flex justify-content-center align-items-center">
                  <div
                    className="spinner-border text-primary"
                    role="status"
                    style={{ width: "3rem", height: "3rem" }}
                  >
                    <span className="visually-hidden">Cargando...</span>
                  </div>
                </div>
              </td>
            </tr>
          ) : datos?.length > 0 ? (
            datos.map((fila, idx) => (
              <tr key={idx}>
                {columnas.map((col, jdx) => (
                  <td key={jdx}>
                    {typeof col?.render === "function"
                      ? col.render(fila[col.nombre], fila)
                      : fila[col.nombre]}
                  </td>
                ))}
                {mostrarAcciones && (
                  <td>
                    {acciones.map((accion, kdx) => (
                      <button
                        key={kdx}
                        className={`btn btn-${accion.color || "secondary"} btn-sm mx-1`}
                        onClick={() => accion.onClick(fila)}
                      >
                        {accion.label}
                      </button>
                    ))}
                  </td>
                )}
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={columnas?.length + (mostrarAcciones ? 1 : 0)}>
                No se encontraron resultados.
              </td>
            </tr>
          )}
        </tbody>
      </table>

      {!cargando && datos?.length > 0 && (
        <nav>
          <ul className="pagination justify-content-end">
            {Array.from({ length: cantidadPaginas }, (_, i) => (
              <li
                key={i}
                className={`page-item ${paginaActual === i + 1 ? "active" : ""}`}
              >
                <button className="page-link" onClick={() => onPaginaCambio(i + 1)}>
                  {i + 1}
                </button>
              </li>
            ))}
          </ul>
        </nav>
      )}
    </div>
  );
};
