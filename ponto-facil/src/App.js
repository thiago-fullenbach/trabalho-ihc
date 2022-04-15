import React from "react";
import "./App.css";
import Login from "./components/View/Login";
import Main from "./components/View/Main";
import { Routes, Route } from "react-router-dom";

function App() {
  const [sessao, setSessao] = React.useState({});

  return (
    <Routes>
      <Route exact path="/main/*" element={<Main seessaoTransf={sessao} setSessaoTransf={setSessao} />}></Route>
      <Route path="/" element={<Login seessaoTransf={sessao} setSessaoTransf={setSessao} />}></Route>
    </Routes>
  )
}

export default App;
