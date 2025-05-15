import { API_POLIZA } from "../constants/constantes";
import { AlertaService } from "./AlertaService";
import ResponseService from "./ResponseService ";

export default class PolizaService {
  async crear(modelo) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const response = await fetch(`${API_POLIZA}`, {
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

  async listaSelectorTipoPoliza(filtroBusqueda) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const response = await fetch(
        `${API_POLIZA}/select-tipo-poliza?filtro=${encodeURIComponent(
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
        "Error al obtener los tipos de pólizas del selector.",
        error?.message
      );
    }
  }

  async listaSelectorPolizaEstado(filtroBusqueda) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const response = await fetch(
        `${API_POLIZA}/select-estado?filtro=${encodeURIComponent(
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
        "Error al obtener los estados de las pólizas del selector.",
        error?.message
      );
    }
  }

  async listaSelectorPolizaCobertura(filtroBusqueda) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const response = await fetch(
        `${API_POLIZA}/select-cobertura?filtro=${encodeURIComponent(
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
        "Error al obtener las coberturas de las pólizas del selector.",
        error?.message
      );
    }
  }

  async listaSelectorPolizaPeriodo(filtroBusqueda) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const response = await fetch(
        `${API_POLIZA}/select-periodo?filtro=${encodeURIComponent(
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
        "Error al obtener los periodos de las pólizas del selector.",
        error?.message
      );
    }
  }
}
