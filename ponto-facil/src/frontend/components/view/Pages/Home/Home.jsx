import React from "react";
import Main from "../../../templates/Main/Main";

import { faHome } from "@fortawesome/free-solid-svg-icons";

const headerProps = {
    icon: faHome,
    title: "Início",
    subtitle: "Registrar presença no trabalho e consultar pontos registrados"
}

export default props => {
    return (
        <Main {...headerProps} >
                <div className="display-4">Bem Vindo!</div>
                <hr />
                <p className="mb-0">Sistema para exemplificar a construção de um cadastro desenvolvido em React!</p>
        </Main>
    )
}