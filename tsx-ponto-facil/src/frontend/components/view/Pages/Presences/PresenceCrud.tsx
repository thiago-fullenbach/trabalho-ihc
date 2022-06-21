import { faTrash, faPencil, faUsers, faClipboard, faUserCheck, faCircleInfo } from "@fortawesome/free-solid-svg-icons";
import axios from 'axios';
import React, { MutableRefObject, useEffect, useRef, useState } from "react";
import Main from "../../../templates/Main/Main";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import Table from "../../../templates/Table/Table";
import { isFieldValid, isNullOrEmpty } from "../../../../../utils/valid";
import { Navigate } from "react-router-dom";
import { getSessionStorageOrDefault, setSessionStorage } from "../../../../../utils/useSessionStorage";

import ApiUtil from '../../../../../chamada-api/ApiUtil';
import LoadingModal from "../../../templates/LoadingModal/LoadingModal";
import { DateConstructor, formataMinutosTrabalhados, formatCpf, formatDate_dd_mm_yyyy, formatDate_hh_mm, listItemsInPortuguese, mountMessage, mountMessageServerErrorIfDefault, toDisplayedDate_dd_mm_yyyy_Local, toDisplayedHour, toDisplayedHourLocal, toDisplayedValue, toDisplayedValueLocal } from "../../../../../utils/formatting";
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
import "./PresenceCrud.css";
import Worktime from "../../../../modelo/presence/worktime";
import Periodo from "../../../../modelo/presence/periodo";
import { addHoras, addHorasCapped, horaComparavel, horaFoiInformada, obterPeriodosBySearchedPresenceArray, subtractHoras } from "../../../../../utils/timeAndHour";

type EmployeeCrudProps = {
    sessionState: SessionState
    loadingState: LoadingState
}

