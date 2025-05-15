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

  async login({ email, password }) {
    try {
      const headers = {
        "Content-Type": "application/json",
      };

      const params = new URLSearchParams();

      params.append("email", email);
      params.append("password", password);

      const response = await fetch(`${API_SEGURIDAD}?${params.toString()}`, {
        method: "GET",
        credentials: "include",
        headers,
      });

      const responsObj = await ResponseService.handle(response);
      
      return responsObj;
    } catch (error) {
      AlertaService.error("Error", `${error?.message}`);
      throw error;
    }
  }
}
