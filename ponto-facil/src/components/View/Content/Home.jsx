import React from "react";
import Button from '../../Generic/Button/Button';
import List from "../../Generic/List/List";

import { useNavigate } from 'react-router-dom';

import './Home.css';

const Home = props => {
    const navigate = useNavigate();
    const heading = ["Local", "Entrada", "Pausa", "Fim Pausa", "Saída"];
    const usersList = [
        {local: "Avenida Parada Pinto, 3524", entrada: "8:00", pausa: "12:00", fimPausa: "13:00", saida: "18:00"},
        {local: "Avenida Parada Pinto, 3524", entrada: "8:00", pausa: "12:00", fimPausa: "13:00", saida: "18:00"},
        {local: "Avenida Parada Pinto, 3524", entrada: "8:00", pausa: "12:00", fimPausa: "13:00", saida: "18:00"},
        {local: "Avenida Parada Pinto, 3524", entrada: "8:00", pausa: "12:00", fimPausa: "13:00", saida: "18:00"},
    ]

    return <div className="Home">
        <h1>Nome do Funcionário</h1>

        <List headingList={heading} itens={usersList} actionAdjust />

        <div className="btnGroup">
            <Button onClick={() => navigate("/main/registrar-ponto")}>Marcar Ponto</Button>
        </div>
        
    </div>
}

export default Home