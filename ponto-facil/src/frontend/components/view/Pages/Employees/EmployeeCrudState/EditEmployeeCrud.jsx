import React from "react";
import { getSessionStorageOrDefault, setSessionStorage } from "../../../../../../utils/useSessionStorage";
import { resourceDescription } from "../../../../../modelo/resourceDescription";
export default class EditEmployeeCrud extends React.Component {
    getSession = () => getSessionStorageOrDefault('session')
    setSession = (sessionValue) => setSessionStorage('session', sessionValue)
    getLoggedUser = () => getSessionStorageOrDefault('user')
    setLoggedUser = (loggedUserValue) => setSessionStorage('user', loggedUserValue)
    loggedUserHasEnabledResource = (resourceCode) => this.getLoggedUser().Acessos.find(x => x.Recurso_cod_en === resourceCode).Eh_habilitado
    loggedUserHasEnabledResourceByDescription = (resourceDesc) => this.loggedUserHasEnabledResource(resourceDescription.find(x => x.recurso_desc.toLowerCase() === resourceDesc.toLowerCase()).recurso_cod_en)
    
    render() {
        let disabled = false;
        return (
        <div className="form">
            <div className="row">
                <div className="col-12">
                    <div className="form-group">
                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label><strong>Editar Funcionário</strong></label>
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Nome</label>
                                <input type="text" 
                                    className="form-control" 
                                    name="nome"
                                    value={this.props.editUser.nome}
                                    onChange={e => this.props.invoker.publicUpdateField(e)}
                                    placeholder="Digite o nome..."/>
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>CPF</label>
                                <input type="text"
                                    className="form-control" 
                                    name="cpf"
                                    value={this.props.editUser.cpf}
                                    onChange={e => this.props.invoker.publicUpdateField(e)}
                                    placeholder="Digite o CPF..." />
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                    <label>Data de Nascimento</label>
                                    <input type="date"
                                        className="form-control" 
                                        name="data_nascimento"
                                        value={this.props.editUser.data_nascimento}
                                        onChange={e => this.props.invoker.publicUpdateField(e)}
                                        placeholder="Selecione a data de nascimento..." />
                            </div>
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Horas Diárias</label>
                                <input type="number" min={2} max={12}
                                    className="form-control" 
                                    name="horas_diarias"
                                    value={this.props.editUser.horas_diarias}
                                    onChange={e => this.props.invoker.publicUpdateField(e, "number")}
                                    placeholder="Digite as horas diárias..." />
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                <label>Login</label>
                                <input type="text"
                                    className="form-control" 
                                    name="login"
                                    value={this.props.editUser.login}
                                    onChange={e => this.props.invoker.publicUpdateField(e)}
                                    placeholder="Digite o login..." />
                            </div>

                            <div className="input col-md-6 col-12 mt-3">
                                    <label>Nova Senha</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="nova_senha"
                                        value={this.props.editUser.nova_senha}
                                        onChange={e => this.props.invoker.publicUpdateField(e)}
                                        placeholder="Digite a senha..." />
                            </div>
                        </div>

                        <div className="input-group row">
                            <div className="input col-md-6 col-12 mt-3">
                                    <label>Confirmar Senha</label>
                                    <input type="text"
                                        className="form-control" 
                                        name="confirmar_senha"
                                        value={this.props.editUser.confirmar_senha}
                                        onChange={e => this.props.invoker.publicUpdateField(e)}
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
                            {this.props.editUser.acessos.map((x, indx) => (
                                <div key={indx} className="input col-md-6 col-12 mt-3 d-flex">
                                    <input type="checkbox"
                                        checked={x.eh_habilitado}
                                        onChange={e => this.props.invoker.publicUpdateAccess(e, x.recurso_cod_en)}
                                        disabled={!this.loggedUserHasEnabledResourceByDescription("CadastrarAcessoTodosUsuarios")} />
                                    <label className="ps-3">{this.props.resourceDescription.find(y => y.recurso_cod_en === x.recurso_cod_en).recurso_desc}</label>
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
                        onClick={e => this.props.invoker.publicValidateEditUser(e)}>
                        Salvar
                    </button>

                    <button className="btn btn-secondary ms-2"
                        onClick={e => this.props.invoker.publicCancelForm(e)}>
                        Cancelar
                    </button>
                </div>
            </div>
        </div>
       )
    }
}