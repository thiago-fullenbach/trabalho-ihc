import './Main.css'
import React from 'react'
import Header from '../Header/Header'
import { IconDefinition } from '@fortawesome/fontawesome-svg-core'

type MainProps = {
    icon: IconDefinition
    title: string
    subtitle: string
    children: React.ReactNode
}

export default (props: MainProps): JSX.Element => {
    return (
        <React.Fragment>
            <Header icon={props.icon} title={props.title} subtitle={props.subtitle} />
            <main className='content container-fluid'>
                <div className="container-fluid p-3 mt-3">
                    {props.children}
                </div>
            </main>
        </React.Fragment>
    )
}