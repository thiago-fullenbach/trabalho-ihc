import React from "react";
import ListItem from "./ListItem";
import './List.css';

const List = ({headingList, itens, actionAdjust, actionEdit, actionDelete}) => {
    const headings = headingList.map((heading, index) => <th key={index}>{heading}</th>)
    const itemList = itens.map((item, index) => <ListItem key={index} item={item} actionAdjust={actionAdjust} actionEdit={actionEdit} actionDelete={actionDelete} />)

    return <table className="List" cellSpacing={0} cellPadding={0}>
        <thead className="ListHead">
            <tr>
                {headings}
                {(actionAdjust || actionEdit || actionDelete) && <th>Ações</th>}
            </tr>
        </thead>

        <tbody className="ListBody">
            {itemList}
        </tbody>
    </table>
}

export default List;