export const ModalComponent = ({ title, onCancel, children }) => {
  return (
    <>
      <div className="modal fade show d-block" tabIndex="-1" aria-hidden="true">
        <div className="modal-dialog modal-lg">
          <div className="modal-content">
            <div className="modal-header bg-warning text-black">
              <h5 className="modal-title text-center">{title}</h5>
              <button
                type="button"
                className="btn-close"
                aria-label="Close"
                onClick={onCancel}
              ></button>
            </div>
            <div className="modal-body">{children}</div>
          </div>
        </div>
      </div>
    </>
  );
};
