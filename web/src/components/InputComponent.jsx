import { ExclamationCircleFill } from "react-bootstrap-icons";

export const InputComponent = ({
  label,
  name,
  value,
  onChange,
  placeholder,
  requerido = false,
  error = "",
  tipo = "text",
  deshabilitar = false,
}) => {
  return (
    <div className="mb-3">
      <label
        htmlFor={name}
        className="form-label d-flex align-items-center gap-1"
      >
        {label}
        {requerido && !error && (
          <span className="text-danger" title="Campo requerido">
            *
          </span>
        )}
        {error && (
          <ExclamationCircleFill className="text-danger" title={error} />
        )}
      </label>

      <input
        type={tipo}
        className={`form-control ${error ? "is-invalid" : ""}`}
        id={name}
        name={name}
        value={value ?? ""}
        placeholder={placeholder}
        onChange={onChange}
        disabled={deshabilitar}
        autoComplete="off"
      />

      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};
