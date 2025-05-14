import { ExclamationCircleFill } from "react-bootstrap-icons";

export const DatePickerComponent = ({
  label,
  value,
  name,
  onChange,
  requerido = false,
  error = "",
}) => {
  return (
    <div className="mb-3">
      <label
        htmlFor="date"
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
        type="date"
        className={`form-control ${error ? "is-invalid" : ""}`}
        id="date"
        value={value}
        name={name}
        onChange={onChange}
        required={requerido}
        autoComplete="off"
      />

      {error && <div className="invalid-feedback">{error}</div>}
    </div>
  );
};
