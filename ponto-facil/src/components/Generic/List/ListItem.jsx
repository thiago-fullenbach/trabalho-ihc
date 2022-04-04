import React from "react";
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { faClock, faPen, faTrash } from '@fortawesome/free-solid-svg-icons';
import './ListItem.css';

const ListItem = ({item, actionAdjust, actionEdit, actionDelete}) => {
    const infoList = Object.values(item).map((info, index) => <td key={index}>{info}</td>);

    return <tr className="ListItem">
        {infoList}
        { (actionAdjust || actionEdit || actionDelete) &&
            <td>
                {actionAdjust && <FontAwesomeIcon icon={faClock} color="#155270" />}
                {actionEdit && <FontAwesomeIcon icon={faPen} color="#155270" />}
                {actionDelete && <FontAwesomeIcon icon={faTrash} color="#155270" />}
            </td>
        }
    </tr>
}

export default ListItem;