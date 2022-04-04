import React from "react";
import Button from '../../Generic/Button/Button';
import List from "../../Generic/List/List";

import { useNavigate } from 'react-router-dom';

import './Funcionarios.css';

const Funcionarios = () => {
    const navigate = useNavigate();
    const heading = ["Funcionário", "CPF", "E-mail", "Telefone"];
    const usersList = [
        {nome: "Thiago", cpf: "46737814816", email: "thiago.fullenbach@hotmail.com", tel: "11944634843"},
        {nome: "Thiago", cpf: "46737814816", email: "thiago.fullenbach@hotmail.com", tel: "11944634843"},
        {nome: "Thiago", cpf: "46737814816", email: "thiago.fullenbach@hotmail.com", tel: "11944634843"},
        {nome: "Thiago", cpf: "46737814816", email: "thiago.fullenbach@hotmail.com", tel: "11944634843"},
        {nome: "Thiago", cpf: "46737814816", email: "thiago.fullenbach@hotmail.com", tel: "11944634843"}
    ]

    return <div className="Home">
        <h1>Funcionários</h1>

        <List headingList={heading} itens={usersList} actionEdit actionDelete />
        
    </div>
}

export default Funcionarios;