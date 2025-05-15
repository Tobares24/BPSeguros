export default class ResponseService {
  static handle(response) {
    return new Promise((accept, reject) => {
      response
        .text()
        .then((resText) => {
          if (response.status === 200) {
            try {
              const resJson = JSON.parse(resText);
              accept(resJson);
            } catch (error) {
              const errorModel = {
                statusCode: 500,
                message: "Error al procesar la respuesta",
                traceId: response.headers.get("Traceid") || "N/A",
              };
              reject(errorModel);
            }
          } else {
            try {
              const resJson = JSON.parse(resText);
              const resErrorModel = resJson;
              reject(resErrorModel);
            } catch (error) {
              const errorModel = {
                statusCode: 500,
                message: "Internal Error",
                traceId: response.headers.get("Traceid") || "N/A",
              };
              reject(errorModel);
            }
          }
        })
        .catch((error) => {
          const errorModel = {
            statusCode: 500,
            message: "Error en la solicitud",
            traceId: response.headers.get("Traceid") || "N/A",
          };
          reject(errorModel);
        });
    });
  }

  static handleLocationHeader(response) {
    return new Promise((resolve, reject) => {
      response.text().then((resText) => {
        if (response.ok) {
          const locationHeader = response.headers.get("location");
          console.log(response.headers);

          if (locationHeader) {
            const responseTemp = locationHeader.split("/");
            const id = responseTemp[responseTemp.length - 1];
            resolve(id);
          } else {
            const errorModel = {
              code: 500,
              message: "No se encontró el encabezado Location en la respuesta.",
              traceId: response.headers.get("Traceid") || "N/A",
              details: "El servidor no envió la ubicación del recurso creado.",
            };
            reject(errorModel);
          }
        } else {
          try {
            const resJson = JSON.parse(resText);
            reject(resJson);
          } catch (error) {
            const errorModel = {
              code: 500,
              message: "Internal Error",
              traceId: response.headers.get("Traceid") || "N/A",
              details: "N/A",
            };
            reject(errorModel);
          }
        }
      });
    });
  }

  static handleNoContent(response) {
    return new Promise((accept, reject) => {
      response.text().then((resText) => {
        if (response.ok) {
          accept();
        } else {
          try {
            const resJson = JSON.parse(resText);
            reject(resJson);
          } catch (error) {
            const errorModel = {
              code: 500,
              message: "Internal Error",
              traceId: response.headers.get("traceid") || "N/A",
              details: "N/A",
            };
            reject(errorModel);
          }
        }
      });
    });
  }
}
