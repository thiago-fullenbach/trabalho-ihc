import React, { useState } from 'react';
import './MsgDialog.css';

import { Alert } from 'react-bootstrap';

export default props => {
    const [show, setShow] = useState(true);

    if (show) {
        return (
        <Alert className="dialog-msg" variant={props.typeAlert} onClose={() => setShow(false)} dismissible>
            <Alert.Heading>{props.msgType}!</Alert.Heading>
            <p>{props.msg}</p>
        </Alert>
        );
    }
    return null;
}

