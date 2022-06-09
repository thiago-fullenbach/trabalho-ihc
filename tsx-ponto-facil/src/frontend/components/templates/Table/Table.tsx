import './Table.css';
import React from "react";

type TableProps = {
    headings: string[]
    children: React.ReactNode
}

export default (props: TableProps): JSX.Element => {
    return (
        <table className="table table-responsive mt4">
            <thead>
                <tr>
                    {props.headings.map((heading, index) => <th key={index}>{heading}</th>)}
                </tr>
            </thead>
            <tbody>
                {props.children}
            </tbody>
        </table>
    )
}