import React, { useState } from 'react';
import './LoadingModal.css';

import { Modal } from 'react-bootstrap';

export default props => {
    return (
        <div className='modal-container'>
            <div className="lds-ellipsis"><div></div><div></div><div></div><div></div></div>
        </div>
    );
}

