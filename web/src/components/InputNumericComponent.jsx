import React, { useCallback } from "react";

export const InputNumericComponent = ({
    setForm = () => {},
    valor = "",
    label = "",
    placeholder = "",
    name = "",
    decimales = 0,
    esObligatorio = false
}) => {
    const handleChange = useCallback((e) => {
        const entrada = e.target.value;
        const patron = `^\\d*(\\.\\d{0,${decimales}})?$`;
        const regex = new RegExp(patron);

        if (entrada === '' || regex.test(entrada)) {
            setForm(prev => ({
                ...prev,
                [name]: entrada
            }));
        }
    }, [setForm, decimales, name]);

    return (
        <div className="form-group mb-3">
            <label className="form-label">
                {label}
                {esObligatorio && <span className="text-danger mx-1">*</span>}
            </label>
            <input
                type="text"
                className="form-control"
                placeholder={placeholder}
                name={name}
                value={valor}
                onChange={handleChange}
            />
        </div>
    );
};