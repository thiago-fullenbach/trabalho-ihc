import { faTrash, faPencil, faUsers, faClipboard } from "@fortawesome/free-solid-svg-icons";
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
import { DateConstructor, formatCpf, formatDate_dd_mm_yyyy, listItemsInPortuguese, mountMessage, mountMessageServerErrorIfDefault, toDisplayedValue } from "../../../../../utils/formatting";
import HiddenEmployeeCrud from "./EmployeeCrudState/HiddenEmployeeCrud";
import NewEmployeeCrud from "./EmployeeCrudState/NewEmployeeCrud";
import EditEmployeeCrud from "./EmployeeCrudState/EditEmployeeCrud";
import ConfirmModal from "../../../templates/ConfirmModal/ConfirmModal";
import { resourceDescription } from "../../../../modelo/resourceDescription";
import SessionState from "../../../../main/SessionState";
import LoadingState from "../../../templates/LoadingModal/LoadingState";
import User from "../../../../modelo/employee/user";
import SearchedUser from "../../../../modelo/employee/searchedUser";
import ConfirmModalState from "../../../templates/ConfirmModal/ConfirmModalState";
import EnEmployeeCrudState from "./EmployeeCrudState/Enum/EnEmployeeCrudState";
import { message } from "../../../../modelo/message";
import DetailedUserDTO from "../../../../modelo/employee/DTO/detailedUserDTO";
import NewUserDTOAdapter from "../../../../modelo/employee/DTO/Adapter/newUserDTOAdapter";
import EditUserDTOAdapter from "../../../../modelo/employee/DTO/Adapter/editUserDTOAdapter";
import UserAdapter from "../../../../modelo/employee/Adapter/userAdapter";
import UserAccess from "../../../../modelo/employee/userAccess";
import EnResource from "../../../../modelo/enum/enResource";
import MsgDialog from "../../../templates/MsgDialog/MsgDialog";

type EmployeeCrudProps = {
    sessionState: SessionState
    loadingState: LoadingState
}

type EmployeeCrudState = {
    user: User
    list: SearchedUser[]
    msg: string
    erro: boolean
    newUser: boolean
    tempAccess: string
    employeeCrudState: EnEmployeeCrudState
    confirmModalProps: ConfirmModalState
}

const headerProps = {
    icon: faUsers,
    title: "Funcionários",
    subtitle: 'Cadastro de Funcionários: Incluir, Listar, Alterar e Excluir'
}

const initialState: EmployeeCrudState = {
    user: {
        id: 0,
        nome: "",
        cpf: "",
        data_nascimento: null,
        horas_diarias: 8,
        login: "",
        nova_senha: "",
        confirmar_senha: "",
        acessos: [
            {
                recurso_cod_en: 1,
                eh_habilitado: true
            },
            {
                recurso_cod_en: 2,
                eh_habilitado: true
            },
            {
                recurso_cod_en: 3,
                eh_habilitado: false
            },
            {
                recurso_cod_en: 4,
                eh_habilitado: false
            },
            {
                recurso_cod_en: 5,
                eh_habilitado: false
            },
            {
                recurso_cod_en: 6,
                eh_habilitado: true
            },
            {
                recurso_cod_en: 7,
                eh_habilitado: true
            },
            {
                recurso_cod_en: 8,
                eh_habilitado: false
            },
            {
                recurso_cod_en: 9,
                eh_habilitado: true
            },
            {
                recurso_cod_en: 10,
                eh_habilitado: false
            },
            {
                recurso_cod_en: 11,
                eh_habilitado: false
            }
        ]
    },
    list:[],
    msg: "",
    erro: false,
    newUser: false,
    tempAccess: "",
    employeeCrudState: EnEmployeeCrudState.Hidden,
    confirmModalProps: {
        confirmTitle: "",
        confirmQuestion: "",
        isRemove: false,
        idRemovedEntity: 0,
        isActivation: false,
        idActivatedEntity: 0
    }
}

