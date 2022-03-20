import React from "react";
import UiBotao from "../../ui/UiBotao";
import UiCampoTexto from "../../ui/UiCampoTexto";
import UiCampoSenha from "../../ui/UiCampoSenha";

const VwEntrar360px = (props) => {
  const { viewProps } = props;
  const {
    logicaDadosLogin,
    changeCampoUsuario,
    changeCampoSenha,
    submitDadosLogin,
  } = viewProps;
  return (
    <div className="d-flex-column">
      <h2>Ponto Fácil</h2>
      <h3>Login</h3>
      <div>
        <UiCampoTexto
          idCampo="campoUsuario"
          labelCampo="Usuário"
          onChangeCampo={changeCampoUsuario}
          valueCampo={logicaDadosLogin.campoUsuario}
        />
        <UiCampoSenha
          idCampo="campoSenha"
          labelCampo="Senha"
          onChangeCampo={changeCampoSenha}
          valueCampo={logicaDadosLogin.campoSenha}
        />
        <UiBotao
          temIconeBotao={false}
          classeIconeBotao=""
          labelBotao="Entrar"
          onClickBotao={submitDadosLogin}
        />
      </div>
    </div>
  );
};

export default VwEntrar360px;
