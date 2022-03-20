import React, { useState } from "react";
import ApiUtil from "../../chamada-api/ApiUtil";
import LogicaDadosLogin from "./LogicaDadosLogin";
import VwEntrar360px from "./VwEntrar360px";
import VwEntrar576px from "./VwEntrar576px";
import VwEntrar992px from "./VwEntrar992px";

const VwEntrar = (props) => {
  const { viewProps } = props;
  const {
    larguraEmUso,
    setLogicaUsuarioAutorizacao,
    caminhoPaginaAtual,
    setCaminhoPaginaAtual,
  } = viewProps;

  let usestateLogicaDadosLogin = new LogicaDadosLogin();
  usestateLogicaDadosLogin.campoUsuario = "";
  usestateLogicaDadosLogin.campoSenha = "";
  const [logicaDadosLogin, setLogicaDadosLogin] = useState(
    usestateLogicaDadosLogin
  );
  if (!logicaDadosLogin.listaUsuariosCadastrados) {
    (async () => {
      await logicaDadosLogin.carregarDadosApi();
    })();
  }

  const changeCampoUsuario = (value) => {
    value = value.target.value;
    let logicaDadosNovo = LogicaDadosLogin.doAntigo(logicaDadosLogin);
    logicaDadosNovo.campoUsuario = value;
    setLogicaDadosLogin(logicaDadosNovo);
  };
  const changeCampoSenha = (value) => {
    value = value.target.value;
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
  const viewPropsFilho = {
    logicaDadosLogin,
    changeCampoUsuario,
    changeCampoSenha,
    submitDadosLogin,
  };

  if (larguraEmUso == "360px")
    return <VwEntrar360px viewProps={viewPropsFilho} />;
  if (larguraEmUso == "576px")
    return <VwEntrar576px viewProps={viewPropsFilho} />;
  if (larguraEmUso == "992px")
    return <VwEntrar992px viewProps={viewPropsFilho} />;
};

export default VwEntrar;
