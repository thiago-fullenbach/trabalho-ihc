import React from "react";

const UiCampoTexto = (props) => {
  const { idCampo, labelCampo, onChangeCampo, valueCampo } = props;

  const handleChangeCampo = (evento) => {
    onChangeCampo(evento.target.value);
  };

  return (
    <div>
      <label htmlFor={idCampo}>{labelCampo}</label>
      <input onChange={handleChangeCampo} value={valueCampo} />
    </div>
  );
};

export default UiCampoTexto;
