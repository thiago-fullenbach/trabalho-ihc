import React, { useEffect, useState } from "react";
import Main from "../../../templates/Main/Main";
import axios from 'axios';

import { faHome, faClock } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import Table from "../../../templates/Table/Table";
import { getSessionStorageOrDefault } from '../../../../../utils/useSessionStorage';
import SessionState from "../../../../main/SessionState";
import LoadingState from "../../../templates/LoadingModal/LoadingState";
import EnResource from "../../../../modelo/enum/enResource";

const headerProps = {
    icon: faHome,
    title: "Início",
    subtitle: "Bem-vindo"
}

const baseUrl = 'http://localhost:3001/presencas';
const initialProps = {
    list: []
}

type HomeProps = {
    sessionState: SessionState
    loadingState: LoadingState
}

export default (props: HomeProps): JSX.Element => {
    const [presencaList, setPresencaList] = useState(initialProps.list)

    return (
        <Main {...headerProps} >
            <h3>Bem-vindo ao Sistema Ponto Fácil!</h3>
            <p>Para começar, navegue por uma das funcionalidades do menu.</p>
        </Main>
    )
}