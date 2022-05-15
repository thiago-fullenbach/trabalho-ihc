import { faTrash, faPencil, faUsers, faClipboard } from "@fortawesome/free-solid-svg-icons";
import axios from 'axios';
import React from "react";
import Main from "../../../templates/Main/Main";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import Table from "../../../templates/Table/Table";
import { isFieldValid } from "../../../../../utils/valid";

const headerProps = {
    icon: faUsers,
    title: "Funcionários",
    subtitle: 'Cadastro de Funcionários: Incluir, Listar, Alterar e Excluir'
}

const baseUrl = 'http://localhost:3001/funcionarios';
const initialState = {
    user: {
        NomeFuncionario: "",
        SobrenomeFuncionario: "",
        RG: "",
        CPF: "",
        Login: "",
        Senha: "",
        DataNascimento: ""
    },
    list:[],
    msg: "",
    erro: false,
    newUser: false,
    tempAccess: ""
}

const msgSucessoNovoUsuario = "Usuário cadastrado com successo!"
const msgSucessoAtualizarUsuario = "Usuário atualizado com sucesso!"
const msgErroCadastroInvalido = "Erro ao cadastrar usuário!"

export default class EmployeeCrud extends React.Component {

    state = { ...initialState }

    componentDidMount() {
        axios(baseUrl).then(resp => {
            this.setState({ list: resp.data })
        })
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

    renderForm() {
        const disabled = !!this.state.user.id

        return (
            <div className="form">
                <div className="row">
                    <div className="col-12">
                        <div className="form-group">

                            <div className="input-group row">
                                <div className="input col-6">
                                    <label>Nome</label>
                                    <input type="text" 
                                        className="form-control col-6" 
                                        name="NomeFuncionario"
                                        value={this.state.user.NomeFuncionario}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o nome..."/>
                                </div>

                                <div className="input col-6">
                                    <label>Sobrenome</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="SobrenomeFuncionario"
                                        value={this.state.user.SobrenomeFuncionario}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o sobrenome..."/>
                                </div>
                            </div>

                            <div className="input-group row">
                                <div className="input col-6">
                                    <label>RG</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="RG"
                                        value={this.state.user.RG}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o RG..."
                                        disabled={disabled}/>
                                </div>

                                <div className="input col-6">
                                        <label>CPF</label>
                                        <input type="text"
                                            className="form-control" 
                                            name="CPF"
                                            value={this.state.user.CPF}
                                            onChange={e => this.updateField(e)}
                                            placeholder="Digite o CPF..."
                                            disabled={disabled}/>
                                </div>
                            </div>

                            <div className="input-group row">
                                <div className="input col-6">
                                    <label>Login</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="Login"
                                        value={this.state.user.Login}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o login..."
                                        disabled={disabled}/>
                                </div>

                                <div className="input col-6">
                                        <label>Senha</label>
                                        <input type="text"
                                            className="form-control" 
                                            name="Senha"
                                            value={this.state.user.Senha}
                                            onChange={e => this.updateField(e)}
                                            placeholder="Digite a senha..."
                                            disabled={disabled}/>
                                </div>
                            </div>

                            <div className="input-group row">
                                <div className="input col-6">
                                        <label>Data de Nascimento</label>
                                        <input type="date"
                                            className="form-control" 
                                            name="DataNascimento"
                                            value={this.state.user.DataNascimento}
                                            onChange={e => this.updateField(e)}
                                            placeholder="Selecione a data de nascimento..."
                                            disabled={disabled}/>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <hr />
                <div className="row">
                    <div className="col-12 d-flex justify-content-end">
                        <button className="btn btn-primary"
                            onClick={e => this.validate(e)}>
                            Salvar
                        </button>

                        <button className="btn btn-secondary ms-2"
                            onClick={e => this.clear(e)}>
                            Cancelar
                        </button>
                    </div>
                </div>
            </div>
        )
    }

    load(user) {
        this.setState({ user })
    }

    remove(user) {
        axios.delete(`${baseUrl}/${user.id}`).then(resp => {
            const list = this.getUpdatedList(user, false)
            this.setState({ list })
        })
    }

    renderTable() {
        return (
            <Table headings={
                [
                    "#", 
                    "Nome", 
                    "Sobrenome", 
                    "RG", 
                    "CPF",
                    "Data de Nascimento"
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
                    <td>{user.id}</td>
                    <td>{user.NomeFuncionario}</td>
                    <td>{user.SobrenomeFuncionario}</td>
                    <td>{user.RG}</td>
                    <td>{user.CPF}</td>
                    <td>{user.DataNascimento}</td>
                    <td>
                        <button className="btn btn-warning"
                            onClick={() => this.load(user)}>
                            <FontAwesomeIcon icon={faPencil} />
                        </button>
                        <button className="btn btn-danger ms-2"
                            onClick={() => this.remove(user)}>
                            <FontAwesomeIcon icon={faTrash} />
                        </button>
                        <button className="btn btn-primary ms-2"
                            onClick={() => this.remove(user)}>
                            <FontAwesomeIcon icon={faClipboard} />
                        </button>
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
                {this.state.newUser && (
                    <div className="alert alert-info">
                        Dados de Acesso temporários
                        <ul>
                            <li>Login: {this.state.tempAccess}</li>
                            <li>Senha: {this.state.tempAccess}</li>
                        </ul>
                    </div>
                )}
                {this.renderForm()}
                {this.renderTable()}
            </Main>
        )
    }
}