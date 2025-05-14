export const FormularioComponent = ({ children }) => {
  return (
    <div className="container">
      <form>
        <div className="row">
          <div className="row g-3">{children}</div>
        </div>
      </form>
    </div>
  );
};
