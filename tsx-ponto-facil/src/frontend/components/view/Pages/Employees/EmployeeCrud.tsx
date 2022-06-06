import { faTrash, faPencil, faUsers, faClipboard } from "@fortawesome/free-solid-svg-icons";
import axios from 'axios';
import React from "react";
import Main from "../../../templates/Main/Main";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import Table from "../../../templates/Table/Table";
import { isFieldValid, isNullOrEmpty } from "../../../../../utils/valid";
import { Navigate } from "react-router-dom";
import { getSessionStorageOrDefault, setSessionStorage } from "../../../../../utils/useSessionStorage";

import ApiUtil from '../../../../../chamada-api/ApiUtil';
import LoadingModal from "../../../templates/LoadingModal/LoadingModal";
import { formatCpf, formatDate_dd_mm_yyyy, listItemsInPortuguese, mountMessage, mountMessageServerErrorIfDefault } from "../../../../../utils/formatting";
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
        idRemovedEntity: 0
    }
}

const tituloConfirmaExclusao = `Atenção!`

export default class EmployeeCrud extends React.Component<EmployeeCrudProps, EmployeeCrudState> {

    state = { ...initialState }

    componentDidMount(): void {
        this.refreshSearchedUsers()
    }

    refreshSearchedUsers = (): void => {
        this.props.loadingState.updateLoading(true);
        (async () => {
            let r = await ApiUtil.getAsync<SearchedUser[]>(this.props.sessionState.stringifiedSession, this.props.sessionState.updateStringifiedSession, this.props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/listarTodos`)
            this.props.loadingState.updateLoading(false)
            let retrievedUsers = r.body !== undefined ? (r.body.getReturned() || []) : []
            if (r.status === 200) {
                this.setState({ erro: false, list: retrievedUsers })
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    publicValidateNewUser = (): void => {
        const user = this.state.user
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
            this.setState({ erro : true, msg : `É necessário preencher ${camposInvalidos}.` })
        } else {
            this.publicSaveNew()
        }
    }

    publicValidateEditUser = (): void => {
        const user = this.state.user
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
            this.setState({ erro : true, msg : `É necessário preencher ${camposInvalidos}.` })
        } else {
            this.publicSaveEdit()
        }
    }

    clear(): void {
        this.setState({ 
            user: initialState.user, 
            erro: initialState.erro, 
            msg: initialState.msg, 
            newUser: initialState.newUser, 
            tempAccess: initialState.tempAccess 
        })
    }

    publicSaveNew = (): void => {
        this.props.loadingState.updateLoading(true);
        (async () => {
            const postNewUser = new NewUserDTOAdapter(this.state.user).getRawNewUserDTO()
            let r = await ApiUtil.postAsync<DetailedUserDTO>(this.props.sessionState.stringifiedSession, this.props.sessionState.updateStringifiedSession, this.props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/insere`, postNewUser)
            this.props.loadingState.updateLoading(false)
            if (r.status === 200) {
                this.setState({ erro: false, msg : message.employee.successCreated, employeeCrudState: EnEmployeeCrudState.Hidden })
                this.refreshSearchedUsers()
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    publicSaveEdit = (): void => {
        this.props.loadingState.updateLoading(true);
        (async () => {
            const postEditUser = new EditUserDTOAdapter(this.state.user).getRawEditUserDTO()
            let r = await ApiUtil.postAsync<DetailedUserDTO>(this.props.sessionState.stringifiedSession, this.props.sessionState.updateStringifiedSession, this.props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/atualiza`, postEditUser)
            this.props.loadingState.updateLoading(false)
            if (r.status === 200) {
                this.setState({ erro: false, msg : message.employee.successUpdated, employeeCrudState: EnEmployeeCrudState.Hidden })
                this.refreshSearchedUsers()
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    publicUpdateField = (event: React.ChangeEvent<HTMLInputElement>): void => {
        const user = { ...this.state.user }
        switch (event.target.name) {
            case `nome`:
                user.nome = event.target.value
                break
            case `cpf`:
                user.cpf = event.target.value
                break
            case `data_nascimento`:
                user.data_nascimento = event.target.valueAsDate
                break
            case `horas_diarias`:
                user.horas_diarias = event.target.valueAsNumber
                break
            case `login`:
                user.login = event.target.value
                break
            case `nova_senha`:
                user.nova_senha = event.target.value
                break
            case `confirmar_senha`:
                user.confirmar_senha = event.target.value
                break
            default:
                break
        }
        this.setState({ user })
    }

    publicUpdateAccess = (event: React.ChangeEvent<HTMLInputElement>, recurso_cod_en: number): void => {
        const aUser = { ...this.state.user }
        aUser.acessos[aUser.acessos.findIndex(x => x.recurso_cod_en === recurso_cod_en)].eh_habilitado = !!event.target.checked
        this.setState({ user: aUser })
    }

    publicShowNewUserForm = (): void => {
        this.clear()
        this.setState({ employeeCrudState: EnEmployeeCrudState.New })
    }

    publicCancelForm = (): void => {
        this.setState({ employeeCrudState: EnEmployeeCrudState.Hidden })
    }

    renderForm(): JSX.Element {
        if (this.state.employeeCrudState.equals(EnEmployeeCrudState.Hidden)) {
            return (this.props.sessionState.loggedUserHasEnabledResourceByDescription("CadastrarDemaisUsuarios") ? <HiddenEmployeeCrud publicShowNewUserForm={this.publicShowNewUserForm} sessionState={this.props.sessionState} /> : <div></div>)
        } else if (this.state.employeeCrudState.equals(EnEmployeeCrudState.New)) {
            return (<NewEmployeeCrud newUser={this.state.user} publicCancelForm={this.publicCancelForm} publicUpdateAccess={this.publicUpdateAccess} publicUpdateField={this.publicUpdateField} publicValidateNewUser={this.publicValidateNewUser} sessionState={this.props.sessionState} />)
        } else {
            return (<EditEmployeeCrud editUser={this.state.user} publicCancelForm={this.publicCancelForm} publicUpdateAccess={this.publicUpdateAccess} publicUpdateField={this.publicUpdateField} publicValidateEditUser={this.publicValidateEditUser} sessionState={this.props.sessionState} />)
        }
    }

    load(idUser: number): void {
        this.props.loadingState.updateLoading(true);
        (async () => {
            let r = await ApiUtil.getAsync<DetailedUserDTO>(this.props.sessionState.stringifiedSession, this.props.sessionState.updateStringifiedSession, this.props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/pegaPeloId?id=${idUser}`)
            this.props.loadingState.updateLoading(false)
            if (r.status === 200) {
                const loadedUser = new UserAdapter(r.body?.getReturned() || new DetailedUserDTO()).getRawUser()
                this.setState({ erro: false, msg : "", employeeCrudState: EnEmployeeCrudState.Edit, user: loadedUser })
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body)
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    remove(idUser: number): void {
        this.props.loadingState.updateLoading(true);
        (async () => {
            let r = await ApiUtil.postAsync<null>(this.props.sessionState.stringifiedSession, this.props.sessionState.updateStringifiedSession, this.props.sessionState.updateLoggedUser, `${ApiUtil.urlApiV1()}/Usuario/excluiPeloId?id=${idUser}`, {})
            this.props.loadingState.updateLoading(false)
            if (r.status === 200) {
                this.setState({ erro: false, msg : message.employee.successRemoved, employeeCrudState: EnEmployeeCrudState.Hidden })
                this.refreshSearchedUsers()
            } else {
                let mensagemCompleta = mountMessageServerErrorIfDefault(r.body)
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    openConfirmRemoveUserModal(idUser: number): void {
        let modalProps = { ...this.state.confirmModalProps }
        modalProps.confirmTitle = tituloConfirmaExclusao
        modalProps.confirmQuestion = message.employee.ensureRemove
        modalProps.isRemove = true
        modalProps.idRemovedEntity = idUser
        this.setState({ confirmModalProps: modalProps })
    }

    closeConfirmModal = (): void => {
        let modalProps = { ...this.state.confirmModalProps }
        modalProps.confirmQuestion = ""
        this.setState({ confirmModalProps: modalProps })
    }

    confirmConfirmModal = (): void => {
        if (this.state.confirmModalProps.isRemove) {
            this.remove(this.state.confirmModalProps.idRemovedEntity)
        }
        this.closeConfirmModal()
    }

    copyToClipboard = (text: string): void => {
        navigator.clipboard.writeText(text);
    }

    renderTable(): JSX.Element {
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
                {this.renderRows()}
            </Table>
        )
    }

    renderRows() {
        return this.state.list.map(user => {
            return (
                <tr key={user.id}>
                    <td>{user.nome.split(' ')[0]}</td>
                    <td>{formatCpf(user.cpf)}</td>
                    <td className="d-flex">
                        <button className="btn btn-primary ms-2"
                            onClick={() => this.copyToClipboard(formatCpf(user.cpf))}>
                            <FontAwesomeIcon icon={faClipboard} />
                        </button></td>
                    <td>{formatDate_dd_mm_yyyy(new Date(user.data_nascimento))}</td>
                    <td>{user.horas_diarias}</td>
                    <td>{user.login}</td>
                    <td className="d-flex">
                        {(this.props.sessionState.loggedUser?.Id === user.id ? this.props.sessionState.loggedUserHasEnabledResourceByDescription("CadastrarUsuario") : this.props.sessionState.loggedUserHasEnabledResourceByDescription("CadastrarDemaisUsuarios")) && <button className="btn btn-warning"
                            onClick={() => this.load(user.id)}>
                            <FontAwesomeIcon icon={faPencil} />
                        </button>}
                        {(this.props.sessionState.loggedUserHasEnabledResourceByDescription("CadastrarDemaisUsuarios") && this.props.sessionState.loggedUser?.Id !== user.id) && <button className="btn btn-danger ms-2"
                            onClick={() => this.openConfirmRemoveUserModal(user.id)}>
                            <FontAwesomeIcon icon={faTrash} />
                        </button>}
                    </td>
                </tr>
            )
        })
    }

    render() {
        return (
            <Main {...headerProps}>
                {this.state.msg && (
                    <div className={`alert alert-${this.state.erro ? 'danger' : 'success'}`} role="alert">
                        {this.state.msg}
                    </div>
                )}
                {/* {this.state.newUser && (
                    <div className="alert alert-info">
                        Dados de Acesso temporários
                        <ul>
                            <li>Login: {this.state.tempAccess}</li>
                            <li>Senha: {this.state.tempAccess}</li>
                        </ul>
                    </div>
                )} */}
                {this.renderTable()}
                {this.renderForm()}
                {(this.state.confirmModalProps.confirmQuestion.length > 0)
                    && <ConfirmModal
                        handleClose={this.closeConfirmModal}
                        handleConfirm={this.confirmConfirmModal}
                        confirmTitle={this.state.confirmModalProps.confirmTitle}
                        confirmQuestion={this.state.confirmModalProps.confirmQuestion} />}
            </Main>
        )
    }
}