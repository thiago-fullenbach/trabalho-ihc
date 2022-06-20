import { faTrash, faPencil, faUsers, faClipboard, faUserCheck } from "@fortawesome/free-solid-svg-icons";
import axios from 'axios';
import React, { useEffect, useState } from "react";
import Main from "../../../templates/Main/Main";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import Table from "../../../templates/Table/Table";
import { isFieldValid, isNullOrEmpty } from "../../../../../utils/valid";
import { Navigate } from "react-router-dom";
import { getSessionStorageOrDefault, setSessionStorage } from "../../../../../utils/useSessionStorage";

import ApiUtil from '../../../../../chamada-api/ApiUtil';
import LoadingModal from "../../../templates/LoadingModal/LoadingModal";
import { DateConstructor, formatCpf, formatDate_dd_mm_yyyy, formatDate_hh_mm, listItemsInPortuguese, mountMessage, mountMessageServerErrorIfDefault, toDisplayedDate_dd_mm_yyyy_Local, toDisplayedHourLocal, toDisplayedValueLocal } from "../../../../../utils/formatting";
import ConfirmModal from "../../../templates/ConfirmModal/ConfirmModal";
import { resourceDescription } from "../../../../modelo/resourceDescription";
import SessionState from "../../../../main/SessionState";
import LoadingState from "../../../templates/LoadingModal/LoadingState";
import User from "../../../../modelo/employee/user";
import SearchedUser from "../../../../modelo/employee/searchedUser";
import ConfirmModalState from "../../../templates/ConfirmModal/ConfirmModalState";
import { message } from "../../../../modelo/message";
import DetailedUserDTO from "../../../../modelo/employee/DTO/detailedUserDTO";
import NewUserDTOAdapter from "../../../../modelo/employee/DTO/Adapter/newUserDTOAdapter";
import EditUserDTOAdapter from "../../../../modelo/employee/DTO/Adapter/editUserDTOAdapter";
import UserAdapter from "../../../../modelo/employee/Adapter/userAdapter";
import UserAccess from "../../../../modelo/employee/userAccess";
import EnResource from "../../../../modelo/enum/enResource";
import SearchedPresence from "../../../../modelo/presence/searchedPresence";
import NewPresence from "../../../../modelo/presence/newPresence";
import EnPresenceCrudState from "./PresenceCrudState/Enum/EnPresenceCrudState";
import PresenceFilter from "../../../../modelo/presence/presenceFilter";
import NewPresenceDTO from "../../../../modelo/presence/DTO/newPresenceDTO";
import EnSelectTipoPresenca from "./PresenceCrudState/Enum/EnSelectTipoPresenca";
import EnSelectTipoTemVisto from "./PresenceCrudState/Enum/EnSelectTipoTemVisto";
import HiddenPresenceCrud from "./PresenceCrudState/HiddenPresenceCrud";
import NewPresenceCrud from "./PresenceCrudState/NewPresenceCrud";
import Place from "../../../../modelo/place/place";
import MsgDialog from "../../../templates/MsgDialog/MsgDialog";

type EmployeeCrudProps = {
    sessionState: SessionState
    loadingState: LoadingState
}

type PresenceCrudState = {
    presenceFilter: PresenceFilter
    novaPresencaEhEntrada: boolean | null,
    newPresence: NewPresence
    list: SearchedPresence[]
    msg: string
    erro: boolean
    isNewPresence: boolean
    presenceCrudState: EnPresenceCrudState
    confirmModalProps: ConfirmModalState
}

const headerProps = {
    icon: faUserCheck,
    title: "Presenças",
    subtitle: 'Registro de Presenças: Relacionar e Registrar'
}

const initialState: PresenceCrudState = {
    presenceFilter: {
        usuario_nome: ``,
        eh_entrada: null,
        data_inicio: null,
        hora_inicio: null,
        data_fim: null,
        hora_fim: null,
        local_resumo: ``,
        tem_visto: null
    },
    novaPresencaEhEntrada: null,
    newPresence: {
        eh_entrada: null,
        local: {
            cep: ``,
            nome_logradouro: ``,
            numero_logradouro: ``,
            complemento_logradouro: ``,
            nome_bairro: ``,
            nome_cidade: ``,
            nome_estado: ``
        } 
    },
    list: [],
    msg: "",
    erro: false,
    isNewPresence: false,
    presenceCrudState: EnPresenceCrudState.Hidden,
    confirmModalProps: {
        confirmTitle: "",
        confirmQuestion: "",
        isRemove: false,
        idRemovedEntity: 0,
        isActivation: false,
        idActivatedEntity: 0
    }
}

