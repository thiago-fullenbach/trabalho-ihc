import React from "react";
import SessionState from "../../../../../main/SessionState";

type HiddenPresenceCrudProps = {
    publicShowNewPresenceForm: () => void
    sessionState: SessionState
}

export default (props: HiddenPresenceCrudProps): JSX.Element => {

    
    return (
        <div className="form">
            <div className="row">
                <div className="col-12 d-flex justify-content-end">
                    <button className="btn btn-primary mt-3"
                        onClick={() => props.publicShowNewPresenceForm() }>
                        Registrar Presença
                    </button>
                </div>
            </div>
        </div>
    )
}