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
import { formatCpf, formatDate_dd_mm_yyyy, listItemsInPortuguese, mountMessage } from "../../../../../utils/formatting.js";
import HiddenEmployeeCrud from "./EmployeeCrudState/HiddenEmployeeCrud";
import NewEmployeeCrud from "./EmployeeCrudState/NewEmployeeCrud";
import EditEmployeeCrud from "./EmployeeCrudState/EditEmployeeCrud";
import ConfirmModal from "../../../templates/ConfirmModal/ConfirmModal";
import { resourceDescription } from "../../../../modelo/resourceDescription";

const headerProps = {
    icon: faUsers,
    title: "Funcionários",
    subtitle: 'Cadastro de Funcionários: Incluir, Listar, Alterar e Excluir'
}

const baseUrl = 'http://localhost:3001/funcionarios';
const initialState = {
    user: {
        nome: "",
        cpf: "",
        data_nascimento: "",
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
    carregando: false,
    employeeCrudState: "hidden",
    confirmModalProps: {
        confirmTitle: "",
        confirmQuestion: "",
        isRemove: false,
        idRemovedUser: 0
    }
}

const msgSucessoNovoUsuario = "Funcionário cadastrado com successo!"
const msgSucessoAtualizarUsuario = "Funcionário atualizado com sucesso!"
const msgSucessoExcluirUsuario = "Funcionário excluído com sucesso!"
const msgErroCadastroInvalido = "Erro ao cadastrar funcionário!"
const tituloConfirmaExclusao = "Atenção!"
const msgConfirmaExclusao = "Deseja excluir o funcionário?"

export default class EmployeeCrud extends React.Component {

    state = { ...initialState }

    getSession = () => getSessionStorageOrDefault('session')
    setSession = (sessionValue) => setSessionStorage('session', sessionValue)
    getLoggedUser = () => getSessionStorageOrDefault('user')
    setLoggedUser = (loggedUserValue) => setSessionStorage('user', loggedUserValue)
    loggedUserHasEnabledResource = (resourceCode) => this.getLoggedUser().Acessos.find(x => x.Recurso_cod_en === resourceCode).Eh_habilitado
    loggedUserHasEnabledResourceByDescription = (resourceDesc) => this.loggedUserHasEnabledResource(resourceDescription.find(x => x.recurso_desc.toLowerCase() === resourceDesc.toLowerCase()).recurso_cod_en)

    componentDidMount() {
        (async () => {
            this.setState({ carregando: true} )
            let r = await ApiUtil.RespostaDoServidor_getAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/listarTodos`)
            this.setState({ carregando: false} )
            if (r.status === 200) {
                this.setState({ erro: false, list: r.corpo.devolvido })
            } else {
                let mensagemCompleta = mountMessage(r);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    validate() {
        const user = this.state.user
        if(
            !isFieldValid(user.NomeFuncionario) ||
            !isFieldValid(user.SobrenomeFuncionario) ||
            !isFieldValid(user.RG) ||
            !isFieldValid(user.CPF) ||
            !isFieldValid(user.DataNascimento)
        ) {
                this.setState({ erro : true, msg : msgErroCadastroInvalido })
        } else {
            this.save();
        }
    }

    publicValidateNewUser = () => {
        const user = this.state.user
        let invalidFields = []
        if (!isFieldValid(user.nome)) {
            invalidFields.push("Nome")
        }
        if (!isFieldValid(user.cpf)) {
            invalidFields.push("CPF")
        }
        if (!isFieldValid(user.data_nascimento)) {
            invalidFields.push("Data de Nascimento")
        }
        if (!isFieldValid(user.horas_diarias)) {
            invalidFields.push("Horas Diárias")
        }
        if (!isFieldValid(user.login)) {
            invalidFields.push("Login")
        }
        if (!isFieldValid(user.nova_senha)) {
            invalidFields.push("Nova Senha")
        }
        if (!isFieldValid(user.confirmar_senha)) {
            invalidFields.push("Confirmar Senha")
        }
        if (invalidFields.length > 0) {
            let camposInvalidos = listItemsInPortuguese(invalidFields)
            this.setState({ erro : true, msg : `É necessário preencher ${camposInvalidos}.` })
        } else {
            this.publicSaveNew()
        }
    }

    publicValidateEditUser = () => {
        const user = this.state.user
        let invalidFields = []
        if (!isFieldValid(user.nome)) {
            invalidFields.push("Nome")
        }
        if (!isFieldValid(user.cpf)) {
            invalidFields.push("CPF")
        }
        if (!isFieldValid(user.data_nascimento)) {
            invalidFields.push("Data de Nascimento")
        }
        if (!isFieldValid(user.horas_diarias)) {
            invalidFields.push("Horas Diárias")
        }
        if (!isFieldValid(user.login)) {
            invalidFields.push("Login")
        }
        if (invalidFields.length > 0) {
            let camposInvalidos = listItemsInPortuguese(invalidFields)
            this.setState({ erro : true, msg : `É necessário preencher ${camposInvalidos}.` })
        } else {
            this.publicSaveEdit()
        }
    }

    clear() {
        this.setState({ 
            user: initialState.user, 
            erro: initialState.erro, 
            msg: initialState.msg, 
            newUser: initialState.newUser, 
            tempAccess: initialState.tempAccess 
        })
    }

    save() {
        const user = this.state.user
        const method = user.id ? 'put' : 'post'
        const url = user.id ? `${baseUrl}/${user.id}` : baseUrl
        const msg = user.id ? msgSucessoAtualizarUsuario : msgSucessoNovoUsuario

        if(!user.id) {
            user.Login = (user.NomeFuncionario[0].trim() + user.SobrenomeFuncionario.trim()).toLowerCase() // Primeira letra do nome + sobrenome
            user.Senha = user.Login // Senha é igual ao login para o primeiro acesso
            this.setState({ newUser: true, tempAccess: user.Login })
        } else {
            this.setState({ newUser: initialState.newUser, tempAccess: initialState.tempAccess })
        }

        axios[method](url, user)
            .then(resp => {
                const list = this.getUpdatedList(resp.data)
                this.setState({ user: initialState.user, list, erro: initialState.erro,  msg})
            })
    }

    publicSaveNew = () => {
        (async () => {
            this.setState({ carregando: true} )
            let r = await ApiUtil.RespostaDoServidor_postAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/insere`, this.state.user)
            this.setState({ carregando: false} )
            if (r.status === 200) {
                this.setState({ erro: false, msg : msgSucessoNovoUsuario, employeeCrudState: "hidden" })
                this.setState({ carregando: true })
                let r = await ApiUtil.RespostaDoServidor_getAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/listarTodos`)
                this.setState({ carregando: false })
                if (r.status === 200) {
                    this.setState({ list: r.corpo.devolvido })
                } else {
                    let mensagemCompleta = mountMessage(r);
                    this.setState({ erro : true, msg : mensagemCompleta })
                }
            } else {
                let mensagemCompleta = mountMessage(r);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    publicSaveEdit = () => {
        (async () => {
            this.setState({ carregando: true} )
            let r = await ApiUtil.RespostaDoServidor_postAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/atualiza`, this.state.user)
            this.setState({ carregando: false} )
            if (r.status === 200) {
                this.setState({ erro: false, msg : msgSucessoAtualizarUsuario, employeeCrudState: "hidden" })
                this.setState({ carregando: true })
                let r = await ApiUtil.RespostaDoServidor_getAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/listarTodos`)
                this.setState({ carregando: false })
                if (r.status === 200) {
                    this.setState({ list: r.corpo.devolvido })
                } else {
                    let mensagemCompleta = mountMessage(r);
                    this.setState({ erro : true, msg : mensagemCompleta })
                }
            } else {
                let mensagemCompleta = mountMessage(r);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    getUpdatedList(user, add = true) {
        const list = this.state.list.filter(u => u.id !== user.id)
        if(add) list.unshift(user)
        return list
    }

    updateField(event) {
        const user = { ...this.state.user }
        user[event.target.name] = event.target.value
        this.setState({ user })
    }

    publicUpdateAccess = (event, recurso_cod_en) => {
        const aUser = { ...this.state.user }
        aUser.acessos[aUser.acessos.findIndex(x => x.recurso_cod_en === recurso_cod_en)].eh_habilitado = !!event.target.checked
        this.setState({ aUser })
    }

    publicUpdateField = (event, specificType) => {
        const user = { ...this.state.user }
        if (specificType === "number") {
            user[event.target.name] = +(event.target.value)
        } else {
            user[event.target.name] = event.target.value
        }
        this.setState({ user })
    }

    publicShowNewUserForm = () => {
        this.clear()
        this.setState({ employeeCrudState: "new" })
    }

    publicCancelForm = () => {
        this.setState({ employeeCrudState: "hidden" })
    }

    renderForm() {
        const disabled = !!this.state.user.id

        if (this.state.employeeCrudState === "hidden") {
            return (this.loggedUserHasEnabledResourceByDescription("CadastrarDemaisUsuarios") && <HiddenEmployeeCrud invoker={this} />)
        } else if (this.state.employeeCrudState === "new") {
            return (<NewEmployeeCrud invoker={this} newUser={this.state.user} resourceDescription={resourceDescription} />)
        } else {
            return (<EditEmployeeCrud invoker={this} editUser={this.state.user} resourceDescription={resourceDescription} />)
        }

    }

    load(userId) {
        (async () => {
            this.setState({ carregando: true } )
            let r = await ApiUtil.RespostaDoServidor_getAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/pegaPeloId?id=${userId}`)
            this.setState({ carregando: false } )
            if (r.status === 200) {
                this.setState({ erro: false, msg : "", employeeCrudState: "edit", user: r.corpo.devolvido })
            } else {
                let mensagemCompleta = mountMessage(r);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    remove(idUser) {
        (async () => {
            this.setState({ carregando: true} )
            let r = await ApiUtil.RespostaDoServidor_postAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/excluiPeloId?id=${idUser}`, {})
            this.setState({ carregando: false} )
            if (r.status === 200) {
                this.setState({ erro: false, msg : msgSucessoExcluirUsuario, employeeCrudState: "hidden" })
                this.setState({ carregando: true })
                let r = await ApiUtil.RespostaDoServidor_getAsync(this.getSession(), this.setSession, this.setLoggedUser, `${ApiUtil.string_urlApiV1}/Usuario/listarTodos`)
                this.setState({ carregando: false })
                if (r.status === 200) {
                    this.setState({ list: r.corpo.devolvido })
                } else {
                    let mensagemCompleta = mountMessage(r);
                    this.setState({ erro : true, msg : mensagemCompleta })
                }
            } else {
                let mensagemCompleta = mountMessage(r);
                this.setState({ erro : true, msg : mensagemCompleta })
            }
        })()
    }

    openConfirmRemoveUserModal(idUser) {
        console.log({ ...this.state.confirmModalProps })
        let modalProps = { ...this.state.confirmModalProps }
        modalProps.confirmTitle = tituloConfirmaExclusao
        modalProps.confirmQuestion = msgConfirmaExclusao
        modalProps.isRemove = true
        modalProps.idRemovedUser = idUser
        this.setState({ confirmModalProps: modalProps })
    }

    closeConfirmModal = () => {
        let modalProps = { ...this.state.confirmModalProps }
        modalProps.confirmQuestion = ""
        this.setState({ confirmModalProps: modalProps })
    }

    confirmConfirmModal = () => {
        if (this.state.confirmModalProps.isRemove) {
            this.remove(this.state.confirmModalProps.idRemovedUser)
        }
        this.closeConfirmModal()
    }

    copyToClipboard = (text) => {
        navigator.clipboard.writeText(text);
    }

    gotoURL(user) {
        let url = `/user/${user.id}`;
        return <Navigate to={url} />
    }

    renderTable() {
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
                        {(this.getLoggedUser().Id === user.id ? this.loggedUserHasEnabledResourceByDescription("CadastrarUsuario") : this.loggedUserHasEnabledResourceByDescription("CadastrarDemaisUsuarios")) && <button className="btn btn-warning"
                            onClick={() => this.load(user.id)}>
                            <FontAwesomeIcon icon={faPencil} />
                        </button>}
                        {(this.loggedUserHasEnabledResourceByDescription("CadastrarDemaisUsuarios") && this.getLoggedUser().Id !== user.id) && <button className="btn btn-danger ms-2"
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
                {this.state.carregando && <LoadingModal/>}
            </Main>
        )
    }
}