const tituloConfirmaVisto = `Atenção!`

export default (props: EmployeeCrudProps): JSX.Element => {

    const [presenceFilter, setPresenceFilter] = useState(initialState.presenceFilter)
    const [newPresence, setNewPresence] = useState(initialState.newPresence)
    const [novaPresencaEhEntrada, setNovaPresencaEhEntrada] = useState(initialState.novaPresencaEhEntrada)
    const [list, setList] = useState(initialState.list)
    const [msg, setMsg] = useState(initialState.msg)
    const [tituloMsg, setTituloMsg] = useState(``)
    const [erro, setErro] = useState(initialState.erro)
    const [isNewPresence, setIsNewPresence] = useState(initialState.isNewPresence)
    const [presenceCrudState, setPresenceCrudState] = useState(initialState.presenceCrudState)
    const [confirmModalProps, setConfirmModalProps] = useState(initialState.confirmModalProps)
    const [apiReqCompleted, setApiReqCompleted] = useState(true)
    const [openStreetMapReqCompleted, setOpenStreetMapReqCompleted] = useState(true)

    const informaErroGenerico = (mensagemErro: string): void => {
        setErro(true)
        setTituloMsg(message.genericError)
        setMsg(mensagemErro)
    }

    const updateApiReqCompleted = (apiReqCompleted: boolean): void => {
        setApiReqCompleted(apiReqCompleted)
    }

    const updateOpenStreetMapReqCompleted = (openStreetMapReqCompleted: boolean): void => {
        setOpenStreetMapReqCompleted(openStreetMapReqCompleted)
    }

    const refreshSearchedPresences = (): void => {
        setApiReqCompleted(false);
        (async (): Promise<void> => {
            let r = await ApiUtil.getAsync<SearchedPresence[]>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Presenca/listarTodas`)
            setApiReqCompleted(true)
            let retrievedPresences = r.body !== undefined ? (r.body.getReturned() || []) : []
            if (r.status === 200) {
                setErro(false)
                retrievedPresences.forEach(x => {
                    x.datahora_presenca = new Date(x.datahora_presenca)
                });
                setList(retrievedPresences)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body);
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    useEffect((): void => {
        refreshSearchedPresences()
    }, [])
    useEffect(() => {
        props.loadingState.updateLoading(!apiReqCompleted || !openStreetMapReqCompleted)
    }, [apiReqCompleted])
    useEffect(() => {
        props.loadingState.updateLoading(!apiReqCompleted || !openStreetMapReqCompleted)
    }, [openStreetMapReqCompleted])
    
    const publicSaveNew = (): void => {
        setApiReqCompleted(false);
        (async (): Promise<void> => {
            const postNewPresence = new NewPresenceDTO()
            postNewPresence.local = newPresence.local
            let r = await ApiUtil.postAsync<null>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Presenca/insere`, postNewPresence)
            setApiReqCompleted(true)
            if (r.status === 200) {
                setPresenceCrudState(EnPresenceCrudState.Hidden)
                refreshSearchedPresences()
                setErro(false)
                setTituloMsg(message.genericSuccess)
                setMsg(message.presence.successRegister)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body)
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    const publicValidateNewPresence = (): void => {
        let invalidFields = []
        if (novaPresencaEhEntrada == null) {
            setErro(true)
            setMsg(`Não foi informado Tipo da Presença.`)
            return
        }
        if (!isFieldValid(newPresence.local.cep)) {
            invalidFields.push(`CEP`)
        }
        if (!isFieldValid(newPresence.local.nome_logradouro)) {
            invalidFields.push(`Rua`)
        }
        if (!isFieldValid(newPresence.local.nome_bairro)) {
            invalidFields.push(`Bairro`)
        }
        if (!isFieldValid(newPresence.local.nome_cidade)) {
            invalidFields.push(`Cidade`)
        }
        if (!isFieldValid(newPresence.local.nome_estado)) {
            invalidFields.push(`Estado`)
        }
        if (invalidFields.length > 0) {
            let camposInvalidos = listItemsInPortuguese(invalidFields)
            setErro(true)
            setTituloMsg(message.genericError)
            setMsg(`Não foi informado: ${camposInvalidos}.`)
        } else {
            publicSaveNew()
        }
    }
    const clear = (): void => {
        setNewPresence(initialState.newPresence)
        setErro(initialState.erro)
        setMsg(initialState.msg)
        setIsNewPresence(initialState.isNewPresence)
    }
    const updateFilterInputField = (event: React.ChangeEvent<HTMLInputElement>): void => {
        const changedFilter = { ...presenceFilter }
        switch (event.target.name) {
            case `usuario`:
                changedFilter.usuario_nome = event.target.value
                break
            case `data_inicio`:
                changedFilter.data_inicio = event.target.valueAsDate
                break
            case `hora_inicio`:
                changedFilter.hora_inicio = event.target.valueAsDate
                break
            case `data_fim`:
                changedFilter.data_fim = event.target.valueAsDate
                break
            case `hora_fim`:
                changedFilter.hora_fim = event.target.valueAsDate
                break
            case `local`:
                changedFilter.local_resumo = event.target.value
                break
            default:
                break
        }
        setPresenceFilter(changedFilter)
    }
    const updateFilterSelectField = (event: React.ChangeEvent<HTMLSelectElement>): void => {
        const changedFilter = { ...presenceFilter }
        switch (event.target.name) {
            case `select_tipo_presenca`:
                switch (parseInt(event.target.value)) {
                    case EnSelectTipoPresenca.Selecione.enValue:
                        changedFilter.eh_entrada = null
                        break
                    case EnSelectTipoPresenca.InicioTrabalho.enValue:
                        changedFilter.eh_entrada = true
                        break
                    case EnSelectTipoPresenca.FimTrabalho.enValue:
                        changedFilter.eh_entrada = false
                        break
                    default:
                        break
                }
                break
            case `tipo_visto`:
                switch (parseInt(event.target.value)) {
                    case EnSelectTipoTemVisto.Selecione.enValue:
                        changedFilter.tem_visto = null
                        break
                    case EnSelectTipoTemVisto.SemVisto.enValue:
                        changedFilter.tem_visto = false
                        break
                    case EnSelectTipoTemVisto.ComVisto.enValue:
                        changedFilter.tem_visto = true
                        break
                    default:
                        break
                }
                break
            default:
                break
        }
        setPresenceFilter(changedFilter)
    }
    const updateInputField = (event: React.ChangeEvent<HTMLInputElement>): void => {
        const changedPresence = { ...newPresence }
        switch (event.target.name) {
            case `numero_local`:
                changedPresence.local.numero_logradouro = event.target.value
                break
            case `complemento_local`:
                changedPresence.local.complemento_logradouro = event.target.value
                break
            default:
                break
        }
        setNewPresence(changedPresence)
    }
    const updatePresenceTypeFromServer = (eh_entrada: boolean): void => {
        setNovaPresencaEhEntrada(eh_entrada)
        setErro(false);
        setMsg(``);
        setApiReqCompleted(true)
    }
    const updatePlace = (place: Place): void => {
        const changedPresence = { ...newPresence }
        changedPresence.eh_entrada = newPresence.eh_entrada
        changedPresence.local = { ...place }
        setNewPresence(changedPresence)
        setOpenStreetMapReqCompleted(true)
    }
    const publicShowNewPresenceForm = (): void => {
        clear()
        setPresenceCrudState(EnPresenceCrudState.New)
    }
    const publicCancelForm = (): void => {
        setPresenceCrudState(EnPresenceCrudState.Hidden)
    }
    const renderForm = (): JSX.Element => {
        if (presenceCrudState.equals(EnPresenceCrudState.Hidden)) {
            return (props.sessionState.loggedUserHasEnabledResourceByEnum(EnResource.RegistrarPonto) ? <HiddenPresenceCrud publicShowNewPresenceForm={publicShowNewPresenceForm} sessionState={props.sessionState} /> : <div></div>)
        } else {
            return (<NewPresenceCrud
                        novaPresencaEhEntrada={novaPresencaEhEntrada}
                        newPresence={newPresence}
                        publicCancelForm={publicCancelForm}
                        updateInputField={updateInputField}
                        updatePresenceTypeFromServer={updatePresenceTypeFromServer}
                        updatePlace={updatePlace}
                        publicValidateNewPresence={publicValidateNewPresence}
                        sessionState={props.sessionState}
                        loadingState={props.loadingState}
                        informaErroGenerico={informaErroGenerico}
                        updateApiReqCompleted={updateApiReqCompleted}
                        updateOpenStreetMapReqCompleted={updateOpenStreetMapReqCompleted} />)
        }
    }
    const activate = (idPresence: number): void => {
        setApiReqCompleted(false);
        (async (): Promise<void> => {
            let r = await ApiUtil.postAsync<null>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Presenca/darVisto?id=${idPresence}`, {})
            setApiReqCompleted(true);
            if (r.status === 200) {
                setPresenceCrudState(EnPresenceCrudState.Hidden)
                refreshSearchedPresences()
                setErro(false)
                setTituloMsg(message.genericSuccess)
                setMsg(message.presence.successActivated)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body)
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    const checkedSemVisto = (event: React.ChangeEvent<HTMLInputElement>, idPresence: number): void => {
        event.preventDefault();
        openConfirmActivatePresenceModal(idPresence);
    }
    const openConfirmActivatePresenceModal = (idPresence: number): void => {
        let modalProps = { ...confirmModalProps }
        modalProps.confirmTitle = tituloConfirmaVisto
        modalProps.confirmQuestion = message.presence.ensureActivation
        modalProps.isActivation = true
        modalProps.idActivatedEntity = idPresence
        setConfirmModalProps(modalProps)
    }
    const closeConfirmModal = (): void => {
        let modalProps = { ...confirmModalProps }
        modalProps.confirmQuestion = ""
        setConfirmModalProps(modalProps)
    }
    const confirmConfirmModal = (): void => {
        closeConfirmModal()
        if (confirmModalProps.isActivation) {
            activate(confirmModalProps.idActivatedEntity)
        }
    }
    const renderRows = (): JSX.Element[] => {
        return list.map((presence: SearchedPresence): JSX.Element => {
            return (
                <tr key={presence.id}>
                    <td>{presence.usuario_nome.split(' ')[0]}</td>
                    <td>{presence.eh_entrada ? `Início` : `Fim`}</td>
                    <td>{toDisplayedDate_dd_mm_yyyy_Local(presence.datahora_presenca)}</td>
                    <td>{toDisplayedHourLocal(presence.datahora_presenca)}</td>
                    <td>{presence.local_resumo}</td>
                    <td className="text-center">
                        {(props.sessionState.loggedUserHasEnabledResourceByEnum(EnResource.VisualizarPontoDemaisUsuarios) || presence.tem_visto)
                         ? <input type="checkbox"
                            onChange={e => checkedSemVisto(e, presence.id)}
                            checked={presence.tem_visto}
                            disabled={presence.tem_visto} />
                         : <input type="checkbox" className="invisible" />}
                    </td>
                </tr>
            )
        })
    }
    const renderTable = (): JSX.Element => {
        return (
            <Table headings={
                [
                    "Funcionário",
                    "Tipo",
                    "Data",
                    "Hora",
                    "Local",
                    "Visto"
                ]
            }>
                {renderRows()}
            </Table>
        )
    }
    return (
        <Main {...headerProps}>
            
            {renderTable()}
            {renderForm()}
            {(confirmModalProps.confirmQuestion.length > 0)
                && <ConfirmModal
                    handleClose={closeConfirmModal}
                    handleConfirm={confirmConfirmModal}
                    confirmTitle={confirmModalProps.confirmTitle}
                    confirmQuestion={confirmModalProps.confirmQuestion} />}
                    
            {msg && 
                <MsgDialog typeAlert={erro ? 'danger' : 'success'} msgType={tituloMsg} msg={msg} onDismiss={() => { setMsg(``) }} />
            }
        </Main>
    )
    
}