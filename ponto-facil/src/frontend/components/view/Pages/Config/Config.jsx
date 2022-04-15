import { faWrench } from "@fortawesome/free-solid-svg-icons";
import React from "react";
import Main from "../../../templates/Main/Main";

const headerProps = {
    icon: faWrench,
    title: "Configurações",
    subtitle: 'Alterar Configurações do Usuário'
}

export default class Config extends React.Component {
    render() {
        return (
            <Main {...headerProps}>
                <h3>Alterar Dados Cadastrais</h3>
                <hr />

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
                                            placeholder="Digite o nome..."/>
                                    </div>

                                    <div className="input col-6">
                                        <label>Sobrenome</label>
                                        <input type="text"
                                            className="form-control" 
                                            name="SobrenomeFuncionario"
                                            placeholder="Digite o sobrenome..."/>
                                    </div>
                                </div>

                                <div className="input-group row">
                                    <div className="input col-6">
                                        <label>RG</label>
                                        <input type="text"
                                            className="form-control" 
                                            name="RG"
                                            placeholder="Digite o RG..."/>
                                    </div>

                                    <div className="input col-6">
                                            <label>CPF</label>
                                            <input type="text"
                                                className="form-control" 
                                                name="CPF"
                                                placeholder="Digite o CPF..."/>
                                    </div>
                                </div>

                                <div className="input-group row">
                                    <div className="input col-6">
                                            <label>Data de Nascimento</label>
                                            <input type="date"
                                                className="form-control" 
                                                name="DataNascimento"
                                                placeholder="Selecione a data de nascimento..."/>
                                    </div>
                                </div>
                            </div>

                            <hr />
                            <div className="row">
                                <div className="col-12 d-flex justify-content-end">
                                    <button className="btn btn-primary"
                                        onClick={() => console.log("Salvar")}>
                                        Salvar
                                    </button>

                                    <button className="btn btn-secondary ms-2"
                                        onClick={() => console.log("Cancelar")}>
                                        Cancelar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <h3 className="mt-5">Alterar Senha</h3>
                <hr />

                <div className="form">
                    <div className="row">
                        <div className="col-12">
                            <div className="form-group">
                                <div className="input-group row">
                                    <div className="input">
                                        <label>Nova Senha</label>
                                        <input type="password" 
                                            className="form-control"
                                            name="NovaSenha"
                                            placeholder="Nova Senha..."/>
                                    </div>

                                    <div className="input">
                                        <label>Confirmar Senha</label>
                                        <input type="password" 
                                            className="form-control"
                                            name="ConfirmaNovaSenha"
                                            placeholder="Confirmar senha..."/>
                                    </div>
                                </div>
                            </div>

                            <hr />
                            <div className="row">
                                <div className="col-12 d-flex justify-content-end">
                                    <button className="btn btn-primary"
                                        onClick={() => console.log("Salvar")}>
                                        Salvar
                                    </button>

                                    <button className="btn btn-secondary ms-2"
                                        onClick={() => console.log("Cancelar")}>
                                        Cancelar
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </Main>
        )
    }
}