export const ModalComponent = ({
  title,
  onCancel,
  children,
  tamnnioModal = "modal-lg",
}) => {
  return (
    <>
      <div className="modal fade show d-block" tabIndex="-1">
        <div className={`modal-dialog ${tamnnioModal}`}>
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
            <div
              className="modal-body"
              style={{ maxHeight: "70vh", overflowY: "auto" }}
            >
              {children}
            </div>
          </div>
        </div>
      </div>
    </>
  );
};
