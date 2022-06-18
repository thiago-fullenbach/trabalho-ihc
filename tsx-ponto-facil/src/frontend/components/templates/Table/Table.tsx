import './Table.css';
import React from "react";

type TableProps = {
    headings: string[]
    children: React.ReactNode
}

export default (props: TableProps): JSX.Element => {
    return (
        <div className='container-tabela-responsiva'>
            <table className="table tabela-responsiva">
                <thead>
                    <tr>
                        {props.headings.map((heading, index) => <th key={index}>{heading}</th>)}
                    </tr>
                </thead>
                <tbody>
                    {props.children}
                </tbody>
            </table>
            
        </div>
    )
}