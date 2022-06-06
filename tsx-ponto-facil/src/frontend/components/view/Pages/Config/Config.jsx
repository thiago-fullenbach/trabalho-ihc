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
                <h3>Alterar Senha</h3>
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