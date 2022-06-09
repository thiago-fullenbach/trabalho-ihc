import React from "react";
import SessionState from "../../../../../main/SessionState";

type HiddenEmployeeCrudProps = {
    publicShowNewUserForm: () => void
    sessionState: SessionState
}

export default (props: HiddenEmployeeCrudProps): JSX.Element => {

    
    return (
        <div className="form">
            <div className="row">
                <div className="col-12 d-flex justify-content-end">
                    <button className="btn btn-primary"
                        onClick={() => props.publicShowNewUserForm() }>
                        Novo Funcionário
                    </button>
                </div>
            </div>
        </div>
    )
}