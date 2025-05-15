import { AlertaService } from "./AlertaService";
import ResponseService from "./ResponseService ";
import { API_PERSONA } from "../constants/constantes";

export default class PersonaService {
  async crear(modelo) {
    try {
      const headers = {
        "Content-Type": "application/json",
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      };

      const response = await fetch(`${API_PERSONA}`, {
        method: "POST",
        credentials: "include",
        body: JSON.stringify(modelo),
        headers,
      });

      await ResponseService.handleNoContent(response);
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
      throw error;
    }
  }

  async listaSelectorPersona(filtroBusqueda) {
    try {
      const headers = {
        "Content-Type": "application/json",
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      };

      const response = await fetch(
        `${API_PERSONA}/select-persona?filtro=${encodeURIComponent(
          filtroBusqueda
        )}`,
        {
          method: "GET",
          credentials: "include",
          headers,
        }
      );

      const responseObject = await ResponseService.handle(response);

      return responseObject;
    } catch (error) {
      AlertaService.error(
        "Error al obtener las personas del selector.",
        error?.message
      );
    }
  }

  async listaSelectorTipoPersona(filtroBusqueda) {
    try {
      const headers = {
        "Content-Type": "application/json",
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      };

      const response = await fetch(
        `${API_PERSONA}/select-tipo-persona?filtro=${encodeURIComponent(
          filtroBusqueda
        )}`,
        {
          method: "GET",
          credentials: "include",
          headers,
        }
      );

      const responseObject = await ResponseService.handle(response);

      return responseObject;
    } catch (error) {
      AlertaService.error(
        "Error al obtener los tipos de personas del selector.",
        error?.message
      );
    }
  }

  async obtener({
    registroPorPagina,
    cedulaAsegurado,
    idTipoPersona,
    nombre,
    paginaActual,
    primerApellido,
    segundoApellido,
  }) {
    try {
      const headers = {
        "Content-Type": "application/json",
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      };

      const params = new URLSearchParams();

      params.append("nombre", nombre);
      params.append("cedulaAsegurado", cedulaAsegurado);
      params.append("primerApellido", primerApellido);
      params.append("segundoApellido", segundoApellido);
      params.append("idTipoPersona", idTipoPersona);
      params.append("paginaActual", paginaActual);
      params.append("registroPorPagina", registroPorPagina);

      const response = await fetch(`${API_PERSONA}?${params.toString()}`, {
        method: "GET",
        credentials: "include",
        headers,
      });

      const responseObject = await ResponseService.handle(response);
      return responseObject;
    } catch (error) {
      AlertaService.error("¡Error!", `${error?.message}`);
      throw error;
    }
  }

  async obtenerPorId(cedulaAsegurado) {
    const headers = {
      "Content-Type": "application/json",
      Authorization: `Bearer ${sessionStorage.getItem("token")}`,
    };

    const response = await fetch(`${API_PERSONA}/${cedulaAsegurado}`, {
      method: "GET",
      headers,
      credentials: "include",
    });

    try {
      const responseObject = await ResponseService.handle(response);
      return responseObject;
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
      throw error;
    }
  }

  async actualizar(modelo, cedulaAsegurado) {
    try {
      const headers = {
        "Content-Type": "application/json",
        Authorization: `Bearer ${sessionStorage.getItem("token")}`,
      };

      const response = await fetch(`${API_PERSONA}/${cedulaAsegurado}`, {
        method: "PUT",
        credentials: "include",
        body: JSON.stringify(modelo),
        headers,
      });

      await ResponseService.handleNoContent(response);
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
      throw error;
    }
  }

  async eliminar(cedulaAsegurado) {
    const headers = {
      "Content-Type": "application/json",
      Authorization: `Bearer ${sessionStorage.getItem("token")}`,
    };

    const response = await fetch(`${API_PERSONA}/${cedulaAsegurado}`, {
      method: "DELETE",
      headers,
      credentials: "include",
    });

    try {
      await ResponseService.handleNoContent(response);
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
      throw error;
    }
  }
}
