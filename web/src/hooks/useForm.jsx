import { useState } from "react";

export const useForm = (
  initialForm = {},
  initialValidateModel = {},
  initialErrorModel = {}
) => {
  const [formState, setFormState] = useState(initialForm);
  const [validateModel, setValidateModel] = useState(initialValidateModel);
  const [errorModel, setErrorModel] = useState(initialErrorModel);

  const onInputChange = ({ target }) => {
    const { name, value } = target;

    setErrorModel((prevData) => ({
      ...prevData,
      [name]: value.length === 0,
    }));

    setValidateModel((prevData) => ({
      ...prevData,
      [name]: value.length > 0 ? "" : "Campo Requerido",
    }));

    setFormState({
      ...formState,
      [name]: value,
    });
  };

  const onResetForm = () => {
    setFormState(initialForm);
    setErrorModel(initialErrorModel);
    setValidateModel(initialValidateModel);
  };

  const onValidateForm = () => {
    let tieneCamposValidos = false;

    Object.keys(validateModel).map((clave) => {
      if (!formState[clave] || formState[clave]?.length === 0 || formState[clave] === "null") {
        validateModel[clave] = true;

        errorModel[clave] = "Campo Requerido";

        setErrorModel({ ...errorModel });

        setValidateModel({ ...validateModel });

        tieneCamposValidos = true;
      } else {
        validateModel[clave] = false;

        setValidateModel({ ...validateModel });
      }
    });

    return tieneCamposValidos;
  };

  return {
    ...formState,
    errorModel,
    formState,
    onInputChange,
    onResetForm,
    onValidateForm,
    setErrorModel,
    setFormState,
    setValidateModel,
    validateModel,
  };
};
