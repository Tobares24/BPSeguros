import { useState } from "react";

export const SelectorComponent = ({
  options,
  loading,
  value,
  placeholder,
  onSearch,
  onSelect,
  label,
  deshabilitar,
  isRequired = true,
  error = "",
}) => {
  const [inputValue, setInputValue] = useState("");

  const handleChange = (event) => {
    const value = event.target.value;
    if (event.key === "Enter" && value?.length >= 4) {
      onSearch(value);
    }
    if (event.key === "Enter" && value?.length === 0) {
      onSearch("");
    }
  };

  return (
    <>
      <label className="form-label d-flex align-items-center gap-1">
        {label}
        {isRequired && !error && (
          <span className="text-danger" title="Campo requerido">
            *
          </span>
        )}
      </label>

      <input
        type="text"
        className={`form-control`}
        value={inputValue}
        onChange={(event) => {
          handleChange(event);
          setInputValue(event.target.value);
        }}
        onKeyDown={handleChange}
        placeholder={placeholder}
        autoComplete="off"
        disabled={deshabilitar}
      />
      {loading && (
        <div className="spinner-border text-primary mt-2" role="status">
          <span className="visually-hidden">Cargando...</span>
        </div>
      )}

      <select
        className={`form-select mt-2 ${error ? "is-invalid" : ""}`}
        required={isRequired}
        onChange={onSelect}
        value={value}
        disabled={deshabilitar}
      >
        {options.length === 0 ? (
          <option value="" disabled>
            Sin registros
          </option>
        ) : (
          options.map((option, index) => (
            <option key={index} value={option.value}>
              {option.label}
            </option>
          ))
        )}
      </select>
      {error && <div className="invalid-feedback d-block">{error}</div>}
    </>
  );
};
