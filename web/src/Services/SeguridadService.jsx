import { API_SEGURIDAD } from "../constants/constantes";
import { AlertaService } from "./AlertaService";
import ResponseService from "./ResponseService ";

export default class SeguridadService {
  async crear(modelo) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const response = await fetch(`${API_SEGURIDAD}`, {
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
}
