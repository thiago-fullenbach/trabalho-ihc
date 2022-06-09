import React, { useState } from 'react';
import './MsgDialog.css';

import { Alert } from 'react-bootstrap';

type MsgDialogProps = {
    typeAlert: string
    msgType: string
    msg: string
    onDismiss: () => void
}

export default (props: MsgDialogProps): JSX.Element => {


    return (
        <Alert className="dialog-msg" variant={props.typeAlert} onClose={() => props.onDismiss()} dismissible>
            <Alert.Heading>{props.msgType}!</Alert.Heading>
            <p>{props.msg}</p>
        </Alert>
    );
}

