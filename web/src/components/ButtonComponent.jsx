export const ButtonComponent = ({
  onClickGuardar = () => {},
  onClickCancelar = () => {},
  labelBotonGuardar = "Guardar",
  labelBotonCancelar = "Cancelar",
  deshabilitarBotonGuardar = false,
  deshabilitarBotonCancelar = false,
}) => {
  return (
    <>
      <div className="d-flex justify-content-center mt-5">
        <button
          className="btn btn-primary mx-2"
          onClick={onClickGuardar}
          disabled={deshabilitarBotonGuardar}
        >
          {labelBotonGuardar}
        </button>
        <button
          className="btn btn-secondary mx-2"
          onClick={onClickCancelar}
          disabled={deshabilitarBotonCancelar}
        >
          {labelBotonCancelar}
        </button>
      </div>
    </>
  );
};