type PresenceCrudState = {
    presenceFilter: PresenceFilter
    novaPresencaEhEntrada: boolean | null,
    newPresence: NewPresence
    list: SearchedPresence[]
    listWorktimes: Worktime[]
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
        data_fim: null,
        hora_inicio: ``,
        hora_fim: ``,
        local_resumo: ``,
        tem_visto: null,
        calcula_hora: false
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
    listWorktimes: [],
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
    const [exibirFiltros, setExibirFiltros] = useState(true)
    const [exibirInfoCalc, setExibirInfoCalc] = useState(false)
    const [listAppliedFilter, setListAppliedFilter] = useState(initialState.list)
    const [listWorktimes, setListWorktimes] = useState(initialState.listWorktimes)

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
                updateListAppliedFilter(retrievedPresences, presenceFilter)
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
    useEffect(() => {
        if (presenceFilter.calcula_hora) {
            calcularBancoHoras()
        }
    }, [listAppliedFilter])
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
    const updateListAppliedFilter = (presencas: SearchedPresence[], filtros: PresenceFilter): void => {
        setListAppliedFilter(presencas.filter(x => {
            return (!isFieldValid(filtros.usuario_nome) || x.usuario_nome.indexOf(filtros.usuario_nome.trim().toUpperCase()) != -1)
            && (filtros.eh_entrada === null || x.eh_entrada === filtros.eh_entrada)
            && (filtros.data_inicio === null || new Date(toDisplayedValue(x.datahora_presenca)).getTime() >= filtros.data_inicio.getTime())
            && (filtros.data_fim === null || new Date(toDisplayedValue(x.datahora_presenca)).getTime() <= filtros.data_fim.getTime())
            && ((horaFoiInformada(filtros.hora_inicio) && horaFoiInformada(filtros.hora_fim) && horaComparavel(filtros.hora_inicio) > horaComparavel(filtros.hora_fim))
            ? horaComparavel(toDisplayedHourLocal(x.datahora_presenca)) >= horaComparavel(filtros.hora_inicio) || horaComparavel(toDisplayedHourLocal(x.datahora_presenca)) <= horaComparavel(filtros.hora_fim)
            : ((!horaFoiInformada(filtros.hora_inicio) || horaComparavel(toDisplayedHourLocal(x.datahora_presenca)) >= horaComparavel(filtros.hora_inicio))
            && (!horaFoiInformada(filtros.hora_fim) || horaComparavel(toDisplayedHourLocal(x.datahora_presenca)) <= horaComparavel(filtros.hora_fim))))
            && (!isFieldValid(filtros.local_resumo) || x.local_resumo.toUpperCase().indexOf(filtros.local_resumo.trim().toUpperCase()) != -1)
            && (filtros.tem_visto === null || x.tem_visto === filtros.tem_visto)
        }))

    }
    const clearFilter = (): void => {
        setPresenceFilter(initialState.presenceFilter)
        setListAppliedFilter(list)
    }
    const updateFilterInputField = (event: React.ChangeEvent<HTMLInputElement>): void => {
        const changedFilter = { ...presenceFilter }
        let horaDestructured: string[]
        switch (event.target.name) {
            case `usuario`:
                changedFilter.usuario_nome = event.target.value
                break
            case `data_inicio`:
                changedFilter.data_inicio = event.target.valueAsDate
                break
            case `data_fim`:
                changedFilter.data_fim = event.target.valueAsDate
                break
            case `hora_hh_inicio`:
                if (changedFilter.hora_inicio === ``) {
                    changedFilter.hora_inicio = `:`
                }
                horaDestructured = changedFilter.hora_inicio.split(':')
                changedFilter.hora_inicio = `${event.target.value}:${horaDestructured[1]}`
                if (changedFilter.hora_inicio === `:`) {
                    changedFilter.hora_inicio = ``
                }
                if (event.target.value.length >= 2) {
                    changedFilter.hora_inicio = `${horaValidaHh(changedFilter.hora_inicio)}:${horaValidaMm(changedFilter.hora_inicio)}`
                    document.getElementById("hora_mm_inicio")?.focus()
                }
                break
            case `hora_mm_inicio`:
                if (changedFilter.hora_inicio === ``) {
                    changedFilter.hora_inicio = `:`
                }
                horaDestructured = changedFilter.hora_inicio.split(':')
                changedFilter.hora_inicio = `${horaDestructured[0]}:${event.target.value}`
                if (changedFilter.hora_inicio === `:`) {
                    changedFilter.hora_inicio = ``
                }
                break
            case `hora_hh_fim`:
                if (changedFilter.hora_fim === ``) {
                    changedFilter.hora_fim = `:`
                }
                horaDestructured = changedFilter.hora_fim.split(':')
                changedFilter.hora_fim = `${event.target.value}:${horaDestructured[1]}`
                if (changedFilter.hora_fim === `:`) {
                    changedFilter.hora_fim = ``
                }
                if (event.target.value.length >= 2) {
                    changedFilter.hora_fim = `${horaValidaHh(changedFilter.hora_fim)}:${horaValidaMm(changedFilter.hora_fim)}`
                    document.getElementById("hora_mm_fim")?.focus()
                }
                break
            case `hora_mm_fim`:
                if (changedFilter.hora_fim === ``) {
                    changedFilter.hora_fim = `:`
                }
                horaDestructured = changedFilter.hora_fim.split(':')
                changedFilter.hora_fim = `${horaDestructured[0]}:${event.target.value}`
                if (changedFilter.hora_fim === `:`) {
                    changedFilter.hora_fim = ``
                }
                break
            case `local`:
                changedFilter.local_resumo = event.target.value
                break
            default:
                break
        }
        setPresenceFilter(changedFilter)
        updateListAppliedFilter(list, changedFilter)
    }
    const horaValidaHh = (hora: string): string => {
        if (hora === ``) {
            return ``
        }
        let horaDestructured = hora.split(':')
        horaDestructured[0] = horaDestructured[0].trim()
        if (/[^0-9]+/.test(horaDestructured[0]) || horaDestructured[0] === ``) {
            return ``
        }
        let hh = parseInt(horaDestructured[0])
        if (hh >= 24) {
            return `23`
        }
        return horaDestructured[0].padStart(2, `0`)
    }
    const horaValidaMm = (hora: string): string => {
        if (hora === ``) {
            return ``
        }
        let horaDestructured = hora.split(':')
        horaDestructured[1] = horaDestructured[1].trim()
        if (/[^0-9]+/.test(horaDestructured[1]) || horaDestructured[1] === ``) {
            return ``
        }
        let hh = parseInt(horaDestructured[1])
        if (hh >= 60) {
            return `59`
        }
        return horaDestructured[1].padStart(2, `0`)
    }
    const validarHoraInicio = () => {
        if (presenceFilter.hora_inicio === ``) {
            return;
        }
        const changedFilter = { ...presenceFilter }
        changedFilter.hora_inicio = `${horaValidaHh(changedFilter.hora_inicio)}:${horaValidaMm(changedFilter.hora_inicio)}`
        setPresenceFilter(changedFilter)
        updateListAppliedFilter(list, changedFilter)
    }
    const validarHoraFim = () => {
        if (presenceFilter.hora_fim === ``) {
            return;
        }
        const changedFilter = { ...presenceFilter }
        changedFilter.hora_fim = `${horaValidaHh(changedFilter.hora_fim)}:${horaValidaMm(changedFilter.hora_fim)}`
        setPresenceFilter(changedFilter)
        updateListAppliedFilter(list, changedFilter)
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
        updateListAppliedFilter(list, changedFilter)
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
    const calcularBancoHoras = () => {
        const funcionarioSet = new Set<number>(listAppliedFilter.map(x => x.usuario_id))
        const funcionariosArray: number[] = []
        funcionarioSet.forEach(x => funcionariosArray.push(x));
        setListWorktimes(funcionariosArray.map(x => {
            const usuarioPresencas = listAppliedFilter
                .filter(y => y.usuario_id === x)
                .sort((a, b) => a.datahora_presenca.getTime() - b.datahora_presenca.getTime())
            const usuarioPeriodos = obterPeriodosBySearchedPresenceArray(usuarioPresencas)
            let tempoTrabalhadoMilliseg = usuarioPeriodos.reduce((prev, cur) => prev + cur.horasTrabalhadasByFilter(presenceFilter), 0)
            const usuarioWorktime = new Worktime()
            usuarioWorktime.usuario_nome = usuarioPresencas[0].usuario_nome
            usuarioWorktime.usuario_horas_diarias = usuarioPresencas[0].usuario_horas_diarias
            usuarioWorktime.tempo_trabalhado_minutos = Math.floor(tempoTrabalhadoMilliseg / 60 / 1000)
            let periodoFiltros = new Periodo()
            periodoFiltros.inicio = presenceFilter.data_inicio ?? new Date()
            periodoFiltros.fim = presenceFilter.data_fim ?? new Date()
            usuarioWorktime.qtd_dias = periodoFiltros.getDiasAssociadosLocal()
                .filter(y => y.getUTCDay() != 0 && y.getUTCDay() != 6).length
            console.log(usuarioWorktime.usuario_horas_diarias)
            const totalPlanejadoMinutos = usuarioWorktime.qtd_dias * usuarioWorktime.usuario_horas_diarias * 60
            const trabalhadoExtraMinutos = usuarioWorktime.tempo_trabalhado_minutos > totalPlanejadoMinutos ? usuarioWorktime.tempo_trabalhado_minutos - totalPlanejadoMinutos : 0
            const trabalhadoFaltaMinutos = totalPlanejadoMinutos > usuarioWorktime.tempo_trabalhado_minutos ? totalPlanejadoMinutos - usuarioWorktime.tempo_trabalhado_minutos : 0
            console.log(totalPlanejadoMinutos, trabalhadoExtraMinutos, trabalhadoFaltaMinutos)
            return usuarioWorktime;
        }))
    }
    const updateCalcHora = (e: React.ChangeEvent<HTMLInputElement>): void => {
        if (!e.target.checked) {
            const changedFilter = { ...presenceFilter }
            changedFilter.calcula_hora = false
            setPresenceFilter(changedFilter)
            return
        }
        if (presenceFilter.data_inicio !== null && presenceFilter.data_fim !== null) {
            calcularBancoHoras()
            const changedFilter = { ...presenceFilter }
            changedFilter.calcula_hora = true
            setPresenceFilter(changedFilter)
            return
        }
        e.preventDefault()
        e.target.checked = false
        let invalidFields = []
        if (presenceFilter.data_inicio == null) {
            invalidFields.push(`Desde a Data`)
        }
        if (presenceFilter.data_fim == null) {
            invalidFields.push(`Até a Data`)
        }
        let camposInvalidos = listItemsInPortuguese(invalidFields)
        setErro(true)
        setTituloMsg(message.genericError)
        setMsg(`É necessário preencher ${camposInvalidos}.`)

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
        return listAppliedFilter.map((presence: SearchedPresence): JSX.Element => {
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
    const renderRowsCalcHora = (): JSX.Element[] => {
        return listWorktimes.map((worktime: Worktime, index: number): JSX.Element => {
            const totalPlanejadoMinutos = worktime.qtd_dias * worktime.usuario_horas_diarias * 60
            const trabalhadoExtraMinutos = worktime.tempo_trabalhado_minutos > totalPlanejadoMinutos ? worktime.tempo_trabalhado_minutos - totalPlanejadoMinutos : 0
            const trabalhadoFaltaMinutos = totalPlanejadoMinutos > worktime.tempo_trabalhado_minutos ? totalPlanejadoMinutos - worktime.tempo_trabalhado_minutos : 0
            return (
                <tr key={index}>
                    <td>{worktime.usuario_nome.split(' ')[0]}</td>
                    <td>{formataMinutosTrabalhados(worktime.tempo_trabalhado_minutos)}</td>
                    <td>{formataMinutosTrabalhados(trabalhadoExtraMinutos)}</td>
                    <td>{formataMinutosTrabalhados(trabalhadoFaltaMinutos)}</td>
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
    const renderTableCalcHora = (): JSX.Element => {
        return (
            <Table headings={
                [
                    "Funcionário",
                    "Horas Trabalhadas",
                    "Extra",
                    "Faltante",
                ]
            }>
                {renderRowsCalcHora()}
            </Table>
        )
    }
    const renderFilter = (): JSX.Element => {
        return (
            <div>
                
                <div className="form">
                    <div className="row">
                        <div className="col-12 d-flex justify-content-start">
                            <button className="btn btn-primary mt-3 mb-3"
                                onClick={() => setExibirFiltros(!exibirFiltros) }>
                                {exibirFiltros ? `Esconder Filtros` : `Mostrar Filtros`}
                            </button>
                        </div>
                    </div>
                </div>
                {exibirFiltros && <div className="form alert alert-info">
                    <div className="row">
                        <div className="col-12">
                            <div className="form-group">
                                <div className="input-group row">
                                    <div className="input col-md-6 col-12">
                                        <label><strong>Filtros</strong></label>
                                    </div>
                                </div>

                                <div className="input-group row">
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Funcionário</label>
                                        <input type="text" 
                                            className="form-control"
                                            name="usuario"
                                            value={presenceFilter.usuario_nome}
                                            onChange={e => updateFilterInputField(e)} />
                                    </div>
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Tipo da Presença</label>
                                        <select
                                            className="form-control" 
                                            name="select_tipo_presenca"
                                            onChange={e => updateFilterSelectField(e)}
                                            value={presenceFilter.eh_entrada == null ? EnSelectTipoPresenca.Selecione.enValue :
                                                (presenceFilter.eh_entrada ? EnSelectTipoPresenca.InicioTrabalho.enValue : EnSelectTipoPresenca.FimTrabalho.enValue)} >
                                            <option value={EnSelectTipoPresenca.Selecione.enValue} >{EnSelectTipoPresenca.Selecione.enDesc}</option>
                                            <option value={EnSelectTipoPresenca.InicioTrabalho.enValue} >{EnSelectTipoPresenca.InicioTrabalho.enDesc}</option>
                                            <option value={EnSelectTipoPresenca.FimTrabalho.enValue} >{EnSelectTipoPresenca.FimTrabalho.enDesc}</option>
                                        </select>
                                    </div>
                                </div>

                                <div className="input-group row">
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Desde a Data</label>
                                        <input type="date"
                                            className="form-control" 
                                            name="data_inicio"
                                            value={toDisplayedValue(presenceFilter.data_inicio)}
                                            onChange={e => updateFilterInputField(e)} />
                                    </div>
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Até a Data</label>
                                        <input type="date"
                                            className="form-control" 
                                            name="data_fim"
                                            value={toDisplayedValue(presenceFilter.data_fim)}
                                            onChange={e => updateFilterInputField(e)} />
                                    </div>
                                </div>
                                <div className="input-group row">
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Desde a Hora</label>
                                        <div className="d-flex">
                                            <input type="text" 
                                                maxLength={2}
                                                name="hora_hh_inicio"
                                                className="horaHH"
                                                value={presenceFilter.hora_inicio.split(':').length == 2 ? presenceFilter.hora_inicio.split(':')[0] : ''}
                                                onChange={e => updateFilterInputField(e)}
                                                onBlur={() => validarHoraInicio()} />
                                            <span>:</span>
                                            <input type="text" id="hora_mm_inicio"
                                                maxLength={2}
                                                name="hora_mm_inicio"
                                                className="horaMM"
                                                value={presenceFilter.hora_inicio.split(':').length == 2 ? presenceFilter.hora_inicio.split(':')[1] : ''}
                                                onChange={e => updateFilterInputField(e)}
                                                onBlur={() => validarHoraInicio()} />
                                        </div>
                                    </div>
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Até a Hora</label>
                                        <div className="d-flex">
                                            <input type="text"
                                                maxLength={2}
                                                name="hora_hh_fim"
                                                className="horaHH"
                                                value={presenceFilter.hora_fim.split(':').length == 2 ? presenceFilter.hora_fim.split(':')[0] : ''}
                                                onChange={e => updateFilterInputField(e)}
                                                onBlur={() => validarHoraFim()} />
                                            <span>:</span>
                                            <input type="text" id="hora_mm_fim"
                                                maxLength={2}
                                                name="hora_mm_fim"
                                                className="horaMM"
                                                value={presenceFilter.hora_fim.split(':').length == 2 ? presenceFilter.hora_fim.split(':')[1] : ''}
                                                onChange={e => updateFilterInputField(e)}
                                                onBlur={() => validarHoraFim()} />
                                        </div>
                                    </div>
                                </div>
                                
                                <div className="input-group row">
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Local</label>
                                        <input type="text"
                                            className="form-control" 
                                            name="local"
                                            value={presenceFilter.local_resumo}
                                            onChange={e => updateFilterInputField(e)} />
                                    </div>
                                    <div className="input col-md-6 col-12 mt-3">
                                        <label>Visto</label>
                                        <select
                                            className="form-control" 
                                            name="tipo_visto"
                                            onChange={e => updateFilterSelectField(e)}
                                            value={presenceFilter.tem_visto == null ? EnSelectTipoTemVisto.Selecione.enValue :
                                                (presenceFilter.tem_visto ? EnSelectTipoTemVisto.ComVisto.enValue : EnSelectTipoTemVisto.SemVisto.enValue)} >
                                            <option value={EnSelectTipoTemVisto.Selecione.enValue} >{EnSelectTipoTemVisto.Selecione.enDesc}</option>
                                            <option value={EnSelectTipoTemVisto.SemVisto.enValue} >{EnSelectTipoTemVisto.SemVisto.enDesc}</option>
                                            <option value={EnSelectTipoTemVisto.ComVisto.enValue} >{EnSelectTipoTemVisto.ComVisto.enDesc}</option>
                                        </select>
                                    </div>
                                </div>

                                <div className="input-group row">
                                    <div className="input col-md-6 col-12 mt-3 d-flex align-items-center">
                                        <input type="checkbox"
                                            onChange={e => updateCalcHora(e)}
                                            checked={presenceFilter.calcula_hora} />
                                        <label className="ps-3" >Calcular Banco de Horas</label>
                                        <button className="btn btn-secondary ms-3"
                                            onMouseEnter={() => setExibirInfoCalc(true)}
                                            onMouseLeave={() => setExibirInfoCalc(false)} ><FontAwesomeIcon icon={faCircleInfo} /></button>
                                    </div>
                                </div>

                                {exibirInfoCalc && <div className="input-group row">
                                    <div className="input col-12 mt-3">
                                        <label>{message.presence.infoWorktime}</label>
                                    </div>
                                </div>}

                                <div className="row">
                                    <div className="col-12 d-flex justify-content-end">
                                        <button className="btn btn-secondary mt-3"
                                            onClick={() => clearFilter()}>
                                            Limpar Filtros
                                        </button>
                                    </div>
                                </div>

                            </div>
                        </div>
                    </div>
                </div>}
            </div>
        )
    }
    return (
        <Main {...headerProps}>
            {renderFilter()}
            {presenceFilter.calcula_hora ? renderTableCalcHora() : renderTable()}
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