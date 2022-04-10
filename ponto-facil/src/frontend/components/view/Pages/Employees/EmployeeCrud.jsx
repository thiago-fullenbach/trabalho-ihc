import { faUsers } from "@fortawesome/free-solid-svg-icons";
import React from "react";
import Main from "../../../templates/Main/Main";

const headerProps = {
    icon: faUsers,
    title: "Funcionários",
    subtitle: 'Cadastro de Funcionários: Incluir, Listar, Alterar e Excluir'
}

export default class EmployeeCrud extends React.Component {
    render() {
        return (
            <Main {...headerProps}>
                Cadastro de Usuário
            </Main>
        )
    }
}