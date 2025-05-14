import { AlertaService } from "./AlertaService";
import ResponseService from "./ResponseService ";
import { API_PERSONA } from "../constants/constantes";

export default class PersonaService {
  async listaSelectorPersona(filtroBusqueda) {
    try {
      const headers = {
        "Content-Type": "application/json",
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

  async crear(modelo) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const response = await fetch(`${API_PERSONA}`, {
        method: "POST",
        credentials: "include",
        body: JSON.stringify(modelo),
        headers,
      });

      const responsId = await ResponseService.handleLocationHeader(response);
      return responsId;
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
      throw error;
    }
  }

  async obtener({
    cantidadRegistros,
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
      };

      const params = new URLSearchParams();

      params.append("nombre", nombre);
      params.append("cedulaAsegurado", cedulaAsegurado);
      params.append("primerApellido", primerApellido);
      params.append("segundoApellido", segundoApellido);
      params.append("idTipoPersona", idTipoPersona);
      params.append("paginaActual", paginaActual);
      params.append("cantidadRegistros", cantidadRegistros);

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
}
