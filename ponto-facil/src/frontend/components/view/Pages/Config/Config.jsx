import { faWrench } from "@fortawesome/free-solid-svg-icons";
import React from "react";
import Main from "../../../templates/Main/Main";

const headerProps = {
    icon: faWrench,
    title: "Configurações",
    subtitle: 'Alterar Configurações do Usuário'
}

export default class Config extends React.Component {
    render() {
        return (
            <Main {...headerProps}>
                Configurações
            </Main>
        )
    }
}