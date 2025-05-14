export const SelectorComponent = ({ label, data = [], cargando = true }) => {
  return (
    <div className="form-group">
      {label && <label className="form-label">{label}</label>}
      <div className="input-group">
        <select
          className="form-select bg-light text-primary border-primary"
          aria-label="Default select example"
          data-name="dropdown"
          disabled={cargando}
        >
          {data.map((item, index) => (
            <option key={index} value={item.value}>
              {item.label}
            </option>
          ))}
        </select>
        <div
          className={`input-group-text spinner-addon ${
            cargando ? "" : "d-none"
          }`}
        >
          <div className="spinner-border spinner-border-sm" role="status">
            <span className="visually-hidden">Cargando...</span>
          </div>
        </div>
      </div>
    </div>
  );
};