const tituloConfirmaExclusao = `Atenção!`

export default (props: EmployeeCrudProps): JSX.Element => {

    const emptyCallback = () => {}
    const [user, setUser] = useState(initialState.user)
    const [list, setList] = useState(initialState.list)
    const [msg, setMsg] = useState(initialState.msg)
    const [tituloMsg, setTituloMsg] = useState(``)
    const [erro, setErro] = useState(initialState.erro)
    const [newUser, setNewUser] = useState(initialState.newUser)
    const [tempAccess, setTempAccess] = useState(initialState.tempAccess)
    const [employeeCrudState, setEmployeeCrudState] = useState(initialState.employeeCrudState)
    const [confirmModalProps, setConfirmModalProps] = useState(initialState.confirmModalProps)

    const refreshSearchedUsers = (): void => {
        props.loadingState.updateLoading(true);
        (async (): Promise<void> => {
            let r = await ApiUtil.getAsync<SearchedUser[]>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/listarTodos`)
            
            props.loadingState.updateLoading(false)
            let retrievedUsers = r.body !== undefined ? (r.body.getReturned() || []) : []
            retrievedUsers.forEach(x => {
                x.data_nascimento = new Date(x.data_nascimento)
            });
            if (r.status === 200) {
                setErro(false)
                setList(retrievedUsers)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body);
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    useEffect((): void => {
        refreshSearchedUsers()
    }, [])
    const publicSaveNew = (): void => {
        props.loadingState.updateLoading(true);
        (async (): Promise<void> => {
            const postNewUser = new NewUserDTOAdapter(user).getRawNewUserDTO()
            let r = await ApiUtil.postAsync<DetailedUserDTO>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/insere`, postNewUser)
            props.loadingState.updateLoading(false)
            if (r.status === 200) {
                setEmployeeCrudState(EnEmployeeCrudState.Hidden)
                refreshSearchedUsers()
                setErro(false)
                setTituloMsg(message.genericSuccess)
                setMsg(message.employee.successCreated)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body)
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    const publicValidateNewUser = (): void => {
        let invalidFields = []
        if (!isFieldValid(user.nome)) {
            invalidFields.push(`Nome`)
        }
        if (!isFieldValid(user.cpf)) {
            invalidFields.push(`CPF`)
        }
        if (!isFieldValid(user.data_nascimento)) {
            invalidFields.push(`Data de Nascimento`)
        }
        if (!isFieldValid(user.horas_diarias)) {
            invalidFields.push(`Horas Diárias`)
        }
        if (!isFieldValid(user.login)) {
            invalidFields.push(`Login`)
        }
        if (!isFieldValid(user.nova_senha)) {
            invalidFields.push(`Nova Senha`)
        }
        if (!isFieldValid(user.confirmar_senha)) {
            invalidFields.push(`Confirmar Senha`)
        }
        if (invalidFields.length > 0) {
            let camposInvalidos = listItemsInPortuguese(invalidFields)
            setErro(true)
            setTituloMsg(message.genericError)
            setMsg(`É necessário preencher ${camposInvalidos}.`)
        } else if (user.nova_senha !== user.confirmar_senha) {
            setErro(true)
            setTituloMsg(message.genericError)
            setMsg(`O campo Nova Senha deve ser igual ao Confirmar Senha.`)
        } else {
            publicSaveNew()
        }
    }
    const publicSaveEdit = (): void => {
        props.loadingState.updateLoading(true);
        (async (): Promise<void> => {
            const postEditUser = new EditUserDTOAdapter(user).getRawEditUserDTO()
            let r = await ApiUtil.postAsync<DetailedUserDTO>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/atualiza`, postEditUser)
            props.loadingState.updateLoading(false)
            if (r.status === 200) {
                setEmployeeCrudState(EnEmployeeCrudState.Hidden)
                refreshSearchedUsers()
                setErro(false)
                setTituloMsg(message.genericSuccess)
                setMsg(message.employee.successUpdated)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body);
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    const publicValidateEditUser = (): void => {
        let invalidFields = []
        if (!isFieldValid(user.nome)) {
            invalidFields.push(`Nome`)
        }
        if (!isFieldValid(user.cpf)) {
            invalidFields.push(`CPF`)
        }
        if (!isFieldValid(user.data_nascimento)) {
            invalidFields.push(`Data de Nascimento`)
        }
        if (!isFieldValid(user.horas_diarias)) {
            invalidFields.push(`Horas Diárias`)
        }
        if (!isFieldValid(user.login)) {
            invalidFields.push(`Login`)
        }
        if (invalidFields.length > 0) {
            let camposInvalidos = listItemsInPortuguese(invalidFields)
            setErro(true)
            setTituloMsg(message.genericError)
            setMsg(`É necessário preencher ${camposInvalidos}.`)
        } else {
            publicSaveEdit()
        }
    }
    const clear = (): void => {
        setUser(initialState.user)
        setErro(initialState.erro)
        setMsg(initialState.msg)
        setNewUser(initialState.newUser)
        setTempAccess(initialState.tempAccess)
    }
    const publicUpdateField = (event: React.ChangeEvent<HTMLInputElement>): void => {
        const changedUser = { ...user }
        switch (event.target.name) {
            case `nome`:
                changedUser.nome = event.target.value
                break
            case `cpf`:
                changedUser.cpf = event.target.value
                break
            case `data_nascimento`:
                changedUser.data_nascimento = event.target.valueAsDate
                break
            case `horas_diarias`:
                changedUser.horas_diarias = event.target.valueAsNumber
                break
            case `login`:
                changedUser.login = event.target.value
                break
            case `nova_senha`:
                changedUser.nova_senha = event.target.value
                break
            case `confirmar_senha`:
                changedUser.confirmar_senha = event.target.value
                break
            default:
                break
        }
        setUser(changedUser)
    }
    const publicUpdateAccess = (event: React.ChangeEvent<HTMLInputElement>, recurso_cod_en: number): void => {
        const aUser = { ...user }
        aUser.acessos[aUser.acessos.findIndex((x: UserAccess): boolean => x.recurso_cod_en === recurso_cod_en)].eh_habilitado = !!event.target.checked
        setUser(aUser)
    }
    const publicShowNewUserForm = (): void => {
        clear()
        setEmployeeCrudState(EnEmployeeCrudState.New)
    }
    const publicCancelForm = (): void => {
        setEmployeeCrudState(EnEmployeeCrudState.Hidden)
    }
    const renderForm = (): JSX.Element => {
        if (employeeCrudState.equals(EnEmployeeCrudState.Hidden)) {
            return (props.sessionState.loggedUserHasEnabledResourceByEnum(EnResource.CadastrarDemaisUsuarios) ? <HiddenEmployeeCrud publicShowNewUserForm={publicShowNewUserForm} sessionState={props.sessionState} /> : <div></div>)
        } else if (employeeCrudState.equals(EnEmployeeCrudState.New)) {
            return (<NewEmployeeCrud newUser={user} publicCancelForm={publicCancelForm} publicUpdateAccess={publicUpdateAccess} publicUpdateField={publicUpdateField} publicValidateNewUser={publicValidateNewUser} sessionState={props.sessionState} />)
        } else {
            return (<EditEmployeeCrud editUser={user} publicCancelForm={publicCancelForm} publicUpdateAccess={publicUpdateAccess} publicUpdateField={publicUpdateField} publicValidateEditUser={publicValidateEditUser} sessionState={props.sessionState} />)
        }
    }
    const load = (idUser: number): void => {
        props.loadingState.updateLoading(true);
        (async (): Promise<void> => {
            let r = await ApiUtil.getAsync<DetailedUserDTO>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/pegaPeloId?id=${idUser}`)
            props.loadingState.updateLoading(false)
            if (r.status === 200) {
                const loadedUser = new UserAdapter(r.body?.getReturned() || new DetailedUserDTO()).getRawUser()
                setErro(false)
                setMsg(``)
                setEmployeeCrudState(EnEmployeeCrudState.Edit)
                setUser(loadedUser)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body)
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    const remove = (idUser: number): void => {
        props.loadingState.updateLoading(true);
        (async (): Promise<void> => {
            let r = await ApiUtil.postAsync<null>(props.sessionState.stringifiedSession, props.sessionState.updateStringifiedSession, props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/excluiPeloId?id=${idUser}`, {})
            props.loadingState.updateLoading(false)
            if (r.status === 200) {
                setEmployeeCrudState(EnEmployeeCrudState.Hidden)
                refreshSearchedUsers()
                setErro(false)
                setTituloMsg(message.genericSuccess)
                setMsg(message.employee.successRemoved)
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body)
                setErro(true)
                setTituloMsg(message.genericError)
                setMsg(mensagemCompleta)
            }
        })()
    }
    const openConfirmRemoveUserModal = (idUser: number): void => {
        let modalProps = { ...confirmModalProps }
        modalProps.confirmTitle = tituloConfirmaExclusao
        modalProps.confirmQuestion = message.employee.ensureRemove
        modalProps.isRemove = true
        modalProps.idRemovedEntity = idUser
        setConfirmModalProps(modalProps)
    }
    const closeConfirmModal = (): void => {
        let modalProps = { ...confirmModalProps }
        modalProps.confirmQuestion = ""
        setConfirmModalProps(modalProps)
    }
    const confirmConfirmModal = (): void => {
        closeConfirmModal()
        if (confirmModalProps.isRemove) {
            remove(confirmModalProps.idRemovedEntity)
        }
    }
    const copyToClipboard = (text: string): void => {
        window.navigator.clipboard.writeText(text);
    }
    const renderRows = (): JSX.Element[] => {
        return list.map((user: SearchedUser): JSX.Element => {
            return (
                <tr key={user.id}>
                    <td>{user.nome.split(' ')[0]}</td>
                    <td>{formatCpf(user.cpf)}</td>
                    <td className="d-flex">
                        <button className="btn btn-primary ms-2"
                            onClick={() => copyToClipboard(formatCpf(user.cpf))}>
                            <FontAwesomeIcon icon={faClipboard} />
                        </button></td>
                    <td>{formatDate_dd_mm_yyyy(user.data_nascimento)}</td>
                    <td>{user.horas_diarias}</td>
                    <td>{user.login}</td>
                    <td className="d-flex">
                        {(props.sessionState.loggedUser?.Id === user.id ? props.sessionState.loggedUserHasEnabledResourceByEnum(EnResource.CadastrarUsuario) : (!user.eh_admin_root && props.sessionState.loggedUserHasEnabledResourceByEnum(EnResource.CadastrarDemaisUsuarios)))
                            ? <button className="btn btn-warning"
                            onClick={() => load(user.id)}>
                                <FontAwesomeIcon icon={faPencil} />
                            </button>
                            : <button className="btn btn-warning invisible">
                                <FontAwesomeIcon icon={faPencil} />
                            </button>
                        }
                        {(!user.eh_admin_root && (props.sessionState.loggedUserHasEnabledResourceByEnum(EnResource.CadastrarDemaisUsuarios) && props.sessionState.loggedUser?.Id !== user.id))
                            ? <button className="btn btn-danger ms-2"
                            onClick={() => openConfirmRemoveUserModal(user.id)}>
                                <FontAwesomeIcon icon={faTrash} />
                            </button>
                            : <button className="btn btn-danger ms-2 invisible">
                                <FontAwesomeIcon icon={faTrash} />
                            </button>
                        }
                    </td>
                </tr>
            )
        })
    }
    const renderTable = (): JSX.Element => {
        return (
            <Table headings={
                [
                    "Nome", 
                    "CPF",
                    "#",
                    "Data de Nascimento",
                    "Horas Diárias",
                    "Login"
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