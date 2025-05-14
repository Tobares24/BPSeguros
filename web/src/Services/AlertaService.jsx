import Swal from "sweetalert2";

export const AlertaService = {
  success: (title, text) => {
    return Swal.fire({
      title: title || "¡Éxito!",
      text: text || "La operación se completó correctamente.",
      icon: "success",
      confirmButtonText: "Aceptar",
    });
  },

  error: (title, text) => {
    return Swal.fire({
      title: title || "¡Error!",
      text: text || "Ha ocurrido un error.",
      icon: "error",
      confirmButtonText: "Aceptar",
    });
  },

  warning: (title, text) => {
    return Swal.fire({
      title: title || "¡Advertencia!",
      text: text || "",
      icon: "warning",
      confirmButtonText: "Aceptar",
    });
  },

  info: (title, text) => {
    return Swal.fire({
      title: title || "Información",
      text: text || "",
      icon: "info",
      confirmButtonText: "Aceptar",
    });
  },

  confirmation: (
    title,
    text,
    callback,
    labelButtonCancel,
    labelButtonConfirm
  ) => {
    return Swal.fire({
      title: title || "¿Estás seguro?",
      text: text || "Esta acción no puede deshacerse.",
      icon: "warning",
      showCancelButton: true,
      confirmButtonText: labelButtonCancel || "Aceptar",
      cancelButtonText: labelButtonConfirm || "Cancelar",
      confirmButtonColor: "#d33",
      cancelButtonColor: "#3085d6",
    }).then((result) => {
      callback(result.isConfirmed);
    });
  },
};
