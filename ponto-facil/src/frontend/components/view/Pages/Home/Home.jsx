import React, { useEffect, useState } from "react";
import Main from "../../../templates/Main/Main";
import axios from 'axios';

import { faHome, faClock } from "@fortawesome/free-solid-svg-icons";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import Table from "../../../templates/Table/Table";

const headerProps = {
    icon: faHome,
    title: "Início",
    subtitle: "Registrar presença no trabalho e consultar pontos registrados"
}

const baseUrl = 'http://localhost:3001/presencas';
const initialProps = {
    list: []
}

export default props => {
    const [presencaList, setPresencaList] = useState(initialProps.list)

    useEffect(() => {
        axios(baseUrl).then(resp => {
            console.log(resp.data)
            setPresencaList(resp.data)
        })
    }, [])

    const getRowColor = status => {
        if(status === "Aprovado") return "default"
        if(status === "Reprovado") return "danger"
        return "warning"
    }

    const renderRows = () => {
        return presencaList.map(presenca => {
            return (
                <tr key={presenca.id}
                    className={`table-${getRowColor(presenca.status)}`}>
                    <td>{presenca.id}</td>
                    <td>{presenca.tipoPresenca}</td>
                    <td>{presenca.horaPresenca}</td>
                    <td>{presenca.dataPresenca}</td>
                    <td>{presenca.localPresenca}</td>
                    <td>{presenca.status}</td>
                    <td>
                        <button className="btn btn-primary" tooltip="Ajustar">
                            <FontAwesomeIcon icon={faClock} />
                        </button>
                    </td>
                </tr>
            )
        })
    }

    return (
        <Main {...headerProps} >
                <div className="display-4">user_name</div>
                <hr />
                
                {presencaList.length > 0 ? (
                    <Table headings={[
                        "#",
                        "Tipo",
                        "Horário",
                        "Data",
                        "Local",
                        "Status"
                    ]}>
                        {renderRows()}
                    </Table>
                ) : (<h3 className="d-flex justify-content-center my-5 text-secondary">Não existem registros de presença para este usuário</h3>)}

                <button className="btn btn-primary">
                    Registrar Presença
                </button>
        </Main>
    )
}