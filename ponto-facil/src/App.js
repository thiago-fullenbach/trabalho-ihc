import React from "react";
import "./App.css";
import Login from "./components/View/Login";
import Main from "./components/View/Main";
import { Routes, Route } from "react-router-dom";

function App() {
  return (
    <Routes>
      <Route exact path="/main/*" element={<Main />}></Route>
      <Route path="/" element={<Login />}></Route>
    </Routes>
  )
}

export default App;
