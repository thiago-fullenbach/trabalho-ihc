import React from "react";

const UiCampoSenha = (props) => {
  const { idCampo, labelCampo, onChangeCampo, valueCampo } = props;

  const handleChangeCampo = (evento) => {
    onChangeCampo(evento.target.value);
  };

  return (
    <div>
      <label htmlFor={idCampo}>{labelCampo}</label>
      <input type="password" onChange={handleChangeCampo} value={valueCampo} />
    </div>
  );
};

export default UiCampoSenha;
