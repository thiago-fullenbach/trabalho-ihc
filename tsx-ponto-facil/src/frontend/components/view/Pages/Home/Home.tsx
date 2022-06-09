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
    subtitle: "Registrar presença no trabalho e consultar pontos registrados"
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

    // useEffect(() => {
    //     axios(baseUrl).then(resp => {
    //         setPresencaList(resp.data)
    //     })
    // }, [])

    // const getRowColor = status => {
    //     if(status === "Aprovado") return "default"
    //     if(status === "Reprovado") return "danger"
    //     return "warning"
    // }

    const getLocation = (): void => {
        props.loadingState.updateLoading(true);
        (async (): Promise<void> => {
            navigator.geolocation.getCurrentPosition(location => {
                const storeCoords = location.coords
                axios.get(`http://nominatim.openstreetmap.org/reverse?format=json&lat=${storeCoords.latitude}&lon=${storeCoords.longitude}&zoom=18&addressdetails=1`)
                .then(response => {
                    props.loadingState.updateLoading(false)
                    console.log(response.data)
                })
            })
        })()
    }

    // const renderRows = () => {
    //     return presencaList.map(presenca => {
    //         return (
    //             <tr key={presenca.id}
    //                 className={`table-${getRowColor(presenca.status)}`}>
    //                 <td>{presenca.id}</td>
    //                 <td>{presenca.tipoPresenca}</td>
    //                 <td>{presenca.horaPresenca}</td>
    //                 <td>{presenca.dataPresenca}</td>
    //                 <td>{presenca.localPresenca}</td>
    //                 <td>{presenca.status}</td>
    //                 <td>
    //                     <button className="btn btn-primary" tooltip="Ajustar">
    //                         <FontAwesomeIcon icon={faClock} />
    //                     </button>
    //                 </td>
    //             </tr>
    //         )
    //     })
    // }

    return (
        <Main {...headerProps} >
                <div className="display-4">{props.sessionState.loggedUser?.Nome}</div>
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
                        {/* {renderRows()} */}
                    </Table>
                ) : (<h3 className="d-flex justify-content-center my-5 text-secondary">Não existem registros de presença para este usuário</h3>)}

                {props.sessionState.loggedUserHasEnabledResourceByEnum(EnResource.RegistrarPonto) && <button className="btn btn-primary" onClick={getLocation}>
                    Registrar Presença
                </button>}
        </Main>
    )
}