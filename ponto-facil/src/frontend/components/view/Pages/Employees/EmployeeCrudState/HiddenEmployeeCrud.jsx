import React from "react";

export default class HiddenEmployeeCrud extends React.Component {
    render() {
       return (
        <div className="form">
            <div className="row">
                <div className="col-12 d-flex justify-content-end">
                    <button className="btn btn-primary"
                        onClick={() => this.props.invoker.publicShowNewUserForm() }>
                        Novo Funcionário
                    </button>
                </div>
            </div>
        </div>
       )
    }
}