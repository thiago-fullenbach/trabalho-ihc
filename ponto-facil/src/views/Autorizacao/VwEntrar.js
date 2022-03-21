import React, { useState } from "react";
import ApiUtil from "../../chamada-api/ApiUtil";
import LogicaDadosLogin from "./LogicaDadosLogin";
import UiCampoTexto from "../../ui/UiCampoTexto";
import UiCampoSenha from "../../ui/UiCampoSenha";
import UiBotao from "../../ui/UiBotao";
import "./VwEntrar.css";

const VwEntrar = (props) => {
  const { viewProps } = props;
  const { setLogicaUsuarioAutorizacao } = viewProps;

  const [logicaDadosLogin, setLogicaDadosLogin] = useState(
    new LogicaDadosLogin()
  );
  if (!logicaDadosLogin.listaUsuariosCadastrados) {
    (async () => {
      let logicaDadosNovo = LogicaDadosLogin.doAntigo(logicaDadosLogin);
      await logicaDadosNovo.carregarDadosApi();
      setLogicaDadosLogin(logicaDadosNovo);
    })();
  }

  const changeCampoUsuario = (value) => {
    let logicaDadosNovo = LogicaDadosLogin.doAntigo(logicaDadosLogin);
    logicaDadosNovo.campoUsuario = value;
    setLogicaDadosLogin(logicaDadosNovo);
  };
  const changeCampoSenha = (value) => {
    let logicaDadosNovo = LogicaDadosLogin.doAntigo(logicaDadosLogin);
    logicaDadosNovo.campoSenha = value;
    setLogicaDadosLogin(logicaDadosNovo);
  };
  const submitDadosLogin = () => {
    (async () => {
      setLogicaUsuarioAutorizacao(
        await ApiUtil.obterLogicaUsuarioAutorizacaoAsync(
          logicaDadosLogin.campoUsuario,
          logicaDadosLogin.campoSenha
        )
      );
    })();
  };

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

export default VwEntrar;
