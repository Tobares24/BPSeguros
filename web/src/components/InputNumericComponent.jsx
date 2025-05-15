export const InputNumericComponent = ({
  setForm = () => {},
  valor = "",
  label = "",
  placeholder = "",
  name = "",
  decimales = 0,
  requerido = false,
}) => {
  const handleChange = (e) => {
    let valor = e.target.value;

    const regex = new RegExp(`^\\d*(\\.\\d{0,${decimales}})?$`);

    if (valor === "" || regex.test(valor)) {
      setForm((prevState) => ({
        ...prevState,
        [name]: valor,
      }));
    }
  };

  return (
    <div className="form-group mb-3">
      <label className="form-label">
        {label}
        {requerido && <span className="text-danger mx-1">*</span>}
      </label>
      <input
        type="text"
        className="form-control"
        placeholder={placeholder}
        name={name}
        value={valor ?? ""}
        onChange={handleChange}
      />
    </div>
  );
};
