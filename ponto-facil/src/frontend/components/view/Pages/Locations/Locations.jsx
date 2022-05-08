import { faTrash, faPencil, faUsers, faClipboard, faLocation } from "@fortawesome/free-solid-svg-icons";
import axios from 'axios';
import React from "react";
import Main from "../../../templates/Main/Main";

import { FontAwesomeIcon } from '@fortawesome/react-fontawesome'
import Table from "../../../templates/Table/Table";
import { isFieldValid } from "../../../../../utils/valid";

const headerProps = {
    icon: faLocation,
    title: "Locais",
    subtitle: 'Cadastro de Locais: Incluir, Listar, Alterar e Excluir'
}

const baseUrl = 'http://localhost:3001/locais';
const initialState = {
    location: {
        Apelido: "",
        CEP: "",
        Bairro: "",
        Logradouro: "",
        Numero: ""
    },
    list:[],
    msg: "",
    erro: false,
}

const msgSucessoNovoUsuario = "Local cadastrado com successo!"
const msgSucessoAtualizarUsuario = "Local atualizado com sucesso!"
const msgErroCadastroInvalido = "Erro ao cadastrar local!"

export default class Locations extends React.Component {

    state = { ...initialState }

    componentDidMount() {
        axios(baseUrl).then(resp => {
            this.setState({ list: resp.data })
        })
    }

    validate() {
        const location = this.state.location
        if(
            !isFieldValid(location.Apelido) ||
            !isFieldValid(location.CEP) ||
            !isFieldValid(location.Bairro) ||
            !isFieldValid(location.Logradouro) ||
            !isFieldValid(location.Numero)
        ) {
                this.setState({ erro : true, msg : msgErroCadastroInvalido })
        } else {
            this.save();
        }
    }

    clear() {
        this.setState({ 
            location: initialState.location, 
            erro: initialState.erro, 
            msg: initialState.msg
        })
    }

    save() {
        const location = this.state.location
        const method = location.id ? 'put' : 'post'
        const url = location.id ? `${baseUrl}/${location.id}` : baseUrl

        axios[method](url, location)
            .then(resp => {
                const list = this.getUpdatedList(resp.data)
                this.setState({ location: initialState.location, list, erro: initialState.erro})
            })
    }

    getUpdatedList(location, add = true) {
        const list = this.state.list.filter(u => u.id !== location.id)
        if(add) list.unshift(location)
        return list
    }

    updateField(event) {
        const location = { ...this.state.location }
        location[event.target.name] = event.target.value
        this.setState({ location })
    }

    renderForm() {
        const disabled = !!this.state.location.id

        return (
            <div className="form">
                <div className="row">
                    <div className="col-12">
                        <div className="form-group">

                            <div className="input-group row">
                                <div className="input col-md-6 col-12">
                                    <label>Apelido</label>
                                    <input type="text" 
                                        className="form-control col-6" 
                                        name="Apelido"
                                        value={this.state.location.apelido}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o apelido..."/>
                                </div>

                                <div className="input col-md-6 col-12">
                                    <label>CEP</label>
                                    <input type="text" 
                                        className="form-control col-6" 
                                        name="CEP"
                                        value={this.state.location.CEP}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o CEP..."/>
                                </div>
                            </div>

                            <div className="input-group row">
                                <div className="input col-md-6 col-12">
                                    <label>Bairro</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="Bairro"
                                        value={this.state.location.bairro}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o bairro..."/>
                                </div>

                                <div className="input col-md-6 col-12">
                                    <label>Logradouro</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="Logradouro"
                                        value={this.state.location.logradouro}
                                        onChange={e => this.updateField(e)}
                                        placeholder="Digite o logradouro..."
                                        disabled={disabled}/>
                                </div>

                                <div className="input col-md-6 col-12">
                                        <label>Número</label>
                                        <input type="text"
                                            className="form-control" 
                                            name="Numero"
                                            value={this.state.location.numero}
                                            onChange={e => this.updateField(e)}
                                            placeholder="Digite o número..."
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

    load(location) {
        this.setState({ location })
    }

    remove(location) {
        axios.delete(`${baseUrl}/${location.id}`).then(resp => {
            const list = this.getUpdatedList(location, false)
            this.setState({ list })
        })
    }

    renderTable() {
        return (
            <Table headings={
                [
                    "#", 
                    "CEP", 
                    "Bairro", 
                    "Logradouro", 
                    "Número"
                ]
            }>
                {this.renderRows()}
            </Table>
        )
    }

    renderRows() {
        return this.state.list.map(location => {
            return (
                <tr key={location.id}>
                    <td>{location.id}</td>
                    <td>{location.CEP}</td>
                    <td>{location.Bairro}</td>
                    <td>{location.Logradouro}</td>
                    <td>{location.Numero}</td>
                    <td>
                        <button className="btn btn-warning"
                            onClick={() => this.load(location)}>
                            <FontAwesomeIcon icon={faPencil} />
                        </button>
                        <button className="btn btn-danger ms-2"
                            onClick={() => this.remove(location)}>
                            <FontAwesomeIcon icon={faTrash} />
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
                {this.renderForm()}
                {this.renderTable()}
            </Main>
        )
    }
}