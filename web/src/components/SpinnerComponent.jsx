export const SpinnerComponent = ({ show }) => {
  if (!show) {
    return null;
  }

  return (
    <div
      className="position-fixed top-0 start-0 w-100 h-100 d-flex justify-content-center align-items-center bg-opacity-50 bg-dark"
      style={{ zIndex: 1050 }}
    >
      <div className="spinner-border text-light" role="status">
        <span className="visually-hidden">Cargando...</span>
      </div>
    </div>
  );
};
