export const formatearFechaGuardar = (fecha) => {
    if (!fecha) return null;

    return new Date(fecha.includes('T') ? fecha : `${fecha}T00:00:00`).toISOString();
};

export const formatearFechaISO = (fechaConHora) => {
    if (!fechaConHora) return "";
    const fecha = new Date(fechaConHora);
    return !isNaN(fecha) ? fecha.toISOString().split("T")[0] : "";
}
