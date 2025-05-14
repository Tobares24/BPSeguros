export const formatearFechaGuardar = (fecha) => {
    if (!fecha) return null;

    return new Date(fecha.includes('T') ? fecha : `${fecha}T00:00:00`).toISOString();
};