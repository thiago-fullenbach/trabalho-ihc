import React from "react";

const VwEntrar576px = (props) => {
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
      <h3>Login 576px</h3>
      <div>
        <label htmlFor="campoUsuario">Usuário</label>
        <input
          id="campoUsuario"
          value={logicaDadosLogin.campoUsuario}
          onChange={changeCampoUsuario}
        />
        <label htmlFor="campoSenha">Senha</label>
        <input
          type="password"
          id="campoSenha"
          value={logicaDadosLogin.campoSenha}
          onChange={changeCampoSenha}
        />
        <button onClick={submitDadosLogin}>Entrar</button>
      </div>
    </div>
  );
};

export default VwEntrar576px;
