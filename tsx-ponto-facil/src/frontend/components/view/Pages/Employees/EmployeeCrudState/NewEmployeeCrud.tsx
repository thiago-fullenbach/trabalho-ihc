import React from "react";
import { toInputValue } from "../../../../../../utils/formatting";
import SessionState from "../../../../../main/SessionState";
import User from "../../../../../modelo/employee/user";
import { resourceDescription } from "../../../../../modelo/resourceDescription";

type NewEmployeeCrudProps = {
    publicUpdateField: (event: React.ChangeEvent<HTMLInputElement>) => void
    publicUpdateAccess: (event: React.ChangeEvent<HTMLInputElement>, recurso_cod_en: number) => void
    publicValidateNewUser: () => void
    publicCancelForm: () => void
    newUser: User
    sessionState: SessionState
}

export default class NewEmployeeCrud extends React.Component<NewEmployeeCrudProps, {}> {

    render(): JSX.Element {
        return (
        <div className="form">
            <div className="row">
                <div className="col-12">
                    <div className="form-group">
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label><strong>Novo Funcionário</strong></label>
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Nome</label>
                                <input type="text" 
                                    className="form-control" 
                                    name="nome"
                                    value={this.props.newUser.nome}
                                    onChange={e => this.props.publicUpdateField(e)}
                                    placeholder="Digite o nome..."/>
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>CPF</label>
                                <input type="text"
                                    className="form-control" 
                                    name="cpf"
                                    value={this.props.newUser.cpf}
                                    onChange={e => this.props.publicUpdateField(e)}
                                    placeholder="Digite o CPF..." />
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                    <label>Data de Nascimento</label>
                                    <input type="date"
                                        className="form-control" 
                                        name="data_nascimento"
                                        value={toInputValue(this.props.newUser.data_nascimento)}
                                        onChange={e => this.props.publicUpdateField(e)}
                                        placeholder="Selecione a data de nascimento..." />
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Horas Diárias</label>
                                <input type="number" min={2} max={12}
                                    className="form-control" 
                                    name="horas_diarias"
                                    value={this.props.newUser.horas_diarias || ``}
                                    onChange={e => this.props.publicUpdateField(e)}
                                    placeholder="Digite as horas diárias..." />
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Login</label>
                                <input type="text"
                                    className="form-control" 
                                    name="login"
                                    value={this.props.newUser.login}
                                    onChange={e => this.props.publicUpdateField(e)}
                                    placeholder="Digite o login..." />
                            </div>

                            <div className="input col-md-6 col-12 mt-3">
                                    <label>Nova Senha</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="nova_senha"
                                        value={this.props.newUser.nova_senha}
                                        onChange={e => this.props.publicUpdateField(e)}
                                        placeholder="Digite a senha..." />
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                    <label>Confirmar Senha</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="confirmar_senha"
                                        value={this.props.newUser.confirmar_senha}
                                        onChange={e => this.props.publicUpdateField(e)}
                                        placeholder="Digite a senha novamente..." />
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <hr />
            <div className="row">
                <div className="col-12">
                    <div className="form-group">
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label><strong>Acessos</strong></label>
                            </div>
                        </div>

                        <div className="input-group row">
                            {this.props.newUser.acessos.map((x, indx) => (
                                <div key={indx} className="input col-md-6 col-12 mt-3 d-flex">
                                    <input type="checkbox"
                                        checked={x.eh_habilitado}
                                        onChange={e => this.props.publicUpdateAccess(e, x.recurso_cod_en)}
                                        disabled={!this.props.sessionState.loggedUserHasEnabledResourceByDescription("CadastrarAcessoTodosUsuarios")} />
                                    <label className="ps-3">{resourceDescription.find(y => y.recurso_cod_en === x.recurso_cod_en)?.recurso_desc}</label>
                                </div>
                            ))}
                        </div>
                    </div>
                </div>
            </div>

            <hr />
            <div className="row">
                <div className="col-12 d-flex justify-content-end">
                    <button className="btn btn-primary"
                        onClick={_ => this.props.publicValidateNewUser()}>
                        Salvar
                    </button>

                    <button className="btn btn-secondary ms-2"
                        onClick={_ => this.props.publicCancelForm()}>
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
       )
    }
}