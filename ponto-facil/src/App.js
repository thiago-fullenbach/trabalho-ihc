import React, { useLayoutEffect, useState } from "react";
import logo from "./logo.svg";
import "./App.css";
import VwEntrar from "./views/Autorizacao/VwEntrar";

function App() {
  const [largura, setLargura] = useState(0);
  const [larguraEmUso, setLarguraEmUso] = useState("360px");

  useLayoutEffect(() => {
    let larg = window.innerWidth;
    setLargura(larg);
    setLarguraEmUso("360px");
    if (larg >= 576) setLarguraEmUso("576px");
    if (larg >= 992) setLarguraEmUso("992px");
  });

  const [caminhoPaginaAtual, setCaminhoPaginaAtual] = useState("/Login");

  const [logicaUsuarioAutorizacao, setLogicaUsuarioAutorizacao] =
    useState(null);

  const viewProps = {
    larguraEmUso,
    logicaUsuarioAutorizacao,
    setLogicaUsuarioAutorizacao,
    caminhoPaginaAtual,
    setCaminhoPaginaAtual,
  };
  console.log(viewProps);
  if (caminhoPaginaAtual == "/Login") return <VwEntrar viewProps={viewProps} />;
}

export default App;
