import React, { useState } from 'react';
import './ConfirmModal.css';

import { Modal, Button } from 'react-bootstrap';

type ConfirmModalProps = {
  handleClose: () => void
  handleConfirm: () => void
  confirmTitle: string
  confirmQuestion: string
}

export default (props: ConfirmModalProps): JSX.Element => {
    return (
        <Modal
        show={true}
        onHide={props.handleClose}
        centered
      >
        <Modal.Header closeButton>
          <Modal.Title>{props.confirmTitle}</Modal.Title>
        </Modal.Header>
        <Modal.Body>
          {props.confirmQuestion}
        </Modal.Body>
        <Modal.Footer>
          <Button variant="secondary" onClick={props.handleClose}>Não</Button>
          <Button variant="primary" onClick={props.handleConfirm}>Sim</Button>
        </Modal.Footer>
      </Modal>
    )
}